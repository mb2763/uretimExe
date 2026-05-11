using System;
using MyUI.Properties;
using MyUI.SiparisModule;
namespace MyUI.MyControl
{
    partial class SiparisPanelControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SiparisPanelControl));
            this.PnlOkBtn = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.PnlOrta = new System.Windows.Forms.FlowLayoutPanel();
            this.BtnTamam = new My.Kontrol.Kontroller.MyButton();
            this.siparisControl1 = new MyUI.SiparisModule.SiparisControl();
            this.siparisControl2 = new MyUI.SiparisModule.SiparisControl();
            this.PnlOkBtn.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.PnlOrta.SuspendLayout();
            this.SuspendLayout();
            // 
            // PnlOkBtn
            // 
            this.PnlOkBtn.Controls.Add(this.groupBox1);
            this.PnlOkBtn.Dock = System.Windows.Forms.DockStyle.Right;
            this.PnlOkBtn.Location = new System.Drawing.Point(887, 0);
            this.PnlOkBtn.Name = "PnlOkBtn";
            this.PnlOkBtn.Size = new System.Drawing.Size(85, 260);
            this.PnlOkBtn.TabIndex = 0;
            this.PnlOkBtn.Visible = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.BtnTamam);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(85, 260);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            // 
            // PnlOrta
            // 
            this.PnlOrta.AutoScroll = true;
            this.PnlOrta.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.PnlOrta.Controls.Add(this.siparisControl1);
            this.PnlOrta.Controls.Add(this.siparisControl2);
            this.PnlOrta.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PnlOrta.Location = new System.Drawing.Point(0, 0);
            this.PnlOrta.Name = "PnlOrta";
            this.PnlOrta.Size = new System.Drawing.Size(887, 260);
            this.PnlOrta.TabIndex = 6;
            this.PnlOrta.Visible = false;
            this.PnlOrta.WrapContents = false;
            // 
            // BtnTamam
            // 
            this.BtnTamam.Appearance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(165)))), ((int)(((byte)(220)))));
            this.BtnTamam.Appearance.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Bold);
            this.BtnTamam.Appearance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.BtnTamam.Appearance.Options.UseBackColor = true;
            this.BtnTamam.Appearance.Options.UseFont = true;
            this.BtnTamam.Appearance.Options.UseForeColor = true;
            this.BtnTamam.AppearanceDisabled.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(165)))), ((int)(((byte)(220)))));
            this.BtnTamam.AppearanceDisabled.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.BtnTamam.AppearanceDisabled.Options.UseBackColor = true;
            this.BtnTamam.AppearanceDisabled.Options.UseForeColor = true;
            this.BtnTamam.AppearanceHovered.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(165)))), ((int)(((byte)(240)))));
            this.BtnTamam.AppearanceHovered.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.BtnTamam.AppearanceHovered.Options.UseBackColor = true;
            this.BtnTamam.AppearanceHovered.Options.UseForeColor = true;
            this.BtnTamam.AppearancePressed.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.BtnTamam.AppearancePressed.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.BtnTamam.AppearancePressed.Options.UseBackColor = true;
            this.BtnTamam.AppearancePressed.Options.UseForeColor = true;
            this.BtnTamam.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("BtnTamam.ImageOptions.Image")));
            this.BtnTamam.Location = new System.Drawing.Point(4, 3);
            this.BtnTamam.LookAndFeel.SkinName = "DevExpress Style";
            this.BtnTamam.LookAndFeel.UseDefaultLookAndFeel = false;
            this.BtnTamam.Name = "BtnTamam";
            this.BtnTamam.Size = new System.Drawing.Size(75, 231);
            this.BtnTamam.TabIndex = 2;
            this.BtnTamam.Text = "Onay";
            this.BtnTamam.Click += new System.EventHandler(this.BtnTamam_Click);
            // 
            // siparisControl1
            // 
            this.siparisControl1.Aciklama = null;
            this.siparisControl1.Beden = null;
            this.siparisControl1.Birim = null;
            this.siparisControl1.Cinsi = null;
            this.siparisControl1.Id = new System.Guid("00000000-0000-0000-0000-000000000000");
            this.siparisControl1.Location = new System.Drawing.Point(3, 3);
            this.siparisControl1.Miktar = 0D;
            this.siparisControl1.Name = "siparisControl1";
            this.siparisControl1.RcAId = new System.Guid("00000000-0000-0000-0000-000000000000");
            this.siparisControl1.RcDId = new System.Guid("00000000-0000-0000-0000-000000000000");
            this.siparisControl1.Renk = null;
            this.siparisControl1.SipHId = new System.Guid("00000000-0000-0000-0000-000000000000");
            this.siparisControl1.SipId = new System.Guid("00000000-0000-0000-0000-000000000000");
            this.siparisControl1.Size = new System.Drawing.Size(311, 236);
            this.siparisControl1.StokAdi = null;
            this.siparisControl1.StokBedenler = null;
            this.siparisControl1.StokKodu = null;
            this.siparisControl1.StokKullan = false;
            this.siparisControl1.Stoklar = null;
            this.siparisControl1.StokRenkler = null;
            this.siparisControl1.TabIndex = 0;
            // 
            // siparisControl2
            // 
            this.siparisControl2.Aciklama = null;
            this.siparisControl2.Beden = null;
            this.siparisControl2.Birim = null;
            this.siparisControl2.Cinsi = null;
            this.siparisControl2.Id = new System.Guid("00000000-0000-0000-0000-000000000000");
            this.siparisControl2.Location = new System.Drawing.Point(320, 3);
            this.siparisControl2.Miktar = 0D;
            this.siparisControl2.Name = "siparisControl2";
            this.siparisControl2.RcAId = new System.Guid("00000000-0000-0000-0000-000000000000");
            this.siparisControl2.RcDId = new System.Guid("00000000-0000-0000-0000-000000000000");
            this.siparisControl2.Renk = null;
            this.siparisControl2.SipHId = new System.Guid("00000000-0000-0000-0000-000000000000");
            this.siparisControl2.SipId = new System.Guid("00000000-0000-0000-0000-000000000000");
            this.siparisControl2.Size = new System.Drawing.Size(311, 237);
            this.siparisControl2.StokAdi = null;
            this.siparisControl2.StokBedenler = null;
            this.siparisControl2.StokKodu = null;
            this.siparisControl2.StokKullan = false;
            this.siparisControl2.Stoklar = null;
            this.siparisControl2.StokRenkler = null;
            this.siparisControl2.TabIndex = 1;
            // 
            // SiparisPanelControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.PnlOrta);
            this.Controls.Add(this.PnlOkBtn);
            this.Name = "SiparisPanelControl";
            this.Size = new System.Drawing.Size(972, 260);
            this.Load += new System.EventHandler(this.SiparisPanelControl_Load);
            this.PnlOkBtn.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.PnlOrta.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Panel PnlOkBtn;
        public System.Windows.Forms.FlowLayoutPanel PnlOrta;
        public SiparisControl siparisControl1;
        public SiparisControl siparisControl2;
        private System.Windows.Forms.GroupBox groupBox1;
        private My.Kontrol.Kontroller.MyButton BtnTamam;
    }
}
