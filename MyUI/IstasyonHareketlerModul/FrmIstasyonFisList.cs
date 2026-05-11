using My.Business.Service.IstasyonTakipler;
using My.Entities.IstasyonTakipler;
using My.Kontrol.Formlar;
using MyUI.MikroModul;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MyUI.IstasyonModul {
    public partial class FrmIstasyonFisList : MyFrmListe {
        private readonly IIstasyonTakipHareketDetayService _srv = Ortak.DbPro.IstasyonTakipHareketDetay;
        private List<IstasyonTakipHareketDetay> _list;
        string aktarim = "";

        public FrmIstasyonFisList() {
            InitializeComponent();
            this.Load += Frm_Load;
        }
        private void Frm_Load(object sender, EventArgs e) {
            TxtTarihi1.Text = DateTime.Now.AddMonths(-1).ToShortDateString();
            TxtTarihi2.Text = "";
            BtnAra.Click += BtnAra_Click;
            BtnTemizle.Click += BtnTemizle_Click;
            myView1.MyEventDoubleClickEnter += MyView1_MyEventDoubleClickEnter;
            Bagla();
            ContexMenuyeEkle();
            SutunGizle();
            myGrid1.GridYerlesimYukle(); 
            if (Ortak.PlKapat) {
                myView1.SutunGizle("Parti");
                myView1.SutunGizle("Lot");
            } 
            myView1.SutunReadOnlyKapat("Sec");
            myView1.SutunEditAc("Sec");
            AktarimAyarla("Bekleyen");
            BtnAra.PerformClick();
        }
        private void SutunGizle() {
            myView1.SutunGizle("Id");
            myView1.SutunGizle("UrId");
            myView1.SutunGizle("UrIId");
            myView1.SutunGizle("IstHrId");
            myView1.SutunFormat("Tarih",DevExpress.Utils.FormatType.DateTime,"dd.MM.yyyy HH:mm");
            if (Ortak.PlKapat) {
                myView1.SutunGizle("Parti");
                myView1.SutunGizle("Lot");
            }
        }
        private void ContexMenuyeEkle() {
            ToolStripMenuItem fm2 = new ToolStripMenuItem("Mikroya Gönder") {
                BackColor = Color.WhiteSmoke,
                ForeColor = Color.Black,
                Text = "Mikroya Gönder",
                TextAlign = ContentAlignment.BottomRight,
                ToolTipText = "Mikroya Gönder"
            };
            fm2.Click += MicroyaGonder_Click;
            myGrid1.MyContextMenu.Items.Add(fm2);
            myGrid1.MyContextMenuAdd(fm2);
        }
        private void Bagla() {
            Cursor.Current = Cursors.WaitCursor;
            string sor = SorguAyarla() + SorguAyarlaTrh();
            if (!string.IsNullOrEmpty(sor)) {
                sor = "where 1=1 " + sor;
            }
            var rs = _srv.GetViewListWhere(sor);
            if (!rs.Success) {
                Cursor.Current = Cursors.Default;
                MesajHata(rs.Message);
                return;
            }
            _list = rs.Data.ToList();
            myGrid1.DataSource = _list;
            if (Ortak.PlKapat) {
                myView1.SutunGizle("Parti");
                myView1.SutunGizle("Lot");
            }
            Cursor.Current = Cursors.Default;
        }
        private string SorguAyarla() {
            string sor = "";
          
            sor = $@" and (IstHD.Turu='{IstasyonTakipHareketDetayTuru.SarfCikisFisi}' or IstHD.Turu='{IstasyonTakipHareketDetayTuru.FireGirisFisi}')";
            string t1 = aktarim.Trim();
            if (!string.IsNullOrEmpty(t1)) {
                if (aktarim == "Aktarilan") {
                    sor += $@" and coalesce(Ent,0) = 1 ";
                }
                else if (aktarim == "Bekleyen") {
                    sor += $@" and coalesce(Ent,0) = 0 ";
                }
                else {  }
            }
            
            return sor;
        }
        private string SorguAyarlaTrh() {
            string sor = "";
            string t1 = TxtTarihi1.Text.Trim();
            if (!string.IsNullOrEmpty(t1)) {
                t1 = TarihAyarla(Convert.ToDateTime(TxtTarihi1.Text));
                if (!string.IsNullOrEmpty(t1)) sor += $"  AND  CAST (coalesce( IstHD.Tarih,'1901-01-01') AS DATE ) >=  CAST('{t1}'  AS DATE ) ";
            }
            t1 = TxtTarihi2.Text.ToString().Trim();
            if (!string.IsNullOrEmpty(t1)) {
                t1 = TarihAyarla(Convert.ToDateTime(TxtTarihi2.Text));
                if (!string.IsNullOrEmpty(t1)) sor += $"  AND  CAST ( coalesce(IstHD.Tarih,'1901-01-01') AS DATE ) <=  CAST('{t1}'  AS DATE ) ";
            }
            return sor;
        }
        private string TarihAyarla(DateTime? T1) {
            if (T1 == null) {  return "";   }
            string FX = T1?.Year + "-" + T1?.Month.ToString().PadLeft(2, '0') + "-" + T1?.Day.ToString().PadLeft(2, '0');
            return FX;
        }
        private void MyView1_MyEventDoubleClickEnter() {
            if (SecimIcinAcildi) {
                var itm = myView1.MyGetCurrentItem<IstasyonTakipHareketDetay>();
                if (itm != null) {
                    SecilenRow = itm;
                    Secildi = true;
                    this.Close();
                }
            }
            else {
                var itm = myView1.MyGetCurrentItem<IstasyonTakipHareketDetay>();
                if (itm != null) {
                   
                }
            }
        }
        private void BtnAra_Click(object sender, EventArgs e) {
            Bagla();
        }
        private void BtnTemizle_Click(object sender, EventArgs e) {
            TxtTarihi1.Text = "";
            TxtTarihi2.Text = "";
        }
        private void MicroyaGonder_Click(object sender, EventArgs e) {
            List<IstasyonTakipHareketDetay> lis = new List<IstasyonTakipHareketDetay>();
            foreach (var itm in _list) {
                if (itm.Sec) {
                    lis.Add(itm.Clone());
                }
            }
            if (lis.Count > 0) {
                FrmMikroyaSarfFireKaydet f = new FrmMikroyaSarfFireKaydet();
                f.FisList = lis;
                f.ShowDialog();
            }
        }
        private void BtnAktarildiTumu_Click(object sender, EventArgs e) {
            AktarimAyarla("Tümü");
            BtnAra.PerformClick();
        } 
        private void BtnAktarildiAktarilan_Click(object sender, EventArgs e) {
            AktarimAyarla("Aktarilan");
            BtnAra.PerformClick();
        } 
        private void BtnAktarildiBekleyen_Click(object sender, EventArgs e) {
            AktarimAyarla("Bekleyen");
            BtnAra.PerformClick();
        }
        private void AktarimAyarla(string durum) {
            if (durum == "Tümü") {
                aktarim = "";
                BtnAktarildiTumu.FilterButonRenklendir(true);
                BtnAktarildiAktarilan.FilterButonRenklendir();
                BtnAktarildiBekleyen.FilterButonRenklendir();
            }
            else if (durum == "Aktarilan") {
                aktarim = "Aktarilan";
                BtnAktarildiTumu.FilterButonRenklendir();
                BtnAktarildiAktarilan.FilterButonRenklendir(true);
                BtnAktarildiBekleyen.FilterButonRenklendir();
            }
            else if (durum == "Bekleyen") {
                aktarim = "Bekleyen";
                BtnAktarildiTumu.FilterButonRenklendir();
                BtnAktarildiAktarilan.FilterButonRenklendir();
                BtnAktarildiBekleyen.FilterButonRenklendir(true);
            }
            else {
                aktarim = "";
                BtnAktarildiTumu.FilterButonRenklendir(true);
                BtnAktarildiAktarilan.FilterButonRenklendir();
                BtnAktarildiBekleyen.FilterButonRenklendir();
            }
        }
    }
}
