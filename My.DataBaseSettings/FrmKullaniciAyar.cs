using Dapper;
using My.Core;
using My.DatabaseSettings.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
namespace My.DatabaseSettings {
    public partial class FrmKullaniciAyar : Form {
        private List<Kullanici> _list;
        private Kullanici _mdl;
        public FrmKullaniciAyar() {
            InitializeComponent();
        }
        private void FrmKullaniciAyar_Load(object sender, System.EventArgs e) {
            Bagla();
        }
        private void Bagla() {
            try {
                string sql = " Select * from Kullanici ;";
                var rs = Ortak.Connection.Query<Kullanici>(sql);
                _list = rs.ToList();
                bs.DataSource = _list;
                dataGridView1.DataSource = bs;
                foreach (DataGridViewColumn itm in dataGridView1.Columns) {
                    if (itm.DataPropertyName == "Id" || itm.DataPropertyName == "Sifre") {
                        itm.Visible = false;
                    }

                    itm.ReadOnly = true;
                }
                TexteleriTemizle();
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }
        private void Duzenle() {
            var rw = bs.GetCurrentRow<Kullanici>();
            if (rw == null) return;
            _mdl = rw.Clone();
            TexteAktar();
        }
        private void TexteAktar() {
            TxtKullaniciAdi.Text = _mdl.KullaniciAdi;
            TxtSifre.Text = _mdl.Sifre.SifreCoz(Ortak.ANA_KEY);
            TxtAdi.Text = _mdl.Adi;
            TxtSoyadi.Text = _mdl.Soyadi;
            ChcAdmin.Checked = _mdl.Admin;
        }
        private void TexteleriTemizle() {
            TxtKullaniciAdi.Text = "";
            TxtSifre.Text =  "";
            TxtAdi.Text =   "";
            TxtSoyadi.Text =  "";
            ChcAdmin.Checked = false;
        }
        private void RowaAktar() {
            _mdl.KullaniciAdi = TxtKullaniciAdi.Text.Trim();
            _mdl.Sifre = TxtSifre.Text.Trim().Sifrele(Ortak.ANA_KEY);
            _mdl.Adi = TxtAdi.Text.Trim();
            _mdl.Soyadi = TxtSoyadi.Text.Trim();
            _mdl.Admin = ChcAdmin.Checked;
        }
        private void dataGridView1_DoubleClick(object sender, EventArgs e) {
            Duzenle();
        }
        private void BtnKapat_Click(object sender, EventArgs e) {
            this.Close();
        }
        private void BtnKaydet_Click(object sender, EventArgs e) {
            try {
                if (_mdl == null) {
                    MessageBox.Show("Kayıt Seçilmemiş");
                    return;
                }
                var sql = @" IF EXISTS(SELECT * FROM Kullanici WHERE  Id=@Id ) 
                           UPDATE Kullanici SET  Adi=@Adi, Soyadi=@Soyadi,KullaniciAdi=@KullaniciAdi,
                           Sifre=@Sifre,Admin=@Admin  WHERE   Id=@Id ;  
                        ELSE  INSERT INTO   Kullanici  (Adi,Soyadi,KullaniciAdi,Sifre,Admin) 
                                  VALUES (@Adi,@Soyadi,@KullaniciAdi,@Sifre,@Admin) ;";
                RowaAktar();
                Ortak.Connection.Execute(sql, _mdl);
                Bagla();
                _mdl = null;
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }
        private void BtnDegistir_Click(object sender, EventArgs e) {
            Duzenle();
        }
        private void BtnYeni_Click(object sender, EventArgs e) {
            _mdl = new Kullanici();
            TexteAktar();
        }
        private void BtnSil_Click(object sender, EventArgs e) {
            try {
                if (_mdl == null) return;
                if (MessageBox.Show("Kaydı Silmek istiyormusunuz?", "Onay", MessageBoxButtons.YesNo) !=
                    DialogResult.Yes) return;
                string sql1 = " Delete From KullaniciIzinler where KulId = " + _mdl.Id + ";";
                string sql0 = " Delete From Kullanici        where Id    = " + _mdl.Id + ";";
                Ortak.Connection.Execute(sql1);
                Ortak.Connection.Execute(sql0);
                Bagla();
                _mdl = null;
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }
        private void BtnYetkiler_Click(object sender, EventArgs e) {
            var rw = bs.GetCurrentRow<Kullanici>();
            if (rw == null) return;
            FrmKullaniciYetkiler f = new FrmKullaniciYetkiler();
            f.Kullanici = rw;
            f.KulIdSi = rw.Id;
            f.ShowDialog();
        }
    }
}