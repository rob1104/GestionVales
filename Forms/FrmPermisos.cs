using System;
using System.Data.Entity;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace GestionValesRdz.Forms
{
    public partial class FrmPermisos : XtraForm
    {
        public FrmPermisos()
        {
            InitializeComponent();
        }

        private void FrmPermisos_Load(object sender, EventArgs e)
        {
            CargaUsuarios();
            CargaTodosLosPermisos();
            cmbUsuario.ItemIndex = 0;
        }

        private void CargaUsuarios()
        {
            var usuarios = Program.Contexto.usuarios.ToList();
            cmbUsuario.Properties.DataSource = usuarios;
            cmbUsuario.Properties.ValueMember = "id";
            cmbUsuario.Properties.DisplayMember = "usuario";
        }

        private void CargaTodosLosPermisos()
        {
            Program.Contexto.permisos.Load();
            chkPermisos.DataSource = Program.Contexto.permisos.Local.ToBindingList();
            chkPermisos.DisplayMember = "permiso";
            chkPermisos.ValueMember = "id";
        }

        private void CargaPermisosUsuario(int id_usuario)
        {
            chkPermisos.UnCheckAll();
            var usuarios = Program.Contexto.usuarios.Where(x => x.id == id_usuario).ToArray();

            foreach (var usuario in usuarios)
            {
                foreach (var permiso in usuario.permisos)
                {
                    for (int i = 0; i < chkPermisos.ItemCount; i++)
                    {
                        var item = (permisos)chkPermisos.GetItem(i);
                        if (Convert.ToInt32(item.id) == permiso.id)
                            chkPermisos.SetItemChecked(i, true);
                    }
                }
            }

        }

        private void cmbUsuario_EditValueChanged(object sender, EventArgs e)
        {
            int usuario_id = Convert.ToInt32(cmbUsuario.EditValue);
            CargaPermisosUsuario(usuario_id);
        }

        private bool AplicaCambios()
        {
            try
            {
                var usuario_id = (int)cmbUsuario.EditValue;

                var borrarpermisos = Program.Contexto.usuarios.Find(usuario_id);
                borrarpermisos.permisos.Clear();
                Program.Contexto.SaveChanges();

                foreach (permisos itemChecked in chkPermisos.CheckedItems)
                {
                    int permiso_id = Convert.ToInt32(itemChecked.id);
                    usuarios user = Program.Contexto.usuarios.Single(x => x.id == usuario_id);
                    permisos p = Program.Contexto.permisos.Single(x => x.id == permiso_id);
                    p.usuarios.Add(user);
                    Program.Contexto.SaveChanges();
                }
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error al atualizar permisos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }

        }

        private void btnAplicar_Click(object sender, EventArgs e)
        {
            if (AplicaCambios())
            {
                XtraMessageBox.Show("Permisos asignados correctamente al usuario seleccionado", "Permisos", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                XtraMessageBox.Show("No se aplicaron los permisos, consulte al administrador del sistema", "Permisos", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void btnNada_Click(object sender, EventArgs e)
        {
            chkPermisos.UnCheckAll();
        }

        private void btnTodo_Click(object sender, EventArgs e)
        {
            chkPermisos.CheckAll();
        }
    }
}
