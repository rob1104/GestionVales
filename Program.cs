using System;
using System.Windows.Forms;
using GestionValesRdz.Forms;
using DevExpress.LookAndFeel;
using DevExpress.UserSkins;
using DevExpress.Skins;
using System.Data.SqlClient;
using System.Diagnostics;

namespace GestionValesRdz
{
    static class Program
    {
        public static string Usuario;
        public static int IdUsuario;
        public static string NombreUsuario;
        public static ValesRdzDatosEntities Contexto = 
            new ValesRdzDatosEntities(AyudanteDeConexion.CrearCadenaDeConexion(Properties.Settings.Default.servidor, Properties.Settings.Default.basedatos));
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {          
            if(Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Length > 1)
            {
                MessageBox.Show("Solo puede ejecutar la aplicación una vez", "Xis Desarrollos", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            BonusSkins.Register();
            SkinManager.EnableFormSkins();
            if (Properties.Settings.Default.tema == string.Empty)
                UserLookAndFeel.Default.SkinName = "Office 2016 Colorful";
            else
                UserLookAndFeel.Default.SkinName = Properties.Settings.Default.tema;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmLogin());
        }

        public static bool EstaElServidorConectado(string servidor, string datos)
        {
            string cs = AyudanteDeConexion.CrearCadenaSencilla(servidor, datos);
            using (SqlConnection con = new SqlConnection(cs))
            {
                try
                {
                    con.Open();
                    con.ChangeDatabase(datos);
                    return true;
                }
                catch(SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                    return false;
                }
            }
        }
    }
}
