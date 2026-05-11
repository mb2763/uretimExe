using DevExpress.XtraEditors;
using My.Business.Manager;
using My.Business.Service.Geneller;
using My.Business.Service.IstasyonKartlar;
using My.Business.Service.MikroModul;
using My.Core;
using My.Entities.IstasyonKartlar;
using My.Entities.Mikro;
using My.Entities.Receteler;
using My.Entities.UretimTalepler;
using My.Kontrol.Formlar;
using MyUI.MikroModule;
using MyUI.ReceteModule;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MyUI.UretimTalepler
{
    public partial class FrmUretimTalepED : MyFrmKayit
    {
        private readonly IMikroStokService _srvCari = Ortak.DbMikro.Stoklar;
        private readonly IGenelService _srvGenel = Ortak.DbPro.GenelServis;
        private readonly IIstasyonKartiService _srvIstasyon = Ortak.DbPro.IstasyonKarti;
        private UretimTalepManager _mng;
        public Action ActionAktar;
        private UretimTalep _mdl;
        //private List<MikroStok> StoklarAll;
        private List<UretimTalepHareket> Hareketler;
        public FrmUretimTalepED()
        {
            InitializeComponent();
            Eventler();
        }
        public void Eventler()
        {
            Load += Frm_Load;
            KeyDown += Frm_KeyDown;

            myView1.ShownEditor += MyView1_ShownEditor;
        }
        private void Frm_Load(object sender, EventArgs e)
        {
            _mng = new UretimTalepManager(Ortak.DbPro);
            Text = "Üretim Talep Kayıt";
            Bagla();
            BaglaIstasyonlar();
            if (!Ortak.LisansAktif)
            {
                BtnKaydet.Enabled = false;
                BtnSil.Enabled = false;
            }
            AcilisBittimi = true;
        }
        public void Bagla()
        {
            if (_mng == null)
            {
                _mng = new UretimTalepManager(Ortak.DbPro);
            }

            if (IdGuid.IsNullOrEmpty())
            {
                YeniKayit = true;
                _mdl = _mng.GetTalepNew();
                Hareketler = new List<UretimTalepHareket>();
                TemizleText();
            }
            else
            {
                YeniKayit = false;
                var rs = _mng.GetTalep(IdGuid);
                if (!rs.Success)
                {
                    MesajHata(rs.Message);
                    return;
                }

                _mdl = rs.Data;
                var rs2 = _mng.GetTalepHareketler(IdGuid);
                if (!rs.Success)
                {
                    MesajHata(rs2.Message);
                    return;
                }

                Hareketler = rs2.Data;
                AktarTextlere();

            }
        }
        public void BaglaIstasyonlar()
        {

            var rs = _srvIstasyon.SelectListWhere("");
            if (!rs.Success)
            {
                MesajHata(rs.Message);
                return;
            }

            var lis = rs.Data.ToList();
            bsCari.DataSource = lis;
            //TxtIstasyonKodu.Properties.DataSource = bsCari;
            //TxtIstasyonKodu.Properties.DisplayMember = "IstasyonKodu";
            //TxtIstasyonKodu.Properties.ValueMember = "IstasyonKodu";
            //TxtIstasyonAdi.Properties.DataSource = bsCari;
            //TxtIstasyonAdi.Properties.DisplayMember = "IstasyonAdi";
            //TxtIstasyonAdi.Properties.ValueMember = "IstasyonAdi";

            colCmbIstasyonKodu.DataSource = bsCari;
            colCmbIstasyonKodu.DisplayMember = "IstasyonKodu";
            colCmbIstasyonKodu.ValueMember = "IstasyonKodu";

            colCmbIstasyonAdi.DataSource = bsCari;
            colCmbIstasyonAdi.DisplayMember = "IstasyonAdi";
            colCmbIstasyonAdi.ValueMember = "IstasyonAdi";


        }
        public void TemizleText()
        {

            TxtAciklama.Text = "";
            TxtTarih.Text = DateTime.Now.ToString();

            GridBagla();
            GridBaglaAcilis();
        }
        public void AktarTextlere()
        {

            TxtAciklama.Text = _mdl.Aciklama;
            TxtTarih.Text = _mdl.Tarih.ToString();
            TxtSiparisKodu.Text = _mdl.EvrakNo;
            GridBagla();
            GridBaglaAcilis();
        }
        public void GridBagla()
        {
            myGrid1.DataSource = null;
            myGrid1.DataSource = Hareketler;
        }

        public void GridBaglaAcilis()
        {
            SutunGizle();
            myGrid1.GridYerlesimYukle();
            myView1.Columns["Miktar"].OptionsColumn.AllowEdit = true;
            myView1.Columns["Miktar"].AppearanceHeader.BackColor = Color.Green;
            myView1.Columns["Aciklama"].OptionsColumn.AllowEdit = true;
            myView1.Columns["Aciklama"].AppearanceHeader.BackColor = Color.Green;
            myView1.Columns["Parti"].OptionsColumn.AllowEdit = true;
            myView1.Columns["Parti"].AppearanceHeader.BackColor = Color.Green;
            myView1.Columns["Lot"].OptionsColumn.AllowEdit = true;
            myView1.Columns["Lot"].AppearanceHeader.BackColor = Color.Green;

        }

        public void SutunGizle()
        {
            //myView1.SutunGizle("UrtTlpHrId");
            //myView1.SutunGizle("UrtTlpId");  
        }

        private void Frm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                BtnStokSec.PerformClick();
            }

            else if (e.KeyCode == Keys.F8)
            {
                BtnStokSil.PerformClick();
            }
        }
        private void MyView1_ShownEditor(object sender, EventArgs e)
        {
            try
            {
                if (myView1.ActiveEditor is TextEdit)
                {
                    var edit = myView1.ActiveEditor as TextEdit;
                    if (edit == null)
                    {
                        return;
                    }
                    if (edit.Text.Length > 0)
                    {
                        edit.SelectAll();
                    }
                }
            }
            catch
            {
            }
        }



        public void StokSecReceteden()
        {
            var f = new FrmReceteListesi
            {
                SecimIcinAcildi = true,
                WindowState = FormWindowState.Maximized,

            };

            f.ShowDialog();
            if (f.Secildi)
            {
                var st = ((ReceteAna)f.SecilenRow).Clone();
                var h = new UretimTalepHareket();
                h.StokKodu = st.ReceteKodu;
                h.StokAdi = st.ReceteAdi;
                h.Miktar = 0;
                h.Birimi = st.EntegreBirim;
                h.Aciklama = "";
                h.Parti = "";
                h.Lot = 0;
                Hareketler.Add(h);
                myGrid1.DataSource = null;
                myGrid1.DataSource = Hareketler;
                myView1.MoveLast();
            }
        }

        public void StokTekSec__()
        {
            var f = new FrmMikroStokListesi
            {
                SecimIcinAcildi = true,
                WindowState = FormWindowState.Maximized,

            };
            f.TumStoklar = true;
            f.ShowDialog();
            if (f.Secildi)
            {
                var st = ((MikroStok)f.SecilenRow).Clone();

                var h = new UretimTalepHareket();

                h.StokKodu = st.StokKodu;
                h.StokAdi = st.StokAdi;
                h.Miktar = 0;
                h.Birimi = st.Birim;
                h.Aciklama = "";
                h.Parti = "";
                h.Lot = 0;
                Hareketler.Add(h);
                myGrid1.DataSource = null;
                myGrid1.DataSource = Hareketler;
                myView1.MoveLast();
            }
        }
        public void StokSil()
        {
            if (!MesajSor("Kaydı silmek istiyormusunuz.."))
            {
                return;
            }
            var data = myView1.MyGetCurrentItem<UretimTalepHareket>();
            if (data == null)
            {
                return;
            }
            Hareketler.Remove(data);
            GridBagla();

        }
        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            Kaydet();
        }
        private void BtnSil_Click(object sender, EventArgs e)
        {
            Sil();
        }
        public void EvrakNoAl()
        {
            var rs = _srvGenel.GetEvrakNo("UretimTalep");
            if (!rs.Success)
            {
                MesajHata(rs.Message);
                return;
            }
            TxtSiparisKodu.Text = rs.Data;
        }
        public void Kaydet()
        {
            if (!TextLeriKontrolEt())
            {
                return;
            }
            AktarModele();
            var rs = _mng.Kaydet(_mdl, Hareketler);
            if (rs.Success)
            {
                KayitEdildi = true;
                ActionAktar?.Invoke();
                Close();
            }
            else
            {
                MesajHata(rs.Message);
            }
        }
        private bool TextLeriKontrolEt()
        {
            if (TxtSiparisKodu.Text.IsNullOrEmpty())
            {
                EvrakNoAl();
            }
            if (TxtTarih.Text.IsNullOrEmpty())
            {
                TxtTarih.EditValue = DateTime.Now;
            }

            foreach (var itm in Hareketler)
            {
                if (string.IsNullOrEmpty(itm.IstasyonKodu))
                {
                    MesajHata("Lütfen İstasyon kodunu giriniz");
                    return false;
                }

                if (itm.Miktar <= 0)
                {
                    MesajHata("Miktar Girilmemiş alanlar var Lütfen miktarları giriniz.");
                    return false;
                }

            }

            return true;
        }
        public void AktarModele()
        {
            if (_mdl.UrtTlpId == null || _mdl.UrtTlpId == Guid.Empty)
            {
                _mdl.UrtTlpId = MyGuid.NewGuid();
            }

            _mdl.Aciklama = TxtAciklama.Text;
            _mdl.EvrakNo = TxtSiparisKodu.Text;
            if (!TxtTarih.Text.IsNullOrEmpty())
            {
                _mdl.Tarih = Convert.ToDateTime(TxtTarih.Text);
            }
            else
            {
                _mdl.Tarih = DateTime.Now;
            }
            if (_mdl.Kullanici.IsNullOrEmpty())
            {
                _mdl.Kullanici = Ortak.KullaniciAdi;
            }
            if (string.IsNullOrEmpty(_mdl.KayitEden))
            {
                _mdl.KayitEden = Ortak.KullaniciAdi;
            }
            if (_mdl.KayitTarihi == null)
            {
                _mdl.KayitTarihi = DateTime.Now;
            }

            foreach (var itm in Hareketler)
            {
                if (itm.UrtTlpHrId == null || itm.UrtTlpHrId == Guid.Empty)
                {
                    itm.UrtTlpHrId = MyGuid.NewGuid();
                }
                itm.UrtTlpId = _mdl.UrtTlpId;
                itm.IstasyonKodu = _mdl.IstasyonKodu;
                itm.IstasyonAdi = _mdl.IstasyonAdi;
                itm.Tarih = _mdl.Tarih;
                itm.EvrakNo = _mdl.EvrakNo;
                itm.Kullanici = _mdl.Kullanici;
                if (string.IsNullOrEmpty(itm.KayitEden))
                {
                    itm.KayitEden = Ortak.KullaniciAdi;
                }
                if (itm.KayitTarihi == null)
                {
                    itm.KayitTarihi = DateTime.Now;
                }
            }


        }
        public void Sil()
        {
            if (!MesajSor("Kaydı silmek istiyormusunuz.."))
            {
                return;
            }

            var rs = _mng.Sil(_mdl);
            if (rs.Success)
            {
                ActionAktar?.Invoke();
                KayitEdildi = true;
                Close();
            }
            else
            {
                MesajHata(rs.Message);
            }
        }
        private void myGrid1_Click(object sender, EventArgs e)
        {

        }
        private void BtnStokSec_Click(object sender, EventArgs e)
        {
            StokSecReceteden();
        }
        private void BtnStokSil_Click(object sender, EventArgs e)
        {
            StokSil();
        }

        private void TxtSiparisKodu_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
        {
            if (!string.IsNullOrEmpty(TxtSiparisKodu.Text))
            {
                if (!MesajSor("Evrak No  Değiştirmek istiyormusunuz"))
                {
                    return;
                }
            }

            EvrakNoAl();
        }

        private void colCmbIstasyonKodu_Leave1111111111(object sender, EventArgs e)
        {
            //if (AcilisBittimi) {
            //    int rh = myView1.FocusedRowHandle;
            //    if (sender is LookUpEdit) {
            //        AcilisBittimi = false;
            //        var lkp = (LookUpEdit)sender;

            //        var mdl = (IstasyonKarti)lkp.GetSelectedDataRow();
            //        if (mdl != null) {

            //            myView1.SetRowCellValue(rh, "IstasyonAdi", mdl.IstasyonAdi);

            //        }
            //        AcilisBittimi = true;
            //    } 
            //}
        }


        private void colCmbIstasyonKodu_EditValueChanged(object sender, EventArgs e)
        {
            if (AcilisBittimi)
            {
                int rh = myView1.FocusedRowHandle;
                if (sender is LookUpEdit)
                {
                    AcilisBittimi = false;
                    var lkp = (LookUpEdit)sender;
                    var mdl = (IstasyonKarti)lkp.GetSelectedDataRow();
                    if (mdl != null)
                    {
                        myView1.SetRowCellValue(rh, "IstasyonKodu", mdl.IstasyonKodu);
                        myView1.SetRowCellValue(rh, "IstasyonAdi", mdl.IstasyonAdi);
                    }
                    AcilisBittimi = true;
                }
            }
        }

        private void colCmbIstasyonAdi_EditValueChanged(object sender, EventArgs e)
        {
            if (AcilisBittimi)
            {
                int rh = myView1.FocusedRowHandle;
                if (sender is LookUpEdit)
                {
                    AcilisBittimi = false;
                    var lkp = (LookUpEdit)sender;
                    var mdl = (IstasyonKarti)lkp.GetSelectedDataRow();
                    if (mdl != null)
                    {
                        myView1.SetRowCellValue(rh, "IstasyonKodu", mdl.IstasyonKodu);
                        myView1.SetRowCellValue(rh, "IstasyonAdi", mdl.IstasyonAdi);
                    }
                    AcilisBittimi = true;
                }
            }
        }
    }
}
