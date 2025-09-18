using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Data.Entity;
using System.Linq;

namespace GestionValesRdz.Forms
{
    public partial class FrmClientes : XtraForm
    {
        private int _idCliente;
        public FrmClientes()
        {
            InitializeComponent();         
            // Call the LoadAsync method to asynchronously get the data for the given DbSet from the database.
            Program.Contexto.clientes.LoadAsync().ContinueWith(loadTask =>
            {
                // Bind data to control when loading complete
                gridControl1.DataSource = Program.Contexto.clientes.Local.ToBindingList();
            }, 
            TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void editarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string id = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "id").ToString();
            _idCliente = Convert.ToInt32(id);
            new FrmNuevoCliente(_idCliente, false).ShowDialog();
        }

        private void borrarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult r = XtraMessageBox.Show("¿Seguro que desea borrar al cliente seleccionado?", "Borrar cliente", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
            if (r == DialogResult.Yes)
            {
                string id = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "id").ToString();
                _idCliente = Convert.ToInt32(id);
                BorraCliente(_idCliente);
            }
        }

        private void BorraCliente(int idCliente)
        {
            clientes cliente = Program.Contexto.clientes.SingleOrDefault(p => p.id == idCliente);
            Program.Contexto.clientes.Remove(cliente);
            Program.Contexto.SaveChanges();
        }

        private void FrmClientes_Load(object sender, EventArgs e)
        {
            if (!Auth.TienePermiso("Editar clientes")) editarToolStripMenuItem.Enabled = false;
            if (!Auth.TienePermiso("Eliminar clientes")) borrarToolStripMenuItem.Enabled = false;
        }
    }
}