﻿namespace PrioritaFrm
{
    partial class PrioritaFrm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.ddlReparto = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lblMessage = new System.Windows.Forms.Label();
            this.btnEsporta = new System.Windows.Forms.Button();
            this.btnTrova = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.ddlSegnalatore = new System.Windows.Forms.ComboBox();
            this.dgvODL = new System.Windows.Forms.DataGridView();
            this.dgvTermini = new System.Windows.Forms.DataGridView();
            this.dgvScadenze = new System.Windows.Forms.DataGridView();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.dtScadenza = new System.Windows.Forms.DateTimePicker();
            this.nmQuantita = new System.Windows.Forms.NumericUpDown();
            this.btnInserisciScadenza = new System.Windows.Forms.Button();
            this.ddlFase = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.bgwBIL = new System.ComponentModel.BackgroundWorker();
            this.statusBar = new System.Windows.Forms.StatusStrip();
            this.statusBarLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.chkSoloAperti = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtArticolo = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvODL)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTermini)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvScadenze)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmQuantita)).BeginInit();
            this.statusBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // ddlReparto
            // 
            this.ddlReparto.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlReparto.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ddlReparto.FormattingEnabled = true;
            this.ddlReparto.Location = new System.Drawing.Point(398, 12);
            this.ddlReparto.Name = "ddlReparto";
            this.ddlReparto.Size = new System.Drawing.Size(295, 26);
            this.ddlReparto.TabIndex = 27;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(333, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 18);
            this.label3.TabIndex = 23;
            this.label3.Text = "Reparto";
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMessage.ForeColor = System.Drawing.Color.Red;
            this.lblMessage.Location = new System.Drawing.Point(1413, 16);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(46, 18);
            this.lblMessage.TabIndex = 18;
            this.lblMessage.Text = "label1";
            // 
            // btnEsporta
            // 
            this.btnEsporta.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEsporta.Location = new System.Drawing.Point(1297, 9);
            this.btnEsporta.Name = "btnEsporta";
            this.btnEsporta.Size = new System.Drawing.Size(94, 32);
            this.btnEsporta.TabIndex = 15;
            this.btnEsporta.Text = "Esporta...";
            this.btnEsporta.UseVisualStyleBackColor = true;
            this.btnEsporta.Click += new System.EventHandler(this.btnEsporta_Click);
            // 
            // btnTrova
            // 
            this.btnTrova.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTrova.Location = new System.Drawing.Point(1194, 9);
            this.btnTrova.Name = "btnTrova";
            this.btnTrova.Size = new System.Drawing.Size(75, 32);
            this.btnTrova.TabIndex = 17;
            this.btnTrova.Text = "Trova";
            this.btnTrova.UseVisualStyleBackColor = true;
            this.btnTrova.Click += new System.EventHandler(this.btnTrova_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(13, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 18);
            this.label1.TabIndex = 23;
            this.label1.Text = "Segnalatore";
            // 
            // ddlSegnalatore
            // 
            this.ddlSegnalatore.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlSegnalatore.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ddlSegnalatore.FormattingEnabled = true;
            this.ddlSegnalatore.Location = new System.Drawing.Point(103, 12);
            this.ddlSegnalatore.Name = "ddlSegnalatore";
            this.ddlSegnalatore.Size = new System.Drawing.Size(215, 26);
            this.ddlSegnalatore.TabIndex = 27;
            // 
            // dgvODL
            // 
            this.dgvODL.AllowUserToAddRows = false;
            this.dgvODL.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvODL.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvODL.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvODL.Location = new System.Drawing.Point(12, 81);
            this.dgvODL.MultiSelect = false;
            this.dgvODL.Name = "dgvODL";
            this.dgvODL.ReadOnly = true;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvODL.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvODL.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvODL.Size = new System.Drawing.Size(1508, 410);
            this.dgvODL.TabIndex = 28;
            this.dgvODL.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvODL_CellClick);
            this.dgvODL.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.dgvODL_RowPrePaint);
            // 
            // dgvTermini
            // 
            this.dgvTermini.AllowUserToAddRows = false;
            this.dgvTermini.AllowUserToDeleteRows = false;
            this.dgvTermini.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvTermini.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvTermini.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvTermini.DefaultCellStyle = dataGridViewCellStyle4;
            this.dgvTermini.Location = new System.Drawing.Point(777, 533);
            this.dgvTermini.Name = "dgvTermini";
            this.dgvTermini.ReadOnly = true;
            this.dgvTermini.RowHeadersVisible = false;
            this.dgvTermini.Size = new System.Drawing.Size(743, 211);
            this.dgvTermini.TabIndex = 29;
            // 
            // dgvScadenze
            // 
            this.dgvScadenze.AllowUserToAddRows = false;
            this.dgvScadenze.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvScadenze.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dgvScadenze.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvScadenze.DefaultCellStyle = dataGridViewCellStyle6;
            this.dgvScadenze.Location = new System.Drawing.Point(12, 537);
            this.dgvScadenze.Name = "dgvScadenze";
            this.dgvScadenze.ReadOnly = true;
            this.dgvScadenze.Size = new System.Drawing.Size(460, 207);
            this.dgvScadenze.TabIndex = 29;
            this.dgvScadenze.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvScadenze_DataError);
            this.dgvScadenze.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.dgvScadenze_RowsRemoved);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(774, 507);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 18);
            this.label2.TabIndex = 30;
            this.label2.Text = "TERMINI";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(13, 507);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 18);
            this.label4.TabIndex = 31;
            this.label4.Text = "SCADENZE";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(509, 535);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(107, 18);
            this.label5.TabIndex = 31;
            this.label5.Text = "Data scadenza";
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(509, 596);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(131, 18);
            this.label6.TabIndex = 31;
            this.label6.Text = "Quantita scadenza";
            // 
            // dtScadenza
            // 
            this.dtScadenza.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.dtScadenza.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtScadenza.Location = new System.Drawing.Point(512, 556);
            this.dtScadenza.Name = "dtScadenza";
            this.dtScadenza.Size = new System.Drawing.Size(200, 22);
            this.dtScadenza.TabIndex = 32;
            // 
            // nmQuantita
            // 
            this.nmQuantita.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.nmQuantita.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nmQuantita.Location = new System.Drawing.Point(512, 617);
            this.nmQuantita.Maximum = new decimal(new int[] {
            90000,
            0,
            0,
            0});
            this.nmQuantita.Name = "nmQuantita";
            this.nmQuantita.Size = new System.Drawing.Size(200, 22);
            this.nmQuantita.TabIndex = 33;
            // 
            // btnInserisciScadenza
            // 
            this.btnInserisciScadenza.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnInserisciScadenza.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnInserisciScadenza.Location = new System.Drawing.Point(512, 664);
            this.btnInserisciScadenza.Name = "btnInserisciScadenza";
            this.btnInserisciScadenza.Size = new System.Drawing.Size(200, 32);
            this.btnInserisciScadenza.TabIndex = 34;
            this.btnInserisciScadenza.Text = "Inserisci scadenza";
            this.btnInserisciScadenza.UseVisualStyleBackColor = true;
            this.btnInserisciScadenza.Click += new System.EventHandler(this.btnInserisciScadenza_Click);
            // 
            // ddlFase
            // 
            this.ddlFase.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlFase.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ddlFase.FormattingEnabled = true;
            this.ddlFase.Location = new System.Drawing.Point(760, 12);
            this.ddlFase.Name = "ddlFase";
            this.ddlFase.Size = new System.Drawing.Size(295, 26);
            this.ddlFase.TabIndex = 36;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(714, 16);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 18);
            this.label7.TabIndex = 35;
            this.label7.Text = "Fase";
            // 
            // bgwBIL
            // 
            this.bgwBIL.WorkerReportsProgress = true;
            this.bgwBIL.WorkerSupportsCancellation = true;
            this.bgwBIL.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwBIL_DoWork);
            this.bgwBIL.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgwBIL_ProgressChanged);
            this.bgwBIL.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgwBIL_RunWorkerCompleted);
            // 
            // statusBar
            // 
            this.statusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusBarLabel});
            this.statusBar.Location = new System.Drawing.Point(0, 734);
            this.statusBar.Name = "statusBar";
            this.statusBar.Size = new System.Drawing.Size(1532, 22);
            this.statusBar.TabIndex = 37;
            this.statusBar.Text = "statusStrip1";
            // 
            // statusBarLabel
            // 
            this.statusBarLabel.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.statusBarLabel.Name = "statusBarLabel";
            this.statusBarLabel.Size = new System.Drawing.Size(131, 17);
            this.statusBarLabel.Text = "toolStripStatusLabel1";
            // 
            // chkSoloAperti
            // 
            this.chkSoloAperti.AutoSize = true;
            this.chkSoloAperti.Checked = true;
            this.chkSoloAperti.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSoloAperti.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkSoloAperti.Location = new System.Drawing.Point(1076, 14);
            this.chkSoloAperti.Name = "chkSoloAperti";
            this.chkSoloAperti.Size = new System.Drawing.Size(98, 22);
            this.chkSoloAperti.TabIndex = 38;
            this.chkSoloAperti.Text = "Solo aperti";
            this.chkSoloAperti.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(42, 52);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(58, 18);
            this.label8.TabIndex = 23;
            this.label8.Text = "Articolo";
            // 
            // txtArticolo
            // 
            this.txtArticolo.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtArticolo.Location = new System.Drawing.Point(103, 53);
            this.txtArticolo.Name = "txtArticolo";
            this.txtArticolo.Size = new System.Drawing.Size(590, 24);
            this.txtArticolo.TabIndex = 39;
            // 
            // PrioritaFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1532, 756);
            this.Controls.Add(this.txtArticolo);
            this.Controls.Add(this.chkSoloAperti);
            this.Controls.Add(this.statusBar);
            this.Controls.Add(this.ddlFase);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.btnInserisciScadenza);
            this.Controls.Add(this.nmQuantita);
            this.Controls.Add(this.dtScadenza);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dgvScadenze);
            this.Controls.Add(this.dgvTermini);
            this.Controls.Add(this.dgvODL);
            this.Controls.Add(this.ddlSegnalatore);
            this.Controls.Add(this.ddlReparto);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.btnEsporta);
            this.Controls.Add(this.btnTrova);
            this.Name = "PrioritaFrm";
            this.Text = "Priorità Form";
            this.Load += new System.EventHandler(this.PrioritaFrm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvODL)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTermini)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvScadenze)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nmQuantita)).EndInit();
            this.statusBar.ResumeLayout(false);
            this.statusBar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ComboBox ddlReparto;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Button btnEsporta;
        private System.Windows.Forms.Button btnTrova;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox ddlSegnalatore;
        private System.Windows.Forms.DataGridView dgvODL;
        private System.Windows.Forms.DataGridView dgvTermini;
        private System.Windows.Forms.DataGridView dgvScadenze;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DateTimePicker dtScadenza;
        private System.Windows.Forms.NumericUpDown nmQuantita;
        private System.Windows.Forms.Button btnInserisciScadenza;
        private System.Windows.Forms.ComboBox ddlFase;
        private System.Windows.Forms.Label label7;
        private System.ComponentModel.BackgroundWorker bgwBIL;
        private System.Windows.Forms.StatusStrip statusBar;
        private System.Windows.Forms.ToolStripStatusLabel statusBarLabel;
        private System.Windows.Forms.CheckBox chkSoloAperti;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtArticolo;
    }
}

