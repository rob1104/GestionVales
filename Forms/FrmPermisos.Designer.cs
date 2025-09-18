
namespace GestionValesRdz.Forms
{
    partial class FrmPermisos
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
            this.btnNada = new DevExpress.XtraEditors.SimpleButton();
            this.btnTodo = new DevExpress.XtraEditors.SimpleButton();
            this.chkPermisos = new DevExpress.XtraEditors.CheckedListBoxControl();
            this.btnAplicar = new DevExpress.XtraEditors.SimpleButton();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.cmbUsuario = new DevExpress.XtraEditors.LookUpEdit();
            ((System.ComponentModel.ISupportInitialize)(this.chkPermisos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cmbUsuario.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // btnNada
            // 
            this.btnNada.Location = new System.Drawing.Point(15, 460);
            this.btnNada.Name = "btnNada";
            this.btnNada.Size = new System.Drawing.Size(100, 23);
            this.btnNada.TabIndex = 10;
            this.btnNada.Text = "Quitar selección";
            this.btnNada.Click += new System.EventHandler(this.btnNada_Click);
            // 
            // btnTodo
            // 
            this.btnTodo.Location = new System.Drawing.Point(121, 460);
            this.btnTodo.Name = "btnTodo";
            this.btnTodo.Size = new System.Drawing.Size(100, 23);
            this.btnTodo.TabIndex = 9;
            this.btnTodo.Text = "Seleccionar todo";
            this.btnTodo.Click += new System.EventHandler(this.btnTodo_Click);
            // 
            // chkPermisos
            // 
            this.chkPermisos.Location = new System.Drawing.Point(12, 119);
            this.chkPermisos.MultiColumn = true;
            this.chkPermisos.Name = "chkPermisos";
            this.chkPermisos.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.chkPermisos.Size = new System.Drawing.Size(572, 335);
            this.chkPermisos.TabIndex = 8;
            // 
            // btnAplicar
            // 
            this.btnAplicar.Location = new System.Drawing.Point(493, 473);
            this.btnAplicar.Name = "btnAplicar";
            this.btnAplicar.Size = new System.Drawing.Size(91, 40);
            this.btnAplicar.TabIndex = 7;
            this.btnAplicar.Text = "Aplicar";
            this.btnAplicar.Click += new System.EventHandler(this.btnAplicar_Click);
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.cmbUsuario);
            this.groupControl1.Location = new System.Drawing.Point(12, 12);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(572, 86);
            this.groupControl1.TabIndex = 6;
            this.groupControl1.Text = "Seleccione un usuario:";
            // 
            // cmbUsuario
            // 
            this.cmbUsuario.Location = new System.Drawing.Point(25, 42);
            this.cmbUsuario.Name = "cmbUsuario";
            this.cmbUsuario.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.cmbUsuario.Properties.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("usuario", "Usuario")});
            this.cmbUsuario.Properties.NullText = "[Seleccione...]";
            this.cmbUsuario.Size = new System.Drawing.Size(533, 20);
            this.cmbUsuario.TabIndex = 0;
            this.cmbUsuario.EditValueChanged += new System.EventHandler(this.cmbUsuario_EditValueChanged);
            // 
            // FrmPermisos
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(596, 525);
            this.Controls.Add(this.btnNada);
            this.Controls.Add(this.btnTodo);
            this.Controls.Add(this.chkPermisos);
            this.Controls.Add(this.btnAplicar);
            this.Controls.Add(this.groupControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmPermisos";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Gestión de Permisos";
            this.Load += new System.EventHandler(this.FrmPermisos_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chkPermisos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.cmbUsuario.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton btnNada;
        private DevExpress.XtraEditors.SimpleButton btnTodo;
        private DevExpress.XtraEditors.CheckedListBoxControl chkPermisos;
        private DevExpress.XtraEditors.SimpleButton btnAplicar;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.LookUpEdit cmbUsuario;
    }
}