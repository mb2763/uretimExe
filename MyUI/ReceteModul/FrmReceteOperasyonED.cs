using My.Business.Manager;
using My.Business.Service.Geneller;
using My.Business.Service.IstasyonKartlar;
using My.Core;
using My.Entities.IstasyonKartlar;
using My.Entities.Mikro;
using My.Entities.Models;
using My.Entities.OperasyonKartlar;
using My.Entities.ReceteIstasyonlar;
using My.Entities.Receteler;
using My.Kontrol.Formlar;
using MyUI.MikroModule;
using MyUI.UretimIstasyonModule;
using MyUI.UretimOperasyonModule;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MyUI.ReceteModule
{
    public partial class FrmReceteOperasyonED : MyFrmKayit
    {
        private IGenelService _srvGenel = Ortak.DbPro.GenelServis;
        private IIstasyonKartiService _srvIstKart = Ortak.DbPro.IstasyonKarti;
        private ReceteOperasyonManager _mng;
        private ReceteOperasyonKayitModel _mdl;
        private int _operasyonSira = 1;
        public bool Kolonlanacak = false;
        public ReceteAna KolonRecete;
        public FrmReceteOperasyonED()
        {
            InitializeComponent();
            EventlerBagla();
        }
        public void EventlerBagla()
        {
            this.Load += Frm_Load;

           

            myView1.FocusedRowChanged += MyView1_FocusedRowChanged;
            myView2.FocusedRowChanged += MyView2_FocusedRowChanged;
            myView2.MyEventDoubleClickEnter += MyView2_MyEventDoubleClickEnter;
        }
        private void MyView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            Grid2Bagla();
        }
        private void MyView2_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            Grid3Bagla();
        }
        private void MyView2_MyEventDoubleClickEnter()
        {
        
        }
        private void Frm_Load(object sender, EventArgs e)
        {
            _mng = new ReceteOperasyonManager(Ortak.DbPro);
            BaglaGrup();
            Bagla();
        }
        private void TxtReceteKodu_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
        }
        private void Bagla()
        {
            if (IdGuid.IsNullOrEmpty())
            {
                YeniKayit = true;
                var rs = _mng.GetOperasyonKayitEdit(IdGuid);
                if (!rs.Success)
                {
                    MesajHata(rs.Message);
                    return;
                }
                _mdl = rs.Data;
                TemizleText();
            }
            else
            {
                var rs = _mng.GetOperasyonKayitEdit(IdGuid);
                if (!rs.Success)
                {
                    MesajHata(rs.Message);
                    return;
                }
                _mdl = rs.Data;
                if (Kolonlanacak)
                {
                    _mdl.Recete = KolonRecete.Clone();
                    BaglaKolonla();
                }
                else
                {
                    YeniKayit = false;
                }
                AktarTextlere();
            }
        }
        private void TemizleText()
        {
            TxtReceteKodu.Text = "";
            TxtReceteAdi.Text = "";
            CmbReceteGrubu.Text = "";
            TxtAciklama.Text = "";
            GridBagla();
            Grid2Bagla();
            Grid3Bagla();
        }
        private void AktarTextlere()
        {
            IdGuid = _mdl.Recete.Id;
            TxtReceteKodu.Text = _mdl.Recete.ReceteKodu;
            TxtReceteAdi.Text = _mdl.Recete.ReceteAdi;
            CmbReceteGrubu.Text = _mdl.Recete.Grubu;
            TxtAciklama.Text = _mdl.Recete.Aciklama;
            GridBagla();
            Grid2Bagla();
            foreach (var itm in _mdl.Operasyonlar)
            {
                if (itm != null && itm.Sira > _operasyonSira)
                {
                    _operasyonSira = itm.Sira;
                }
            }
        }
        private void GridBagla()
        {
            myGrid1.DataSource = null;
            bs.DataSource = null;
            bs.DataSource = _mdl.Operasyonlar;
            myGrid1.DataSource = bs;

            myView1.Columns["UretimSure"].Caption = "Uretim Sure / Dk";

            myView1.Columns["Sira"].OptionsColumn.AllowEdit = true;
            myView1.Columns["UretimSure"].OptionsColumn.AllowEdit = true;
            myView1.Columns["MaliyetFiyat"].OptionsColumn.AllowEdit = true;
            myView1.Columns["KullanilacakAparat"].OptionsColumn.AllowEdit = true;

            myView1.Columns["Sira"].AppearanceHeader.BackColor = Color.Green;
            myView1.Columns["UretimSure"].AppearanceHeader.BackColor = Color.Green;
            myView1.Columns["MaliyetFiyat"].AppearanceHeader.BackColor = Color.Green; 
            myView1.Columns["KullanilacakAparat"].AppearanceHeader.BackColor = Color.Green; 
            myView1.Columns["OlcumDegeriMin"].OptionsColumn.AllowEdit = true;
            myView1.Columns["OlcumDegeriMax"].OptionsColumn.AllowEdit = true;
            myView1.Columns["OlcumDegeriMin"].AppearanceHeader.BackColor = Color.Green;
            myView1.Columns["OlcumDegeriMax"].AppearanceHeader.BackColor = Color.Green;
            myView1.Columns["OlcumDegeriMin2"].OptionsColumn.AllowEdit = true;
            myView1.Columns["OlcumDegeriMax2"].OptionsColumn.AllowEdit = true;
            myView1.Columns["OlcumDegeriMin2"].AppearanceHeader.BackColor = Color.Green;
            myView1.Columns["OlcumDegeriMax2"].AppearanceHeader.BackColor = Color.Green;
            myView1.Columns["OlcumDegeriMin3"].OptionsColumn.AllowEdit = true;
            myView1.Columns["OlcumDegeriMax3"].OptionsColumn.AllowEdit = true;
            myView1.Columns["OlcumDegeriMin3"].AppearanceHeader.BackColor = Color.Green;
            myView1.Columns["OlcumDegeriMax3"].AppearanceHeader.BackColor = Color.Green;
            myView1.SutunEditAc(nameof(ReceteOperasyon.OlcumDegeriMin4)); 
            myView1.SutunEditAc(nameof(ReceteOperasyon.OlcumDegeriMax4));
            myView1.SutunEditAc(nameof(ReceteOperasyon.OlcumDegeriMin5)); 
            myView1.SutunEditAc(nameof(ReceteOperasyon.OlcumDegeriMax5)); 
            myView1.SutunCaptionColor(nameof(ReceteOperasyon.OlcumDegeriMin4), Color.Green);
            myView1.SutunCaptionColor(nameof(ReceteOperasyon.OlcumDegeriMax4),Color.Green);
            myView1.SutunCaptionColor(nameof(ReceteOperasyon.OlcumDegeriMin5), Color.Green);
            myView1.SutunCaptionColor(nameof(ReceteOperasyon.OlcumDegeriMax5),Color.Green);
            SutunGizle();
            myGrid1.GridYerlesimYukle(myGrid1.MyGridKayitAdi);
            if (_mdl.Operasyonlar.Count > 0)
            {
                Grid2Bagla(_mdl.Operasyonlar.FirstOrDefault());
            }
        }
        private void SutunGizle()
        {
            myView1.SutunGizle("Id");
            myView1.SutunGizle("RcAId");

        }
        private void Grid2Bagla(ReceteOperasyon opr = null)
        {
            ReceteOperasyon data = null;
            if (opr == null)
            {
                data = myView1.MyGetCurrentItem<ReceteOperasyon>();
            }
            else
            {
                data = opr;
            }
            if (data == null)
            {
                return;
            }
            myGrid2.DataSource = null;
            myView2.RefreshData();
            bsIs.DataSource = null;
            var lis = _mdl.Istasyonlar.Where(c => c.RcOId == data.Id).ToList();
            if (lis.Count <= 0)
            {
                return;
            }
            bsIs.DataSource = lis;
            myGrid2.DataSource = bsIs;
            try
            {
                myView2.Columns["Fason"].OptionsColumn.AllowEdit = true;
                myView2.Columns["MaliyetFiyat"].OptionsColumn.AllowEdit = true;
                myView2.Columns["Fason"].AppearanceHeader.BackColor = Color.Green;
                myView2.Columns["MaliyetFiyat"].AppearanceHeader.BackColor = Color.Green;
                SutunGizle2();
                myGrid2.GridYerlesimYukle(myGrid2.MyGridKayitAdi);
            }
            catch
            {
            }
        }
        private void SutunGizle2()
        {
            myView2.SutunGizle("Id");
            myView2.SutunGizle("RcAId");
            myView2.SutunGizle("RcOId");

        }
        private void Grid3Bagla(ReceteIstasyon opr = null)
        {
            ReceteIstasyon data = null;
            if (opr == null)
            {
                data = myView2.MyGetCurrentItem<ReceteIstasyon>();
            }
            else
            {
                data = opr;
            }
            if (data == null)
            {
                return;
            }
            myGrid3.DataSource = null;
            bsc.DataSource = null;
            var lis = _mdl.Cariler.Where(c => c.RcIstId == data.Id).ToList();
            if (lis.Count <= 0)
            {
                return;
            }
            bsc.DataSource = lis;
            myGrid3.DataSource = bsc;
            SutunGizle3();
            myGrid3.GridYerlesimYukle();
        }
        private void SutunGizle3()
        {
            myView3.SutunGizle("Id");
            myView3.SutunGizle("RcAId");
            myView3.SutunGizle("RcOId");
            myView3.SutunGizle("RcIstId");

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
        private void BaglaKolonla()
        {
            IdGuid = Guid.Empty;
            Guid? oldOprguid;
            Guid? oldIstguid;
            foreach (var opr in _mdl.Operasyonlar)
            {
                oldOprguid = opr.Id;
                opr.Id = MyGuid.NewGuid();
                opr.RcAId = _mdl.Recete.Id;
                foreach (var ist in _mdl.Istasyonlar)
                {
                    oldIstguid = ist.Id;

                    if (ist.RcOId == oldOprguid)
                    {
                        ist.RcOId = opr.Id;
                    }
                    ist.Id = MyGuid.NewGuid();
                    ist.RcAId = opr.RcAId;
                    foreach (var cr in _mdl.Cariler)
                    {
                        cr.Id = MyGuid.NewGuid();
                        if (cr.RcOId == oldOprguid) cr.RcOId = opr.Id;
                        if (cr.RcIstId == oldIstguid) cr.RcIstId = ist.Id;
                        cr.RcAId = opr.RcAId;
                    }
                }
            }
            YeniKayit = true;
            AktarTextlere();
            GridBagla();
            Grid2Bagla();
            Grid3Bagla();
        }
        private void AktarModele()
        {
            if (IdGuid == Guid.Empty) IdGuid = MyGuid.NewGuid();
            _mdl.Recete.Id = IdGuid;
            _mdl.Recete.ReceteKodu = TxtReceteKodu.Text.ToString();
            _mdl.Recete.ReceteAdi = TxtReceteAdi.Text.ToString();
            _mdl.Recete.Grubu = CmbReceteGrubu.Text.ToString();
            _mdl.Recete.Aciklama = TxtAciklama.Text.ToString();
            if (_mdl.Recete.Id.IsNullOrEmpty())
            {
                _mdl.Recete.Id = MyGuid.NewGuid();
            }
            foreach (var dty in _mdl.Operasyonlar)
            {
                dty.RcAId = _mdl.Recete.Id;
            }
            if (_mdl.Operasyonlar.Count == 1)
            {
                _mdl.Operasyonlar[0].Sira = 1;
            }
            foreach (var sto in _mdl.Istasyonlar)
            {
                sto.RcAId = _mdl.Recete.Id;
                //sto.RcOId = Guid.Empty;
            }
            foreach (var sto in _mdl.Cariler)
            {
                sto.RcAId = _mdl.Recete.Id;

                //sto.RcOId = Guid.Empty;
                //sto.RcIstId = Guid.Empty;
            }
        }
        private void Kaydet()
        {
            if (!TextLeriKontrolEt())
            {
                return;
            }
            AktarModele();
            if (!OperasyonSiraKontrol())
            {
                return;
            }
            var rs = _mng.OperasyonKaydet(_mdl, YeniKayit);
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
        public bool OperasyonSiraKontrol()
        {
            bool birvarmi = false;
            foreach (var itm in _mdl.Operasyonlar)
            {
                if (itm.Sira == 1)
                {
                    birvarmi = true;
                    break;
                }
            }
            if (!birvarmi)
            {
                MesajHata(" 1 nolu sira girilmemiş lütfen sira noları 1 den başlayarak ardışık giriniz.");
            }
            foreach (var itm in _mdl.Operasyonlar)
            {
                if (itm.Sira <= 0)
                {
                    MesajHata(itm.OperasyonKodu + " " + itm.OperasyonAdi + " operasyonun sirasini giriniz 1'sayısından aşağı operasyon sirasi hataya sebep olabilir");
                    return false;
                }
                foreach (var itm2 in _mdl.Operasyonlar)
                {
                    if (itm.Sira == itm2.Sira && itm.Id != itm2.Id)
                    {
                        MesajHata(itm.Sira + " Kodlu Sira çoklu kayıt edilemez.");
                        return false;
                    }
                }
            }
            return true;
        }
        private void Sil()
        {
            if (!MesajSor("Kaydı silmek istiyormusunuz.."))
            {
                return;
            }
            var rs = _mng.OperasyonSil(_mdl);
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
            if (string.IsNullOrEmpty(TxtReceteKodu.Text))
            {
                MesajHata("Lütfen Reçete kodunu giriniz");
                return false;
            }
            if (_mdl.Operasyonlar.Count <= 0)
            {
                MesajHata("Operasyon Girilmemiş Lütfen Operasyon giriniz");
                return false;
            }
            //if (_mdl.Istasyonlar.Count <= 0)
            //{
            //    MesajHata("Istasyon Girilmemiş Lütfen Istasyon giriniz");
            //    return false;
            //}
            foreach (var itm in _mdl.Operasyonlar)
            {
                if (itm.Sira <= 0)
                {
                    MesajHata("lütfen Operasyonlarda sira 0 olanlari düzenleyiniz");
                    return false;
                }
                //var istadet = _mdl.Istasyonlar.Where(c => c.RcOId == itm.Id).ToList().Count;
                //if (istadet <= 0)
                //{
                //    MesajHata(itm.OperasyonKodu + " kodlu Operasyona İstasyon ekleyiniz");
                //    return false;
                //}
            }
            return true;
        }
        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            Kaydet();
        }
        private void BtnSil_Click(object sender, EventArgs e)
        {
            var cik = false;
            foreach (var itm in _mdl.Operasyonlar)
            {
                var rs = _mng.OperasyonSilKontrol(itm.Id);
                if (!rs.Success)
                {
                    MesajHata(rs.Message);
                    cik = true;
                    break;
                }
            }
            if (cik == true)
            {
                return;
            }
            Sil();
        }
        private void BtnOperasyonEkle_Click(object sender, EventArgs e)
        {
            FrmOperasyonKartlari f = new FrmOperasyonKartlari();
            f.SecimIcinAcildi = true;
            f.ShowDialog();
            if (f.Secildi)
            {
                OperasyonSiraDegistir();
                var donen = ((OperasyonKarti)f.SecilenRow).Clone();
                var opr = new ReceteOperasyon() { Id = MyGuid.NewGuid(), Sira = _operasyonSira++, OperasyonAdi = donen.OperasyonAdi, OperasyonKodu = donen.OperasyonKodu, RcAId = _mdl.Recete.Id };
                _mdl.Operasyonlar.Add(opr);
                if (!string.IsNullOrEmpty(donen.VarsayilanIstasyonKodu))
                {
                    // varsayilan istasyon ekle
                    var rsist = _srvIstKart.SelectFirst(c => c.IstasyonKodu == donen.VarsayilanIstasyonKodu);//((IstasyonKarti)f.SecilenRow).Clone();
                    if (!rsist.Success)
                    {
                        MesajHata(rsist.Message);
                        return;
                    }
                    var ist = rsist.Data;
                    var eklenen = new ReceteIstasyon()
                    {
                        Id = MyGuid.NewGuid(),
                        OperasyonKodu = opr.OperasyonKodu,
                        OperasyonAdi = opr.OperasyonAdi,
                        IstasyonKodu = ist.IstasyonKodu,
                        IstasyonAdi = ist.IstasyonAdi,
                        RcAId = opr.RcAId,
                        RcOId = opr.Id,

                    };
                    _mdl.Istasyonlar.Add(eklenen);

                    // bagli istasyonlar ekle
                    var rsistlist = _srvIstKart.SelectList(c => c.Operasyon == donen.OperasyonKodu);//((IstasyonKarti)f.SecilenRow).Clone();
                    if (!rsistlist.Success)
                    {
                        MesajHata(rsistlist.Message);
                        return;
                    }
                    var istlis = rsistlist.Data.ToList();

                    foreach (var istitm in istlis)
                    {

                        if (ist.IstasyonKodu == istitm.IstasyonKodu)
                        {
                            continue;// onceden varsayilan olarak eklenmisse atla
                        }
                        var ekle = new ReceteIstasyon()
                        {
                            Id = MyGuid.NewGuid(),
                            OperasyonKodu = opr.OperasyonKodu,
                            OperasyonAdi = opr.OperasyonAdi,
                            IstasyonKodu = istitm.IstasyonKodu,
                            IstasyonAdi = istitm.IstasyonAdi,
                            RcAId = opr.RcAId,
                            RcOId = opr.Id,

                        };
                        _mdl.Istasyonlar.Add(ekle);
                    }

                    Grid2Bagla();
                    Grid3Bagla();
                }
                GridBagla();
            }
        }
        private void BtnOperasyonSil_Click(object sender, EventArgs e)
        {
            if (!MesajSor("Kaydı silmek istiyormusunuz.."))
            {
                return;
            }

            var data = myView1.MyGetCurrentItem<ReceteOperasyon>();
            if (data == null)
            {
                return;
            }
            var rs = _mng.OperasyonSilKontrol(data.Id);
            if (!rs.Success)
            {
                MesajHata(rs.Message);
                return;
            }
            var rc = _mdl.Operasyonlar.Find(c => c.Id == data.Id);
            _mdl.Operasyonlar.Remove(rc);
            OperasyonSiraDegistir();
            GridBagla();
            List<ReceteIstasyon> istLis = new List<ReceteIstasyon>();
            var ist = _mdl.Istasyonlar.Where(c => c.RcOId != data.Id);
            foreach (var itm in ist)
            {
                istLis.Add(itm.Clone());
            }
            _mdl.Istasyonlar.Clear();
            _mdl.Istasyonlar.InsertRange(0, ist);
            List<ReceteIstasyonCari> crlLis = new List<ReceteIstasyonCari>();
            foreach (var itm in _mdl.Istasyonlar)
            {
                var crl = _mdl.Cariler.Where(c => c.RcIstId != itm.Id);
                foreach (var itCr in crl)
                {
                    crlLis.Add(itCr.Clone());
                }
            }
            _mdl.Cariler.Clear();
            _mdl.Cariler.InsertRange(0, crlLis);
            Grid2Bagla();
        }
        private void BtnIstasyonEkle_Click(object sender, EventArgs e)
        {
            var data = myView1.MyGetCurrentItem<ReceteOperasyon>();
            if (data == null)
            {
                return;
            }
            FrmIstasyonKartlari f = new FrmIstasyonKartlari();
            f.SecimIcinAcildi = true;
            f.Aranan = data.OperasyonKodu;
            f.ShowDialog();
            if (f.Secildi)
            {
                var donen = ((IstasyonKarti)f.SecilenRow).Clone();
                var eklenen = new ReceteIstasyon()
                {
                    Id = MyGuid.NewGuid(),
                    OperasyonKodu = data.OperasyonKodu,
                    OperasyonAdi = data.OperasyonAdi,
                    IstasyonKodu = donen.IstasyonKodu,
                    IstasyonAdi = donen.IstasyonAdi,
                    RcAId = data.RcAId,
                    RcOId = data.Id,

                };
                _mdl.Istasyonlar.Add(eklenen);
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
            var data = myView2.MyGetCurrentItem<ReceteIstasyon>();
            if (data == null)
            {
                return;
            }
            var rs = _mng.IstasyonSilKontrol(data.Id);
            if (!rs.Success)
            {
                MesajHata(rs.Message);
                return;
            }
            _mdl.Istasyonlar.Remove(data);
            List<ReceteIstasyonCari> crlLis = new List<ReceteIstasyonCari>();
            var crl = _mdl.Cariler.Where(c => c.RcIstId != data.Id);
            foreach (var itCr in crl)
            {
                crlLis.Add(itCr.Clone());
            }
            _mdl.Cariler.Clear();
            _mdl.Cariler.InsertRange(0, crlLis);
            Grid2Bagla();
            Grid3Bagla();
        }
        private void BtnCariEkle_Click(object sender, EventArgs e)
        {
            var data = myView2.MyGetCurrentItem<ReceteIstasyon>();
            if (data == null)
            {
                return;
            }
            FrmMikroCariListesi f = new FrmMikroCariListesi();
            f.SecimIcinAcildi = true;
            f.WindowState = FormWindowState.Maximized;
            f.ShowDialog();
            if (f.Secildi)
            {
                var st = ((MikroCari)f.SecilenRow).Clone();
                _mdl.Cariler.Add(new ReceteIstasyonCari()
                {
                    Id = MyGuid.NewGuid(),
                    RcOId = data.RcOId,
                    RcAId = data.RcAId,
                    RcIstId = data.Id,
                    CariKodu = st.CariKodu,
                    CariUnvani = st.CariUnvani1
                });
                Grid3Bagla();
            }
        }
        private void BtnCariSil_Click(object sender, EventArgs e)
        {
            if (!MesajSor("Kaydı silmek istiyormusunuz.."))
            {
                return;
            }
            var data = myView3.MyGetCurrentItem<ReceteIstasyonCari>();
            if (data == null)
            {
                return;
            }
            _mdl.Cariler.Remove(data);
            Grid3Bagla();
        }
        private void OperasyonSiraDegistir()
        {

            // //    var donen = ((OperasyonKarti)f.SecilenRow).Clone();
            // var opr = new ReceteOperasyon() { Id = MyGuid.NewGuid(), Sira = _operasyonSira++, OperasyonAdi = donen.OperasyonAdi, OperasyonKodu = donen.OperasyonKodu, RcAId = _mdl.Recete.Id };
            // //var geclis=_mdl.Operasyonlar
            // List<ReceteOperasyon> oplis = new List<ReceteOperasyon>();
            // var sir = _mdl.Operasyonlar.OrderBy(c => c.Sira);

            _operasyonSira = 1;
            foreach (var itm in _mdl.Operasyonlar.OrderBy(c => c.Sira))
            {
                itm.Sira = _operasyonSira++;
            }


        }

    }
}