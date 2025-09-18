using System;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace GestionValesRdz.Forms
{
    public partial class FrmCanjeVales : XtraForm
    {
        public FrmCanjeVales()
        {
            InitializeComponent();
        }

        public void BuscaVale(string folio)
        {
            try
            {
                if (cmbEstacion.EditValue == null)
                {
                    XtraMessageBox.Show("Seleccione una estación", "Canje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    cmbEstacion.Select();
                    return;
                }
                vales vale = Program.Contexto.vales.SingleOrDefault(v => v.codigo == folio);
                if(vale.estatus == "C")
                {
                    XtraMessageBox.Show("No se puede canjear un vale cancelado, verifique", "Canje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                if (vale.estatus == "P")
                {
                    XtraMessageBox.Show("EL vale ya ha sido canjeado, verifique", "Canje", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                if (!lv.Items.ContainsKey(txtLetra.Text))
                {
                    AgregarVale(vale.folio.ToString(), vale.empresas.nombre, vale.clientes.nombre, vale.importe, vale.id_empresa.ToString(), vale.id_cliente.ToString());
                    
                }
                    
            }
            catch (Exception e)
            {
                XtraMessageBox.Show("Vale no encontrado, verifique folio: " + e.Message, "Canje Vales", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtLetra.Select();
            }
        }

        private void AgregarVale(string folio, string Empresa, string Cliente, double importe, string idEmpresa, string idCliente)
        {
            ListViewItem item = new ListViewItem(folio) { Name = folio };
            item.SubItems.Add(Empresa);
            item.SubItems.Add(Cliente);
            item.SubItems.Add(importe.ToString());
            item.SubItems.Add(idEmpresa);
            item.SubItems.Add(idCliente);
            item.SubItems.Add(cmbEstacion.EditValue.ToString());
            lv.Items.Add(item);
            txtValesCanjeados.Text = lv.Items.Count.ToString();
            txtTotal.Text = SumaImporteListView().ToString();
        }

        private void txtLetra_KeyDown(object sender, KeyEventArgs e)
        {
            if(txtLetra.Text != string.Empty)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    BuscaVale(txtLetra.Text);
                    txtLetra.Text = string.Empty;
                    txtLetra.Select();
                }
               
            }
        }

        private void btnCanjear_Click(object sender, EventArgs e)
        {
            if(lv.Items.Count > 0)
            {
                foreach(ListViewItem item in lv.Items)
                {
                    int folioVale = Convert.ToInt32(item.Text);
                    vales miVale = Program.Contexto.vales.SingleOrDefault(v => v.folio == folioVale);
                    miVale.estatus = "P";
                    miVale.id_estacion = Convert.ToInt32(item.SubItems[6].Text);
                    miVale.fecha_corte = dtpFechaCorte.DateTime;
                    miVale.fecha_canje = DateTime.Now;
                }
                Program.Contexto.SaveChanges();
                XtraMessageBox.Show("Vales canjeados correctamente");
                Dispose();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtLetra.Text != string.Empty)
            {                
                BuscaVale(txtLetra.Text);
                txtLetra.Text = string.Empty;
                txtLetra.Select();
            }
           
        }

        private void FrmCanjeVales_Load(object sender, EventArgs e)
        {
            var estacion = Program.Contexto.estaciones.ToList();
            cmbEstacion.Properties.DataSource = estacion;
            cmbEstacion.Properties.ValueMember = "id";
            cmbEstacion.Properties.DisplayMember = "nombre";
        }

        private void quitarDeLaListaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lv.SelectedItems)
                lv.Items.Remove(item);
            txtValesCanjeados.Text = lv.Items.Count.ToString();
            txtTotal.Text = SumaImporteListView().ToString();
        }

        public float SumaImporteListView()
        {
            float aux = 0;

            foreach(ListViewItem item in lv.Items)
            {
                aux += Convert.ToSingle(item.SubItems[3].Text);
            }

            return aux;
        }
    }
}