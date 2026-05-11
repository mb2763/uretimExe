using My.Entities.ReceteIstasyonlar;
using My.Kontrol.Formlar;
using System;
using System.Collections.Generic;
namespace MyUI.UretimIstasyonModule
{
    public partial class FrmUretimIstasyonCariSec : MyFrmSadeFull
    {
        public List<ReceteIstasyonCari> Cariler;
        public FrmUretimIstasyonCariSec()
        {
            InitializeComponent();
            this.Load += Frm_Load;
            myView1.MyEventDoubleClickEnter += MyView1_MyEventDoubleClickEnter;
        }
        public void Bagla()
        {
            myGrid1.DataSource = Cariler;
            SutunGizle();
            myGrid1.GridYerlesimYukle();
        }
        public void SutunGizle()
        {
            myView1.SutunGizle("Id");
            myView1.SutunGizle("RcAId");
            myView1.SutunGizle("RcOId");
            myView1.SutunGizle("RcIstId");

        }
        private void Frm_Load(object sender, EventArgs e)
        {
            Bagla();
        }
        private void MyView1_MyEventDoubleClickEnter()
        {
            var itm = myView1.MyGetCurrentItem<ReceteIstasyonCari>();
            SecilenRow = itm;
            SecilenKod = itm.CariKodu;
            Secildi = true;
            this.Close();
        }
    }
}