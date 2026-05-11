using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Columns;
using My.Entities.Mikro;
using My.Entities.Models;
using My.Entities.UretimIstasyonlar;
using MyUI.MikroModule;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace MyUI.UretimOperasyonModule
{

    public partial class OperasyonIstasyonControlV2 : UserControl
    {

        public Guid? IdGuid = Guid.Empty;
        public Guid? RcOId = Guid.Empty;
        public string Operasyon = "";
        public string ReceteKodu = "";
        public string ReceteAdi = "";
        public double Miktar = 0;
        public List<string> istasyonlar;
        public ReceteOperasyonKayitModel oprKayMdl;
        public List<UretimIstasyon> IstasyonHareketler;
        public EventHandler<object> MyIstasyonEkleEvent;
        public EventHandler<object> MyIstasyonSilEvent;
        public OperasyonIstasyonControlV2()
        {
            InitializeComponent();
            this.Load += OperasyonIstasyonControl_Load;
            myView1.ShownEditor += MyView1_ShownEditor;
        }
        private void OperasyonIstasyonControl_Load(object sender, EventArgs e)
        {
        }
        public void Bagla()
        {
            LblReceteKodu.Text = ReceteKodu;
            LblReceteAdi.Text = ReceteAdi;
            LblOperasyonKodu.Text = Operasyon;
            LblMiktar.Text = Miktar.ToString();
            GridBagla();
        }

        public void GridBagla()
        {
            myGrid1.DataSource = IstasyonHareketler;
            myView1.Columns["FasonCariKodu"].ColumnEdit = ColBtnCariKodu;
            myView1.Columns["FasonCariUnvani"].ColumnEdit = ColBtnCariUnvani;
            //    FasonCariKodu    FasonCariUnvani 
            try
            {
                myView1.Columns["PlanlananMiktar"].OptionsColumn.AllowEdit = true;
                myView1.Columns["Fason"].OptionsColumn.AllowEdit = true;
                myView1.Columns["BaslangicTarihi"].OptionsColumn.AllowEdit = true;
                myView1.Columns["BitisTarihi"].OptionsColumn.AllowEdit = true;
                (myView1.Columns["BaslangicTarihi"] as GridColumn).DisplayFormat.FormatType = FormatType.Custom;
                (myView1.Columns["BaslangicTarihi"] as GridColumn).DisplayFormat.FormatString = "dd.MM.yyyy HH:mm";
                (myView1.Columns["BitisTarihi"] as GridColumn).DisplayFormat.FormatType = FormatType.Custom;
                (myView1.Columns["BitisTarihi"] as GridColumn).DisplayFormat.FormatString = "dd.MM.yyyy HH:mm";
            }
            catch { }
            try
            {
                //    List<CellButton> buttons = new List<CellButton>();
                //    buttons.Add(new CellButton() { Text = "Sec", Size = new Size(33, 21), Style = new ButtonCellStyleInfo() { Padding = new Padding(2), BackColor = Color.CadetBlue, TextColor = Color.White } });  //new ButtonCellStyleInfo() { Padding = new Padding(0) } });
                //    buttons.Add(new CellButton() { Text = "Sil", Size = new Size(33, 21), Style = new ButtonCellStyleInfo() { Padding = new Padding(2), BackColor = Color.Brown, TextColor = Color.White } });
                //    myGrid1.Columns.Add(new GridButtonColumn() { MappingName = "FasonSec", HeaderText = "Fason", Buttons = buttons, Orientation = Orientation.Horizontal });
            }
            catch { }
            SutunGizle();
            myGrid1.GridYerlesimYukle();
            for (int i = 0; i < myView1.Columns.Count; i++)
            {
                myView1.Columns[i].OptionsFilter.AllowFilter = false;
            }

            try
            {
                myView1.Columns["PlanlananMiktar"].AppearanceHeader.BackColor = Color.Green;
                myView1.Columns["Fason"].AppearanceHeader.BackColor = Color.Green;
                myView1.Columns["BaslangicTarihi"].AppearanceHeader.BackColor = Color.Green;
                myView1.Columns["BitisTarihi"].AppearanceHeader.BackColor = Color.Green;
            }
            catch
            {

            }
        }
        private void MyView1_ShownEditor(object sender, EventArgs e)
        {
            try
            {
                TextEdit edit = myView1.ActiveEditor as TextEdit;
                if (edit == null) return;
                if (edit.Text.Length > 0) edit.SelectAll();
            }
            catch
            {

            }

        }
        public void SutunGizle()
        {
            myView1.SutunGizle("Id");
            myView1.SutunGizle("UrId");
            myView1.SutunGizle("UrOId");
            myView1.SutunGizle("UrOHId");
            myView1.SutunGizle("UrOHDId");
            myView1.SutunGizle("RcAId");
            myView1.SutunGizle("RcOId");
            myView1.SutunGizle("RcIstId");
            myView1.SutunGizle("SipId");

        }

        private void BtnCariEkle_Click(object sender, EventArgs e)
        {
            var itm = myView1.MyGetCurrentItem<UretimIstasyon>();
            if (itm == null) return;
            FrmMikroCariListesi f = new FrmMikroCariListesi();
            f.SecimIcinAcildi = true;
            f.ShowDialog();
            if (f.Secildi)
            {
                var cr = ((MikroCari)f.SecilenRow);
                itm.FasonCariKodu = cr.CariKodu;
                itm.FasonCariUnvani = cr.CariUnvani1;
                itm.Fason = true;
            }
        }

        private void BtnCariTemizle_Click(object sender, EventArgs e)
        {
            var itm = myView1.MyGetCurrentItem<UretimIstasyon>();
            if (itm == null) return;
            itm.FasonCariKodu = "";
            itm.FasonCariUnvani = "";
            itm.Fason = false;
        }

        private void BtnIstasyonEkle_Click(object sender, EventArgs e)
        {
            MyIstasyonEkleEvent?.Invoke(sender, e);
        }

        private void BtnIstasyonSil_Click(object sender, EventArgs e)
        {
            var itm = myView1.MyGetCurrentItem<UretimIstasyon>();
            if (itm == null) return;
            if (itm.UretimMiktari > 0)
            {
                MessageBox.Show("Üretim Girilmiş Kayıt Silinemez.");
                return;
            }
            MyIstasyonSilEvent?.Invoke(sender, itm);
        }
    }
}



//public class MyEventArgs : EventArgs {
//    private object _value;

//    public MyEventArgs(object value)
//        : base() {
//        _value = value;
//    }

//    public object Value {
//        get { return _value; }
//    }
//}