using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace GestionValesRdz.Forms
{
    public partial class FrmFechaCanje : XtraForm
    {
        private int _vale;
        public FrmFechaCanje(int vale)
        {
            _vale = vale;
            InitializeComponent();
        }

        private void FrmFechaCanje_Load(object sender, EventArgs e)
        {
            dtFecha.EditValue = DateTime.Today;
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            var vale = Program.Contexto.vales.SingleOrDefault(p => p.folio == _vale);
            vale.fecha_canje = Convert.ToDateTime(dtFecha.EditValue);
            vale.fecha_corte = Convert.ToDateTime(dtFecha.EditValue);
            Program.Contexto.SaveChanges();
            XtraMessageBox.Show("Fecha cambiada correctamente");
            Dispose();
        }
    }
}
