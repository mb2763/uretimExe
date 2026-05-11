using My.Business;
using My.Business.Manager;
using My.Business.Service.Geneller;
using My.Business.Service.UretimEmirler;
using My.Business.Service.UretimOperasyonlar;
using My.Entities.UretimEmirler;
using My.Entities.UretimOperasyonlar;
using My.Kontrol.Formlar;
using My.Kontrol.Yazdirma;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MyUI.UretimModule
{
    public partial class FrmUretimEmriListesi : MyFrmListe
    {
        private DatabaseFactoryMikro _db = Ortak.DbMikro;
        IUretimEmriService _srv = Ortak.DbPro.UretimEmri;
        IUretimOperasyonService _srvOpr = Ortak.DbPro.UretimOperasyon;
        IGenelService _srvGenel = Ortak.DbPro.GenelServis;
        private List<UretimEmri> _list;
        private List<UretimOperasyon> _listOpr;
        private SiparisManager _mng;
        private UretimEmriManager _mngUretim;
        string durumu = "Uretimde";

        public bool SiparisKodundanFiltrele { get; set; } = false; 
        public string SiparisKodu { get; set; } = "";


        public FrmUretimEmriListesi()
        {
            InitializeComponent();
            this.Load += Frm_Load;

        }
        private void Frm_Load(object sender, EventArgs e)
        {
            _mng = new SiparisManager(Ortak.DbPro, Ortak.DbMikro);
            _mngUretim = new UretimEmriManager(Ortak.DbPro);

            TxtTarihi1.Text = DateTime.Now.AddMonths(-1).ToShortDateString();
            TxtTarihi2.Text = "";
            TxtTarihi3.Text = "";
            TxtTarihi4.Text = "";
            BtnAra.Click += BtnAra_Click;
            BtnTemizle.Click += BtnTemizle_Click;
            BtnYazdir.Click += BtnYazdir_Click;
            BtnTemizle.Click += BtnTemizle_Click;
            myView1.MyEventDoubleClickEnter += MyView1_MyEventDoubleClickEnter;
            myView1.FocusedRowChanged += MyView1_FocusedRowChanged;

            BaglaTuru();
            BaglaDurum();
            BaglaReceteAdi();

            if (SiparisKodundanFiltrele)
            {
                SiparisFiltreAyarla();
            }

            Bagla();
            SutunGizle();
            myGrid1.GridYerlesimYukle();
            TxtAdi.Focus();
            DurumuAyarla(durumu);
            BtnAra.PerformClick();
        }

        void SiparisFiltreAyarla()
        {
            durumu = "Tümü";
            TxtKodu.Text = SiparisKodu;
            TxtTarihi1.EditValue = Convert.ToDateTime(DateTime.Now.Year + "-01-01");

        }
        public void SutunGizle()
        {
            myView1.SutunGizle("Id");
            myView1.SutunGizle("RcAId");
            myView1.SutunGizle("SipId");
            myView1.SutunFormat("BaslangicTarihi", DevExpress.Utils.FormatType.DateTime, "dd.MM.yyyy HH:mm");
            myView1.SutunFormat("BitisTarihi", DevExpress.Utils.FormatType.DateTime, "dd.MM.yyyy HH:mm");
            myView1.SutunCaptionDegistir("SiparisKodu", "IsEmriKodu");
        }
        private void SutunGizle2()
        {
            myView2.SutunGizle("Id");
            myView2.SutunGizle("UrId");
            myView2.SutunGizle("RcAId");
            myView2.SutunGizle("RcOId");
            myView2.SutunGizle("SipId");
            myView2.SutunGizle("SipHId");
            myView2.SutunFormat("BaslangicTarihi", DevExpress.Utils.FormatType.DateTime, "dd.MM.yyyy HH:mm");
            myView2.SutunFormat("BitisTarihi", DevExpress.Utils.FormatType.DateTime, "dd.MM.yyyy HH:mm");

        }

        private void Bagla()
        {
            Cursor.Current = Cursors.WaitCursor;
            string sor = SorguAyarla() + SorguAyarlaTrh();
            if (!string.IsNullOrEmpty(sor))
            {
                sor = "where 1=1 " + sor;
            }
            var rs = _srv.SelectListWhere(sor);
            if (!rs.Success)
            {
                Cursor.Current = Cursors.Default;
                MesajHata(rs.Message);
                return;
            }
            _list = rs.Data.ToList();
            myGrid1.DataSource = _list;
            if (rs.Data.Any())
            {
                BaglaOperasyon(rs.Data.FirstOrDefault().Id);
            }
            Cursor.Current = Cursors.Default;
        }
        private void BaglaOperasyon(Guid? UrId)
        {
            Cursor.Current = Cursors.WaitCursor;
            string sor = "where UrId='" + UrId + "' ";
            var rs = _srvOpr.SelectListWhere(sor);
            if (!rs.Success)
            {
                Cursor.Current = Cursors.Default;
                MesajHata(rs.Message);
                return;
            }
            _listOpr = rs.Data.ToList();
            myGrid2.DataSource = _listOpr;
            SutunGizle2();
            myGrid2.GridYerlesimYukle();
            Cursor.Current = Cursors.Default;
        }
        public void BaglaTuru()
        {
            var rs = _srvGenel.GrupListesi("UretimEmri", "Turu");
            if (!rs.Success)
            {
                MesajHata(rs.Message);
                return;
            }
            var dt = rs.Data.ToList();
            dt.Insert(0, "");
            CmbTuru.MyDataBagla(dt);
        }
        public void BaglaDurum()
        {
            var rs = _srvGenel.GrupListesi("UretimEmri", "Durumu");
            if (!rs.Success)
            {
                MesajHata(rs.Message);
                return;
            }
            var dt = rs.Data.ToList();
            dt.Insert(0, "");
            CmbDurumu.MyDataBagla(dt);
        }
        public void BaglaReceteAdi()
        {
            var rs = _srvGenel.GrupListesi("UretimEmri", "ReceteAdi");
            if (!rs.Success)
            {
                MesajHata(rs.Message);
                return;
            }
            var dt = rs.Data.ToList();
            dt.Insert(0, "");
            CmbReceteAdi.MyDataBagla(dt);
        }
        private void DurumGuncelle(object sender, EventArgs e)
        {
            var itm = myView1.MyGetCurrentItem<UretimEmri>();
            if (itm != null)
            {
                var rs = _mngUretim.UretimDurumGuncelle(itm.Id);
                if (!rs.Success)
                {
                    MesajHata(rs.Message);
                    return;
                }
                MesajBilgi("Güncellendi.");
            }
        }
        public string SorguAyarla()
        {
            string sor = "";
            string t1 = TxtKodu.Text.Trim();
            if (!string.IsNullOrEmpty(t1)) { sor += $" AND  SiparisKodu    like('%{t1}%')"; }
            t1 = TxtCariKodu.Text.Trim();
            if (!string.IsNullOrEmpty(t1)) { sor += $" AND  SiparisCariKodu    like('%{t1}%')"; }
            t1 = TxtAdi.Text.Trim();
            if (!string.IsNullOrEmpty(t1)) { sor += $" AND  SiparisCariUnvani like('%{t1}%')  "; }
            t1 = CmbTuru.Text.Trim();
            if (!string.IsNullOrEmpty(t1)) { sor += $" AND  Turu = '{t1}' "; }
            //t1 = CmbDurumu.Text.Trim();
            //if (!string.IsNullOrEmpty(t1)) { sor += $" AND  Durumu = '{t1}' "; }
            t1 = durumu.Trim();
            if (!string.IsNullOrEmpty(t1)) { sor += $" AND  Durumu = '{t1}' "; }

            t1 = CmbReceteAdi.Text.Trim();
            if (!string.IsNullOrEmpty(t1)) { sor += $" AND  ReceteAdi like('%{t1}%')  "; }
            return sor;
        }
        private string SorguAyarlaTrh()
        {
            string sor = "";
            string t1 = "";
            t1 = TxtTarihi1.Text.ToString().Trim();
            if (!string.IsNullOrEmpty(t1))
            {
                t1 = TarihAyarla(Convert.ToDateTime(TxtTarihi1.Text)).ToString();
                if (!string.IsNullOrEmpty(t1)) sor += $"  AND  CAST (coalesce( BaslangicTarihi,'1901-01-01') AS DATE ) >=  CAST('{t1}'  AS DATE ) ";
            }
            t1 = TxtTarihi2.Text.ToString().Trim();
            if (!string.IsNullOrEmpty(t1))
            {
                t1 = TarihAyarla(Convert.ToDateTime(TxtTarihi2.Text)).ToString();
                if (!string.IsNullOrEmpty(t1)) sor += $"  AND  CAST ( coalesce(BaslangicTarihi,'1901-01-01') AS DATE ) <=  CAST('{t1}'  AS DATE ) ";
            }
            t1 = TxtTarihi3.Text.ToString().Trim();
            if (!string.IsNullOrEmpty(t1))
            {
                t1 = TarihAyarla(Convert.ToDateTime(TxtTarihi3.Text)).ToString();
                if (!string.IsNullOrEmpty(t1)) sor += $"  AND  CAST (coalesce( BitisTarihi,'1901-01-01') AS DATE ) >=  CAST('{t1}'  AS DATE ) ";
            }
            t1 = TxtTarihi4.Text.ToString().Trim();
            if (!string.IsNullOrEmpty(t1))
            {
                t1 = TarihAyarla(Convert.ToDateTime(TxtTarihi4.Text)).ToString();
                if (!string.IsNullOrEmpty(t1)) sor += $"  AND  CAST (coalesce( BitisTarihi,'1901-01-01') AS DATE ) <=  CAST('{t1}'  AS DATE ) ";
            }
            return sor;
        }

        public string TarihAyarla(DateTime? T1)
        {
            if (T1 == null)
            {
                return "";
            }
            string FX = T1?.Year + "-" + T1?.Month.ToString().PadLeft(2, '0') + "-" +
                        T1?.Day.ToString().PadLeft(2, '0');
            return FX;
        }
        private void MyView1_MyEventDoubleClickEnter()
        {
            if (SecimIcinAcildi)
            {
                var itm = myView1.MyGetCurrentItem<UretimEmri>();
                if (itm != null)
                {
                    SecilenKod = itm.SiparisKodu;
                    SecilenRow = itm;
                    Secildi = true;
                    this.Close();
                }
            }
            else
            {
                var itm = myView1.MyGetCurrentItem<UretimEmri>();
                if (itm != null)
                {
                    FrmUretimEmriED f = new FrmUretimEmriED();
                    f.pblAlt.Visible = false;
                    f.IdGuid = itm.Id;
                    f.ActionAktar = Bagla;
                    f.ShowDialog();
                }
            }
        }
        private void MyView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {

            var itm = myView1.MyGetCurrentItem<UretimEmri>();
            if (itm != null)
            {
                BaglaOperasyon(itm.Id);
            }
            else
            {
                myGrid2.DataSource = null;

            }
        }
        private void BtnAra_Click(object sender, EventArgs e)
        {
            Bagla();
        }
        private void BtnTemizle_Click(object sender, EventArgs e)
        {
            TxtKodu.Text = "";
            TxtAdi.Text = "";
            CmbDurumu.Text = "";
            CmbTuru.Text = "";
            CmbReceteAdi.Text = "";
            TxtCariKodu.Text = "";
            TxtTarihi1.Text = "";
            TxtTarihi2.Text = "";
            TxtTarihi3.Text = "";
            TxtTarihi4.Text = "";
        }

        private void BtnYazdir_Click(object sender, EventArgs e)
        {
            Yazdir();
        }
        public void Yazdir()
        {
            var itm = myView1.MyGetCurrentItem<UretimEmri>();
            if (itm != null)
            {
                var rs = _mng.GetSiparis(itm.Id);
                if (!rs.Success)
                {
                    MesajHata(rs.Message);
                    return;
                }
                var _mdl = rs.Data;
                string YazdirmaAdi = "UretimEmriListesi";
                DataSet ds = new DataSet("UretimEmriDS");
                ds.Tables.Add(_list.ToDataTable("UretimEmirleri"));
                ds.Yaz(YazdirmaAdi, false);
            }
        }

        private void BtnEkleReceteden_Click(object sender, EventArgs e)
        {
            FrmUretimEmriED f = new FrmUretimEmriED();
            f.ActionAktar = Bagla;
            f.UretimTuru = "Recete";
            f.ShowDialog();
        }
        private void BtnEkleSiparisden_Click(object sender, EventArgs e)
        {
            FrmUretimEmriED f = new FrmUretimEmriED();
            f.ActionAktar = Bagla;
            f.UretimTuru = "Siparis";
            f.ShowDialog();
        }

        private void BtnDurumuHepsi_Click(object sender, EventArgs e)
        {
            DurumuAyarla("Hepsi");
            BtnAra.PerformClick();
        }

        private void BtnDurumuBeklemede_Click(object sender, EventArgs e)
        {
            DurumuAyarla("Beklemede");
            BtnAra.PerformClick();
        }

        private void BtnDurumuUretimde_Click(object sender, EventArgs e)
        {
            DurumuAyarla("Uretimde");
            BtnAra.PerformClick();
        }

        private void BtnDurumuHazir_Click(object sender, EventArgs e)
        {
            DurumuAyarla("Hazir");
            BtnAra.PerformClick();
        }
        private void DurumuAyarla(string durum)
        {
            if (durum == "Hepsi")
            {
                durumu = "";
                BtnDurumuHepsi.FilterButonRenklendir(true);
                BtnDurumuBeklemede.FilterButonRenklendir();
                BtnDurumuUretimde.FilterButonRenklendir();
                BtnDurumuHazir.FilterButonRenklendir();
            }
            else if (durum == "Beklemede")
            {
                durumu = "Beklemede";
                BtnDurumuHepsi.FilterButonRenklendir();
                BtnDurumuBeklemede.FilterButonRenklendir(true);
                BtnDurumuUretimde.FilterButonRenklendir();
                BtnDurumuHazir.FilterButonRenklendir();
            }
            else if (durum == "Uretimde")
            {
                durumu = "Uretimde";
                BtnDurumuHepsi.FilterButonRenklendir();
                BtnDurumuBeklemede.FilterButonRenklendir();
                BtnDurumuUretimde.FilterButonRenklendir(true);
                BtnDurumuHazir.FilterButonRenklendir();
            }
            else if (durum == "Hazir")
            {
                durumu = "Hazir";
                BtnDurumuHepsi.FilterButonRenklendir();
                BtnDurumuBeklemede.FilterButonRenklendir();
                BtnDurumuUretimde.FilterButonRenklendir();
                BtnDurumuHazir.FilterButonRenklendir(true);
            }
            else
            {
                durumu = "";
                BtnDurumuHepsi.FilterButonRenklendir(true);
                BtnDurumuBeklemede.FilterButonRenklendir();
                BtnDurumuUretimde.FilterButonRenklendir();
                BtnDurumuHazir.FilterButonRenklendir();
            }
        }
    }
}