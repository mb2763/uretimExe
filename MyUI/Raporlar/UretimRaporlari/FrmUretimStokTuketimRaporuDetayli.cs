using DevExpress.XtraEditors.Repository;
using My.Business.Service.IstasyonTakipler;
using My.Business.Service.Templer;
using My.Kontrol.Formlar;
using System;
using System.Linq;
using System.Windows.Forms;

namespace MyUI.Raporlar.UretimRaporlari {
    public partial class FrmUretimStokTuketimRaporuDetayli : MyFrmListe {

        IIstasyonTakipStokHareketDetayService _srv = Ortak.DbPro.IstasyonTakipStokHareketDetay;
        ITempMikroStokService _TmpMikroStok = Ortak.DbPro.TempMikroStok;
        public FrmUretimStokTuketimRaporuDetayli() {
            InitializeComponent();
            EventlerBagla();
        }

        private void EventlerBagla() {
            this.Load += Frm_Load;
            myView1.MyEventDoubleClickEnter += MyView1_MyEventDoubleClickEnter;
            myView1.FocusedRowChanged += MyView1_FocusedRowChanged;
            TempGuncelle();
        }
        private void Frm_Load(object sender, EventArgs e) {
            BaglaKategoriList();
            BaglaKaliteKontrolList();
            BaglaReyonList();
        }
        private void TempGuncelle() {
            Cursor.Current = Cursors.WaitCursor;

            var rs = _TmpMikroStok.MikroStokKategoriGuncelle(Ortak.DbMikro.Settings.Database);
            if (!rs.Success) {
                Cursor.Current = Cursors.Default;
                MesajHata(rs.Message);
                return;
            }
            var rs1 = _TmpMikroStok.MikroStokGuncelle(Ortak.DbMikro.Settings.Database);
            if (!rs1.Success) {
                Cursor.Current = Cursors.Default;
                MesajHata(rs1.Message);
                return;
            }
            var rs3 = _srv.DetaylarGuncelleToplu();
            if (!rs3.Success) {
                Cursor.Current = Cursors.Default;
                MesajHata(rs3.Message);
                return;
            }
            Cursor.Current = Cursors.Default;
        }
        private void Bagla() {
            Cursor.Current = Cursors.WaitCursor;
            string sor = SorguAyarla() + SorguAyarlaTrh();   // if (!string.IsNullOrEmpty(sor)) {  sor = "where  1 = 1 " + sor;  }
            string sor2 = SorguAyarla2() + SorguAyarlaTrh();  
            var rs = _srv.GetListViewInKategoriDetayli(sor, sor2);
            if (!rs.Success) {
                Cursor.Current = Cursors.Default;
                MesajHata(rs.Message);
                return;
            }
            lblKayitSayisi.Caption = rs.Data?.Count().ToString();
            myGrid1.DataSource = rs.Data;
            myView1.SutunGizle("Id");
            
            //  myView1.SutunFormat(nameof(IstasyonRaporHareketModel.Tarih), FormatType.DateTime, "dd.MM.yyyy HH:mm");
            myGrid1.GridYerlesimYukle();
            Cursor.Current = Cursors.Default;
        }
        private string SorguAyarla() {
            string sor = "";
            string t1 = CmbKategori.Text.Trim();
            t1 = t1.Replace("; ", ";");
            if (!string.IsNullOrEmpty(t1)) {
                var lisAra = t1.Split(';');
                var arananlar = "";
                foreach (var itm in lisAra) {
                    if (!string.IsNullOrEmpty(itm)) {
                        if (string.IsNullOrEmpty(arananlar)) {
                            arananlar = "'" + itm + "'";
                        }
                        else {
                            arananlar = arananlar + ",'" + itm + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(arananlar)) {
                    sor += $" AND TMPS.KategoriAdi in ({arananlar}) ";
                }
            }
            t1 = CmbKaliteKontrol.Text.Trim();
            t1 = t1.Replace("; ", ";");
            if (!string.IsNullOrEmpty(t1)) {
                var lisAra = t1.Split(';');
                var arananlar = "";
                foreach (var itm in lisAra) {
                    if (!string.IsNullOrEmpty(itm)) {
                        if (string.IsNullOrEmpty(arananlar)) {
                            arananlar = "'" + itm + "'";
                        }
                        else {
                            arananlar = arananlar + ",'" + itm + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(arananlar)) {
                    sor += $" AND TMPS.KaliteKontrolAdi in ({arananlar}) ";
                }
            }

            t1 = CmbReyon.Text.Trim();
            t1 = t1.Replace("; ", ";");
            if (!string.IsNullOrEmpty(t1)) {
                var lisAra = t1.Split(';');
                var arananlar = "";
                foreach (var itm in lisAra) {
                    if (!string.IsNullOrEmpty(itm)) {
                        if (string.IsNullOrEmpty(arananlar)) {
                            arananlar = "'" + itm + "'";
                        } else {
                            arananlar = arananlar + ",'" + itm + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(arananlar)) {
                    sor += $" AND TMPS.ReyonAdi in ({arananlar}) ";
                }
            }

            t1 = TxtAra.Text.Trim();
            if (!string.IsNullOrEmpty(t1)) {
                sor += $@" AND ( ITSHD.IsEmriKodu LIKE('%{t1}%') OR ITSHD.UrtStokKodu LIKE('%{t1}%') OR ITSHD.UrtStokAdi LIKE('%{t1}%')  
                           OR ITSHD.StokKodu LIKE('%{t1}%') OR ITSHD.StokAdi LIKE('%{t1}%')    )";
            }
            return sor;
        }

        private string SorguAyarla2() {
            string sor = "";
            string t1 = CmbKategori.Text.Trim();
            t1 = t1.Replace("; ", ";");
            if (!string.IsNullOrEmpty(t1)) {
                var lisAra = t1.Split(';');
                var arananlar = "";
                foreach (var itm in lisAra) {
                    if (!string.IsNullOrEmpty(itm)) {
                        if (string.IsNullOrEmpty(arananlar)) {
                            arananlar = "'" + itm + "'";
                        }
                        else {
                            arananlar = arananlar + ",'" + itm + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(arananlar)) {
                    sor += $" AND TMPS.KategoriAdi in ({arananlar}) ";
                }
            }
            t1 = CmbKaliteKontrol.Text.Trim();
            t1 = t1.Replace("; ", ";");
            if (!string.IsNullOrEmpty(t1)) {
                var lisAra = t1.Split(';');
                var arananlar = "";
                foreach (var itm in lisAra) {
                    if (!string.IsNullOrEmpty(itm)) {
                        if (string.IsNullOrEmpty(arananlar)) {
                            arananlar = "'" + itm + "'";
                        }
                        else {
                            arananlar = arananlar + ",'" + itm + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(arananlar)) {
                    sor += $" AND TMPS.KaliteKontrolAdi in ({arananlar}) ";
                }
            }

            t1 = CmbReyon.Text.Trim();
            t1 = t1.Replace("; ", ";");
            if (!string.IsNullOrEmpty(t1)) {
                var lisAra = t1.Split(';');
                var arananlar = "";
                foreach (var itm in lisAra) {
                    if (!string.IsNullOrEmpty(itm)) {
                        if (string.IsNullOrEmpty(arananlar)) {
                            arananlar = "'" + itm + "'";
                        } else {
                            arananlar = arananlar + ",'" + itm + "'";
                        }
                    }
                }
                if (!string.IsNullOrEmpty(arananlar)) {
                    sor += $" AND TMPS.ReyonAdi in ({arananlar}) ";
                }
            }

            t1 = TxtAra.Text.Trim();
            if (!string.IsNullOrEmpty(t1)) {
                sor += $@" AND ( ITH.SiparisKodu LIKE('%{t1}%') OR ITH.ReceteKodu LIKE('%{t1}%') OR ITH.ReceteAdi LIKE('%{t1}%')  
                           OR ITHD.StokKodu LIKE('%{t1}%') OR ITHD.StokAdi LIKE('%{t1}%')    )";
            }
            return sor;
        }
        private string SorguAyarlaTrh() {
            string sor = "";
            var saat1 = TxtSaat1.Text.ToString();
            var saat2 = TxtSaat2.Text.ToString();
            var t1 = TxtTarihi1.Text.Trim();
            if (!string.IsNullOrEmpty(t1)) {
                t1 = TarihAyarla(Convert.ToDateTime(TxtTarihi1.EditValue));
                t1 = t1 + " " + saat1;
                if (!string.IsNullOrEmpty(t1)) sor += $"  AND  CAST (coalesce( ITHD.Tarih,'1901-01-01') AS datetime ) >=  CAST('{t1}'  AS datetime ) ";
            }
            t1 = TxtTarihi2.Text.Trim();
            if (!string.IsNullOrEmpty(t1)) {
                t1 = TarihAyarla(Convert.ToDateTime(TxtTarihi2.EditValue));
                t1 = t1 + " " + saat2;
                if (!string.IsNullOrEmpty(t1)) sor += $"  AND  CAST (coalesce( ITHD.Tarih,'1901-01-01') AS datetime ) <=  CAST('{t1}'  AS datetime ) ";
            }
            return sor;
        }
        private string TarihAyarla(DateTime? T1) {
            if (T1 == null) {
                return "";
            }
            string FX = T1?.Year + "-" + T1?.Month.ToString().PadLeft(2, '0') + "-" +  T1?.Day.ToString().PadLeft(2, '0');
            return FX;
        }
        public void BaglaKategoriList() { 
            var rs = _TmpMikroStok.GetStokKategoriListStokKategori();
            if (!rs.Success) {
                MesajHata(rs.Message);
                return;
            }
            var dt = rs.Data;
            CmbKategori.Properties.SeparatorChar = ';';
            CmbKategori.Properties.EditValueType = EditValueTypeCollection.List;
            foreach (var itm in dt) {
                CmbKategori.Properties.Items.Add(itm.KategoriKodu, itm.KategoriAdi, CheckState.Unchecked, true);
            }
        }
        public void BaglaKaliteKontrolList() { 
            var rs = _TmpMikroStok.GetStokKategoriListKaliteKontrol();
            if (!rs.Success) {
                MesajHata(rs.Message);
                return;
            }
            var dt = rs.Data;
            CmbKaliteKontrol.Properties.SeparatorChar = ';';
            CmbKaliteKontrol.Properties.EditValueType = EditValueTypeCollection.List;
            foreach (var itm in dt) {
                CmbKaliteKontrol.Properties.Items.Add(itm.KategoriKodu, itm.KategoriAdi, CheckState.Unchecked, true);
            }
        }
        public void BaglaReyonList() { 
            var rs = _TmpMikroStok.GetStokReyonList();
            if (!rs.Success) {
                MesajHata(rs.Message);
                return;
            }
            var dt = rs.Data;
            CmbReyon.Properties.SeparatorChar = ';';
            CmbReyon.Properties.EditValueType = EditValueTypeCollection.List;
            foreach (var itm in dt) {
                CmbReyon.Properties.Items.Add(itm.KategoriKodu, itm.KategoriAdi, CheckState.Unchecked, true);
            }
        }
        private void MyView1_MyEventDoubleClickEnter() {
            //    var _rwid = myView1.FocusedRowHandle;
            //    var itm = myView1.MyGetCurrentItem<IstasyonTakipHareket>();
            //    if (itm == null) return;
            //    if (SecimIcinAcildi) {
            //    SecilenKod = itm.ReceteKodu;
            //    SecilenRow = itm;
            //    SecilenId = itm.Id.ToString();
            //    Secildi = true;
            //    this.Close();
            //    return;
        }
        private void MyView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e) {
            // var itm = myView1.MyGetCurrentItem<IstasyonTakipHareket>();
            // if (itm != null) {
            // }
            // else {
            // }
        }
        private void BtnAra_Click(object sender, EventArgs e) {
            Bagla();
        }
    }
}

