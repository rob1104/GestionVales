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

            // 1. Validamos que la lista no esté vacía.
            if (lv.Items.Count == 0)
            {
                XtraMessageBox.Show("No hay vales en la lista para registrar e imprimir.", "Lista Vacía", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var r = XtraMessageBox.Show("Se guardarán los vales en la base de datos y se enviarán todos los lotes de la lista a la cola de impresión.\n\n¿Desea continuar?",
                "Confirmar Proceso", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (r != DialogResult.Yes)
            {
                return;
            }

            try
            {
                // --- INICIA LA NUEVA LÓGICA DE IMPRESIÓN ---

                // 2. Primero, guardamos los vales en la base de datos.
                // El servicio de impresión los leerá desde ahí.
                GuardaEnBase();
                

                // 3. Extraemos todos los números de folio del ListView y los ordenamos.
                var foliosAImprimir = lv.Items.Cast<ListViewItem>()
                                           .Select(item => int.Parse(item.Text))
                                           .OrderBy(folio => folio)
                                           .ToList();

                if (!foliosAImprimir.Any()) return;

                GeneraVenta();

                // 4. Agrupamos los folios en rangos consecutivos para una impresión eficiente.
                var rangosDeImpresion = new List<PrintJob>();

                // Empezamos el primer rango con el primer folio de la lista.
                var rangoActual = new PrintJob
                {
                    StartFolio = foliosAImprimir[0],
                    TotalVales = 1,
                    ConnectionString = AyudanteDeConexion.CrearContexto().Database.Connection.ConnectionString
                };

                // Recorremos la lista a partir del segundo folio para encontrar los rangos.
                for (int i = 1; i < foliosAImprimir.Count; i++)
                {
                    // Si el folio actual es exactamente el siguiente al último del rango...
                    if (foliosAImprimir[i] == rangoActual.StartFolio + rangoActual.TotalVales)
                    {
                        // ...simplemente extendemos el rango actual.
                        rangoActual.TotalVales++;
                    }
                    else
                    {
                        // Si hay un salto, el rango anterior ha terminado. Lo guardamos.
                        rangoActual.JobName = $"Vales Folio {rangoActual.StartFolio} al {rangoActual.StartFolio + rangoActual.TotalVales - 1}";
                        rangosDeImpresion.Add(rangoActual);

                        // Y empezamos un nuevo rango con el folio actual.
                        rangoActual = new PrintJob
                        {
                            StartFolio = foliosAImprimir[i],
                            TotalVales = 1,
                            ConnectionString = rangoActual.ConnectionString // Reutilizamos la connection string
                        };
                    }
                }

                // ¡Importante! Guardamos el último rango que se estaba procesando.
                rangoActual.JobName = $"Vales Folio {rangoActual.StartFolio} al {rangoActual.StartFolio + rangoActual.TotalVales - 1}";
                rangosDeImpresion.Add(rangoActual);

                // 5. Añadimos todos los rangos encontrados a la cola de impresión.
                foreach (var job in rangosDeImpresion)
                {
                    PrintingService.Instance.AddPrintJob(job);
                }

                // --- FIN DE LA NUEVA LÓGICA ---

                // 6. Notificamos al usuario y limpiamos la pantalla para el siguiente lote.
                NotificadorDatos.AnunciarCambio();

                //XtraMessageBox.Show($"{rangosDeImpresion.Count} lote(s) con un total de {foliosAImprimir.Count} vales se han guardado y enviado a la cola de impresión.",
                  //  "Proceso Exitoso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                lv.Items.Clear();
                txtFolioInicial.Text = ObtieneFolioMayor().ToString();
                txtFolioFinal.Text = "";
                txtCantidad.Text = "0";
                txtImporte.Text = "0";
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show($"Ocurrió un error al procesar la lista: {ex.Message}", "Error Crítico", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            // Si no hay items en la lista, no hacemos nada.
            if (lv.Items.Count == 0)
            {
                return;
            }

            try
            {
                // --- 1. AGRUPAR VALES POR DENOMINACIÓN ---
                // Usamos LINQ para agrupar todos los items del ListView por su monto (denominación).
                // Asumimos que el monto está en la 4ª columna (índice 3).
                var gruposPorDenominacion = lv.Items.Cast<ListViewItem>()
                    .GroupBy(item => decimal.Parse(item.SubItems[3].Text, System.Globalization.NumberStyles.Currency));

                // Creamos una lista para guardar los registros de venta que vamos a generar.
                var nuevasVentas = new List<ventas>();

                // --- 2. PROCESAR CADA GRUPO ---
                // Recorremos cada grupo de vales (ej. todos los de $100, luego todos los de $500, etc.).
                foreach (var grupo in gruposPorDenominacion)
                {
                    decimal denominacionActual = grupo.Key;

                    // Extraemos los folios de todos los vales que pertenecen a este grupo.
                    var foliosDelGrupo = grupo.Select(item => int.Parse(item.Text)).OrderBy(folio => folio).ToList();

                    if (!foliosDelGrupo.Any()) continue; // Si el grupo está vacío, lo ignoramos.

                    // Calculamos los datos específicos para ESTA venta.
                    int cantidad = foliosDelGrupo.Count;
                    decimal importeTotal = cantidad * denominacionActual;
                    int folioInicio = foliosDelGrupo.First();
                    int folioFin = foliosDelGrupo.Last();

                    // Creamos el objeto 'venta' para esta denominación.
                    var venta = new ventas()
                    {
                        fecha = DateTime.Now.Date,
                        folioInicio = folioInicio,
                        folioFin = folioFin,
                        denominacion = (double)denominacionActual, // <-- ¡Ahora se guarda la denominación correcta!
                        cantidad = cantidad,
                        importe = (double)importeTotal,
                        id_cliente = Convert.ToInt32(cmbCliente.EditValue),
                        id_empresa = Convert.ToInt32(cmbEmpresa.EditValue),
                        id_usuario = Auth.Id
                    };

                    nuevasVentas.Add(venta); // Añadimos la nueva venta a nuestra lista.
                }

                // --- 3. GUARDAR TODAS LAS VENTAS ---
                // Si se generó al menos una venta, la guardamos en la base de datos.
                if (nuevasVentas.Any())
                {
                    using (var contexto = AyudanteDeConexion.CrearContexto())
                    {
                        // Usamos AddRange para guardar todos los registros de venta en una sola transacción.
                        contexto.ventas.AddRange(nuevasVentas);
                        contexto.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                // Lanzamos la excepción para que el método que lo llamó (btnGuardar_Click) la capture.
                throw new Exception("Ocurrió un error al generar los registros de venta.", ex);
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
            // Si no hay nada en la lista, no hay nada que guardar.
            if (lv.Items.Count == 0)
            {
                return;
            }

            // 1. Creamos una lista para almacenar los vales que vamos a crear desde el ListView.
            var listaNuevosVales = new List<vales>();
            DateTime fecha = DateTime.Now;
            int idUsuario = Auth.Id;

            // 2. Recorremos cada item que el usuario agregó a la lista (ListView).
            foreach (ListViewItem item in lv.Items)
            {
                // Extraemos los datos de cada columna del item.
                // Asegúrate de que el orden de los SubItems sea el correcto.
                int folio = Convert.ToInt32(item.SubItems[0].Text);
                double monto = double.Parse(item.SubItems[3].Text, System.Globalization.NumberStyles.Currency); // Leemos el monto
                int idEmpresa = Convert.ToInt32(item.SubItems[4].Text);
                int idCliente = Convert.ToInt32(item.SubItems[5].Text);
                string codigo = item.SubItems[6].Text;

                // Creamos el objeto 'vale' con los datos de la fila actual.
                var nuevoVale = new vales
                {
                    folio = folio,
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
            }

            // 3. Guardamos TODOS los vales de la lista en la base de datos en UNA SOLA TRANSACCIÓN.
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
            // Validaciones de UI antes de llamar a la lógica principal
            if (cmbEmpresa.EditValue == null || cmbCliente.EditValue == null || string.IsNullOrEmpty(txtImporte.Text))
            {
                XtraMessageBox.Show("Debe seleccionar una empresa, un cliente y especificar un importe.", "Datos Incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Llama al método que hace el trabajo pesado de validar y agregar.
            AgregaALista();

            // --- ¡LA MEJORA CLAVE PARA EL USUARIO! ---
            // Una vez agregado un lote, preparamos la pantalla para el siguiente.
            if (int.TryParse(txtFolioFinal.Text, out int ultimoFolioAgregado))
            {
                // 1. El nuevo folio inicial es el siguiente al último folio final.
                txtFolioInicial.Text = (ultimoFolioAgregado + 1).ToString();

                // 2. Limpiamos el folio final y ponemos el foco ahí.
                txtFolioFinal.Text = "";
                txtFolioFinal.Focus();
            }
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