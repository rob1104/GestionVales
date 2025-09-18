using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using DevExpress.DataAccess.Sql;
using DevExpress.XtraEditors;
using DevExpress.XtraReports.UI;
using GestionValesRdz.Servicios;

namespace GestionValesRdz.Forms
{
    public partial class FrmPrincipal : XtraForm
    {
        private bool _isPrinting = false;

        public FrmPrincipal()
        {
            InitializeComponent();
            barStaticItem1.Caption = string.Format("Usuario: {0}", Program.NombreUsuario);
        }

        private void FrmPrincipal_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void btnNuevoUsuario_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            new FrmNuevoUsuario().ShowDialog();
        }

        private void btnBaseDatos_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            new FrmBaseDatos().ShowDialog();
        }

        private void btnUsuarios_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            foreach (Form form in Application.OpenForms)
            {
                if (form.GetType() == typeof(FrmUsuarios))
                {
                    form.Activate();
                    return;
                }
            }
            var FormUsuarios = new FrmUsuarios() { MdiParent = this };
            FormUsuarios.Show();
        }

        private void btnTemas_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            new FrmTemas().ShowDialog();
        }

        private void btnClientes_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            foreach(Form form in Application.OpenForms)
            {
                if(form.GetType() == typeof(FrmClientes))
                {
                    form.Activate();
                    return;
                }
            }

            var FormCLientes = new FrmClientes() { MdiParent = this };
            FormCLientes.Show();
        }

        private void btnNuevoCliente_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            new FrmNuevoCliente().ShowDialog();
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            foreach (Form form in Application.OpenForms)
            {
                if (form.GetType() == typeof(FrmLogs))
                {
                    form.Activate();
                    return;
                }
            }
            var FormLogs = new FrmLogs() { MdiParent = this };
            FormLogs.Show();
        }

        private void btnNuevaEmpresa_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            new FrmNuevaEmpresa().ShowDialog();
        }

        private void btnEmpresas_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            foreach (Form form in Application.OpenForms)
            {
                if (form.GetType() == typeof(FrmEmpresas))
                {
                    form.Activate();
                    return;
                }
            }
            var FormEmpresas = new FrmEmpresas() { MdiParent = this };
            FormEmpresas.Show();
        }

        private void btnRegistrarVales_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            new FrmRegistraVales().ShowDialog();
        }

        private void btnGestionVales_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var FormGestion = new FrmGestionVales() { MdiParent = this };
            FormGestion.Show();
        }

        private void btnReimpresión_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            new FrmReimpresion().ShowDialog();
        }

        private void btnCanjeVale_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            new FrmCanjeVales().ShowDialog();
        }

        private void btnValesCancelados_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            new FrmRptCancelados().ShowDialog();
        }

        private void btnValesRecibidos_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            new FrmRptValesRecibidos().ShowDialog();
        }

        private void btnValesPorDia_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            new FrmVentaEspecifica().ShowDialog();
        }

        private void btnValesEspecificos_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            new FrmRptVentaEspecificaPorCliente().ShowDialog();
        }

        private void btnEstaciones_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var FormGestion = new FrmEstaciones() { MdiParent = this };
            FormGestion.Show();
        }

        private void btnNuevaEstacion_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            new FrmNuevaEstacion().ShowDialog();
        }

        private void btnValesPorEstacion_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            new FrmRptValesPorEstacion().ShowDialog();
        }

        private void btnRptInd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            new FrmReporteValeInd().ShowDialog();
        }

        private void btnPermisos_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            new FrmPermisos().ShowDialog();
        }

        private void FrmPrincipal_Load(object sender, System.EventArgs e)
        {
            //CATALOGOS
            if (!Auth.TienePermiso("Registrar clientes")) btnNuevoCliente.Enabled = false;
            if (!Auth.TienePermiso("Ver clientes")) btnClientes.Enabled = false;
            if (!Auth.TienePermiso("Registrar empresas")) btnNuevaEmpresa.Enabled = false;
            if (!Auth.TienePermiso("Ver empresas")) btnEmpresas.Enabled = false;
            if (!Auth.TienePermiso("Registrar estaciones")) btnNuevaEstacion.Enabled = false;
            if (!Auth.TienePermiso("Ver estaciones")) btnEstaciones.Enabled = false;
            ////////////////////////////////

            //OPERACIONES
            if (!Auth.TienePermiso("Registrar vales")) btnRegistrarVales.Enabled = false;
            if (!Auth.TienePermiso("Ver vales")) btnGestionVales.Enabled = false;
            if (!Auth.TienePermiso("Canjear vales")) btnCanjeVale.Enabled = false;
            if (!Auth.TienePermiso("Reimprimir vales")) btnReimpresión.Enabled = false;
            ///////////////////////////////////

            //REPORTES
            if (!Auth.TienePermiso("Reporte de vales recibidos")) btnValesRecibidos.Enabled = false;
            if (!Auth.TienePermiso("Reporte de vales por venta")) btnValesPorDia.Enabled = false;
            if (!Auth.TienePermiso("Reporte de ventas especificas por cliente")) btnValesEspecificos.Enabled = false;
            if (!Auth.TienePermiso("Reporte de vales cancelados")) btnValesCancelados.Enabled = false;
            if (!Auth.TienePermiso("Reporte de vales por estación")) btnValesPorEstacion.Enabled = false;
            if (!Auth.TienePermiso("Reporte de vales emitidos por cliente y fecha")) btnRptInd.Enabled = false;
            if (!Auth.TienePermiso("Reporte de vales pendientes por canjear")) btnValesPendientes.Enabled = false;
            ///////////////////////////////////

            //CONFIGURACION
            if (!Auth.TienePermiso("Registrar usuarios")) btnNuevoUsuario.Enabled = false;
            if (!Auth.TienePermiso("Ver usuarios")) btnUsuarios.Enabled = false;
            if (!Auth.TienePermiso("Gestionar permisos")) btnPermisos.Enabled = false;
            if (!Auth.TienePermiso("Base de datos")) btnBaseDatos.Enabled = false;
            if (!Auth.TienePermiso("Temas")) btnTemas.Enabled = false;
            if (!Auth.TienePermiso("Logs")) barButtonItem3.Enabled = false;
        }

        private void btnValesPendientes_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            new FrmRptValePendienteCanje().ShowDialog();
        }

        private void timerImpresion_Tick(object sender, System.EventArgs e)
        {
            if (_isPrinting) return;

            if(PrintingService.Instance.TryPeekNextJob(out PrintJob currentJob))
            {
                _isPrinting = true;

                try
                {
                    //Lote actual
                    int batchSize = 50;
                    int startFolio = currentJob.StartFolio + currentJob.ValesImpresos;
                    int remainingVales = currentJob.TotalVales - currentJob.ValesImpresos;
                    int endFolio = startFolio + Math.Min(batchSize, remainingVales) - 1;

                    PrintBatch(startFolio, endFolio, currentJob.ConnectionString);
                    currentJob.ValesImpresos += Math.Min(batchSize, remainingVales);
                    if (currentJob.ValesImpresos >= currentJob.TotalVales)
                    {
                        PrintingService.Instance.TryDequeueJob(out _);
                    }
                }
                catch (Exception ex)
                {
                    PrintingService.Instance.TryDequeueJob(out _);
                }
                finally
                {
                    _isPrinting = false;
                }
            }           
        }

        private void PrintBatch(int startFolio, int endFolio, string connectionString)
        {
            // Esta es la misma lógica de impresión, ahora viviendo de forma segura aquí.
            using (var vale = new rptVale2())
            {
                if (vale.DataSource is SqlDataSource ds)
                {
                    ds.Connection.ConnectionString = connectionString;
                }

                vale.Parameters["folio"].Value = startFolio.ToString();
                vale.Parameters["folio"].Visible = false;
                vale.Parameters["folio2"].Value = endFolio;
                vale.Parameters["folio2"].Visible = false;
                vale.ShowPrintMarginsWarning = false;

                using (ReportPrintTool tool = new ReportPrintTool(vale))
                {
                    tool.Print();
                }
            }
        }

        private void btnRespaldoBase_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            // 1. Abrir el diálogo para que el usuario elija dónde guardar el archivo.
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Archivos de Backup SQL Server (*.bak)|*.bak";
            saveFileDialog.Title = "Guardar Respaldo de Base de Datos";
            // Sugerimos un nombre de archivo con la fecha y hora actual.
            saveFileDialog.FileName = $"GestionValesRdz_{DateTime.Now:yyyyMMdd_HHmmss}.bak";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string rutaArchivo = saveFileDialog.FileName;
                string baseDeDatos = Properties.Settings.Default.basedatos;

              

                try
                {
                    // 2. Construir la cadena de conexión al servidor (a la base de datos 'master').
                    // Para respaldar, es mejor conectarse a 'master'.
                    var connectionStringBuilder = new SqlConnectionStringBuilder
                    {
                        DataSource = Properties.Settings.Default.servidor,
                        InitialCatalog = "master", // Conectar a master para operaciones administrativas
                        UserID = "sa",
                        Password = "9753186400", // Cuidado con tener contraseñas en el código
                        IntegratedSecurity = false
                    };

                    // 3. Ejecutar el comando SQL de BACKUP.
                    using (SqlConnection connection = new SqlConnection(connectionStringBuilder.ConnectionString))
                    {
                        string query = $"BACKUP DATABASE [{baseDeDatos}] TO DISK = N'{rutaArchivo}' WITH NOFORMAT, INIT, NAME = N'{baseDeDatos}-Full Database Backup', SKIP, NOREWIND, NOUNLOAD, STATS = 10";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            connection.Open();
                            command.ExecuteNonQuery();
                        }
                    }

                    XtraMessageBox.Show("¡Respaldo completado exitosamente!", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    XtraMessageBox.Show($"Ocurrió un error al crear el respaldo:\n\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnRestaurarBase_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            // 1. ADVERTENCIA CRÍTICA para el usuario.
            var confirmacion = XtraMessageBox.Show(
                "ADVERTENCIA: Está a punto de restaurar la base de datos.\n\n" +
                "Este proceso REEMPLAZARÁ TODOS los datos actuales con los del archivo de respaldo. Esta acción es IRREVERSIBLE.\n\n" +
                "¿Está seguro de que desea continuar?",
                "Confirmación de Restauración",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirmacion != DialogResult.Yes)
            {
                return; // El usuario canceló la operación.
            }

            // 2. Abrir el diálogo para que el usuario elija el archivo .bak.
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Archivos de Backup SQL Server (*.bak)|*.bak";
            openFileDialog.Title = "Seleccionar Archivo de Respaldo para Restaurar";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string rutaArchivo = openFileDialog.FileName;
                string baseDeDatos = Properties.Settings.Default.basedatos;

                XtraMessageBox.Show("Iniciando la restauración. Esta operación podria tardar. Presione aceptar para continuar, espere.", "Restaurando", MessageBoxButtons.OK, MessageBoxIcon.Information);

                try
                {
                    // 3. Conectar a la base de datos 'master' para tomar control.
                    var connectionStringBuilder = new SqlConnectionStringBuilder
                    {
                        DataSource = Properties.Settings.Default.servidor,
                        InitialCatalog = "master",
                        UserID = "sa",
                        Password = "9753186400",
                        IntegratedSecurity = false
                    };

                    using (SqlConnection connection = new SqlConnection(connectionStringBuilder.ConnectionString))
                    {
                        connection.Open();

                        // 4. Poner la base de datos en modo "un solo usuario" para desconectar a todos.
                        string setSingleUserQuery = $"ALTER DATABASE [{baseDeDatos}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE";
                        using (SqlCommand singleUserCmd = new SqlCommand(setSingleUserQuery, connection))
                        {
                            singleUserCmd.ExecuteNonQuery();
                        }

                        // 5. Ejecutar el comando SQL de RESTORE.
                        string restoreQuery = $"RESTORE DATABASE [{baseDeDatos}] FROM DISK = N'{rutaArchivo}' WITH FILE = 1, NOUNLOAD, REPLACE, STATS = 5";
                        using (SqlCommand restoreCmd = new SqlCommand(restoreQuery, connection))
                        {
                            restoreCmd.ExecuteNonQuery();
                        }

                        // 6. Poner la base de datos de nuevo en modo "multi usuario".
                        string setMultiUserQuery = $"ALTER DATABASE [{baseDeDatos}] SET MULTI_USER";
                        using (SqlCommand multiUserCmd = new SqlCommand(setMultiUserQuery, connection))
                        {
                            multiUserCmd.ExecuteNonQuery();
                        }
                    }

                    XtraMessageBox.Show("¡Restauración completada exitosamente! Se recomienda reiniciar la aplicación.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    XtraMessageBox.Show($"Ocurrió un error durante la restauración:\n\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    // Intenta volver a poner la BD en modo multi-usuario si falla la restauración.
                    try
                    {
                        // ... (código para volver a conectar y ejecutar "ALTER DATABASE ... SET MULTI_USER")
                    }
                    catch { }
                }
            }
        }
    }
}
