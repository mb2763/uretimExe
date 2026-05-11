using My.Business.Service.UretimAciklamalar;
using My.Core;
using My.Entities.Receteler;
using My.Entities.UretimAciklamalar;
using My.Kontrol.Formlar;
using System.Collections.Generic;
using System.Linq;

namespace MyUI.ReceteModul
{
    public partial class FrmReceteAciklamalar : MyFrmKayit
    {
        IAciklamaKodService kodService = Ortak.DbPro.AciklamaKod;
        IAciklamaDegerService degerService = Ortak.DbPro.AciklamaDeger;
        List<AciklamaDeger> degerler;
        List<AciklamaKod> kodlar;
        public ReceteAna Recete { get; set; }

        public FrmReceteAciklamalar()
        {
            InitializeComponent();
        }

        private void Frm_Load(object sender, System.EventArgs e)
        {
            Bagla();
        }
        private void Bagla()
        {
            BaglaDeger();
        }
        private void BaglaDeger()
        {
            var rs = degerService.SelectListWhere(" where Modul='ReceteAciklama' and EntId='" + Recete.Id + "' Order By Sira,Kodu");
            if (!rs.Success)
            {
                MesajHata(rs.Message); return;
            }
            degerler = rs.Data.ToList();
            if (degerler == null || degerler.Count <= 0)
            {
                BaglaKod();
            }
            GridBagla();
            SutunGizle();
            myGrid1.GridYerlesimYukle();
        }
        private void BaglaKod()
        {
            var rs = kodService.SelectListWhere(" where Modul='ReceteAciklama' Order By Sira,Kodu");
            if (!rs.Success)
            {
                MesajHata(rs.Message); return;
            }
            kodlar = rs.Data.ToList();
            foreach (var itm in kodlar)
            {
                degerler.Add(new AciklamaDeger
                {
                    Id = MyGuid.NewGuid(),
                    Modul = itm.Modul,
                    Kodu = itm.Kodu,
                    Sira = itm.Sira,
                    Deger1 = itm.Deger1,
                    Deger2 = itm.Deger2,
                    Deger3 = itm.Deger3,
                    EntId = Recete.Id,
                    EntKodu = ""
                });
            }
        }

        public void GridBagla()
        {
            myGrid1.DataSource = null;
            bs.DataSource = degerler;
            myGrid1.DataSource = bs;
        }

        public void SutunGizle()
        {
            myView1.SutunGizle("Id");
            myView1.SutunGizle("Modul");
            myView1.SutunGizle("Deger2");
            myView1.SutunGizle("Deger3");
            myView1.SutunGizle("EntKodu");
            myView1.SutunGizle("EntId");
            /**/
            myView1.SutunEditAc("Sira");
            myView1.SutunEditAc("Deger1");
            myView1.SutunReadOnlyKapat("Sira");
            myView1.SutunReadOnlyKapat("Deger1");
        }

        private void BtnKaydet_Click(object sender, System.EventArgs e)
        {
            Kaydet();
        }

        private void BtnSil_Click(object sender, System.EventArgs e)
        {
            Sil();
        }



        private void Kaydet()
        {
            var rsSil = degerService.Delete(c => c.EntId == Recete.Id);
            if (!rsSil.Success)
            {
                MesajHata(rsSil.Message);
                return;
            }

            foreach (var itm in degerler)
            {
                if (itm.Deger1 == null)
                {
                    itm.Deger1 = "";
                }
                if (itm.Deger2 == null)
                {
                    itm.Deger2 = "";
                }
                if (itm.Deger3 == null)
                {
                    itm.Deger3 = "";
                }
            }

            var rs = degerService.InsertOrUpdate(degerler);
            if (!rs.Success)
            {
                MesajHata(rsSil.Message);
                return;
            }
            MesajBilgi("Kayıt Edildi");

        }

        private void Sil()
        {
            if (!MesajSor("Kaydı silmek istiyormusunuz.."))
            {
                return;
            }
            if (Recete.Id.IsNullOrEmpty())
            {
                MesajBilgi("Silinecek Kayıt Seçilmemiş");
            }

            var rs = degerService.Delete(c => c.EntId == Recete.Id);
            if (rs.Success)
            {
                //  KayitEdildi = true; 
            }
            else
            {
                MesajHata(rs.Message);
            }
        }

    }
}
