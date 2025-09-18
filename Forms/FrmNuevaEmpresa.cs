using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace GestionValesRdz.Forms
{
    public partial class FrmNuevaEmpresa : XtraForm
    {
        private bool _esNuevEmpresa;
        private int _idEmpresa;

        public FrmNuevaEmpresa()
        {
            InitializeComponent();
            _esNuevEmpresa = true;
        }

        public FrmNuevaEmpresa(int idEmpresa, bool esNuevaEmpresa)
        {
            InitializeComponent();
            _esNuevEmpresa = esNuevaEmpresa;
            _idEmpresa = idEmpresa;
        }

        private void CargaEmpresa()
        {
            empresas empresa = Program.Contexto.empresas.SingleOrDefault(p => p.id == _idEmpresa);
            txtCalle.Text = empresa.calle;
            txtCiudad.Text = empresa.ciudad;
            txtColonia.Text = empresa.colonia;
            txtCP.Text = empresa.cp;
            txtEmail.Text = empresa.email;
            txtMunicipio.Text = empresa.municipio;
            txtNoExt.Text = empresa.numext;
            txtNoInt.Text = empresa.numint;
            txtNombre.Text = empresa.nombre;
            txtObs.Text = empresa.obs;
            txtPais.Text = empresa.pais;
            txtRFC.Text = empresa.rfc;
            txtTelefono.Text = empresa.telefono;
            cmbEstado.Text = empresa.estado;
            cmbEstatus.EditValue = empresa.estatus;
            Text = string.Format("Editar empresa No. {0}", empresa.id);
        }

        private void FrmNuevaEmpresa_Load(object sender, EventArgs e)
        {
            var estatus = new Dictionary<string, string>();
            estatus.Add("A", "ACTIVO");
            estatus.Add("I", "INACTIVO");
            estatus.Add("S", "SUSPENDIDO");
            estatus.Add("B", "BAJA");
            cmbEstatus.Properties.DataSource = estatus;
            cmbEstatus.Properties.DisplayMember = "Value";
            cmbEstatus.Properties.ValueMember = "Key";
            cmbEstatus.ItemIndex = 0;
            cmbEstado.SelectedIndex = 27;
            if (!_esNuevEmpresa)
                CargaEmpresa();
        }

        private void Guarda()
        {
            if (_esNuevEmpresa)
            {
                empresas empresa = new empresas()
                {
                    calle = txtCalle.Text,
                    ciudad = txtCiudad.Text,
                    colonia = txtColonia.Text,
                    cp = txtCP.Text,
                    email = txtEmail.Text,
                    estado = cmbEstado.Text,
                    estatus = cmbEstatus.EditValue.ToString(),
                    fecha_alta = DateTime.Today,
                    municipio = txtMunicipio.Text,
                    nombre = txtNombre.Text,
                    numext = txtNoExt.Text,
                    numint = txtNoInt.Text,
                    obs = txtObs.Text,
                    pais = txtPais.Text,
                    rfc = txtRFC.Text,
                    telefono = txtTelefono.Text,
                    usuario = Program.IdUsuario
                };
                Program.Contexto.empresas.Add(empresa);
            }
            else
            {
                empresas empresa = Program.Contexto.empresas.SingleOrDefault(p => p.id == _idEmpresa);
                empresa.calle = txtCalle.Text;
                empresa.ciudad = txtCiudad.Text;
                empresa.colonia = txtColonia.Text;
                empresa.cp = txtCP.Text;
                empresa.email = txtEmail.Text;
                empresa.estado = cmbEstado.Text;
                empresa.estatus = cmbEstatus.EditValue.ToString();
                empresa.municipio = txtMunicipio.Text;
                empresa.nombre = txtNombre.Text;
                empresa.numext = txtNoExt.Text;
                empresa.numint = txtNoInt.Text;
                empresa.obs = txtObs.Text;
                empresa.pais = txtPais.Text;
                empresa.rfc = txtRFC.Text;
                empresa.telefono = txtTelefono.Text;
            }
            Program.Contexto.SaveChanges();
            Dispose();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (txtNombre.Text == string.Empty)
            {
                XtraMessageBox.Show("El nombre de la empresa es requerido", "Empresas", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtNombre.Select();
                return;
            }
            Guarda();
        }
    }
}