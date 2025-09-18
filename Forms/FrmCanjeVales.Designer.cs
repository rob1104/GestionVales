namespace GestionValesRdz.Forms
{
    partial class FrmCanjeVales
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
            this.components = new System.ComponentModel.Container();
            this.lv = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.idEstacion = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.cm = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.quitarDeLaListaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnCanjear = new DevExpress.XtraEditors.SimpleButton();
            this.txtLetra = new DevExpress.XtraEditors.TextEdit();
            this.labelControl7 = new DevExpress.XtraEditors.LabelControl();
            this.btnAdd = new DevExpress.XtraEditors.SimpleButton();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.cmbEstacion = new DevExpress.XtraEditors.LookUpEdit();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.dtpFechaCorte = new DevExpress.XtraEditors.DateEdit();
            this.txtValesCanjeados = new DevExpress.XtraEditors.TextEdit();
            this.labelControl3 = new DevExpress.XtraEditors.LabelControl();
            this.txtTotal = new DevExpress.XtraEditors.TextEdit();
            this.labelControl4 = new DevExpress.XtraEditors.LabelControl();
            this.cm.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtLetra.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbEstacion.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpFechaCorte.Properties.CalendarTimeProperties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpFechaCorte.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValesCanjeados.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotal.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // lv
            // 
            this.lv.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader5,
            this.columnHeader6,
            this.idEstacion});
            this.lv.ContextMenuStrip = this.cm;
            this.lv.FullRowSelect = true;
            this.lv.HideSelection = false;
            this.lv.Location = new System.Drawing.Point(12, 50);
            this.lv.Name = "lv";
            this.lv.Size = new System.Drawing.Size(731, 224);
            this.lv.TabIndex = 2;
            this.lv.UseCompatibleStateImageBehavior = false;
            this.lv.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Folio";
            this.columnHeader1.Width = 84;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Estación";
            this.columnHeader2.Width = 239;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Cliente";
            this.columnHeader3.Width = 271;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Importe";
            this.columnHeader4.Width = 133;
            // 
            // idEstacion
            // 
            this.idEstacion.Text = "IdEstacion";
            // 
            // cm
            // 
            this.cm.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.quitarDeLaListaToolStripMenuItem});
            this.cm.Name = "cm";
            this.cm.Size = new System.Drawing.Size(160, 26);
            // 
            // quitarDeLaListaToolStripMenuItem
            // 
            this.quitarDeLaListaToolStripMenuItem.Image = global::GestionValesRdz.Properties.Resources.cancel24;
            this.quitarDeLaListaToolStripMenuItem.Name = "quitarDeLaListaToolStripMenuItem";
            this.quitarDeLaListaToolStripMenuItem.Size = new System.Drawing.Size(159, 22);
            this.quitarDeLaListaToolStripMenuItem.Text = "Quitar de la lista";
            this.quitarDeLaListaToolStripMenuItem.Click += new System.EventHandler(this.quitarDeLaListaToolStripMenuItem_Click);
            // 
            // btnCanjear
            // 
            this.btnCanjear.Location = new System.Drawing.Point(671, 283);
            this.btnCanjear.Name = "btnCanjear";
            this.btnCanjear.Size = new System.Drawing.Size(72, 28);
            this.btnCanjear.TabIndex = 3;
            this.btnCanjear.Text = "Canjear";
            this.btnCanjear.Click += new System.EventHandler(this.btnCanjear_Click);
            // 
            // txtLetra
            // 
            this.txtLetra.Location = new System.Drawing.Point(567, 24);
            this.txtLetra.Name = "txtLetra";
            this.txtLetra.Properties.Mask.EditMask = "f0";
            this.txtLetra.Size = new System.Drawing.Size(150, 20);
            this.txtLetra.TabIndex = 0;
            this.txtLetra.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtLetra_KeyDown);
            // 
            // labelControl7
            // 
            this.labelControl7.Location = new System.Drawing.Point(535, 27);
            this.labelControl7.Name = "labelControl7";
            this.labelControl7.Size = new System.Drawing.Size(26, 13);
            this.labelControl7.TabIndex = 15;
            this.labelControl7.Text = "Folio:";
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(723, 25);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(19, 17);
            this.btnAdd.TabIndex = 1;
            this.btnAdd.Text = "+";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(12, 27);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(44, 13);
            this.labelControl1.TabIndex = 16;
            this.labelControl1.Text = "Estación:";
            // 
            // cmbEstacion
            // 
            this.cmbEstacion.Location = new System.Drawing.Point(62, 24);
            this.cmbEstacion.Name = "cmbEstacion";
            this.cmbEstacion.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbEstacion.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("nombre", "Alias")});
            this.cmbEstacion.Size = new System.Drawing.Size(255, 20);
            this.cmbEstacion.TabIndex = 17;
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(340, 27);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(61, 13);
            this.labelControl2.TabIndex = 18;
            this.labelControl2.Text = "Fecha corte:";
            // 
            // dtpFechaCorte
            // 
            this.dtpFechaCorte.EditValue = null;
            this.dtpFechaCorte.Location = new System.Drawing.Point(407, 24);
            this.dtpFechaCorte.Name = "dtpFechaCorte";
            this.dtpFechaCorte.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtpFechaCorte.Properties.CalendarTimeProperties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.dtpFechaCorte.Size = new System.Drawing.Size(106, 20);
            this.dtpFechaCorte.TabIndex = 19;
            // 
            // txtValesCanjeados
            // 
            this.txtValesCanjeados.EditValue = "0";
            this.txtValesCanjeados.Location = new System.Drawing.Point(165, 287);
            this.txtValesCanjeados.Name = "txtValesCanjeados";
            this.txtValesCanjeados.Properties.AppearanceReadOnly.BackColor = System.Drawing.Color.White;
            this.txtValesCanjeados.Properties.AppearanceReadOnly.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtValesCanjeados.Properties.AppearanceReadOnly.ForeColor = System.Drawing.Color.Blue;
            this.txtValesCanjeados.Properties.AppearanceReadOnly.Options.UseBackColor = true;
            this.txtValesCanjeados.Properties.AppearanceReadOnly.Options.UseFont = true;
            this.txtValesCanjeados.Properties.AppearanceReadOnly.Options.UseForeColor = true;
            this.txtValesCanjeados.Properties.AppearanceReadOnly.Options.UseTextOptions = true;
            this.txtValesCanjeados.Properties.AppearanceReadOnly.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtValesCanjeados.Properties.Mask.EditMask = "f0";
            this.txtValesCanjeados.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtValesCanjeados.Properties.ReadOnly = true;
            this.txtValesCanjeados.Size = new System.Drawing.Size(56, 20);
            this.txtValesCanjeados.TabIndex = 20;
            // 
            // labelControl3
            // 
            this.labelControl3.Location = new System.Drawing.Point(17, 290);
            this.labelControl3.Name = "labelControl3";
            this.labelControl3.Size = new System.Drawing.Size(142, 13);
            this.labelControl3.TabIndex = 21;
            this.labelControl3.Text = "Cantidad de vales canjeados:";
            // 
            // txtTotal
            // 
            this.txtTotal.EditValue = "0";
            this.txtTotal.Location = new System.Drawing.Point(482, 287);
            this.txtTotal.Name = "txtTotal";
            this.txtTotal.Properties.AppearanceReadOnly.BackColor = System.Drawing.Color.White;
            this.txtTotal.Properties.AppearanceReadOnly.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTotal.Properties.AppearanceReadOnly.ForeColor = System.Drawing.Color.Blue;
            this.txtTotal.Properties.AppearanceReadOnly.Options.UseBackColor = true;
            this.txtTotal.Properties.AppearanceReadOnly.Options.UseFont = true;
            this.txtTotal.Properties.AppearanceReadOnly.Options.UseForeColor = true;
            this.txtTotal.Properties.AppearanceReadOnly.Options.UseTextOptions = true;
            this.txtTotal.Properties.AppearanceReadOnly.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
            this.txtTotal.Properties.DisplayFormat.FormatString = "{0:c2}";
            this.txtTotal.Properties.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Custom;
            this.txtTotal.Properties.Mask.EditMask = "c2";
            this.txtTotal.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            this.txtTotal.Properties.ReadOnly = true;
            this.txtTotal.Size = new System.Drawing.Size(79, 20);
            this.txtTotal.TabIndex = 22;
            // 
            // labelControl4
            // 
            this.labelControl4.Location = new System.Drawing.Point(448, 290);
            this.labelControl4.Name = "labelControl4";
            this.labelControl4.Size = new System.Drawing.Size(28, 13);
            this.labelControl4.TabIndex = 23;
            this.labelControl4.Text = "Total:";
            // 
            // FrmCanjeVales
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(754, 323);
            this.Controls.Add(this.txtTotal);
            this.Controls.Add(this.labelControl4);
            this.Controls.Add(this.txtValesCanjeados);
            this.Controls.Add(this.labelControl3);
            this.Controls.Add(this.dtpFechaCorte);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.cmbEstacion);
            this.Controls.Add(this.labelControl1);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.txtLetra);
            this.Controls.Add(this.labelControl7);
            this.Controls.Add(this.btnCanjear);
            this.Controls.Add(this.lv);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmCanjeVales";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Canjear Vales";
            this.Load += new System.EventHandler(this.FrmCanjeVales_Load);
            this.cm.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtLetra.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cmbEstacion.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpFechaCorte.Properties.CalendarTimeProperties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dtpFechaCorte.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtValesCanjeados.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtTotal.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lv;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private DevExpress.XtraEditors.SimpleButton btnCanjear;
        private DevExpress.XtraEditors.TextEdit txtLetra;
        private DevExpress.XtraEditors.LabelControl labelControl7;
        private DevExpress.XtraEditors.SimpleButton btnAdd;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LookUpEdit cmbEstacion;
        private System.Windows.Forms.ColumnHeader idEstacion;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.DateEdit dtpFechaCorte;
        private System.Windows.Forms.ContextMenuStrip cm;
        private System.Windows.Forms.ToolStripMenuItem quitarDeLaListaToolStripMenuItem;
        private DevExpress.XtraEditors.TextEdit txtValesCanjeados;
        private DevExpress.XtraEditors.LabelControl labelControl3;
        private DevExpress.XtraEditors.TextEdit txtTotal;
        private DevExpress.XtraEditors.LabelControl labelControl4;
    }
}