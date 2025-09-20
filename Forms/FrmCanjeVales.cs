using System;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Data.Entity;

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
            if (lv.Items.Count > 0)
            {
                // 4. Usamos un contexto nuevo también para la operación de guardado.
                using (var contexto =  AyudanteDeConexion.CrearContexto())
                {
                    foreach (ListViewItem item in lv.Items)
                    {
                        int folio = Convert.ToInt32(item.SubItems[0].Text);
                        vales vale = contexto.vales.SingleOrDefault(p => p.folio == folio);

                        if (vale != null)
                        {
                            vale.estatus = "P";
                            vale.id_estacion = Convert.ToInt32(cmbEstacion.EditValue);
                            vale.fecha_corte = dtpFechaCorte.DateTime;
                            vale.fecha_canje = DateTime.Now;
                        }
                    }
                    contexto.SaveChanges(); // Guardamos todos los cambios en una sola transacción.
                }

                XtraMessageBox.Show("Vales canjeados correctamente");
                NotificadorDatos.AnunciarCambio();
                Close(); // Usar Close() es más limpio que Dispose() aquí.
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