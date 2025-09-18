using System;
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
    }
}
