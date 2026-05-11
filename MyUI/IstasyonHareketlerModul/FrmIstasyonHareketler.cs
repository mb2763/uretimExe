using DevExpress.Utils;
using My.Business.Manager;
using My.Entities.IstasyonTakipler;
using My.Kontrol.Formlar;
using System;
using System.Linq;
using System.Windows.Forms;

namespace MyUI.IstasyonModul {
    public partial class FrmIstasyonHareketler : MyFrmListe {

        private IstasyonHareketManager _mng;
        string durumu = "";
        public bool SiparisKodundanFiltrele { get; set; } = false;
        public string SiparisKodu { get; set; } = "";
        public FrmIstasyonHareketler() {
            InitializeComponent();
            EventlerBagla();
        }
        private void EventlerBagla() { 
            this.Load += Frm_Load;
            myView1.MyEventDoubleClickEnter += MyView1_MyEventDoubleClickEnter;
            myView1.FocusedRowChanged += MyView1_FocusedRowChanged; 
        }
        private void Frm_Load(object sender, EventArgs e) {
            _mng = new IstasyonHareketManager(Ortak.DbPro, Ortak.DbMikro);
            TxtTarihi1.Text = DateTime.Now.AddMonths(-1).ToShortDateString();
            TxtTarihi2.Text = "";
            BaglaIstasyonList();
            DurumuAyarla(IstasyonTakipHareketDurumuTuru.Aktif);
            if (SiparisKodundanFiltrele)
            {
                DurumuAyarla(null);
                SiparisFiltreAyarla();
            }
            BtnAra.PerformClick();
        }
        private void Bagla() {
            Cursor.Current = Cursors.WaitCursor;
            string sor = SorguAyarla() + SorguAyarlaTrh();
            if (!string.IsNullOrEmpty(sor)) {
                sor = "where  1 = 1 " + sor;
            }
            var rs = _mng.IstHareketService.GetViewListWhere(sor);
            if (!rs.Success) {
                Cursor.Current = Cursors.Default;
                MesajHata(rs.Message);
                return;
            }
            myGrid1.DataSource = rs.Data;
            myView1.SutunGizle("Id");
            myView1.SutunGizle("UrId");
            myView1.SutunGizle("UrIId");
            myView1.SutunFormat(nameof(IstasyonTakipHareket.KayitTarihi), FormatType.DateTime, "dd.MM.yyyy HH:mm");
            myView1.SutunFormat(nameof(IstasyonTakipHareket.Tarih), FormatType.DateTime, "dd.MM.yyyy HH:mm");
            myView1.SutunCaptionDegistir("SiparisKodu", "IsEmriKodu");
            myGrid1.GridYerlesimYukle();
            if (Ortak.PlKapat) {
                myView1.SutunGizle("Parti");
                myView1.SutunGizle("Lot");
            }
            myGrid2.DataSource = null;
            myGrid3.DataSource = null;
            if (rs.Data.Any()) {
                BaglaDetay(rs.Data.FirstOrDefault().Id);
                BaglaStok(rs.Data.FirstOrDefault().UrIId);
            }

            Cursor.Current = Cursors.Default;
        }
        void SiparisFiltreAyarla()
        {
            durumu = "";
            TxtKodu.Text = SiparisKodu;
            TxtTarihi1.EditValue = Convert.ToDateTime(DateTime.Now.Year + "-01-01");

        }
        private string SorguAyarla() {
            string sor = "";
            string t1 = CmbIstasyon.Text.Trim();
            if (!string.IsNullOrEmpty(t1)) { sor += $" AND IstasyonKodu='{t1}' "; }
            t1 = durumu.Trim();
            if (!string.IsNullOrEmpty(t1)) { sor += $" AND Durumu='{t1}' "; }
            t1 = TxtKodu.Text.Trim();
            if (!string.IsNullOrEmpty(t1)) { sor += $" AND SiparisKodu    like('%{t1}%')"; }


            return sor;
        }
        private string SorguAyarlaTrh() {
            string sor = "";
            var t1 = TxtTarihi1.Text.Trim();
            if (!string.IsNullOrEmpty(t1)) {
                t1 = TarihAyarla(Convert.ToDateTime(TxtTarihi1.EditValue));
                if (!string.IsNullOrEmpty(t1)) sor += $"  AND  CAST (coalesce( Tarih,'1901-01-01') AS DATE ) >=  CAST('{t1}'  AS DATE ) ";
            }
            t1 = TxtTarihi2.Text.Trim();
            if (!string.IsNullOrEmpty(t1)) {
                t1 = TarihAyarla(Convert.ToDateTime(TxtTarihi2.EditValue));
                if (!string.IsNullOrEmpty(t1)) sor += $"  AND  CAST (coalesce( Tarih,'1901-01-01') AS DATE ) <=  CAST('{t1}'  AS DATE ) ";
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
        public void BaglaIstasyonList() {

            var rs = _mng.IstKartService.SelectListWhere("");
            if (!rs.Success) {
                MesajHata(rs.Message);
                return;
            }
            var dt = rs.Data;
            foreach (var itm in dt) {
                CmbIstasyon.Items.Add(itm.IstasyonKodu);
            }
        }
        private void BaglaDetay(Guid? IstHrId) {

            string sor = $" where  IstHD.IstHrId='{IstHrId}'  ";
            var rs = _mng.IstHareketDetayService.GetViewListWhere(sor);
            if (!rs.Success) {
                MesajHata(rs.Message);
                return;
            }
            myGrid2.DataSource = rs.Data;
            myView2.SutunGizle("Sec");
            myView2.SutunGizle("Id");
            myView2.SutunGizle("UrId");
            myView2.SutunGizle("UrIId");
            myView2.SutunGizle("IstHrId");
            myView2.SutunFormat("Tarih", DevExpress.Utils.FormatType.DateTime, "dd.MM.yyyy HH:mm");
            myView2.SutunFormat(nameof(IstasyonTakipHareketDetay.Tarih), DevExpress.Utils.FormatType.DateTime, "dd.MM.yyyy HH:mm");
            myView2.SutunFormat(nameof(IstasyonTakipHareketDetay.KayitTarihi), DevExpress.Utils.FormatType.DateTime, "dd.MM.yyyy HH:mm");
            myView2.SutunCaptionDegistir("SiparisKodu", "IsEmriKodu");
            myGrid2.GridYerlesimYukle();
            if (Ortak.PlKapat) {
                myView2.SutunGizle("Parti");
                myView2.SutunGizle("Lot");
            }
        }
        private void BaglaStok(Guid? UrIId) {

            string sor = $" where  UrIId='{UrIId}'  ";
            var rs = _mng.IstHareketStokService.GetStokHareketByUrIId(Guid.Parse(UrIId.ToString()));
            if (!rs.Success) {
                MesajHata(rs.Message);
                return;
            }
            myGrid3.DataSource = rs.Data;
            myView3.SutunGizle("Id");
            myView3.SutunGizle("UrId");
            myView3.SutunGizle("UrIId");
            myView3.SutunGizle("RcAId");
            myView3.SutunGizle("RcDId");
            myView3.SutunGizle("SipId");
            myView3.SutunGizle("SipHId");
            myView3.SutunFormat(nameof(IstasyonTakipStokHareket.KayitTarihi), DevExpress.Utils.FormatType.DateTime, "dd.MM.yyyy HH:mm");
            myGrid3.GridYerlesimYukle();
            if (Ortak.PlKapat) {
                myView3.SutunGizle("Parti");
                myView3.SutunGizle("Lot");
            }
        }


        private void MyView1_MyEventDoubleClickEnter() {
            var _rwid = myView1.FocusedRowHandle;
            var itm = myView1.MyGetCurrentItem<IstasyonTakipHareket>();
            if (itm == null) return;
            if (SecimIcinAcildi) {
                SecilenKod = itm.ReceteKodu;
                SecilenRow = itm;
                SecilenId = itm.Id.ToString();
                Secildi = true;
                this.Close();
                return;
            }


        }
        private void MyView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e) {


            var itm = myView1.MyGetCurrentItem<IstasyonTakipHareket>();
            if (itm != null) {
                BaglaDetay(itm.Id);
                BaglaStok(itm.UrIId);
            }
            else {
                myGrid2.DataSource = null;
                myGrid3.DataSource = null;
            }
        }

        private void BtnAra_Click(object sender, EventArgs e) {
            Bagla();
        }
        private void BtnDurumuHepsi_Click(object sender, EventArgs e) {
            DurumuAyarla(null);
            BtnAra.PerformClick();
        }

        private void BtnDurumuAktif_Click(object sender, EventArgs e) {
            DurumuAyarla(IstasyonTakipHareketDurumuTuru.Aktif);
            BtnAra.PerformClick();
        }

        private void BtnDurumuBeklemede_Click(object sender, EventArgs e) {
            DurumuAyarla(IstasyonTakipHareketDurumuTuru.Beklemede);
            BtnAra.PerformClick();
        }

        private void BtnDurumuDurduruldu_Click(object sender, EventArgs e) {
            DurumuAyarla(IstasyonTakipHareketDurumuTuru.Durduruldu);
            BtnAra.PerformClick();
        }

        private void BtnDurumuBitti_Click(object sender, EventArgs e) {
            DurumuAyarla(IstasyonTakipHareketDurumuTuru.Bitti);
            BtnAra.PerformClick();
        }
        private void DurumuAyarla(IstasyonTakipHareketDurumuTuru? durum) {

            if (durum == null) {
                durumu = "";
                BtnDurumuHepsi.FilterButonRenklendir(true);
                BtnDurumuAktif.FilterButonRenklendir();
                BtnDurumuBeklemede.FilterButonRenklendir();
                BtnDurumuDurduruldu.FilterButonRenklendir();
                BtnDurumuBitti.FilterButonRenklendir();
            }
            else if (durum == IstasyonTakipHareketDurumuTuru.Aktif) {
                durumu = durum.ToString();
                BtnDurumuHepsi.FilterButonRenklendir();
                BtnDurumuAktif.FilterButonRenklendir(true);
                BtnDurumuBeklemede.FilterButonRenklendir();
                BtnDurumuDurduruldu.FilterButonRenklendir();
                BtnDurumuBitti.FilterButonRenklendir();
            }
            else if (durum == IstasyonTakipHareketDurumuTuru.Beklemede) {
                durumu = durum.ToString();
                BtnDurumuHepsi.FilterButonRenklendir();
                BtnDurumuAktif.FilterButonRenklendir();
                BtnDurumuBeklemede.FilterButonRenklendir(true);
                BtnDurumuDurduruldu.FilterButonRenklendir();
                BtnDurumuBitti.FilterButonRenklendir();
            }
            else if (durum == IstasyonTakipHareketDurumuTuru.Durduruldu) {
                durumu = durum.ToString();
                BtnDurumuHepsi.FilterButonRenklendir();
                BtnDurumuAktif.FilterButonRenklendir();
                BtnDurumuBeklemede.FilterButonRenklendir();
                BtnDurumuDurduruldu.FilterButonRenklendir(true);
                BtnDurumuBitti.FilterButonRenklendir();
            }
            else if (durum == IstasyonTakipHareketDurumuTuru.Bitti) {
                durumu = durum.ToString();
                BtnDurumuHepsi.FilterButonRenklendir();
                BtnDurumuAktif.FilterButonRenklendir();
                BtnDurumuBeklemede.FilterButonRenklendir();
                BtnDurumuDurduruldu.FilterButonRenklendir();
                BtnDurumuBitti.FilterButonRenklendir(true);
            }
            else {
                durumu = "";
                BtnDurumuHepsi.FilterButonRenklendir(true);
                BtnDurumuAktif.FilterButonRenklendir();
                BtnDurumuBeklemede.FilterButonRenklendir();
                BtnDurumuDurduruldu.FilterButonRenklendir();
                BtnDurumuBitti.FilterButonRenklendir();
            }
        }

        private void BtnTemizle_Click(object sender, EventArgs e) {
            TxtTarihi1.Text = DateTime.Now.AddMonths(-1).ToShortDateString();
            TxtTarihi2.Text = "";
            CmbIstasyon.Text = "";
            TxtKodu.Text = "";
        }
    }
}
