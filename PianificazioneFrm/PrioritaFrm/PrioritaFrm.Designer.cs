namespace PrioritaFrm
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
            this.ddlReparto = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lblMessage = new System.Windows.Forms.Label();
            this.btnEsporta = new System.Windows.Forms.Button();
            this.btnSalva = new System.Windows.Forms.Button();
            this.btnTrova = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.ddlSegnalatore = new System.Windows.Forms.ComboBox();
            this.dgvODL = new System.Windows.Forms.DataGridView();
            this.dgvTermini = new System.Windows.Forms.DataGridView();
            this.dgvScadenze = new System.Windows.Forms.DataGridView();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvODL)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTermini)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvScadenze)).BeginInit();
            this.SuspendLayout();
            // 
            // ddlReparto
            // 
            this.ddlReparto.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlReparto.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ddlReparto.FormattingEnabled = true;
            this.ddlReparto.Location = new System.Drawing.Point(402, 9);
            this.ddlReparto.Name = "ddlReparto";
            this.ddlReparto.Size = new System.Drawing.Size(295, 26);
            this.ddlReparto.TabIndex = 27;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(337, 13);
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
            this.lblMessage.Location = new System.Drawing.Point(1094, 13);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(46, 18);
            this.lblMessage.TabIndex = 18;
            this.lblMessage.Text = "label1";
            // 
            // btnEsporta
            // 
            this.btnEsporta.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEsporta.Location = new System.Drawing.Point(941, 6);
            this.btnEsporta.Name = "btnEsporta";
            this.btnEsporta.Size = new System.Drawing.Size(94, 32);
            this.btnEsporta.TabIndex = 15;
            this.btnEsporta.Text = "Esporta...";
            this.btnEsporta.UseVisualStyleBackColor = true;
            // 
            // btnSalva
            // 
            this.btnSalva.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSalva.Location = new System.Drawing.Point(842, 6);
            this.btnSalva.Name = "btnSalva";
            this.btnSalva.Size = new System.Drawing.Size(75, 32);
            this.btnSalva.TabIndex = 16;
            this.btnSalva.Text = "Salva";
            this.btnSalva.UseVisualStyleBackColor = true;
            // 
            // btnTrova
            // 
            this.btnTrova.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTrova.Location = new System.Drawing.Point(744, 6);
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
            this.label1.Location = new System.Drawing.Point(13, 14);
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
            this.ddlSegnalatore.Location = new System.Drawing.Point(103, 9);
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
            this.dgvODL.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvODL.Location = new System.Drawing.Point(12, 56);
            this.dgvODL.Name = "dgvODL";
            this.dgvODL.ReadOnly = true;
            this.dgvODL.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvODL.Size = new System.Drawing.Size(1508, 482);
            this.dgvODL.TabIndex = 28;
            this.dgvODL.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvODL_CellClick);
            this.dgvODL.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.dgvODL_RowPrePaint);
            // 
            // dgvTermini
            // 
            this.dgvTermini.AllowUserToAddRows = false;
            this.dgvTermini.AllowUserToDeleteRows = false;
            this.dgvTermini.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTermini.Location = new System.Drawing.Point(777, 566);
            this.dgvTermini.Name = "dgvTermini";
            this.dgvTermini.ReadOnly = true;
            this.dgvTermini.Size = new System.Drawing.Size(743, 178);
            this.dgvTermini.TabIndex = 29;
            // 
            // dgvScadenze
            // 
            this.dgvScadenze.AllowUserToDeleteRows = false;
            this.dgvScadenze.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvScadenze.Location = new System.Drawing.Point(12, 566);
            this.dgvScadenze.Name = "dgvScadenze";
            this.dgvScadenze.ReadOnly = true;
            this.dgvScadenze.Size = new System.Drawing.Size(743, 178);
            this.dgvScadenze.TabIndex = 29;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(774, 545);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 18);
            this.label2.TabIndex = 30;
            this.label2.Text = "TERMINI";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(13, 541);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 18);
            this.label4.TabIndex = 31;
            this.label4.Text = "SCADENZE";
            // 
            // PrioritaFrm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1532, 756);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dgvScadenze);
            this.Controls.Add(this.dgvTermini);
            this.Controls.Add(this.dgvODL);
            this.Controls.Add(this.ddlSegnalatore);
            this.Controls.Add(this.ddlReparto);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.btnEsporta);
            this.Controls.Add(this.btnSalva);
            this.Controls.Add(this.btnTrova);
            this.Name = "PrioritaFrm";
            this.Text = "Priorità Form";
            this.Load += new System.EventHandler(this.PrioritaFrm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvODL)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTermini)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvScadenze)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ComboBox ddlReparto;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Button btnEsporta;
        private System.Windows.Forms.Button btnSalva;
        private System.Windows.Forms.Button btnTrova;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox ddlSegnalatore;
        private System.Windows.Forms.DataGridView dgvODL;
        private System.Windows.Forms.DataGridView dgvTermini;
        private System.Windows.Forms.DataGridView dgvScadenze;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
    }
}

