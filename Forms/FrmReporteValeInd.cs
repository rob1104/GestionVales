using DevExpress.DataAccess.Sql;
using DevExpress.XtraEditors;
using DevExpress.XtraReports.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GestionValesRdz.Forms
{
    public partial class FrmReporteValeInd : XtraForm
    {
        public FrmReporteValeInd()
        {
            InitializeComponent();
            /*Carga clientes*/
            var cliente = Program.Contexto.clientes.ToList();
            cmbCliente.Properties.DataSource = cliente;
            cmbCliente.Properties.ValueMember = "id";
            cmbCliente.Properties.DisplayMember = "nombre";

            dtpDesde.DateTime = DateTime.Now;
            dtpHasta.DateTime = DateTime.Now;
        }

        private void btnGenerarReporte_Click(object sender, EventArgs e)
        {
            if(cmbCliente.EditValue == null)
            {
                XtraMessageBox.Show("Seleccione un cliente", "Reportes", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbCliente.Focus();
                return;
            }

            var vale = new RptValesIndividuales();
            SqlDataSource ds = vale.DataSource as SqlDataSource;
            ds.Connection.ConnectionString = string.Format(@"XpoProvider=MSSqlServer;data source={0};initial catalog={1};User Id=sa;Password=9753186400;", Properties.Settings.Default.servidor, Properties.Settings.Default.basedatos);
            vale.Parameters["parc"].Value = Convert.ToInt64(cmbCliente.EditValue);
            vale.Parameters["parfd"].Value = Convert.ToDateTime(dtpDesde.EditValue).Date;
            vale.Parameters["parfh"].Value = Convert.ToDateTime(dtpHasta.EditValue).Date;
            vale.Parameters["cliente"].Value = cmbCliente.Text;
            vale.Parameters["usuario"].Value = Program.Usuario;
            vale.ShowPrintMarginsWarning = false;
            ReportPrintTool tool = new ReportPrintTool(vale);
            tool.ShowRibbonPreview();
        }
    }
}
