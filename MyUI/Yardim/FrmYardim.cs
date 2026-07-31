using System.Drawing;
using System.Windows.Forms;

namespace MyUI.Yardim
{
    /// <summary>
    /// Yardim icerigini gosteren basit (DevExpress'siz) WinForms penceresi.
    /// </summary>
    public class FrmYardim : Form
    {
        public FrmYardim(string formAdi, HelpItem item)
        {
            Text = (item != null && !string.IsNullOrEmpty(item.Baslik))
                ? ("Yardim - " + item.Baslik)
                : "Yardim";
            StartPosition = FormStartPosition.CenterParent;
            Size = new Size(700, 580);
            MinimizeBox = false;
            MaximizeBox = false;
            ShowIcon = false;
            ShowInTaskbar = false;

            var rtb = new RichTextBox
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                BorderStyle = BorderStyle.None,
                BackColor = Color.White,
                Font = new Font("Segoe UI", 10F),
                DetectUrls = false
            };
            var pad = new Panel { Dock = DockStyle.Fill, Padding = new Padding(16), BackColor = Color.White };
            pad.Controls.Add(rtb);
            Controls.Add(pad);

            var alt = new Panel { Dock = DockStyle.Bottom, Height = 46, BackColor = Color.FromArgb(245, 245, 245) };
            var btnKapat = new Button
            {
                Text = "Kapat (Esc)",
                Width = 120,
                Height = 30,
                Anchor = AnchorStyles.Right,
                DialogResult = DialogResult.Cancel
            };
            btnKapat.Location = new Point(Size.Width - btnKapat.Width - 30, 8);
            alt.Controls.Add(btnKapat);
            Controls.Add(alt);
            CancelButton = btnKapat;

            Doldur(rtb, formAdi, item);
        }

        private void Doldur(RichTextBox r, string formAdi, HelpItem item)
        {
            if (item == null)
            {
                r.SelectionFont = new Font("Segoe UI", 11F, FontStyle.Bold);
                r.AppendText("Bu ekran icin yardim icerigi bulunamadi.\n\n");
                r.SelectionFont = new Font("Segoe UI", 9F, FontStyle.Regular);
                r.SelectionColor = Color.Gray;
                r.AppendText("Form: " + formAdi + "\n");
                return;
            }

            Bolum(r, "Ne ise yarar", item.Amac);
            Bolum(r, "Once ne olmali (onkosul)", item.Onkosul);
            Bolum(r, "Sonra ne olur", item.Sonra);

            if (item.Butonlar != null && item.Butonlar.Length > 0)
            {
                Baslik(r, "Butonlar & kisayollar");
                r.SelectionFont = new Font("Segoe UI", 10F, FontStyle.Regular);
                r.SelectionColor = Color.Black;
                foreach (var b in item.Butonlar)
                {
                    if (!string.IsNullOrWhiteSpace(b)) r.AppendText("   • " + b + "\n");
                }
                r.AppendText("\n");
            }

            Bolum(r, "Istasyon sirasiyla iliskisi", item.Istasyon);
            Bolum(r, "Notlar", item.Notlar);

            r.SelectionStart = 0;
            r.ScrollToCaret();
        }

        private void Baslik(RichTextBox r, string baslik)
        {
            r.SelectionFont = new Font("Segoe UI", 11F, FontStyle.Bold);
            r.SelectionColor = Color.FromArgb(21, 101, 192);
            r.AppendText(baslik + "\n");
        }

        private void Bolum(RichTextBox r, string baslik, string metin)
        {
            if (string.IsNullOrWhiteSpace(metin)) return;
            Baslik(r, baslik);
            r.SelectionFont = new Font("Segoe UI", 10F, FontStyle.Regular);
            r.SelectionColor = Color.Black;
            r.AppendText(metin + "\n\n");
        }
    }
}
