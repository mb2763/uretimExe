using My.Entities.Models;
using My.Entities.UretimIstasyonlar;
using MyUI.MyControl;
using MyUI.UretimIstasyonModule;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
namespace MyUI.UretimOperasyonModule
{
    public partial class OperasyonCardControlV2 : UserControl
    {
        public string OperasyonKodu { get; set; } = "";
        public string OperasyonAdi { get; set; } = "";
        public List<UretimTakipModelV2> Hareketler { get; set; }
        public Action Action;
        public OperasyonCardControlV2()
        {
            InitializeComponent();
        }
        private void Card_Load(object sender, EventArgs e)
        {
            Bagla();
        }
        public void Bagla()
        {
            LblOperasyon.Text = OperasyonKodu + " - " + OperasyonAdi;
            PnlBekleyen.Controls.Clear();
            PnlUretimde.Controls.Clear();
            if (Hareketler == null || Hareketler.Count < 1)
            {
                return;
            }
            foreach (var itm in Hareketler)
            {

                if (itm.PlanlananMiktar - itm.IslemdekiMiktar > 0)
                {
                    var tur = itm.Clone();
                    tur.Turu = "Beklemede";
                    var card = new OperasyonCardRowControlV2()
                    {
                        Id = OperasyonCardManagerV2.RowId++,
                        Model = tur,
                        Action = BekleyenDetaylarAc,
                    };
                    PnlBekleyen.Controls.Add(card);
                }
                if (itm.IslemdekiMiktar > 0)
                {
                    var tur = itm.Clone();
                    tur.Turu = "Uretimde";
                    var card = new OperasyonCardRowControlV2()
                    {
                        Id = OperasyonCardManagerV2.RowId++,
                        Model = tur,
                        Action = UretimdeDetaylarAc,
                    };
                    PnlUretimde.Controls.Add(card);
                }
            }
        }
        private void DetaylarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BekleyenDetaylarAc();
        }
        private void ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            UretimdeDetaylarAc();
        }
        private void BekleyenDetaylarAc()
        {
            if (string.IsNullOrEmpty(OperasyonKodu)) return;
            Ortak.UretimTakipBekleyenAcV2(OperasyonKodu);
        }
        private void UretimdeDetaylarAc()
        {
            if (string.IsNullOrEmpty(OperasyonKodu)) return;
            Ortak.UretimTakipUretimdeAcV2(OperasyonKodu, Action);
        }
        private void UretimlerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            const string fra = "IstasyonDetaylar";
            FormAc.FormSec(fra);
        }
        private void IstasyonGirisleriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            const string fra = "OprerasyonDetaylar";
            FormAc.FormSec(fra);
        }

        private void operasyonDetaylariToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmUretimOperasyonHareketDetayList f = new FrmUretimOperasyonHareketDetayList();
            f.ShowDialog();
        }

        private void operasyonDetaylariToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            FrmUretimOperasyonHareketDetayList f = new FrmUretimOperasyonHareketDetayList();
            f.ShowDialog();
        }

        private void uretimGirisiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // UrO.OperasyonKodu,UrO.OperasyonAdi

            FrmUretimIstasyonHareketSec f1 = new FrmUretimIstasyonHareketSec
            {
                SecimIcinAcildi = true,
                OperasyondanBagla = true,
                OperasyonKodu = OperasyonKodu,
                OperasyonAdi = OperasyonAdi
            };
            f1.ShowDialog();
            if (f1.Secildi)
            {
                FrmUretimIstasyonUretimGir f2 = new FrmUretimIstasyonUretimGir
                {
                    MdlIst = f1.SecilenRow as UretimIstasyon
                };
                f2.ShowDialog();
            }
        }
    }
}