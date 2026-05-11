using DevExpress.XtraEditors;
using My.Business.Manager;
using My.Business.Service.Geneller;
using My.Business.Service.MikroModul;
using My.Core;
using My.Entities.IstasyonKartlar;
using My.Entities.Mikro;
using My.Entities.Models;
using My.Entities.ReceteIstasyonlar;
using My.Entities.Receteler;
using My.Entities.ReceteStoklar;
using My.Kontrol.Formlar;
using MyUI.MikroModule;
using MyUI.ReceteIstasyonGrupModul;
using MyUI.ReceteModul;
using MyUI.UretimIstasyonModule;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MyUI.ReceteModule
{
    public partial class FrmReceteED : MyFrmKayit
    {
        private readonly IGenelService _srvGenel = Ortak.DbPro.GenelServis;
        private readonly IMikroStokService _mikroStokService = Ortak.DbMikro.Stoklar;
        private ReceteManager _mng;
        private MikroReceteManager _mngMikro;
        private ReceteKayitModel _mdl;
        private int receteSira = 0;
        public bool MikrodanAktar = false;
        public string MikroReceteKodu = "";

        //public List<MikroStokCinsi> stokCinsiList = new List<MikroStokCinsi>();
        public List<MikroStokCinsi> stokCinsiListFull = new List<MikroStokCinsi>();

        public FrmReceteED()
        {
            InitializeComponent();
            EventlerBagla();
        }
        private void EventlerBagla()
        {
            this.Load += Frm_Load;
            myView1.FocusedRowChanged += MyView1_FocusedRowChanged;
            myView1.MyEventDoubleClickEnter += MyView1_MyEventDoubleClickEnter;
            myView2.MyEventDoubleClickEnter += MyView2_MyEventDoubleClickEnter;
            myView1.ShownEditor += MyView1_ShownEditor;
        }
        private void Frm_Load(object sender, EventArgs e)
        {
            _mng = new ReceteManager(Ortak.DbPro);
            _mngMikro = new MikroReceteManager(Ortak.DbPro, Ortak.DbMikro);
            BaglaGrup();
            BaglaStokCinsi();
            if (MikrodanAktar)
            {
                if (string.IsNullOrEmpty(MikroReceteKodu))
                {
                    MesajHata("Mikro recete kodu boş olamaz.");
                    return;
                }
                BaglaMikroRecete();
            }
            else
            {
                Bagla();
            }
        }
        private void Bagla()
        {
            if (IdGuid.IsNullOrEmpty())
            {
                YeniKayit = true;
                _mdl = _mng.GetReceteKayit();
                TemizleText();
            }
            else
            {
                YeniKayit = false;
                var rs = _mng.GetReceteKayit(IdGuid);
                if (!rs.Success)
                {
                    MesajHata(rs.Message);
                    return;
                }
                _mdl = rs.Data;
                foreach (var itm in _mdl.ReceteDetaylar)
                {
                    if (receteSira < itm.ReceteSira)
                    {
                        receteSira = itm.ReceteSira;
                    }
                }
                AktarTextlere();
            }
        }


        public void BaglaStokCinsi()
        {

            /*  stokCinsiList MikroStokCinsiManager.GetCinsList();*/
            stokCinsiListFull = MikroStokCinsiManager.GetCinsListFull();
            CmbStokCinsi.MyDataBagla(stokCinsiListFull, "Kodu", "Adi", new int[] { 1, 2 });
            CmbStokCinsi.Text = "Mamül";
        }
        public void StokDanCinsiBagla(string stokKodu)
        {
            //  kodundan cinsini bul fullden adiniyaz
            var rs = _mikroStokService.GetViewListWhere(" where S.sto_kod ='" + stokKodu + "'", Ortak.MikroStokGrubu);
            if (!rs.Success)
            {
                MesajHata(rs.Message);
                return;
            }
            var st = rs.Data.FirstOrDefault();
            if (st == null) return;
            int cinsiKodu = Convert.ToInt32(st.StokCinsi);
            foreach (var itm in stokCinsiListFull)
            {
                if (cinsiKodu == itm.Kodu)
                {
                    CmbStokCinsi.Text = itm.Adi;
                }
            }

        }
        private void BaglaMikroRecete()
        {
            YeniKayit = true;
            _mdl = _mng.GetReceteKayit();
            TemizleText();
            IMikroStokService _srv = Ortak.DbMikro.Stoklar;
            var rsMikroStok = _srv.GetViewListWhere(" where S.sto_kod = '" + MikroReceteKodu + "' ", Ortak.MikroStokGrubu);
            if (!rsMikroStok.Success)
            {
                MesajHata(rsMikroStok.Message);
                return;
            }
            var st = rsMikroStok.Data.FirstOrDefault();
            if (st != null)
            {
                TxtEntegreStokKodu.Text = st.StokKodu;
                TxtEntegreStokAdi.Text = st.StokAdi;
                TxtEntegreBirim.Text = st.Birim;
                TxtModelKodu.Text = st.ModelKodu;
            }
            var rsMikRc = _mngMikro.GetMikroReceteList(" where rec_anakod ='" + MikroReceteKodu + "'");
            if (!rsMikRc.Success)
            {
                MesajHata(rsMikRc.Message);
                return;
            }
            var rc = rsMikRc.Data.FirstOrDefault();
            if (rc != null)
            {
                TxtReceteKodu.Text = MikroReceteKodu;
                TxtReceteAdi.Text = rc.ReceteAdi;
            }
            var rsMikRcHr = _mngMikro.GetMikroReceteHareketler(MikroReceteKodu);
            if (!rsMikRcHr.Success)
            {
                MesajHata(rsMikRcHr.Message);
                return;
            }
            foreach (var itm in rsMikRcHr.Data)
            {
                var Detay = new ReceteDetay
                {
                    Cinsi = "MikroRecete",
                    Birim = itm.Birimi,
                    Miktar = itm.Miktar,
                    ReceteSira = ++receteSira,
                    StokTuru = "Sabit",
                    VarsayilanStokAdi = itm.StokAdi,
                    VarsayilanStokKodu = itm.StokKodu,
                    Renk = "",
                    Beden = "",
                    Ebat = "",
                    Gram = "",
                    Olcu = "",
                    Aciklama = "",
                    StokAnaGrup = "",
                    StokKullan = true,
                    SiparisdeGosterme = true,
                    FireYuzde = itm.FireYuzde
                };
                _mdl.ReceteDetaylar.Add(Detay);
            }
            GridBagla();
            Grid2Bagla();
        }
        private void TemizleText()
        {
            TxtReceteKodu.Text = "";
            TxtReceteAdi.Text = "";
            CmbReceteGrubu.Text = "";
            TxtAciklama.Text = "";
            txtAmbalajSekli.Text = "";
            TxtEntegreStokKodu.Text = "";
            TxtEntegreStokAdi.Text = "";
            TxtEntegreBirim.Text = "";
            TxtModelKodu.Text = "";
            CmbStokCinsi.Text = "Mamül";
            ChcHaziriSonrakiIstasyonaGonder.Checked = false;
            ChcIstasyonGruplamaKullan.Checked = false;
            ChcAparatZorunlu.Checked = false;
            ChcOlcumZorunlu.Checked = false;
            TxtRafOmru.Text = "0";
            GridBagla();
            Grid3Bagla();
        }
        private void AktarTextlere()
        {
            IdGuid = _mdl.Recete.Id;
            TxtReceteKodu.Text = _mdl.Recete.ReceteKodu;
            TxtReceteAdi.Text = _mdl.Recete.ReceteAdi;
            CmbReceteGrubu.Text = _mdl.Recete.Grubu;
            TxtAciklama.Text = _mdl.Recete.Aciklama;
            txtAmbalajSekli.Text = _mdl.Recete.AmbalajSekli;
            TxtEntegreStokKodu.Text = _mdl.Recete.EntegreStokKodu;
            TxtEntegreStokAdi.Text = _mdl.Recete.EntegreStokAdi;
            TxtEntegreBirim.Text = _mdl.Recete.EntegreBirim;
            TxtModelKodu.Text = _mdl.Recete.ModelKodu;
            TxtRafOmru.Text = _mdl.Recete.RafOmru.ToString();
            ChcHaziriSonrakiIstasyonaGonder.Checked = _mdl.Recete.HaziriSonrakiIstasyonaGonder;
            ChcIstasyonGruplamaKullan.Checked = _mdl.Recete.IstasyonGruplamaKullan;
            ChcAparatZorunlu.Checked = _mdl.Recete.AparatZorunlu;
            ChcOlcumZorunlu.Checked = _mdl.Recete.OlcumZorunlu;
            if (string.IsNullOrEmpty(_mdl.Recete.EntegreStokKodu))
            {
                CmbStokCinsi.Text = _mdl.Recete.StokCinsiAdi;
            }
            else
            {
                StokDanCinsiBagla(TxtEntegreStokKodu.Text);
            }



            GridBagla();
            Grid2Bagla();
            Grid3Bagla();

        }
        private void GridBagla()
        {
            myGrid1.DataSource = null;
            bs.DataSource = null;
            bs.DataSource = _mdl.ReceteDetaylar;
            myGrid1.DataSource = bs;
            SutunGizle();
            myGrid1.GridYerlesimYukle(myGrid1.MyGridKayitAdi);
            myView1.Columns["ReceteSira"].OptionsColumn.AllowEdit = true;
            myView1.Columns["ReceteSira"].AppearanceHeader.BackColor = Color.Green;
            myView1.Columns["Miktar"].OptionsColumn.AllowEdit = true;
            myView1.Columns["Miktar"].AppearanceHeader.BackColor = Color.Green;
            myView1.Columns["Ebat"].OptionsColumn.AllowEdit = true;
            myView1.Columns["Ebat"].AppearanceHeader.BackColor = Color.Green;
            myView1.Columns["Gram"].OptionsColumn.AllowEdit = true;
            myView1.Columns["Gram"].AppearanceHeader.BackColor = Color.Green;
            myView1.Columns["Olcu"].OptionsColumn.AllowEdit = true;
            myView1.Columns["Olcu"].AppearanceHeader.BackColor = Color.Green;
            myView1.Columns["FireYuzde"].OptionsColumn.AllowEdit = true;
            myView1.Columns["FireYuzde"].AppearanceHeader.BackColor = Color.Green;
        }
        private void MyView1_ShownEditor(object sender, EventArgs e)
        {
            try
            {
                TextEdit edit = myView1.ActiveEditor as TextEdit;
                if (edit == null) return;
                if (edit.Text.Length > 0) edit.SelectAll();
            }
            catch { }
        }
        private void SutunGizle()
        {
            myView1.SutunGizle("Id");
            myView1.SutunGizle("RcAId");

        }
        private void Grid2Bagla()
        {
            var data = myView1.MyGetCurrentItem<ReceteDetay>();
            if (data == null)
            {
                return;
            }
            var data2 = _mdl.ReceteStoklar.Where(c => c.RcDId == data.Id).ToList();
            bss.DataSource = data2;
            myGrid2.DataSource = bss;
            SutunGizle2();
            myView2.Columns["Ebat"].OptionsColumn.AllowEdit = true;
            myView2.Columns["Ebat"].AppearanceHeader.BackColor = Color.Green;
            myView2.Columns["Gram"].OptionsColumn.AllowEdit = true;
            myView2.Columns["Gram"].AppearanceHeader.BackColor = Color.Green;
            myView2.Columns["Olcu"].OptionsColumn.AllowEdit = true;
            myView2.Columns["Olcu"].AppearanceHeader.BackColor = Color.Green;
            myView2.SutunBackColor("Miktar", Color.Green);
            myView2.SutunEditAc("Miktar");

            myGrid2.GridYerlesimYukle();
            myGrid2.Refresh();
            myView2.RefreshData();
        }
        private void Grid3Bagla()
        {

            myGrid3.DataSource = null;
            bsIs.DataSource = null;
            bsIs.DataSource = _mdl.ReceteyeBagliIstasyonlar;
            myGrid3.DataSource = bsIs;
            SutunGizle3();
            myGrid3.GridYerlesimYukle();
        }
        private void SutunGizle2()
        {
            myView2.SutunGizle("Id");
            myView2.SutunGizle("RcAId");
            myView2.SutunGizle("RcDId");
        }
        private void SutunGizle3()
        {
            myView3.SutunGizle("Id");
            myView3.SutunGizle("RcAId");
            myView3.SutunGizle("RcIId");

        }
        private void BaglaGrup()
        {
            var rs = _srvGenel.GrupListesi("ReceteAna", "Grubu");
            if (!rs.Success)
            {
                MesajHata(rs.Message);
                return;
            }
            CmbReceteGrubu.MyDataBagla(rs.Data.ToList());
        }
        private void AktarModele()
        {
            if (IdGuid == Guid.Empty) IdGuid = MyGuid.NewGuid();
            _mdl.Recete.Id = IdGuid;
            _mdl.Recete.ReceteKodu = TxtReceteKodu.Text;
            _mdl.Recete.ReceteAdi = TxtReceteAdi.Text;
            _mdl.Recete.Grubu = CmbReceteGrubu.Text;
            _mdl.Recete.Aciklama = TxtAciklama.Text;
            _mdl.Recete.AmbalajSekli = txtAmbalajSekli.Text;
            _mdl.Recete.EntegreStokKodu = TxtEntegreStokKodu.Text;
            _mdl.Recete.EntegreStokAdi = TxtEntegreStokAdi.Text;
            _mdl.Recete.EntegreBirim = TxtEntegreBirim.Text;
            _mdl.Recete.ModelKodu = TxtModelKodu.Text;
            if (string.IsNullOrEmpty(TxtRafOmru.Text)) {
                TxtRafOmru.Text = "0";
            }
            _mdl.Recete.RafOmru = Convert.ToInt32(TxtRafOmru.Text);
            _mdl.Recete.HaziriSonrakiIstasyonaGonder = ChcHaziriSonrakiIstasyonaGonder.Checked;
            _mdl.Recete.IstasyonGruplamaKullan = ChcIstasyonGruplamaKullan.Checked;
            _mdl.Recete.AparatZorunlu = ChcAparatZorunlu.Checked;
            _mdl.Recete.OlcumZorunlu = ChcOlcumZorunlu.Checked;

            if (string.IsNullOrEmpty(_mdl.Recete.KayitEden))
            {
                _mdl.Recete.KayitEden = Ortak.KullaniciAdi;
            }

            if (string.IsNullOrEmpty(_mdl.Recete.Degistiren))
            {
                _mdl.Recete.Degistiren = Ortak.KullaniciAdi;
            }

            if (_mdl.Recete.KayitTarihi == null)
            {
                _mdl.Recete.KayitTarihi = DateTime.Now;
            }
            _mdl.Recete.DegistirmeTarihi = DateTime.Now;


            if (_mdl.Recete.Id.IsNullOrEmpty())
            {
                _mdl.Recete.Id = MyGuid.NewGuid();
            }
            foreach (var dty in _mdl.ReceteDetaylar)
            {
                dty.RcAId = _mdl.Recete.Id;

            }
            foreach (var sto in _mdl.ReceteStoklar)
            {
                sto.RcAId = _mdl.Recete.Id;

            }
            foreach (var sto in _mdl.ReceteyeBagliIstasyonlar)
            {
                sto.RcAId = _mdl.Recete.Id;

            }
        }
        private void EvrakNoAl()
        {
            var rs = _srvGenel.GetEvrakNo("Recete");
            if (!rs.Success)
            {
                MesajHata(rs.Message);
                return;
            }
            TxtReceteKodu.Text = rs.Data;
        }
        private void Kaydet()
        {
            bool stokCinsiBulundu = false;
            foreach (var itm in stokCinsiListFull)
            {
                if (itm.Adi == CmbStokCinsi.Text.ToString())
                {
                    _mdl.Recete.StokCinsiKodu = itm.Kodu;
                    _mdl.Recete.StokCinsiAdi = itm.Adi;
                    stokCinsiBulundu = true;
                }
            }

            if (!stokCinsiBulundu)
            {
                _mdl.Recete.StokCinsiKodu = -1;
            }
            if (!TextLeriKontrolEt())
            {
                return;
            }
            AktarModele();

            // aynı entegre stokkodu daha önce girilmişmi kontrol et

            var rsKontrol = _mng.ReceteStokKoduDahaonceGirilmismi(_mdl.Recete.Id, _mdl.Recete.EntegreStokKodu);

            if (!rsKontrol.Success)
            {
                MesajBilgi(rsKontrol.Message);
                return;
            }

            var rs = _mng.ReceteKaydet(_mdl, YeniKayit);
            if (rs.Success)
            {
                KayitEdildi = true;

                MesajBilgi("Kayıt Edildi");
                IdGuid = rs.Data.Recete.Id;
                MikrodanAktar = false;
                MikroReceteKodu = "";
                Bagla();
            }
            else
            {
                MesajHata(rs.Message);
            }
        }
        private void Sil()
        {
            if (!MesajSor("Kaydı silmek istiyormusunuz.."))
            {
                return;
            }
            var rs = _mng.ReceteSil(_mdl);
            if (rs.Success)
            {
                KayitEdildi = true;
                this.Close();
            }
            else
            {
                MesajHata(rs.Message);
            }
        }
        private bool TextLeriKontrolEt()
        {
            if (_mdl.ReceteDetaylar.Count <= 0)
            {
                MesajHata("Lütfen Reçete Detay giriniz");
                return false;
            }
            if (string.IsNullOrEmpty(TxtReceteKodu.Text))
            {
                EvrakNoAl();
            }
            if (string.IsNullOrEmpty(TxtReceteKodu.Text))
            {
                MesajHata("Lütfen Reçete kodunu giriniz");
                return false;
            }
            if (_mdl.Recete.StokCinsiKodu < 0)
            {
                MesajHata("Lütfen Sok Cinsini Seçiniz");
                return false;
            }
            return true;
        }
        private void MyView1_MyEventDoubleClickEnter()
        {
            var data = myView1.MyGetCurrentItem<ReceteDetay>();
            if (data == null)
            {
                return;
            }
            FrmReceteDetayED f = new FrmReceteDetayED { Detay = data, YeniKayit = false };
            f.ShowDialog();
            if (f.KayitEdildi)
            {
                if (f.Detay.ReceteSira > receteSira)
                {
                    receteSira = f.Detay.ReceteSira;
                }
                var donen = f.Detay.Clone();
                if (f.YeniKayit)
                {
                    _mdl.ReceteDetaylar.Add(donen);
                }
                else
                {
                    foreach (var itm in _mdl.ReceteDetaylar)
                    {
                        if (donen.Id == itm.Id)
                        {
                            itm.SetDetay(donen);
                            break;
                        }
                    }
                }
                GridBagla();
            }
        }
        private void MyView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            Grid2Bagla();
        }
        private void MyView2_MyEventDoubleClickEnter()
        {
            var data = myView2.MyGetCurrentItem<ReceteStok>();
            if (data == null) return;
            List<ReceteStokRenkBeden> rblis = new List<ReceteStokRenkBeden>();

            foreach (var itm in _mdl.ReceteStokRenkBedenler)
            {
                if (itm.RcAId == data.RcAId && itm.RcDId == data.RcDId && itm.RcSTId == data.Id)
                {
                    rblis.Add(itm.Clone());
                }
            }
            FrmReceteStokSec fs = new FrmReceteStokSec();
            fs.Edit = true;
            fs.ReceteStok = data;
            fs.RenkBedenListSecilen = rblis;
            fs.ShowDialog();
            if (fs.Secildi)
            {
                var sc = fs.ReceteStok;
                foreach (var itm in _mdl.ReceteStoklar)
                {
                    if (itm.Id == sc.Id)
                    {
                        itm.Renk = sc.Renk;
                        itm.Beden = sc.Beden;
                        itm.Ebat = sc.Ebat;
                        itm.Gram = sc.Gram;
                        itm.Olcu = sc.Olcu;
                    }
                }
                List<ReceteStokRenkBeden> storb = new List<ReceteStokRenkBeden>();
                var stlrb = _mdl.ReceteStokRenkBedenler.Where(c => c.RcSTId != data.Id);
                foreach (var itm in stlrb)
                {
                    storb.Add(itm.Clone());
                }
                _mdl.ReceteStokRenkBedenler.Clear();
                _mdl.ReceteStokRenkBedenler.InsertRange(0, storb);
                _mdl.ReceteStokRenkBedenler.InsertRange(0, fs.RenkBedenListSecilen);

                myView2.RefreshData();
                myGrid2.Refresh();
            }
        }
        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            Kaydet();
        }
        private void BtnSil_Click(object sender, EventArgs e)
        {
            var rs = _mng.ReceteSilKontrol(_mdl.Recete.Id);
            if (!rs.Success)
            {
                MesajHata(rs.Message);
                return;
            }
            Sil();
        }
        private void BtnDegistir_Click(object sender, EventArgs e)
        {
            IdGuid = MyGuid.NewGuid();
            _mdl.Recete.Id = IdGuid;
            _mdl.Recete.ReceteKodu = "";
            Guid? oldguid;
            foreach (var itm in _mdl.ReceteDetaylar)
            {
                oldguid = itm.Id;
                itm.Id = MyGuid.NewGuid();
                itm.RcAId = _mdl.Recete.Id;

                foreach (var itmx in _mdl.ReceteStoklar)
                {
                    if (itmx.RcDId == oldguid) itmx.RcDId = itm.Id;
                    itmx.Id = MyGuid.NewGuid();
                    itmx.RcDId = itm.Id;
                    itmx.RcAId = _mdl.Recete.Id;
                }
            }
            YeniKayit = true;
            AktarTextlere();
            GridBagla();
            Grid2Bagla();
        }
        private void BtnDetayEkle_Click(object sender, EventArgs e)
        {
            FrmReceteDetayED f = new FrmReceteDetayED
            {
                Detay = new ReceteDetay(),
                Sira = receteSira + 1,
                YeniKayit = true
            };
            f.ShowDialog();
            if (f.KayitEdildi)
            {
                if (f.Detay.ReceteSira > receteSira)
                {
                    receteSira = f.Detay.ReceteSira;
                }
                var donen = f.Detay.Clone();
                if (f.YeniKayit)
                {
                    _mdl.ReceteDetaylar.Add(donen);
                }
                else
                {
                    foreach (var itm in _mdl.ReceteDetaylar)
                    {
                        if (donen.Id == itm.Id)
                        {
                            itm.SetDetay(donen);
                            break;
                        }
                    }
                }
                GridBagla();
            }
        }
        private void BtnDetaySil_Click(object sender, EventArgs e)
        {
            if (!MesajSor("Kaydı silmek istiyormusunuz.."))
            {
                return;
            }
            var data = myView1.MyGetCurrentItem<ReceteDetay>();
            if (data == null)
            {
                return;
            }
            var rc = _mdl.ReceteDetaylar.Find(c => c.Id == data.Id);
            _mdl.ReceteDetaylar.Remove(rc);
            GridBagla();
            List<ReceteStok> sto = new List<ReceteStok>();
            var stl = _mdl.ReceteStoklar.Where(c => c.RcDId != data.Id);
            foreach (var itm in stl)
            {
                sto.Add(itm.Clone());
            }
            _mdl.ReceteStoklar.Clear();
            _mdl.ReceteStoklar.InsertRange(0, sto);

            List<ReceteStokRenkBeden> storb = new List<ReceteStokRenkBeden>();
            var stlrb = _mdl.ReceteStokRenkBedenler.Where(c => c.RcDId != data.Id);
            foreach (var itm in stlrb)
            {
                storb.Add(itm.Clone());
            }
            _mdl.ReceteStokRenkBedenler.Clear();
            _mdl.ReceteStokRenkBedenler.InsertRange(0, storb);


            Grid2Bagla();
        }
        private void BtnStokEkle_Click(object sender, EventArgs e)
        {
            var data = myView1.MyGetCurrentItem<ReceteDetay>();
            if (data == null)
            {
                return;
            }
            FrmMikroStokListesi f = new FrmMikroStokListesi
            {
                SecimIcinAcildi = true,
                WindowState = FormWindowState.Maximized
            };
            f.ShowDialog();
            if (f.Secildi)
            {
                var st = ((MikroStok)f.SecilenRow).Clone();
                FrmReceteStokSec fs = new FrmReceteStokSec();
                fs.MikroStok = st;

                fs.ReceteStok = new ReceteStok()
                {
                    Id = MyGuid.NewGuid(),
                    RcAId = data.RcAId,
                    RcDId = data.Id
                };
                fs.ShowDialog();
                if (fs.Secildi)
                {
                    var sc = fs.ReceteStok;
                    _mdl.ReceteStoklar.Add(new ReceteStok()
                    {
                        Id = sc.Id,
                        RcAId = data.RcAId,
                        RcDId = data.Id,
                        StokAdi = sc.StokAdi,
                        StokKodu = sc.StokKodu,
                        Renk = sc.Renk,
                        Beden = sc.Beden,
                        Ebat = sc.Ebat,
                        Gram = sc.Gram,
                        Olcu = sc.Olcu,
                    });

                    List<ReceteStokRenkBeden> storb = new List<ReceteStokRenkBeden>();
                    var stlrb = _mdl.ReceteStokRenkBedenler.Where(c => c.RcSTId != data.Id);
                    foreach (var itm in stlrb)
                    {
                        storb.Add(itm.Clone());
                    }
                    _mdl.ReceteStokRenkBedenler.Clear();
                    _mdl.ReceteStokRenkBedenler.InsertRange(0, storb);
                    _mdl.ReceteStokRenkBedenler.InsertRange(0, fs.RenkBedenListSecilen);

                }

                //_mdl.ReceteStoklar.Add(new ReceteStok() {
                //    Id = MyGuid.NewGuid(),
                //    RcAId = data.RcAId,
                //    RcDId = data.Id,
                //    StokAdi = st.StokAdi,
                //    StokKodu = st.StokKodu
                //});
                Grid2Bagla();
            }
        }
        private void BtnStokSil_Click(object sender, EventArgs e)
        {
            if (!MesajSor("Kaydı silmek istiyormusunuz.."))
            {
                return;
            }
            var data = myView2.MyGetCurrentItem<ReceteStok>();
            if (data == null)
            {
                return;
            }
            _mdl.ReceteStoklar.Remove(data);


            List<ReceteStokRenkBeden> storb = new List<ReceteStokRenkBeden>();
            var stlrb = _mdl.ReceteStokRenkBedenler.Where(c => c.RcSTId != data.Id);
            foreach (var itm in stlrb)
            {
                storb.Add(itm.Clone());
            }
            _mdl.ReceteStokRenkBedenler.Clear();
            _mdl.ReceteStokRenkBedenler.InsertRange(0, storb);

            Grid2Bagla();
        }
        private void BtnOperasyonEkle_Click(object sender, EventArgs e)
        {
            if (IdGuid.IsNullOrEmpty())
            {
                MesajBilgi("Reçete Kayıt Edilmeden operasyon Eklenemiyor..");
                return;
            }
            FrmReceteOperasyonED f = new FrmReceteOperasyonED { IdGuid = IdGuid };
            f.ShowDialog();
        }
        private void BtnOperasyonKopyala_Click(object sender, EventArgs e)
        {
            if (IdGuid.IsNullOrEmpty())
            {
                MesajBilgi("Reçete Kayıt Edilmeden operasyon Eklenemiyor..");
                return;
            }
            FrmReceteListesi fsec = new FrmReceteListesi { SecimIcinAcildi = true };
            fsec.ShowDialog();
            if (fsec.Secildi)
            {
                var don = (ReceteAna)fsec.SecilenRow;
                if (don != null)
                {
                    var itm = _mdl.Recete;
                    if (itm != null)
                    {
                        FrmReceteOperasyonED f = new FrmReceteOperasyonED
                        {
                            IdGuid = don.Id,
                            Kolonlanacak = true,
                            KolonRecete = itm.Clone()
                        };
                        f.ShowDialog();
                    }
                }
            }
        }
        private void TxtReceteKodu_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (!string.IsNullOrEmpty(TxtReceteKodu.Text))
            {
                if (!MesajSor("Reçete Kodunu Değiştirmek istiyormusunuz"))
                {
                    return;
                }
            }
            EvrakNoAl();
        }
        private void TxtEntegreStokKodu_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            FrmMikroStokListesi f = new FrmMikroStokListesi { SecimIcinAcildi = true };
            f.ShowDialog();
            if (f.Secildi)
            {
                var itm = (MikroStok)f.SecilenRow;
                TxtEntegreStokKodu.Text = itm.StokKodu;
                TxtEntegreStokAdi.Text = itm.StokAdi;
                TxtEntegreBirim.Text = itm.Birim;
                StokDanCinsiBagla(TxtEntegreStokKodu.Text);
            }
        }

        private void BtnStokMaliyet_Click(object sender, EventArgs e)
        {
            FrmReceteMaliyetGenel f = new FrmReceteMaliyetGenel();
            f.Model = _mdl.Clone();
            f.ShowDialog();
        }
        private void BtnIstasyonEkle_Click(object sender, EventArgs e)
        {
            FrmIstasyonKartlari f = new FrmIstasyonKartlari();
            f.SecimIcinAcildi = true;
            f.ShowDialog();
            if (f.Secildi)
            {
                var donen = ((IstasyonKarti)f.SecilenRow).Clone();
                var eklenen = new ReceteyeBagliIstasyon()
                {
                    Id = MyGuid.NewGuid(),
                    IstasyonKodu = donen.IstasyonKodu,
                    IstasyonAdi = donen.IstasyonAdi,
                    RcIId = donen.Id,
                };
                _mdl.ReceteyeBagliIstasyonlar.Add(eklenen);
                Grid2Bagla();
                Grid3Bagla();
            }
        }

        private void BtnIstasyonSil_Click(object sender, EventArgs e)
        {
            if (!MesajSor("Kaydı silmek istiyormusunuz.."))
            {
                return;
            }
            var data = myView3.MyGetCurrentItem<ReceteyeBagliIstasyon>();
            if (data == null)
            {
                return;
            }
            _mdl.ReceteyeBagliIstasyonlar.Remove(data);
            Grid3Bagla();
        }

        private void BtnRecetAdiAktar_Click(object sender, EventArgs e)
        {
            TxtReceteKodu.Text = TxtEntegreStokKodu.Text;
            TxtReceteAdi.Text = TxtEntegreStokAdi.Text;
        }

        private void BtnAciklamalar_Click(object sender, EventArgs e)
        {
            if (IdGuid.IsNullOrEmpty())
            {
                MesajBilgi("Reçete Kayıt Edilmeden Açıklama Eklenemiyor..");
                return;
            }
            FrmReceteAciklamalar f = new FrmReceteAciklamalar();
            f.Recete = _mdl.Recete;
            f.ShowDialog();
        }

        private void BtnIstasyonGruplar_Click(object sender, EventArgs e)
        {
            if (IdGuid.IsNullOrEmpty())
            {
                MesajBilgi("Reçete Kayıt Edilmeden Istasyon Grup Eklenemiyor..");
                return;
            }
            FrmReceteIstasyonGrupIstasyonEslestir f = new FrmReceteIstasyonGrupIstasyonEslestir();
            f.RcAId = (Guid)_mdl.Recete.Id; 
            f.ShowDialog();
             
        }

        private void BtnIstasyonGrupAyarlar_Click(object sender, EventArgs e)
        {
            FrmReceteIstasyonGrupOperasyonEslestir f = new FrmReceteIstasyonGrupOperasyonEslestir();
            f.ShowDialog();
        }
    }
}