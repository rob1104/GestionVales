using System;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraReports.UI;
using System.Data.Entity.Validation;
using System.Data.Entity.Infrastructure;
using DevExpress.DataAccess.Sql;
using System.Security.Cryptography;
using GestionValesRdz.Servicios;

namespace GestionValesRdz.Forms
{
    public partial class FrmRegistraVales : XtraForm
    {

        private static Random random = new Random();


        public FrmRegistraVales()
        {
            InitializeComponent();

            Height = 519;
            tc.Height = 117;

            /*Carga clientes*/
            var cliente = Program.Contexto.clientes.ToList();
            cmbCliente.Properties.DataSource = cliente;
            cmbCliente.Properties.ValueMember = "id";
            cmbCliente.Properties.DisplayMember = "nombre";
            /*Carga empresas*/
            var empresa = Program.Contexto.empresas.ToList();
            cmbEmpresa.Properties.DataSource = empresa;
            cmbEmpresa.Properties.ValueMember = "id";
            cmbEmpresa.Properties.DisplayMember = "nombre";


            cmbCliente.ItemIndex = 0;

            txtFolioInicial.Text = ObtieneFolioMayor().ToString();

            txtDesde50.Text = ObtieneFolioMayor().ToString();

            txtHasta50.Text = txtDesde50.Text;
            txtDesde100.Text = txtHasta50.Text;
            txtHasta100.Text = txtDesde100.Text;
            txtDesde200.Text = txtHasta100.Text;
            txtHasta200.Text = txtDesde200.Text;
            txtDesde500.Text = txtHasta200.Text;
            txtHasta500.Text = txtDesde500.Text;

            txtCantidad.Focus();


            try
            {
                cmbEmpresa.EditValue = 1;
            }
            catch (Exception)
            {
                //throw;
            }
        }

        private void txtCantidad_EditValueChanged(object sender, EventArgs e)
        {
            CalculaCantidadVales();
        }

        private int ObtieneFolioMayor()
        {
            int i;
            try
            {
                i = Convert.ToInt32(Program.Contexto.vales.Max(v => v.folio));
            }
            catch (Exception)
            {
                i = 0;
            }
            
            return i + 1;
        }

        private void CalculaCantidadVales()
        {
            try
            {
                if(txtFolioInicial.Text != string.Empty)
                {
                    int folio_desde = Convert.ToInt32(txtFolioInicial.Text);
                    txtFolioFinal.Text = (folio_desde + Convert.ToInt32(txtCantidad.Text) - 1).ToString();
                }                
            }
            catch (Exception e)
            {
                XtraMessageBox.Show(e.Message, "Error al calcular la cantidad de vales", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CalculaCantidadVales(int importe)
        {
            try
            {
                switch(importe)
                {
                    case 50:
                        if (txtCantidad50.Text != string.Empty && Convert.ToInt32(txtCantidad50.Text) > 0)
                        {
                            int folio_desde = Convert.ToInt32(txtDesde50.Text);
                            txtHasta50.Text = (folio_desde + Convert.ToInt32(txtCantidad50.Text) - 1).ToString();
                        }
                        else
                            txtHasta50.Text = txtDesde50.Text;
                        break;
                    case 100:
                        if (txtCantidad100.Text != string.Empty && Convert.ToInt32(txtCantidad100.Text) > 0)
                        {
                            int folio_desde = Convert.ToInt32(txtDesde100.Text);
                            txtHasta100.Text = (folio_desde + Convert.ToInt32(txtCantidad100.Text) - 1).ToString();
                        }
                        else
                            txtHasta100.Text = txtDesde100.Text;
                        break;
                    case 200:
                        if (txtCantidad200.Text != string.Empty && Convert.ToInt32(txtCantidad200.Text) > 0)
                        {
                            int folio_desde = Convert.ToInt32(txtDesde200.Text);
                            txtHasta200.Text = (folio_desde + Convert.ToInt32(txtCantidad200.Text) - 1).ToString();
                        }
                        else
                            txtHasta200.Text = txtDesde200.Text;
                        break;
                    case 500:
                        if (txtCantidad500.Text != string.Empty && Convert.ToInt32(txtCantidad500.Text) > 0)
                        {
                            int folio_desde = Convert.ToInt32(txtDesde500.Text);
                            txtHasta500.Text = (folio_desde + Convert.ToInt32(txtCantidad500.Text) - 1).ToString();
                        }
                        else
                            txtHasta500.Text = txtDesde500.Text;
                        break;
                }
                    
                
            }
            catch (Exception e)
            {
                XtraMessageBox.Show(e.Message, "Error al calcular la cantidad de vales", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtFolioInicial_Leave(object sender, EventArgs e)
        {
            CalculaCantidadVales();
        }

        private void txtImporte_Leave(object sender, EventArgs e)
        {
            txtLetra.Text = NumerosALetras.Convertir(txtImporte.Text, true);
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
           
            //***********************************************************************************************************

            DialogResult r = XtraMessageBox.Show(string.Format("¿Seguro que desea generar los vales caputrados?"), "Verificar datos", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (r == DialogResult.Yes)
            {
                try
                {
                    GuardaEnBase();
                    GeneraVenta();
                    if (lv.Items.Count == 0)
                    {
                        XtraMessageBox.Show("Rango de vales incorrecto, especifique un nuevo rango", "Vales", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }

                    var printJob = new PrintJob
                    {
                        JobName = $"Vales del folio {txtFolioInicial.Text}",
                        StartFolio = Convert.ToInt32(txtFolioInicial.Text),
                        TotalVales = GetTotalValesToPrint(),
                        ConnectionString = string.Format(@"XpoProvider=MSSqlServer;data source={0};initial catalog={1};User Id=sa;Password=9753186400;",
                            Properties.Settings.Default.servidor, Properties.Settings.Default.basedatos)
                    };
                    PrintingService.Instance.AddPrintJob(printJob);
                    NotificadorDatos.AnunciarCambio();
                    XtraMessageBox.Show("Los vales se han agregado a la impresora y se imprimirán en segundo plano. Puede cerrar esta ventana y continuar trabajando",
                        "Impresión en Curso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    //Imprime();                
                    btnGuardar.Enabled = false;
                    Close();
                }
                catch (Exception ex)
                {
                    XtraMessageBox.Show($"Error al generar vales: {ex.Message}", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    btnGuardar.Enabled = true;
                }                
            }
        }

        private int GetTotalValesToPrint()
        {
            if (tc.SelectedIndex == 0)
                return Convert.ToInt32(txtCantidad.Text);
            else
                return Convert.ToInt32(txtCantidad50.Text) +
                       Convert.ToInt32(txtCantidad100.Text) +
                       Convert.ToInt32(txtCantidad200.Text) +
                       Convert.ToInt32(txtCantidad500.Text);
        }

        private void GeneraVenta()
        {
            try
            {

                if(lv.Items.Count == 0)
                {
                    XtraMessageBox.Show("No hay vales agregados, verifique", "Vales", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                double importex = 0;
                if (Convert.ToDecimal(txtImporte.Text) == 0)
                {

                    if (txtCantidad50.Text == string.Empty) txtCantidad50.Text = "0";
                    if (txtCantidad100.Text == string.Empty) txtCantidad100.Text = "0";
                    if (txtCantidad200.Text == string.Empty) txtCantidad200.Text = "0";
                    if (txtCantidad500.Text == string.Empty) txtCantidad500.Text = "0";

                    var aux50 = Convert.ToInt32(txtCantidad50.Text) * Convert.ToDouble(txtImporte50.Text);
                    var aux100 = Convert.ToInt32(txtCantidad100.Text) * Convert.ToDouble(txtImporte100.Text);
                    var aux200 = Convert.ToInt32(txtCantidad200.Text) * Convert.ToDouble(txtImporte200.Text);
                    var aux500 = Convert.ToInt32(txtCantidad500.Text) * Convert.ToDouble(txtImporte500.Text);
                    importex = aux50 + aux100 + aux200 + aux500;
                }
                else
                    importex = Math.Round(Convert.ToInt32(txtCantidad.Value) * Convert.ToDouble(txtImporte.Text), 2);

                ventas venta = new ventas()
                {
                    fecha = DateTime.Now.Date,
                    folioInicio = Convert.ToInt32(txtFolioInicial.Text),
                    folioFin = Convert.ToInt32(txtFolioInicial.Text) + lv.Items.Count - 1,
                    denominacion = Convert.ToDouble(txtImporte.Text),
                    cantidad = lv.Items.Count,
                    importe = importex,
                    id_cliente = Convert.ToInt32(cmbCliente.EditValue),
                    id_empresa = Convert.ToInt32(cmbEmpresa.EditValue),
                    id_usuario = Program.IdUsuario
                };
                Program.Contexto.ventas.Add(venta);
                Program.Contexto.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
           
        }

        private void AgregaALista()
        {
            try
            {
                var misVales = Program.Contexto.vales.ToList();
                bool agrega;
                for (int i = Convert.ToInt32(txtFolioInicial.Text); i <= Convert.ToInt32(txtFolioFinal.Text); i++)
                {
                    agrega = true;
                    foreach (var valee in misVales)
                    {
                        if (valee.folio == i)
                        {
                            XtraMessageBox.Show(string.Format("El vale {0} ya ha sido generado anteriormente, verifique", i), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            agrega = false;
                            break;
                        }
                    }
                    if (!lv.Items.ContainsKey(i.ToString()) && agrega)
                        AgregarVale(i.ToString(), cmbEmpresa.Text, cmbCliente.Text, Convert.ToDouble(txtImporte.Text), cmbEmpresa.EditValue.ToString(), cmbCliente.EditValue.ToString());
                }
            }
            catch (Exception)
            {
                throw;
                
            }
           
        }

        private void AgregaMultiples()
        {
            try
            {
                var misVales = Program.Contexto.vales.ToList();
                bool agrega;
                if (txtCantidad50.Text != string.Empty && Convert.ToInt32(txtCantidad50.Text) > 0)
                {                   
                    for (int i = Convert.ToInt32(txtDesde50.Text); i <= Convert.ToInt32(txtHasta50.Text); i++)
                    {
                        agrega = true;
                        foreach (var valee in misVales)
                        {
                            if (valee.folio == i)
                            {
                                XtraMessageBox.Show(string.Format("El vale {0} ya ha sido generado anteriormente, verifique", i), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                agrega = false;
                                break;
                            }
                        }
                        if (!lv.Items.ContainsKey(i.ToString()) && agrega)
                            AgregarVale(i.ToString(), cmbEmpresa.Text, cmbCliente.Text, Convert.ToDouble(txtImporte50.Text), cmbEmpresa.EditValue.ToString(), cmbCliente.EditValue.ToString());
                    }
                }

                if (txtCantidad100.Text != string.Empty && Convert.ToInt32(txtCantidad100.Text) > 0)
                {
                    for (int i = Convert.ToInt32(txtDesde100.Text); i <= Convert.ToInt32(txtHasta100.Text); i++)
                    {
                        agrega = true;
                        foreach (var valee in misVales)
                        {
                            if (valee.folio == i)
                            {
                                XtraMessageBox.Show(string.Format("El vale {0} ya ha sido generado anteriormente, verifique", i), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                agrega = false;
                                break;
                            }
                        }
                        if (!lv.Items.ContainsKey(i.ToString()) && agrega)
                            AgregarVale(i.ToString(), cmbEmpresa.Text, cmbCliente.Text, Convert.ToDouble(txtImporte100.Text), cmbEmpresa.EditValue.ToString(), cmbCliente.EditValue.ToString());
                    }
                }

                if (txtCantidad200.Text != string.Empty && Convert.ToInt32(txtCantidad200.Text) > 0)
                {
                    for (int i = Convert.ToInt32(txtDesde200.Text); i <= Convert.ToInt32(txtHasta200.Text); i++)
                    {
                        agrega = true;
                        foreach (var valee in misVales)
                        {
                            if (valee.folio == i)
                            {
                                XtraMessageBox.Show(string.Format("El vale {0} ya ha sido generado anteriormente, verifique", i), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                agrega = false;
                                break;
                            }
                        }
                        if (!lv.Items.ContainsKey(i.ToString()) && agrega)
                            AgregarVale(i.ToString(), cmbEmpresa.Text, cmbCliente.Text, Convert.ToDouble(txtImporte200.Text), cmbEmpresa.EditValue.ToString(), cmbCliente.EditValue.ToString());
                    }
                }

                if (txtCantidad500.Text != string.Empty && Convert.ToInt32(txtCantidad500.Text) > 0)
                {
                    for (int i = Convert.ToInt32(txtDesde500.Text); i <= Convert.ToInt32(txtHasta500.Text); i++)
                    {
                        agrega = true;
                        foreach (var valee in misVales)
                        {
                            if (valee.folio == i)
                            {
                                XtraMessageBox.Show(string.Format("El vale {0} ya ha sido generado anteriormente, verifique", i), "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                agrega = false;
                                break;
                            }
                        }
                        if (!lv.Items.ContainsKey(i.ToString()) && agrega)
                            AgregarVale(i.ToString(), cmbEmpresa.Text, cmbCliente.Text, Convert.ToDouble(txtImporte500.Text), cmbEmpresa.EditValue.ToString(), cmbCliente.EditValue.ToString());
                    }
                }
            }
            catch (Exception e)
            {
                XtraMessageBox.Show(e.Message, "Error al agregar vales, verifique.", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        private void AgregarVale(string folio, string Empresa, string Cliente, double importe,string idEmpresa,string idCliente)
        {
            ListViewItem item = new ListViewItem(folio) { Name = folio };
            item.SubItems.Add(Empresa);
            item.SubItems.Add(Cliente);
            item.SubItems.Add(importe.ToString());
            item.SubItems.Add(idEmpresa);
            item.SubItems.Add(idCliente);
            item.SubItems.Add(StringAleatorio(12));
            lv.Items.Add(item);
        }

        private string StringAleatorio(int longitudnuevacadena)
        {

            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            string nuevacadena = new string(Enumerable.Repeat(chars, longitudnuevacadena)
              .Select(s => s[random.Next(s.Length)]).ToArray());

            string fecha = DateTime.Today.ToString("yyyyMMdd");
            return fecha + nuevacadena;
        }

        private void GuardaEnBase()
        {
            try
            {
                foreach (ListViewItem vale in lv.Items)
                {
                    Guid g = Guid.NewGuid();
                    vales miVale = new vales()
                    {
                        folio = Convert.ToInt32(vale.Text),
                        importe = Convert.ToDouble(vale.SubItems[3].Text),
                        importe_letra = NumerosALetras.Convertir(vale.SubItems[3].Text, true),
                        fecha_emision = DateTime.Now,
                        id_cliente = Convert.ToInt32(cmbCliente.EditValue),
                        id_empresa = Convert.ToInt32(cmbEmpresa.EditValue),
                        id_usuario = Program.IdUsuario,
                        estatus = "A",
                        token = g.ToString(),
                        codigo = vale.SubItems[6].Text
                    };
                    Program.Contexto.vales.Add(miVale);
                }
                Program.Contexto.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            } 
            catch(DbUpdateException)
            {
                MessageBox.Show("Folio duplicado, verifique");
            } 
        }

        private void Imprime()
        {

            int cant = 0;

            if (tc.SelectedIndex == 0)
                cant = Convert.ToInt32(txtCantidad.Text);
            else
                cant = Convert.ToInt32(txtCantidad50.Text) + Convert.ToInt32(txtCantidad100.Text) + Convert.ToInt32(txtCantidad200.Text) + Convert.ToInt32(txtCantidad500.Text); 

            var vale = new rptVale2();
            SqlDataSource ds = vale.DataSource as SqlDataSource;
            ds.Connection.ConnectionString = string.Format(@"XpoProvider=MSSqlServer;data source={0};initial catalog={1};User Id=sa;Password=9753186400;", Properties.Settings.Default.servidor, Properties.Settings.Default.basedatos);
            vale.Parameters["folio"].Value = txtFolioInicial.Text;
            vale.Parameters["folio"].Visible = false;
            vale.Parameters["folio2"].Value = Convert.ToInt32(txtFolioInicial.Text) + cant - 1;
            vale.Parameters["folio2"].Visible = false;
            vale.ShowPrintMarginsWarning = false;
            ReportPrintTool tool = new ReportPrintTool(vale);
            tool.Print();
            tool.Dispose();
            vale.Dispose();          
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            if(lv.Items.Count > 0)
            {
                int? max = (from m in lv.Items.Cast<ListViewItem>()
                            select int.Parse(m.Text)).Max();
                txtFolioInicial.Text = (max + 1).ToString();
                
            }
            txtFolioInicial.Select();
            txtCantidad.Value = 1;
            txtCantidad.Select();
            cmbCliente.ItemIndex = 0;
            cmbEmpresa.ItemIndex = 0;
            txtImporte.Text = "0";
            lv.Items.Clear();
            btnGuardar.Enabled = true;
            txtCantidad.Select();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            //Validaciones
            if (txtFolioFinal.Text == string.Empty || Convert.ToInt32(txtCantidad.Text) <= 0)
            {
                XtraMessageBox.Show("Rango de vales incorrecto, especifique un nuevo rango", "Vales", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (cmbCliente.EditValue == null)
            {
                XtraMessageBox.Show("Especifique un cliente", "Vales", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cmbCliente.Select();
                return;
            }
            if (cmbEmpresa.EditValue == null)
            {
                XtraMessageBox.Show("Especifique una empresa", "Vales", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cmbCliente.Select();
                return;
            }
            if (Convert.ToDouble(txtImporte.Text) == 0 || txtImporte.Text == string.Empty)
            {
                XtraMessageBox.Show("Especifique el importe del vale", "Vales", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cmbCliente.Select();
                return;
            }

            AgregaALista();
        }

        private void tc_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(tc.SelectedIndex == 1)
            {
                Height = 619;
                tc.Height = 217;
                txtCantidad50.Focus();
            }
            else if(tc.SelectedIndex == 0)
            {
                Height = 519;
                tc.Height = 117;
                txtCantidad.Focus();
            }
        }

        private void btnAgregaMultiple_Click(object sender, EventArgs e)
        {
            if (cmbCliente.EditValue == null)
            {
                XtraMessageBox.Show("Especifique un cliente", "Vales", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cmbCliente.Select();
                return;
            }
            if (cmbEmpresa.EditValue == null)
            {
                XtraMessageBox.Show("Especifique una empresa", "Vales", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cmbCliente.Select();
                return;
            }
            AgregaMultiples();
        }

        private void txtCantidad50_EditValueChanged(object sender, EventArgs e)
        {
            CalculaCantidadVales(50);
        }

        private void txtCantidad100_EditValueChanged(object sender, EventArgs e)
        {
            CalculaCantidadVales(100);
            
        }

        private void txtCantidad200_EditValueChanged(object sender, EventArgs e)
        {
            CalculaCantidadVales(200);
        }

        private void txtCantidad500_EditValueChanged(object sender, EventArgs e)
        {
            CalculaCantidadVales(500);
        }

        private void txtCantidad50_Leave(object sender, EventArgs e)
        {
            if (txtCantidad50.Text == string.Empty) txtCantidad50.Text = "0";

            if (Convert.ToInt32(txtCantidad50.Text) == 0)
            {
                txtHasta50.Text = txtDesde50.Text;
                txtDesde100.Text = txtHasta50.Text;
            }
            else
                txtDesde100.Text = (Convert.ToInt32(txtHasta50.Text) + 1).ToString();
        }

        private void txtCantidad100_Leave(object sender, EventArgs e)
        {
            if (txtCantidad100.Text == string.Empty) txtCantidad100.Text = "0";

            if (Convert.ToInt32(txtCantidad100.Text) == 0)
            {
                txtHasta100.Text = txtDesde100.Text;
                txtDesde200.Text = txtHasta100.Text;
            }
            else
                txtDesde200.Text = (Convert.ToInt32(txtHasta100.Text) + 1).ToString();
        }        

        private void txtCantidad200_Leave(object sender, EventArgs e)
        {
            if (txtCantidad200.Text == string.Empty) txtCantidad200.Text = "0";

            if (Convert.ToInt32(txtCantidad200.Text) == 0)
            {
                txtHasta200.Text = txtDesde200.Text;
                txtDesde500.Text = txtHasta200.Text;
            }
            else
                txtDesde500.Text = (Convert.ToInt32(txtHasta200.Text) + 1).ToString();
        }

        private void txtCantidad500_Leave(object sender, EventArgs e)
        {
            if (txtCantidad500.Text == string.Empty) txtCantidad500.Text = "0";

            if (Convert.ToInt32(txtCantidad500.Text) == 0)
            {
                txtHasta500.Text = txtDesde500.Text;
                //txtDesde200.Text = txtHasta100.Text;
            }
            //else
                //txtDesde200.Text = (Convert.ToInt32(txtHasta100.Text) + 1).ToString();
        }

        private void txtCantidad_Leave(object sender, EventArgs e)
        {
            CalculaCantidadVales();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }
    }
}