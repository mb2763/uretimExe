using My.Business.Manager;
using My.Business.Service.Geneller;
using My.Business.Service.Siparisler;
using My.Business.Service.Templer;
using My.Core.Result;
using My.Entities.Siparisler;
using My.Kontrol.Formlar;
using My.Kontrol.Yazdirma;
using MyUI.IstasyonModul;
using MyUI.MalKabul;
using MyUI.MikroModul;
using MyUI.UretimModule;
using MyUI.UretimOperasyonModule;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
namespace MyUI.SiparisModule {
    public partial class FrmSiparisListesi : MyFrmListe {
        private readonly IGenelService _srvGenel = Ortak.DbPro.GenelServis;
        private readonly ISiparisService _srv = Ortak.DbPro.Siparis;
        private readonly ITempMikroStokService _srvTmpStk = Ortak.DbPro.TempMikroStok;
        private readonly ISiparisHareketService _srvHareket = Ortak.DbPro.SiparisHareket;  
        string durumu = "";
        string kapandi = "Açık";
        public List<SiparisHareket> Hareketler { get; set; }
        /*   private ISiparisHareketDetayService _srvDetay = Ortak.DbPro.SiparisHareketDetay;   */
        private SiparisManager _mng;
        public string Turu = "Siparis";
        public FrmSiparisListesi() {
            InitializeComponent();
            this.Load += Frm_Load;
            myView1.FocusedRowChanged += MyView1_FocusedRowChanged;
            myView1.MyEventDoubleClickEnter += MyView1_MyEventDoubleClickEnter;
        }
        private void Frm_Load(object sender, EventArgs e) {
            if (Turu == "Recete") {
                this.Text = "Recete Üretim Listesi";
                BtnEkleSiparis.Text = "Üretim Ekle";
            }
            else if (Turu == "MikroSiparis") {
                this.Text = "Mikrodan Aktarılan Siparis Listesi";
                BtnEkleSiparis.Visible = true;
            }
            //TxtTarihi1.EditValue = Convert.ToDateTime("01.01." + DateTime.Now.Year.ToString());
            //TxtTarihi1.Text = "01.01." + DateTime.Now.Year.ToString();           
            TxtTarihi1.EditValue = DateTime.Now;
            TxtTarihi1.Text = DateTime.Now.ToShortDateString();
            CmbKapandi.Text = "Açık";
            _mng = new SiparisManager(Ortak.DbPro, Ortak.DbMikro);
            BaglaDurum();
            Bagla();
            SutunGizle();
            myGrid1.GridYerlesimYukle();
            if (Ortak.PlKapat) {
                myView1.SutunGizle("Parti");
                myView1.SutunGizle("Lot");
            }
            ContexMenuyeEkle();
            DurumuAyarla(durumu);
            KapandiAyarla(kapandi);
            BtnAra.PerformClick();
        }
        private void Bagla() {
            var rwId = myView1.FocusedRowHandle;
            Cursor.Current = Cursors.WaitCursor;
            string sor = SorguAyarla() + SorguAyarlaTrh();
            if (!string.IsNullOrEmpty(sor)) {
                sor = "where 1=1 " + sor;
            }
            var rs = _srv.SelectListWhere(sor);
            if (!rs.Success) {
                Cursor.Current = Cursors.Default;
                MesajHata(rs.Message);
                return;
            }
            myGrid1.DataSource = rs.Data;
            if (rs.Data.Any()) {
                Siparis first = null;
                foreach (var siparis in rs.Data) {
                    first = siparis;
                    break;
                }
                if (first != null) {
                    BaglaHareket(first.Id);
                }
                else {
                    Hareketler = new List<SiparisHareket>();
                }
            }
            Cursor.Current = Cursors.Default;
            if (Turu == "Recete") {
                LblSiparisTarihi.Text = "İş Emri Tarihi";
                LblSiparisKodu.Text = "İş Emri Kodu";
            }
            try {
                myView1.FocusedRowHandle = rwId;
            } catch { }
        }
        private void BaglaDurum() {
            var rs = _srvGenel.GrupListesi("Siparis", "Durumu");
            if (!rs.Success) {
                MesajHata(rs.Message);
                return;
            }
            var dt = rs.Data.ToList();
            dt.Insert(0, "");
            CmbDurumu.DataSource = dt;
        }
        private void BaglaHareket(Guid? sipid) {
            Cursor.Current = Cursors.WaitCursor;
            var rs = _srvHareket.GetViewListKalanMiktarliWhere($" where SH.SipId='{sipid}'");
            if (!rs.Success) {
                Cursor.Current = Cursors.Default;
                MesajHata(rs.Message);
                return;
            }
            Hareketler = rs.Data.ToList();
            myGrid2.DataSource = Hareketler;
            SutunGizle2();
            myGrid2.GridYerlesimYukle();
            if (Ortak.PlKapat) {
                myView2.SutunGizle("Parti");
                myView2.SutunGizle("Lot");
            }
            Cursor.Current = Cursors.Default;
        }
        private IDataResult<IEnumerable<SiparisHareket>> BaglaHareketList(Guid? sipid) {
            Cursor.Current = Cursors.WaitCursor;
            var rs = _srvHareket.GetViewListKalanMiktarliWhere($" where SH.SipId='{sipid}'");
            Cursor.Current = Cursors.Default;
            return rs; 
        }
        private void UretimEmriEmriOlusturBaslat(Siparis sip, bool OtoBaslat = false) { 
            var rs = BaglaHareketList(sip.Id);
            if (!rs.Success) {
                MesajHata(rs.Message);
                return;
            }
            var lis = rs.Data.ToList();
            //  TakipTip =1   {  /*PARTİ TAKİPLİ ÜRÜN*/}  TakipTip   =2 { /*PARTİ VE LOT TAKİPLİ ÜRÜN*/ } TakipTip   =3 {/*SERİ NOLU TAKİP*/ } 
            //  RbTakipTip =1 {  /*RENK  TAKİPLİ ÜRÜN*/}  RbTakipTip =2 { /*BEDEN TAKİPLİ ÜRÜN*/  }       RbTakipTip =3 { /*RENK VE BEDEN TAKİPLİ ÜRÜN*/}   
            //  üretim emri partilot varsa partilot girme  zorunlu olacak
            foreach (var itm in lis) {
                if (Ortak.PlKapat) {
                    continue;
                }
                if (itm.Parti.IsNullOrEmpty()) {
                    var rsknt = _srvTmpStk.SelectFirst(c => c.StokKodu == itm.StokKodu, "*");
                    if (rsknt.IsError) {
                        MesajHata(rsknt.Message);
                        return;
                    }
                    if (rsknt.Data.TakipTip == 1 || rsknt.Data.TakipTip == 2) {
                        MesajBilgi(itm.StokKodu + " " + itm.StokAdi + " Üründe parti takibi var lütfen parti no giriniz. ");
                        return;
                    }
                }
            }
            FrmUretimEmriED f = new FrmUretimEmriED { UretimTuru = Turu, SipId = sip.Id, ActionAktar = Bagla };
            f.OtoBaslat = OtoBaslat;
            f.ShowDialog();
        }
        private void BtnAra_Click(object sender, EventArgs e) {
            Bagla();
        }
        private void BtnEkleSiparis_Click(object sender, EventArgs e) {
            if (Turu == "Recete") {
                FrmSiparisReceteEd fr = new FrmSiparisReceteEd {
                    MdiParent = FrmAna.ActiveForm,
                    pblAlt = { Visible = false },
                    ActionAktar = Bagla
                };
                fr.Show();
                return;
            }
            else {
                FrmSiparisEd f = new FrmSiparisEd {
                    MdiParent = FrmAna.ActiveForm,
                    pblAlt = { Visible = false },
                    ActionAktar = Bagla
                };
                f.Show();
            }
        }
        private void BtnTemizle_Click(object sender, EventArgs e) {
            TxtKodu.Text = "";
            TxtAdi.Text = "";
            CmbDurumu.Text = "";
            TxtCariKodu.Text = "";
            TxtTarihi1.Text = "01.01." + DateTime.Now.Year.ToString();
            TxtTarihi2.Text = "";
        }
        private void BtnYazdir_Click(object sender, EventArgs e) {
            Yazdir();
        }
        private void BtnYazdirTek_Click(object sender, EventArgs e) {
            Yazdir2();
        }
        private void ContexMenuyeEkle() {
            myGrid1.MyContextMenu.Items.Add(new ToolStripSeparator());
            var mn1 = new ToolStripMenuItem("UretimEmriBaslat") {
                BackColor = Color.WhiteSmoke,
                ForeColor = Color.Black,
                Text = "Uretimi Emri (Olustur / Oto Başlat)",
                TextAlign = ContentAlignment.BottomRight,
                ToolTipText = "Uretimi Emri (Olustur / Oto Başlat)"
            };
            mn1.Click += ConIsEmriOlusturBaslat;
            myGrid1.MyContextMenu.Items.Add(mn1);
            var mn2 = new ToolStripMenuItem("Uretim_EmriOlustur") {
                BackColor = Color.WhiteSmoke,
                ForeColor = Color.Black,
                Text = "Uretim Emri (Olustur / Guncelle)",
                TextAlign = ContentAlignment.BottomRight,
                ToolTipText = "Uretim Emri (Olustur / Guncelle)"
            };
            mn2.Click += ConIsEmriOlusturV2;
            myGrid1.MyContextMenu.Items.Add(mn2);
            myGrid1.MyContextMenu.Items.Add(new ToolStripSeparator());
            var mn3 = new ToolStripMenuItem("OperasyonBaslat") {
                BackColor = Color.WhiteSmoke,
                ForeColor = Color.Black,
                Text = "Operasyon (İlk İstasyonlara Gönder)",
                TextAlign = ContentAlignment.BottomRight,
                ToolTipText = "Operasyon (İlk İstasyonlara Gönder)"
            };
            mn3.Click += ConOperasyonIstasyonBaslat;
            myGrid1.MyContextMenu.Items.Add(mn3);
            myGrid1.MyContextMenu.Items.Add(new ToolStripSeparator());
            //var fm2 = new ToolStripMenuItem("MikroyaUretimGonder")
            //{
            //    BackColor = Color.WhiteSmoke,
            //    ForeColor = Color.Black,
            //    Text = "Mikroya Uretim Gonder Eski(silinecek)",
            //    TextAlign = ContentAlignment.BottomRight,
            //    ToolTipText = "Mikroya Uretim Gonder"
            //};
            //fm2.Click += ConMikroyaUretimGonder;
            //myGrid1.MyContextMenu.Items.Add(fm2);
            var mn4 = new ToolStripMenuItem("UretimAdetGuncelle") {
                BackColor = Color.WhiteSmoke,
                ForeColor = Color.Black,
                Text = "Üretilen Adetleri Güncelle",
                TextAlign = ContentAlignment.BottomRight,
                ToolTipText = "Üretilen Adetleri Güncelle"
            };
            mn4.Click += ConUretimAdetleriGuncelle;
            myGrid1.MyContextMenu.Items.Add(mn4);
            var mn5 = new ToolStripMenuItem("MikroyaUretimGonder2") {
                BackColor = Color.WhiteSmoke,
                ForeColor = Color.Black,
                Text = "Mikroya Uretim Gonder",
                TextAlign = ContentAlignment.BottomRight,
                ToolTipText = "Mikroya Uretim Gonder"
            };
            mn5.Click += ConMikroyaUretimGonderYeni;
            myGrid1.MyContextMenu.Items.Add(mn5);
            var mn51 = new ToolStripMenuItem("MikroyaGonderilenFisler") {
                BackColor = Color.WhiteSmoke,
                ForeColor = Color.Black,
                Text = "Mikroya Aktarilan Uretim Fisleri",
                TextAlign = ContentAlignment.BottomRight,
                ToolTipText = "Mikroya Aktarilan Uretim Fisleri"
            };
            mn51.Click += ConMikroyaGonderilenFisler;
            myGrid1.MyContextMenu.Items.Add(mn51);
            var mn52 = new ToolStripMenuItem("MalKabulFisi") {
                BackColor = Color.WhiteSmoke,
                ForeColor = Color.Black,
                Text = "Mal Kabul Fisi",
                TextAlign = ContentAlignment.BottomRight,
                ToolTipText = "Mal Kabul Fisi"
            };
            mn52.Click += ConMalKabulFisAc;
            myGrid1.MyContextMenu.Items.Add(mn52);
            var mn6 = new ToolStripMenuItem("IptalEdilenleriDepoKontroleGeriAktar") {
                BackColor = Color.WhiteSmoke,
                ForeColor = Color.Black,
                Text = "Iptal Edilenleri Depo Kontrole GeriAktar",
                TextAlign = ContentAlignment.BottomRight,
                ToolTipText = "Iptal Edilenleri Depo Kontrole GeriAktar"
            };
            mn6.Click += ConIptalFisiniDepoyaAktar;
            myGrid1.MyContextMenu.Items.Add(mn6);
            myGrid1.MyContextMenu.Items.Add(new ToolStripSeparator());
            ToolStripMenuItem git;
            git = new ToolStripMenuItem("Git") {
                BackColor = Color.WhiteSmoke,
                ForeColor = Color.Black,
                Text = "Git",
                TextAlign = ContentAlignment.BottomRight,
                ToolTipText = "Git"
            };
            git.DropDownItems.Add("Üretim Emri", null, Git_UretimEmirleri);
            git.DropDownItems.Add("Üretim Operasyonlar", null, Git_UretimOperasyonlar);
            git.DropDownItems.Add("Istasyon Hareketler", null, Git_IstasyonHareketleri);
            git.DropDownItems.Add("Istasyon Bekleyenler", null, Git_IstasyonBekleyenler);
            git.DropDownItems.Add("Istasyon Hareket Detaylar", null, Git_IstasyonHareketDetaylar);
            myGrid1.MyContextMenu.Items.Add(git);
        }
        private void Git_UretimEmirleri(object sender, EventArgs e) {
            var itm = myView1.MyGetCurrentItem<Siparis>();
            if (itm == null) return;
            FrmUretimEmriListesi f = new FrmUretimEmriListesi();
            f.SiparisKodundanFiltrele = true;
            f.SiparisKodu = itm.SiparisKodu;
            f.Show();
        }
        private void Git_UretimOperasyonlar(object sender, EventArgs e) {
            var itm = myView1.MyGetCurrentItem<Siparis>();
            if (itm == null) return;
            FrmUretimOperasyonList f = new FrmUretimOperasyonList();
            f.SiparisKodundanFiltrele = true;
            f.SiparisKodu = itm.SiparisKodu;
            f.Show();
        }
        private void Git_IstasyonHareketleri(object sender, EventArgs e) {
            var itm = myView1.MyGetCurrentItem<Siparis>();
            if (itm == null) return;
            FrmIstasyonHareketler f = new FrmIstasyonHareketler();
            f.SiparisKodundanFiltrele = true;
            f.SiparisKodu = itm.SiparisKodu;
            f.Show();
        }
        private void Git_IstasyonBekleyenler(object sender, EventArgs e) {
            var itm = myView1.MyGetCurrentItem<Siparis>();
            if (itm == null) return;
            FrmIstasyonBekleyenler f = new FrmIstasyonBekleyenler();
            f.SiparisKodundanFiltrele = true;
            f.SiparisKodu = itm.SiparisKodu;
            f.Show();
        }   
        private void Git_IstasyonHareketDetaylar(object sender, EventArgs e) {
            var itm = myView1.MyGetCurrentItem<Siparis>();
            if (itm == null) return; 
            FrmIstasyonHareketDetaylar f = new FrmIstasyonHareketDetaylar();
            f.MdiParent = FrmAna.ActiveForm;
            f.SiparisKodu = itm.SiparisKodu;
            f.Show(); 
        }
        private void ConIsEmriOlusturV2(object sender, EventArgs e) {
            var itm = myView1.MyGetCurrentItem<Siparis>();
            if (itm != null) {
                UretimEmriEmriOlusturBaslat(itm, false);
            }
        }
        private void ConIsEmriOlusturBaslat(object sender, EventArgs e) {
            var itm = myView1.MyGetCurrentItem<Siparis>();
            if (itm != null) {
                UretimEmriEmriOlusturBaslat(itm, true);
            }
        }
        private void ConMikroyaUretimGonderYeni(object sender, EventArgs e) {
            var itm = myView1.MyGetCurrentItem<Siparis>();
            if (itm != null) {
                if (itm.Durumu == "Hazir" || itm.Durumu == "Hazır") {
                    var rs = _mng.GetTempSiparisUretimMiktarBySipId(itm.Id, Ortak.KullaniciAdi);
                    if (!rs.Success) {
                        MesajHata(rs.Message);
                        return;
                    }
                    FrmMikroyaUretimKaydetV2 f = new FrmMikroyaUretimKaydetV2();
                    f.SipId = itm.Id;
                    f.ShowDialog();
                }
                else {
                    MesajBilgi("Durumu Hazır Olmayan Sipariş Mikroya Gönderilemez");
                }
            }
        }
        private void ConMikroyaGonderilenFisler(object sender, EventArgs e) {
            var itm = myView1.MyGetCurrentItem<Siparis>();
            if (itm != null) {
                FrmMikroUretimKaydedilenFisler f = new FrmMikroUretimKaydedilenFisler();
                f.BelgeNo = itm.SiparisKodu;
                f.SipId = itm.Id;
                f.ShowDialog();
            }
        }
        private void ConMalKabulFisAc(object sender, EventArgs e) {
            var itm = myView1.MyGetCurrentItem<Siparis>();
            if (itm != null) {
                FrmMalKabulED f = new FrmMalKabulED();
                f.SiparisIdDenAra = true;
                f.SipId = itm.Id;
                f.ShowDialog();
            }
        }
        private void ConIptalFisiniDepoyaAktar(object sender, EventArgs e) {
            var itm = myView1.MyGetCurrentItem<Siparis>();
            if (itm != null) {
                if (itm.Durumu == "Hazir" || itm.Durumu == "Hazır") {
                    var rs = _mng.GetTempSiparisUretimMiktarBySipId(itm.Id, Ortak.KullaniciAdi);
                    if (!rs.Success) {
                        MesajHata(rs.Message);
                        return;
                    }
                    FrmUretimIptalleriDepoyaAktar f = new FrmUretimIptalleriDepoyaAktar();
                    f.SipId = itm.Id;
                    f.ShowDialog();
                }
                else {
                    MesajBilgi("Durumu Hazır Olmayan Siparişe işlem yapılamaz.");
                }
            }
        }
        private void ConUretimAdetleriGuncelle(object sender, EventArgs e) {
            var itm = myView1.MyGetCurrentItem<Siparis>();
            if (itm != null) {
                var rs = _mng.GetTempSiparisUretimMiktarBySipId(itm.Id, Ortak.KullaniciAdi);
                if (!rs.Success) {
                    MesajHata(rs.Message);
                    return;
                }
                BtnAra.PerformClick();
            }
        }
        private void ConOperasyonIstasyonBaslat(object sender, EventArgs e) {
            var mdl = myView1.MyGetCurrentItem<Siparis>();
            if (mdl == null) return;
            if (mdl.Durumu == "YeniKayit") {
                MesajHata(" Uretim Başlatılmamış. ");
                return;
            }
            List<Guid> list = new List<Guid>();
            List<string> lisKod = new List<string>();
            foreach (var itm in Hareketler) {
                var f = list.Find(C => C == itm.RcAId);
                if (f == null || f == Guid.Empty) {
                    list.Add(Guid.Parse(itm.RcAId?.ToString()));
                    var rs = _mng.GetOperasyonKoduByRcAId(itm.RcAId);
                    if (!rs.Success) {
                        MesajHata(rs.Message);
                        return;
                    }
                    var dt = rs.Data;
                    if (!string.IsNullOrEmpty(dt)) {
                        var f2 = lisKod.Find(C => C == dt);
                        if (string.IsNullOrEmpty(f2)) {
                            lisKod.Add(dt.ToString());
                        }
                    }
                }
            }
            foreach (var itm in lisKod) {
                FrmOperasyonTakipDetaylar f = new FrmOperasyonTakipDetaylar { Operasyon = itm, Durumu = "Beklemede" };
                f.SipIdDenBagla = true;
                f.SipId = mdl.Id.ToString();
                f.ShowDialog();
            }
        }
        private void MyView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e) {
            var itm = myView1.MyGetCurrentItem<Siparis>();
            if (itm != null) {
                BaglaHareket(itm.Id);
            }
        }
        private void MyView1_MyEventDoubleClickEnter() {
            var itm = myView1.MyGetCurrentItem<Siparis>();
            if (itm == null) return;
            if (SecimIcinAcildi) {
                SecilenKod = itm.SiparisKodu;
                SecilenRow = itm;
                Secildi = true;
                this.Close();
            }
            else {
                if (itm.Turu == "Recete") {
                    FrmSiparisReceteEd f = new FrmSiparisReceteEd {
                        MdiParent = FrmAna.ActiveForm,
                        pblAlt = { Visible = false },
                        IdGuid = itm.Id,
                        ActionAktar = Bagla
                    };
                    f.Show();
                }
                else {
                    FrmSiparisEd f = new FrmSiparisEd {
                        MdiParent = FrmAna.ActiveForm,
                        pblAlt = { Visible = false },
                        IdGuid = itm.Id,
                        ActionAktar = Bagla
                    };
                    f.Show();
                }
            }
        }
        private string SorguAyarla() {
            string sor = "";
            string t1 = Turu;
            if (!string.IsNullOrEmpty(t1)) { sor += $" AND  Turu = '{t1}' "; }
            t1 = TxtKodu.Text.Trim();
            if (!string.IsNullOrEmpty(t1)) { sor += $" AND  SiparisKodu  like('%{t1}%') "; }
            t1 = TxtCariKodu.Text.Trim();
            if (!string.IsNullOrEmpty(t1)) { sor += $" AND  CariKodu     like('%{t1}%') "; }
            t1 = TxtAdi.Text.Trim();
            if (!string.IsNullOrEmpty(t1)) { sor += $" AND  CariUnvani   like('%{t1}%') "; }
            t1 = durumu.Trim();
            if (!string.IsNullOrEmpty(t1)) { sor += $" AND  Durumu = '{t1}' "; }
            if (kapandi == "Kapandı") {
                sor += " AND  coalesce(Kapandi,0) = 1   ";
            }
            else if (kapandi == "Açık") {
                sor += " AND  coalesce(Kapandi,0) = 0   ";
            }
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
        private void SutunGizle() {
            myView1.SutunGizle("Id");
            myView1.SutunGizle("EntKayitSeri");
            myView1.SutunGizle("EntKayitSira");
            myView1.SutunGizle("EntKayitGuid");
            myView1.SutunFormat("Miktar", DevExpress.Utils.FormatType.Numeric, "N0");
            myView1.SutunFormat("Tarih", DevExpress.Utils.FormatType.DateTime, "dd.MM.yyyy HH:mm");
            myView1.SutunCaptionDegistir("SiparisKodu", "IsEmriKodu");
            if (Ortak.PlKapat) {
                myView1.SutunGizle("Parti");
                myView1.SutunGizle("Lot");
            }
        }
        private void SutunGizle2() {
            myView2.SutunGizle("Id");
            myView2.SutunGizle("RcAId");
            myView2.SutunGizle("SipId");
            myView2.SutunGizle("EntKayitSeri");
            myView2.SutunGizle("EntKayitSira");
            myView2.SutunGizle("EntKayitGuid");
            if (Ortak.PlKapat) {
                myView2.SutunGizle("Parti");
                myView2.SutunGizle("Lot");
            }
        }
        private string TarihAyarla(DateTime? T1) {
            if (T1 == null) {
                return "";
            }
            string FX = T1?.Year + "-" + T1?.Month.ToString().PadLeft(2, '0') + "-" + T1?.Day.ToString().PadLeft(2, '0');
            return FX;
        }
        private void Yazdir() {
            var itm = myView1.MyGetCurrentItem<Siparis>();
            if (itm != null) {
                var rs = _mng.GetSiparis(itm.Id);
                if (!rs.Success) {
                    MesajHata(rs.Message);
                    return;
                }
                var _mdl = rs.Data;
                const string YazdirmaAdi = "Siparis";
                DataSet ds = new DataSet("SiparisDS");
                ds.Tables.Add(_mdl.Siparis.ToDataTable("Siparis"));
                ds.Tables.Add(_mdl.Hareketler.ToDataTable("Hareketler"));
                ds.Tables.Add(_mdl.Detaylar.ToDataTable("Detaylar"));
                ds.Yaz(YazdirmaAdi, false);
            }
        }
        private void Yazdir2() {
            var row = myView1.MyGetCurrentItem<Siparis>();
            if (row != null) {
                var rs = _mng.GetSiparis(row.Id);
                if (!rs.Success) {
                    MesajHata(rs.Message);
                    return;
                }
                var _mdl = rs.Data;
                const string YazdirmaAdi = "Siparis";
                YaziciAyar ayar = null;
                foreach (var itm in _mdl.Hareketler) {
                    DataSet ds = new DataSet("SiparisDS");
                    ds.Tables.Add(_mdl.Siparis.ToDataTable("Siparis"));
                    ds.Tables.Add(_mdl.Hareketler.Where(c => c.Id == itm.Id).ToList().ToDataTable("Hareketler"));
                    ds.Tables.Add(_mdl.Detaylar.Where(c => c.SipHId == itm.Id).ToList().ToDataTable("Detaylar"));
                    if (ayar == null) ayar = ds.YaziciAyarAl(YazdirmaAdi); // ayar al
                    if (ayar == null) break; // ayar alinanamadiysa çık
                    if (ayar.YazdirmaTuru == YazdirmaTuru.Iptal || ayar.YazdirmaTuru == YazdirmaTuru.Dizayn) {// yazdirma iptal edildiyse yada dizayn acildiysa yazdirma iptal
                        break;
                    }
                    else if (ayar.YazdirmaTuru == YazdirmaTuru.Onizle) {
                        for (int i = 0; i < ayar.Adet; i++) {
                            YazdirDevexp.Yazdir_Yolile(ds, false, true, ayar.YaziciAdi, ayar.DizaynYol);
                        }
                    }
                    else // yazdir
                    {
                        for (int i = 0; i < ayar.Adet; i++) {
                            YazdirDevexp.Yazdir_Yolile(ds, false, false, ayar.YaziciAdi, ayar.DizaynYol);
                        }
                    }
                }
            }
        }
        private void BtnUretimEmriOlustur_Click(object sender, EventArgs e) {
            var itm = myView1.MyGetCurrentItem<Siparis>();
            if (itm != null) {
                UretimEmriEmriOlusturBaslat(itm, false);
            }
        }
        private void BtnDurumuHepsi_Click(object sender, EventArgs e) {
            DurumuAyarla("Hepsi");
            BtnAra.PerformClick();
        }
        private void BtnDurumuYeniKayit_Click(object sender, EventArgs e) {
            DurumuAyarla("YeniKayit");
            BtnAra.PerformClick();
        }
        private void BtnDurumuBeklemede_Click(object sender, EventArgs e) {
            DurumuAyarla("Beklemede");
            BtnAra.PerformClick();
        }
        private void BtnDurumuUretimde_Click(object sender, EventArgs e) {
            DurumuAyarla("Uretimde");
            BtnAra.PerformClick();
        }
        private void BtnDurumuHazir_Click(object sender, EventArgs e) {
            DurumuAyarla("Hazir");
            BtnAra.PerformClick();
        }
        private void BtnKapandiTumu_Click(object sender, EventArgs e) {
            KapandiAyarla("Tümü");
            BtnAra.PerformClick();
        }
        private void BtnKapandiAcik_Click(object sender, EventArgs e) {
            KapandiAyarla("Açık");
            BtnAra.PerformClick();
        }
        private void BtnKapandiKapandi_Click(object sender, EventArgs e) {
            KapandiAyarla("Kapandı");
            BtnAra.PerformClick();
        }
        private void DurumuAyarla(string durum) {
            if (durum == "Hepsi") {
                durumu = "";
                BtnDurumuHepsi.FilterButonRenklendir(true);
                BtnDurumuYeniKayit.FilterButonRenklendir();
                BtnDurumuBeklemede.FilterButonRenklendir();
                BtnDurumuUretimde.FilterButonRenklendir();
                BtnDurumuHazir.FilterButonRenklendir();
            }
            else if (durum == "Beklemede") {
                durumu = "Beklemede";
                BtnDurumuHepsi.FilterButonRenklendir();
                BtnDurumuYeniKayit.FilterButonRenklendir();
                BtnDurumuBeklemede.FilterButonRenklendir(true);
                BtnDurumuUretimde.FilterButonRenklendir();
                BtnDurumuHazir.FilterButonRenklendir();
            }
            else if (durum == "YeniKayit") {
                durumu = "YeniKayit";
                BtnDurumuHepsi.FilterButonRenklendir();
                BtnDurumuYeniKayit.FilterButonRenklendir(true);
                BtnDurumuBeklemede.FilterButonRenklendir();
                BtnDurumuUretimde.FilterButonRenklendir();
                BtnDurumuHazir.FilterButonRenklendir();
            }
            else if (durum == "Uretimde") {
                durumu = "Uretimde";
                BtnDurumuHepsi.FilterButonRenklendir();
                BtnDurumuYeniKayit.FilterButonRenklendir();
                BtnDurumuBeklemede.FilterButonRenklendir();
                BtnDurumuUretimde.FilterButonRenklendir(true);
                BtnDurumuHazir.FilterButonRenklendir();
            }
            else if (durum == "Hazir") {
                durumu = "Hazir";
                BtnDurumuHepsi.FilterButonRenklendir();
                BtnDurumuYeniKayit.FilterButonRenklendir();
                BtnDurumuBeklemede.FilterButonRenklendir();
                BtnDurumuUretimde.FilterButonRenklendir();
                BtnDurumuHazir.FilterButonRenklendir(true);
            }
            else {
                durumu = "";
                BtnDurumuHepsi.FilterButonRenklendir(true);
                BtnDurumuYeniKayit.FilterButonRenklendir();
                BtnDurumuBeklemede.FilterButonRenklendir();
                BtnDurumuUretimde.FilterButonRenklendir();
                BtnDurumuHazir.FilterButonRenklendir();
            }
        }
        private void KapandiAyarla(string durum) {
            if (durum == "Tümü") {
                kapandi = "";
                BtnKapandiTumu.FilterButonRenklendir(true);
                BtnKapandiAcik.FilterButonRenklendir();
                BtnKapandiKapandi.FilterButonRenklendir();
            }
            else if (durum == "Açık") {
                kapandi = "Açık";
                BtnKapandiTumu.FilterButonRenklendir();
                BtnKapandiAcik.FilterButonRenklendir(true);
                BtnKapandiKapandi.FilterButonRenklendir();
            }
            else if (durum == "Kapandı") {
                kapandi = "Kapandı";
                BtnKapandiTumu.FilterButonRenklendir();
                BtnKapandiAcik.FilterButonRenklendir();
                BtnKapandiKapandi.FilterButonRenklendir(true);
            }
            else {
                kapandi = "";
                BtnKapandiTumu.FilterButonRenklendir(true);
                BtnKapandiAcik.FilterButonRenklendir();
                BtnKapandiKapandi.FilterButonRenklendir();
            }
        }
    }
}