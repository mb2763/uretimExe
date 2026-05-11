
namespace MyUI
{
    partial class FrmMasaustu1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmMasaustu1));
            this.panel1 = new System.Windows.Forms.Panel();
            this.frmContext = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.duzenleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.kaydetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ıptalToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.btnContext = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.duzenleToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.kaydetToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.ıptalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.yenidenIsıimverToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.renkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.kaldirToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.myButton1 = new My.Kontrol.Kontroller.MyButton();
            this.panel1.SuspendLayout();
            this.frmContext.SuspendLayout();
            this.btnContext.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(233)))), ((int)(((byte)(236)))), ((int)(((byte)(239)))));
            this.panel1.Controls.Add(this.myButton1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(763, 467);
            this.panel1.TabIndex = 0;
            // 
            // frmContext
            // 
            this.frmContext.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.duzenleToolStripMenuItem,
            this.kaydetToolStripMenuItem,
            this.ıptalToolStripMenuItem1});
            this.frmContext.Name = "frmContext";
            this.frmContext.Size = new System.Drawing.Size(117, 70);
            // 
            // duzenleToolStripMenuItem
            // 
            this.duzenleToolStripMenuItem.Name = "duzenleToolStripMenuItem";
            this.duzenleToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.duzenleToolStripMenuItem.Text = "Duzenle";
            this.duzenleToolStripMenuItem.Click += new System.EventHandler(this.DuzenleToolStripMenuItem_Click);
            // 
            // kaydetToolStripMenuItem
            // 
            this.kaydetToolStripMenuItem.Name = "kaydetToolStripMenuItem";
            this.kaydetToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
            this.kaydetToolStripMenuItem.Text = "Kaydet";
            this.kaydetToolStripMenuItem.Click += new System.EventHandler(this.KaydetToolStripMenuItem_Click);
            // 
            // ıptalToolStripMenuItem1
            // 
            this.ıptalToolStripMenuItem1.Name = "ıptalToolStripMenuItem1";
            this.ıptalToolStripMenuItem1.Size = new System.Drawing.Size(116, 22);
            this.ıptalToolStripMenuItem1.Text = "Iptal";
            this.ıptalToolStripMenuItem1.Click += new System.EventHandler(this.IptalToolStripMenuItem1_Click);
            // 
            // btnContext
            // 
            this.btnContext.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.duzenleToolStripMenuItem1,
            this.kaydetToolStripMenuItem1,
            this.ıptalToolStripMenuItem,
            this.toolStripSeparator1,
            this.yenidenIsıimverToolStripMenuItem,
            this.renkToolStripMenuItem,
            this.kaldirToolStripMenuItem});
            this.btnContext.Name = "btnContext";
            this.btnContext.Size = new System.Drawing.Size(155, 142);
            // 
            // duzenleToolStripMenuItem1
            // 
            this.duzenleToolStripMenuItem1.Name = "duzenleToolStripMenuItem1";
            this.duzenleToolStripMenuItem1.Size = new System.Drawing.Size(154, 22);
            this.duzenleToolStripMenuItem1.Text = "Duzenle";
            this.duzenleToolStripMenuItem1.Click += new System.EventHandler(this.DuzenleToolStripMenuItem1_Click);
            // 
            // kaydetToolStripMenuItem1
            // 
            this.kaydetToolStripMenuItem1.Name = "kaydetToolStripMenuItem1";
            this.kaydetToolStripMenuItem1.Size = new System.Drawing.Size(154, 22);
            this.kaydetToolStripMenuItem1.Text = "Kaydet";
            this.kaydetToolStripMenuItem1.Click += new System.EventHandler(this.KaydetToolStripMenuItem1_Click);
            // 
            // ıptalToolStripMenuItem
            // 
            this.ıptalToolStripMenuItem.Name = "ıptalToolStripMenuItem";
            this.ıptalToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.ıptalToolStripMenuItem.Text = "Iptal";
            this.ıptalToolStripMenuItem.Click += new System.EventHandler(this.IptalToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(151, 6);
            // 
            // yenidenIsıimverToolStripMenuItem
            // 
            this.yenidenIsıimverToolStripMenuItem.Name = "yenidenIsıimverToolStripMenuItem";
            this.yenidenIsıimverToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.yenidenIsıimverToolStripMenuItem.Text = "YenidenIsimver";
            this.yenidenIsıimverToolStripMenuItem.Click += new System.EventHandler(this.YenidenIsimverToolStripMenuItem_Click);
            // 
            // renkToolStripMenuItem
            // 
            this.renkToolStripMenuItem.Name = "renkToolStripMenuItem";
            this.renkToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.renkToolStripMenuItem.Text = "Renk";
            this.renkToolStripMenuItem.Click += new System.EventHandler(this.renkToolStripMenuItem_Click);
            // 
            // kaldirToolStripMenuItem
            // 
            this.kaldirToolStripMenuItem.Name = "kaldirToolStripMenuItem";
            this.kaldirToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.kaldirToolStripMenuItem.Text = "Butonu Kaldır";
            this.kaldirToolStripMenuItem.Click += new System.EventHandler(this.KaldirToolStripMenuItem_Click);
            // 
            // myButton1
            // 
            this.myButton1.Appearance.BackColor = System.Drawing.Color.Black;
            this.myButton1.Appearance.Font = new System.Drawing.Font("Tahoma", 10F);
            this.myButton1.Appearance.ForeColor = System.Drawing.Color.DimGray;
            this.myButton1.Appearance.Options.UseBackColor = true;
            this.myButton1.Appearance.Options.UseFont = true;
            this.myButton1.Appearance.Options.UseForeColor = true;
            this.myButton1.Location = new System.Drawing.Point(113, 58);
            this.myButton1.LookAndFeel.UseDefaultLookAndFeel = false;
            this.myButton1.Name = "myButton1";
            this.myButton1.Size = new System.Drawing.Size(181, 54);
            this.myButton1.TabIndex = 0;
            this.myButton1.Text = "myButton1";
            // 
            // FrmMasaustu1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(763, 467);
            this.ContextMenuStrip = this.frmContext;
            this.ControlBox = false;
            this.Controls.Add(this.panel1);
            this.FormBorderEffect = DevExpress.XtraEditors.FormBorderEffect.None;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.IconOptions.Icon = ((System.Drawing.Icon)(resources.GetObject("FrmMasaustu1.IconOptions.Icon")));
            this.MinimizeBox = false;
            this.Name = "FrmMasaustu1";
            this.Text = "* Favoriler";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmMasaustu_FormClosing);
            this.Load += new System.EventHandler(this.FrmMasaustu_Load);
            this.panel1.ResumeLayout(false);
            this.frmContext.ResumeLayout(false);
            this.btnContext.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ContextMenuStrip frmContext;
        private System.Windows.Forms.ToolStripMenuItem duzenleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem kaydetToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip btnContext;
        private System.Windows.Forms.ToolStripMenuItem duzenleToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem kaydetToolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem yenidenIsıimverToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem kaldirToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ıptalToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ıptalToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem renkToolStripMenuItem;
        private My.Kontrol.Kontroller.MyButton myButton1;
    }
}