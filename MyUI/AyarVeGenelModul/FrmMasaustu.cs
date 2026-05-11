using Microsoft.VisualBasic;
using Newtonsoft.Json; 
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using My.Kontrol.Kontroller;
namespace MyUI
{
    public partial class FrmMasaustu1 : XtraForm
    {
        public string AktifButon = "";
        private bool PanelDuzenle = false;
        bool isDragged = false;
        Point ptOffset;
        Point pold;
        Point pnew;
        private string dosyayolu = Application.StartupPath + "\\FavorilerSettings.json";
        private List<MasaUstuButonlar> butonList = new List<MasaUstuButonlar>();
        public FrmMasaustu1()
        {
            InitializeComponent();
        }
        private void FrmMasaustu_Load(object sender, EventArgs e)
        {
            dosyayolu = Application.StartupPath + "\\FavorilerSettings.json";
            ButonBagla();
            ButonlariOlustur();
        }
        private   void FrmMasaustu_FormClosing(object sender, FormClosingEventArgs e)
        {
          //e.Cancel = Ortak.MasaUstuKapanmasin;
        }
        async Task FormKapat()
        {
            await Task.Delay(500);
            if (!Ortak.MasaUstuKapanmasin)
            {
                this.Close();
            }
        }
        private void button_MouseDown(object sender, MouseEventArgs e)
        {
            if (!PanelDuzenle)
            {
                return;
            }
            var btn = (MyButton)sender;
            if (e.Button == MouseButtons.Left)
            {
                isDragged = true;
                Point ptStartPosition = btn.PointToScreen(new Point(e.X, e.Y));
                pold = btn.PointToScreen(new Point(e.X, e.Y));
                pnew = btn.PointToScreen(new Point(e.X, e.Y));
                ptOffset = new Point();
                ptOffset.X = btn.Location.X - ptStartPosition.X;
                ptOffset.Y = btn.Location.Y - ptStartPosition.Y;
            }
            else
            {
                isDragged = false;
            }
        }
        private void button_MouseEnter(object sender, EventArgs e)
        {
            if (sender.GetType() != typeof(MyButton))
            {
                return;
            }
            var btn = (MyButton)sender;
            AktifButon = btn.Tag.ToString();
        }
        private void button_MouseMove(object sender, MouseEventArgs e)
        {
            if (!PanelDuzenle)
            {
                return;
            }
            var btn = (MyButton)sender;
            if (isDragged)
            {
                Point newPoint = btn.PointToScreen(new Point(e.X, e.Y));
                newPoint.Offset(ptOffset);
                btn.Location = newPoint;
                pnew = btn.PointToScreen(new Point(e.X, e.Y));
            }
        }
        private void button_MouseUp(object sender, MouseEventArgs e)
        {
            if (!PanelDuzenle)
            {
                return;
            }
            isDragged = false;
        }
        private void button_Click(object sender, EventArgs e)
        {
            if (PanelDuzenle)
            {
                var btn1 = (MyButton)sender;
                AktifButon = btn1.Tag.ToString();
                return;
            }
            if (sender.GetType() != typeof(MyButton))
            {
                return;
            }
            if ((pnew.X < pold.X - 5 || pnew.X > pold.X + 5))
            {
                return;
            }
            var btn = (MyButton)sender;
            FormAc.FormSec(btn.Tag.ToString());
        }
        private void SfButton1_Click(object sender, EventArgs e)
        {
            if ((pnew.X < pold.X - 5 || pnew.X > pold.X + 5))
            {
                return;
            }
            MyButton btn = new MyButton();
            btn.Text = "Buton";
            Point p = new Point(100, 100);
            Size s = new Size(100, 50);
            btn.MouseDown += button_MouseDown;
            btn.MouseMove += button_MouseMove;
            btn.MouseUp += button_MouseUp;
            btn.MouseEnter += button_MouseEnter;
            btn.Location = p;
            btn.Size = s;
            panel1.Controls.Add(btn);
        }
        private void DuzenleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DuzenleAc();
        }
        private void KaydetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DuzenleKapat();
        }
        private void DuzenleToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            DuzenleAc();
        }
        private void KaydetToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            DuzenleKapat();
        }
        private void YenidenIsimverToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (PanelDuzenle == false)
            {
                DuzenleAc();
            }
            string YeniAd = Interaction.InputBox("Bilgi Girişi", "İsim Giriniz.", "", -1, -1);
            if (string.IsNullOrEmpty(YeniAd.ToString().Trim()))
            {
                return;
            }
            foreach (var itm in butonList)
            {
                if (itm.Name == AktifButon)
                {
                    itm.Text = YeniAd;
                }
            }
            foreach (Control c in panel1.Controls)
            {
                Type ct = c.GetType();
                Type ct2 = typeof(MyButton);
                if (c.GetType() == typeof(MyButton))
                {
                    var btn = (MyButton)c;
                    if (btn.Tag.ToString() == AktifButon)
                    {
                        btn.Text = YeniAd;
                    }
                }
            }
        }
        private void KaldirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (PanelDuzenle == false)
            {
                DuzenleAc();
            }
            if (MessageBox.Show("Buton Kaldirma Onayla", "Onay", MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                return;
            }
            MasaUstuButonlar tbnAra = null;
            foreach (var itm in butonList)
            {
                if (itm.Name == AktifButon)
                {
                    tbnAra = itm;
                }
            }
            if (tbnAra != null)
            {
                butonList.Remove(tbnAra);
            }
            ButonlariOlustur();
        }
        private void IptalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DuzenleKapat();
        }
        private void IptalToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            DuzenleKapat();
        }
        private void DuzenleAc()
        {
            PanelDuzenle = true;
            panel1.BackColor = Color.DarkCyan;
        }
        private void DuzenleKapat()
        {
            pnew.X = pold.X;
            PanelDuzenle = false;
            panel1.BackColor = Color.FromArgb(233, 236, 239);
            AyarKaydet();
        }
        public void ButonEkle(string Tag)
        {
            if (butonList == null)
            {
                butonList = new List<MasaUstuButonlar>();
            }
            var bt = new MasaUstuButonlar()
            {
                Name = Tag,
                Text = Tag,
                LocationX = 5,
                LocationY = 5,
                SizeX = 200,
                SizeY = 70
            };
            MyButton btn = new MyButton();
            btn.Text = bt.Text;
            btn.Name = bt.Name;
            btn.Tag = bt.Name;
            Point p = new Point(bt.LocationX, bt.LocationY);
            Size s = new Size(bt.SizeX, bt.SizeY);
            btn.MouseDown += button_MouseDown;
            btn.MouseMove += button_MouseMove;
            btn.MouseUp += button_MouseUp;
            btn.Click += button_Click;
            btn.ContextMenuStrip = btnContext;
            btn.Location = p;
            btn.Size = s;
            btn.LookAndFeel.UseDefaultLookAndFeel = false;
            btn.Appearance.BackColor = Color.DarkSlateGray;
            try
            {
                btn.Image = MyUI.Properties.Resources.Bayaz_menu_32px1;
            }
            catch { }
            panel1.Controls.Add(btn);
            ButonRenklendir();
        }
        public void ButonBagla()
        {
            butonList = new List<MasaUstuButonlar>();
            //butonList.Add(new MasaUstuButonlar() {  Name = "Siparisler", Text = "Sipriş Listesi",  LocationX = 10, LocationY = 10,  SizeX = 150, SizeY = 50 });
            if (!File.Exists(dosyayolu))
            {
                string json = JsonConvert.SerializeObject(butonList, Newtonsoft.Json.Formatting.Indented);
                System.IO.File.WriteAllText(dosyayolu, json);
            }
            var str = System.IO.File.ReadAllText(dosyayolu);
            var dosyalar = JsonConvert.DeserializeObject<List<MasaUstuButonlar>>(str);
            butonList = dosyalar;
        }
        public void ButonlariOlustur()
        {
            panel1.Controls.Clear();
            foreach (var itm in butonList)
            {
                MyButton btn = new MyButton();
                btn.Text = itm.Text;
                btn.Name = itm.Name;
                btn.Tag = itm.Name;
                Point p = new Point(itm.LocationX, itm.LocationY);
                Size s = new Size(itm.SizeX, itm.SizeY);
                btn.MouseDown += button_MouseDown;
                btn.MouseMove += button_MouseMove;
                btn.MouseUp += button_MouseUp;
                btn.Click += button_Click;
                btn.ContextMenuStrip = btnContext;
                btn.Location = p;
                btn.Size = s;
                btn.LookAndFeel.UseDefaultLookAndFeel = false;
                btn.Appearance.BackColor = itm.Renk;
                btn.AppearanceHovered.BackColor = Color.FromArgb(64, 64, 64);
                try
                {
                    btn.Image = MyUI.Properties.Resources.Bayaz_menu_32px1;
                }
                catch { }
                panel1.Controls.Add(btn);
            }
            ButonRenklendir();
        }
        private void AyarKaydet()
        {
            var butonList2 = new List<MasaUstuButonlar>();
            foreach (Control c in panel1.Controls)
            {
                Type ct = c.GetType();
                Type ct2 = typeof(MyButton);
                if (c.GetType() == typeof(MyButton))
                {
                    var btn = (MyButton)c;
                    butonList2.Add(new MasaUstuButonlar()
                    {
                        Name = btn.Tag.ToString(),
                        Text = btn.Text,
                        LocationX = btn.Location.X,
                        LocationY = btn.Location.Y,
                        SizeX = btn.Size.Width,
                        SizeY = btn.Size.Height,
                    Renk = btn.Appearance.BackColor
                    });
                }
            }
            string json = JsonConvert.SerializeObject(butonList2, Newtonsoft.Json.Formatting.Indented);
            System.IO.File.WriteAllText(dosyayolu, json);
        }
        private void renkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (PanelDuzenle == false)
            {
                DuzenleAc();
            }
            ColorDialog Renk = new ColorDialog();
            if (Renk.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            ;
            var renk = Renk.Color;
            // FrmRenkSec f = new FrmRenkSec();
            // f.ShowDialog();
            //if (!f.Secildi) { return; }
            // if (f.Renk==null) { return; }
            foreach (var itm in butonList)
            {
                if (itm.Name == AktifButon)
                {
                    itm.Renk = renk;
                }
            }
            foreach (Control c in panel1.Controls)
            {
                Type ct = c.GetType();
                Type ct2 = typeof(MyButton);
                if (c.GetType() == typeof(MyButton))
                {
                    var btn = (MyButton)c;
                    if (btn.Tag.ToString() == AktifButon)
                    {
                         
                        btn.Appearance.BackColor = renk;
                    }
                }
            }
        }
        private void ButonRenklendir()
        {
            foreach (Control c in panel1.Controls)
            {
                Type ct = c.GetType();
                Type ct2 = typeof(MyButton);
                if (c.GetType() == typeof(MyButton))
                {
                    var btn = (MyButton)c;
                    btn.LookAndFeel.UseDefaultLookAndFeel = false;
                    //btn.Appearance.BackColor = Color.Black; 
                    //btn.AppearanceDisabled.BackColor = Color.Black;
                    //btn.AppearanceHovered.BackColor = Color.FromArgb(64,64,64); 
                    btn.Appearance.ForeColor = Color.White;
                }
            }
            panel1.Refresh();
        }
    }
}