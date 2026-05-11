namespace MyUI.AyarVeGenelModul {
    partial class FrmStokKodGuncelle {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
                }
            base.Dispose(disposing);
            }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.label1 = new System.Windows.Forms.Label();
            this.txtEskiStokKod = new System.Windows.Forms.TextBox();
            this.txtYeniStokkod = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnKaydet = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(35, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Eski Stok Kodu";
            // 
            // txtEskiStokKod
            // 
            this.txtEskiStokKod.Location = new System.Drawing.Point(121, 27);
            this.txtEskiStokKod.Name = "txtEskiStokKod";
            this.txtEskiStokKod.Size = new System.Drawing.Size(193, 20);
            this.txtEskiStokKod.TabIndex = 1;
            // 
            // txtYeniStokkod
            // 
            this.txtYeniStokkod.Location = new System.Drawing.Point(121, 53);
            this.txtYeniStokkod.Name = "txtYeniStokkod";
            this.txtYeniStokkod.Size = new System.Drawing.Size(193, 20);
            this.txtYeniStokkod.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(35, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(81, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Yeni Stok Kodu";
            // 
            // btnKaydet
            // 
            this.btnKaydet.Location = new System.Drawing.Point(239, 79);
            this.btnKaydet.Name = "btnKaydet";
            this.btnKaydet.Size = new System.Drawing.Size(75, 23);
            this.btnKaydet.TabIndex = 4;
            this.btnKaydet.Text = "Kaydet";
            this.btnKaydet.UseVisualStyleBackColor = true;
            this.btnKaydet.Click += new System.EventHandler(this.btnKaydet_Click);
            // 
            // FrmStokKodGuncelle
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(388, 127);
            this.Controls.Add(this.btnKaydet);
            this.Controls.Add(this.txtYeniStokkod);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtEskiStokKod);
            this.Controls.Add(this.label1);
            this.Name = "FrmStokKodGuncelle";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Stok Kodu Güncelleme";
            this.ResumeLayout(false);
            this.PerformLayout();

            }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtEskiStokKod;
        private System.Windows.Forms.TextBox txtYeniStokkod;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnKaydet;
        }
    }