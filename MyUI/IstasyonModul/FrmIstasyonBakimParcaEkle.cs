using My.Entities.IstasyonBakimlar;
using My.Kontrol.Formlar;
using System;

namespace MyUI.IstasyonModul
{
    public partial class FrmIstasyonBakimParcaEkle : MyFrmSade
    {
        public IstasyonBakimParca Parca { get; set; }
        public FrmIstasyonBakimParcaEkle()
        {
            InitializeComponent();
        }
        private void FrmIstasyonBakimParcaEkle_Load(object sender, EventArgs e)
        {
            AktarTextlere();
        }
        public void AktarTextlere()
        {
            TxtParca.Text = Parca.Parca;
            TxtParcaNo.Text = Parca.ParcaNo;
            TxtEvrakNo.Text = Parca.EvrakNo;
            TxtAciklama.Text = Parca.Aciklama;
            ChcGaranti.Checked = Parca.Garanti;


        }
        public void AktarModele()
        {
            Parca.Parca = TxtParca.Text;
            Parca.ParcaNo = TxtParcaNo.Text;
            Parca.EvrakNo = TxtEvrakNo.Text;
            Parca.Aciklama = TxtAciklama.Text;
            Parca.Garanti = ChcGaranti.Checked;
        }

        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TxtParca.Text))
            {
                MesajBilgi("Lütfen Parça giriniz..");
                return;
            }

            AktarModele();
            KayitEdildi = true;
            this.Close();
        }


    }
}
