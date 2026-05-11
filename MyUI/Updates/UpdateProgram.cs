using DevExpress.XtraBars;
using My.FtpManager;
using System;
using System.IO;
using System.Windows.Forms;

namespace MyUI.Updates {
    public class UpdateProgram {
        public string user { get; set; } = "update_uretimv1";
        public string pass { get; set; } = "14531453**";
      //  public string pass { get; set; } = "19771984cx.";
    //    public string url { get; set; } = "athena.domainhizmetleri.com/";
        public string url { get; set; } = "update.ceppatron.com/update_uretimv1/";
        public string ProgramYolu { get; set; } = Application.StartupPath;
        public string ProgramAdi { get; set; } = "CepPatronERP";
        public string Versiyon { get; set; } = "1000";
        public string VersiyonDosyaAdi { get; set; } = "versiyon.txt";
        public string DownloadFileDosyaAdi { get; set; } = "download.zip";

        FtpManager ftpManager;
        public UpdateProgram() {
            ftpManager = new FtpManager(user, pass, url, ProgramYolu, ProgramAdi, VersiyonDosyaAdi, DownloadFileDosyaAdi);
            GetProVersiyon();
        }
        private void GetProVersiyon() {
            try {
                var vers = File.ReadAllText(VersiyonDosyaAdi);
                Versiyon = vers;
            }
            catch (Exception) {
                File.WriteAllText(VersiyonDosyaAdi, "1000");
            }
        }
        public bool VersiyonKontrol() {
            try {
                ftpManager.OldFileDelete();
            }
            catch (Exception) { 
            } 
            try {
                ftpManager.UpdateFolderDelete();
            }
            catch (Exception) { 
            }
            try {

                var downvers = ftpManager.GetVersiyon();
                if (!downvers.StartsWith("Error")) {
                    if (Convert.ToInt64(downvers) > Convert.ToInt64(Versiyon)) {
                        if (MesajSor("Güncelleme Bulundu Güncelleme Yapmak istiyormusunuz..")) { 
                            var sn = ftpManager.DownloadFile(downvers);
                            if (sn.StartsWith("Error")) { 
                                MessageBox.Show(sn); 
                            }
                            return true;
                        }
                        else {
                            return false;
                        } 
                    }
                }
                else {
                    MessageBox.Show(downvers);
                }
            }
            catch (Exception e) {
                MessageBox.Show(e.Message);
            }
            return false;
        }
        public bool MesajSor(string mesaj) {

            DialogResult kontrol = new DialogResult();
            kontrol = FlyoutMessageBox.Show(mesaj, "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            return kontrol == DialogResult.Yes ? true : false;
        }
    }
}
