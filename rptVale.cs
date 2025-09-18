using System;
using System.Linq;
using DevExpress.XtraReports.UI;

namespace GestionValesRdz
{
    public partial class rptVale : XtraReport
    {
        public rptVale()
        {
            InitializeComponent();
        }

        private void rptVale_DataSourceDemanded(object sender, EventArgs e)
        {
            DataSource = (from ex in Program.Contexto.vales                        
                                 select ex).ToList();
            
        }
    }
}
