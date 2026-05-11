using My.Core;
using My.Entities.Mikro;
using My.Entities.Models;
using My.Entities.Siparisler;
using MyUI.SiparisModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace MyUI.MyControl
{
    public partial class SiparisPanelControl : UserControl
    {
        public List<MikroStok> StoklarAll;
        public List<MikroStokRenk> StokRenklerAll;
        public List<MikroStokBeden> StokBedenlerAll;
        public SiparisHareket Hareket;
        public List<SiparisHareketDetay> Detaylar;
        /*                                             */
        public ReceteKayitModel Recete;
        private List<MikroStok> Stoklar;
        private List<MikroStokRenk> StokRenkler;
        private List<MikroStokBeden> StokBedenler;
        public bool YeniKayit = false;
        public Action KayitAction;
        public SiparisPanelControl()
        {
            InitializeComponent();
        }
        public void MesajHata(string Hata)
        {
            MessageBox.Show(Hata, "Hata");
        }
        private void SiparisPanelControl_Load(object sender, EventArgs e)
        {
            if (Recete == null)
            {
                return;
            }
            BaglaStoklar();
            BaglaComponent();
            PnlOkBtn.Visible = true;
            PnlOrta.Visible = true;
        }
        public void BaglaStoklar()
        {
            /*********/
            List<string> Stokkodlari = new List<string>();
            string stokkodu = "";
            foreach (var itm in Recete.ReceteStoklar)
            {
                stokkodu = itm.StokKodu;
                if (!string.IsNullOrEmpty(stokkodu))
                {
                    var varmi = Stokkodlari.Find(c => c == stokkodu);
                    if (varmi != null)
                    {
                        continue;
                    }
                    Stokkodlari.Add(stokkodu);
                }
            }
            string kodlar = "";
            foreach (var itm in Stokkodlari)
            {
                if (string.IsNullOrEmpty(kodlar))
                {
                    kodlar += "'" + itm + "'";
                }
                else
                {
                    kodlar += ",'" + itm + "'";
                }
            }
            if (!string.IsNullOrEmpty(kodlar))
            {
                var rsSt = Ortak.DbMikro.Stoklar.GetViewListWhere(" where S.sto_kod in (" + kodlar + ")", Ortak.MikroStokGrubu);
                if (!rsSt.Success)
                {
                    MesajHata(rsSt.Message);
                    return;
                }
                Stoklar = rsSt.Data.ToList();
                BaglaRenkler();
                BaglaBedenler();
            }
        }
        public void BaglaRenkler()
        {
            List<string> Renkkodlari = new List<string>();
            string renkkodu = "";
            foreach (var itm in Stoklar)
            {
                renkkodu = itm.RenkKodu;
                if (!string.IsNullOrEmpty(renkkodu))
                {
                    var varmi = Renkkodlari.Find(c => c == renkkodu);
                    if (varmi != null)
                    {
                        continue;
                    }
                    Renkkodlari.Add(renkkodu);
                }
            }

            string kodlar = "";
            foreach (var itm in Renkkodlari)
            {
                if (string.IsNullOrEmpty(kodlar))
                {
                    kodlar += "'" + itm + "'";
                }
                else
                {
                    kodlar += ",'" + itm + "'";
                }
            }
            if (!string.IsNullOrEmpty(kodlar))
            {
                var rsSt = Ortak.DbMikro.Stoklar.GetRenkListWhere(" where rnk_kodu in (" + kodlar + ")");
                if (!rsSt.Success)
                {
                    MesajHata(rsSt.Message);
                    return;
                }
                StokRenkler = rsSt.Data.ToList();
            }
        }
        public void BaglaBedenler()
        {
            List<string> Bedenkodlari = new List<string>();
            string bedenkodu = "";
            foreach (var itm in Stoklar)
            {
                bedenkodu = itm.BedenKodu;
                if (!string.IsNullOrEmpty(bedenkodu))
                {
                    var varmi = Bedenkodlari.Find(c => c == bedenkodu);
                    if (varmi != null)
                    {
                        continue;
                    }
                    Bedenkodlari.Add(bedenkodu);
                }
            }
            string kodlar = "";
            foreach (var itm in Bedenkodlari)
            {
                if (string.IsNullOrEmpty(kodlar))
                {
                    kodlar += "'" + itm + "'";
                }
                else
                {
                    kodlar += ",'" + itm + "'";
                }
            }
            if (!string.IsNullOrEmpty(kodlar))
            {
                var rsSt = Ortak.DbMikro.Stoklar.GetBedenListWhere(" where bdn_kodu in (" + kodlar + ")");
                if (!rsSt.Success)
                {
                    MesajHata(rsSt.Message);
                    return;
                }
                StokBedenler = rsSt.Data.ToList();
            }
        }
        public void BaglaComponent()
        {
            PnlOrta.Controls.Clear();
            SiparisControl cont;
            int sira = 1;
            foreach (var itm in Recete.ReceteDetaylar.Where(c => c.SiparisdeGosterme == false))
            {
                cont = new SiparisControl();
                if (sira == 1)
                {
                    cont.ilk = true;
                }
                cont.Name = "SipCont" + sira++;
                cont.Cinsi = itm.Cinsi;
                cont.RcAId = itm.RcAId;
                cont.RcDId = itm.Id;
                cont.StokKullan = itm.StokKullan;
                if (YeniKayit)
                {
                    cont.StokKodu = itm.VarsayilanStokKodu;
                    cont.StokAdi = itm.VarsayilanStokAdi;
                    cont.Birim = itm.Birim;
                    cont.Renk = itm.Renk;
                    cont.Beden = itm.Beden;
                    cont.Miktar = itm.Miktar;
                    cont.Aciklama = itm.Aciklama;
                }
                else
                {
                    foreach (var itmDet in Detaylar)
                    {
                        if (itmDet.RcAId == itm.RcAId && itmDet.RcDId == itm.Id)
                        {
                            cont.StokKodu = itmDet.StokKodu;
                            cont.StokAdi = itmDet.StokAdi;
                            cont.Birim = itmDet.Birim;
                            cont.Renk = itmDet.Renk;
                            cont.Beden = itmDet.Beden;
                            cont.Miktar = itmDet.Miktar;
                            cont.Aciklama = itmDet.Aciklama;
                            break;
                        }
                    }
                }
                if (itm.StokKullan)
                {
                    // Turler Stok, Grup , Tümü , Sabit
                    if (itm.StokTuru == "Tümü")
                    {
                        cont.Stoklar = StoklarAll;
                        cont.StokRenkler = StokRenklerAll;
                        cont.StokBedenler = StokBedenlerAll;
                        cont.ReceteRenkBedenler = Recete.ReceteStokRenkBedenler;
                        cont.ReceteStoklar = Recete.ReceteStoklar;
                    }
                    else if (itm.StokTuru == "Grup")
                    {
                        cont.Stoklar = StoklarAll.Where(c => c.AnaGrup == itm.StokAnaGrup).ToList();
                        cont.StokRenkler = StokRenklerAll;
                        cont.StokBedenler = StokBedenlerAll;
                        cont.ReceteRenkBedenler = Recete.ReceteStokRenkBedenler;
                        cont.ReceteStoklar = Recete.ReceteStoklar;
                    }
                    else
                    {
                        List<MikroStok> stk = new List<MikroStok>();
                        foreach (var itms in Recete.ReceteStoklar)
                        {
                            if (itms.RcDId == itm.Id)
                            {
                                var s = Stoklar.Where(c => c.StokKodu == itms.StokKodu);
                                stk.InsertRange(stk.Count, s);
                            }
                        }
                        cont.Stoklar = stk;
                        cont.StokRenkler = StokRenkler;
                        cont.StokBedenler = StokBedenler;
                        cont.ReceteRenkBedenler = Recete.ReceteStokRenkBedenler;
                        cont.ReceteStoklar = Recete.ReceteStoklar;
                    }
                }
                PnlOrta.Controls.Add(cont);
            }
        }
        public bool Aktar()
        {
            foreach (SiparisControl itm in PnlOrta.Controls)
            {
                itm.SetTextToModel();
                if (string.IsNullOrEmpty(itm.StokKodu))
                {
                    MesajHata("stok kodu girilmemiş alan var..");
                    return false;
                }
            }
            Hareket.ReceteKodu = Recete.Recete.ReceteKodu;
            Hareket.ReceteAdi = Recete.Recete.ReceteAdi;
            Hareket.RcAId = Recete.Recete.Id;
            if (YeniKayit)
            {
                SiparisHareketDetay det;
                foreach (SiparisControl itm in PnlOrta.Controls)
                {
                    itm.SetTextToModel();
                    if (itm.ilk)
                    {
                        //Hareket.StokKodu = itm.StokKodu;
                        //Hareket.StokAdi = itm.StokAdi;
                        Hareket.Renk = itm.Renk;
                        Hareket.Beden = itm.Beden;
                        //Hareket.Birim = itm.Birim;

                    }
                    det = new SiparisHareketDetay();
                    det.Id = MyGuid.NewGuid();
                    det.Cinsi = itm.Cinsi;
                    det.StokKodu = itm.StokKodu;
                    det.StokAdi = itm.StokAdi;
                    det.Birim = itm.Birim;
                    det.Renk = itm.Renk;
                    det.Beden = itm.Beden;
                    det.Miktar = itm.Miktar;
                    det.Aciklama = itm.Aciklama;
                    det.SipHId = Hareket.Id;
                    det.RcAId = itm.RcAId;
                    det.RcDId = itm.RcDId;
                    Detaylar.Add(det);
                }
            }
            else
            {
                foreach (SiparisControl itm in PnlOrta.Controls)
                {
                    itm.SetTextToModel();
                    if (itm.ilk)
                    {
                        //Hareket.StokKodu = itm.StokKodu;
                        //Hareket.StokAdi = itm.StokAdi;
                        Hareket.Renk = itm.Renk;
                        Hareket.Beden = itm.Beden;
                        //Hareket.Birim = itm.Birim;

                    }
                    foreach (var det in Detaylar)
                    {
                        if (itm.RcAId == det.RcAId && itm.RcDId == det.RcDId)
                        {
                            det.Cinsi = itm.Cinsi;
                            det.StokKodu = itm.StokKodu;
                            det.StokAdi = itm.StokAdi;
                            det.Birim = itm.Birim;
                            det.Renk = itm.Renk;
                            det.Beden = itm.Beden;
                            det.Miktar = itm.Miktar;
                            det.Aciklama = itm.Aciklama;
                            det.SipHId = Hareket.Id;
                            det.RcAId = itm.RcAId;
                            det.RcDId = itm.RcDId;
                        }
                    }
                }
            }
            return true;
        }
        private void BtnTamam_Click(object sender, EventArgs e)
        {
            if (KayitAction != null)
            {
                if (Aktar())
                {
                    KayitAction.Invoke();
                }
            }
        }
    }
}