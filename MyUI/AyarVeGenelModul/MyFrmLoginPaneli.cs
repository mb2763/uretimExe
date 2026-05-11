using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyUI {
    public partial class MyFrmLoginPaneli : Form {
        

        public bool FormOlusturuldu { get; set; } = false;
        public MyFrmLoginPaneli() {
            InitializeComponent();
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmMyGiris_KeyDown);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Lbl_Baslik_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Lbl_Baslik_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.LblBaslik_MouseUp);
            this.lblBaslik.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Lbl_Baslik_MouseDown);
            this.lblBaslik.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Lbl_Baslik_MouseMove);
            this.lblBaslik.MouseUp += new System.Windows.Forms.MouseEventHandler(this.LblBaslik_MouseUp);
            this.lbl_bilgi.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Lbl_Baslik_MouseDown);
            this.lbl_bilgi.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Lbl_Baslik_MouseMove);
            this.lbl_bilgi.MouseUp += new System.Windows.Forms.MouseEventHandler(this.LblBaslik_MouseUp);
            this.pblAlt.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Lbl_Baslik_MouseDown);
            this.pblAlt.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Lbl_Baslik_MouseMove);
            this.pblAlt.MouseUp += new System.Windows.Forms.MouseEventHandler(this.LblBaslik_MouseUp);
            this.BtnKapat.Click += new System.EventHandler(this.BtnKapat_Click);
            this.Shown += new System.EventHandler(this.MyFrm_Shown);
        }
        private void FrmMyGiris_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Escape) {
                BtnKapat.PerformClick();
            }
            else if (e.KeyCode == Keys.F2) {
                BtnGiris.PerformClick();
            }
        }
        private void MyFrm_Shown(object sender, EventArgs e) {
            FormOlusturuldu = true;
        }
        private void BtnKapat_Click(object sender, EventArgs e) {
            this.Close();
        }
        private bool Tasindi = false;
        private int MouseDownX;
        private int MouseDownY;
        private void Lbl_Baslik_MouseDown(object sender, MouseEventArgs e) {
            if ((e.Button == MouseButtons.Left)) {
                Tasindi = true;
                MouseDownX = e.X;
                MouseDownY = e.Y;
            }
        }
        private void Lbl_Baslik_MouseMove(object sender, MouseEventArgs e) {
            if (Tasindi) {
                Point temp = new Point((this.Location.X + (e.X - MouseDownX)), (this.Location.Y + (e.Y - MouseDownY)));
                this.Location = temp;
                //    temp = null;
            }
        }
        private void LblBaslik_MouseUp(object sender, MouseEventArgs e) {
            if (e.Button == MouseButtons.Left) {
                Tasindi = false;
            }
        }
        private void Me_KeyDown(object sender, KeyEventArgs e) {
        }
        public static void MesajHata(string Hata) {
            MessageBox.Show(Hata, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
        }
        public static void MesajBilgi(string Hata) {
            MessageBox.Show(Hata, "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
        }
        public static bool MesajSor(string mesaj) {
            DialogResult kontrol = new DialogResult();
            kontrol = MessageBox.Show(mesaj, "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            return kontrol == DialogResult.Yes ? true : false;
        }

        public static string GetKontrol(Action action) {
            try {
                action.Invoke();
            }
            catch (Exception e) {
                string hata = "Hata :" + e.Message;
                MesajHata(hata);
            }
            return "";
        }

       
       
    }

}
