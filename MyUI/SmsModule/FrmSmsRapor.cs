
using My.Business.Service.SmsAyarlar;
using My.Kontrol.Formlar;
using My.Sms;
using My.Sms.NetGsm;
using System;
namespace MyUI.SmsModule {
    public partial class FrmSmsRapor : MyFrmListe {
        ISmsAyarService srv = Ortak.DbPro.SmsAyar;
        SmsSettings settings;
        NetGsmSmsManager smsManager;

        public FrmSmsRapor() {
            InitializeComponent();
        }
        private void FrmSmsRapor_Load(object sender, System.EventArgs e) {
            BaglaSmsAyar();
            // settings = new SmsSettings("2243340752", "Dehset21.", "C.ERATILMIS");
            smsManager = new NetGsmSmsManager(settings);
            GetRapor();
            myGrid1.GridYerlesimYukle();
        }

        private void BaglaSmsAyar() {
            var rs = srv.SelectFirstWhere("");
            if (!rs.Success) {
                MesajHata(rs.Message);
                return;
            }
            var dt = rs.Data;
            if (dt == null) {
                MesajBilgi("Sms Ay,arları Yapılmamış.");
                return;
            }
            settings = new SmsSettings(dt.KullaniciKodu, dt.Sifre, dt.Baslik, dt.GunderimUrl, dt.RaporUrl);
        }

        private void GetRapor() {
            DateTime bas = Convert.ToDateTime(myDateEdit1.Text);
            DateTime son = Convert.ToDateTime(myDateEdit2.Text);
            var rs = smsManager.GetRaporByTarih(bas, son);
            if (!rs.Success) {
                MesajHata(rs.Message);
            }
            myGrid1.DataSource = rs.Data;
        }
        private void myButton2_Click(object sender, EventArgs e) {
            //var rs = smsManager.SmsSend(myTextEdit1.Text, richTextBox2.Text);
            //richTextBox1.Text += rs.Message + "\n" + rs.OrjMessage.ToString();
        }


        private void BtnAra_Click(object sender, EventArgs e) {
            GetRapor();
        }
    }
}
