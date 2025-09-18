using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace GestionValesRdz.Forms
{
    public partial class FrmPrincipal : XtraForm
    {
        public FrmPrincipal()
        {
            InitializeComponent();
            barStaticItem1.Caption = string.Format("Usuario: {0}", Program.NombreUsuario);
        }

        private void FrmPrincipal_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void btnNuevoUsuario_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            new FrmNuevoUsuario().ShowDialog();
        }

        private void btnBaseDatos_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            new FrmBaseDatos().ShowDialog();
        }

        private void btnUsuarios_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            foreach (Form form in Application.OpenForms)
            {
                if (form.GetType() == typeof(FrmUsuarios))
                {
                    form.Activate();
                    return;
                }
            }
            var FormUsuarios = new FrmUsuarios() { MdiParent = this };
            FormUsuarios.Show();
        }

        private void btnTemas_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            new FrmTemas().ShowDialog();
        }

        private void btnClientes_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            foreach(Form form in Application.OpenForms)
            {
                if(form.GetType() == typeof(FrmClientes))
                {
                    form.Activate();
                    return;
                }
            }

            var FormCLientes = new FrmClientes() { MdiParent = this };
            FormCLientes.Show();
        }

        private void btnNuevoCliente_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            new FrmNuevoCliente().ShowDialog();
        }

        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            foreach (Form form in Application.OpenForms)
            {
                if (form.GetType() == typeof(FrmLogs))
                {
                    form.Activate();
                    return;
                }
            }
            var FormLogs = new FrmLogs() { MdiParent = this };
            FormLogs.Show();
        }

        private void btnNuevaEmpresa_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            new FrmNuevaEmpresa().ShowDialog();
        }

        private void btnEmpresas_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            foreach (Form form in Application.OpenForms)
            {
                if (form.GetType() == typeof(FrmEmpresas))
                {
                    form.Activate();
                    return;
                }
            }
            var FormEmpresas = new FrmEmpresas() { MdiParent = this };
            FormEmpresas.Show();
        }

        private void btnRegistrarVales_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            new FrmRegistraVales().ShowDialog();
        }

        private void btnGestionVales_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var FormGestion = new FrmGestionVales() { MdiParent = this };
            FormGestion.Show();
        }

        private void btnReimpresión_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            new FrmReimpresion().ShowDialog();
        }

        private void btnCanjeVale_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            new FrmCanjeVales().ShowDialog();
        }

        private void btnValesCancelados_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            new FrmRptCancelados().ShowDialog();
        }

        private void btnValesRecibidos_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            new FrmRptValesRecibidos().ShowDialog();
        }

        private void btnValesPorDia_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            new FrmVentaEspecifica().ShowDialog();
        }

        private void btnValesEspecificos_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            new FrmRptVentaEspecificaPorCliente().ShowDialog();
        }

        private void btnEstaciones_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            var FormGestion = new FrmEstaciones() { MdiParent = this };
            FormGestion.Show();
        }

        private void btnNuevaEstacion_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            new FrmNuevaEstacion().ShowDialog();
        }

        private void btnValesPorEstacion_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            new FrmRptValesPorEstacion().ShowDialog();
        }

        private void btnRptInd_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            new FrmReporteValeInd().ShowDialog();
        }

        private void btnPermisos_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            new FrmPermisos().ShowDialog();
        }

        private void FrmPrincipal_Load(object sender, System.EventArgs e)
        {
            //CATALOGOS
            if (!Auth.TienePermiso("Registrar clientes")) btnNuevoCliente.Enabled = false;
            if (!Auth.TienePermiso("Ver clientes")) btnClientes.Enabled = false;
            if (!Auth.TienePermiso("Registrar empresas")) btnNuevaEmpresa.Enabled = false;
            if (!Auth.TienePermiso("Ver empresas")) btnEmpresas.Enabled = false;
            if (!Auth.TienePermiso("Registrar estaciones")) btnNuevaEstacion.Enabled = false;
            if (!Auth.TienePermiso("Ver estaciones")) btnEstaciones.Enabled = false;
            ////////////////////////////////

            //OPERACIONES
            if (!Auth.TienePermiso("Registrar vales")) btnRegistrarVales.Enabled = false;
            if (!Auth.TienePermiso("Ver vales")) btnGestionVales.Enabled = false;
            if (!Auth.TienePermiso("Canjear vales")) btnCanjeVale.Enabled = false;
            if (!Auth.TienePermiso("Reimprimir vales")) btnReimpresión.Enabled = false;
            ///////////////////////////////////

            //REPORTES
            if (!Auth.TienePermiso("Reporte de vales recibidos")) btnValesRecibidos.Enabled = false;
            if (!Auth.TienePermiso("Reporte de vales por venta")) btnValesPorDia.Enabled = false;
            if (!Auth.TienePermiso("Reporte de ventas especificas por cliente")) btnValesEspecificos.Enabled = false;
            if (!Auth.TienePermiso("Reporte de vales cancelados")) btnValesCancelados.Enabled = false;
            if (!Auth.TienePermiso("Reporte de vales por estación")) btnValesPorEstacion.Enabled = false;
            if (!Auth.TienePermiso("Reporte de vales emitidos por cliente y fecha")) btnRptInd.Enabled = false;
            if (!Auth.TienePermiso("Reporte de vales pendientes por canjear")) btnValesPendientes.Enabled = false;
            ///////////////////////////////////

            //CONFIGURACION
            if (!Auth.TienePermiso("Registrar usuarios")) btnNuevoUsuario.Enabled = false;
            if (!Auth.TienePermiso("Ver usuarios")) btnUsuarios.Enabled = false;
            if (!Auth.TienePermiso("Gestionar permisos")) btnPermisos.Enabled = false;
            if (!Auth.TienePermiso("Base de datos")) btnBaseDatos.Enabled = false;
            if (!Auth.TienePermiso("Temas")) btnTemas.Enabled = false;
            if (!Auth.TienePermiso("Logs")) barButtonItem3.Enabled = false;
        }

        private void btnValesPendientes_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            new FrmRptValePendienteCanje().ShowDialog();
        }
    }
}
