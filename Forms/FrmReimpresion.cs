using System;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraReports.UI;
using DevExpress.DataAccess.Sql;

namespace GestionValesRdz.Forms
{
    public partial class FrmReimpresion : XtraForm
    {
        public FrmReimpresion()
        {
            InitializeComponent();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                int ini = Convert.ToInt32(txtFolioInicial.Text);
                int fin = Convert.ToInt32(txtFolioFinal.Text);
                /*for (int i = ini; i <= fin; i++)
                {
                    vales miVale = Program.Contexto.vales.FirstOrDefault(v => v.folio == i);
                    if(miVale == null)
                    {
                        XtraMessageBox.Show(string.Format("El vale con el folio {0} no está registrado", i),"Vales", MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                    }
                    else if(miVale.estatus == "C")
                    {
                        XtraMessageBox.Show(string.Format("El vale con el folio {0} está cancelado, no puede reimprimirse", i), "Vales", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    else if (miVale.estatus == "P")
                    {
                        XtraMessageBox.Show(string.Format("El vale con el folio {0} ya ha sido canjeado y no puede reimprimirse", i), "Vales", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    else
                    {
                        rptVale2 vale = new rptVale2();
                        SqlDataSource ds = vale.DataSource as SqlDataSource;
                        ds.Connection.ConnectionString = string.Format(@"XpoProvider=MSSqlServer;data source={0};initial catalog={1};User Id=sa;Password=9753186400;", Properties.Settings.Default.servidor, Properties.Settings.Default.basedatos);
                        vale.Parameters["folio"].Value = i.ToString();
                        vale.Parameters["folio"].Visible = false;
                        vale.ShowPrintMarginsWarning = false;
                        ReportPrintTool tool = new ReportPrintTool(vale);
                        tool.Print();
                        tool.Dispose();
                        vale.Dispose();
                    }
                }*/

                var vale = new rptVale2();
                SqlDataSource ds = vale.DataSource as SqlDataSource;
                ds.Connection.ConnectionString = string.Format(@"XpoProvider=MSSqlServer;data source={0};initial catalog={1};User Id=sa;Password=9753186400;", Properties.Settings.Default.servidor, Properties.Settings.Default.basedatos);
                vale.Parameters["folio"].Value = ini;
                vale.Parameters["folio"].Visible = false;
                vale.Parameters["folio2"].Value = fin;
                vale.Parameters["folio2"].Visible = false;
                vale.ShowPrintMarginsWarning = false;
                ReportPrintTool tool = new ReportPrintTool(vale);
                tool.Print();
                tool.Dispose();
                vale.Dispose();
                Dispose();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "Verifique el rango de folios");
            }
            
        }
    }
}