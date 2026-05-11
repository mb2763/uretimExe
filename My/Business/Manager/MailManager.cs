using My.Core;
using My.Core.Result;
using My.Entities.MailAyarlar;
using My.Entities.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace My.Business.Manager
{
    public class MailManager
    {
        private readonly DatabaseFactoryMikro _dbMikro;
        private DatabaseFactoryPro _dbPro;

        public MailManager(DatabaseFactoryPro dbPro, DatabaseFactoryMikro dbMikro)
        {
            _dbPro = dbPro;
            _dbMikro = dbMikro;
        }

        public IDataResult<IEnumerable<MailAyar>> GetMailSettings()
        {
            try
            {
                return _dbPro.MailSettings.SelectListWhere("");
            }
            catch (Exception e)
            {
                return e.ErrorResultByException<IEnumerable<MailAyar>>();
            }
        }
        public IResult MailSettingsKaydet(MailAyar fis)
        {
            try
            {
                return _dbPro.MailSettings.InsertOrUpdate(fis);
            }
            catch (Exception e)
            {
                return e.ErrorResultByException<string>();
            }
        }
        public async Task<IResult> MailSettingsSil(Guid? id)
        {
            try
            {
                await Task.Delay(1);
                return _dbPro.MailSettings.Delete(c => c.Id == id);
            }
            catch (Exception e)
            {
                return e.ErrorResultByException<string>();
            }
        }

        public static IResult GonderTek(MailAyar ayar, string alicimail, string konu, string body, byte[] dosya, string masterKey, string dosyaUzanti = "pdf")
        {

            SmtpClient sc = new SmtpClient();
            sc.Host = ayar.Host;
            sc.Port = Convert.ToInt32(ayar.Port);
            sc.EnableSsl = ayar.EnableSsl;
            string pass = ayar.Pass.SifreCoz(masterKey);
            sc.Credentials = new NetworkCredential(ayar.MailAdres, pass);
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(ayar.MailAdres, ayar.DisplayName);
            mail.To.Add(alicimail);
            mail.Subject = konu;
            mail.Body = body + "\r\n";
            mail.IsBodyHtml = true;
            mail.Attachments.Clear();
            if (dosya != null)
            {
                Attachment att = null;
                att = new Attachment(new MemoryStream(dosya), "Dosya." + dosyaUzanti);
                mail.Attachments.Add(att);
            }

            try
            {
                sc.Send(mail);
                return new SuccessResult("Mail Gonderildi");
            }
            catch (SmtpException ex)
            {
                var x = "Mail Gonderilemedi .. Hata :" + ex.Message;
                mail.Dispose();
                return new ErrorResult(x);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                var x = "Mail Gonderilemedi .. Hata :" + ex.Message;
                mail.Dispose();
                return new ErrorResult(x);
            }
            catch (InvalidOperationException ex)
            {
                var x = "Mail Gonderilemedi .. Hata :" + ex.Message;
                mail.Dispose();
                return new ErrorResult(x);
            }

        }

        public static IResult Gonder(MailAyar ayar, string alicimail, string konu, string body, List<MailEklentiModel> dosyalar, string masterKey)
        {

            SmtpClient sc = new SmtpClient();
            sc.Host = ayar.Host;
            sc.Port = Convert.ToInt32(ayar.Port);
            sc.EnableSsl = ayar.EnableSsl;
            string pass = ayar.Pass.SifreCoz(masterKey);
            sc.Credentials = new NetworkCredential(ayar.MailAdres, pass);
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(ayar.MailAdres, ayar.DisplayName);
            mail.To.Add(alicimail);
            mail.Subject = konu;
            mail.Body = body + "\r\n";
            mail.IsBodyHtml = true;
            mail.Attachments.Clear();
            if (dosyalar != null)
            {
                Attachment att = null;
                foreach (var byt in dosyalar)
                {
                    att = new Attachment(new MemoryStream(byt.Eklenti), byt.DosyaAdi);
                    mail.Attachments.Add(att);
                }
            }

            try
            {
                sc.Send(mail);
                return new SuccessResult("Mail Gonderildi");
            }
            catch (SmtpException ex)
            {
                var x = "Mail Gonderilemedi .. Hata :" + ex.Message;
                mail.Dispose();
                return new ErrorResult(x);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                var x = "Mail Gonderilemedi .. Hata :" + ex.Message;
                mail.Dispose();
                return new ErrorResult(x);
            }
            catch (InvalidOperationException ex)
            {
                var x = "Mail Gonderilemedi .. Hata :" + ex.Message;
                mail.Dispose();
                return new ErrorResult(x);
            }

        }

        public static void Ornek()
        {
            SmtpClient sc = new SmtpClient();
            sc.Port = 587;
            sc.Host = "smtp.gmail.com";
            sc.EnableSsl = true;
            sc.Credentials = new NetworkCredential("eposta@gmail.com", "gmail_sifre");
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("eposta@gmail.com", "Ekranda Görünecek İsim");
            mail.To.Add("alici1@mail.com");
            mail.To.Add("alici2@mail.com");
            mail.CC.Add("alici3@mail.com");
            mail.CC.Add("alici4@mail.com");
            mail.Subject = "E-Posta Konusu";
            mail.IsBodyHtml = true;
            mail.Body = "E-Posta İçeriği";
            mail.Attachments.Add(new Attachment(@"C:\Rapor.xlsx"));
            mail.Attachments.Add(new Attachment(@"C:\Sonuc.pptx"));
            sc.Send(mail);
        }
    }
}
