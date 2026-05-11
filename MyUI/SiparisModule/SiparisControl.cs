using My.Entities.Mikro;
using My.Entities.ReceteStoklar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace MyUI.SiparisModule
{
    public partial class SiparisControl : UserControl
    {

        public bool ilk = false;
        public Guid? Id { get; set; }
        public string Cinsi { get; set; }
        public string StokKodu { get; set; }
        public string StokAdi { get; set; }
        public string Birim { get; set; }
        public string Renk { get; set; }
        public string Beden { get; set; }
        public double Miktar { get; set; }
        public string Aciklama { get; set; }
        public Guid? SipId { get; set; }
        public Guid? SipHId { get; set; }
        public Guid? RcAId { get; set; }
        public Guid? RcDId { get; set; }
        public bool StokKullan { get; set; }
        public List<MikroStok> Stoklar { get; set; }
        public List<MikroStokRenk> StokRenkler { get; set; }
        public List<MikroStokBeden> StokBedenler { get; set; }
        public List<ReceteStokRenkBeden> ReceteRenkBedenler { get; set; }

        public List<ReceteStok> ReceteStoklar { get; set; }
        private bool AcilisBittimi = false;
        public SiparisControl()
        {
            InitializeComponent();
            this.Load += SiparisControl_Load;

        }
        private void SiparisControl_Load(object sender, EventArgs e)
        {
            SetStoklar();

            SetModelToText();

            AcilisBittimi = true;

        }

        public void SetStoklar()
        {
            if (StokKullan)
            {
                bs.DataSource = Stoklar;
                CmbStokKodu.Properties.DataSource = bs;
                CmbStokKodu.Properties.DisplayMember = "StokKodu";
                CmbStokKodu.Properties.ValueMember = "StokKodu";

                CmbStokAdi.Properties.DataSource = bs;
                CmbStokAdi.Properties.DisplayMember = "StokAdi";
                CmbStokAdi.Properties.ValueMember = "StokAdi";
                CmbStokKodu.Leave += CmbStokKodu_Leave;
                CmbStokAdi.Leave += CmbStokAdi_Leave;
                //CmbStokKodu.Popup += CmbStokKodu_Popup;

            }
        }
        private void CmbStokKodu_Popup(object sender, EventArgs e)
        {
            //CmbStokKodu.Properties.ForceInitialize();
        }

        public void SetModelToText()
        {
            TxtCinsi1.Text = Cinsi;
            TxtAciklama.Text = Aciklama;
            TxtMiktar.Text = Miktar.ToString();
            TxtBirim.Text = Birim;

            CmbStokKodu.Text = StokKodu;
            CmbStokAdi.Text = StokAdi;

            RenkBagla();
            CmbRenk.Text = Renk;
            BedenBagla();
            CmbBeden.Text = Beden;
        }
        public void SetTextToModel()
        {
            Cinsi = TxtCinsi1.Text;
            Aciklama = TxtAciklama.Text;
            Miktar = TxtMiktar.MyDegerDouble();
            Birim = TxtBirim.Text;
            StokKodu = CmbStokKodu.Text;
            StokAdi = CmbStokAdi.Text;
            Renk = CmbRenk.Text;
            Beden = CmbBeden.Text;

            CmbStokKodu.Properties.DropDownRows = bs.Count;
            CmbStokAdi.Properties.DropDownRows = bs.Count;
        }

        private void CmbStokKodu_Leave(object sender, EventArgs e)
        {
            if (AcilisBittimi)
            {
                AcilisBittimi = false;
                var mdl = (MikroStok)CmbStokKodu.GetSelectedDataRow();
                if (mdl != null)
                {
                    CmbStokAdi.Text = mdl.StokAdi.ToString();
                    TxtBirim.Text = mdl.Birim.ToString();
                    RenkBagla();
                    BedenBagla();
                }
                AcilisBittimi = true;
            }
        }
        private void CmbStokAdi_Leave(object sender, EventArgs e)
        {
            if (AcilisBittimi)
            {
                AcilisBittimi = false;
                var mdl = (MikroStok)CmbStokAdi.GetSelectedDataRow();
                if (mdl != null)
                {
                    CmbStokKodu.Text = mdl.StokKodu.ToString();
                    TxtBirim.Text = mdl.Birim.ToString();
                    RenkBagla();
                    BedenBagla();
                }
                AcilisBittimi = true;
            }
        }
        public void RenkBagla()
        {


            if (!StokKullan) return;
            try
            {
                CmbRenk.Properties.Items.Clear();
                var mdl = (MikroStok)CmbStokKodu.GetSelectedDataRow();
                if (mdl == null) return;

                var st = ReceteStoklar.Where(s => s.StokKodu == mdl.StokKodu).FirstOrDefault();
                if (st == null) if (mdl == null) return;
                var re = ReceteRenkBedenler.Where(c => c.RcDId == RcDId && c.RcSTId == st.Id && c.Turu == "Renk");
                if (re.Count() <= 0)
                {
                    var rnk = StokRenkler.Where(r => r.rnk_kodu == mdl.RenkKodu.ToString()).FirstOrDefault();
                    if (rnk != null)
                    {
                        CmbRenk.MyDataBagla(rnk.GetRenkler());
                    }
                }
                else
                {
                    List<string> l = new List<string>();

                    foreach (var itm in re)
                    {
                        l.Add(itm.Kodu);
                    }
                    if (l.Count() > 0)
                    {
                        CmbRenk.MyDataBagla(l);
                    }

                }

            }
            catch
            {
            }
        }
        public void BedenBagla()
        {
            if (!StokKullan) return;
            try
            {
                CmbBeden.Properties.Items.Clear();
                var mdl = (MikroStok)CmbStokKodu.GetSelectedDataRow();
                if (mdl == null) return;

                var st = ReceteStoklar.Where(s => s.StokKodu == mdl.StokKodu).FirstOrDefault();
                if (st == null) if (mdl == null) return;
                var re = ReceteRenkBedenler.Where(c => c.RcDId == RcDId && c.RcSTId == st.Id && c.Turu == "Beden");
                if (re.Count() <= 0)
                {
                    var bdn = StokBedenler.Where(r => r.bdn_kodu == mdl.BedenKodu.ToString()).FirstOrDefault();
                    if (bdn != null)
                    {
                        CmbBeden.MyDataBagla(bdn.GetBedenler());
                    }
                }
                else
                {
                    List<string> l = new List<string>();

                    foreach (var itm in re)
                    {
                        l.Add(itm.Kodu);
                    }
                    if (l.Count() > 0)
                    {
                        CmbBeden.MyDataBagla(l);
                    }

                }


            }
            catch
            {
            }
        }
        public void RenkBagla___()
        {
            if (!StokKullan) return;
            try
            {
                CmbRenk.Properties.Items.Clear();

                var mdl = (MikroStok)CmbStokKodu.GetSelectedDataRow();
                if (mdl == null) return;
                var rnk = StokRenkler.Where(r => r.rnk_kodu == mdl.RenkKodu.ToString()).FirstOrDefault();
                if (rnk != null)
                {
                    CmbRenk.MyDataBagla(rnk.GetRenkler());
                }
            }
            catch
            {
            }
        }
        public void BedenBagla___()
        {
            if (!StokKullan) return;
            try
            {
                CmbBeden.Properties.Items.Clear();
                var mdl = (MikroStok)CmbStokKodu.GetSelectedDataRow();
                if (mdl == null) return;
                var bdn = StokBedenler.Where(r => r.bdn_kodu == mdl.BedenKodu.ToString()).FirstOrDefault();
                if (bdn != null)
                {
                    CmbBeden.MyDataBagla(bdn.GetBedenler());
                }
            }
            catch
            {
            }
        }


    }
}
