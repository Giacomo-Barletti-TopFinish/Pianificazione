namespace PianificazioneFrm
{
    partial class Form1
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
            this.btnTrova = new System.Windows.Forms.Button();
            this.dtAl = new System.Windows.Forms.DateTimePicker();
            this.dtDal = new System.Windows.Forms.DateTimePicker();
            this.lblMessage = new System.Windows.Forms.Label();
            this.dgvGriglia = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.ddlReparto = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.ddlFase = new System.Windows.Forms.ComboBox();
            this.btnSalva = new System.Windows.Forms.Button();
            this.btnEsporta = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGriglia)).BeginInit();
            this.SuspendLayout();
            // 
            // btnTrova
            // 
            this.btnTrova.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTrova.Location = new System.Drawing.Point(690, 8);
            this.btnTrova.Name = "btnTrova";
            this.btnTrova.Size = new System.Drawing.Size(75, 32);
            this.btnTrova.TabIndex = 5;
            this.btnTrova.Text = "Trova";
            this.btnTrova.UseVisualStyleBackColor = true;
            this.btnTrova.Click += new System.EventHandler(this.btnTrova_Click);
            // 
            // dtAl
            // 
            this.dtAl.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtAl.Location = new System.Drawing.Point(407, 12);
            this.dtAl.Name = "dtAl";
            this.dtAl.Size = new System.Drawing.Size(245, 24);
            this.dtAl.TabIndex = 4;
            // 
            // dtDal
            // 
            this.dtDal.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtDal.Location = new System.Drawing.Point(78, 12);
            this.dtDal.Name = "dtDal";
            this.dtDal.Size = new System.Drawing.Size(245, 24);
            this.dtDal.TabIndex = 3;
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMessage.ForeColor = System.Drawing.Color.Red;
            this.lblMessage.Location = new System.Drawing.Point(791, 15);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(46, 18);
            this.lblMessage.TabIndex = 9;
            this.lblMessage.Text = "label1";
            // 
            // dgvGriglia
            // 
            this.dgvGriglia.AllowUserToAddRows = false;
            this.dgvGriglia.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvGriglia.Location = new System.Drawing.Point(12, 79);
            this.dgvGriglia.Name = "dgvGriglia";
            this.dgvGriglia.Size = new System.Drawing.Size(1508, 617);
            this.dgvGriglia.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(31, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(30, 18);
            this.label1.TabIndex = 11;
            this.label1.Text = "Dal";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(374, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(20, 18);
            this.label2.TabIndex = 11;
            this.label2.Text = "Al";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(31, 50);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 18);
            this.label3.TabIndex = 11;
            this.label3.Text = "Reparto";
            // 
            // ddlReparto
            // 
            this.ddlReparto.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlReparto.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ddlReparto.FormattingEnabled = true;
            this.ddlReparto.Location = new System.Drawing.Point(108, 49);
            this.ddlReparto.Name = "ddlReparto";
            this.ddlReparto.Size = new System.Drawing.Size(215, 26);
            this.ddlReparto.TabIndex = 12;
            this.ddlReparto.SelectedIndexChanged += new System.EventHandler(this.ddlReparto_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(373, 50);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 18);
            this.label4.TabIndex = 11;
            this.label4.Text = "Fase";
            // 
            // ddlFase
            // 
            this.ddlFase.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlFase.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ddlFase.FormattingEnabled = true;
            this.ddlFase.Location = new System.Drawing.Point(437, 49);
            this.ddlFase.Name = "ddlFase";
            this.ddlFase.Size = new System.Drawing.Size(215, 26);
            this.ddlFase.TabIndex = 12;
            // 
            // btnSalva
            // 
            this.btnSalva.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSalva.Location = new System.Drawing.Point(690, 45);
            this.btnSalva.Name = "btnSalva";
            this.btnSalva.Size = new System.Drawing.Size(75, 32);
            this.btnSalva.TabIndex = 5;
            this.btnSalva.Text = "Salva";
            this.btnSalva.UseVisualStyleBackColor = true;
            this.btnSalva.Click += new System.EventHandler(this.btnSalva_Click);
            // 
            // btnEsporta
            // 
            this.btnEsporta.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEsporta.Location = new System.Drawing.Point(785, 45);
            this.btnEsporta.Name = "btnEsporta";
            this.btnEsporta.Size = new System.Drawing.Size(94, 32);
            this.btnEsporta.TabIndex = 5;
            this.btnEsporta.Text = "Esporta...";
            this.btnEsporta.UseVisualStyleBackColor = true;
            this.btnEsporta.Click += new System.EventHandler(this.btnEsporta_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1532, 756);
            this.Controls.Add(this.dgvGriglia);
            this.Controls.Add(this.ddlFase);
            this.Controls.Add(this.ddlReparto);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.btnEsporta);
            this.Controls.Add(this.btnSalva);
            this.Controls.Add(this.btnTrova);
            this.Controls.Add(this.dtAl);
            this.Controls.Add(this.dtDal);
            this.Name = "Form1";
            this.Text = "Pianificazione";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvGriglia)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnTrova;
        private System.Windows.Forms.DateTimePicker dtAl;
        private System.Windows.Forms.DateTimePicker dtDal;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.DataGridView dgvGriglia;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox ddlReparto;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox ddlFase;
        private System.Windows.Forms.Button btnSalva;
        private System.Windows.Forms.Button btnEsporta;
    }
}

