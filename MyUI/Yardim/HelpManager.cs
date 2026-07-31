using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace MyUI.Yardim
{
    /// <summary>
    /// Sayfa-ici yardim sistemi. Program.cs'te HelpManager.Baslat() ile kurulur.
    /// help-uretim.json (uygulama klasoru\Yardim) icindeki form-tip-adi -> HelpItem
    /// sozlugunu yukler; F1 veya formdaki "?" butonu ile FrmYardim'i acar.
    /// </summary>
    public static class HelpManager
    {
        private static Dictionary<string, HelpItem> _data;
        private static readonly HashSet<Form> _dekoreEdilen = new HashSet<Form>();

        public static void Baslat()
        {
            Yukle();
            Application.AddMessageFilter(new HelpMessageFilter());
        }

        private static void Yukle()
        {
            _data = new Dictionary<string, HelpItem>(StringComparer.OrdinalIgnoreCase);
            try
            {
                var path = Path.Combine(Application.StartupPath, "Yardim", "help-uretim.json");
                if (File.Exists(path))
                {
                    var json = File.ReadAllText(path);
                    var d = JsonConvert.DeserializeObject<Dictionary<string, HelpItem>>(json);
                    if (d != null)
                    {
                        _data = new Dictionary<string, HelpItem>(d, StringComparer.OrdinalIgnoreCase);
                    }
                }
            }
            catch
            {
                // Yardim icerigi yuklenemezse uygulama calismaya devam etsin.
            }
        }

        /// <summary>
        /// MDI senaryosunda aktif cocugu, aksi halde aktif formu doner.
        /// </summary>
        public static Form EtkinForm()
        {
            var f = Form.ActiveForm;
            if (f != null && f.IsMdiContainer && f.ActiveMdiChild != null)
            {
                return f.ActiveMdiChild;
            }
            return f;
        }

        public static HelpItem Bul(Form f)
        {
            if (f == null || _data == null) return null;
            HelpItem item;
            return _data.TryGetValue(f.GetType().Name, out item) ? item : null;
        }

        public static void Goster(Form f)
        {
            var item = Bul(f);
            var formAdi = f != null ? f.GetType().Name : "";
            using (var d = new FrmYardim(formAdi, item))
            {
                if (f != null) d.ShowDialog(f); else d.ShowDialog();
            }
        }

        /// <summary>
        /// Forma sag-ust kosesinde kucuk bir "?" yardim butonu ekler (bir kez).
        /// </summary>
        public static void Dekore(Form f)
        {
            if (f == null || _dekoreEdilen.Contains(f)) return;
            _dekoreEdilen.Add(f);
            f.FormClosed += (s, e) => _dekoreEdilen.Remove(f);

            // Login / masaustu / yardim / lisans formlarinda "?" butonu gosterme
            var formAd = f.GetType().Name;
            if (formAd == "FrmLogin" || formAd == "FrmMasaustu1" || formAd == "FrmYardim" || formAd == "FrmLisansGiris") return;

            try
            {
                var btn = new Button
                {
                    Text = "?",
                    Size = new Size(30, 26),
                    Anchor = AnchorStyles.Top | AnchorStyles.Right,
                    FlatStyle = FlatStyle.Flat,
                    BackColor = Color.FromArgb(21, 101, 192),
                    ForeColor = Color.White,
                    Font = new Font("Tahoma", 11F, FontStyle.Bold),
                    Cursor = Cursors.Hand,
                    TabStop = false,
                    Name = "btnYardimGenel"
                };
                btn.FlatAppearance.BorderSize = 0;
                btn.Location = new Point(System.Math.Max(0, f.ClientSize.Width - btn.Width - 8), 6);
                btn.Click += (s, e) => Goster(f);
                f.Controls.Add(btn);
                btn.BringToFront();
            }
            catch
            {
                // Buton eklenemezse F1 kisayolu yine de calisir.
            }
        }
    }
}
