using My.Business.Service.Geneller;
using My.Business.Service.UretimKontroller;
using My.Entities.UretimKontroller;
using My.Kontrol.Formlar;
using My.Kontrol.Yazdirma;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace MyUI.UretimKontroller {
    public partial class FrmOlcuKontrolListesi : MyFrmListe {

        private readonly IGenelService _srvGenel = Ortak.DbPro.GenelServis;
        private readonly IUretimKontrolService _srv = Ortak.DbPro.UretimKontroller;
        public List<UretimKontrol> list { get; set; }

        public FrmOlcuKontrolListesi() {
            InitializeComponent();
            this.Load += Frm_Load;
            myView1.FocusedRowChanged += MyView1_FocusedRowChanged;
            myView1.MyEventDoubleClickEnter += MyView1_MyEventDoubleClickEnter;
        }
        private void Frm_Load(object sender, EventArgs e) {

            TxtTarihi1.EditValue = Convert.ToDateTime("01.01." + DateTime.Now.Year.ToString());
            TxtTarihi1.Text = "01.01." + DateTime.Now.Year.ToString();
            Bagla();
            SutunGizle();
            myGrid1.GridYerlesimYukle();

            BtnAra.PerformClick();
        }

        private void Bagla() {
            var rwId = myView1.FocusedRowHandle;
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
            list = rs.Data.ToList();
            bs.DataSource = list;
            myGrid1.DataSource = bs;
            try {
                myView1.FocusedRowHandle = rwId;
            } catch { }
        }

        private void BtnAra_Click(object sender, EventArgs e) {
            Bagla();
        }
        private void BtnTemizle_Click(object sender, EventArgs e) {
            TxtAra.Text = "";
            TxtTarihi1.Text = "01.01." + DateTime.Now.Year.ToString();
            TxtTarihi2.Text = "";
        }
        private void BtnYazdir_Click(object sender, EventArgs e) {
            Yazdir();
        }
        private void MyView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e) {
            var itm = myView1.MyGetCurrentItem<UretimKontrol>();
            if (itm != null) {
                //  BaglaHareket(itm.Id);
            }
        }
        private void MyView1_MyEventDoubleClickEnter() {
            var itm = myView1.MyGetCurrentItem<UretimKontrol>();
            if (itm == null) return;
            if (SecimIcinAcildi) {
                SecilenRow = itm;
                Secildi = true;
                this.Close();
            }
            else {
            }
        }
        private string SorguAyarla() {
            // UretimStokFis UF UretimEmri UR  UretimIstasyon UrI
            string sor = "";
            string t1 = " and UK.Turu='OlcumGiris' ";
            t1 = TxtAra.Text.Trim();
            if (!string.IsNullOrEmpty(t1)) { sor += $@" AND  (UK.IstasyonKodu   like('%{t1}%') OR UK.IstasyonAdi   like('%{t1}%')  
                OR UR.IsEmriNo   like('%{t1}%') OR UR.SiparisKodu   like('%{t1}%')  
                )"; } 
            return sor;
        }
        private string SorguAyarlaTrh() {
            string sor = "";
            var t1 = TxtTarihi1.Text.Trim();
            if (!string.IsNullOrEmpty(t1)) {
                t1 = TarihAyarla(Convert.ToDateTime(TxtTarihi1.EditValue));
                if (!string.IsNullOrEmpty(t1)) sor += $"  AND  CAST (coalesce( UK.Tarih,'1901-01-01') AS DATE ) >=  CAST('{t1}'  AS DATE ) ";
            }
            t1 = TxtTarihi2.Text.Trim();
            if (!string.IsNullOrEmpty(t1)) {
                t1 = TarihAyarla(Convert.ToDateTime(TxtTarihi2.EditValue));
                if (!string.IsNullOrEmpty(t1)) sor += $"  AND  CAST (coalesce( UK.Tarih,'1901-01-01') AS DATE ) <=  CAST('{t1}'  AS DATE ) ";
            }
            return sor;
        }
        private void SutunGizle() {
            myView1.SutunGizle("Id");
            myView1.SutunGizle("UrId");
            myView1.SutunGizle("UrIId");
            myView1.SutunGizle("IstHrId");
            myView1.SutunFormat("Tarih", DevExpress.Utils.FormatType.DateTime, "dd.MM.yyyy HH:mm"); 
        }
        private string TarihAyarla(DateTime? T1) {
            if (T1 == null) {
                return "";
            }
            string FX = T1?.Year + "-" + T1?.Month.ToString().PadLeft(2, '0') + "-" + T1?.Day.ToString().PadLeft(2, '0');
            return FX;
        }
        private void Yazdir() {
            var itm = myView1.MyGetCurrentItem<UretimKontrol>();
            if (itm != null) {
                const string YazdirmaAdi = "UretimKontrol";
                DataSet ds = new DataSet("UretimKontrolDS");
                ds.Tables.Add(list.ToDataTable("Hareketler"));
                ds.Yaz(YazdirmaAdi, false);
            }
        }
    }
}
