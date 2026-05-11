using DevExpress.XtraEditors;
using My.Business.Manager;
using My.Entities.MailAyarlar;
using My.Entities.Models;
using My.Kontrol.Yazdirma;
using System;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Windows.Forms;


namespace MyUI.MailModule
{
    public partial class FrmFisMailGonder : XtraForm
    {

        private MailManager _mng;
        private MailAyar _setting;

        public string YazdirmaAdi = "SiparisMail";
        public string MailAdres = "";
        public bool OtoGonder = false;
        public SiparisKayitModel Mdl;

        public FrmFisMailGonder()
        {
            InitializeComponent();
        }
        private void FrmMail_Load(object sender, System.EventArgs e)
        {
            _mng = new MailManager(Ortak.DbPro, Ortak.DbMikro);
            Bagla();
            int i = 1;
            Logyaz("Gonderilecek Mail Adedi : " + i);
            TxtMailAdress.Text = MailAdres;
            if (OtoGonder)
            {
                if (GonderPdf())
                {
                    TmrKapat.Start();
                }
            }
        }

        public string GetMailAdres()
        {
            return TxtMailAdress.Text.ToString().Trim();
        }
        public bool Mailkontrol()
        {
            var mail = GetMailAdres();
            try
            {

                MailAddress m = new MailAddress(mail);
                return true;
            }
            catch
            {
                MessageBox.Show("Hatalı mail adresi: " + mail);
                return false;
            }
        }
        private void Logyaz(string mesaj)
        {
            TxtBody.AppendText(mesaj + "\n");
        }

        private void Bagla()
        {
            var rs = _mng.GetMailSettings();
            if (!rs.Success)
            {
                rs.Message.MesajHata();
                return;
            }
            _setting = rs.Data.ToList().FirstOrDefault();
        }
        private bool GonderPdf()
        {
            if (!Mailkontrol())
            {
                return false;
            }

            var mailAdress = GetMailAdres();
            if (_setting == null)
            {
                Logyaz("mail Ayarları bulunamadı");
                ("mail Ayarları bulunamadı").MesajHata();
                return false;
            }
            Logyaz("Dosya oluşturuluyor..");
            var ds = DatasetOlustur();
            var dosya = ds.YazdirPdf(YazdirmaAdi);
            if (dosya == null)
            {
                Logyaz(Mdl.Siparis.CariKodu + " Cariye mail gönderilemedi  Hata Dosya Olusturulamadı. ");
                return false;
            }
            Logyaz("Mail Gonderiliyor..");
            var rsSendmail = MailManager.GonderTek(_setting, mailAdress, _setting.Konu, _setting.Body,
                dosya, Ortak.GetKey(), "PDF");
            if (!rsSendmail.Success)
            {
                Logyaz(Mdl.Siparis.CariKodu + " Cariye mail gönderilemedi  Hata : " + rsSendmail.Message);
                rsSendmail.Message.MesajHata();
                return false;
            }
            else
            {
                Logyaz(Mdl.Siparis.CariKodu + " CariKodu ile " + TxtMailAdress.Text.Trim() + " adresine mail Gonderildi ");
                return true;
            }

        }

        private bool GonderXlsx()
        {
            if (!Mailkontrol())
            {
                return false;
            }
            var mailAdress = GetMailAdres();
            if (_setting == null)
            {
                Logyaz("mail Ayarları bulunamadı");
                ("mail Ayarları bulunamadı").MesajHata();
                return false;
            }

            var ds = DatasetOlustur();
            var dosya = ds.YazdirXlsx(YazdirmaAdi);

            if (dosya == null)
            {
                Logyaz(Mdl.Siparis.CariKodu + " Cariye mail gönderilemedi  Hata Dosya Olusturulamadı. ");
                return false;
            }
            var rsSendmail = MailManager.GonderTek(_setting, mailAdress, _setting.Konu, _setting.Body,
                dosya, Ortak.GetKey(), "xlsx");
            if (!rsSendmail.Success)
            {
                Logyaz(Mdl.Siparis.CariKodu + " Cariye mail gönderilemedi  Hata : " + rsSendmail.Message);
                rsSendmail.Message.MesajHata();
                return false;
            }
            else
            {
                Logyaz(Mdl.Siparis.CariKodu + " CariKodu ile " + TxtMailAdress.Text.Trim() + " adresine mail Gonderildi ");
                return true;
            }

        }
        private DataSet DatasetOlustur()
        {

            var ds = new DataSet("SiparisDS");
            ds.Tables.Add(Mdl.Siparis.ToDataTable("Siparis"));
            ds.Tables.Add(Mdl.Hareketler.ToDataTable("Hareketler"));
            ds.Tables.Add(Mdl.Detaylar.ToDataTable("Detaylar"));
            return ds;
        }
        private void BtnKapat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void BtnGonderPdf_Click(object sender, EventArgs e)
        {
            GonderPdf();
        }

        private void BtnGonderXlsx_Click(object sender, EventArgs e)
        {
            GonderXlsx();
        }

        private void TmrKapat_Tick(object sender, EventArgs e)
        {
            TmrKapat.Stop();
            this.Close();
        }
    }
}
