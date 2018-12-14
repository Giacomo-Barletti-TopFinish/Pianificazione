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
            this.SuspendLayout();
            // 
            // btnTrova
            // 
            this.btnTrova.Location = new System.Drawing.Point(641, 26);
            this.btnTrova.Name = "btnTrova";
            this.btnTrova.Size = new System.Drawing.Size(75, 23);
            this.btnTrova.TabIndex = 5;
            this.btnTrova.Text = "Trova";
            this.btnTrova.UseVisualStyleBackColor = true;
            this.btnTrova.Click += new System.EventHandler(this.btnTrova_Click);
            // 
            // dtAl
            // 
            this.dtAl.Location = new System.Drawing.Point(318, 25);
            this.dtAl.Name = "dtAl";
            this.dtAl.Size = new System.Drawing.Size(200, 20);
            this.dtAl.TabIndex = 4;
            // 
            // dtDal
            // 
            this.dtDal.Location = new System.Drawing.Point(22, 25);
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
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1532, 756);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.btnTrova);
            this.Controls.Add(this.dtAl);
            this.Controls.Add(this.dtDal);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnTrova;
        private System.Windows.Forms.DateTimePicker dtAl;
        private System.Windows.Forms.DateTimePicker dtDal;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}

