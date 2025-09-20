using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraReports.UI;
using System.Data.Entity.Validation;
using System.Data.Entity.Infrastructure;
using DevExpress.DataAccess.Sql;
using System.Security.Cryptography;
using GestionValesRdz.Servicios;
using DevExpress.DataProcessing;

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
                    //XtraMessageBox.Show("Los vales se han agregado a la impresora y se imprimirán en segundo plano. Puede cerrar esta ventana y continuar trabajando",
                      //  "Impresión en Curso", MessageBoxButtons.OK, MessageBoxIcon.Information);

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
                int folioInicial = Convert.ToInt32(txtFolioInicial.Text);
                int folioFinal = Convert.ToInt32(txtFolioFinal.Text);

                // 1. Preguntamos a la base de datos SÓLO por los folios que nos interesan y que ya existen.
                // Es una consulta súper rápida y eficiente.
                using (var contexto = AyudanteDeConexion.CrearContexto())
                {
                    var foliosExistentes = contexto.vales
                        .Where(v => v.folio >= folioInicial && v.folio <= folioFinal)
                        .Select(v => v.folio) // Solo nos interesa el número de folio, no el objeto completo
                        .ToHashSet(); // Usamos un HashSet para búsquedas en memoria casi instantáneas.

                    // 2. Si se encontraron folios existentes, notificamos al usuario DE UNA SOLA VEZ.
                    if (foliosExistentes.Any())
                    {
                        // Unimos los números de folio en un solo string para un mensaje claro.
                        string foliosRepetidos = string.Join(", ", foliosExistentes);
                        XtraMessageBox.Show($"Los siguientes folios ya existen y no se agregarán:\n{foliosRepetidos}",
                                            "Folios Duplicados", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                    // 3. Optimizamos la actualización del ListView para que sea más rápida.
                    lv.BeginUpdate(); // Detiene el redibujado del control

                    var nuevosItems = new List<ListViewItem>();

                    // 4. Recorremos el rango deseado y agregamos solo los que NO existen.
                    for (int i = folioInicial; i <= folioFinal; i++)
                    {
                        // La comprobación 'foliosExistentes.Contains(i)' es extremadamente rápida.
                        if (!foliosExistentes.Contains(i) && !lv.Items.ContainsKey(i.ToString()))
                        {
                            // Creamos el item pero no lo agregamos directamente al ListView todavía
                            ListViewItem item = new ListViewItem(i.ToString()) { Name = i.ToString() };
                            item.SubItems.Add(cmbEmpresa.Text);
                            item.SubItems.Add(cmbCliente.Text);
                            item.SubItems.Add(Convert.ToDouble(txtImporte.Text).ToString("C")); // Formato de moneda
                            item.SubItems.Add(cmbEmpresa.EditValue.ToString());
                            item.SubItems.Add(cmbCliente.EditValue.ToString());
                            item.SubItems.Add(StringAleatorio(12)); // Asumo que tienes este método
                            nuevosItems.Add(item);
                        }
                    }

                    // Agregamos todos los nuevos items al ListView de una sola vez.
                    lv.Items.AddRange(nuevosItems.ToArray());

                    lv.EndUpdate(); // Reactiva el redibujado del control, mostrando todo al instante.
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show($"Ocurrió un error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            lv.Items.Clear();

            // 1. Creamos una lista para almacenar todos los nuevos vales en memoria.
            var listaNuevosVales = new List<vales>();
            int folioActual = Convert.ToInt32(txtFolioInicial.Text);

            // Datos comunes para todos los vales que se van a crear.
            int idEmpresa = Convert.ToInt32(cmbEmpresa.EditValue);
            int idCliente = Convert.ToInt32(cmbCliente.EditValue);
            DateTime fecha = DateTime.Now;
            int idUsuario = Auth.Id;

            lv.BeginUpdate(); // Pausamos el redibujado del ListView para máxima velocidad.

            if (tc.SelectedIndex == 0) // Pestaña "Por Cantidad"
            {
                int cantidad = Convert.ToInt32(txtCantidad.Text);
                double monto = Convert.ToDouble(txtImporte.Text);

                for (int i = 0; i < cantidad; i++)
                {                    
                    var nuevoVale = new vales
                    {
                        folio = folioActual,
                        codigo = StringAleatorio(12),
                        importe = monto,
                        id_empresa = idEmpresa,
                        id_cliente = idCliente,
                        fecha_emision = fecha,
                        estatus = "A",
                        id_usuario = idUsuario,
                        importe_letra = NumerosALetras.Convertir(monto.ToString(), true),
                        token = Guid.NewGuid().ToString()
                };
                    listaNuevosVales.Add(nuevoVale);
                    // Preparamos la vista previa
                    var item = new ListViewItem(folioActual.ToString());
                    item.SubItems.Add(monto.ToString("C"));
                    lv.Items.Add(item);
                    folioActual++; // Incrementamos el folio para el siguiente vale.
                }
            }
            else // Pestaña "Por Denominación"
            {
                // Usamos un diccionario para hacer el código más limpio y escalable.
                // La clave es el monto, el valor es el control TextBox.
                var denominaciones = new Dictionary<double, TextEdit>
                {
                    { 50, txtCantidad50 },
                    { 100, txtCantidad100 },
                    { 200, txtCantidad200 },
                    { 500, txtCantidad500 }
                };

                // Recorremos cada par denominación/cantidad.
                foreach (var par in denominaciones)
                {
                    double monto = par.Key;
                    TextEdit txtCantidadDenominacion = par.Value;

                    // Si el campo de cantidad no está vacío y es un número válido...
                    if (int.TryParse(txtCantidadDenominacion.Text, out int cantidad) && cantidad > 0)
                    {
                        // ...creamos la cantidad de vales especificada para esa denominación.
                        for (int i = 0; i < cantidad; i++)
                        {
                            var nuevoVale = new vales
                            {
                                folio = folioActual,
                                codigo = StringAleatorio(12),
                                importe = monto,
                                id_empresa = idEmpresa,
                                id_cliente = idCliente,
                                fecha_emision = fecha,
                                estatus = "A",
                                id_usuario = idUsuario,
                                importe_letra = NumerosALetras.Convertir(monto.ToString(), true),
                                token = Guid.NewGuid().ToString()
                            };
                            listaNuevosVales.Add(nuevoVale);

                            // Preparamos la vista previa
                            var item = new ListViewItem(folioActual.ToString());
                            item.SubItems.Add(monto.ToString("C"));
                            lv.Items.Add(item);

                            folioActual++; // Incrementamos el folio para el siguiente vale.
                        }
                    }
                }
            }

            lv.EndUpdate(); // Reactivamos el redibujado, mostrando todos los items de golpe.

            // 2. Ahora, guardamos TODOS los vales en la base de datos en UNA SOLA TRANSACCIÓN.
            if (listaNuevosVales.Any())
            {
                using (var contexto = AyudanteDeConexion.CrearContexto())
                {
                    try
                    {
                        contexto.Configuration.AutoDetectChangesEnabled = false;
                        contexto.vales.AddRange(listaNuevosVales);
                        contexto.SaveChanges();
                    }
                    finally
                    {
                        contexto.Configuration.AutoDetectChangesEnabled = true;
                    }
                }
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

        private void FrmRegistraVales_Load(object sender, EventArgs e)
        {
            if(!Properties.Settings.Default.v1)
            {
                txtFolioInicial.ReadOnly = false;
                tabPage2.Parent = null;

            }
        }
    }
}