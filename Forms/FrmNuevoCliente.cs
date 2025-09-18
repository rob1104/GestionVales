using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace GestionValesRdz.Forms
{
    public partial class FrmNuevoCliente : XtraForm
    {
        private bool _esNuevoCliente;
        private int _idCliente;

        public FrmNuevoCliente()
        {
            InitializeComponent();
            _esNuevoCliente = true;
        }

        public FrmNuevoCliente(int idCliente, bool esNuevoCliente)
        {
            InitializeComponent();
            _idCliente = idCliente;
            _esNuevoCliente = esNuevoCliente;
        }

        private void FrmNuevoCliente_Load(object sender, EventArgs e)
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
            if (!_esNuevoCliente)
                CargaCliente();
        }

        private void CargaCliente()
        {
            clientes cliente = Program.Contexto.clientes.SingleOrDefault(p => p.id == _idCliente);
            txtCalle.Text = cliente.calle;
            txtCiudad.Text = cliente.ciudad;
            txtColonia.Text = cliente.colonia;
            txtCP.Text = cliente.cp;
            txtEmail.Text = cliente.email;
            txtMunicipio.Text = cliente.municipio;
            txtNoExt.Text = cliente.numext;
            txtNoInt.Text = cliente.numint;
            txtNombre.Text = cliente.nombre;
            txtObs.Text = cliente.obs;
            txtPais.Text = cliente.pais;
            txtRFC.Text = cliente.rfc;
            txtTelefono.Text = cliente.telefono;
            cmbEstado.Text = cliente.estado;
            cmbEstatus.EditValue = cliente.estatus;
            Text = string.Format("Editar cliente No. {0}", cliente.id);
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if(txtNombre.Text == string.Empty)
            {
                XtraMessageBox.Show("El nombre del cliente es requerido", "Clientes", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtNombre.Select();
                return;
            }
            Guarda();
        }

        private void Guarda()
        {
            if(_esNuevoCliente)
            {
                clientes cliente = new clientes()
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
                Program.Contexto.clientes.Add(cliente);
            }
            else
            {
                clientes cliente = Program.Contexto.clientes.SingleOrDefault(p => p.id == _idCliente);
                cliente.calle = txtCalle.Text;
                cliente.ciudad = txtCiudad.Text;
                cliente.colonia = txtColonia.Text;
                cliente.cp = txtCP.Text;
                cliente.email = txtEmail.Text;
                cliente.estado = cmbEstado.Text;
                cliente.estatus = cmbEstatus.EditValue.ToString();
                cliente.municipio = txtMunicipio.Text;
                cliente.nombre = txtNombre.Text;
                cliente.numext = txtNoExt.Text;
                cliente.numint = txtNoInt.Text;
                cliente.obs = txtObs.Text;
                cliente.pais = txtPais.Text;
                cliente.rfc = txtRFC.Text;
                cliente.telefono = txtTelefono.Text;
            }
            Program.Contexto.SaveChanges();
            Dispose();
        }
    }
}