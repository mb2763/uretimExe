using My.Entities.IstasyonTakipler;
using My.Entities.Models;
using My.Kontrol.Formlar;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyUI.MikroModul {
    public partial class FrmMikroMalKabulHesaplama : MyFrmKayit {

        public List<MalKabulFisKullanilanStokModel> MalKabulFis;
        public List<IstasyonTakipStokHareketKullanilan> IstasyonHareket;
        public   List<IstasyonTakipHareketDetay>  StokFireListPartili; 
        MikroyaKaydetMalKabulHesaplama hsp;
        public FrmMikroMalKabulHesaplama() {
            InitializeComponent();
        }

        private void FrmMikroMalKabulHesaplama_Load(object sender, EventArgs e) {
            hsp =new MikroyaKaydetMalKabulHesaplama(MalKabulFis, IstasyonHareket, StokFireListPartili);
            myGrid1.DataSource=MalKabulFis;
            myGrid2.DataSource= IstasyonHareket;
          

        }

        private void myButton1_Click(object sender, EventArgs e) {
            myGrid4.DataSource = hsp.Convert();
            myGrid3.DataSource = hsp.GetKalanList();
        }
    }
}
