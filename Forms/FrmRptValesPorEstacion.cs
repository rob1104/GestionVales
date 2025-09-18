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
    public partial class FrmRptValesPorEstacion : DevExpress.XtraEditors.XtraForm
    {
        public FrmRptValesPorEstacion()
        {
            InitializeComponent();
            VerficaRadio();
        }

        private void FrmRptValesPorEstacion_Load(object sender, EventArgs e)
        {
            dtpDesdeCanje.DateTime = DateTime.Now;
            dtpHastaCanje.DateTime = DateTime.Now;
            dtpDesdeCorte.DateTime = DateTime.Now;
            dtpHastaCorte.DateTime = DateTime.Now;
        }

        private void btnGenerar_Click(object sender, EventArgs e)
        {
            if(radioCanje.Checked)
            {
                var misVales = Program.Contexto.vales.Select(
                v => new
                {
                    Folio = v.folio,
                    Fecha_Canje = v.fecha_canje,
                    Fecha_Corte = v.fecha_corte,
                    Estacion = v.estaciones.nombre,
                    Cliente = v.clientes.nombre,
                    Importe = v.importe,
                    Estatus = v.estatus
                }).Where(v => v.Fecha_Canje >= dtpDesdeCanje.DateTime.Date && v.Fecha_Canje <= dtpHastaCanje.DateTime.Date).OrderByDescending(v => v.Folio).ToList();
                gridControl1.DataSource = misVales;
            }
            else
            {
                var misVales = Program.Contexto.vales.Select(
                v => new
                {
                    Folio = v.folio,
                    Fecha_Canje = v.fecha_canje,
                    Fecha_Corte = v.fecha_corte,
                    Estacion = v.estaciones.nombre,
                    Cliente = v.clientes.nombre,
                    Importe = v.importe,
                    Estatus = v.estatus
                }).Where(v => v.Fecha_Corte >= dtpDesdeCorte.DateTime.Date && v.Fecha_Corte <= dtpHastaCorte.DateTime.Date).OrderByDescending(v => v.Folio).ToList();
                gridControl1.DataSource = misVales;
            }
            
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

        private void radioCorte_CheckedChanged(object sender, EventArgs e)
        {
            VerficaRadio();
        }

        private void VerficaRadio()
        {
            groupCorte.Enabled = radioCorte.Checked;
            groupCanje.Enabled = radioCanje.Checked;
        }
    }
}