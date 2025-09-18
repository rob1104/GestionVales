using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace GestionValesRdz.Forms
{
    public partial class FrmLogin : XtraForm
    {
        private int _idUsuario;
        private string _nombreUsuario;
        public FrmLogin()
        {
            InitializeComponent();
            
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            VerificaCS();
            if (Login(txtUsuario.Text, txtPassword.Text))
            {
                Program.IdUsuario = _idUsuario;
                Program.NombreUsuario = _nombreUsuario;
                Program.Usuario = txtUsuario.Text;

                var userLogged = Program.Contexto.usuarios.Where(x => x.usuario == txtUsuario.Text && x.estatus == "A").FirstOrDefault();

                Auth.Id = userLogged.id;
                Auth.Usuario = userLogged.usuario;
                Auth.Nombre = userLogged.nombre;
                Auth.Permisos = Permisos(Auth.Id);

                InsertaLog();

                InsertaLog();
                Hide();
                new FrmPrincipal().Show();
            }
            else
            {
                XtraMessageBox.Show("Usuario o contraseña incorrectos, intente de nuevo.", "Entrar al sistema", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtPassword.Select();
            }
        }

        public List<permisos> Permisos(int id)
        {
            int user_id = id;
            return Program.Contexto.usuarios.Where(x => x.id == id).SelectMany(x => x.permisos).ToList();
        }

        private void VerificaCS()
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var connectionStringsSection = (ConnectionStringsSection)config.GetSection("connectionStrings");
            connectionStringsSection.ConnectionStrings [@"localhost\sqlexpress_ValesRdzDatos_Connection"].ConnectionString = string.Format(@"XpoProvider=MSSqlServer;data source={0};integrated security=SSPI;initial catalog=ValesRdzDatos", Properties.Settings.Default.servidor);
            config.Save();
            ConfigurationManager.RefreshSection("connectionStrings");
        }

        private void InsertaLog()
        {
            var log = new logs()
            {
                fecha = DateTime.Today,
                hora = DateTime.Now.TimeOfDay,
                id_usuario = Program.IdUsuario,
                host = Environment.MachineName,
                direccion_ip = GetIp()
            };
            Program.Contexto.logs.Add(log);

            try
            {
                Program.Contexto.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    string error = string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    MessageBox.Show(error);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        string detail = string.Format("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                        MessageBox.Show(detail);
                    }
                }
                throw;
            }
        }

        public string GetIp()
        {
            try
            {
                return new WebClient().DownloadString("http://icanhazip.com");
            }
            catch (Exception)
            {
                return Dns.GetHostEntry(Dns.GetHostName()).AddressList.First(f => f.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork).ToString();
            }
        }

        private bool Login(string user, string pass)
        {
            pass = Convert.ToBase64String(Encoding.UTF8.GetBytes(pass));
            var query = (from b in Program.Contexto.usuarios where b.usuario == user && b.contrasena == pass select b).ToList();
            if (query.Count > 0)
            {
                _idUsuario = query.FirstOrDefault().id;
                _nombreUsuario = query.FirstOrDefault().nombre;
            }
            return query.Count > 0;
        }

        private void FrmLogin_Load(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.servidor == string.Empty || Properties.Settings.Default.basedatos == string.Empty)
            {
                XtraMessageBox.Show("El motor de base de datos SQL Server no ha sido configurado, se abrirá la configuración para capturar los datos requeridos.", "SQL Server", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                new FrmBaseDatos().ShowDialog();
            }
            try
            {

            }
            catch (Exception)
            {
                var usuariosRegistrados = Program.Contexto.usuarios.FirstOrDefault(ex => ex.estatus == "A");
                if (usuariosRegistrados == null)
                {
                    XtraMessageBox.Show("No hay usuarios registrados en el sistema, se abrirá la configuración para crear un nuevo usuario.", "Usuarios", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    new FrmNuevoUsuario().ShowDialog();
                    Application.Exit();
                }
            }
            catch { }
            

            //lblVersion.Text = "Versión: " + Application.ProductVersion;
        }

        private void lblVersion_DoubleClick(object sender, EventArgs e)
        {
            new FrmBaseDatos().ShowDialog();
        }

        private void lblVersion_Click(object sender, EventArgs e)
        {
            new FrmBaseDatos().ShowDialog();
        }
    }
}
