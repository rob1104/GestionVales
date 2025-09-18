using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Text;

namespace GestionValesRdz.Forms
{
    public partial class FrmNuevoUsuario : XtraForm
    {
        private bool _esNuevoUsuario;
        private int _idUsuario;

        public FrmNuevoUsuario()
        {
            InitializeComponent();
            _esNuevoUsuario = true;
        }

        public FrmNuevoUsuario(int idUsuario, bool esNuevoUsuario)
        {
            InitializeComponent();
            _idUsuario = idUsuario;
            _esNuevoUsuario = esNuevoUsuario;
        }

        private void CargaUsuario()
        {
            usuarios usuario = Program.Contexto.usuarios.SingleOrDefault(p => p.id == _idUsuario);
            txtNombre.Text = usuario.nombre;
            txtUsuario.Text = usuario.usuario;
            txtPass.Text = Encoding.UTF8.GetString(Convert.FromBase64String(usuario.contrasena));
            txtPass2.Text = txtPass.Text;
            txtObs.Text = usuario.obs;
            cmbEstatus.EditValue = usuario.estatus;
            Text = string.Format("Editar usuario No. {0}", usuario.id);
        }

        private void FrmNuevoUsuario_Load(object sender, EventArgs e)
        {
            var estatus = new Dictionary<string, string>();
            estatus.Add("A", "ACTIVO");
            estatus.Add("I", "INACTIVO");
            estatus.Add("S", "SUSPENDIDO");
            estatus.Add("B", "BAJA");
            cmbEstatus.Properties.DataSource = estatus;
            cmbEstatus.Properties.DisplayMember = "Value";
            cmbEstatus.Properties.ValueMember = "Key";
            cmbEstatus.ItemIndex = 0;
            if (!_esNuevoUsuario)
                CargaUsuario();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (txtPass.Text != txtPass2.Text)
            {
                XtraMessageBox.Show("Las contraseñas no coinciden, verifique.", "Usuarios", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtPass2.Select();
                return;
            }


            if(txtUsuario.Text == string.Empty)
            {
                XtraMessageBox.Show("El nombre de usuario es requerido, verifique.", "Usuarios", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtUsuario.Select();
                return;
            }

            if (txtPass.Text == string.Empty)
            {
                XtraMessageBox.Show("Lacontraseña es requerida, verifique.", "Usuarios", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtPass.Select();
                return;
            }
       
            Guarda();
        }

        private void Guarda()
        {
            if(_esNuevoUsuario)
            {
                usuarios usuario = new usuarios()
                {
                    estatus = cmbEstatus.EditValue.ToString(),
                    nombre = txtNombre.Text,
                    usuario = txtUsuario.Text,
                    contrasena = Convert.ToBase64String(Encoding.UTF8.GetBytes(txtPass.Text)),
                    fecha_alta = DateTime.Today,
                    obs = txtObs.Text
                };
                Program.Contexto.usuarios.Add(usuario);
            }
            else
            {
                usuarios usuario = Program.Contexto.usuarios.SingleOrDefault(p => p.id == _idUsuario);
                usuario.estatus = cmbEstatus.EditValue.ToString();
                usuario.nombre = txtNombre.Text;
                usuario.usuario = txtUsuario.Text;
                usuario.contrasena = Convert.ToBase64String(Encoding.UTF8.GetBytes(txtPass.Text));
                usuario.obs = txtObs.Text;
            }            
            Program.Contexto.SaveChanges();
            Dispose();
        }
    }
}