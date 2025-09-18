using System;
using DevExpress.XtraEditors;
using DevExpress.Skins;
using DevExpress.LookAndFeel;

namespace GestionValesRdz.Forms
{
    public partial class FrmTemas : XtraForm
    {
        public FrmTemas()
        {
            InitializeComponent();
        }

        private void FrmTemas_Load(object sender, EventArgs e)
        {
            cmbTemas.Properties.Items.Clear();
            foreach(SkinContainer tema in SkinManager.Default.Skins)
            {
                cmbTemas.Properties.Items.Add(tema.SkinName);
            }
            cmbTemas.Properties.Sorted = true;
            cmbTemas.Text = Properties.Settings.Default.tema;
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.tema = cmbTemas.Text;
            Properties.Settings.Default.Save();
            Dispose();
        }

        private void cmbTemas_SelectedIndexChanged(object sender, EventArgs e)
        {
            UserLookAndFeel.Default.SkinName = cmbTemas.Text;
        }

    }
}