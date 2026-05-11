using DevExpress.XtraEditors;
using My.Business.Manager;
using My.Entities.MailAyarlar;
using System.Collections.Generic;
using System.Linq;

namespace MyUI.MailModule
{
    public partial class FrmMailGonderOrnek : XtraForm
    {

        private MailManager _mng;
        private MailAyar _mdl;
        private List<MailAyar> _list;
        public FrmMailGonderOrnek()
        {
            InitializeComponent();
        }

        private void FrmMailSendOrnek_Load(object sender, System.EventArgs e)
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

        }
        private void BtnGonder_Click(object sender, System.EventArgs e)
        {
            _mdl = _list.FirstOrDefault();

            if (_mdl == null)
            {
                ("mail Ayarları bulunamadı").MesajHata();
                return;
            }

            var rsSendmail = MailManager.GonderTek(_mdl, TxtMail.Text, TxtKonu.Text, TxtBody.Text, null, Ortak.GetKey());

            if (!rsSendmail.Success)
            {
                rsSendmail.Message.MesajHata();
                return;
            }
            rsSendmail.Message.MesajBilgi();

        }
        private void BtnKapat_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }
    }
}
