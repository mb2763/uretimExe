using My.Business.Manager;
using My.Business.Service.Geneller;
using My.Business.Service.UretimOperasyonlar;
using My.Entities.UretimOperasyonlar;
using My.Kontrol.Formlar;
using MyUI.UretimIstasyonModule;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MyUI.UretimOperasyonModule
{
    public partial class FrmUretimOperasyonList : MyFrmListe
    {

        private readonly IUretimOperasyonService _srv = Ortak.DbPro.UretimOperasyon;
        private readonly IUretimOperasyonHareketService _srvHrk = Ortak.DbPro.UretimOperasyonHareket;
        private readonly IUretimOperasyonHareketDetayService _srvHrkDetay = Ortak.DbPro.UretimOperasyonHareketDetay;
        private readonly IGenelService _srvGenel = Ortak.DbPro.GenelServis;
        private UretimEmriManager _mngUretim;


        private List<UretimOperasyon> _list;
        private List<UretimOperasyonHareket> _listHrk;
        private List<UretimOperasyonHareketDetay> _listHrkDty;

        string durumu = "";
        public bool SiparisKodundanFiltrele { get; set; } = false;
        public string SiparisKodu { get; set; } = "";
        public FrmUretimOperasyonList()
        {
            InitializeComponent();
            this.Load += Frm_Load;
            myView1.MyEventDoubleClickEnter += MyView1_MyEventDoubleClickEnter;
            myView3.MyEventDoubleClickEnter += MyView3_MyEventDoubleClickEnter;
            myView1.FocusedRowChanged += MyView1_FocusedRowChanged;
        }
        private void Frm_Load(object sender, EventArgs e)
        {
            _mngUretim = new UretimEmriManager(Ortak.DbPro);
            TxtTarihi1.Text = DateTime.Now.AddMonths(-1).ToShortDateString();
            TxtTarihi2.Text = "";
            TxtTarihi3.Text = "";
            TxtTarihi4.Text = "";
            BtnAra.Click += BtnAra_Click;
            BtnTemizle.Click += BtnTemizle_Click;
            BaglaDurum();
            BaglaReceteAdi();
            BaglaOperasyon();
            if (SiparisKodundanFiltrele)
            {
                SiparisFiltreAyarla();
            }

            Bagla();
            SutunGizle();
            ContexMenuyeEkle();
            myGrid1.GridYerlesimYukle();
            DurumuAyarla(durumu);
            BtnAra.PerformClick();
        }
        void SiparisFiltreAyarla()
        {
            durumu = "";
            TxtKodu.Text = SiparisKodu;
            TxtTarihi1.EditValue = Convert.ToDateTime(DateTime.Now.Year + "-01-01");

        }
        private void SutunGizle()
        {
            myView1.SutunGizle("Id");
            myView1.SutunGizle("UrId");
            myView1.SutunGizle("RcAId");
            myView1.SutunGizle("RcOId");
            myView1.SutunGizle("SipId");
            myView1.SutunGizle("SipHId");
            myView1.SutunFormat(nameof(UretimOperasyon.BaslangicTarihi), DevExpress.Utils.FormatType.DateTime, "dd.MM.yyyy HH:mm");
            myView1.SutunFormat(nameof(UretimOperasyon.BitisTarihi), DevExpress.Utils.FormatType.DateTime, "dd.MM.yyyy HH:mm");
            myView1.SutunFormat(nameof(UretimOperasyon.KayitTarihi), DevExpress.Utils.FormatType.DateTime, "dd.MM.yyyy HH:mm");
            myView1.SutunFormat(nameof(UretimOperasyon.DegistirmeTarihi), DevExpress.Utils.FormatType.DateTime, "dd.MM.yyyy HH:mm");
        }
        private void SutunGizle2()
        {
            myView2.SutunGizle("Id");
            myView2.SutunGizle("UrId");
            myView2.SutunGizle("UrOId");
            myView2.SutunGizle("RcAId");
            myView2.SutunGizle("RcOId");
            myView2.SutunGizle("SipId");
            myView2.SutunFormat(nameof(UretimOperasyonHareket.BaslangicTarihi), DevExpress.Utils.FormatType.DateTime, "dd.MM.yyyy HH:mm");
            myView2.SutunFormat(nameof(UretimOperasyonHareket.BitisTarihi), DevExpress.Utils.FormatType.DateTime, "dd.MM.yyyy HH:mm");
            myView2.SutunFormat(nameof(UretimOperasyonHareket.KayitTarihi), DevExpress.Utils.FormatType.DateTime, "dd.MM.yyyy HH:mm");
            myView2.SutunFormat(nameof(UretimOperasyonHareket.DegistirmeTarihi), DevExpress.Utils.FormatType.DateTime, "dd.MM.yyyy HH:mm");

        }
        private void SutunGizle3()
        {
            myView3.SutunGizle("Id");
            myView3.SutunGizle("UrId");
            myView3.SutunGizle("UrOId");
            myView3.SutunGizle("UrOHId");
            myView3.SutunGizle("RcAId");
            myView3.SutunGizle("RcOId");
            myView3.SutunGizle("SipId");
            myView3.SutunFormat(nameof(UretimOperasyonHareketDetay.Tarih), DevExpress.Utils.FormatType.DateTime, "dd.MM.yyyy HH:mm");
            myView3.SutunFormat(nameof(UretimOperasyonHareketDetay.KayitTarihi), DevExpress.Utils.FormatType.DateTime, "dd.MM.yyyy HH:mm");
            myView3.SutunFormat(nameof(UretimOperasyonHareketDetay.DegistirmeTarihi), DevExpress.Utils.FormatType.DateTime, "dd.MM.yyyy HH:mm");

        }

        private void ContexMenuyeEkle()
        {
            //ToolStripMenuItem fm2 = new ToolStripMenuItem("Sonraki_Operasyon") {
            //    BackColor = Color.WhiteSmoke,
            //    ForeColor = Color.Black,
            //    Text = "Sonraki Operasyon",
            //    TextAlign = ContentAlignment.BottomRight,
            //    ToolTipText = "Sonraki Operasyon"
            //};
            //// Fm2.Click += Sonraki_Operasyon;

            ToolStripMenuItem m1 = new ToolStripMenuItem("DurumGuncelle")
            {
                BackColor = Color.WhiteSmoke,
                ForeColor = Color.Black,
                Text = "Durum Güncelle",
                ToolTipText = "Durum Güncelle",
                TextAlign = ContentAlignment.BottomRight,
            };
            m1.Click += DurumGuncelle;
            myGrid1.MyContextMenuAdd(m1);
        }
        private void Bagla()
        {
            Cursor.Current = Cursors.WaitCursor;
            string sor = SorguAyarla() + SorguAyarlaTrh();
            if (!string.IsNullOrEmpty(sor))
            {
                sor = "where 1=1 " + sor;
            }
            var rs = _srv.GetViewListWhere(sor);
            if (!rs.Success)
            {
                Cursor.Current = Cursors.Default;
                MesajHata(rs.Message);
                return;
            }
            _list = rs.Data.ToList();
            myGrid1.DataSource = _list;
            myGrid2.DataSource = null;
            myGrid3.DataSource = null;
            if (rs.Data.Any())
            {
                BaglaHareket(rs.Data.FirstOrDefault().Id);
            }

            Cursor.Current = Cursors.Default;
        }
        private void BaglaHareket(Guid? UrOId)
        {
            Cursor.Current = Cursors.WaitCursor;
            string sor = "where UrOId= '" + UrOId + "' ";
            var rs = _srvHrk.GetViewListWhere(sor);
            if (!rs.Success)
            {
                Cursor.Current = Cursors.Default;
                MesajHata(rs.Message);
                return;
            }
            _listHrk = rs.Data.ToList();
            myGrid2.DataSource = _listHrk;
            SutunGizle2();
            myGrid2.GridYerlesimYukle();
            if (rs.Data.Any())
            {
                BaglaHareketDetay(rs.Data.FirstOrDefault().Id);
            }
            Cursor.Current = Cursors.Default;
        }
        private void BaglaHareketDetay(Guid? UrOHId)
        {

            Cursor.Current = Cursors.WaitCursor;
            string sor = "where UrOHId= '" + UrOHId + "' ";
            var rs = _srvHrkDetay.GetViewListWhere(sor);
            if (!rs.Success)
            {
                Cursor.Current = Cursors.Default;
                MesajHata(rs.Message);
                return;
            }
            _listHrkDty = rs.Data.ToList();
            myGrid3.DataSource = _listHrkDty;
            SutunGizle3();
            myGrid3.GridYerlesimYukle();
            Cursor.Current = Cursors.Default;
        }
        private void BaglaDurum()
        {
            var rs = _srvGenel.GrupListesi("UretimOperasyon", "Durumu");
            if (!rs.Success)
            {
                MesajHata(rs.Message);
                return;
            }
            var dt = rs.Data.ToList();
            dt.Insert(0, "");
            CmbDurumu.DataSource = dt;
        }
        private void BaglaOperasyon()
        {
            var rs = _srvGenel.GrupListesi("UretimOperasyon", "OperasyonKodu");
            if (!rs.Success)
            {
                MesajHata(rs.Message);
                return;
            }
            var dt = rs.Data.ToList();
            dt.Insert(0, "");
            CmbOperasyon.DataSource = dt;
        }
        private void BaglaReceteAdi()
        {
            var rs = _srvGenel.GrupListesi("UretimOperasyon", "ReceteAdi");
            if (!rs.Success)
            {
                MesajHata(rs.Message);
                return;
            }
            var dt = rs.Data.ToList();
            dt.Insert(0, "");
            CmbReceteAdi.DataSource = dt;
        }
        private void DurumGuncelle(object sender, EventArgs e)
        {
            var itm = myView1.MyGetCurrentItem<UretimOperasyon>();
            if (itm != null)
            {
                var rs = _mngUretim.UretimDurumGuncelle(itm.UrId);
                if (!rs.Success)
                {
                    MesajHata(rs.Message);
                    return;
                }
                MesajBilgi("Güncellendi.");
            }


        }
        private string SorguAyarla()
        {
            string sor = "";
            string t1 = CmbOperasyon.Text.Trim();
            if (!string.IsNullOrEmpty(t1)) { sor += $" AND  UrO.OperasyonKodu = '{t1}' "; }
            t1 = durumu.Trim();
            if (!string.IsNullOrEmpty(t1)) { sor += $" AND  UrO.Durumu = '{t1}' "; }
            t1 = CmbReceteAdi.Text.Trim();
            if (!string.IsNullOrEmpty(t1)) { sor += $" AND  UrO.ReceteAdi like('%{t1}%')  "; }

            t1 = TxtKodu.Text.Trim();
            if (!string.IsNullOrEmpty(t1)) { sor += $" AND  Sip.SiparisKodu    like('%{t1}%')"; }
            return sor;
        }
        private string SorguAyarlaTrh()
        {
            string sor = "";
            string t1 = TxtTarihi1.Text.Trim();
            if (!string.IsNullOrEmpty(t1))
            {
                t1 = TarihAyarla(Convert.ToDateTime(TxtTarihi1.Text));
                if (!string.IsNullOrEmpty(t1)) sor += $"  AND  CAST (coalesce( UrO.BaslangicTarihi,'1901-01-01') AS DATE ) >=  CAST('{t1}'  AS DATE ) ";
            }
            t1 = TxtTarihi2.Text.ToString().Trim();
            if (!string.IsNullOrEmpty(t1))
            {
                t1 = TarihAyarla(Convert.ToDateTime(TxtTarihi2.Text));
                if (!string.IsNullOrEmpty(t1)) sor += $"  AND  CAST ( coalesce(UrO.BaslangicTarihi,'1901-01-01') AS DATE ) <=  CAST('{t1}'  AS DATE ) ";
            }
            t1 = TxtTarihi3.Text.ToString().Trim();
            if (!string.IsNullOrEmpty(t1))
            {
                t1 = TarihAyarla(Convert.ToDateTime(TxtTarihi3.Text));
                if (!string.IsNullOrEmpty(t1)) sor += $"  AND  CAST (coalesce( UrO.BitisTarihi,'1901-01-01') AS DATE ) >=  CAST('{t1}'  AS DATE ) ";
            }
            t1 = TxtTarihi4.Text.ToString().Trim();
            if (!string.IsNullOrEmpty(t1))
            {
                t1 = TarihAyarla(Convert.ToDateTime(TxtTarihi4.Text));
                if (!string.IsNullOrEmpty(t1)) sor += $"  AND  CAST (coalesce( UrO.BitisTarihi,'1901-01-01') AS DATE ) <=  CAST('{t1}'  AS DATE ) ";
            }
            return sor;
        }
        private string TarihAyarla(DateTime? T1)
        {
            if (T1 == null)
            {
                return "";
            }
            string FX = T1?.Year + "-" + T1?.Month.ToString().PadLeft(2, '0') + "-" +
                        T1?.Day.ToString().PadLeft(2, '0');
            return FX;
        }
        private void MyView1_MyEventDoubleClickEnter()
        {
            if (SecimIcinAcildi)
            {
                var itm = myView1.MyGetCurrentItem<UretimOperasyon>();
                if (itm != null)
                {
                    SecilenRow = itm;
                    Secildi = true;
                    this.Close();
                }
            }
            else
            {
                var itm = myView1.MyGetCurrentItem<UretimOperasyon>();
                if (itm != null)
                {
                    //FrmUretimEmriED_V2 f = new FrmUretimEmriED_V2();
                    //f.AltBarStatusPanel.Visible = false;
                    //f.Idsi = itm.Id;
                    //f.ActionAktar = Bagla;
                    //f.ShowDialog();
                }
            }
        }
        private void MyView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {


            var itm = myView1.MyGetCurrentItem<UretimOperasyon>();
            if (itm != null)
            {
                BaglaHareket(itm.Id);
            }
            else
            {
                myGrid2.DataSource = null;
                myGrid3.DataSource = null;
            }
        }
        private void MyView3_MyEventDoubleClickEnter()
        {
            var itm = myView3.MyGetCurrentItem<UretimOperasyonHareketDetay>();
            if (itm != null)
            {
                FrmUretimIstasyonED f = new FrmUretimIstasyonED
                {
                    OperasyonTuru = OperasyonTuruEnum.Degistir,
                    IdGuid = itm.Id,
                    OprId = itm.UrOHId
                };
                f.ShowDialog();
            }
        }
        private void BtnAra_Click(object sender, EventArgs e)
        {
            Bagla();
        }
        private void BtnTemizle_Click(object sender, EventArgs e)
        {
            CmbDurumu.Text = "";
            CmbReceteAdi.Text = "";
            TxtTarihi1.Text = "";
            TxtTarihi2.Text = "";
            TxtTarihi3.Text = "";
            TxtTarihi4.Text = "";
        }

        private void BtnDurumuHepsi_Click(object sender, EventArgs e)
        {
            DurumuAyarla("Hepsi");
            BtnAra.PerformClick();
        }
        private void BtnDurumuBeklemede_Click(object sender, EventArgs e)
        {
            DurumuAyarla("Beklemede");
            BtnAra.PerformClick();
        }
        private void BtnDurumuUretimde_Click(object sender, EventArgs e)
        {
            DurumuAyarla("Uretimde");
            BtnAra.PerformClick();
        }
        private void BtnDurumuHazir_Click(object sender, EventArgs e)
        {
            DurumuAyarla("Hazir");
            BtnAra.PerformClick();
        }
        private void DurumuAyarla(string durum)
        {
            if (durum == "Hepsi")
            {
                durumu = "";
                BtnDurumuHepsi.FilterButonRenklendir(true);
                BtnDurumuBeklemede.FilterButonRenklendir();
                BtnDurumuUretimde.FilterButonRenklendir();
                BtnDurumuHazir.FilterButonRenklendir();
            }
            else if (durum == "Beklemede")
            {
                durumu = "Beklemede";
                BtnDurumuHepsi.FilterButonRenklendir();
                BtnDurumuBeklemede.FilterButonRenklendir(true);
                BtnDurumuUretimde.FilterButonRenklendir();
                BtnDurumuHazir.FilterButonRenklendir();
            }
            else if (durum == "Uretimde")
            {
                durumu = "Uretimde";
                BtnDurumuHepsi.FilterButonRenklendir();
                BtnDurumuBeklemede.FilterButonRenklendir();
                BtnDurumuUretimde.FilterButonRenklendir(true);
                BtnDurumuHazir.FilterButonRenklendir();
            }
            else if (durum == "Hazir")
            {
                durumu = "Hazir";
                BtnDurumuHepsi.FilterButonRenklendir();
                BtnDurumuBeklemede.FilterButonRenklendir();
                BtnDurumuUretimde.FilterButonRenklendir();
                BtnDurumuHazir.FilterButonRenklendir(true);
            }
            else
            {
                durumu = "";
                BtnDurumuHepsi.FilterButonRenklendir(true);
                BtnDurumuBeklemede.FilterButonRenklendir();
                BtnDurumuUretimde.FilterButonRenklendir();
                BtnDurumuHazir.FilterButonRenklendir();
            }
        }

    }
}