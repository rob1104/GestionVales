using System;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using GestionValesRdz.Servicios;

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
            //try
            //{
            //    int ini = Convert.ToInt32(txtFolioInicial.Text);
            //    int fin = Convert.ToInt32(txtFolioFinal.Text);
            //    /*for (int i = ini; i <= fin; i++)
            //    {
            //        vales miVale = Program.Contexto.vales.FirstOrDefault(v => v.folio == i);
            //        if(miVale == null)
            //        {
            //            XtraMessageBox.Show(string.Format("El vale con el folio {0} no está registrado", i),"Vales", MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
            //        }
            //        else if(miVale.estatus == "C")
            //        {
            //            XtraMessageBox.Show(string.Format("El vale con el folio {0} está cancelado, no puede reimprimirse", i), "Vales", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //        }
            //        else if (miVale.estatus == "P")
            //        {
            //            XtraMessageBox.Show(string.Format("El vale con el folio {0} ya ha sido canjeado y no puede reimprimirse", i), "Vales", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //        }
            //        else
            //        {
            //            rptVale2 vale = new rptVale2();
            //            SqlDataSource ds = vale.DataSource as SqlDataSource;
            //            ds.Connection.ConnectionString = string.Format(@"XpoProvider=MSSqlServer;data source={0};initial catalog={1};User Id=sa;Password=9753186400;", Properties.Settings.Default.servidor, Properties.Settings.Default.basedatos);
            //            vale.Parameters["folio"].Value = i.ToString();
            //            vale.Parameters["folio"].Visible = false;
            //            vale.ShowPrintMarginsWarning = false;
            //            ReportPrintTool tool = new ReportPrintTool(vale);
            //            tool.Print();
            //            tool.Dispose();
            //            vale.Dispose();
            //        }
            //    }*/

            //    var vale = new rptVale2();
            //    SqlDataSource ds = vale.DataSource as SqlDataSource;
            //    ds.Connection.ConnectionString = string.Format(@"XpoProvider=MSSqlServer;data source={0};initial catalog={1};User Id=sa;Password=9753186400;", Properties.Settings.Default.servidor, Properties.Settings.Default.basedatos);
            //    vale.Parameters["folio"].Value = ini;
            //    vale.Parameters["folio"].Visible = false;
            //    vale.Parameters["folio2"].Value = fin;
            //    vale.Parameters["folio2"].Visible = false;
            //    vale.ShowPrintMarginsWarning = false;
            //    ReportPrintTool tool = new ReportPrintTool(vale);
            //    tool.Print();
            //    tool.Dispose();
            //    vale.Dispose();
            //    Dispose();
            //}
            //catch (Exception ex)
            //{
            //    XtraMessageBox.Show(ex.Message, "Verifique el rango de folios");
            //}

            try
            {
                // 1. Validamos los datos de entrada.
                if (!int.TryParse(txtFolioInicial.Text, out int folioInicial) || !int.TryParse(txtFolioFinal.Text, out int folioFinal))
                {
                    XtraMessageBox.Show("Por favor, ingrese números de folio válidos.", "Datos Inválidos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (folioFinal < folioInicial)
                {
                    XtraMessageBox.Show("El folio final no puede ser menor que el folio inicial.", "Rango Inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 2. Calculamos el total de vales a reimprimir.
                int totalVales = (folioFinal - folioInicial) + 1;

                // 3. Creamos el objeto del trabajo de impresión.
                var printJob = new PrintJob
                {
                    JobName = $"Reimpresión Folios {folioInicial}-{folioFinal}",
                    StartFolio = folioInicial,
                    TotalVales = totalVales,
                    ConnectionString = string.Format(@"XpoProvider=MSSqlServer;data source={0};initial catalog={1};User Id=sa;Password=9753186400;",
                        Properties.Settings.Default.servidor, Properties.Settings.Default.basedatos)
                };

                // 4. Añadimos el trabajo a la cola del servicio de impresión.
                PrintingService.Instance.AddPrintJob(printJob);

                // 5. Informamos al usuario y cerramos el formulario.
                XtraMessageBox.Show($"El trabajo de reimpresión para {totalVales} vales se ha agregado a la cola.",
                    "Reimpresión en Cola", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.Close();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Ocurrió un error al preparar la reimpresión: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}