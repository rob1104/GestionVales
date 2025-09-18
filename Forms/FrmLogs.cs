using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Data.Entity;
using DevExpress.Data;

namespace GestionValesRdz.Forms
{
    public partial class FrmLogs : XtraForm
    {
        public FrmLogs()
        {
            InitializeComponent();
            // Call the LoadAsync method to asynchronously get the data for the given DbSet from the database.
            Program.Contexto.logs.LoadAsync().ContinueWith(loadTask =>
            {
                // Bind data to control when loading complete
                gridControl1.DataSource = Program.Contexto.logs.Local.ToBindingList();
            }, TaskScheduler.FromCurrentSynchronizationContext());

            gridView1.Columns["id"].SortOrder = ColumnSortOrder.Descending;
        }
    }
}