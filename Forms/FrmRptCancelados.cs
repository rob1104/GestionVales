using System;
using System.Linq;
using DevExpress.XtraEditors;

namespace GestionValesRdz.Forms
{
    public partial class FrmRptCancelados : DevExpress.XtraEditors.XtraForm
    {
        public FrmRptCancelados()
        {
            InitializeComponent();
        }


        private void FrmRptCancelados_Load(object sender, EventArgs e)
        {
            cmbDesde.DateTime = DateTime.Now;
            cmbHasta.DateTime = DateTime.Now;
        }

        private void btnGenerar_Click(object sender, EventArgs e)
        {
            var misVales = Program.Contexto.vales.Select(
                v => new
                {
                    Folio = v.folio,
                    Fecha_Emision = v.fecha_emision,
                    Empresa = v.empresas.nombre,
                    Cliente = v.clientes.nombre,
                    Importe = v.importe,
                    Estatus = v.estatus
            }).Where(v => v.Estatus == "C" && v.Fecha_Emision >= cmbDesde.DateTime.Date && v.Fecha_Emision <= cmbHasta.DateTime.Date).OrderByDescending(v => v.Folio).ToList();
            gridControl1.DataSource = misVales;
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            // Check whether the GridControl can be previewed.
            if (!gridControl1.IsPrintingAvailable)
            {
                XtraMessageBox.Show("The 'DevExpress.XtraPrinting' library is not found", "Error");
                return;
            }

            // Open the Preview window.
            gridControl1.ShowPrintPreview();
        }
    }
}