
namespace MyUI
{
    partial class FrmLogin
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmLogin));
            this.LblClose = new My.Kontrol.Kontroller.MyLabel();
            this.LblVersiyon = new My.Kontrol.Kontroller.MyLabel();
            this.pblAlt.SuspendLayout();
            this.SuspendLayout();
            // 
            // TxtSifre
            // 
            this.TxtSifre.Location = new System.Drawing.Point(155, 325);
            this.TxtSifre.Size = new System.Drawing.Size(125, 25);
            this.TxtSifre.Tag = "Şifre";
            this.TxtSifre.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // TxtKullanici
            // 
            this.TxtKullanici.Location = new System.Drawing.Point(155, 291);
            this.TxtKullanici.Size = new System.Drawing.Size(125, 25);
            this.TxtKullanici.Tag = "Kullanıcı Adı";
            this.TxtKullanici.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // pnlButton
            // 
            this.pnlButton.BackColor = System.Drawing.Color.Transparent;
            this.pnlButton.Location = new System.Drawing.Point(0, 387);
            this.pnlButton.Size = new System.Drawing.Size(427, 12);
            this.pnlButton.Visible = false;
            // 
            // lbl_bilgi
            // 
            this.lbl_bilgi.Location = new System.Drawing.Point(253, 0);
            this.lbl_bilgi.Size = new System.Drawing.Size(174, 17);
            this.lbl_bilgi.Text = "Cep Patron";
            // 
            // pblAlt
            // 
            this.pblAlt.Location = new System.Drawing.Point(0, 399);
            this.pblAlt.Size = new System.Drawing.Size(427, 17);
            this.pblAlt.Visible = false;
            // 
            // lblBaslik
            // 
            this.lblBaslik.Size = new System.Drawing.Size(427, 10);
            this.lblBaslik.Text = "";
            this.lblBaslik.Visible = false;
            // 
            // LblClose
            // 
            this.LblClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.LblClose.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(152)))), ((int)(((byte)(255)))));
            this.LblClose.Appearance.BorderColor = System.Drawing.Color.Gainsboro;
            this.LblClose.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.LblClose.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(94)))), ((int)(((byte)(94)))), ((int)(((byte)(84)))));
            this.LblClose.Appearance.Options.UseBackColor = true;
            this.LblClose.Appearance.Options.UseBorderColor = true;
            this.LblClose.Appearance.Options.UseFont = true;
            this.LblClose.Appearance.Options.UseForeColor = true;
            this.LblClose.AppearanceHovered.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(102)))), ((int)(((byte)(255)))));
            this.LblClose.AppearanceHovered.Options.UseBackColor = true;
            this.LblClose.ImageOptions.SvgImage = ((DevExpress.Utils.Svg.SvgImage)(resources.GetObject("LblClose.ImageOptions.SvgImage")));
            this.LblClose.Location = new System.Drawing.Point(391, 3);
            this.LblClose.Margin = new System.Windows.Forms.Padding(0);
            this.LblClose.Name = "LblClose";
            this.LblClose.Size = new System.Drawing.Size(32, 32);
            this.LblClose.TabIndex = 64;
            this.LblClose.Click += new System.EventHandler(this.LblClose_Click);
            // 
            // LblVersiyon
            // 
            this.LblVersiyon.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(18)))), ((int)(((byte)(153)))), ((int)(((byte)(243)))));
            this.LblVersiyon.Appearance.BorderColor = System.Drawing.Color.Gainsboro;
            this.LblVersiyon.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.LblVersiyon.Appearance.ForeColor = System.Drawing.Color.Gainsboro;
            this.LblVersiyon.Appearance.Options.UseBackColor = true;
            this.LblVersiyon.Appearance.Options.UseBorderColor = true;
            this.LblVersiyon.Appearance.Options.UseFont = true;
            this.LblVersiyon.Appearance.Options.UseForeColor = true;
            this.LblVersiyon.Location = new System.Drawing.Point(12, 356);
            this.LblVersiyon.Name = "LblVersiyon";
            this.LblVersiyon.Size = new System.Drawing.Size(20, 16);
            this.LblVersiyon.TabIndex = 65;
            this.LblVersiyon.Text = "V:0";
            // 
            // FrmLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(427, 416);
            this.Controls.Add(this.LblVersiyon);
            this.Controls.Add(this.LblClose);
            this.FormOlusturuldu = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FrmLogin";
            this.Text = "Giriş";
            this.Controls.SetChildIndex(this.pblAlt, 0);
            this.Controls.SetChildIndex(this.pnlButton, 0);
            this.Controls.SetChildIndex(this.lblBaslik, 0);
            this.Controls.SetChildIndex(this.TxtKullanici, 0);
            this.Controls.SetChildIndex(this.TxtSifre, 0);
            this.Controls.SetChildIndex(this.LblClose, 0);
            this.Controls.SetChildIndex(this.LblVersiyon, 0);
            this.pblAlt.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

   
        private My.Kontrol.Kontroller.MyLabel LblClose;
        private My.Kontrol.Kontroller.MyLabel LblVersiyon;
    }
}