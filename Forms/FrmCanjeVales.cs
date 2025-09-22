using System;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Data.Entity;
using System.Collections.Generic;

namespace GestionValesRdz.Forms
{
    public partial class FrmCanjeVales : XtraForm
    {
        public FrmCanjeVales()
        {
            InitializeComponent();
        }

        public void BuscaVale(string folio)
        {
            try
            {
                if (cmbEstacion.EditValue == null)
                {
                    XtraMessageBox.Show("Seleccione una estación", "Canje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    cmbEstacion.Select();
                    return;
                }

                // 1. Creamos un contexto nuevo solo para esta búsqueda.
                using (var contexto = AyudanteDeConexion.CrearContexto())
                {
                    vales vale = null;
                    // 2. Incluimos los datos relacionados para evitar errores de carga diferida.
                    if (Properties.Settings.Default.v1)
                    {
                        vale = contexto.vales
                            .Include(v => v.empresas)
                            .Include(v => v.clientes)
                            .SingleOrDefault(v => v.codigo == folio);
                    }else
                    {
                        var f = Convert.ToInt32(folio);
                        vale = contexto.vales
                            .Include(v => v.empresas)
                            .Include(v => v.clientes)
                            .SingleOrDefault(v => v.folio == f);
                    }
                    

                    // 3. Validamos el resultado de la búsqueda.
                    if (vale == null)
                    {
                        XtraMessageBox.Show("El folio del vale no existe.", "Vale no encontrado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    if (vale.estatus == "C")
                    {
                        XtraMessageBox.Show("No se puede canjear un vale cancelado, verifique.", "Canje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }

                    if (vale.estatus == "P")
                    {
                        XtraMessageBox.Show("El vale ya ha sido canjeado, verifique.", "Canje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }

                    if (!lv.Items.ContainsKey(vale.codigo))
                    {
                        AgregarVale(vale.folio.ToString(), vale.empresas.nombre, vale.clientes.nombre, vale.importe.ToString(), vale.codigo);
                    }
                    else
                    {
                        XtraMessageBox.Show("Este vale ya ha sido agregado a la lista.", "Duplicado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show($"Ocurrió un error al buscar el vale: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AgregarVale(string folio, string empresa, string cliente, string monto, string codigo)
        {
            ListViewItem item = new ListViewItem(folio);
            item.Name = codigo; // Usamos el código como clave para evitar duplicados.
            item.SubItems.Add(empresa);
            item.SubItems.Add(cliente);
            item.SubItems.Add(monto);
            lv.Items.Add(item);
            txtValesCanjeados.Text = lv.Items.Count.ToString();
            txtTotal.Text = SumaImporteListView().ToString("N2");
        }

        private void txtLetra_KeyDown(object sender, KeyEventArgs e)
        {
            if(txtLetra.Text != string.Empty)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    BuscaVale(txtLetra.Text);
                    txtLetra.Text = string.Empty;
                    txtLetra.Select();
                }
               
            }
        }

        private void btnCanjear_Click(object sender, EventArgs e)
        {
            // --- 1. VALIDACIONES INICIALES ---
            // Nos aseguramos de que todo esté listo antes de empezar.
            if (lv.Items.Count == 0)
            {
                return; // No hay nada que hacer.
            }

            if (cmbEstacion.EditValue == null)
            {
                XtraMessageBox.Show("Debe seleccionar una estación para el canje.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // --- 2. RECOPILAR TODOS LOS FOLIOS ---
            // Juntamos todos los folios de la lista en un solo lugar.
            var foliosACanjear = new List<int>();
            foreach (ListViewItem item in lv.Items)
            {
                if (int.TryParse(item.SubItems[0].Text, out int folio))
                {
                    foliosACanjear.Add(folio);
                }
            }

            if (!foliosACanjear.Any())
            {
                XtraMessageBox.Show("No se encontraron folios válidos en la lista.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // --- 3. ACTUALIZACIÓN EN LOTE (MÁS RÁPIDO Y SEGURO) ---
            try
            {
                List<string> erroresDeValidacion = new List<string>();
                int valesActualizados = 0;

                using (var contexto = AyudanteDeConexion.CrearContexto())
                {
                    // 3.1. Hacemos UNA SOLA CONSULTA a la base de datos para traer todos los vales.
                    var valesEncontrados = contexto.vales
                        .Where(v => foliosACanjear.Contains(v.folio))
                        .ToList();

                    // 3.2. Preparamos los cambios en memoria.
                    foreach (var vale in valesEncontrados)
                    {
                        // Re-validamos cada vale para asegurarnos de que no ha sido canjeado o cancelado
                        // por otro usuario mientras estaba en nuestra lista.
                        if (vale.estatus != "A")
                        {
                            erroresDeValidacion.Add($"El vale {vale.folio} ya no está activo (estado: {vale.estatus}).");
                            continue; // Saltar al siguiente vale
                        }

                        // Si es válido, lo actualizamos.
                        vale.estatus = "P"; // 'P' de Procesado/Pagado
                        vale.id_estacion = Convert.ToInt32(cmbEstacion.EditValue);
                        vale.fecha_corte = dtpFechaCorte.DateTime;
                        vale.fecha_canje = DateTime.Now;
                        valesActualizados++;
                    }

                    // 3.3. Guardamos TODOS los cambios en una sola transacción.
                    contexto.SaveChanges();
                }

                // --- 4. INFORMAR AL USUARIO ---
                var mensajeFinal = new System.Text.StringBuilder();
                mensajeFinal.AppendLine($"{valesActualizados} vales canjeados correctamente.");

                // Si hubo vales que no se pudieron canjear, los informamos.
                if (erroresDeValidacion.Any())
                {
                    mensajeFinal.AppendLine("\nAlgunos vales no se pudieron procesar:");
                    erroresDeValidacion.ForEach(error => mensajeFinal.AppendLine($"- {error}"));
                }

                XtraMessageBox.Show(mensajeFinal.ToString(), "Proceso Completado", MessageBoxButtons.OK, MessageBoxIcon.Information);

                NotificadorDatos.AnunciarCambio();
                Close();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show($"Ocurrió un error inesperado al guardar los cambios:\n\n{ex.Message}", "Error de Base de Datos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtLetra.Text != string.Empty)
            {                
                BuscaVale(txtLetra.Text);
                txtLetra.Text = string.Empty;
                txtLetra.Select();
            }
           
        }

        private void FrmCanjeVales_Load(object sender, EventArgs e)
        {
            // 5. El Load también debe usar su propio contexto.
            using (var contexto = AyudanteDeConexion.CrearContexto())
            {
                // Usamos .ToList() para materializar la consulta y cerrar el contexto de forma segura.
                var estaciones = contexto.estaciones.ToList();
                cmbEstacion.Properties.DataSource = estaciones;
            }
            cmbEstacion.Properties.ValueMember = "id";
            cmbEstacion.Properties.DisplayMember = "nombre";
        }

        private void quitarDeLaListaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lv.SelectedItems)
                lv.Items.Remove(item);
            txtValesCanjeados.Text = lv.Items.Count.ToString();
            txtTotal.Text = SumaImporteListView().ToString();
        }

        public float SumaImporteListView()
        {
            float aux = 0;

            foreach(ListViewItem item in lv.Items)
            {
                aux += Convert.ToSingle(item.SubItems[3].Text);
            }

            return aux;
        }
    }
}