using System;
using System.Drawing;
using System.Windows.Forms;

namespace My.DataPaneli {
    public partial class MyFrmDataPaneli : Form {
        public bool AyarDegisti = false;
        public string DbAdi = "";
        public MyFrmDataPaneli() {
            InitializeComponent();


        }
        public MyFrmDataPaneli(string dbAdi) {
            InitializeComponent();

            DbAdi = dbAdi;
            EventBagla();
        }
        private bool Tasindi = false;
        private int MouseDownX;
        private int MouseDownY;
        private void EventBagla() {
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmMyGiris_KeyDown);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Lbl_Baslik_MouseDown);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Lbl_Baslik_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.LblBaslik_MouseUp);
            this.lblBaslik.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Lbl_Baslik_MouseDown);
            this.lblBaslik.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Lbl_Baslik_MouseMove);
            this.lblBaslik.MouseUp += new System.Windows.Forms.MouseEventHandler(this.LblBaslik_MouseUp);
            this.pnlButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Lbl_Baslik_MouseDown);
            this.pnlButton.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Lbl_Baslik_MouseMove);
            this.pnlButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.LblBaslik_MouseUp);
            this.pblAlt.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Lbl_Baslik_MouseDown);
            this.pblAlt.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Lbl_Baslik_MouseMove);
            this.pblAlt.MouseUp += new System.Windows.Forms.MouseEventHandler(this.LblBaslik_MouseUp);
            this.BtnKapat.Click += new System.EventHandler(this.BtnKapat_Click);
            this.btnDatabase.Click += new System.EventHandler(this.BtnDatabase_Click);
        }
        private static Color SetTransparency(int A, Color color) {
            return Color.FromArgb(A, color.R, color.G, color.B);
        }
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
            if ((e.Button == MouseButtons.Left)) {
                Tasindi = false;
            }
        }
        private void BtnKapat_Click(object sender, EventArgs e) {
            this.Close();
        }
        private void FrmMyGiris_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Escape) {
                BtnKapat.PerformClick();
            }
            else if (e.KeyCode == Keys.F2) {
                BtnKaydet.PerformClick();
            }
        }
        private void BtnDatabase_Click(object sender, EventArgs e) {
            txtDataBase.Text = @"Veritabanı";
            txtIp.Text = "ServerAdi";
            txtPort.Text = "0";
            txtKullaniciAdi.Text = "sa";
            txtSifre.Text = "";
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
