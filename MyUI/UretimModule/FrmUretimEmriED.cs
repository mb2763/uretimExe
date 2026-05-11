using DevExpress.XtraGrid.Views.Grid;
using My.Business.Manager;
using My.Business.Service.Geneller;
using My.Business.Service.Receteler;
using My.Core;
using My.Entities.Mikro;
using My.Entities.Models;
using My.Entities.Siparisler;
using My.Entities.UretimOperasyonlar;
using My.Kontrol.Formlar;
using MyUI.MikroModule;
using MyUI.ReceteIstasyonGrupModul;
using MyUI.SiparisModule;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Action = System.Action;
namespace MyUI.UretimModule
{
    public partial class FrmUretimEmriED : MyFrmKayitFull
    {
        private readonly IGenelService _srvGenel = Ortak.DbPro.GenelServis;
        private readonly IReceteAnaService _srvRct = Ortak.DbPro.ReceteAna;
        private UretimEmriKayitModelSiparis _mdl;
        private UretimEmriManager _mng;
        public Action ActionAktar;
        public Guid? SipId = Guid.Empty;
        public string UretimTuru = "Siparis";
        public bool OtoBaslat { get; set; } = false;

        public FrmUretimEmriED()
        {
            InitializeComponent();
            this.Load += Frm_Load;
            this.FormClosing += Frm_FormClosing;
        }
        private void Frm_Load(object sender, EventArgs e)
        {
            TxtIsEmriNo.ButtonClick += TxtIsEmriNo_ButtonClick;
            BtnSil.Click += BtnSil_Click;
            BtnKaydet.Click += BtnKaydet_Click;
            // BtnUretimeBasla.Click += BtnUretimeBasla_Click; // clicde var 2 ci defa tiklamaya sebep oluyor
            // BtnUretimiSil.Click += BtnUretimiSil_Click;// clicde var 2 ci defa tiklamaya sebep oluyor
            // BtnUretimeAitTumKayitlariSil.Click += BtnUretimeAitTumKayitlariSil_Click;// clicde var 2 ci defa tiklamaya sebep oluyor
            myView1.RowStyle += GridView_RowStyle;
            _mng = new UretimEmriManager(Ortak.DbPro);
            BaglaTuru();
            BaglaDurum();
            if (!Bagla())
            {
                return;
            }
            GridBagla();
            GridBaglaAcilis();
            GridBaglaStok();
            GridBaglaAcilisStok();
            GridBaglaIstasyon();
            GridBaglaAcilisIstasyon();

            if (OtoBaslat)
            {
                var rss = KaydetBaslat();
                if (rss)
                {
                    this.Close();
                }
            }
        }
        private void Frm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (YeniKayit && KayitEdildi)
            {
                if (_mdl.UretimOperasyonHareketler == null || _mdl.UretimOperasyonHareketler.Count < 1)
                {
                    MesajHata("Lütfen üretimi başlatın");
                    e.Cancel = true;
                }
            }
            else if (!YeniKayit)
            {
                if (_mdl.UretimOperasyonHareketler == null || _mdl.UretimOperasyonHareketler.Count < 1)
                {
                    if (!MesajSor("Üretim Başlatılmamış Çıkmak istediğinizden eminmisiniz.."))
                    {
                        e.Cancel = true;
                    }
                }
            }
        }
        private bool Bagla()
        {
            return BaglaSiparisden();
        }
        private bool BaglaSiparisden()
        {
            if (IdGuid.IsNullOrEmpty())
            {
                if (SipId == null || SipId == Guid.Empty)
                {
                    FrmSiparisListesi fsip = new FrmSiparisListesi { SecimIcinAcildi = true };
                    fsip.Turu = UretimTuru;
                    fsip.ShowDialog();
                    if (!fsip.Secildi)
                    {
                        MesajHata("sipariş seçilmemiş işemri oluşturulamadı.");
                        this.Close();
                        return false;
                    }
                    SipId = ((Siparis)fsip.SecilenRow).Id;
                }
                var rsvarmi = Ortak.DbPro.UretimEmri.SelectFirst(c => c.SipId == SipId);
                if (!rsvarmi.Success)
                {
                    MesajHata(rsvarmi.Message);
                    return false;
                }
                if (rsvarmi.Data != null)
                {
                    IdGuid = rsvarmi.Data.Id;
                    BaglaEdit();
                }
                else
                {
                    BaglaYeni();
                }
            }
            else
            {
                BaglaEdit();
            }
            return true;
        }
        private void BaglaYeni()
        {
            var rs = _mng.GetUretimSiparisNew(SipId);
            if (!rs.Success)
            {
                MesajHata(rs.Message);
                return;
            }
            _mdl = rs.Data;
            if (_mdl.UretimOperasyonlar.Count < 1)
            {
                OperasyonlarOlustur();
                //if (MesajSor("Üretime ait operasyonlar oluşturulsunmu"))
                //{ }

            }
            YeniKayit = true;
            TemizleText();
            CmbTuru.Text = UretimTuru;
        }
        private bool BaglaEditAktar()
        {
            var rss = BaglaEdit();
            GridBagla();
            GridBaglaStok();
            GridBaglaIstasyon();
            return rss;
        }
        private bool BaglaEdit()
        {
            YeniKayit = false;
            var rs = _mng.GetUretimSiparisEdit(IdGuid);
            if (!rs.Success)
            {
                MesajHata(rs.Message);
                return false;
            }
            _mdl = rs.Data;
            bs.DataSource = _mdl;
            bs.EndEdit();
            AktarTextlere();
            return true;
        }
        private void BaglaTuru()
        {
            List<string> lis = new List<string> { "Siparis", "Recete", "MikroSiparis" };
            CmbTuru.MyDataBagla(lis);
            CmbTuru.Text = UretimTuru;
        }
        private void BaglaDurum()
        {
            List<string> lis = new List<string>
            {
                "Beklemede",
                "Uretimde",
                "Hazır",
                "Beklemede/Uretimde",
                "Beklemede/Hazır",
                "Uretimde/Hazır",
                "Beklemede/Uretimde/Hazır"
            };
            CmbDurumu.MyDataBagla(lis);
            CmbDurumu.Text = "Beklemede";
        }
        private void OperasyonlarOlustur()
        {
            foreach (var siphr in _mdl.SiparisModel.Hareketler)
            {
                foreach (var oprmod in _mdl.OperasyonModeller)
                {
                    if (oprmod.Recete.Id == siphr.RcAId)
                    {
                        foreach (var oprs in oprmod.Operasyonlar)
                        {
                            _mdl.UretimOperasyonlar.Add(new UretimOperasyon()
                            {
                                Id = MyGuid.NewGuid(),
                                IsEmriNo = "",
                                BaslangicTarihi = DateTime.Now,
                                BitisTarihi = null,
                                Durumu = "Beklemede",
                                OperasyonKodu = oprs.OperasyonKodu,
                                OperasyonAdi = oprs.OperasyonAdi,
                                RcOId = oprs.Id,
                                RcAId = oprs.RcAId,
                                SipId = siphr.SipId,
                                SipHId = siphr.Id,
                                UrId = _mdl.UretimEmri.Id,
                                PlanlananMiktar = siphr.Miktar,
                                ReceteKodu = siphr.ReceteKodu,
                                ReceteAdi = siphr.ReceteAdi,
                                Sira = oprs.Sira
                            });
                        }
                    }
                }
            }
        }
        private bool UretimBaslat()
        {
            if (_mdl.UretimOperasyonHareketler == null || _mdl.UretimOperasyonHareketler.Count < 1)
            {
                if (_mdl.UretimOperasyonHareketler == null)
                {
                    _mdl.UretimOperasyonHareketler = new List<UretimOperasyonHareket>();
                }
                foreach (var oprs in _mdl.UretimOperasyonlar.Where(c => c.Sira <= 1))
                {
                    _mdl.UretimOperasyonHareketler.Add(new UretimOperasyonHareket()
                    {
                        Id = MyGuid.NewGuid(),
                        BaslangicTarihi = _mdl.UretimEmri.BaslangicTarihi,
                        PlanlananMiktar = oprs.PlanlananMiktar,
                        KalanMiktar = oprs.PlanlananMiktar,
                        UrId = oprs.UrId,
                        UrOId = oprs.Id,
                        RcOId = oprs.RcOId,
                        RcAId = oprs.RcAId,
                        SipId = oprs.SipId,
                        KayitEden = Ortak.KullaniciAdi,
                        KayitTarihi = DateTime.Now,
                        Sira = oprs.Sira
                    });
                }
            }
            else
            {
                bool varmi = false;
                foreach (var oprs in _mdl.UretimOperasyonlar.Where(c => c.Sira <= 1))
                {
                    varmi = false;
                    foreach (var itm in _mdl.UretimOperasyonHareketler)
                    {
                        if (itm.UrOId == oprs.Id)
                        {
                            varmi = true;
                        }
                    }
                    if (!varmi)
                    {
                        _mdl.UretimOperasyonHareketler.Add(new UretimOperasyonHareket()
                        {
                            Id = MyGuid.NewGuid(),
                            BaslangicTarihi = _mdl.UretimEmri.BaslangicTarihi,
                            PlanlananMiktar = oprs.PlanlananMiktar,
                            KalanMiktar = oprs.PlanlananMiktar,
                            UrId = oprs.UrId,
                            UrOId = oprs.Id,
                            RcOId = oprs.Id,
                            RcAId = oprs.RcAId,
                            SipId = oprs.SipId,
                            KayitEden = Ortak.KullaniciAdi,
                            KayitTarihi = DateTime.Now,
                            Sira = oprs.Sira
                        });
                    }
                }
            }
            var rs = _mng.UretimOperasyonHareketKaydet(_mdl.UretimOperasyonHareketler);
            if (!rs.Success)
            {
                MesajHata(rs.Message);
                return false;
            }
            else
            {
                var rr = BaglaEditAktar();
                return rr;
            }
        }
        private void GridBagla()
        {
            myGrid1.DataSource = null;
            myGrid1.DataSource = _mdl.UretimOperasyonlar;
        }

        private void GridBaglaStok()
        {
            myGrid2.DataSource = null;
            myGrid2.DataSource = _mdl.UretimStoklar;
        }
        private void GridBaglaIstasyon()
        {
            myGrid3.DataSource = null;
            myGrid3.DataSource = _mdl.UretimOperasyonHareketler;
        }
        private void GridBaglaAcilis()
        {
            SutunGizle();
            myGrid1.GridYerlesimYukle();
        }
        private void SutunGizle()
        {
            myView1.SutunGizle("Id");
            myView1.SutunGizle("UrId");
            myView1.SutunGizle("RcAId");
            myView1.SutunGizle("RcOId");
            myView1.SutunGizle("SipId");
            myView1.SutunGizle("SipHId");

        }
        private void GridBaglaAcilisStok()
        {
            SutunGizle2();
            myGrid2.GridYerlesimYukle();
        }
        private void SutunGizle2()
        {
            myView2.SutunGizle("Id");
            myView2.SutunGizle("UrId");
            myView2.SutunGizle("RcAId");
            myView2.SutunGizle("RcDId");
            myView2.SutunGizle("SipId");
            myView2.SutunGizle("SipHId");

        }
        private void GridBaglaAcilisIstasyon()
        {
            SutunGizle3();
            myGrid3.GridYerlesimYukle();
        }
        private void SutunGizle3()
        {
            myView3.SutunGizle("Id");
            myView3.SutunGizle("UrId");
            myView3.SutunGizle("UrOId");
            myView3.SutunGizle("RcAId");
            myView3.SutunGizle("RcOId");
            myView3.SutunGizle("SipId");

        }
        private void EvrakNoAl()
        {
            var rs = _srvGenel.GetEvrakNo("UretimEmri");
            if (!rs.Success)
            {
                MesajHata(rs.Message);
                return;
            }
            TxtIsEmriNo.Text = rs.Data;
        }
        private void AktarTextlere()
        {
            TxtSiparisKodu.Text = _mdl.UretimEmri.SiparisKodu;
            TxtCariKodu.Text = _mdl.UretimEmri.SiparisCariKodu;
            TxtCariUnvani.Text = _mdl.UretimEmri.SiparisCariUnvani;
            TxtAciklama.Text = _mdl.UretimEmri.Aciklama;
            TxtTarih.Text = _mdl.UretimEmri.BaslangicTarihi.ToString();
            if (_mdl.UretimEmri.BitisTarihi != null)
            {
                TxtTarih2.Text = _mdl.UretimEmri.BitisTarihi.ToString();
            }
            else
            {
                TxtTarih2.Text = "";
            }
            CmbTuru.Text = _mdl.UretimEmri.Turu;
            CmbDurumu.Text = _mdl.UretimEmri.Durumu;
            TxtIsEmriNo.Text = _mdl.UretimEmri.IsEmriNo;
            ChcKapandi.Checked = _mdl.UretimEmri.Kapandi;
            TxtIstasyonGrubu.Text = _mdl.UretimEmri.IstasyonGrupKodu;
        }
        private void AktarModele()
        {
            if (_mdl.SiparisModel?.Siparis != null)
            {
                SipId = _mdl.SiparisModel.Siparis.Id;
            }
            _mdl.UretimEmri.SiparisKodu = TxtSiparisKodu.Text;
            _mdl.UretimEmri.SiparisCariKodu = TxtCariKodu.Text;
            _mdl.UretimEmri.SiparisCariUnvani = TxtCariUnvani.Text;
            _mdl.UretimEmri.Aciklama = TxtAciklama.Text;
            _mdl.UretimEmri.BaslangicTarihi = Convert.ToDateTime(TxtTarih.Text);
            if (string.IsNullOrEmpty(TxtTarih2.Text.Trim()))
            {
                _mdl.UretimEmri.BitisTarihi = null;
            }
            else
            {
                _mdl.UretimEmri.BitisTarihi = Convert.ToDateTime(TxtTarih2.Text);
            }
            _mdl.UretimEmri.Turu = CmbTuru.Text;
            _mdl.UretimEmri.Durumu = CmbDurumu.Text;
            _mdl.UretimEmri.IsEmriNo = TxtIsEmriNo.Text;
            _mdl.UretimEmri.Kapandi = ChcKapandi.Checked;
            _mdl.UretimEmri.SipId = SipId;
            _mdl.UretimEmri.IstasyonGrupKodu = TxtIstasyonGrubu.Text;
            if (string.IsNullOrEmpty(_mdl.UretimEmri.Durumu))
            {
                _mdl.UretimEmri.Durumu = "Beklemede";
            }
            foreach (var itm in _mdl.UretimOperasyonlar)
            {
                itm.UrId = _mdl.UretimEmri.Id;
                itm.IsEmriNo = TxtIsEmriNo.Text;
                itm.SipId = SipId;
                itm.BaslangicTarihi = _mdl.UretimEmri.BaslangicTarihi;
            }
            foreach (var itm in _mdl.UretimOperasyonHareketler)
            {
                itm.BaslangicTarihi = _mdl.UretimEmri.BaslangicTarihi;
            }
            foreach (var itm in _mdl.UretimStoklar)
            {
                itm.UrId = _mdl.UretimEmri.Id;
                itm.SipId = SipId;
                // adet += itm.Miktar;
            }
        }
        private void TemizleText()
        {
            TxtSiparisKodu.Text = _mdl.SiparisModel.Siparis.SiparisKodu;
            TxtCariKodu.Text = _mdl.SiparisModel.Siparis.CariKodu;
            TxtCariUnvani.Text = _mdl.SiparisModel.Siparis.CariUnvani;
            TxtAciklama.Text = "";
            TxtTarih.Text = DateTime.Now.ToString();
            TxtTarih2.Text = "";
            ChcKapandi.Checked = false;
            CmbTuru.Text = "Siparis";
            CmbDurumu.Text = "Beklemede";

        }
        private bool TextLeriKontrolEt()
        {
            //Guid rcaid = Guid.Empty;
            //foreach (var itm in _mdl.UretimOperasyonlar)
            //{
            //    if (rcaid==Guid.Empty)
            //    {
            //        rcaid =(Guid) itm.RcAId;
            //    }else if (rcaid!=Guid.Empty)
            //    {

            //    } 
            //}

            Guid? rcaid = Guid.Empty;
            foreach (var itm in _mdl.ReceteModeller)
            {
                if (itm.Recete.IstasyonGruplamaKullan)
                {
                    if (rcaid == Guid.Empty)
                    {
                        rcaid = itm.Recete.Id;
                    }
                    else if (rcaid != itm.Recete.Id)
                    {
                        MesajHata("İstasyon Gruplama Kullanılacaksa Tek Reçete Seçilebilir. Üretime Lütfen Tek Reçete Giriniz.");
                        return false;
                    }
                }
            }


            foreach (var itm in _mdl.ReceteModeller) {
                var rs = _srvRct.SelectFind(itm.Recete.Id);
                if (!rs.Success) {
                    MesajHata(rs.Message);
                    return false;
                }
                if (rs.Data.IstasyonGruplamaKullan && string.IsNullOrEmpty(TxtIstasyonGrubu.Text)) {
                    MesajHata("Lütfen İstasyon Grubu Seçiniz.");
                    return false;
                }
            } 

            if (string.IsNullOrEmpty(TxtIsEmriNo.Text))
            {
                EvrakNoAl();
            }

            if (string.IsNullOrEmpty(TxtSiparisKodu.Text))
            {
                MesajHata("Lütfen Sipariş kodunu giriniz");
                return false;
            }
            if (string.IsNullOrEmpty(TxtIsEmriNo.Text))
            {
                MesajHata("Lütfen İş Emri kodunu giriniz");
                return false;
            }
            return true;
        }
        private void Kaydet()
        {
            if (_mdl.UretimOperasyonlar.Count <= 0)
            {
                MesajBilgi("Operasyon Bulunamadı Üretim Kayıt Edilemez");
                return;
            }
            if (!TextLeriKontrolEt())
            {
                return;
            }
            AktarModele();
            var rs = _mng.UretimEmriKaydetBySiparis(_mdl, YeniKayit);
            if (rs.Success)
            {
                KayitEdildi = true;
                ActionAktar?.Invoke();
                IdGuid = rs.Data.Id;
                BaglaEditAktar();
            }
            else
            {
                MesajHata(rs.Message);
            }
        }
        private bool KaydetBaslat()
        {
            if (_mdl.UretimOperasyonlar.Count <= 0)
            {
                MesajBilgi("Operasyon Bulunamadı Üretim Kayıt Edilemez");
                return false;
            }
            if (!TextLeriKontrolEt())
            {
                return false;
            }
            
            AktarModele(); 
            var rs = _mng.UretimEmriKaydetBySiparis(_mdl, YeniKayit);
            if (rs.Success)
            {
                KayitEdildi = true;
                ActionAktar?.Invoke();
                IdGuid = rs.Data.Id;
                var r1 = BaglaEditAktar();
                if (!r1) return false;
                var r2 = UretimBaslat();
                if (!r2) return false;
                return true;
            }
            else
            {
                MesajHata(rs.Message);
                return false;
            }
        }

        private void Sil()
        {
            if (_mdl.UretimOperasyonHareketler.Count > 0)
            {
                MesajHata("Operasyona baglı üretimi başlatılmış hareketler var önce onları siliniz");
                return;
            }
            if (!MesajSor("Kaydı silmek istiyormusunuz.."))
            {
                return;
            }
            var rs = _mng.UretimEmriSil(_mdl);
            if (rs.Success)
            {
                ActionAktar?.Invoke();
                KayitEdildi = true;
                this.Close();
            }
            else
            {
                MesajHata(rs.Message);
            }
        }
        private void OperasyonHareketSil()
        {
            if (!MesajSor("Kayıtları silmek istiyormusunuz.."))
            {
                return;
            }
            var hata = false;
            foreach (var itm in _mdl.UretimOperasyonHareketler)
            {
                var rssor = _mng.UretimOperasyonHareketKayitVarmi(itm.Id);
                if (!rssor.Success)
                {
                    hata = true;
                    MesajHata(rssor.Message);
                    break;
                }
                if (rssor.Data > 0)
                {
                    MesajBilgi(itm.OperasyonKodu + " operasyona bagli hareketler var kayıt silinemez..");
                    continue;
                }
                var rs = _mng.UretimOperasyonHareketSil(itm.Id);
                if (!rs.Success)
                {
                    hata = true;
                    MesajHata(rs.Message);
                    break;
                }
            }
            if (!hata)
            {
                BaglaEditAktar();
            }
        }
        private void OperasyonaAitTumKayitlariSil()
        {
            if (!MesajSor("Uretim Emrine bağlı  Kayıtları silmek istiyormusunuz?"))
            {
                return;
            }
            if (!MesajSor("Uretim Emrine bağlı tüm Üretim ve istasyon kayıtlarını  silmek istiyormusunuz?"))
            {
                return;
            }
            var rs = _mng.UretimeBagliTumHareketleriSil(_mdl.UretimEmri.Id);
            if (!rs.Success)
            {
                MesajHata(rs.Message);
                return;
            }
            BaglaEditAktar();
        }
        private void TxtIsEmriNo_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (!string.IsNullOrEmpty(TxtIsEmriNo.Text))
            {
                if (!MesajSor("İşEmri Kodunu Değiştirmek istiyormusunuz"))
                {
                    return;
                }
            }
            EvrakNoAl();
        }
        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            Kaydet();
        }
        private void BtnSil_Click(object sender, EventArgs e)
        {
            Sil();
        }
        private void BtnUretimeBasla_Click(object sender, EventArgs e)
        {
            var rss = KaydetBaslat();
            if (rss)
            {
                this.Close();
            }
        }
        private void BtnUretimiSil_Click(object sender, EventArgs e)
        {
            OperasyonHareketSil();
        }
        private void BtnUretimeAitTumKayitlariSil_Click(object sender, EventArgs e)
        {
            OperasyonaAitTumKayitlariSil();
        }
        private void GridView1_RowCellStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowCellStyleEventArgs e)
        {
            //if (e.Column.Name=="colGC" && e.CellValue.ToString() == "1")
            //{
            //    var x = e.RowHandle;
            //    e.Appearance.BackColor = Color.Red;
            //}
        }
        private void GridView_RowStyle(object sender, RowStyleEventArgs e)
        {
            GridView View = sender as GridView;
            if (e.RowHandle >= 0)
            {
                string priority = View.GetRowCellDisplayText(e.RowHandle, View.Columns["Durumu"]);
                if (priority == "Hazir")
                {
                    e.Appearance.BackColor = Color.FromArgb(200, Color.DarkGreen);

                    e.Appearance.ForeColor = Color.LightGray;
                }
                else if (priority == "Uretimde")
                {
                    e.Appearance.BackColor = Color.FromArgb(200, Color.Indigo);
                    e.Appearance.ForeColor = Color.LightGray;
                }
                e.HighPriority = true;
            }
        }

        private void TxtIstasyonGrubu_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            FrmReceteIstasyonGrupIstasyonEslestir f = new FrmReceteIstasyonGrupIstasyonEslestir();
            f.SecimIcinAcildi = true;
            f.RcAId = (Guid) _mdl.ReceteModeller.FirstOrDefault().Recete.Id;
            f.ShowDialog();
            if (f.Secildi)
            {
                TxtIstasyonGrubu.Text = f.SecilenKod;
            }
        }

        private void TxtCariKodu_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e) {
            FrmMikroCariListesi f = new FrmMikroCariListesi();
            f.SecimIcinAcildi = true;
            f.ShowDialog();
            if (f.Secildi) {
                var rw = (MikroCari)f.SecilenRow;
                TxtCariKodu.Text = rw.CariKodu;
                TxtCariUnvani.Text = rw.CariUnvani1;
            }
        }

        private void BtnCariTemizle_Click(object sender, EventArgs e) {
            TxtCariKodu.Text = "";
            TxtCariUnvani.Text = "";
        }
    }
}