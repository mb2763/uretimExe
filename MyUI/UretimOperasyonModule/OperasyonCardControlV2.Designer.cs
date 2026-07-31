
namespace MyUI.UretimOperasyonModule
{
    partial class OperasyonCardControlV2
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
            this.components = new System.ComponentModel.Container();
            this.LblOperasyon = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.PnlBekleyen = new System.Windows.Forms.FlowLayoutPanel();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.detaylarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.operasyonDetaylariToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.PnlUretimde = new System.Windows.Forms.FlowLayoutPanel();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.uretimlerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.operasyonDetaylariToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.uretimGirisiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip1.SuspendLayout();
            this.contextMenuStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // LblOperasyon
            // 
            this.LblOperasyon.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(28)))), ((int)(((byte)(60)))), ((int)(((byte)(104)))));
            this.LblOperasyon.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LblOperasyon.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.LblOperasyon.ForeColor = System.Drawing.Color.White;
            this.LblOperasyon.Location = new System.Drawing.Point(3, 3);
            this.LblOperasyon.Name = "LblOperasyon";
            this.LblOperasyon.Size = new System.Drawing.Size(669, 28);
            this.LblOperasyon.TabIndex = 1;
            this.LblOperasyon.Text = "Operasyon";
            this.LblOperasyon.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(58)))), ((int)(((byte)(82)))));
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(4, 34);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(333, 31);
            this.label3.TabIndex = 4;
            this.label3.Text = "Bekleyen";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(58)))), ((int)(((byte)(82)))));
            this.label9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label9.ForeColor = System.Drawing.Color.White;
            this.label9.Location = new System.Drawing.Point(340, 34);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(332, 31);
            this.label9.TabIndex = 6;
            this.label9.Text = "Üretimde";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // PnlBekleyen
            // 
            this.PnlBekleyen.AutoScroll = true;
            this.PnlBekleyen.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(62)))), ((int)(((byte)(98)))));
            this.PnlBekleyen.ContextMenuStrip = this.contextMenuStrip1;
            this.PnlBekleyen.Location = new System.Drawing.Point(4, 90);
            this.PnlBekleyen.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.PnlBekleyen.Name = "PnlBekleyen";
            this.PnlBekleyen.Size = new System.Drawing.Size(333, 211);
            this.PnlBekleyen.TabIndex = 8;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.detaylarToolStripMenuItem,
            this.operasyonDetaylariToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(225, 48);
            // 
            // detaylarToolStripMenuItem
            // 
            this.detaylarToolStripMenuItem.Name = "detaylarToolStripMenuItem";
            this.detaylarToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.detaylarToolStripMenuItem.Text = "Detaylar > İstasyona Gönder";
            this.detaylarToolStripMenuItem.Click += new System.EventHandler(this.DetaylarToolStripMenuItem_Click);
            // 
            // operasyonDetaylariToolStripMenuItem
            // 
            this.operasyonDetaylariToolStripMenuItem.Name = "operasyonDetaylariToolStripMenuItem";
            this.operasyonDetaylariToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
            this.operasyonDetaylariToolStripMenuItem.Text = "Operasyon Hareket Detaylari";
            this.operasyonDetaylariToolStripMenuItem.Click += new System.EventHandler(this.operasyonDetaylariToolStripMenuItem_Click);
            // 
            // PnlUretimde
            // 
            this.PnlUretimde.AutoScroll = true;
            this.PnlUretimde.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(92)))), ((int)(((byte)(72)))));
            this.PnlUretimde.ContextMenuStrip = this.contextMenuStrip2;
            this.PnlUretimde.Location = new System.Drawing.Point(339, 90);
            this.PnlUretimde.Margin = new System.Windows.Forms.Padding(0, 3, 3, 3);
            this.PnlUretimde.Name = "PnlUretimde";
            this.PnlUretimde.Size = new System.Drawing.Size(333, 211);
            this.PnlUretimde.TabIndex = 9;
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.uretimGirisiToolStripMenuItem,
            this.toolStripMenuItem1,
            this.uretimlerToolStripMenuItem,
            this.operasyonDetaylariToolStripMenuItem1});
            this.contextMenuStrip2.Name = "contextMenuStrip1";
            this.contextMenuStrip2.Size = new System.Drawing.Size(232, 114);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(231, 22);
            this.toolStripMenuItem1.Text = "Detaylar > Uretim Girişi";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.ToolStripMenuItem1_Click);
            // 
            // uretimlerToolStripMenuItem
            // 
            this.uretimlerToolStripMenuItem.Name = "uretimlerToolStripMenuItem";
            this.uretimlerToolStripMenuItem.Size = new System.Drawing.Size(231, 22);
            this.uretimlerToolStripMenuItem.Text = " İstasyon Hareketler/Uretimler";
            this.uretimlerToolStripMenuItem.Click += new System.EventHandler(this.UretimlerToolStripMenuItem_Click);
            // 
            // operasyonDetaylariToolStripMenuItem1
            // 
            this.operasyonDetaylariToolStripMenuItem1.Name = "operasyonDetaylariToolStripMenuItem1";
            this.operasyonDetaylariToolStripMenuItem1.Size = new System.Drawing.Size(231, 22);
            this.operasyonDetaylariToolStripMenuItem1.Text = "Operasyon Hareket Detaylari";
            this.operasyonDetaylariToolStripMenuItem1.Click += new System.EventHandler(this.operasyonDetaylariToolStripMenuItem1_Click);
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(62)))), ((int)(((byte)(98)))));
            this.label5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label5.ForeColor = System.Drawing.Color.Silver;
            this.label5.Location = new System.Drawing.Point(129, 65);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(50, 25);
            this.label5.TabIndex = 10;
            this.label5.Text = "Miktar";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(62)))), ((int)(((byte)(98)))));
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label4.ForeColor = System.Drawing.Color.Silver;
            this.label4.Location = new System.Drawing.Point(179, 65);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(85, 25);
            this.label4.TabIndex = 11;
            this.label4.Text = "Sip.Kodu";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(62)))), ((int)(((byte)(98)))));
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label2.ForeColor = System.Drawing.Color.Silver;
            this.label2.Location = new System.Drawing.Point(4, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(125, 25);
            this.label2.TabIndex = 12;
            this.label2.Text = "Reçete";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label13
            // 
            this.label13.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(92)))), ((int)(((byte)(72)))));
            this.label13.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label13.ForeColor = System.Drawing.Color.Silver;
            this.label13.Location = new System.Drawing.Point(464, 65);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(50, 25);
            this.label13.TabIndex = 13;
            this.label13.Text = "Miktar";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label14
            // 
            this.label14.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(92)))), ((int)(((byte)(72)))));
            this.label14.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label14.ForeColor = System.Drawing.Color.Silver;
            this.label14.Location = new System.Drawing.Point(514, 65);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(85, 25);
            this.label14.TabIndex = 14;
            this.label14.Text = "Sip.Kodu";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label15
            // 
            this.label15.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(92)))), ((int)(((byte)(72)))));
            this.label15.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label15.ForeColor = System.Drawing.Color.Silver;
            this.label15.Location = new System.Drawing.Point(339, 65);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(125, 25);
            this.label15.TabIndex = 15;
            this.label15.Text = "Reçete";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(62)))), ((int)(((byte)(98)))));
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label1.ForeColor = System.Drawing.Color.Silver;
            this.label1.Location = new System.Drawing.Point(264, 65);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 25);
            this.label1.TabIndex = 11;
            this.label1.Text = "Unvani";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(92)))), ((int)(((byte)(72)))));
            this.label6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label6.ForeColor = System.Drawing.Color.Silver;
            this.label6.Location = new System.Drawing.Point(599, 65);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(73, 25);
            this.label6.TabIndex = 16;
            this.label6.Text = "Unvani";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // uretimGirisiToolStripMenuItem
            // 
            this.uretimGirisiToolStripMenuItem.Name = "uretimGirisiToolStripMenuItem";
            this.uretimGirisiToolStripMenuItem.Size = new System.Drawing.Size(231, 22);
            this.uretimGirisiToolStripMenuItem.Text = "Direk>Üretim Girişi";
            this.uretimGirisiToolStripMenuItem.Click += new System.EventHandler(this.uretimGirisiToolStripMenuItem_Click);
            // 
            // OperasyonCardControlV2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(40)))), ((int)(((byte)(54)))));
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.PnlUretimde);
            this.Controls.Add(this.PnlBekleyen);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.LblOperasyon);
            this.Name = "OperasyonCardControlV2";
            this.Size = new System.Drawing.Size(675, 305);
            this.Load += new System.EventHandler(this.Card_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.contextMenuStrip2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label LblOperasyon;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.FlowLayoutPanel PnlBekleyen;
        private System.Windows.Forms.FlowLayoutPanel PnlUretimde;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem detaylarToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ToolStripMenuItem uretimlerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem operasyonDetaylariToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem operasyonDetaylariToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem uretimGirisiToolStripMenuItem;
    }
}
