
using GestionValesRdz.Services;

namespace GestionValesRdz.Forms
{
    partial class FrmPrintMonitor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                _updateTimer?.Dispose();
                PrintingService.Instance.ProgressChanged -= PrintingService_Progresschanged;
                PrintingService.Instance.JobCompleted -= PrintingService_JobCompleted;
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblCurrentJob = new DevExpress.XtraEditors.LabelControl();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.lblQueueStatus = new DevExpress.XtraEditors.LabelControl();
            this.lblLastUpdate = new DevExpress.XtraEditors.LabelControl();
            this.btnClose = new DevExpress.XtraEditors.SimpleButton();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblCurrentJob
            // 
            this.lblCurrentJob.Location = new System.Drawing.Point(246, 4);
            this.lblCurrentJob.Name = "lblCurrentJob";
            this.lblCurrentJob.Size = new System.Drawing.Size(114, 13);
            this.lblCurrentJob.TabIndex = 0;
            this.lblCurrentJob.Text = "No hay trabajos activos";
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(12, 31);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(360, 23);
            this.progressBar.TabIndex = 1;
            // 
            // lblQueueStatus
            // 
            this.lblQueueStatus.Location = new System.Drawing.Point(12, 60);
            this.lblQueueStatus.Name = "lblQueueStatus";
            this.lblQueueStatus.Size = new System.Drawing.Size(97, 13);
            this.lblQueueStatus.TabIndex = 2;
            this.lblQueueStatus.Text = "Cola de impresión: 0";
            // 
            // lblLastUpdate
            // 
            this.lblLastUpdate.Location = new System.Drawing.Point(12, 79);
            this.lblLastUpdate.Name = "lblLastUpdate";
            this.lblLastUpdate.Size = new System.Drawing.Size(104, 13);
            this.lblLastUpdate.TabIndex = 3;
            this.lblLastUpdate.Text = "Última actualización: -";
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(321, 122);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "Cerrar";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.lblCurrentJob);
            this.groupControl1.Controls.Add(this.progressBar);
            this.groupControl1.Controls.Add(this.lblQueueStatus);
            this.groupControl1.Controls.Add(this.lblLastUpdate);
            this.groupControl1.Location = new System.Drawing.Point(12, 12);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(384, 98);
            this.groupControl1.TabIndex = 5;
            this.groupControl1.Text = "Estado de Impresión";
            // 
            // FrmPrintMonitor
            // 
            this.ClientSize = new System.Drawing.Size(408, 156);
            this.Controls.Add(this.groupControl1);
            this.Controls.Add(this.btnClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "FrmPrintMonitor";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Monitor de Impresión";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmPrintMonitor_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl lblCurrentJob;
        private System.Windows.Forms.ProgressBar progressBar;
        private DevExpress.XtraEditors.LabelControl lblQueueStatus;
        private DevExpress.XtraEditors.LabelControl lblLastUpdate;
        private DevExpress.XtraEditors.SimpleButton btnClose;
        private DevExpress.XtraEditors.GroupControl groupControl1;
    }
}