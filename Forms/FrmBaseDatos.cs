using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace GestionValesRdz.Forms
{
    public partial class FrmBaseDatos : XtraForm
    {
        public FrmBaseDatos()
        {
            InitializeComponent();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.servidor = txtServidor.Text;
            Properties.Settings.Default.basedatos = txtBaseDatos.Text;
            Properties.Settings.Default.Save();
            XtraMessageBox.Show("El programa se cerrará para aplicar los cambios.");
            Application.Exit();
        }

        private void FrmBaseDatos_Load(object sender, EventArgs e)
        {
            txtServidor.Text = Properties.Settings.Default.servidor;
            txtBaseDatos.Text = Properties.Settings.Default.basedatos;
        }

        private void btnProbar_Click(object sender, EventArgs e)
        {
            if (Program.EstaElServidorConectado(txtServidor.Text, txtBaseDatos.Text))
            {
                XtraMessageBox.Show("Conexión establecida correctamente");
            }
            else
            {
                XtraMessageBox.Show("No se pudo establecer una conexion a la base de datos, verifique los parametros o contacte al administrador del sistema.");
            }
        }

        private void btnRestore_Click(object sender, EventArgs e)
        {
            // La advertencia y la selección del archivo no cambian.
            var confirmacion = XtraMessageBox.Show(
                "ADVERTENCIA: Este proceso REEMPLAZARÁ TODOS los datos actuales con los del archivo de respaldo. Esta acción es IRREVERSIBLE.\n\n" +
                "¿Está seguro de que desea continuar?",
                "Confirmación de Restauración",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirmacion != DialogResult.Yes) return;

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Archivos de Backup SQL Server (*.bak)|*.bak";
            openFileDialog.Title = "Seleccionar Archivo de Respaldo para Restaurar";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string rutaArchivo = openFileDialog.FileName;
                string baseDeDatos = txtBaseDatos.Text;

                XtraMessageBox.Show("Iniciando la restauración. Por favor, espere.", "Restaurando", MessageBoxButtons.OK, MessageBoxIcon.Information);

                try
                {
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

                        // --- INICIA CAMBIO ---
                        // 1. Verificar si la base de datos existe antes de intentar modificarla.
                        bool dbExists = false;
                        string checkDbQuery = $"SELECT COUNT(*) FROM sys.databases WHERE name = '{baseDeDatos}'";
                        using (SqlCommand checkDbCmd = new SqlCommand(checkDbQuery, connection))
                        {
                            dbExists = (Convert.ToInt32(checkDbCmd.ExecuteScalar()) > 0);
                        }

                        // 2. Si la base de datos existe, la ponemos en modo de un solo usuario.
                        if (dbExists)
                        {
                            string setSingleUserQuery = $"ALTER DATABASE [{baseDeDatos}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE";
                            using (SqlCommand singleUserCmd = new SqlCommand(setSingleUserQuery, connection))
                            {
                                singleUserCmd.ExecuteNonQuery();
                            }
                        }
                        // --- TERMINA CAMBIO ---

                        // 3. Ejecutar la restauración. Este comando CREARÁ la BD si no existe.
                        string restoreQuery = $"RESTORE DATABASE [{baseDeDatos}] FROM DISK = N'{rutaArchivo}' WITH FILE = 1, NOUNLOAD, REPLACE, STATS = 5";
                        using (SqlCommand restoreCmd = new SqlCommand(restoreQuery, connection))
                        {
                            restoreCmd.ExecuteNonQuery();
                        }

                        // 4. Poner la base de datos de nuevo en modo "multi usuario" para asegurar que sea accesible.
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
                }
            }
        }
    }
}
