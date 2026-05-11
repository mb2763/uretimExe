using My.Business;
using My.Core.Helpers;
using MyUI.AyarVeGenel;
using MyUI.UretimModule;
using System;
using System.Text;
using MyUI.UretimIstasyonModule;
using System.Collections.Generic;
using My.Entities.Ayarlar;
using My.Business.Service.Ayarlar;
using System.Windows.Forms;
using System.Linq;
using My.Core;
using My.Kontrol.Kontroller;
using System.Drawing;

namespace MyUI {
    public static class Ortak {

        public static int ToInt32(this string deger) {
            if (string.IsNullOrEmpty(deger)) {
                return 0;
            }
            try {
                return Convert.ToInt32(deger);
            } catch (Exception) {
                return 0;
            }
        }
     

        public static bool MasaUstuKapanmasin = false;

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

        public static IniDosyasi AyarIni = new IniDosyasi(My.Core.ProgramYolAyarlari.AyarDosyasiYol);
        public static DatabaseFactoryPro DbPro { get; set; }
        public static DatabaseFactoryMikro DbMikro { get; set; }

      
         
        public static void DatabaseGuncelleUretim() {
            string name = My.Core.ProgramYolAyarlari.DbProAdi;
            var ANA_KEY = GetKey();
            DatabaseModel Settings;
            Settings = DbConnectionSettings.GetSetting(name, ANA_KEY);
            var Connection = DbConnectionSettings.GetConnectionByString(Settings.ConnectionString); 
            My.DatabaseSettings.DatabaseCreate.GenelCreate dbGenel = new My.DatabaseSettings.DatabaseCreate.GenelCreate(Connection);
            My.DatabaseSettings.DatabaseCreate.UretimCreate dbUretim = new My.DatabaseSettings.DatabaseCreate.UretimCreate(Connection);
            
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
        public static List<Ayar> MikroEntAyarlar { get; set; }
        public static List<Ayar> GenelAyarlar { get; set; }
        public static List<Ayar> IstasyonAyarlar { get; set; }
     
        public static bool PlKapat { get; set; } = true;
        public static bool MalKabulKullan { get; set; } = false; 
        public static string KullaniciAdi { get; set; }
        public static string MikroStokGrubu { get; set; } = "sto_anagrup_kod";

        public static bool LisansAktif { get; set; } = false;
        public static IniDosyasi DatabaseIni = new IniDosyasi(My.Core.ProgramYolAyarlari.DataBaseAyarDosyasiYol);

        public static void LisansKayit() {
            FrmLisansGiris f = new FrmLisansGiris();
            f.ShowDialog();
        }
        public static void MikroEntAyarlarBagla() {
            IAyarService ayarService = DbPro.Ayarlar;
            var rs = ayarService.SelectListWhere(" where Modul='MikroEntegre' ");
            if (!rs.Success) {
                MessageBox.Show(rs.Message);
                return;
            }
            MikroEntAyarlar = rs.Data.ToList();
        }
     public static void GenelAyarlarBagla() {
            IAyarService ayarService = DbPro.Ayarlar;
            var rs = ayarService.SelectListWhere(" where Modul='Genel' ");
            if (!rs.Success) {
                MessageBox.Show(rs.Message);
                return;
            }
            GenelAyarlar = rs.Data.ToList();

            foreach (var itm in GenelAyarlar) {
                if (itm.Modul=="Genel" && itm.Grup=="Genel" && itm.Kodu== "PlKapat") {
                    if (itm.Deger=="0") {
                        PlKapat = false;
                    }
                }
            }

        }
     
         public static void IstasyonAyarlarBagla() {
            IAyarService ayarService = DbPro.Ayarlar;
            var rs = ayarService.SelectListWhere(" where Modul='IstasyonUretim' ");
            if (!rs.Success) {
                MessageBox.Show(rs.Message);
                return;
            }
            IstasyonAyarlar = rs.Data.ToList();

            foreach (var itm in IstasyonAyarlar) {
                if (itm.Modul == "IstasyonUretim" && itm.Grup == "Istasyon" && itm.Kodu == "MalKabulKullan") {
                    if (itm.Deger == "1") {
                        MalKabulKullan = true;
                    }
                }
            }

        }
        public static void UretimTakipBekleyenAcV2(string operasyon) {
            FrmOperasyonTakipDetaylar f = new FrmOperasyonTakipDetaylar { Operasyon = operasyon, Durumu = "Beklemede" };
            f.ShowDialog();
        }
        public static void UretimTakipUretimdeAcV2(string operasyon, Action action = null) {
            FrmOperasyonTakipDetaylar f = new FrmOperasyonTakipDetaylar {
                Action = action,
                Operasyon = operasyon,
                Durumu = "Uretimde"
            };
            f.ShowDialog();
        }
        public static void IstasyonKontrolDetayAc(string istasyon, bool fason) {
            FrmUretimIstasyonTakipDetay f = new FrmUretimIstasyonTakipDetay {
                IstasyonKodu = istasyon,
                Fason = fason ? 1 : 0
            };
            f.ShowDialog();
        }


        public static void FilterButonRenklendir(this MyButton btn, bool renklendir = false) {
            btn.LookAndFeel.UseDefaultLookAndFeel = false;
            if (renklendir) {
                btn.Appearance.BackColor = Color.Purple;
                btn.Appearance.ForeColor = Color.White;
                btn.AppearancePressed.BackColor = Color.Purple;
                btn.AppearancePressed.ForeColor = Color.White;
            }
            else { 
                btn.Appearance.BackColor = Color.FromArgb(255, 248, 248, 248);
                btn.Appearance.ForeColor = Color.DimGray;
                btn.AppearancePressed.BackColor = Color.FromArgb(255, 248, 248, 248);
                btn.AppearancePressed.ForeColor = Color.DimGray;
            }
        }
    }
}