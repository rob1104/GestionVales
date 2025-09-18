using DevExpress.XtraEditors;
using System;
using System.Data.Entity;
using System.Linq;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace GestionValesRdz.Forms
{
    public partial class FrmUsuarios : XtraForm
    {

        private int _idUsuario;
        public FrmUsuarios()
        {
            InitializeComponent();
            // Call the LoadAsync method to asynchronously get the data for the given DbSet from the database.
            Program.Contexto.usuarios.LoadAsync().ContinueWith(loadTask =>
            {
                // Bind data to control when loading complete
                gridControl1.DataSource = Program.Contexto.usuarios.Local.ToBindingList();
            },  
            TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void editarToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            string id = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "id").ToString();
            _idUsuario = Convert.ToInt32(id);
            new FrmNuevoUsuario(_idUsuario, false).ShowDialog();
        }

        private void borrarToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            DialogResult r = XtraMessageBox.Show("¿Seguro que desea borrar al usuario seleccionado?", "Borrar usuario", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
            if(r == DialogResult.Yes)
            {
                string id = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "id").ToString();
                _idUsuario = Convert.ToInt32(id);
                BorraUsuario(_idUsuario);
            }            
        }

        private void BorraUsuario(int idUsuario)
        {
            usuarios usuario = Program.Contexto.usuarios.SingleOrDefault(p => p.id == idUsuario);
            Program.Contexto.usuarios.Remove(usuario);
            Program.Contexto.SaveChanges();
        }

        private void cm_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (gridView1.RowCount <= 0)
                e.Cancel = true;
        }
    }
}