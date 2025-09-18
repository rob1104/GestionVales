using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Data.Entity;

namespace GestionValesRdz.Forms
{
    public partial class FrmGestionVales : XtraForm
    {
        public FrmGestionVales()
        {
            InitializeComponent();            
            // Call the LoadAsync method to asynchronously get the data for the given DbSet from the database.
            Program.Contexto.vales.LoadAsync().ContinueWith(loadTask =>
            {
            // Bind data to control when loading complete
            gridControl1.DataSource = Program.Contexto.vales.Local.ToBindingList().OrderByDescending(p => p.folio);
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void cancelarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult r = XtraMessageBox.Show("¿Seguro que desea cancelar el vale seleccionado?. No se puede deshacer esta operación", "Cancelar vale", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
            if(r == DialogResult.Yes)
            {
                CancelaVale(Convert.ToInt32(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "folio").ToString()));
            }
        }

        private void CancelaVale(int folio)
        {
            vales vale = Program.Contexto.vales.SingleOrDefault(p => p.folio == folio);
            vale.estatus = "C";
            Program.Contexto.SaveChanges();
        }

        private void borrarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult r = XtraMessageBox.Show("Al borrar un vale se perderá el folio para volver a ser usado, use esta opción en caso de error de impresión o para cambiar los datos de un vale ya registrado, ¿Desea continuar?", "Borrar vale", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
            if(r == DialogResult.Yes)
            {
                int folio = Convert.ToInt32(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "folio").ToString());
                vales vale = Program.Contexto.vales.SingleOrDefault(v => v.folio == folio);
                Program.Contexto.vales.Remove(vale);
                Program.Contexto.SaveChanges();
            }
        }

        private void FrmGestionVales_Load(object sender, EventArgs e)
        {
            if (!Auth.TienePermiso("Revertir vales canjeados")) revertirToolStripMenu.Enabled = false;
            if (!Auth.TienePermiso("Cancelar vales")) cancelarToolStripMenuItem.Enabled = false;
            if (!Auth.TienePermiso("Borrar vales")) borrarToolStripMenuItem.Enabled = false;
            if (!Auth.TienePermiso("Cambiar fecha a vales canjeados")) cambiarFechaDeCanjeToolStripMenuItem.Enabled = false;
        }

        private void revertirToolStripMenu_Click(object sender, EventArgs e)
        {
            string estatus = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "estatus").ToString();


            if (estatus == "A")
            {
                XtraMessageBox.Show("El vale aun no ha sido canjeado, solo se pueden revertir vales canjeados.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }


            var r = XtraMessageBox.Show("¿Seguro que desea revertir el vale seleccionado?", "Revertir vale", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
            if (r == DialogResult.Yes)
            {
                RevertirVale(Convert.ToInt32(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "folio").ToString()));
            }
        }

        private void RevertirVale(int folio)
        {
           
            vales vale = Program.Contexto.vales.SingleOrDefault(p => p.folio == folio);
            vale.estatus = "A";
            vale.fecha_canje = null;
            vale.fecha_corte = null;
            vale.id_estacion = null;
            Program.Contexto.SaveChanges();
        }

        private void cambiarFechaDeCanjeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string estatus = gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "estatus").ToString();

            if (estatus == "C")
            {
                XtraMessageBox.Show("El vale está cancelado, solo se puede cambiar la fecha a vales canjeados.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (estatus == "A")
            {
                XtraMessageBox.Show("El vale aun no ha sido canjeado, solo se puede cambiar la fecha a vales canjeados.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            int vale = Convert.ToInt32(gridView1.GetRowCellValue(gridView1.FocusedRowHandle, "folio").ToString());
            new FrmFechaCanje(vale).ShowDialog();
        }
    }
}