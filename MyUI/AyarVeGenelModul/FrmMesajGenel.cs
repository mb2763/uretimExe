using My.Business.Service.MesajLar;
using My.Entities.MesajLar;
using My.Kontrol.Formlar;
using System;

namespace MyUI.AyarVeGenelModul
{
    public partial class FrmMesajGenel : MyFrmKayit
    {
        IMesajlarService _srv = Ortak.DbPro.Mesajlar;
        Mesajlar msj;
        public FrmMesajGenel()
        {
            InitializeComponent();
        }

        private void FrmMesajGenel_Load(object sender, EventArgs e)
        {
            MesajBagla();
        }


        private void MesajBagla()
        {
            var rs = _srv.SelectFirstWhere(" where Modul='Genel' and Kodu='Acil'");
            if (!rs.Success)
            {
                MesajHata(rs.Message);
                return;
            }

            msj = rs.Data;
            richTextBox1.Text = rs.Data.Mesaj;
        }

        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            msj.Mesaj = richTextBox1.Text;
            var rs = _srv.InsertOrUpdate(msj);
            if (!rs.Success)
            {
                MesajHata(rs.Message);
                return;
            }
            this.Close();
        }
    }
}
