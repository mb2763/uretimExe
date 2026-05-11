using My.Business.Manager;
using My.Business.Service.Geneller;
using My.Core;
using My.Entities.Models;
using My.Entities.ReceteGruplar;
using My.Entities.Receteler;
using My.Kontrol.Formlar;
using System;
using System.Linq;
using System.Windows.Forms;

namespace MyUI.ReceteModule
{
    public partial class FrmReceteGrupED : MyFrmKayit
    {

        private IGenelService _srvGenel = Ortak.DbPro.GenelServis;
        private ReceteGrupManager _mng;
        private ReceteGrupKayitmodel _mdl;
        public FrmReceteGrupED()
        {
            InitializeComponent();
            EventlerBagla();
        }
        public void EventlerBagla()
        {
            this.Load += Frm_Load;
        }
        private void Frm_Load(object sender, EventArgs e)
        {
            _mng = new ReceteGrupManager(Ortak.DbPro);
            BaglaGrup();
            Bagla();
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
                AktarTextlere();
            }
        }
        private void TemizleText()
        {
            TxtReceteKodu.Text = "";
            CmbReceteGrubu.Text = "";
            TxtAciklama.Text = "";
            GridBagla();
        }
        private void AktarTextlere()
        {
            IdGuid = _mdl.Grup.Id;
            TxtReceteKodu.Text = _mdl.Grup.ReceteGrupKodu;
            CmbReceteGrubu.Text = _mdl.Grup.Grubu;
            TxtAciklama.Text = _mdl.Grup.Aciklama;
            GridBagla();
        }
        private void GridBagla()
        {
            myGrid1.DataSource = null;
            bs.DataSource = _mdl.Detaylar;
            myGrid1.DataSource = bs;
            SutunGizle();
            myGrid1.GridYerlesimYukle(myGrid1.MyGridKayitAdi);
            myView1.Columns["Miktar"].OptionsColumn.AllowEdit = true;
            myView1.Columns["Aciklama"].OptionsColumn.AllowEdit = true;

        }
        private void SutunGizle()
        {
            myView1.SutunGizle("Id");
            myView1.SutunGizle("RcGId");
            myView1.SutunGizle("RcAId");

        }
        private void BaglaGrup()
        {
            var rs = _srvGenel.GrupListesi("ReceteGrup", "Grubu");
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

            _mdl.Grup.Id = IdGuid;
            _mdl.Grup.ReceteGrupKodu = TxtReceteKodu.Text.ToString();
            _mdl.Grup.Grubu = CmbReceteGrubu.Text.ToString();
            _mdl.Grup.Aciklama = TxtAciklama.Text.ToString();

            if (_mdl.Grup.Id.IsNullOrEmpty())
            {
                _mdl.Grup.Id = MyGuid.NewGuid();
            }
            foreach (var dty in _mdl.Detaylar)
            {
                dty.RcGId = _mdl.Grup.Id;
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
            if (!TextLeriKontrolEt())
            {
                return;
            }
            AktarModele();
            var rs = _mng.ReceteGrupKaydet(_mdl, YeniKayit);
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
        private void Sil()
        {
            var rs = _mng.ReceteGrupSil(_mdl);
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
                EvrakNoAl();
            }
            if (string.IsNullOrEmpty(TxtReceteKodu.Text))
            {
                MesajHata("Lütfen Reçete Grup kodunu giriniz");
                return false;
            }
            return true;
        }
        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            Kaydet();
        }
        private void BtnSil_Click(object sender, EventArgs e)
        {
            if (!MesajSor("Kaydı silmek istiyormusunuz.."))
            {
                return;
            }

            Sil();

        }
        private void BtnDetayEkle_Click(object sender, EventArgs e)
        {

            FrmReceteListesi f = new FrmReceteListesi();
            f.SecimIcinAcildi = true;
            f.WindowState = FormWindowState.Maximized;
            f.ShowDialog();
            if (f.Secildi)
            {
                var st = ((ReceteAna)f.SecilenRow).Clone();
                _mdl.Detaylar.Add(new ReceteGrupDetay()
                {
                    Id = MyGuid.NewGuid(),
                    RcAId = st.Id,
                    Miktar = 1,
                    Aciklama = "",
                    ReceteKodu = st.ReceteKodu,
                    ReceteAdi = st.ReceteAdi,

                });
                GridBagla();
            }
        }
        private void BtnDetaySil_Click(object sender, EventArgs e)
        {
            if (!MesajSor("Kaydı silmek istiyormusunuz.."))
            {
                return;
            }
            var data = myView1.MyGetCurrentItem<ReceteGrupDetay>();
            if (data == null)
            {
                return;
            }
            _mdl.Detaylar.Remove(data);
            GridBagla();
        }

        private void TxtReceteKodu_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (!string.IsNullOrEmpty(TxtReceteKodu.Text))
            {
                if (MesajSor("Recete Grup Kodunu Değiştirmek istiyormusunuz") != true)
                {
                    return;
                }
            }
            EvrakNoAl();
        }
    }
}
