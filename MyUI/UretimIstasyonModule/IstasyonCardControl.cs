using My.Entities.Models;
using System.Drawing;
using System.Windows.Forms;

namespace MyUI.MyControl
{
    public partial class IstasyonCardControl : UserControl
    {

        public UretimIstasyonTakipModel Model;
        public IstasyonCardControl()
        {
            InitializeComponent();
        }

        private void IstasyonCardControl_Load(object sender, System.EventArgs e)
        {
            if (Model == null)
            {
                LblIstasyonKodu.Visible = false;
                LblIstasyonAdi.Visible = false;
                LblFason.Visible = false;
                LblUretimMiktari.Visible = false;
                LblFireMiktari.Visible = false;
                LblPlanlanan.Visible = false;
                LblFasonCariKodu.Visible = false;
                LblFasonCariUnvani.Visible = false;
                LblOperasyon.Visible = false;

                label1.Visible = false;
                label2.Visible = false;
                label3.Visible = false;
                label4.Visible = false;
                label5.Visible = false;

                label6.Visible = false;
                label7.Visible = false;
                label8.Visible = false;
                label9.Visible = false;
                label10.Visible = false;
                label11.Visible = false;
                label12.Visible = false;
                label14.Visible = false;
                label15.Visible = false;
                label13.Visible = false;
                label16.Visible = false;

                return;
            }

            LblIstasyonKodu.Text = Model.IstasyonKodu?.ToString();
            LblIstasyonAdi.Text = Model.IstasyonAdi?.ToString();
            LblFason.Text = Model.Fason == true ? "Evet" : "Hayır";
            LblUretimMiktari.Text = Model.UretimMiktari.ToString();
            LblFireMiktari.Text = Model.FireMiktari.ToString();
            LblPlanlanan.Text = Model.PlanlananMiktar.ToString();
            LblFasonCariKodu.Text = Model.FasonCariKodu.ToString();
            LblFasonCariUnvani.Text = Model.FasonCariUnvani.ToString();
            LblOperasyon.Text = Model.OperasyonKodu + "-" + Model.OperasyonAdi;
        }

        private void pictureBox2_Click(object sender, System.EventArgs e)
        {
            if (Model == null)
            {
                return;
            }
            if (string.IsNullOrEmpty(Model.IstasyonKodu?.ToString())) return;
            Ortak.IstasyonKontrolDetayAc(Model.IstasyonKodu?.ToString(), Model.Fason);
        }

        private void pictureBox2_MouseHover(object sender, System.EventArgs e)
        {
            pictureBox2.BackColor = Color.Maroon;
        }

        private void pictureBox2_MouseLeave(object sender, System.EventArgs e)
        {
            pictureBox2.BackColor = Color.Transparent;
        }
    }
}
