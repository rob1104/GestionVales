using System;
using System.Collections.Concurrent;

namespace GestionValesRdz.Servicios
{
    // El servicio ahora solo es un contenedor para la cola de impresión.
    public sealed class PrintingService
    {
        private static readonly Lazy<PrintingService> _instance = new Lazy<PrintingService>(() => new PrintingService());
        private readonly ConcurrentQueue<PrintJob> _printQueue = new ConcurrentQueue<PrintJob>();

        public static PrintingService Instance => _instance.Value;

        private PrintingService() { }

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