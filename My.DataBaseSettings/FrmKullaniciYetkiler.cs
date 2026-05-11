using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Dapper;
using My.DatabaseSettings.Entites; 
namespace My.DatabaseSettings {
    public partial class FrmKullaniciYetkiler : Form {
        public Guid? KulIdSi = Guid.Empty;
        public Kullanici Kullanici;
        private List<KullaniciYetkilerModel> _list;
        public FrmKullaniciYetkiler() {
            InitializeComponent();
        }
        private void FrmKullaniciYetkiler_Load(object sender, System.EventArgs e) {
            if (Kullanici==null)
            {
                MessageBox.Show("Kullanıcı Seçilmemiş.");
                this.Close();
                return;
            } 
            TxtKullaniciAdi.Text = Kullanici.KullaniciAdi;
            TxtAdi.Text = Kullanici.Adi;
            TxtSoyadi.Text = Kullanici.Soyadi;
            Bagla();
        }
        private void Bagla()
        { 
            string guiEmp = Guid.Empty.ToString();
            string sql = @"     SELECT  COALESCE (IZ.Durum, 0) AS Durum ,YT.Modul, YT.Yetki, YT.Aciklama, 
                YT.Id as YetId ,COALESCE(IZ.KulId, '" + guiEmp + @"') AS KulId  FROM KullaniciYetkiler YT
                LEFT OUTER JOIN   KullaniciIzinler IZ ON(IZ.YetId = YT.Id AND IZ.KulId =  '" + KulIdSi + "')";

            var rs = Ortak.Connection.Query<KullaniciYetkilerModel>(sql); 
            _list = rs.ToList();
            bs.DataSource = _list;
            dataGridView1.DataSource = bs;
            foreach (DataGridViewColumn itm in dataGridView1.Columns) {
                if (itm.DataPropertyName == "KulId" || itm.DataPropertyName == "YetId") {
                    itm.Visible = false;
                } 
                if (itm.DataPropertyName== "Durum")
                {
                    itm.ReadOnly = false;
                }
                else
                {
                    itm.ReadOnly = true;
                }
            }
        }
        private void Kaydet()
        {
            try {
                bs.EndEdit();
                if (_list == null) return;
                var sql = @" IF EXISTS(SELECT * FROM KullaniciIzinler WHERE  KulId=@KulId and YetId=@YetId ) 
                                UPDATE KullaniciIzinler SET Durum=@Durum  WHERE KulId=@KulId and YetId=@YetId ;  
                             ELSE INSERT INTO KullaniciIzinler (Durum,KulId,YetId ) VALUES (@Durum,@KulId,@YetId ) ;";
                foreach (var itm in _list)
                {
                    itm.KulId = KulIdSi;
                }
                Ortak.Connection.Execute(sql, _list);
                Bagla();
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            } 
        }
        private void Sil()
        {
            try {
                if (_list == null) return;
                if (MessageBox.Show("Kaydı Silmek istiyormusunuz?", "Onay", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
                string sql1 = " Delete From KullaniciIzinler  WHERE KulId=@KulId and YetId=@YetId ; "; 
                Ortak.Connection.Execute(sql1, _list); 
                Bagla();
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            }
        }
        private void BtnKaydet_Click(object sender, System.EventArgs e)
        {
            Kaydet();
        }
        private void BtnSil_Click(object sender, System.EventArgs e)
        {
            Sil();
        }
        private void BtnKapat_Click(object sender, System.EventArgs e) {
            this.Close();
        } 
    }
}