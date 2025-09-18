using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using GestionValesRdz.Services;

namespace GestionValesRdz.Forms
{
    public partial class FrmPrintMonitor : XtraForm
    {
        private static FrmPrintMonitor _instance;
        private Timer _updateTimer;

        public static void ShowMonitor()
        {
            if(_instance == null || _instance.IsDisposed)
            {
                _instance = new FrmPrintMonitor();
            }

            if(!_instance.Visible)
            {
                _instance.Show();
            }
            else
            {
                _instance.BringToFront();
            }
        }

        public static void HideMonitor()
        {
            if(_instance == null && _instance.IsDisposed)
            {
                _instance.Hide();
            }
        }

        public FrmPrintMonitor()
        {
            InitializeComponent();
            InitializeEvents();
            InitializeTimer();
            UpdateStatus();
        }

        private void InitializeEvents()
        {
            PrintingService.Instance.ProgressChanged += PrintingService_Progresschanged;
            PrintingService.Instance.JobCompleted += PrintingService_JobCompleted;
        }

        private void InitializeTimer()
        {
            _updateTimer = new Timer();
            _updateTimer.Interval = 2000;
            _updateTimer.Tick += UpdateTimer_Tick;
            _updateTimer.Start();
        }

        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            UpdateStatus();
        }

        private void UpdateStatus()
        {
            if(InvokeRequired)
            {
                BeginInvoke(new Action(UpdateStatus));
                return;
            }

            int queueSize = PrintingService.Instance.GetQueueSize();
            lblQueueStatus.Text = $"Trabajos en cola: {queueSize}";
            lblLastUpdate.Text = $"Última actualización: {DateTime.Now:HH:mm:ss}";

            if(queueSize == 0 && progressBar.Value == 0)
            {
                lblCurrentJob.Text = "No hay trabajos activos";
                progressBar.Value = 0;
            }
        }

        private void PrintingService_Progresschanged(object sender, PrintProgressEventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<object, PrintProgressEventArgs>(PrintingService_Progresschanged), sender, e);
                return;
            }

            lblCurrentJob.Text = e.Message;
            progressBar.Value = e.Progress;
            lblQueueStatus.Text = $"Trabajos en cola: {e.QueueSize}";
            lblLastUpdate.Text = $"Última actualización: {DateTime.Now:HH:mm:ss}";
        }

        private void PrintingService_JobCompleted(object sender, PrintCompletedEventArgs e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<object, PrintCompletedEventArgs>(PrintingService_JobCompleted), sender, e);
                return;
            }

            if (e.Success)
            {
                lblCurrentJob.Text = $"Completado: {e.Job.JobName}";
                progressBar.Value = 100;
            }
            else
            {
                lblCurrentJob.Text = $"Error: {e.Job.JobName}";
                progressBar.Value = 0;
                // Mostrar notificación de error
                BackColor = Color.LightCoral;
                var timer = new Timer();
                timer.Interval = 3000;
                timer.Tick += (s, args) =>
                {
                    BackColor = SystemColors.Control;
                    ((Timer)s).Dispose();
                };
                timer.Start();
            }

            lblQueueStatus.Text = $"Trabajos en cola: {e.QueueSize}";
            lblLastUpdate.Text = $"Última actualización: {DateTime.Now:HH:mm:ss}";
        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void FrmPrintMonitor_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }
    }
}
