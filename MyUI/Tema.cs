using System;
using System.Drawing;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;

namespace MyUI
{
    /// <summary>
    /// Tum MDI formlarina tek yerden uygulanan gorsel tema:
    /// - Butonlar ada gore rol bazli renklendirilir (Kaydet/Sil/Kapat/Yazdir/Ara/Yeni/Ekle/...) + beyaz ikon.
    /// - Grid'lere lacivert baslik (CustomDraw) + alternatif satir + odak satiri temasi.
    /// - Buton iceren paneller (alt arac cubugu) acik tonla boyanir.
    /// Global skin'e DOKUNMAZ. FrmAna.MdiChildActivate'ten her form icin bir kez cagrilir.
    /// </summary>
    public static class Tema
    {
        static readonly Color ToolbarBg = Color.FromArgb(238, 242, 248);
        static readonly Color SatirCift = Color.FromArgb(234, 242, 252);
        static readonly Color Odak = Color.FromArgb(255, 213, 79);
        static readonly Color BaslikUst = Color.FromArgb(33, 64, 110);
        static readonly Color BaslikAlt = Color.FromArgb(21, 101, 192);

        public static void FormGuzellestir(Form f)
        {
            if (f == null) return;
            try { f.BackColor = ToolbarBg; } catch { }
            KontrolGez(f);
            try { f.Refresh(); } catch { }
        }

        static void KontrolGez(Control parent)
        {
            foreach (Control c in parent.Controls)
            {
                try
                {
                    GridControl gc = c as GridControl;
                    if (gc != null) { GridBoya(gc); }
                    else
                    {
                        SimpleButton sb = c as SimpleButton;
                        if (sb != null) { ButonBoya(sb); }
                        else
                        {
                            PanelControl pc = c as PanelControl;
                            if (pc != null) { PanelBoyaBelki(pc); }
                        }
                    }
                }
                catch { }
                if (c.HasChildren) KontrolGez(c);
            }
        }

        // ---------------- BUTON ----------------

        static void ButonBoya(SimpleButton btn)
        {
            string n = ((btn.Name ?? "") + "|" + (btn.Text ?? "")).ToLowerInvariant();

            // Filtre/durum butonlari kendi secim renk mantigini yonetir -> dokunma
            if (n.Contains("durumu") || n.Contains("aktarildi") || n.Contains("kapandi")) return;

            Color bg = Color.Empty;
            string svg = null;

            if (n.Contains("kaydet") || n.Contains("giris")) { bg = C(46, 125, 50); svg = S(IK_SAVE); }
            else if (n.Contains("sil")) { bg = C(198, 40, 40); svg = S(IK_TRASH); }
            else if (n.Contains("kapat")) { bg = C(198, 40, 40); svg = S(IK_X); }
            else if (n.Contains("yazdir")) { bg = C(69, 90, 100); svg = S(IK_PRINT); }
            else if (n.Contains("temizle")) { bg = C(96, 125, 139); svg = S(IK_ERASE); }
            else if (n.Contains("yenile")) { bg = C(2, 136, 209); svg = S(IK_REFRESH); }
            else if (n.Contains("yeni")) { bg = C(0, 131, 143); svg = S(IK_PLUS); }
            else if (n.Contains("siparis") && n.Contains("ekle")) { bg = C(0, 131, 143); svg = S(IK_CART); }
            else if (n.Contains("recete") && n.Contains("ekle")) { bg = C(46, 125, 50); svg = S(IK_CLIP); }
            else if (n.Contains("ekle")) { bg = C(46, 125, 50); svg = S(IK_PLUS); }
            else if (n.Contains("gonder")) { bg = C(2, 136, 209); svg = S(IK_SEND); }
            else if (n.Contains("degistir") || n.Contains("duzenle")) { bg = C(57, 73, 171); svg = S(IK_PEN); }
            else if (n.Contains("sec")) { bg = C(21, 101, 192); svg = S(IK_CHECK); }
            else if (n.Contains("ara")) { bg = C(21, 101, 192); svg = S(IK_SEARCH); }
            else if (n.Contains("bagla") || n.Contains("aktar")) { bg = C(0, 131, 143); svg = S(IK_LINK); }
            else if (n.Contains("tamam") || n.Contains("onay") || n.Contains("uygula") || n.Contains("basla")) { bg = C(46, 125, 50); svg = S(IK_CHECK); }

            if (bg == Color.Empty) { NotrButon(btn); return; }
            VividButon(btn, bg, svg);
        }

        static void VividButon(SimpleButton btn, Color bg, string svg)
        {
            Color hover = Karart(bg, 0.82);
            btn.LookAndFeel.UseDefaultLookAndFeel = false;
            btn.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
            btn.Appearance.BackColor = bg;
            btn.Appearance.BackColor2 = bg;
            btn.Appearance.BorderColor = bg;
            btn.Appearance.ForeColor = Color.White;
            btn.Appearance.Options.UseBackColor = true;
            btn.Appearance.Options.UseBorderColor = true;
            btn.Appearance.Options.UseForeColor = true;
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
            if (svg != null)
            {
                Image ic = SvgIkon(svg, 20);
                if (ic != null)
                {
                    btn.ImageOptions.Image = ic;
                    btn.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
                }
            }
        }

        static void NotrButon(SimpleButton btn)
        {
            btn.LookAndFeel.UseDefaultLookAndFeel = false;
            btn.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
            btn.Appearance.BackColor = C(236, 239, 244);
            btn.Appearance.BorderColor = C(206, 214, 226);
            btn.Appearance.ForeColor = C(42, 58, 82);
            btn.Appearance.Options.UseBackColor = true;
            btn.Appearance.Options.UseBorderColor = true;
            btn.Appearance.Options.UseForeColor = true;
            btn.AppearanceHovered.BackColor = C(220, 228, 240);
            btn.AppearanceHovered.Options.UseBackColor = true;
            btn.AppearancePressed.BackColor = C(206, 216, 232);
            btn.AppearancePressed.Options.UseBackColor = true;
        }

        // ---------------- PANEL ----------------

        static void PanelBoyaBelki(PanelControl p)
        {
            bool butonVar = false;
            foreach (Control ch in p.Controls)
            {
                if (ch is SimpleButton) { butonVar = true; break; }
            }
            if (!butonVar) return;
            p.Appearance.BackColor = ToolbarBg;
            p.Appearance.BackColor2 = ToolbarBg;
            p.Appearance.Options.UseBackColor = true;
        }

        // ---------------- GRID ----------------

        static void GridBoya(GridControl gc)
        {
            foreach (DevExpress.XtraGrid.Views.Base.BaseView bv in gc.Views)
            {
                GridView gv = bv as GridView;
                if (gv != null) GridViewStil(gv);
            }
            GridView mv = gc.MainView as GridView;
            if (mv != null) GridViewStil(mv);
        }

        static void GridViewStil(GridView gv)
        {
            gv.OptionsView.EnableAppearanceEvenRow = true;
            gv.OptionsView.EnableAppearanceOddRow = true;
            gv.Appearance.EvenRow.BackColor = SatirCift;
            gv.Appearance.EvenRow.Options.UseBackColor = true;
            gv.Appearance.OddRow.BackColor = Color.White;
            gv.Appearance.OddRow.Options.UseBackColor = true;
            gv.Appearance.FocusedRow.BackColor = Odak;
            gv.Appearance.FocusedRow.ForeColor = Color.Black;
            gv.Appearance.FocusedRow.Options.UseBackColor = true;
            gv.Appearance.FocusedRow.Options.UseForeColor = true;
            gv.CustomDrawColumnHeader -= GridBaslikCiz;
            gv.CustomDrawColumnHeader += GridBaslikCiz;
            if (gv.GridControl != null) gv.GridControl.Refresh();
        }

        static void GridBaslikCiz(object sender, DevExpress.XtraGrid.Views.Grid.ColumnHeaderCustomDrawEventArgs e)
        {
            if (e.Column == null) return;
            Rectangle r = e.Bounds;
            using (var br = new System.Drawing.Drawing2D.LinearGradientBrush(r, BaslikUst, BaslikAlt, System.Drawing.Drawing2D.LinearGradientMode.Vertical))
                e.Graphics.FillRectangle(br, r);
            using (var pen = new Pen(Color.FromArgb(70, 100, 150)))
                e.Graphics.DrawLine(pen, r.Right - 1, r.Top + 3, r.Right - 1, r.Bottom - 3);
            string cap = e.Column.GetCaption();
            var sf = new StringFormat { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Near, FormatFlags = StringFormatFlags.NoWrap, Trimming = StringTrimming.EllipsisCharacter };
            using (var fnt = new Font("Segoe UI Semibold", 9.5F, FontStyle.Bold))
                e.Graphics.DrawString(cap, fnt, Brushes.White, new RectangleF(r.Left + 7, r.Top, r.Width - 9, r.Height), sf);
            e.Handled = true;
        }

        // ---------------- YARDIMCI ----------------

        static Color C(int r, int g, int b) { return Color.FromArgb(r, g, b); }

        static Color Karart(Color c, double f)
        {
            int r = (int)(c.R * f); int g = (int)(c.G * f); int b = (int)(c.B * f);
            if (r < 0) r = 0; if (g < 0) g = 0; if (b < 0) b = 0;
            return Color.FromArgb(r, g, b);
        }

        static Image SvgIkon(string svg, int size)
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

        static string S(string inner)
        {
            return "<svg xmlns='http://www.w3.org/2000/svg' viewBox='0 0 24 24' fill='none' stroke='#FFFFFF' stroke-width='2.1' stroke-linecap='round' stroke-linejoin='round'>" + inner + "</svg>";
        }

        // Ikon govdeleri (beyaz cizgi)
        const string IK_SAVE = "<path d='M5 3h11l3 3v15H5z'/><path d='M8 3v6h7V3'/><rect x='8' y='13' width='8' height='6'/>";
        const string IK_TRASH = "<polyline points='3 6 5 6 21 6'/><path d='M8 6V4h8v2M6 6l1 14h10l1-14'/><line x1='10' y1='10.5' x2='10' y2='17'/><line x1='14' y1='10.5' x2='14' y2='17'/>";
        const string IK_X = "<line x1='18' y1='6' x2='6' y2='18'/><line x1='6' y1='6' x2='18' y2='18'/>";
        const string IK_PRINT = "<path d='M6 9V2h12v7'/><path d='M6 18H4a2 2 0 0 1-2-2v-5a2 2 0 0 1 2-2h16a2 2 0 0 1 2 2v5a2 2 0 0 1-2 2h-2'/><rect x='6' y='14' width='12' height='8'/>";
        const string IK_ERASE = "<polyline points='23 4 23 10 17 10'/><path d='M20.5 15a9 9 0 1 1-2.1-9.4L23 10'/>";
        const string IK_REFRESH = "<polyline points='23 4 23 10 17 10'/><path d='M20.5 15a9 9 0 1 1-2.1-9.4L23 10'/>";
        const string IK_PLUS = "<line x1='12' y1='5' x2='12' y2='19'/><line x1='5' y1='12' x2='19' y2='12'/>";
        const string IK_CHECK = "<polyline points='20 6 9 17 4 12'/>";
        const string IK_PEN = "<path d='M12 20h9'/><path d='M16.5 3.5a2.1 2.1 0 0 1 3 3L7 19l-4 1 1-4z'/>";
        const string IK_SEND = "<line x1='22' y1='2' x2='11' y2='13'/><polygon points='22 2 15 22 11 13 2 9 22 2'/>";
        const string IK_LINK = "<path d='M10 13a5 5 0 0 0 7 0l3-3a5 5 0 0 0-7-7l-1 1'/><path d='M14 11a5 5 0 0 0-7 0l-3 3a5 5 0 0 0 7 7l1-1'/>";
        const string IK_SEARCH = "<circle cx='11' cy='11' r='7'/><line x1='21' y1='21' x2='16.7' y2='16.7'/>";
        const string IK_CART = "<circle cx='9' cy='21' r='1.4'/><circle cx='19' cy='21' r='1.4'/><path d='M2 3h3l2.4 12a1.5 1.5 0 0 0 1.5 1.2h8.7a1.5 1.5 0 0 0 1.5-1.2L22 7H6'/>";
        const string IK_CLIP = "<rect x='5' y='4' width='14' height='17' rx='2'/><path d='M9 4V3h6v1'/><line x1='12' y1='9' x2='12' y2='16'/><line x1='8.5' y1='12.5' x2='15.5' y2='12.5'/>";
    }
}
