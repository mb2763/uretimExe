using My.Business.Service.Geneller;
using My.Business.Service.IstasyonKartlar;
using My.Business.Service.Personeller;
using My.Core.Data;
using My.Core.Result;
using My.Entities.Personeller;
using My.Kontrol.Formlar;
using System;
using System.Linq;

namespace MyUI.PersonelModule
{
    public partial class FrmPersonelKartlari : MyFrmKayit
    {
        private IGenelService _srvGenel = Ortak.DbPro.GenelServis;
        private IPersonelService _srv = Ortak.DbPro.Personel;
        private readonly IIstasyonKartiService _srvIst = Ortak.DbPro.IstasyonKarti;
        private Personel _mdl;

        public FrmPersonelKartlari()
        {
            InitializeComponent();
            EventlerBagla();
        }
        private void EventlerBagla()
        {
            this.Load += Frm_Load;
            BtnKaydet.Click += BtnKaydet_Click;
            BtnSil.Click += BtnSil_Click;
            BtnDuzenle.Click += BtnDegistir_Click;
            BtnYeni.Click += BtnYeni_Click;
            myView1.MyEventDoubleClickEnter += myView1_MyEventDoubleClickEnter;
        }
        private void Frm_Load(object sender, EventArgs e)
        {
            BaglaIstasyon();
            BaglaGrup();
            BaglaGorevi();
            Bagla();
        }
        private void Bagla()
        {
            var rs = _srv.SelectListWhere();
            if (!rs.Success)
            {
                MesajHata(rs.Message);
                return;
            }
            var data = rs.Data;
            myGrid1.DataSource = null;
            myGrid1.DataSource = data;
            SutunGizle();
            myGrid1.GridYerlesimYukle();
        }
        private void BaglaGrup()
        {
            var rs = _srvGenel.GrupListesi("Personel", "Grup");
            if (!rs.Success)
            {
                MesajHata(rs.Message);
                return;
            }
            var dt = rs.Data.ToList();
            dt.Insert(0, "");
            CmbGrup.MyDataBagla(dt);
        }
        private void EvrakNoAl()
        {
            var rs = _srvGenel.GetEvrakNo("Personel");
            if (!rs.Success)
            {
                MesajHata(rs.Message);
                return;
            }
            TxtKodu.Text = rs.Data;
        }
        private void BaglaGorevi()
        {
            var rs = _srvGenel.GrupListesi("Personel", "Gorevi");
            if (!rs.Success)
            {
                MesajHata(rs.Message);
                return;
            }
            var dt = rs.Data.ToList();
            dt.Insert(0, "");
            CmbGorevi.MyDataBagla(dt);
        }
        private void SutunGizle()
        {
            myView1.SutunGizle("Id");
            myView1.SutunGizle("Sifre");
        }
        private void Temizle()
        {
            _mdl = new Personel();
            TxtKodu.Text = "";
            TxtAdi.Text = "";
            TxtSoyadi.Text = "";
            CmbGrup.Text = "";
            CmbGorevi.Text = "";
            TxtSifre.Text = "";
            ChcYetkili.Checked = false;
            ChcAdmin.Checked = false;
            ChcIstasyonPersoneli.Checked = false;
            CmbIstasyonKodu.Text = "";
            TxtCeptel.EditValue = "";
            ChcSmsGonder.Checked = false;
            YeniKayit = true;
            GrpKayit.Enabled = false;
            TxtKodu.Enabled = true;
            TxtAdi.Enabled = true;
        }
        private void Kaydet()
        {

            if (string.IsNullOrEmpty(TxtSifre.Text.ToString().Trim()))
            {

                var rs = _srv.PersonelKaydetSifresiz(_mdl, YeniKayit);
                if (!rs.Success)
                {
                    MesajHata(rs.Message);
                    return;
                }
            }
            else
            {
                _mdl.Sifre = _srv.Sifrele64(TxtSifre.Text.ToString().Trim());
                var rs = _srv.PersonelKaydet(_mdl, YeniKayit);
                if (!rs.Success)
                {
                    MesajHata(rs.Message);
                    return;
                }
            }
            Bagla();
            Temizle();
        }

        private void AktarTextlere(Personel data)
        {

            _mdl = data.Clone();
            TxtKodu.Text = _mdl.Kodu;
            TxtAdi.Text = _mdl.Adi;
            TxtSoyadi.Text = _mdl.Soyadi;
            CmbGrup.Text = _mdl.Grup;
            CmbGorevi.Text = _mdl.Gorevi;
            ChcYetkili.Checked = _mdl.Yetkili;
            ChcAdmin.Checked = _mdl.Admin;
            ChcIstasyonPersoneli.Checked = _mdl.IstasyonPersoneli;
            CmbIstasyonKodu.Text = _mdl.IstasyonKodu;
            TxtCeptel.Text = _mdl.CepTel;
            ChcSmsGonder.Checked = _mdl.SmsGonder;
            YeniKayit = false;
            GrpKayit.Enabled = true;
            if (TxtKodu.Text.Trim()=="Admin"|| TxtKodu.Text.Trim() == "admin")
            {
                TxtKodu.Enabled = false;
                TxtAdi.Enabled = false;
            }
            else
            {
                TxtKodu.Enabled = true;
                TxtAdi.Enabled = true;
            }
        }

        private void myView1_MyEventDoubleClickEnter()
        {
            var data = myView1.MyGetCurrentItem<Personel>();
            if (data == null)
            {
                return;
            }
            if (SecimIcinAcildi)
            {
                Secildi = true;
                SecilenId = data.Id.ToString();
                SecilenKod = data.Kodu;
                SecilenRow = data;
                this.Close();
                return;
            }
            else
            {
                AktarTextlere(data);
            }
        }
        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(TxtKodu.Text))
                {
                    EvrakNoAl();
                }
                _mdl.Kodu = TxtKodu.Text;
                _mdl.Adi = TxtAdi.Text;
                _mdl.Soyadi = TxtSoyadi.Text;
                _mdl.Grup = CmbGrup.Text;
                _mdl.Gorevi = CmbGorevi.Text;
                _mdl.Yetkili = ChcYetkili.Checked;
                _mdl.Admin = ChcAdmin.Checked;
                _mdl.IstasyonPersoneli = ChcIstasyonPersoneli.Checked;
                _mdl.IstasyonKodu = CmbIstasyonKodu.Text;
                _mdl.CepTel = TxtCeptel.EditValue?.ToString().Replace("-", "").Replace(" ", "");
                _mdl.SmsGonder = ChcSmsGonder.Checked;


                bool yeni = _mdl.Id == null || _mdl.Id == Guid.Empty ? true : false;
                var kont = KodVarmi(_mdl, "Kodu", yeni);
                if (!kont.Success)
                {
                    MesajBilgi(kont.Message);
                    return;
                }
                if (string.IsNullOrEmpty(TxtSifre.Text) && yeni)
                {
                    MesajBilgi("Lütfen şifre Giriniz");
                    return;
                }
                Kaydet();

            }
            catch (Exception ex)
            {
                MesajHata(ex.Message);
            }

        }
        private IResult KodVarmi(Personel entity, string kontrolalan, bool yenikayitmi)
        {
            var tabloadi = ClassExtensions.GetClassTableName(typeof(Personel));
            var GetId = ClassExtensions.GetClassColumnNameKey(typeof(Personel));
            var sql2 = $" where  {GetId} <> @{GetId} and {kontrolalan} = @{kontrolalan} ";
            if (yenikayitmi == true) sql2 = $" where  {kontrolalan} = @{kontrolalan} ";
            var sql = string.Format("Select count(*) From {0}  {1};", tabloadi, sql2);
            var rs = _srvGenel.Query<int>(sql, entity);
            if (!rs.Success)
            {
                return new ErrorResult(rs.Message);
            }
            if (rs.Data.FirstOrDefault() > 0) new ErrorResult("Aynı " + kontrolalan + " Kodla Kayıt Var");
            return new SuccessResult();
        }
        private void BtnSil_Click(object sender, EventArgs e)
        {
            if (!MesajSor("Kaydı silmek istiyormusunuz"))
            {
                return;
            }
            var data = myView1.MyGetCurrentItem<Personel>();
            if (data == null)
            {
                return;
            }
            var rs = _srv.Delete(data);
            if (!rs.Success)
            {
                MesajHata(rs.Message);
                return;
            }
            Bagla();
            Temizle();
        }
        private void BtnDegistir_Click(object sender, EventArgs e)
        {
            var data = myView1.MyGetCurrentItem<Personel>();
            if (data == null)
            {
                return;
            }
            AktarTextlere(data);

        }
        private void BtnYeni_Click(object sender, EventArgs e)
        {
            Temizle();
            GrpKayit.Enabled = true;
        }
        public void BaglaIstasyon()
        {
            var rs = _srvIst.SelectListWhere(" Order By IstasyonKodu ");
            if (!rs.Success)
            {
                MesajHata(rs.Message);
                return;
            }
            var dt = rs.Data.ToList();
            CmbIstasyonKodu.MyDataBagla(dt, "IstasyonKodu", "IstasyonKodu", new int[] { 1, 2 });

        }

    }
}