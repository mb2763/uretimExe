using System;
using System.Windows.Forms;
using My.DatabaseSettings;
namespace My.DataPaneli {
    public partial class FrmDataPanel : Form {
        public FrmDataPanel() {
            InitializeComponent();
        }  
        private void Frm_Load(object sender, EventArgs e)
        {
            FrmLogin f = new FrmLogin();
            f.ShowDialog();
            if (f.GirisYapildi)
            {
                Ortak.GetSettings();
            }
            else
            {
                Application.Exit();
            } 
        }
        private void BtnKapat_Click(object sender, EventArgs e) {
            this.Close();
        }
        private void BtnBaglantiPro_Click(object sender, EventArgs e) {
            FrmDataPaneli frm = new FrmDataPaneli(My.Core.ProgramYolAyarlari.DbProAdi);
            frm.ShowDialog();
        }
        private void BtnBaglantiMikro_Click(object sender, EventArgs e) {
            FrmDataPaneli frm = new FrmDataPaneli(My.Core.ProgramYolAyarlari.DbMikroAdi);
            frm.ShowDialog();
        }
        private void BtnKullaniciAyar_Click(object sender, EventArgs e) {
            FrmKullaniciAyar f = new FrmKullaniciAyar();
            f.ShowDialog();
        }
        private void BtnDatabaseOlusturUretim_Click(object sender, EventArgs e) {
            Ortak.DatabaseGuncelleUretim();
        }
        private void BtnDatabaseOlusturDepoKabul_Click(object sender, EventArgs e) {
            Ortak.DatabaseGuncelleDepoKabul();
        }
        private void BtnDatabaseKontrol_Click(object sender, EventArgs e)
        {
            Ortak.DatabaseControl();
        }
    }
}