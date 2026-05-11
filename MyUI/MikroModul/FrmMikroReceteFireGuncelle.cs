using DevExpress.XtraEditors;
using My.Business.Manager;
using My.Business.Service.Geneller;
using My.Business.Service.MikroModul;
using My.Core;
using My.Entities.Models;
using My.Entities.Receteler;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace MyUI.MikroModul
{
    public partial class FrmMikroReceteFireGuncelle : My.Kontrol.Formlar.MyFrmKayit
    {
        private readonly IGenelService _srvGenel = Ortak.DbPro.GenelServis;
        private readonly IMikroStokService _mikroStokService = Ortak.DbMikro.Stoklar;
        private ReceteManager _mng;
        private MikroReceteManager _mngMikro;
        private ReceteKayitModel _mdl;
        private List<ReceteDetay> receteDetaylar;
        private int receteSira = 0;
        public bool MikrodanAktar = false;
        public string MikroReceteKodu = "";
        // public List<MikroStokCinsi> stokCinsiList = new List<MikroStokCinsi>();
        public List<MikroStokCinsi> stokCinsiListFull = new List<MikroStokCinsi>();
        public FrmMikroReceteFireGuncelle()
        {
            InitializeComponent();
            EventlerBagla();
        }
        private void EventlerBagla()
        {
            this.Load += Frm_Load;
            myView1.FocusedRowChanged += MyView1_FocusedRowChanged;
            myView1.MyEventDoubleClickEnter += MyView1_MyEventDoubleClickEnter;
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
                    MesajHata(" Mikro recete kodu boş olamaz. ");
                    return;
                }
                BaglaMikroRecete();
                BaglaDetaylar();
            }

        }
        private void BaglaDetaylar()
        {
            var rs = _mng.GetReceteDetayByReceteKodu(MikroReceteKodu);
            if (!rs.Success)
            {
                MesajHata(rs.Message);
                return;
            }
            receteDetaylar = rs.Data.ToList();

            foreach (var itm1 in receteDetaylar)
            {
                foreach (var itm2 in _mdl.ReceteDetaylar)
                {
                    if (itm1.ReceteSira == itm2.ReceteSira)
                    {
                        itm1.FireYuzde = itm2.FireYuzde;
                    }
                }
            }

            Grid2Bagla();
        }

        public void BaglaStokCinsi()
        {
            /*  stokCinsiList MikroStokCinsiManager.GetCinsList(); */
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

        }
        private void TemizleText()
        {
            TxtReceteKodu.Text = "";
            TxtReceteAdi.Text = "";
            CmbReceteGrubu.Text = "";
            TxtAciklama.Text = "";
            TxtEntegreStokKodu.Text = "";
            TxtEntegreStokAdi.Text = "";
            TxtEntegreBirim.Text = "";
            TxtModelKodu.Text = "";
            CmbStokCinsi.Text = "Mamül";
            GridBagla();

        }
        private void AktarTextlere()
        {
            IdGuid = _mdl.Recete.Id;
            TxtReceteKodu.Text = _mdl.Recete.ReceteKodu;
            TxtReceteAdi.Text = _mdl.Recete.ReceteAdi;
            CmbReceteGrubu.Text = _mdl.Recete.Grubu;
            TxtAciklama.Text = _mdl.Recete.Aciklama;
            TxtEntegreStokKodu.Text = _mdl.Recete.EntegreStokKodu;
            TxtEntegreStokAdi.Text = _mdl.Recete.EntegreStokAdi;
            TxtEntegreBirim.Text = _mdl.Recete.EntegreBirim;
            TxtModelKodu.Text = _mdl.Recete.ModelKodu;
            if (string.IsNullOrEmpty(_mdl.Recete.EntegreStokKodu))
            {
                CmbStokCinsi.Text = _mdl.Recete.StokCinsiAdi;
            }
            else
            {
                StokDanCinsiBagla(TxtEntegreStokKodu.Text);
            }
            GridBagla();
        }
        private void GridBagla()
        {
            myGrid1.DataSource = null;
            bs.DataSource = null;
            bs.DataSource = _mdl.ReceteDetaylar;
            myGrid1.DataSource = bs;
            SutunGizle();
            myGrid1.GridYerlesimYukle(myGrid1.MyGridKayitAdi);
            //myView1.Columns["ReceteSira"].OptionsColumn.AllowEdit = true;
            //myView1.Columns["ReceteSira"].AppearanceHeader.BackColor = Color.Green;
            //myView1.Columns["Miktar"].OptionsColumn.AllowEdit = true;
            //myView1.Columns["Miktar"].AppearanceHeader.BackColor = Color.Green;
            //myView1.Columns["Ebat"].OptionsColumn.AllowEdit = true;
            //myView1.Columns["Ebat"].AppearanceHeader.BackColor = Color.Green;
            //myView1.Columns["Gram"].OptionsColumn.AllowEdit = true;
            //myView1.Columns["Gram"].AppearanceHeader.BackColor = Color.Green;
            //myView1.Columns["Olcu"].OptionsColumn.AllowEdit = true;
            //myView1.Columns["Olcu"].AppearanceHeader.BackColor = Color.Green;
        }
        private void Grid2Bagla()
        {
            myGrid2.DataSource = null;
            bsd.DataSource = null;
            bsd.DataSource = receteDetaylar;
            myGrid2.DataSource = bsd;
            Sutun2Gizle();
            myGrid2.GridYerlesimYukle();
            myView2.Columns["ReceteSira"].OptionsColumn.AllowEdit = true;
            myView2.Columns["ReceteSira"].AppearanceHeader.BackColor = Color.Green;
            myView2.Columns["Miktar"].OptionsColumn.AllowEdit = true;
            myView2.Columns["Miktar"].AppearanceHeader.BackColor = Color.Green;
            myView2.Columns["Ebat"].OptionsColumn.AllowEdit = true;
            myView2.Columns["Ebat"].AppearanceHeader.BackColor = Color.Green;
            myView2.Columns["Gram"].OptionsColumn.AllowEdit = true;
            myView2.Columns["Gram"].AppearanceHeader.BackColor = Color.Green;
            myView2.Columns["Olcu"].OptionsColumn.AllowEdit = true;
            myView2.Columns["Olcu"].AppearanceHeader.BackColor = Color.Green;
            myView2.Columns["FireYuzde"].OptionsColumn.AllowEdit = true;
            myView2.Columns["FireYuzde"].AppearanceHeader.BackColor = Color.Green;
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
        private void Sutun2Gizle()
        {
            myView1.SutunGizle("Id");
            myView1.SutunGizle("RcAId");

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
            _mdl.Recete.EntegreStokKodu = TxtEntegreStokKodu.Text;
            _mdl.Recete.EntegreStokAdi = TxtEntegreStokAdi.Text;
            _mdl.Recete.EntegreBirim = TxtEntegreBirim.Text;
            _mdl.Recete.ModelKodu = TxtModelKodu.Text;

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

            // aynı entegre stokkodu daha önce girilmişmi kontrol et 

            var rs = _mng.ReceteDetayFireYuzdeGuncelle(receteDetaylar);
            if (rs.Success)
            {
                KayitEdildi = true;
                MesajBilgi("Kayıt Edildi");
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

        }
        private void MyView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {

        }

        private void BtnKaydet_Click(object sender, EventArgs e)
        {

            Kaydet();

        }

        private void TxtReceteKodu_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            //if (!string.IsNullOrEmpty(TxtReceteKodu.Text)) {
            //    if (!MesajSor("Reçete Kodunu Değiştirmek istiyormusunuz")) {
            //        return;
            //    }
            //}
            //EvrakNoAl();
        }
        private void TxtEntegreStokKodu_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {

            //FrmMikroStokListesi f = new FrmMikroStokListesi { SecimIcinAcildi = true };
            //f.ShowDialog();
            //if (f.Secildi) {
            //    var itm = (MikroStok)f.SecilenRow;
            //    TxtEntegreStokKodu.Text = itm.StokKodu;
            //    TxtEntegreStokAdi.Text = itm.StokAdi;
            //    TxtEntegreBirim.Text = itm.Birim;
            //    StokDanCinsiBagla(TxtEntegreStokKodu.Text);
            //}

        }

        private void myButton1_Click(object sender, EventArgs e)
        {
            TxtReceteKodu.Text = TxtEntegreStokKodu.Text;
            TxtReceteAdi.Text = TxtEntegreStokAdi.Text;
        }


    }
}
