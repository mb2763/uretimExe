using My.Core;
using My.Core.Helpers;
using System.Data;
using System.Text;
using System.Windows.Forms;
namespace My.DatabaseSettings {
    public static class Ortak {
        public static string ANA_KEY = "";
        public static IniDosyasi AyarIni = new IniDosyasi(My.Core.ProgramYolAyarlari.AyarDosyasiYol);
        public static DatabaseModel Settings;
        public static IDbConnection Connection;
        public static string GetKey() {
            StringBuilder sb = new StringBuilder();
            sb.Append("O");
            sb.Append("z");
            sb.Append("e");
            sb.Append("l");
            sb.Append("A");
            sb.Append("n");
            sb.Append("a");
            sb.Append("h");
            sb.Append("t");
            sb.Append("a");
            sb.Append("r");
            sb.Append("1");
            sb.Append("2");
            sb.Append("3");
            sb.Append("4");
            sb.Append("%");
            sb.Append("&");
            return sb.ToString();
        }
        public static void GetSettings() {
            string name = My.Core.ProgramYolAyarlari.DbProAdi;
            ANA_KEY = GetKey();
            Settings = DbConnectionSettings.GetSetting(name, ANA_KEY);
            Connection = DbConnectionSettings.GetConnectionByString(Settings.ConnectionString);
        }
        public static void DatabaseControl() {
            var dbname = Connection.Database;
            DatabaseCreate.GenelCreate dbGenel = new DatabaseCreate.GenelCreate(Connection);
            DatabaseCreate.UretimCreate dbUretim = new DatabaseCreate.UretimCreate(Connection);
            DatabaseCreate.DepoKabulCreate dbDepoKabul = new DatabaseCreate.DepoKabulCreate(Connection);
            var rs = dbGenel.DatabaseKontrol(dbname);
            if (!rs.Success) {
                MessageBox.Show(rs.Message);
            }
            if (rs.Data == 0) {
                if (MessageBox.Show("Veritabanı Bulunamadı Olusturmak İstiyormusunuz", "Veritabanı Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes) {
                    var cont = dbGenel.DatabaseOlustur(dbname);
                    if (!cont.Success) {
                        MessageBox.Show(cont.Message);
                        return;
                    }
                    var cont2 = dbUretim.DatabaseOlustur(dbname);
                    if (!cont2.Success) {
                        MessageBox.Show(cont2.Message);
                        return;
                    }
                    var cont3 = dbDepoKabul.DatabaseOlustur(dbname);
                    if (!cont3.Success) {
                        MessageBox.Show(cont3.Message);
                        return;
                    }
                    MessageBox.Show("Database Olusturuldu.");
                }
            }
        }
        public static void DatabaseGuncelleUretim() {
            DatabaseCreate.GenelCreate dbGenel = new DatabaseCreate.GenelCreate(Connection);
            DatabaseCreate.UretimCreate dbUretim = new DatabaseCreate.UretimCreate(Connection);
            var rs = dbGenel.DatabaseGuncelle();
            if (!rs.Success) {
                MessageBox.Show(rs.Message + rs.Data);
                return;
            }
            var cont2 = dbUretim.DatabaseGuncelle();
            if (!cont2.Success) {
                MessageBox.Show(cont2.Message + cont2.Data);
                return;
            }
            MessageBox.Show("Database Güncellendi.");
        }
        public static void DatabaseGuncelleDepoKabul() {
            DatabaseCreate.GenelCreate dbGenel = new DatabaseCreate.GenelCreate(Connection);
            DatabaseCreate.DepoKabulCreate dbDepoKabul = new DatabaseCreate.DepoKabulCreate(Connection);
            var rs = dbGenel.DatabaseGuncelle();
            if (!rs.Success) {
                MessageBox.Show(rs.Message + rs.Data);
                return;
            }
            var cont2 = dbDepoKabul.DatabaseGuncelle();
            if (!cont2.Success) {
                MessageBox.Show(cont2.Message + cont2.Data);
                return;
            }
            MessageBox.Show("Database Güncellendi.");
        }
        public static T GetCurrentRow<T>(this BindingSource bs) {
            if (bs.Position < 0)
                return default;
            var rw = (T)bs.Current;
            if (rw == null) {
                return default;
            }
            return rw;
        }
    }
}