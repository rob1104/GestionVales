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

namespace GestionValesRdz.Forms
{
    public partial class FrmRptValesRecibidos : DevExpress.XtraEditors.XtraForm
    {
        public FrmRptValesRecibidos()
        {
            InitializeComponent();
        }

        private void btnGenerar_Click(object sender, EventArgs e)
        {
            var misVales = Program.Contexto.vales.Select(
                v => new
                {
                    Folio = v.folio,
                    Fecha_Emision = v.fecha_emision,
                    Fecha_Corte = v.fecha_corte,
                    Empresa = v.empresas.nombre,
                    Cliente = v.clientes.nombre,
                    Importe = v.importe,
                    Estatus = v.estatus
                }).Where(v => v.Fecha_Emision >= cmbDesde.DateTime.Date && v.Fecha_Emision <= cmbHasta.DateTime.Date).OrderByDescending(v => v.Folio).ToList();
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

        private void FrmRptValesRecibidos_Load(object sender, EventArgs e)
        {
            cmbDesde.DateTime = DateTime.Now;
            cmbHasta.DateTime = DateTime.Now;
        }
    }
}