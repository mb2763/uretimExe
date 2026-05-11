using My.Business.Service.ReceteIstasyonGruplar;
using My.Core;
using My.Entities.ReceteIstasyonGruplar;
using My.Kontrol.Formlar;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyUI.ReceteIstasyonGrupModul
{
    public partial class FrmReceteIstasyonGrupKodlari : MyFrmKayit
    {

        private readonly IReceteIstasyonGrupKodService _srv = Ortak.DbPro.ReceteIstasyonGrupKodlar;

        private List<ReceteIstasyonGrupKod> _list;
        private ReceteIstasyonGrupKod _mdl;

        public FrmReceteIstasyonGrupKodlari()
        {
            InitializeComponent();
            EventlerBagla();
        }
        public void EventlerBagla()
        {
            this.Load += Frm_Load;
            BtnKaydet.Click += BtnKaydet_Click;
            BtnSil.Click += BtnSil_Click;
            BtnDuzenle.Click += BtnDegistir_Click;
            BtnYeni.Click += BtnYeni_Click;
            myView1.MyEventDoubleClickEnter += MyView1_MyEventDoubleClickEnter;

        }


        private void Frm_Load(object sender, EventArgs e)
        {


            Bagla(); 
            SutunGizle();
            myGrid1.GridYerlesimYukle();
            AcilisBittimi = true;
        }
        public void SutunGizle()
        {
            myView1.SutunGizle("Id");
        }
        public void Bagla()
        {
            YeniKayit = true;
            var rs = _srv.SelectListWhere(" Order By Kodu ");
            if (!rs.Success)
            {
                MesajHata(rs.Message);
                return;
            }
            _list = rs.Data.ToList();
            bs.DataSource = _list;
            GridBagla();
            TemizleText();
        }


        private void TemizleText()
        {
            YeniKayit = false;
            _mdl = new ReceteIstasyonGrupKod() { Id = Guid.Empty };
            IdGuid = Guid.Empty;
            TxtKodu.Text = "";
            TxtAdi.Text = "";
            TxtAciklama.Text = "";
        }
        private void AktarTextlere()
        {
            YeniKayit = false;
            IdGuid = _mdl.Id;
            TxtKodu.Text = _mdl.Kodu;
            TxtAdi.Text = _mdl.Adi;
            TxtAciklama.Text = _mdl.Aciklama;
        }
        public void GridBagla()
        {
            myGrid1.DataSource = null;
            bs.DataSource = _list;
            myGrid1.DataSource = bs;
        }
        private void AktarModele()
        {
            if (_mdl == null)
            {
                _mdl = new ReceteIstasyonGrupKod() { Id = MyGuid.NewGuid() };
            }
            if (IdGuid == Guid.Empty) IdGuid = MyGuid.NewGuid();
            _mdl.Id = IdGuid;
            _mdl.Kodu = TxtKodu.Text;
            _mdl.Adi = TxtAdi.Text;
            _mdl.Aciklama = TxtAciklama.Text;
        }
        public void Kaydet()
        {
            if (!TextLeriKontrolEt())
            {
                return;
            }
            AktarModele();

            var kodvarmi= _srv.KodVarmi<ReceteIstasyonGrupKod>(_mdl, "Kodu",YeniKayit);
            if (!kodvarmi.Success)
            {
                MesajHata(kodvarmi.Message);
                return;
            }
            var rs = _srv.InsertOrUpdate(_mdl);
            if (rs.Success)
            {
                KayitEdildi = true;
                Bagla();
            }
            else
            {
                MesajHata(rs.Message);
            }
        }
        public void Sil()
        {
            var rs = _srv.Delete(_mdl);
            if (rs.Success)
            {
                KayitEdildi = true;
                //this.Close();
            }
            else
            {
                MesajHata(rs.Message);
            }
        }
        private bool TextLeriKontrolEt()
        {
            if (string.IsNullOrEmpty(TxtKodu.Text))
            {
                MesajHata("Lütfen  kodunu giriniz");
                return false;
            }
            return true;
        }
        private void MyView1_MyEventDoubleClickEnter()
        {
            if (SecimIcinAcildi)
            {
                var itm = myView1.MyGetCurrentItem<ReceteIstasyonGrupKod>();
                if (itm != null)
                {
                    SecilenKod = itm.Kodu;
                    SecilenRow = itm;
                    SecilenId = itm.Id.ToString();
                    Secildi = true;
                    this.Close();
                }
            }
            else
            {
                BtnDuzenle.PerformClick();
            }
        }
        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            Kaydet();
            Bagla();
        }
        private void BtnSil_Click(object sender, EventArgs e)
        {
            if (!MesajSor("Kaydı silmek istiyormusunuz.."))
            {
                return;
            }
            if (_mdl.Id.IsNullOrEmpty())
            {
                MesajBilgi("Silinecek Kayıt Seçilmemiş Kayda Çift Tıklayın");
            }
            Sil();
            Bagla();
        }
        private void BtnDegistir_Click(object sender, EventArgs e)
        {

            var itm = myView1.MyGetCurrentItem<ReceteIstasyonGrupKod>();
            if (itm == null) return;
            _mdl = itm.Clone();
            AcilisBittimi = false;
            AktarTextlere();
            AcilisBittimi = true;
        }
        private void BtnYeni_Click(object sender, EventArgs e)
        {
            TemizleText();
            YeniKayit = true;
        }


        private void BtnBagla_Click(object sender, EventArgs e)
        {
            Bagla();
            SutunGizle();
            myGrid1.GridYerlesimYukle();
            AcilisBittimi = true;
        }
    }
}
