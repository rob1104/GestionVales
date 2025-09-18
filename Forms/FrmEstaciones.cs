using DevExpress.XtraEditors;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GestionValesRdz.Forms
{
    public partial class FrmEstaciones : DevExpress.XtraEditors.XtraForm
    {
        private int _idEstacion;

        public FrmEstaciones()
        {
            InitializeComponent();
            Program.Contexto.estaciones.LoadAsync().ContinueWith(loadTask =>
            {
                gridControl1.DataSource = Program.Contexto.estaciones.Local.ToBindingList();
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void BorraEstacion(int idEstacion)
        {
            estaciones estacion = Program.Contexto.estaciones.SingleOrDefault(p => p.id == idEstacion);
            Program.Contexto.estaciones.Remove(estacion);
            Program.Contexto.SaveChanges();
        }

        private void editarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string id = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "id").ToString();
            _idEstacion = Convert.ToInt32(id);
            new FrmNuevaEstacion(_idEstacion, false).ShowDialog();
        }

        private void borrarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult r = XtraMessageBox.Show("¿Seguro que desea borrar a la estación seleccionada?", "Borrar estación", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
            if (r == DialogResult.Yes)
            {
                string id = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "id").ToString();
                _idEstacion = Convert.ToInt32(id);
                BorraEstacion(_idEstacion);
            }
        }

        private void FrmEstaciones_Load(object sender, EventArgs e)
        {
            if (!Auth.TienePermiso("Editar estaciones")) editarToolStripMenuItem.Enabled = false;
            if (!Auth.TienePermiso("Eliminar estaciones")) borrarToolStripMenuItem.Enabled = false;
        }
    }
}
