using My.Business.Service.Geneller;
using My.Business.Service.MikroModul;
using My.Entities.Mikro;
using My.Entities.Receteler;
using My.Kontrol.Formlar;
using MyUI.MikroModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace MyUI.ReceteModule
{
    public partial class FrmReceteDetayED : MyFrmSade
    {
        public ReceteDetay Detay;
        private IGenelService _srvGenel = Ortak.DbPro.GenelServis;
        private IMikroGenelService _srvGenelMikro = Ortak.DbMikro.GenelServis;
        private IMikroStokService _mikroStokService = Ortak.DbMikro.Stoklar;
        public int Sira = 1;
        public FrmReceteDetayED()
        {
            InitializeComponent();
            this.Load += Frm_Load;
            TxtVarsayilanStokKodu1.ButtonClick += TxtVarsayilanStokKodu_ButtonClick;
            TxtVarsayilanStokAdi1.ButtonClick += TxtVarsayilanStokAdi_ButtonClick;
        }
        private void Frm_Load(object sender, EventArgs e)
        {
            KategorilerBagla();   // BaglaCinsi();
            BaglaStokAnaGrup();
            AktarTextlere();
        }

        public void BaglaCinsi()
        {
            try
            {
                var rs = _srvGenel.GrupListesi("ReceteDetay", "Cinsi");
                if (!rs.Success)
                {
                    MesajHata(rs.Message);
                    return;
                }
                var dt = rs.Data.ToList();
                if (dt == null) dt = new List<string>();
                dt.Insert(0, "");
                CmbCinsi1.MyDataBagla(dt);
            }
            catch
            {
            }
        }
        public void BaglaStokAnaGrup()
        {
            try
            {
                var rs = _srvGenelMikro.GrupListesi("STOKLAR", Ortak.MikroStokGrubu);
                if (!rs.Success)
                {
                    MesajHata(rs.Message);
                    return;
                }
                var dt = rs.Data.ToList();
                if (dt == null) dt = new List<string>();
                dt.Insert(0, "");
                CmbStokAnaGrup1.MyDataBagla(dt);
            }
            catch
            {
            }
        }
        public void AktarTextlere()
        {
            CmbCinsi1.Text = Detay.Cinsi;
            TxtBirim1.Text = Detay.Birim;
            TxtMiktar1.Text = Detay.Miktar.ToString();
            TxtOperasyonMaliyet.Text = Detay.OperasyonMaliyet.ToString();
            TxtFireYuzde.Text = Detay.FireYuzde.ToString();
            TxtReceteSira1.Text = Detay.ReceteSira.ToString();
            TxtVarsayilanStokAdi1.Text = Detay.VarsayilanStokAdi;
            TxtVarsayilanStokKodu1.Text = Detay.VarsayilanStokKodu;
            TxtAciklama1.Text = Detay.Aciklama;
            ChcStokKullan1.Checked = Detay.StokKullan;
            ChcSiparisdeGosterme1.Checked = Detay.SiparisdeGosterme;
            if (YeniKayit)
            {
                ChcStokKullan1.Checked = true;
                ChcSiparisdeGosterme1.Checked = true;
            }
            if (!string.IsNullOrEmpty(Detay.VarsayilanStokKodu))
            {
                RenkBagla(Detay.VarsayilanStokKodu);
                BedenBagla(Detay.VarsayilanStokKodu);
                CmbRenk.Text = Detay.Renk;
                CmbBeden.Text = Detay.Beden;
            }

            CmbStokAnaGrup1.Text = Detay.StokAnaGrup;
            if (Sira > Detay.ReceteSira)
            {
                TxtReceteSira1.Text = Sira.ToString();
            }
            //"Stok","Grup","Sabit","Tümü" 
            string getturu = string.IsNullOrEmpty(Detay.StokTuru) ? "Sabit" : Detay.StokTuru;
            if (getturu == "Grup")
            {
                RadGrup1.Checked = true;
            }
            else if (getturu == "Sabit")
            {
                RadSabit1.Checked = true;
            }
            else if (getturu == "Tümü")
            {
                RadTumu1.Checked = true;
            }
            else
            {
                RadStok1.Checked = true;
            }
        }
        public void AktarModele()
        {
            Detay.Cinsi = CmbCinsi1.Text;
            Detay.Birim = TxtBirim1.Text;
            Detay.Renk = CmbRenk.Text;
            Detay.Beden = CmbBeden.Text;
            Detay.Miktar = TxtMiktar1.MyDegerDouble();
            Detay.OperasyonMaliyet = TxtOperasyonMaliyet.MyDegerDouble();
            Detay.FireYuzde = TxtFireYuzde.MyDegerDouble();
            Detay.ReceteSira = (int)TxtReceteSira1.MyDegerInteger();
            Detay.VarsayilanStokAdi = TxtVarsayilanStokAdi1.Text;
            Detay.VarsayilanStokKodu = TxtVarsayilanStokKodu1.Text;
            Detay.Aciklama = TxtAciklama1.Text;
            Detay.StokKullan = ChcStokKullan1.Checked;
            Detay.SiparisdeGosterme = ChcSiparisdeGosterme1.Checked;
            string getturu = "Stok";
            if (RadGrup1.Checked == true)
            {
                getturu = "Grup";
            }
            else if (RadSabit1.Checked == true)
            {
                getturu = "Sabit";
            }
            else if (RadTumu1.Checked == true)
            {
                getturu = "Tümü";
            }
            Detay.StokTuru = getturu;
            Detay.StokAnaGrup = CmbStokAnaGrup1.Text;
        }
        public void RenkBagla(string stokkodu)
        {

            try
            {
                var rs = _mikroStokService.GetRenkByStokKodu(stokkodu);
                if (!rs.Success)
                {
                    MesajHata(rs.Message);
                    return;
                }
                var rnk = rs.Data;
                if (rnk != null)
                {
                    CmbRenk.Properties.Items.Clear();
                    CmbRenk.MyDataBagla(rnk.GetRenkler());
                }
            }
            catch
            {
            }
        }
        public void BedenBagla(string stokkodu)
        {

            try
            {
                var rs = _mikroStokService.GetBedenByStokKodu(stokkodu);
                if (!rs.Success)
                {
                    MesajHata(rs.Message);
                    return;
                }
                var bdn = rs.Data;
                if (bdn != null)
                {
                    CmbBeden.Properties.Items.Clear();
                    CmbBeden.MyDataBagla(bdn.GetBedenler());
                }
            }
            catch
            {
            }
        }
        public void KategorilerBagla()
        {

            try
            {
                var rs = _mikroStokService.GetStokKategoriler();
                if (!rs.Success)
                {
                    MesajHata(rs.Message);
                    return;
                }
                var bdn = rs.Data;
                bdn.Insert(0, "");
                if (bdn != null)
                {
                    CmbCinsi1.Properties.Items.Clear();
                    CmbCinsi1.MyDataBagla(bdn);
                }
            }
            catch
            {
            }
        }

        private void BtnTamam_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(CmbCinsi1.Text))
            {
                MesajBilgi("Lütfen Cinsini giriniz..");
                return;
            }
            if (TxtMiktar1.MyDegerDouble() <= 0.0d)
            {
                MesajBilgi("Lütfen Miktarını giriniz..");
                return;
            }
            if (RadGrup1.Checked == true)
            {
                if (string.IsNullOrEmpty(CmbStokAnaGrup1.Text))
                {
                    MesajBilgi("Lütfen Stok Ana Grubunu  giriniz..");
                    return;
                }
            }
            else if (RadSabit1.Checked == true)
            {
                if (string.IsNullOrEmpty(TxtVarsayilanStokKodu1.Text))
                {
                    MesajBilgi("Lütfen Varsayilan Stok Kodunu  giriniz..");
                    return;
                }
            }
            AktarModele();
            KayitEdildi = true;
            this.Close();
        }
        private void TxtVarsayilanStokKodu_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            FrmMikroStokListesi f = new FrmMikroStokListesi();
            f.SecimIcinAcildi = true;
            f.WindowState = FormWindowState.Maximized;
            f.ShowDialog();
            if (f.Secildi)
            {
                var st = ((MikroStok)f.SecilenRow).Clone();
                TxtVarsayilanStokAdi1.Text = st.StokAdi;
                TxtVarsayilanStokKodu1.Text = st.StokKodu;
                TxtBirim1.Text = st.Birim;
                RenkBagla(st.StokKodu);
                BedenBagla(st.StokKodu);
            }
        }
        private void TxtVarsayilanStokAdi_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            FrmMikroStokListesi f = new FrmMikroStokListesi();
            f.SecimIcinAcildi = true;
            f.WindowState = FormWindowState.Maximized;
            f.ShowDialog();
            if (f.Secildi)
            {
                var st = ((MikroStok)f.SecilenRow).Clone();
                TxtVarsayilanStokAdi1.Text = st.StokAdi;
                TxtVarsayilanStokKodu1.Text = st.StokKodu;
                TxtBirim1.Text = st.Birim;
                RenkBagla(st.StokKodu);
                BedenBagla(st.StokKodu);
            }
        }

    }
}