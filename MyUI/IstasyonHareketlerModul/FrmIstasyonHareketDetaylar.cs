using My.Business.Service.Geneller;
using My.Business.Service.IstasyonKartlar;
using My.Business.Service.IstasyonTakipler;
using My.Entities.IstasyonTakipler;
using My.Kontrol.Formlar;
using MyUI.IstasyonHareketlerModul;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace MyUI.IstasyonModul {
    public partial class FrmIstasyonHareketDetaylar : MyFrmListe {

        private readonly IIstasyonTakipHareketDetayService _srv = Ortak.DbPro.IstasyonTakipHareketDetay;
        private readonly IIstasyonKartiService _srvKrt = Ortak.DbPro.IstasyonKarti;
        private readonly IGenelService _srvGnl = Ortak.DbPro.GenelServis;
        public string SiparisKodu { get; set; } = "";
        private List<IstasyonTakipHareketDetay> _list;
   
        public FrmIstasyonHareketDetaylar() {
            InitializeComponent();
            this.Load += Frm_Load;
        }
        private void Frm_Load(object sender, EventArgs e) {
            TxtTarihi1.Text = DateTime.Now.AddMonths(-1).ToShortDateString();
            TxtTarihi2.Text = "";
            TxtSaat1.Text = "00:00:00";
            TxtSaat2.Text = "23:59:59";
            BtnAra.Click += BtnAra_Click;
            BtnTemizle.Click += BtnTemizle_Click;
            myView1.MyEventDoubleClickEnter += MyView1_MyEventDoubleClickEnter;
            BaglaIstasyonList();
            BaglaTuruList();
            if (!string.IsNullOrEmpty(SiparisKodu)) {
                TxtKodu.Text = SiparisKodu;
            }
            Bagla();

            SutunGizle();
            myGrid1.GridYerlesimYukle();
            if (Ortak.PlKapat) {
                myView1.SutunGizle("Parti");
                myView1.SutunGizle("Lot");
            }
        }
        private void SutunGizle() {
            myView1.SutunGizle("Sec");
            myView1.SutunGizle("Id");
            myView1.SutunGizle("UrId");
            myView1.SutunGizle("UrIId");
            myView1.SutunGizle("IstHrId");
            myView1.SutunFormat("Tarih", DevExpress.Utils.FormatType.DateTime, "dd.MM.yyyy HH:mm");
            myView1.SutunCaptionDegistir("SiparisKodu", "IsEmriKodu");
            if (Ortak.PlKapat) {
                myView1.SutunGizle("Parti");
                myView1.SutunGizle("Lot");
            }
        }

        private void Bagla() {
            Cursor.Current = Cursors.WaitCursor;
            string sor = SorguAyarla() + SorguAyarlaTrh();
            if (!string.IsNullOrEmpty(sor)) {
                sor = "where 1 = 1 " + sor;
            }
            var rs = _srv.GetViewListWhere(sor);
            if (!rs.Success) {
                Cursor.Current = Cursors.Default;
                MesajHata(rs.Message);
                return;
            }
            _list = rs.Data.ToList();
            myGrid1.DataSource = _list;
            Cursor.Current = Cursors.Default;
        }
        public void BaglaIstasyonList() {

            var rs = _srvKrt.SelectListWhere("");
            if (!rs.Success) {
                MesajHata(rs.Message);
                return;
            }
            var dt = rs.Data;
            foreach (var itm in dt) {
                CmbIstasyon.Items.Add(itm.IstasyonKodu);
            }
        }
        public void BaglaTuruList() {

            var rs = _srvGnl.GrupListesi("IstasyonTakipHareketDetay", "Turu");
            if (!rs.Success) {
                MesajHata(rs.Message);
                return;
            }
            CmbTuru.Items.Clear();
            var dt = rs.Data;
            foreach (var itm in dt) {
                if (itm != IstasyonTakipHareketDetayTuru.SarfCikisFisi.ToString() && itm != IstasyonTakipHareketDetayTuru.FireGirisFisi.ToString()) {
                    CmbTuru.Items.Add(itm);
                }

            }
        }
        private string SorguAyarla() {
            string sor = "";
            string t1 = CmbIstasyon.Text.Trim();
            if (!string.IsNullOrEmpty(t1)) { sor += $" AND IstHr.IstasyonKodu='{t1}' "; }
            t1 = TxtKodu.Text.Trim();
            if (!string.IsNullOrEmpty(t1)) { sor += $" AND IstHr.SiparisKodu like('%{t1}%') "; }
            t1 = CmbTuru.Text.Trim();
            if (!string.IsNullOrEmpty(t1)) { sor += $" AND IstHD.Turu ='{t1}' "; }

            sor += $@" and (IstHD.Turu<>'{IstasyonTakipHareketDetayTuru.SarfCikisFisi}' AND IstHD.Turu <> '{IstasyonTakipHareketDetayTuru.FireGirisFisi}')";

            return sor;
        }
        private string SorguAyarlaTrh() {
            string sor = "";
            string t1 = TxtTarihi1.Text.Trim();
            if (!string.IsNullOrEmpty(t1)) {
                t1 = TarihAyarla(Convert.ToDateTime(TxtTarihi1.Text)) + " "+TxtSaat1.Text;
                if (!string.IsNullOrEmpty(t1)) sor += $"  AND  CAST (coalesce( IstHD.Tarih,'1901-01-01') AS DATETIME ) >=  CAST('{t1}'  AS DATETIME ) ";
            }
            t1 = TxtTarihi2.Text.ToString().Trim();
            if (!string.IsNullOrEmpty(t1)) {
                t1 = TarihAyarla(Convert.ToDateTime(TxtTarihi2.Text)) + " " + TxtSaat2.Text;
                if (!string.IsNullOrEmpty(t1)) sor += $"  AND  CAST ( coalesce(IstHD.Tarih,'1901-01-01') AS DATETIME ) <=  CAST('{t1}'  AS DATETIME ) ";
            }

            return sor;
        }
     
        private string TarihAyarla(DateTime? T1) {
            if (T1 == null) {
                return "";
            }
            string FX = T1?.Year + "-" + T1?.Month.ToString().PadLeft(2, '0') + "-" +
                        T1?.Day.ToString().PadLeft(2, '0');
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
                    FrmIstasyonHareketDetayED f = new FrmIstasyonHareketDetayED();
                    f.Model = itm;
                    f.ShowDialog();
                    Bagla();
                }
            }
        }
        private void BtnAra_Click(object sender, EventArgs e) {
            Bagla();
        }
        private void BtnTemizle_Click(object sender, EventArgs e) {

            TxtTarihi1.Text = DateTime.Now.AddMonths(-1).ToShortDateString();
            TxtTarihi2.Text = "";
            CmbIstasyon.Text = "";
            TxtKodu.Text = "";
            TxtSaat1.Text = "00:00:00";
            TxtSaat2.Text = "23:59:59";
        }

    }
}
