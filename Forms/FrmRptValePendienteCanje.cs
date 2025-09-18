using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.DataAccess.Sql;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraReports.UI;

namespace GestionValesRdz.Forms
{
    public partial class FrmRptValePendienteCanje : XtraForm
    {
        public FrmRptValePendienteCanje()
        {
            InitializeComponent();
            /*Carga clientes*/
            var cliente = Program.Contexto.clientes.OrderBy(c => c.nombre).ToList();
            cmbCliente.Properties.DataSource = cliente;
            cmbCliente.Properties.ValueMember = "id";
            cmbCliente.Properties.DisplayMember = "nombre";

            dtpDesde.DateTime = DateTime.Now;
            dtpHasta.DateTime = DateTime.Now;
        }

        private void btnGenerarReporte_Click(object sender, EventArgs e)
        {


            var auxclientes = cmbCliente.EditValue.ToString();
            var arrClientes = auxclientes.Split(new[] { ", " }, StringSplitOptions.RemoveEmptyEntries);

            var vale = new RptValesPendientesPorCanjear();
            SqlDataSource ds = vale.DataSource as SqlDataSource;
            ds.Connection.ConnectionString = string.Format(@"XpoProvider=MSSqlServer;data source={0};initial catalog={1};User Id=sa;Password=9753186400;", Properties.Settings.Default.servidor, Properties.Settings.Default.basedatos);
            vale.Parameters["FechaDesde"].Value = Convert.ToDateTime(dtpDesde.EditValue).Date;
            vale.Parameters["FechaHasta"].Value = Convert.ToDateTime(dtpHasta.EditValue).Date;
            vale.Parameters["IdCliente"].MultiValue = true;
            vale.Parameters["IdCliente"].Value = arrClientes;
            vale.ShowPrintMarginsWarning = false;
            ReportPrintTool tool = new ReportPrintTool(vale);
            tool.ShowRibbonPreview();
        }

       
    }
}
