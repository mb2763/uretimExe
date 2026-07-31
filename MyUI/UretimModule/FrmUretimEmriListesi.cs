using My.Business;
using My.Business.Manager;
using My.Business.Service.Geneller;
using My.Business.Service.UretimEmirler;
using My.Business.Service.UretimOperasyonlar;
using My.Entities.UretimEmirler;
using My.Entities.UretimOperasyonlar;
using My.Kontrol.Formlar;
using My.Kontrol.Yazdirma;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace MyUI.UretimModule
{
    public partial class FrmUretimEmriListesi : MyFrmListe
    {
        private DatabaseFactoryMikro _db = Ortak.DbMikro;
        IUretimEmriService _srv = Ortak.DbPro.UretimEmri;
        IUretimOperasyonService _srvOpr = Ortak.DbPro.UretimOperasyon;
        IGenelService _srvGenel = Ortak.DbPro.GenelServis;
        private List<UretimEmri> _list;
        private List<UretimOperasyon> _listOpr;
        private SiparisManager _mng;
        private UretimEmriManager _mngUretim;
        string durumu = "Uretimde";

        public bool SiparisKodundanFiltrele { get; set; } = false; 
        public string SiparisKodu { get; set; } = "";


        public FrmUretimEmriListesi()
        {
            InitializeComponent();
            this.Load += Frm_Load;

        }
        private void Frm_Load(object sender, EventArgs e)
        {
            _mng = new SiparisManager(Ortak.DbPro, Ortak.DbMikro);
            _mngUretim = new UretimEmriManager(Ortak.DbPro);

            TxtTarihi1.Text = DateTime.Now.AddMonths(-1).ToShortDateString();
            TxtTarihi2.Text = "";
            TxtTarihi3.Text = "";
            TxtTarihi4.Text = "";
            BtnAra.Click += BtnAra_Click;
            BtnTemizle.Click += BtnTemizle_Click;
            BtnYazdir.Click += BtnYazdir_Click;
            BtnTemizle.Click += BtnTemizle_Click;
            myView1.MyEventDoubleClickEnter += MyView1_MyEventDoubleClickEnter;
            myView1.FocusedRowChanged += MyView1_FocusedRowChanged;

            BaglaTuru();
            BaglaDurum();
            BaglaReceteAdi();

            if (SiparisKodundanFiltrele)
            {
                SiparisFiltreAyarla();
            }

            Bagla();
            SutunGizle();
            myGrid1.GridYerlesimYukle();
            TxtAdi.Focus();
            DurumuAyarla(durumu);
            BtnAra.PerformClick();
            TasarimUygula();
        }

        /// <summary>Bu forma ozel gorsel stil: grid temasi + acik dairesel arka plan. (Global temaya dokunmaz.)</summary>
        private void TasarimUygula()
        {
            // Grid temasini ONCE uygula (arka plandan bagimsiz - dosya kilidi olsa bile etkilenmez)
            try { GridStil(myView1); } catch { }
            try { GridStil(myView2); } catch { }
            // Arka plani dosyayi KILITLEMEDEN yukle (Image.FromFile kilitler; ReadAllBytes + clone kilitlemez)
            try
            {
                var bg = System.IO.Path.Combine(Application.StartupPath, "Resimler", "senkron-form-bg.png");
                if (System.IO.File.Exists(bg))
                {
                    var bytes = System.IO.File.ReadAllBytes(bg);
                    using (var ms = new System.IO.MemoryStream(bytes))
                    using (var tmp = Image.FromStream(ms))
                        this.BackgroundImage = new Bitmap(tmp);
                    this.BackgroundImageLayout = ImageLayout.Stretch;
                }
                else { this.BackColor = Color.FromArgb(238, 243, 250); }
            }
            catch { this.BackColor = Color.FromArgb(238, 243, 250); }
            ButonlariBoya();
        }

        private void GridStil(DevExpress.XtraGrid.Views.Grid.GridView gv)
        {
            if (gv == null) return;
            gv.OptionsView.ShowGroupPanel = false;
            gv.OptionsView.EnableAppearanceEvenRow = true;
            gv.OptionsView.EnableAppearanceOddRow = true;
            gv.Appearance.EvenRow.BackColor = Color.FromArgb(234, 242, 252);
            gv.Appearance.EvenRow.Options.UseBackColor = true;
            gv.Appearance.OddRow.BackColor = Color.White;
            gv.Appearance.OddRow.Options.UseBackColor = true;
            gv.Appearance.Row.Font = new Font("Segoe UI", 9.5F);
            gv.Appearance.Row.ForeColor = Color.FromArgb(33, 43, 60);
            gv.Appearance.Row.Options.UseFont = true;
            gv.Appearance.Row.Options.UseForeColor = true;
            gv.Appearance.FocusedRow.BackColor = Color.FromArgb(255, 213, 79);
            gv.Appearance.FocusedRow.ForeColor = Color.Black;
            gv.Appearance.FocusedRow.Options.UseBackColor = true;
            gv.Appearance.FocusedRow.Options.UseForeColor = true;
            gv.RowHeight = 27;
            // Baslik rengini skin EZIYOR -> kendimiz cizerek baypas ediyoruz
            gv.CustomDrawColumnHeader -= GridBaslikCiz;
            gv.CustomDrawColumnHeader += GridBaslikCiz;
            if (gv.GridControl != null) gv.GridControl.Refresh();
        }

        private void GridBaslikCiz(object sender, DevExpress.XtraGrid.Views.Grid.ColumnHeaderCustomDrawEventArgs e)
        {
            if (e.Column == null) return; // satir gostergesi / bos baslik -> varsayilan
            var r = e.Bounds;
            using (var br = new System.Drawing.Drawing2D.LinearGradientBrush(r, Color.FromArgb(33, 64, 110), Color.FromArgb(21, 101, 192), System.Drawing.Drawing2D.LinearGradientMode.Vertical))
                e.Graphics.FillRectangle(br, r);
            using (var pen = new Pen(Color.FromArgb(70, 100, 150)))
                e.Graphics.DrawLine(pen, r.Right - 1, r.Top + 3, r.Right - 1, r.Bottom - 3);
            string cap = e.Column.GetCaption();
            var sf = new StringFormat { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Near, FormatFlags = StringFormatFlags.NoWrap, Trimming = StringTrimming.EllipsisCharacter };
            using (var f = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold))
                e.Graphics.DrawString(cap, f, Brushes.White, new RectangleF(r.Left + 7, r.Top, r.Width - 9, r.Height), sf);
            e.Handled = true;
        }

        // ---- Bu forma ozel buton + alt arac cubugu temasi (global temaya dokunmaz) ----

        private void ButonlariBoya()
        {
            Color toolbar = Color.FromArgb(238, 242, 248);
            try { this.BackColor = toolbar; } catch { }
            PanelBoya(pnlAltBtn, toolbar);
            PanelBoya(pnlAltBtn2, toolbar);
            PanelBoya(pnl_altBtn1, toolbar);
            PanelBoya(pblAraBtn, toolbar);

            const string icAra = "<svg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 24 24' fill='none' stroke='#FFFFFF' stroke-width='2.3' stroke-linecap='round' stroke-linejoin='round'><circle cx='11' cy='11' r='7'/><line x1='21' y1='21' x2='16.7' y2='16.7'/></svg>";
            const string icTemizle = "<svg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 24 24' fill='none' stroke='#FFFFFF' stroke-width='2.3' stroke-linecap='round' stroke-linejoin='round'><polyline points='23 4 23 10 17 10'/><path d='M20.5 15a9 9 0 1 1-2.1-9.4L23 10'/></svg>";
            const string icYazdir = "<svg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 24 24' fill='none' stroke='#FFFFFF' stroke-width='2.1' stroke-linecap='round' stroke-linejoin='round'><path d='M6 9V2h12v7'/><path d='M6 18H4a2 2 0 0 1-2-2v-5a2 2 0 0 1 2-2h16a2 2 0 0 1 2 2v5a2 2 0 0 1-2 2h-2'/><rect x='6' y='14' width='12' height='8'/></svg>";
            const string icRecete = "<svg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 24 24' fill='none' stroke='#FFFFFF' stroke-width='2.1' stroke-linecap='round' stroke-linejoin='round'><rect x='5' y='4' width='14' height='17' rx='2'/><path d='M9 4V3h6v1'/><line x1='12' y1='9' x2='12' y2='16'/><line x1='8.5' y1='12.5' x2='15.5' y2='12.5'/></svg>";
            const string icSiparis = "<svg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 24 24' fill='none' stroke='#FFFFFF' stroke-width='2.1' stroke-linecap='round' stroke-linejoin='round'><circle cx='9' cy='21' r='1.4'/><circle cx='19' cy='21' r='1.4'/><path d='M2 3h3l2.4 12a1.5 1.5 0 0 0 1.5 1.2h8.7a1.5 1.5 0 0 0 1.5-1.2L22 7H6'/></svg>";
            const string icKapat = "<svg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 24 24' fill='none' stroke='#FFFFFF' stroke-width='2.6' stroke-linecap='round' stroke-linejoin='round'><line x1='18' y1='6' x2='6' y2='18'/><line x1='6' y1='6' x2='18' y2='18'/></svg>";

            ButonStil(BtnAra,            Color.FromArgb(21, 101, 192), Color.FromArgb(13, 71, 161),  icAra);
            ButonStil(BtnTemizle,        Color.FromArgb(96, 125, 139), Color.FromArgb(69, 90, 100),  icTemizle);
            ButonStil(BtnYazdir,         Color.FromArgb(69, 90, 100),  Color.FromArgb(38, 50, 56),   icYazdir);
            ButonStil(BtnEkleReceteden,  Color.FromArgb(46, 125, 50),  Color.FromArgb(27, 94, 32),   icRecete);
            ButonStil(BtnEkleSiparisden, Color.FromArgb(0, 131, 143),  Color.FromArgb(0, 96, 100),   icSiparis);
            ButonStil(BtnKapat,          Color.FromArgb(198, 40, 40),  Color.FromArgb(183, 28, 28),  icKapat);

            DurumStil(BtnDurumuHepsi);
            DurumStil(BtnDurumuBeklemede);
            DurumStil(BtnDurumuUretimde);
            DurumStil(BtnDurumuHazir);
        }

        private void ButonStil(DevExpress.XtraEditors.SimpleButton btn, Color bg, Color hover, string svg)
        {
            if (btn == null) return;
            try
            {
                btn.LookAndFeel.UseDefaultLookAndFeel = false;
                btn.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
                btn.Appearance.BackColor = bg;
                btn.Appearance.BackColor2 = bg;
                btn.Appearance.BorderColor = bg;
                btn.Appearance.ForeColor = Color.White;
                btn.Appearance.Font = new Font("Segoe UI Semibold", 9.25F, FontStyle.Bold);
                btn.Appearance.Options.UseBackColor = true;
                btn.Appearance.Options.UseBorderColor = true;
                btn.Appearance.Options.UseForeColor = true;
                btn.Appearance.Options.UseFont = true;
                btn.AppearanceHovered.BackColor = hover;
                btn.AppearanceHovered.BackColor2 = hover;
                btn.AppearanceHovered.ForeColor = Color.White;
                btn.AppearanceHovered.BorderColor = hover;
                btn.AppearanceHovered.Options.UseBackColor = true;
                btn.AppearanceHovered.Options.UseForeColor = true;
                btn.AppearanceHovered.Options.UseBorderColor = true;
                btn.AppearancePressed.BackColor = hover;
                btn.AppearancePressed.ForeColor = Color.White;
                btn.AppearancePressed.Options.UseBackColor = true;
                btn.AppearancePressed.Options.UseForeColor = true;
                var ic = SvgIkon(svg, 22);
                if (ic != null)
                {
                    btn.ImageOptions.Image = ic;
                    btn.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
                }
            }
            catch { }
        }

        private void DurumStil(DevExpress.XtraEditors.SimpleButton btn)
        {
            if (btn == null) return;
            try
            {
                btn.LookAndFeel.UseDefaultLookAndFeel = false;
                btn.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
                btn.Appearance.BackColor = Color.FromArgb(236, 239, 244);
                btn.Appearance.BorderColor = Color.FromArgb(206, 214, 226);
                btn.Appearance.ForeColor = Color.FromArgb(55, 71, 79);
                btn.Appearance.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Bold);
                btn.Appearance.Options.UseBackColor = true;
                btn.Appearance.Options.UseBorderColor = true;
                btn.Appearance.Options.UseForeColor = true;
                btn.Appearance.Options.UseFont = true;
                btn.AppearanceHovered.BackColor = Color.FromArgb(214, 224, 240);
                btn.AppearanceHovered.ForeColor = Color.FromArgb(21, 101, 192);
                btn.AppearanceHovered.Options.UseBackColor = true;
                btn.AppearanceHovered.Options.UseForeColor = true;
                btn.AppearancePressed.BackColor = Color.FromArgb(21, 101, 192);
                btn.AppearancePressed.ForeColor = Color.White;
                btn.AppearancePressed.Options.UseBackColor = true;
                btn.AppearancePressed.Options.UseForeColor = true;
            }
            catch { }
        }

        private void PanelBoya(DevExpress.XtraEditors.PanelControl p, Color c)
        {
            if (p == null) return;
            try
            {
                p.Appearance.BackColor = c;
                p.Appearance.BackColor2 = c;
                p.Appearance.Options.UseBackColor = true;
            }
            catch { }
        }

        private Image SvgIkon(string svg, int size)
        {
            try
            {
                using (var ms = new System.IO.MemoryStream(System.Text.Encoding.UTF8.GetBytes(svg)))
                {
                    var simg = DevExpress.Utils.Svg.SvgImage.FromStream(ms);
                    return new DevExpress.Utils.Svg.SvgBitmap(simg).Render(new Size(size, size), (DevExpress.Utils.Design.ISvgPaletteProvider)null);
                }
            }
            catch { return null; }
        }

        void SiparisFiltreAyarla()
        {
            durumu = "Tümü";
            TxtKodu.Text = SiparisKodu;
            TxtTarihi1.EditValue = Convert.ToDateTime(DateTime.Now.Year + "-01-01");

        }
        public void SutunGizle()
        {
            myView1.SutunGizle("Id");
            myView1.SutunGizle("RcAId");
            myView1.SutunGizle("SipId");
            myView1.SutunFormat("BaslangicTarihi", DevExpress.Utils.FormatType.DateTime, "dd.MM.yyyy HH:mm");
            myView1.SutunFormat("BitisTarihi", DevExpress.Utils.FormatType.DateTime, "dd.MM.yyyy HH:mm");
            myView1.SutunCaptionDegistir("SiparisKodu", "IsEmriKodu");
        }
        private void SutunGizle2()
        {
            myView2.SutunGizle("Id");
            myView2.SutunGizle("UrId");
            myView2.SutunGizle("RcAId");
            myView2.SutunGizle("RcOId");
            myView2.SutunGizle("SipId");
            myView2.SutunGizle("SipHId");
            myView2.SutunFormat("BaslangicTarihi", DevExpress.Utils.FormatType.DateTime, "dd.MM.yyyy HH:mm");
            myView2.SutunFormat("BitisTarihi", DevExpress.Utils.FormatType.DateTime, "dd.MM.yyyy HH:mm");

        }

        private void Bagla()
        {
            Cursor.Current = Cursors.WaitCursor;
            string sor = SorguAyarla() + SorguAyarlaTrh();
            if (!string.IsNullOrEmpty(sor))
            {
                sor = "where 1=1 " + sor;
            }
            var rs = _srv.SelectListWhere(sor);
            if (!rs.Success)
            {
                Cursor.Current = Cursors.Default;
                MesajHata(rs.Message);
                return;
            }
            _list = rs.Data.ToList();
            myGrid1.DataSource = _list;
            if (rs.Data.Any())
            {
                BaglaOperasyon(rs.Data.FirstOrDefault().Id);
            }
            Cursor.Current = Cursors.Default;
        }
        private void BaglaOperasyon(Guid? UrId)
        {
            Cursor.Current = Cursors.WaitCursor;
            string sor = "where UrId='" + UrId + "' ";
            var rs = _srvOpr.SelectListWhere(sor);
            if (!rs.Success)
            {
                Cursor.Current = Cursors.Default;
                MesajHata(rs.Message);
                return;
            }
            _listOpr = rs.Data.ToList();
            myGrid2.DataSource = _listOpr;
            SutunGizle2();
            myGrid2.GridYerlesimYukle();
            Cursor.Current = Cursors.Default;
        }
        public void BaglaTuru()
        {
            var rs = _srvGenel.GrupListesi("UretimEmri", "Turu");
            if (!rs.Success)
            {
                MesajHata(rs.Message);
                return;
            }
            var dt = rs.Data.ToList();
            dt.Insert(0, "");
            CmbTuru.MyDataBagla(dt);
        }
        public void BaglaDurum()
        {
            var rs = _srvGenel.GrupListesi("UretimEmri", "Durumu");
            if (!rs.Success)
            {
                MesajHata(rs.Message);
                return;
            }
            var dt = rs.Data.ToList();
            dt.Insert(0, "");
            CmbDurumu.MyDataBagla(dt);
        }
        public void BaglaReceteAdi()
        {
            var rs = _srvGenel.GrupListesi("UretimEmri", "ReceteAdi");
            if (!rs.Success)
            {
                MesajHata(rs.Message);
                return;
            }
            var dt = rs.Data.ToList();
            dt.Insert(0, "");
            CmbReceteAdi.MyDataBagla(dt);
        }
        private void DurumGuncelle(object sender, EventArgs e)
        {
            var itm = myView1.MyGetCurrentItem<UretimEmri>();
            if (itm != null)
            {
                var rs = _mngUretim.UretimDurumGuncelle(itm.Id);
                if (!rs.Success)
                {
                    MesajHata(rs.Message);
                    return;
                }
                MesajBilgi("Güncellendi.");
            }
        }
        public string SorguAyarla()
        {
            string sor = "";
            string t1 = TxtKodu.Text.Trim();
            if (!string.IsNullOrEmpty(t1)) { sor += $" AND  SiparisKodu    like('%{t1}%')"; }
            t1 = TxtCariKodu.Text.Trim();
            if (!string.IsNullOrEmpty(t1)) { sor += $" AND  SiparisCariKodu    like('%{t1}%')"; }
            t1 = TxtAdi.Text.Trim();
            if (!string.IsNullOrEmpty(t1)) { sor += $" AND  SiparisCariUnvani like('%{t1}%')  "; }
            t1 = CmbTuru.Text.Trim();
            if (!string.IsNullOrEmpty(t1)) { sor += $" AND  Turu = '{t1}' "; }
            //t1 = CmbDurumu.Text.Trim();
            //if (!string.IsNullOrEmpty(t1)) { sor += $" AND  Durumu = '{t1}' "; }
            t1 = durumu.Trim();
            if (!string.IsNullOrEmpty(t1)) { sor += $" AND  Durumu = '{t1}' "; }

            t1 = CmbReceteAdi.Text.Trim();
            if (!string.IsNullOrEmpty(t1)) { sor += $" AND  ReceteAdi like('%{t1}%')  "; }
            return sor;
        }
        private string SorguAyarlaTrh()
        {
            string sor = "";
            string t1 = "";
            t1 = TxtTarihi1.Text.ToString().Trim();
            if (!string.IsNullOrEmpty(t1))
            {
                t1 = TarihAyarla(Convert.ToDateTime(TxtTarihi1.Text)).ToString();
                if (!string.IsNullOrEmpty(t1)) sor += $"  AND  CAST (coalesce( BaslangicTarihi,'1901-01-01') AS DATE ) >=  CAST('{t1}'  AS DATE ) ";
            }
            t1 = TxtTarihi2.Text.ToString().Trim();
            if (!string.IsNullOrEmpty(t1))
            {
                t1 = TarihAyarla(Convert.ToDateTime(TxtTarihi2.Text)).ToString();
                if (!string.IsNullOrEmpty(t1)) sor += $"  AND  CAST ( coalesce(BaslangicTarihi,'1901-01-01') AS DATE ) <=  CAST('{t1}'  AS DATE ) ";
            }
            t1 = TxtTarihi3.Text.ToString().Trim();
            if (!string.IsNullOrEmpty(t1))
            {
                t1 = TarihAyarla(Convert.ToDateTime(TxtTarihi3.Text)).ToString();
                if (!string.IsNullOrEmpty(t1)) sor += $"  AND  CAST (coalesce( BitisTarihi,'1901-01-01') AS DATE ) >=  CAST('{t1}'  AS DATE ) ";
            }
            t1 = TxtTarihi4.Text.ToString().Trim();
            if (!string.IsNullOrEmpty(t1))
            {
                t1 = TarihAyarla(Convert.ToDateTime(TxtTarihi4.Text)).ToString();
                if (!string.IsNullOrEmpty(t1)) sor += $"  AND  CAST (coalesce( BitisTarihi,'1901-01-01') AS DATE ) <=  CAST('{t1}'  AS DATE ) ";
            }
            return sor;
        }

        public string TarihAyarla(DateTime? T1)
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
                var itm = myView1.MyGetCurrentItem<UretimEmri>();
                if (itm != null)
                {
                    SecilenKod = itm.SiparisKodu;
                    SecilenRow = itm;
                    Secildi = true;
                    this.Close();
                }
            }
            else
            {
                var itm = myView1.MyGetCurrentItem<UretimEmri>();
                if (itm != null)
                {
                    FrmUretimEmriED f = new FrmUretimEmriED();
                    f.pblAlt.Visible = false;
                    f.IdGuid = itm.Id;
                    f.ActionAktar = Bagla;
                    f.ShowDialog();
                }
            }
        }
        private void MyView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {

            var itm = myView1.MyGetCurrentItem<UretimEmri>();
            if (itm != null)
            {
                BaglaOperasyon(itm.Id);
            }
            else
            {
                myGrid2.DataSource = null;

            }
        }
        private void BtnAra_Click(object sender, EventArgs e)
        {
            Bagla();
        }
        private void BtnTemizle_Click(object sender, EventArgs e)
        {
            TxtKodu.Text = "";
            TxtAdi.Text = "";
            CmbDurumu.Text = "";
            CmbTuru.Text = "";
            CmbReceteAdi.Text = "";
            TxtCariKodu.Text = "";
            TxtTarihi1.Text = "";
            TxtTarihi2.Text = "";
            TxtTarihi3.Text = "";
            TxtTarihi4.Text = "";
        }

        private void BtnYazdir_Click(object sender, EventArgs e)
        {
            Yazdir();
        }
        public void Yazdir()
        {
            var itm = myView1.MyGetCurrentItem<UretimEmri>();
            if (itm != null)
            {
                var rs = _mng.GetSiparis(itm.Id);
                if (!rs.Success)
                {
                    MesajHata(rs.Message);
                    return;
                }
                var _mdl = rs.Data;
                string YazdirmaAdi = "UretimEmriListesi";
                DataSet ds = new DataSet("UretimEmriDS");
                ds.Tables.Add(_list.ToDataTable("UretimEmirleri"));
                ds.Yaz(YazdirmaAdi, false);
            }
        }

        private void BtnEkleReceteden_Click(object sender, EventArgs e)
        {
            FrmUretimEmriED f = new FrmUretimEmriED();
            f.ActionAktar = Bagla;
            f.UretimTuru = "Recete";
            f.ShowDialog();
        }
        private void BtnEkleSiparisden_Click(object sender, EventArgs e)
        {
            FrmUretimEmriED f = new FrmUretimEmriED();
            f.ActionAktar = Bagla;
            f.UretimTuru = "Siparis";
            f.ShowDialog();
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