using DevExpress.XtraEditors;
using My.Business.Manager;
using My.Core;
using My.Entities.MailAyarlar;
using My.Kontrol.Kontroller;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyUI.MailModule
{
    public partial class FrmMailSettings : XtraForm
    {

        private MailManager _mng;
        private MailAyar _fis;
        private List<MailAyar> _list;
        public FrmMailSettings()
        {
            InitializeComponent();
        }
        private void Frm_Load(object sender, System.EventArgs e)
        {
            _mng = new MailManager(Ortak.DbPro, Ortak.DbMikro);
            Bagla();
        }
        private void Bagla()
        {
            var rs = _mng.GetMailSettings();
            if (!rs.Success)
            {
                rs.Message.MesajHata();
                return;
            }
            _list = rs.Data.ToList();
            bs.DataSource = _list;
            myGrid1.DataSource = bs;
            foreach (MyGridColumn itm in myView1.Columns)
            {
                itm.OptionsColumn.ReadOnly = true;
            }
        }
        private void AktarTextlere()
        {
            TxtMailKodu.Text = _fis.MailKodu.ToString();
            TxtHost.Text = _fis.Host;
            TxtPort.Text = _fis.Port;
            TxtMailAdres.Text = _fis.MailAdres;
            TxtDisplayName.Text = _fis.DisplayName;
            TxtPass.Text = _fis.Pass.SifreCoz(Ortak.GetKey());
            ChcEnableSsl.Checked = _fis.EnableSsl;
            TxtKonu.Text = _fis.Konu;
            TxtBody.Text = _fis.Body;
        }
        private void TemizleText()
        {
            _fis = new MailAyar();
            TxtMailKodu.Text = "";
            TxtHost.Text = "";
            TxtPort.Text = "";
            TxtMailAdres.Text = "";
            TxtDisplayName.Text = "";
            TxtPass.Text = "";
            ChcEnableSsl.Checked = false;
            TxtKonu.Text = "";
            TxtBody.Text = "";

        }
        private void AktarRowa()
        {
            if (_fis == null)
            {
                _fis = new MailAyar();
            }
            _fis.MailKodu = TxtMailKodu.Text;
            _fis.Host = TxtHost.Text;
            _fis.Port = TxtPort.Text;
            _fis.MailAdres = TxtMailAdres.Text;
            _fis.DisplayName = TxtDisplayName.Text;
            _fis.Pass = TxtPass.Text.Sifrele(Ortak.GetKey());
            _fis.EnableSsl = ChcEnableSsl.Checked;
            _fis.Konu = TxtKonu.Text;
            _fis.Body = TxtBody.Text;

        }
        private bool TextleriKontrolEt()
        {
            if (string.IsNullOrEmpty(TxtHost.Text))
            {
                ("Lütfen Hostu giriniz").MesajHata();
                return false;
            }
            if (string.IsNullOrEmpty(TxtPort.Text))
            {
                ("Lütfen Portu  giriniz").MesajHata();
                return false;
            }
            if (string.IsNullOrEmpty(TxtMailAdres.Text))
            {
                ("Lütfen MailAdresini  giriniz").MesajHata();
                return false;
            }
            if (string.IsNullOrEmpty(TxtPass.Text))
            {
                ("Lütfen Şifre giriniz").MesajHata();
                return false;
            }

            return true;
        }
        private void Kaydet()
        {
            if (!(TextleriKontrolEt())) return;
            AktarRowa();
            var rs = _mng.MailSettingsKaydet(_fis);
            if (!rs.Success)
            {
                rs.Message.MesajHata();
            }
            Bagla();
            TemizleText();
        }
        private async void Sil()
        {
            AktarRowa();
            var rs = await _mng.MailSettingsSil(_fis.Id);
            if (!rs.Success)
            {
                rs.Message.MesajHata();
            }
            Bagla();
            TemizleText();
        }
        private void BtnKapat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void BtnKaydet_Click(object sender, EventArgs e)
        {
            Kaydet();
        }
        private void BtnYeni_Click(object sender, EventArgs e)
        {
            TemizleText();
        }
        private void BtnSil_Click(object sender, EventArgs e)
        {
            if (!("Kaydı Silmek İstiyormusunuz").MesajSor())
            {
                return;
            }
            else
            {
                Sil();
            }
        }
        private void MyView1_MyEventDoubleClickEnter()
        {
            var row = myView1.MyGetCurrentItem<MailAyar>();
            if (row != null)
            {
                _fis = row.Clone();
                AktarTextlere();
            }
        }

        private void BtnTestMail_Click(object sender, EventArgs e)
        {
            FrmMailGonderOrnek f = new FrmMailGonderOrnek();
            f.ShowDialog();
        }
    }
}
