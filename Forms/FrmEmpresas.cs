using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace GestionValesRdz.Forms
{
    public partial class FrmEmpresas : XtraForm
    {
        private int _idEmpresa;
        public FrmEmpresas()
        {
            InitializeComponent();

            Program.Contexto.empresas.LoadAsync().ContinueWith(loadTask =>
            {
                gridControl1.DataSource = Program.Contexto.empresas.Local.ToBindingList();
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void editarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string id = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "id").ToString();
            _idEmpresa = Convert.ToInt32(id);
            new FrmNuevaEmpresa(_idEmpresa, false).ShowDialog();
        }

        private void borrarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult r = XtraMessageBox.Show("¿Seguro que desea borrar a la empresa seleccionado?", "Borrar empresa", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
            if (r == DialogResult.Yes)
            {
                string id = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "id").ToString();
                _idEmpresa = Convert.ToInt32(id);
                BorraEmpresa(_idEmpresa);
            }
        }

        private void BorraEmpresa(int idEmpresa)
        {
            empresas empresa = Program.Contexto.empresas.SingleOrDefault(p => p.id == idEmpresa);
            Program.Contexto.empresas.Remove(empresa);
            Program.Contexto.SaveChanges();
        }

        private void FrmEmpresas_Load(object sender, EventArgs e)
        {
            if (!Auth.TienePermiso("Editar empresas")) editarToolStripMenuItem.Enabled = false;
            if (!Auth.TienePermiso("Eliminar empresas")) borrarToolStripMenuItem.Enabled = false;
        }
    }
}