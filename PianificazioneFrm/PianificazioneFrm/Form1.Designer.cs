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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.rdPerData = new System.Windows.Forms.RadioButton();
            this.rdPerCommessa = new System.Windows.Forms.RadioButton();
            this.txCommessa = new System.Windows.Forms.TextBox();
            this.lblMessage = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnTrova
            // 
            this.btnTrova.Location = new System.Drawing.Point(641, 23);
            this.btnTrova.Name = "btnTrova";
            this.btnTrova.Size = new System.Drawing.Size(75, 23);
            this.btnTrova.TabIndex = 5;
            this.btnTrova.Text = "Trova";
            this.btnTrova.UseVisualStyleBackColor = true;
            this.btnTrova.Click += new System.EventHandler(this.btnTrova_Click);
            // 
            // dtAl
            // 
            this.dtAl.Location = new System.Drawing.Point(355, 24);
            this.dtAl.Name = "dtAl";
            this.dtAl.Size = new System.Drawing.Size(200, 20);
            this.dtAl.TabIndex = 4;
            // 
            // dtDal
            // 
            this.dtDal.Location = new System.Drawing.Point(123, 24);
            this.dtDal.Name = "dtDal";
            this.dtDal.Size = new System.Drawing.Size(200, 20);
            this.dtDal.TabIndex = 3;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 85);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 656F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1508, 656);
            this.tableLayoutPanel1.TabIndex = 6;
            // 
            // rdPerData
            // 
            this.rdPerData.AutoSize = true;
            this.rdPerData.Checked = true;
            this.rdPerData.Location = new System.Drawing.Point(12, 26);
            this.rdPerData.Name = "rdPerData";
            this.rdPerData.Size = new System.Drawing.Size(65, 17);
            this.rdPerData.TabIndex = 7;
            this.rdPerData.Text = "Per data";
            this.rdPerData.UseVisualStyleBackColor = true;
            // 
            // rdPerCommessa
            // 
            this.rdPerCommessa.AutoSize = true;
            this.rdPerCommessa.Location = new System.Drawing.Point(12, 62);
            this.rdPerCommessa.Name = "rdPerCommessa";
            this.rdPerCommessa.Size = new System.Drawing.Size(94, 17);
            this.rdPerCommessa.TabIndex = 7;
            this.rdPerCommessa.Text = "Per commessa";
            this.rdPerCommessa.UseVisualStyleBackColor = true;
            // 
            // txCommessa
            // 
            this.txCommessa.Location = new System.Drawing.Point(123, 61);
            this.txCommessa.Name = "txCommessa";
            this.txCommessa.Size = new System.Drawing.Size(200, 20);
            this.txCommessa.TabIndex = 8;
            // 
            // lblMessage
            // 
            this.lblMessage.AutoSize = true;
            this.lblMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMessage.ForeColor = System.Drawing.Color.Red;
            this.lblMessage.Location = new System.Drawing.Point(786, 28);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(46, 18);
            this.lblMessage.TabIndex = 9;
            this.lblMessage.Text = "label1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1532, 756);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.txCommessa);
            this.Controls.Add(this.rdPerCommessa);
            this.Controls.Add(this.rdPerData);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.btnTrova);
            this.Controls.Add(this.dtAl);
            this.Controls.Add(this.dtDal);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnTrova;
        private System.Windows.Forms.DateTimePicker dtAl;
        private System.Windows.Forms.DateTimePicker dtDal;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.RadioButton rdPerData;
        private System.Windows.Forms.RadioButton rdPerCommessa;
        private System.Windows.Forms.TextBox txCommessa;
        private System.Windows.Forms.Label lblMessage;
    }
}

