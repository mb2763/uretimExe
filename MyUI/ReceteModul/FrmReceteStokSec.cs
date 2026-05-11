using My.Business.Service.MikroModul;
using My.Business.Service.ReceteStoklar;
using My.Entities.Mikro;
using My.Entities.ReceteStoklar;
using My.Kontrol.Formlar;
using System.Collections.Generic;
using System.Linq;

namespace MyUI.ReceteModul
{
    public partial class FrmReceteStokSec : MyFrmSade
    {
        IMikroStokService mikroStokService = Ortak.DbMikro.Stoklar;
        IReceteStokRenkBedenService renkBedenService = Ortak.DbPro.ReceteStokRenkBeden;
        public MikroStok MikroStok { get; set; }
        public ReceteStok ReceteStok { get; set; }
        List<string> Bedenler { get; set; }
        List<ReceteStokRenkBeden> RenkBedenList { get; set; }
        public List<ReceteStokRenkBeden> RenkBedenListSecilen { get; set; }

        List<ReceteStokRenkBeden> BedenList { get; set; }


        public bool Edit = false;
        public FrmReceteStokSec()
        {
            InitializeComponent();
        }
        private void FrmReceteStokSec_Load(object sender, System.EventArgs e)
        {

            if (Edit)
            {

                BedenBagla(ReceteStok.StokKodu);
                TxtStokKodu.Text = ReceteStok.StokKodu;
                TxtStokAdi.Text = ReceteStok.StokAdi;
                CmbRenk.Text = ReceteStok.Renk;
                CmbBeden.Text = ReceteStok.Beden;
                TxtEbat.Text = ReceteStok.Ebat;
                TxtGram.Text = ReceteStok.Gram;
                TxtOlcu.Text = ReceteStok.Olcu;
                RenkBedenListBaglaEdit();
            }
            else
            {
                if (MikroStok != null)
                {
                    BedenBagla(MikroStok.StokKodu);
                    TxtStokKodu.Text = MikroStok.StokKodu;
                    TxtStokAdi.Text = MikroStok.StokAdi;
                    RenkBedenListBagla();
                }
            }

        }

        public void BedenBagla(string stokkodu)
        {
            try
            {
                var rs = mikroStokService.GetBedenByStokKodu(stokkodu);
                if (!rs.Success)
                {
                    MesajHata(rs.Message);
                    return;
                }
                var bdn = rs.Data;
                if (bdn != null)
                {
                    Bedenler = bdn.GetBedenler();
                    CmbBeden.Properties.Items.Clear();
                    CmbBeden.MyDataBagla(Bedenler);
                }
            }
            catch
            {
            }
        }

        public void RenkBedenListBagla()
        {
            if (BedenList == null) BedenList = new List<ReceteStokRenkBeden>();
            if (RenkBedenListSecilen == null) RenkBedenListSecilen = new List<ReceteStokRenkBeden>();
            if (RenkBedenList == null) RenkBedenList = new List<ReceteStokRenkBeden>();
            if (Bedenler == null) Bedenler = new List<string>();
            try
            {
                var rs = renkBedenService.SelectListWhere($" where RcAId='{ReceteStok.RcAId}' and  RcDId='{ReceteStok.RcDId}' and RcSTId='{ReceteStok.Id}' ");
                if (!rs.Success)
                {
                    MesajHata(rs.Message);
                    return;
                }
                RenkBedenList = rs.Data.ToList();
                foreach (var itm in RenkBedenList)
                {
                    itm.Sec = true;
                    if (itm.Turu == "Beden")
                    {
                        BedenList.Add(itm.Clone());
                    }
                }

                foreach (var itm in Bedenler)
                {
                    if (string.IsNullOrEmpty(itm))
                    {
                        continue;
                    }
                    var r = BedenList?.Find(c => c.Kodu == itm);
                    if (r == null)
                    {
                        BedenList.Add(ReceteStokRenkBeden.GetBeden(ReceteStok, itm));
                    }
                }
                myGrid2.DataSource = BedenList;
                myView2.SutunGizle("Id");
                myView2.SutunGizle("Turu");
                myView2.SutunGizle("RcAId");
                myView2.SutunGizle("RcDId");
                myView2.SutunGizle("RcSTId");
                myView2.SutunEditAc("Sec");
                myView2.SutunEditAc("Miktar");
            }
            catch
            {
            }
        }

        public void RenkBedenListBaglaEdit()
        {

            if (BedenList == null) BedenList = new List<ReceteStokRenkBeden>();
            if (RenkBedenListSecilen == null) RenkBedenListSecilen = new List<ReceteStokRenkBeden>();
            if (RenkBedenList == null) RenkBedenList = new List<ReceteStokRenkBeden>();
            if (Bedenler == null) Bedenler = new List<string>();

            try
            {

                foreach (var itm in RenkBedenListSecilen)
                {
                    itm.Sec = true;
                    RenkBedenList.Add(itm.Clone());
                }
                foreach (var itm in RenkBedenList)
                {
                    itm.Sec = true;
                    if (itm.Turu == "Beden")
                    {
                        BedenList.Add(itm.Clone());
                    }
                }
                foreach (var itm in Bedenler)
                {
                    if (string.IsNullOrEmpty(itm))
                    {
                        continue;
                    }
                    var r = BedenList?.Find(c => c.Kodu == itm);
                    if (r == null)
                    {
                        BedenList.Add(ReceteStokRenkBeden.GetBeden(ReceteStok, itm));
                    }
                }
                myGrid2.DataSource = BedenList;
                myView2.SutunGizle("Id");
                myView2.SutunGizle("Turu");
                myView2.SutunGizle("RcAId");
                myView2.SutunGizle("RcDId");
                myView2.SutunGizle("RcSTId");
                myView2.SutunGizle("StokKodu");
                myView2.SutunEditAc("Sec");
                myView2.SutunEditAc("Miktar");
            }
            catch
            {
            }
        }
        private void BtnKaydet_Click(object sender, System.EventArgs e)
        {
            RenkBedenListSecilen = new List<ReceteStokRenkBeden>();
            ReceteStok.StokKodu = TxtStokKodu.Text;
            ReceteStok.StokAdi = TxtStokAdi.Text;
            ReceteStok.Renk = CmbRenk.Text;
            ReceteStok.Beden = CmbBeden.Text;
            ReceteStok.Ebat = TxtEbat.Text;
            ReceteStok.Gram = TxtGram.Text;
            ReceteStok.Olcu = TxtOlcu.Text;
            RenkBedenListSecilen.InsertRange(0, BedenList.Where(c => c.Sec == true));
            foreach (var itm in RenkBedenListSecilen)
            {
                itm.StokKodu = TxtStokKodu.Text;
            }

            Secildi = true;
            this.Close();
        }
    }
}
