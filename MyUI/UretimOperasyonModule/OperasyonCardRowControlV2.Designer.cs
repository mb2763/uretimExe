
namespace MyUI.UretimOperasyonModule
{
    partial class OperasyonCardRowControlV2
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
            this.LblMiktar = new System.Windows.Forms.Label();
            this.LblSiparisKodu = new System.Windows.Forms.Label();
            this.LblRecete = new System.Windows.Forms.Label();
            this.LblCariUnvani = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // LblMiktar
            // 
            this.LblMiktar.BackColor = System.Drawing.Color.Transparent;
            this.LblMiktar.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.LblMiktar.ForeColor = System.Drawing.Color.White;
            this.LblMiktar.Location = new System.Drawing.Point(125, 0);
            this.LblMiktar.Name = "LblMiktar";
            this.LblMiktar.Size = new System.Drawing.Size(50, 20);
            this.LblMiktar.TabIndex = 2;
            this.LblMiktar.Text = "Miktar";
            this.LblMiktar.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.LblMiktar.Click += new System.EventHandler(this.LblRecete_Click);
            this.LblMiktar.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.LblMiktar_MouseDoubleClick);
            // 
            // LblSiparisKodu
            // 
            this.LblSiparisKodu.BackColor = System.Drawing.Color.Transparent;
            this.LblSiparisKodu.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.LblSiparisKodu.ForeColor = System.Drawing.Color.White;
            this.LblSiparisKodu.Location = new System.Drawing.Point(175, 0);
            this.LblSiparisKodu.Name = "LblSiparisKodu";
            this.LblSiparisKodu.Size = new System.Drawing.Size(85, 20);
            this.LblSiparisKodu.TabIndex = 3;
            this.LblSiparisKodu.Text = "SiparişKodu";
            this.LblSiparisKodu.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.LblSiparisKodu.Click += new System.EventHandler(this.LblRecete_Click);
            this.LblSiparisKodu.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.LblMiktar_MouseDoubleClick);
            // 
            // LblRecete
            // 
            this.LblRecete.BackColor = System.Drawing.Color.Transparent;
            this.LblRecete.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.LblRecete.ForeColor = System.Drawing.Color.White;
            this.LblRecete.Location = new System.Drawing.Point(0, 0);
            this.LblRecete.Name = "LblRecete";
            this.LblRecete.Size = new System.Drawing.Size(125, 20);
            this.LblRecete.TabIndex = 4;
            this.LblRecete.Text = "Reçete";
            this.LblRecete.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.LblRecete.Click += new System.EventHandler(this.LblRecete_Click);
            this.LblRecete.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.LblMiktar_MouseDoubleClick);
            // 
            // LblCariUnvani
            // 
            this.LblCariUnvani.BackColor = System.Drawing.Color.Transparent;
            this.LblCariUnvani.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.LblCariUnvani.ForeColor = System.Drawing.Color.White;
            this.LblCariUnvani.Location = new System.Drawing.Point(260, 0);
            this.LblCariUnvani.Name = "LblCariUnvani";
            this.LblCariUnvani.Size = new System.Drawing.Size(175, 20);
            this.LblCariUnvani.TabIndex = 3;
            this.LblCariUnvani.Text = "Unvani";
            this.LblCariUnvani.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.LblCariUnvani.Click += new System.EventHandler(this.LblRecete_Click);
            this.LblCariUnvani.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.LblMiktar_MouseDoubleClick);
            // 
            // OperasyonCardRowControlV2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.LblMiktar);
            this.Controls.Add(this.LblCariUnvani);
            this.Controls.Add(this.LblSiparisKodu);
            this.Controls.Add(this.LblRecete);
            this.Name = "OperasyonCardRowControlV2";
            this.Size = new System.Drawing.Size(435, 20);
            this.Load += new System.EventHandler(this.OperasyonCardRowControl_Load);
            this.Enter += new System.EventHandler(this.LblRecete_Click);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label LblMiktar;
        private System.Windows.Forms.Label LblSiparisKodu;
        private System.Windows.Forms.Label LblRecete;
        private System.Windows.Forms.Label LblCariUnvani;
    }
}
