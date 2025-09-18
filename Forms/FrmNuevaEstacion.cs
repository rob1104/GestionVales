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
    public partial class FrmNuevaEstacion : DevExpress.XtraEditors.XtraForm
    {
        private bool _esNuevaEstacion;
        private int _idEstacion;

        public FrmNuevaEstacion()
        {
            InitializeComponent();
            _esNuevaEstacion = true;
        }

        public FrmNuevaEstacion(int idEstacion, bool esNuevaEstacion)
        {
            InitializeComponent();
            _esNuevaEstacion = esNuevaEstacion;
            _idEstacion = idEstacion;
        }

        private void cargaEstacion()
        {
            estaciones estacion = Program.Contexto.estaciones.SingleOrDefault(p => p.id == _idEstacion);
            txtNoEstacion.Text = estacion.numero;
            txtAlias.Text = estacion.nombre;
            txtDireccion.Text = estacion.direccion;
            txtObs.Text = estacion.obs;           
            Text = string.Format("Editar estacion No. {0}", estacion.id);
        }

        private void Guarda()
        {
            if(_esNuevaEstacion)
            {
                estaciones estacion = new estaciones()
                {
                    numero = txtNoEstacion.Text,
                    nombre = txtAlias.Text,
                    direccion = txtDireccion.Text,
                    obs = txtObs.Text,
                    id_usuario = Program.IdUsuario
                };
                Program.Contexto.estaciones.Add(estacion);
            }
            else
            {
                estaciones estacion = Program.Contexto.estaciones.SingleOrDefault(p => p.id == _idEstacion);
                estacion.numero = txtNoEstacion.Text;
                estacion.nombre = txtAlias.Text;
                estacion.direccion = txtDireccion.Text;
                estacion.obs = txtObs.Text;
            }
            Program.Contexto.SaveChanges();
            Dispose();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (txtNoEstacion.Text == string.Empty)
            {
                XtraMessageBox.Show("El numero de la estación es requerido", "Estaciones", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtNoEstacion.Select();
                return;
            }
            Guarda();

        }

        private void FrmNuevaEstacion_Load(object sender, EventArgs e)
        {
            if (!_esNuevaEstacion)
                cargaEstacion();

        }
    }
}