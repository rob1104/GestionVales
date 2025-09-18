using System;
using System.Linq;
using DevExpress.XtraEditors;

namespace GestionValesRdz.Forms
{
    public partial class FrmVentaEspecifica : DevExpress.XtraEditors.XtraForm
    {
        public FrmVentaEspecifica()
        {
            InitializeComponent();
        }

        private void FrmVentaEspecifica_Load(object sender, EventArgs e)
        {
            cmbDesde.DateTime = DateTime.Now;
            cmbHasta.DateTime = DateTime.Now;
        }

        private void btnGenerar_Click(object sender, EventArgs e)
        {
            var misVentas = Program.Contexto.ventas.Select(
               v => new
               {
                   Folio_inicial = v.folioInicio,
                   Folio_final = v.folioFin,
                   Fecha_Venta = v.fecha,
                   Empresa = v.empresas.nombre,
                   Cliente = v.clientes.nombre,
                   Denominacion = v.denominacion,
                   Cantidad = v.cantidad,
                   Importe = v.importe
               }).Where(v => v.Fecha_Venta >= cmbDesde.DateTime.Date && v.Fecha_Venta <= cmbHasta.DateTime.Date).OrderByDescending(v => v.Fecha_Venta).ToList();
            gridControl1.DataSource = misVentas;
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