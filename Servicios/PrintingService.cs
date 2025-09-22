using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using DevExpress.DataAccess.Sql;
using DevExpress.XtraReports.UI;
using GestionValesRdz.Old;

namespace GestionValesRdz.Servicios
{
    // El servicio ahora solo es un contenedor para la cola de impresión.
    public sealed class PrintingService
    {
        private static readonly Lazy<PrintingService> _instance = new Lazy<PrintingService>(() => new PrintingService());
        private readonly ConcurrentQueue<PrintJob> _printQueue = new ConcurrentQueue<PrintJob>();
        public static PrintingService Instance => _instance.Value;
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        private PrintingService() { }

        public void Start()
        {
            Task.Run(async () =>
            {
                while (!_cancellationTokenSource.IsCancellationRequested)
                {
                    if(_printQueue.TryDequeue(out PrintJob job))
                    {
                        ProcessPrintJob(job);
                    }
                    else
                    {
                        await Task.Delay(2000);
                    }
                }
            });
        }

        private void ProcessPrintJob(PrintJob job)
        {
            // Obtenemos los datos necesarios para el lote.
            int totalBatches = (int)Math.Ceiling((double)job.TotalVales / 50); // Lotes de 50

            for (int batch = 0; batch < totalBatches; batch++)
            {
                int startFolio = job.StartFolio + (batch * 50);
                int endFolio = Math.Min(startFolio + 50 - 1, job.StartFolio + job.TotalVales - 1);

                string connectionString = AyudanteDeConexion.CrearContexto().Database.Connection.ConnectionString;

                // La lógica que tenías en FrmPrincipal ahora está aquí.
                if (Properties.Settings.Default.v1)
                {
                    // Impresión por rango para rptVale2
                    using (var vale = new rptVale2())
                    {
                        if (vale.DataSource is SqlDataSource ds) ds.Connection.ConnectionString = connectionString;
                        vale.Parameters["folio"].Value = startFolio;
                        vale.Parameters["folio2"].Value = endFolio;
                        vale.ShowPrintMarginsWarning = false;
                        using (var tool = new ReportPrintTool(vale)) tool.Print();
                    }
                }
                else
                {
                    // Impresión individual y pausada para rptVale4 (matriz de puntos)
                    for (int folioActual = startFolio; folioActual <= endFolio; folioActual++)
                    {
                        using (var vale = new rptVale4())
                        {
                            if (vale.DataSource is SqlDataSource ds) ds.Connection.ConnectionString = connectionString;
                            vale.Parameters["folio"].Value = folioActual;
                            vale.ShowPrintMarginsWarning = false;
                            using (var tool = new ReportPrintTool(vale)) tool.Print();
                        }
                        // La pausa sigue siendo una buena idea para no saturar el hardware.
                        Thread.Sleep(500);
                    }
                }
            }
        }

        public void ShutDown()
        {
            _cancellationTokenSource.Cancel();
        }

        // Agrega un trabajo a la cola.
        public void AddPrintJob(PrintJob job)
        {
            _printQueue.Enqueue(job);
        }

        // Intenta obtener el siguiente trabajo de la cola sin quitarlo.
        public bool TryPeekNextJob(out PrintJob job)
        {
            return _printQueue.TryPeek(out job);
        }

        // Quita el trabajo actual de la cola.
        public bool TryDequeueJob(out PrintJob job)
        {
            return _printQueue.TryDequeue(out job);
        }
    }

    // La clase PrintJob necesita un campo más para llevar el conteo.
    public class PrintJob
    {
        public string JobName { get; set; }
        public int StartFolio { get; set; }
        public int TotalVales { get; set; }
        public string ConnectionString { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // --- CAMPO NUEVO ---
        // Para saber cuántos vales de este trabajo ya se han impreso.
        public int ValesImpresos { get; set; } = 0;
    }
}