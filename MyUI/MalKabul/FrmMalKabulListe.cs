using My.Business.Service.Geneller;
using My.Business.Service.UretimStoklar;
using My.Entities.UretimStoklar;
using My.Kontrol.Formlar;
using My.Kontrol.Yazdirma;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace MyUI.MalKabul {
    public partial class FrmMalKabulListe : MyFrmListe {

        private readonly IGenelService _srvGenel = Ortak.DbPro.GenelServis;
        private readonly IUretimStokFisService _srv = Ortak.DbPro.UretimStokFis;

        string durumu = "";

        public List<UretimStokFisModel> list { get; set; }

        //     private SiparisManager _mng;

        public FrmMalKabulListe() {
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
            var rs = _srv.GetFisList(sor);
            if (!rs.Success) {
                Cursor.Current = Cursors.Default;
                MesajHata(rs.Message);
                return;
            }
            list = rs.Data;
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
            var itm = myView1.MyGetCurrentItem<UretimStokFisModel>();
            if (itm != null) {
                //  BaglaHareket(itm.Id);
            }
        } 
        private void MyView1_MyEventDoubleClickEnter() {
            var itm = myView1.MyGetCurrentItem<UretimStokFisModel>();
            if (itm == null) return;
            if (SecimIcinAcildi) {
                SecilenKod = itm.EvrakNo;
                SecilenRow = itm;
                Secildi = true;
                this.Close();
            }
            else {
                FrmMalKabulED f = new FrmMalKabulED { IdGuid = itm.Id };
                f.ShowDialog();
            }
        } 

        private string SorguAyarla() { // UretimStokFis UF UretimEmri UR  UretimIstasyon UrI
            string sor = "";
            string t1 = "";
            t1 = TxtAra.Text.Trim();
            if (!string.IsNullOrEmpty(t1)) { sor += $" AND  (URI.IstasyonKodu   like('%{t1}%') OR URI.IstasyonAdi   like('%{t1}%')  )"; }
            t1 = durumu.Trim();
            if (!string.IsNullOrEmpty(t1)) { sor += $" AND  UF.Durumu = '{t1}' "; }
            return sor;
        }

        private string SorguAyarlaTrh() {
            string sor = "";
            var t1 = TxtTarihi1.Text.Trim();
            if (!string.IsNullOrEmpty(t1)) {
                t1 = TarihAyarla(Convert.ToDateTime(TxtTarihi1.EditValue));
                if (!string.IsNullOrEmpty(t1)) sor += $"  AND  CAST (coalesce( UF.Tarih,'1901-01-01') AS DATE ) >=  CAST('{t1}'  AS DATE ) ";
            }
            t1 = TxtTarihi2.Text.Trim();
            if (!string.IsNullOrEmpty(t1)) {
                t1 = TarihAyarla(Convert.ToDateTime(TxtTarihi2.EditValue));
                if (!string.IsNullOrEmpty(t1)) sor += $"  AND  CAST (coalesce( UF.Tarih,'1901-01-01') AS DATE ) <=  CAST('{t1}'  AS DATE ) ";
            }
            return sor;
        }

        private void SutunGizle() {
            myView1.SutunGizle("Id");
            myView1.SutunGizle("UrId");
            myView1.SutunFormat("Tarih", DevExpress.Utils.FormatType.DateTime, "dd.MM.yyyy");
            myView1.SutunCaptionDegistir("SiparisKodu", "IsEmriKodu");
        }

        private string TarihAyarla(DateTime? T1) {
            if (T1 == null) {
                return "";
            }
            string FX = T1?.Year + "-" + T1?.Month.ToString().PadLeft(2, '0') + "-" + T1?.Day.ToString().PadLeft(2, '0');
            return FX;
        }

        private void Yazdir() {
            var itm = myView1.MyGetCurrentItem<UretimStokFisModel>();
            if (itm != null) {
                const string YazdirmaAdi = "MalKabul";
                DataSet ds = new DataSet("MalKabulDS");
                ds.Tables.Add(list.ToDataTable("Hareketler"));
                ds.Yaz(YazdirmaAdi, false);
            }
        }

    }
}
