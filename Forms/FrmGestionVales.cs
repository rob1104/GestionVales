using System;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Data.Entity;

namespace GestionValesRdz.Forms
{
    public partial class FrmGestionVales : XtraForm
    {
        public FrmGestionVales()
        {
            InitializeComponent();
            ConfigurarFiltroAnio();
            NotificadorDatos.DatosCambiados += AlRecibirNotificacionDeCambio;
        }

        private void AlRecibirNotificacionDeCambio(object sender, EventArgs e)
        {
            // Simplemente volvemos a cargar los datos del año que está seleccionado.
            // Usamos BeginInvoke para asegurar que la actualización se ejecute de forma segura en el hilo de la UI.
            if (beiAnio.EditValue != null)
            {
                this.BeginInvoke(new Action(() => {
                    CargarValesPorAnio((int)beiAnio.EditValue);
                }));
            }
        }

        private void ConfigurarFiltroAnio()
        {
            // Obtenemos el repositorio del ComboBox que creamos en el diseñador
            var cboAnioRepo = beiAnio.Edit as DevExpress.XtraEditors.Repository.RepositoryItemComboBox;
            if (cboAnioRepo == null) return;

            // Llenamos el ComboBox con los años, desde 2023 hasta el año actual.
            int anioActual = DateTime.Now.Year;
            for (int anio = 2020; anio <= anioActual; anio++)
            {
                cboAnioRepo.Items.Add(anio);
            }

            // Leemos el año que guardamos en los settings
            int anioGuardado = Properties.Settings.Default.AnioFiltroVales;
            // Si no hay nada guardado, usamos el año actual como predeterminado
            if (anioGuardado < 2020)
            {
                anioGuardado = anioActual;
            }

            // Asignamos el valor al ComboBox y cargamos los datos de ese año.
            beiAnio.EditValue = anioGuardado;
            CargarValesPorAnio(anioGuardado);
        }

        private async void CargarValesPorAnio(int anio)
        {
            gridView1.ShowLoadingPanel();
            try
            {
                using (var contexto = AyudanteDeConexion.CrearContexto())
                {
                    // --- INICIA CAMBIO ---
                    // Usamos .Include() para cargar las tablas relacionadas que necesita el grid.
                    // Esto evita la carga diferida (Lazy Loading) más tarde.
                    var valesDelAnio = await contexto.vales
                        .Include(v => v.clientes) // Carga la información del cliente relacionado
                        .Include(v => v.empresas) // Carga la información de la empresa relacionada
                        .Where(v => v.fecha_emision.Year == anio)
                        .OrderByDescending(p => p.folio)
                        .ToListAsync();
                    // --- TERMINA CAMBIO ---

                    gridControl1.DataSource = valesDelAnio;
                }
            }
            catch (Exception ex)
            {
                string mensajeError = $"Error al cargar los vales: {ex.Message}";
        if (ex.InnerException != null)
        {
            mensajeError += $"\n\nDetalles: {ex.InnerException.Message}";
            if (ex.InnerException.InnerException != null)
            {
                mensajeError += $"\nMás Detalles: {ex.InnerException.InnerException.Message}";
            }
        }
        XtraMessageBox.Show(mensajeError, "Error de Conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                gridView1.HideLoadingPanel();
            }
        }

        private void cancelarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult r = XtraMessageBox.Show("¿Seguro que desea cancelar el vale seleccionado?. No se puede deshacer esta operación", "Cancelar vale", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
            if(r == DialogResult.Yes)
            {
                CancelaVale(Convert.ToInt32(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "folio").ToString()));
            }
        }

        private void CancelaVale(int folio)
        {
            using (var contexto = AyudanteDeConexion.CrearContexto())
            {
                vales vale = contexto.vales.SingleOrDefault(p => p.folio == folio);
                if (vale != null)
                {
                    vale.estatus = "C";
                    contexto.SaveChanges(); // <-- Se guarda el cambio en la BD

                    // --- LÍNEA A AGREGAR ---
                    // Vuelve a cargar los datos del año actual para refrescar el grid.
                    CargarValesPorAnio((int)beiAnio.EditValue);
                }
            }
        }

        private void borrarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult r = XtraMessageBox.Show("Al borrar un vale se perderá el folio para volver a ser usado, use esta opción en caso de error de impresión o para cambiar los datos de un vale ya registrado, ¿Desea continuar?", "Borrar vale", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
            if(r == DialogResult.Yes)
            {
                int folio = Convert.ToInt32(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "folio").ToString());
                vales vale = Program.Contexto.vales.SingleOrDefault(v => v.folio == folio);
                Program.Contexto.vales.Remove(vale);
                Program.Contexto.SaveChanges();
            }
        }

        private void FrmGestionVales_Load(object sender, EventArgs e)
        {
            if (!Auth.TienePermiso("Revertir vales canjeados")) revertirToolStripMenu.Enabled = false;
            if (!Auth.TienePermiso("Cancelar vales")) cancelarToolStripMenuItem.Enabled = false;
            if (!Auth.TienePermiso("Borrar vales")) borrarToolStripMenuItem.Enabled = false;
            if (!Auth.TienePermiso("Cambiar fecha a vales canjeados")) cambiarFechaDeCanjeToolStripMenuItem.Enabled = false;
        }

        private void revertirToolStripMenu_Click(object sender, EventArgs e)
        {
            string estatus = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "estatus").ToString();


            if (estatus == "A")
            {
                XtraMessageBox.Show("El vale aun no ha sido canjeado, solo se pueden revertir vales canjeados.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }


            var r = XtraMessageBox.Show("¿Seguro que desea revertir el vale seleccionado?", "Revertir vale", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
            if (r == DialogResult.Yes)
            {
                RevertirVale(Convert.ToInt32(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "folio").ToString()));
            }
        }

        private void RevertirVale(int folio)
        {
            using (var contexto = AyudanteDeConexion.CrearContexto())
            {
                vales vale = contexto.vales.SingleOrDefault(p => p.folio == folio);
                if (vale != null)
                {
                    vale.estatus = "A";
                    vale.fecha_canje = null;
                    vale.fecha_corte = null;
                    vale.id_estacion = null;
                    contexto.SaveChanges(); // <-- Se guarda el cambio en la BD

                    // --- LÍNEA A AGREGAR ---
                    // Vuelve a cargar los datos del año actual para refrescar el grid.
                    CargarValesPorAnio((int)beiAnio.EditValue);
                }
            }
        }

        private void cambiarFechaDeCanjeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string estatus = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "estatus").ToString();

            if (estatus == "C")
            {
                XtraMessageBox.Show("El vale está cancelado, solo se puede cambiar la fecha a vales canjeados.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (estatus == "A")
            {
                XtraMessageBox.Show("El vale aun no ha sido canjeado, solo se puede cambiar la fecha a vales canjeados.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            int vale = Convert.ToInt32(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "folio").ToString());
            using (var formFecha = new FrmFechaCanje(vale))
            {
                // 2. Usamos ShowDialog() que pausa el código hasta que el formulario se cierra.
                //    Comprobamos si el resultado fue 'OK' (es decir, si el usuario guardó).
                if (formFecha.ShowDialog() == DialogResult.OK)
                {
                    // 3. Si se guardaron cambios, refrescamos el grid.
                    CargarValesPorAnio((int)beiAnio.EditValue);
                }
            }
        }

        private void beiAnio_EditValueChanged(object sender, EventArgs e)
        {
            // Obtenemos el nuevo año seleccionado.
            if (beiAnio.EditValue is int anioSeleccionado)
            {
                // 1. Guardamos el nuevo año en la configuración para que se recuerde.
                Properties.Settings.Default.AnioFiltroVales = anioSeleccionado;
                Properties.Settings.Default.Save();

                // 2. Volvemos a cargar los datos del grid con el nuevo año.
                CargarValesPorAnio(anioSeleccionado);
            }
        }
    }
}