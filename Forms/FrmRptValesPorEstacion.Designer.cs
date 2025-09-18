namespace GestionValesRdz.Forms
{
    partial class FrmRptValesPorEstacion
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmRptValesPorEstacion));
            this.btnImprimir = new System.Windows.Forms.Button();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colFolio = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colFecha = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colFechaCorte = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colEmpresa = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCliente = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colImporte = new DevExpress.XtraGrid.Columns.GridColumn();
            this.btnGenerar = new System.Windows.Forms.Button();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.radioCorte = new System.Windows.Forms.RadioButton();
            this.groupCorte = new System.Windows.Forms.GroupBox();
            this.dtpDesdeCorte = new DevExpress.XtraEditors.DateEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.dtpHastaCorte = new DevExpress.XtraEditors.DateEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.radioCanje = new System.Windows.Forms.RadioButton();
            this.groupCanje = new System.Windows.Forms.GroupBox();
            this.dtpDesdeCanje = new DevExpress.XtraEditors.DateEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.dtpHastaCanje = new DevExpress.XtraEditors.DateEdit();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            this.groupCorte.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtpDesdeCorte.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpDesdeCorte.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpHastaCorte.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpHastaCorte.Properties)).BeginInit();
            this.groupCanje.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtpDesdeCanje.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpDesdeCanje.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpHastaCanje.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpHastaCanje.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // btnImprimir
            // 
            this.btnImprimir.Location = new System.Drawing.Point(638, 107);
            this.btnImprimir.Name = "btnImprimir";
            this.btnImprimir.Size = new System.Drawing.Size(75, 35);
            this.btnImprimir.TabIndex = 11;
            this.btnImprimir.Text = "Imprimir";
            this.btnImprimir.UseVisualStyleBackColor = true;
            this.btnImprimir.Click += new System.EventHandler(this.btnImprimir_Click);
            // 
            // gridControl1
            // 
            this.gridControl1.Location = new System.Drawing.Point(12, 148);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(701, 377);
            this.gridControl1.TabIndex = 10;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colFolio,
            this.colFecha,
            this.colFechaCorte,
            this.colEmpresa,
            this.colCliente,
            this.colImporte});
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.GroupCount = 1;
            this.gridView1.GroupSummary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridGroupSummaryItem(DevExpress.Data.SummaryItemType.Sum, "Importe", this.colImporte, "TOTAL = {0:c2}"),
            new DevExpress.XtraGrid.GridGroupSummaryItem(DevExpress.Data.SummaryItemType.Count, "colCliente", this.colCliente, "Cantidad = {0:f0}")});
            this.gridView1.Name = "gridView1";
            this.gridView1.OptionsPrint.PrintVertLines = false;
            this.gridView1.OptionsPrint.RtfPageFooter = resources.GetString("gridView1.OptionsPrint.RtfPageFooter");
            this.gridView1.OptionsPrint.RtfReportFooter = resources.GetString("gridView1.OptionsPrint.RtfReportFooter");
            this.gridView1.OptionsPrint.RtfReportHeader = resources.GetString("gridView1.OptionsPrint.RtfReportHeader");
            this.gridView1.OptionsView.ColumnAutoWidth = false;
            this.gridView1.OptionsView.ShowFooter = true;
            this.gridView1.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.colEmpresa, DevExpress.Data.ColumnSortOrder.Ascending)});
            // 
            // colFolio
            // 
            this.colFolio.AppearanceHeader.Options.UseTextOptions = true;
            this.colFolio.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colFolio.Caption = "Folio";
            this.colFolio.FieldName = "Folio";
            this.colFolio.Name = "colFolio";
            this.colFolio.Visible = true;
            this.colFolio.VisibleIndex = 0;
            this.colFolio.Width = 66;
            // 
            // colFecha
            // 
            this.colFecha.AppearanceHeader.Options.UseTextOptions = true;
            this.colFecha.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colFecha.Caption = "Fecha canje";
            this.colFecha.FieldName = "Fecha_Canje";
            this.colFecha.Name = "colFecha";
            this.colFecha.Visible = true;
            this.colFecha.VisibleIndex = 1;
            this.colFecha.Width = 90;
            // 
            // colFechaCorte
            // 
            this.colFechaCorte.AppearanceHeader.Options.UseTextOptions = true;
            this.colFechaCorte.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colFechaCorte.Caption = "Fecha corte";
            this.colFechaCorte.FieldName = "Fecha_Corte";
            this.colFechaCorte.Name = "colFechaCorte";
            this.colFechaCorte.Visible = true;
            this.colFechaCorte.VisibleIndex = 2;
            this.colFechaCorte.Width = 87;
            // 
            // colEmpresa
            // 
            this.colEmpresa.AppearanceHeader.Options.UseTextOptions = true;
            this.colEmpresa.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colEmpresa.Caption = "Estacion";
            this.colEmpresa.FieldName = "Estacion";
            this.colEmpresa.Name = "colEmpresa";
            this.colEmpresa.Visible = true;
            this.colEmpresa.VisibleIndex = 2;
            this.colEmpresa.Width = 184;
            // 
            // colCliente
            // 
            this.colCliente.AppearanceHeader.Options.UseTextOptions = true;
            this.colCliente.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colCliente.Caption = "Cliente";
            this.colCliente.FieldName = "Cliente";
            this.colCliente.Name = "colCliente";
            this.colCliente.Visible = true;
            this.colCliente.VisibleIndex = 3;
            this.colCliente.Width = 239;
            // 
            // colImporte
            // 
            this.colImporte.AppearanceCell.Options.UseTextOptions = true;
            this.colImporte.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.colImporte.AppearanceHeader.Options.UseTextOptions = true;
            this.colImporte.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.colImporte.Caption = "Importe";
            this.colImporte.DisplayFormat.FormatString = "c";
            this.colImporte.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colImporte.FieldName = "Importe";
            this.colImporte.Name = "colImporte";
            this.colImporte.Summary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
            new DevExpress.XtraGrid.GridColumnSummaryItem(DevExpress.Data.SummaryItemType.Sum, "Importe", "TOTAL = {0:c2}")});
            this.colImporte.Visible = true;
            this.colImporte.VisibleIndex = 4;
            this.colImporte.Width = 129;
            // 
            // btnGenerar
            // 
            this.btnGenerar.Location = new System.Drawing.Point(638, 56);
            this.btnGenerar.Name = "btnGenerar";
            this.btnGenerar.Size = new System.Drawing.Size(75, 36);
            this.btnGenerar.TabIndex = 9;
            this.btnGenerar.Text = "Generar";
            this.btnGenerar.UseVisualStyleBackColor = true;
            this.btnGenerar.Click += new System.EventHandler(this.btnGenerar_Click);
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.radioCorte);
            this.groupControl1.Controls.Add(this.groupCorte);
            this.groupControl1.Controls.Add(this.radioCanje);
            this.groupControl1.Controls.Add(this.groupCanje);
            this.groupControl1.Location = new System.Drawing.Point(12, 12);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(614, 130);
            this.groupControl1.TabIndex = 8;
            this.groupControl1.Text = "Filtro";
            // 
            // radioCorte
            // 
            this.radioCorte.AutoSize = true;
            this.radioCorte.Location = new System.Drawing.Point(284, 33);
            this.radioCorte.Name = "radioCorte";
            this.radioCorte.Size = new System.Drawing.Size(14, 13);
            this.radioCorte.TabIndex = 7;
            this.radioCorte.UseVisualStyleBackColor = true;
            this.radioCorte.CheckedChanged += new System.EventHandler(this.radioCorte_CheckedChanged);
            // 
            // groupCorte
            // 
            this.groupCorte.Controls.Add(this.dtpDesdeCorte);
            this.groupCorte.Controls.Add(this.labelControl3);
            this.groupCorte.Controls.Add(this.dtpHastaCorte);
            this.groupCorte.Controls.Add(this.labelControl4);
            this.groupCorte.Enabled = false;
            this.groupCorte.Location = new System.Drawing.Point(304, 33);
            this.groupCorte.Name = "groupCorte";
            this.groupCorte.Size = new System.Drawing.Size(200, 78);
            this.groupCorte.TabIndex = 6;
            this.groupCorte.TabStop = false;
            this.groupCorte.Text = "Fecha de corte";
            // 
            // dtpDesdeCorte
            // 
            this.dtpDesdeCorte.EditValue = null;
            this.dtpDesdeCorte.Location = new System.Drawing.Point(58, 20);
            this.dtpDesdeCorte.Name = "dtpDesdeCorte";
            this.dtpDesdeCorte.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtpDesdeCorte.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtpDesdeCorte.Size = new System.Drawing.Size(100, 20);
            this.dtpDesdeCorte.TabIndex = 0;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(18, 50);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(32, 13);
            this.labelControl3.TabIndex = 3;
            this.labelControl3.Text = "Hasta:";
            // 
            // dtpHastaCorte
            // 
            this.dtpHastaCorte.EditValue = null;
            this.dtpHastaCorte.Location = new System.Drawing.Point(58, 46);
            this.dtpHastaCorte.Name = "dtpHastaCorte";
            this.dtpHastaCorte.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtpHastaCorte.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtpHastaCorte.Size = new System.Drawing.Size(100, 20);
            this.dtpHastaCorte.TabIndex = 1;
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(18, 23);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(34, 13);
            this.labelControl4.TabIndex = 2;
            this.labelControl4.Text = "Desde:";
            // 
            // radioCanje
            // 
            this.radioCanje.AutoSize = true;
            this.radioCanje.Checked = true;
            this.radioCanje.Location = new System.Drawing.Point(6, 33);
            this.radioCanje.Name = "radioCanje";
            this.radioCanje.Size = new System.Drawing.Size(14, 13);
            this.radioCanje.TabIndex = 5;
            this.radioCanje.TabStop = true;
            this.radioCanje.UseVisualStyleBackColor = true;
            // 
            // groupCanje
            // 
            this.groupCanje.Controls.Add(this.dtpDesdeCanje);
            this.groupCanje.Controls.Add(this.labelControl2);
            this.groupCanje.Controls.Add(this.dtpHastaCanje);
            this.groupCanje.Controls.Add(this.labelControl1);
            this.groupCanje.Location = new System.Drawing.Point(26, 33);
            this.groupCanje.Name = "groupCanje";
            this.groupCanje.Size = new System.Drawing.Size(200, 78);
            this.groupCanje.TabIndex = 4;
            this.groupCanje.TabStop = false;
            this.groupCanje.Text = "Fecha de canje";
            // 
            // dtpDesdeCanje
            // 
            this.dtpDesdeCanje.EditValue = null;
            this.dtpDesdeCanje.Location = new System.Drawing.Point(58, 20);
            this.dtpDesdeCanje.Name = "dtpDesdeCanje";
            this.dtpDesdeCanje.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtpDesdeCanje.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtpDesdeCanje.Size = new System.Drawing.Size(100, 20);
            this.dtpDesdeCanje.TabIndex = 0;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(18, 50);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(32, 13);
            this.labelControl2.TabIndex = 3;
            this.labelControl2.Text = "Hasta:";
            // 
            // dtpHastaCanje
            // 
            this.dtpHastaCanje.EditValue = null;
            this.dtpHastaCanje.Location = new System.Drawing.Point(58, 46);
            this.dtpHastaCanje.Name = "dtpHastaCanje";
            this.dtpHastaCanje.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtpHastaCanje.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtpHastaCanje.Size = new System.Drawing.Size(100, 20);
            this.dtpHastaCanje.TabIndex = 1;
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(18, 23);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(34, 13);
            this.labelControl1.TabIndex = 2;
            this.labelControl1.Text = "Desde:";
            // 
            // FrmRptValesPorEstacion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(731, 537);
            this.Controls.Add(this.btnImprimir);
            this.Controls.Add(this.gridControl1);
            this.Controls.Add(this.btnGenerar);
            this.Controls.Add(this.groupControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "FrmRptValesPorEstacion";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Vales por estación";
            this.Load += new System.EventHandler(this.FrmRptValesPorEstacion_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            this.groupCorte.ResumeLayout(false);
            this.groupCorte.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtpDesdeCorte.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpDesdeCorte.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpHastaCorte.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpHastaCorte.Properties)).EndInit();
            this.groupCanje.ResumeLayout(false);
            this.groupCanje.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtpDesdeCanje.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpDesdeCanje.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpHastaCanje.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpHastaCanje.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnImprimir;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraGrid.Columns.GridColumn colFolio;
        private DevExpress.XtraGrid.Columns.GridColumn colFecha;
        private DevExpress.XtraGrid.Columns.GridColumn colEmpresa;
        private DevExpress.XtraGrid.Columns.GridColumn colCliente;
        private DevExpress.XtraGrid.Columns.GridColumn colImporte;
        private System.Windows.Forms.Button btnGenerar;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.DateEdit dtpHastaCanje;
        private DevExpress.XtraEditors.DateEdit dtpDesdeCanje;
        private DevExpress.XtraGrid.Columns.GridColumn colFechaCorte;
        private System.Windows.Forms.RadioButton radioCorte;
        private System.Windows.Forms.GroupBox groupCorte;
        private DevExpress.XtraEditors.DateEdit dtpDesdeCorte;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.DateEdit dtpHastaCorte;
        private DevExpress.XtraEditors.LabelControl labelControl4;
        private System.Windows.Forms.RadioButton radioCanje;
        private System.Windows.Forms.GroupBox groupCanje;
    }
}