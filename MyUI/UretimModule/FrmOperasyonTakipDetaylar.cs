using My.Business.Manager;
using My.Entities.Models;
using My.Entities.UretimIstasyonlar;
using My.Kontrol.Formlar;
using MyUI.UretimIstasyonModule;
using MyUI.UretimOperasyonModule;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
namespace MyUI.UretimModule
{
    public partial class FrmOperasyonTakipDetaylar : MyFrmSadeFull
    {
        public string Operasyon { get; set; }
        public string Durumu { get; set; }
        private UretimTakipManagerV2 _mng;
        private List<UretimTakipDetayModelV2> _list;
        public Action Action;
        public bool TumunuBagla = false;
        public bool SipIdDenBagla = false;
        public string SipId = "";

        public FrmOperasyonTakipDetaylar()
        {
            InitializeComponent();
            this.Load += Frm_Load;
            myView1.MyEventDoubleClickEnter += MyView1_MyEventDoubleClickEnter;
        }
        private void Frm_Load(object sender, EventArgs e)
        {
            _mng = new UretimTakipManagerV2(Ortak.DbPro);
            if (TumunuBagla)
            {
                BaglaTumu();
            }
            else
            {
                Bagla();
            }
            ContexMenuyeEkle();
            this.Text = "Üretim Takip Detay  =  Durumu : " + Durumu + " - Operasyon : " + Operasyon + "  ";

            if (Durumu == "Uretimde")
            {
                BtnUretimGirisi.Text = "Uretim Giris";

            }
            else if (Durumu == "Beklemede")
            {
                BtnUretimGirisi.Text = "Istasyona Gonder";
            }
        }
        private void Bagla()
        {
            if (!SipIdDenBagla)
            {
                SipId = "";
            }
            var rs = _mng.GetTakipDetayList(Operasyon, Durumu, SipId);
            if (!rs.Success)
            {
                MesajHata(rs.Message);
                return;
            }
            _list = rs.Data.ToList();
            var query = _list.Select((fruit) => fruit.OperasyonKodu);
            var Grp1 = query.GroupBy(item => item).ToList();
            myGrid1.DataSource = _list;
            SutunGizle();
            myGrid1.GridYerlesimYukle();
        }
        public void SutunGizle()
        {
            myView1.SutunGizle("Id");
        }
        private void BaglaTumu()
        {
            var rs = _mng.GetTakipDetayListAll();
            if (!rs.Success)
            {
                MesajHata(rs.Message);
                return;
            }
            _list = rs.Data.ToList();
            var query = _list.Select((fruit) => fruit.OperasyonKodu);
            var Grp1 = query.GroupBy(item => item).ToList();
            myGrid1.DataSource = _list;
            myView1.SutunGizle("Id");
            myGrid1.GridYerlesimYukle();
        }
        private void MyView1_MyEventDoubleClickEnter()
        {
            var itm = myView1.MyGetCurrentItem<UretimTakipDetayModelV2>();
            if (itm != null)
            {
            }
        }
        public void ContexMenuyeEkle()
        {
            if (Durumu == "Uretimde")
            {
                ToolStripMenuItem fm1 = new ToolStripMenuItem("UretimGiris")
                {
                    BackColor = Color.WhiteSmoke,
                    ForeColor = Color.Black,
                    Text = "Üretim Girişi",
                    TextAlign = ContentAlignment.BottomRight,
                    ToolTipText = "Üretim Girişi"
                };
                fm1.Click += UretimGirisi;
                myGrid1.MyContextMenu.Items.Add(fm1);
                //ToolStripMenuItem Fm21 = new ToolStripMenuItem("Sonraki_Operasyon")
                //{
                //    BackColor = Color.WhiteSmoke,
                //    ForeColor = Color.Black,
                //    Text = "Sonraki Operasyon",
                //    TextAlign = ContentAlignment.BottomRight,
                //    ToolTipText = "Sonraki Operasyon"
                //};
                //Fm1.Click += Sonraki_Operasyon;
                //myGridSf1.MyContextMenu.Items.Add(Fm1);
            }
            else if (Durumu == "Beklemede")
            {
                ToolStripMenuItem fm1 = new ToolStripMenuItem("Istasyona_Gonder")
                {
                    BackColor = Color.WhiteSmoke,
                    ForeColor = Color.Black,
                    Text = "Istasyona Gonder",
                    TextAlign = ContentAlignment.BottomRight,
                    ToolTipText = "Istasyona Gonder"
                };
                fm1.Click += UretimeGonder_Operasyon;
                myGrid1.MyContextMenu.Items.Add(fm1);
            }
            ToolStripMenuItem fm3 = new ToolStripMenuItem("Hareketler")
            {
                BackColor = Color.WhiteSmoke,
                ForeColor = Color.Black,
                Text = "Detay Hareketler",
                TextAlign = ContentAlignment.BottomRight,
                ToolTipText = "Hareketler"
            };
            fm3.Click += Detaylar;
            myGrid1.MyContextMenu.Items.Add(fm3);
        }

        private void UretimeGonder_Operasyon(object sender, EventArgs e)
        {
            var itm = myView1.MyGetCurrentItem<UretimTakipDetayModelV2>();
            if (itm != null)
            {
                if (itm.Id.IsNullOrEmpty())
                {
                    MesajBilgi("Operasyon Baslatilmamis İşlem Yapılamaz Bir önceki operasyondan işlem yapınız");
                    return;
                }

                FrmUretimIstasyonED f = new FrmUretimIstasyonED
                {
                    Action = Action,
                    OperasyonTuru = OperasyonTuruEnum.IstasyonEkle,
                    OprId = itm.Id
                };
                f.ShowDialog();
                this.Close();
            }
        }

        private void UretimGirisi(object sender, EventArgs e)
        {
            var itm = myView1.MyGetCurrentItem<UretimTakipDetayModelV2>();
            if (itm != null)
            {
                if (itm.Id.IsNullOrEmpty())
                {
                    MesajBilgi("Operasyon Baslatilmamis İşlem Yapılamaz Bir önceki operasyondan işlem yapınız");
                    return;
                }

                FrmUretimIstasyonHareketSec f1 = new FrmUretimIstasyonHareketSec { SecimIcinAcildi = true, UrOHDId = itm.Id };
                f1.ShowDialog();
                if (f1.Secildi)
                {
                    FrmUretimIstasyonUretimGir f2 = new FrmUretimIstasyonUretimGir
                    {
                        MdlIst = f1.SecilenRow as UretimIstasyon
                    };
                    f2.ShowDialog();
                }
                this.Close();
            }
        }

        private void Detaylar(object sender, EventArgs e)
        {
            var itm = myView1.MyGetCurrentItem<UretimTakipDetayModelV2>();
            if (itm != null)
            {
                if (itm.Id.IsNullOrEmpty())
                {
                    MesajBilgi("Operasyon Baslatilmamis İşlem Yapılamaz Bir önceki operasyondan işlem yapınız");
                    return;
                }

                FrmUretimOperasyonHareketDetayList f = new FrmUretimOperasyonHareketDetayList
                {
                    DetayGoster = true,
                    DetayId = itm.Id
                };
                f.ShowDialog();
                this.Close();
            }
        }

        private void BtnUretimGirisi_Click(object sender, EventArgs e)
        {
            if (Durumu == "Uretimde")
            {
                var itm = myView1.MyGetCurrentItem<UretimTakipDetayModelV2>();
                if (itm != null)
                {
                    if (itm.Id.IsNullOrEmpty())
                    {
                        MesajBilgi("Operasyon Baslatilmamis İşlem Yapılamaz Bir önceki operasyondan işlem yapınız");
                        return;
                    }

                    FrmUretimIstasyonHareketSec f1 = new FrmUretimIstasyonHareketSec { SecimIcinAcildi = true, UrOHDId = itm.Id };
                    f1.ShowDialog();
                    if (f1.Secildi)
                    {
                        FrmUretimIstasyonUretimGir f2 = new FrmUretimIstasyonUretimGir
                        {
                            MdlIst = f1.SecilenRow as UretimIstasyon
                        };
                        f2.ShowDialog();
                    }
                    this.Close();
                }

            }
            else if (Durumu == "Beklemede")
            {
                var itm = myView1.MyGetCurrentItem<UretimTakipDetayModelV2>();
                if (itm != null)
                {
                    if (itm.Id.IsNullOrEmpty())
                    {
                        MesajBilgi("Operasyon Baslatilmamis İşlem Yapılamaz Bir önceki operasyondan işlem yapınız");
                        return;
                    }

                    FrmUretimIstasyonED f = new FrmUretimIstasyonED
                    {
                        Action = Action,
                        OperasyonTuru = OperasyonTuruEnum.IstasyonEkle,
                        OprId = itm.Id
                    };
                    f.ShowDialog();
                    this.Close();
                }
            }

        }
    }
}