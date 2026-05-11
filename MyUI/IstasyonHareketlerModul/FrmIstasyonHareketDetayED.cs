using My.Business.Service.IstasyonTakipler;
using My.Core.Result;
using My.Core;
using My.DataAccess.IstasyonTakipler;
using My.Entities.IstasyonTakipler;
using My.Entities.UretimIstasyonlar;
using My.Kontrol.Formlar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using My.Business.Manager;

namespace MyUI.IstasyonHareketlerModul {
    public partial class FrmIstasyonHareketDetayED : MyFrmKayit {
        IstasyonHareketManager mng;
        public IstasyonTakipHareketDetay Model { get; set; }


        public FrmIstasyonHareketDetayED() {
            InitializeComponent();
        }
        private void FrmIstasyonHareketDetayED_Load(object sender, EventArgs e) {
            mng = new IstasyonHareketManager(Ortak.DbPro, Ortak.DbMikro);
            AktarTextlere();
        }

        private void AktarTextlere() {
            TxtStokKodu.Text = Model.StokKodu.ToString();
            TxtStokAdi.Text = Model.StokAdi.ToString();
            TxtReceteKodu.Text = Model.ReceteKodu.ToString();
            TxtReceteAdi.Text = Model.ReceteAdi.ToString();
            TxtOperasyonKodu.Text = Model.OperasyonKodu.ToString();
            TxtOperasyonAdi.Text = Model.OperasyonAdi.ToString(); 

            TxtMiktar.Text = Model.Miktar.ToString();
            TxtFireMiktar.Text = Model.FireMiktar.ToString();
            TxtIptalMiktar.Text = Model.IptalMiktar.ToString();
        }

        private void AktarRowa() {
            Model.Miktar = TxtMiktar.MyDegerDouble();
            Model.FireMiktar = TxtFireMiktar.MyDegerDouble();
            Model.IptalMiktar = TxtIptalMiktar.MyDegerDouble(); 
        }
        private void Kaydet() {
            AktarRowa();
            var rs = mng.SaveIstasyonTakipHareketDetayUpdate(Model);
            if (!rs.Success) {
                MesajHata(rs.Message);
                return;
            }
            this.Close(); 
        }

        private void BtnKaydet_Click(object sender, EventArgs e) {
            Kaydet();
        }
    }
}
