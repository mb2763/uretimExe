using MyUI.IstasyonModul;
using MyUI.MikroModule;
using MyUI.Raporlar.IstasyonRaporlari;
using MyUI.ReceteModule;
using MyUI.SiparisModule;
using MyUI.UretimIstasyonModule;
using MyUI.UretimModule;
using MyUI.UretimOperasyonModule;
using MyUI.UretimTalepler;

namespace MyUI {
    public static class FormAc {
     
        public static void FormSec(string tag) {
            switch (tag) {
                case "Siparisler":
                    Siparisler();
                    break;
                case "SiparisEkle":
                    SiparisEkle();
                    break;
                case "ReceteUretimler":
                    ReceteSiparisler();
                    break;
                case "MikrodanAlinanSiparis":
                    MikrodanAlinanSiparis();
                    break;
                case "ReceteUretimEkle":/*Uretim Ekle*/
                    ReceteSiparisEkle();
                    break;
                case "MikroStokListesi":
                    MikroStokListesi();
                    break;
                case "MikroCariListesi":
                    MikroCariListesi();
                    break;
                case "MikroSiparisler":
                    MikroSiparisler();
                    break;
                case "MikroReceteListesi":
                    MikroReceteListesi();
                    break;
                case "ReceteListesi":
                    ReceteListesi();
                    break;
                case "ReceteEkle":
                    ReceteEkle();
                    break;
                case "ReceteGrupTakimlar":
                    ReceteGrupTakimlar();
                    break;
                case "UretimEmirleri":
                    UretimEmirleri();
                    break;
                case "OperasyonTakip":
                    OperasyonTakip();
                    break;
                case "UretimGirisi":
                    UretimGirisi();
                    break;
                case "OperasyonKartlari":
                    OperasyonKartlari();
                    break;
                case "IstasyonKartlari":
                    IsIstasyonKartlari();
                    break;
                case "Personelkartlari":
                    Personelkartlari();
                    break;
                case "IstasyonTakip":
                    IstasyonTakip();
                    break;
                case "Operasyonlar":
                    Operasyonlar();
                    break;
                case "OprerasyonHareketler":
                    OprerasyonHareketler();
                    break;
                case "OprerasyonDetaylar":
                    OprerasyonDetaylar();
                    break;
                case "IstasyonHareketler":
                    IstasyonHareketler();
                    break;
                case "IstasyonDetaylar":
                    IstasyonDetaylar();
                    break;
                case "UretimTalepler":
                    UretimTalepler();
                    break;
                case "IstasyonHareketleri":
                    IstasyonHareketleri();
                    break;
                case "IstasyonBekleyenler":
                    IstasyonBekleyenler();
                    break;
                case "IstasyonFisler":
                    IstasyonFisler();
                    break;
                case "IstasyonHareketDetaylar":
                    IstasyonHareketDetaylar();
                    break;
                case "IstasyonRaporu":
                    IstasyonRaporu();
                    break; 
                default:
                    break;
            }
        }
        private static void Siparisler() {
            FrmSiparisListesi f = new FrmSiparisListesi { Turu = "Siparis", MdiParent = FrmAna.ActiveForm };
            f.Show();
        }
        private static void ReceteSiparisler() {
            FrmSiparisListesi f = new FrmSiparisListesi { Turu = "Recete", MdiParent = FrmAna.ActiveForm };
            f.Show();
        }
        private static void MikrodanAlinanSiparis() {
            FrmSiparisListesi f = new FrmSiparisListesi { Turu = "MikroSiparis", MdiParent = FrmAna.ActiveForm };
            f.Show();
        }
        private static void SiparisEkle() {
            FrmSiparisEd f = new FrmSiparisEd { MdiParent = FrmAna.ActiveForm, pblAlt = { Visible = false } };
            f.Show();
        }
        private static void ReceteSiparisEkle() {
            FrmSiparisReceteEd f = new FrmSiparisReceteEd { MdiParent = FrmAna.ActiveForm, pblAlt = { Visible = false } };
            f.Show();
        }
        private static void MikroStokListesi() {
            FrmMikroStokListesi f = new FrmMikroStokListesi { MdiParent = FrmAna.ActiveForm };
            f.Show();
        }
        private static void MikroCariListesi() {
            FrmMikroCariListesi f = new FrmMikroCariListesi { MdiParent = FrmAna.ActiveForm };
            f.Show();
        }
        private static void MikroSiparisler() {
            FrmMikroSiparisListesi f = new FrmMikroSiparisListesi { MdiParent = FrmAna.ActiveForm };
            f.Show();
        }
        private static void MikroReceteListesi() {
            FrmMikroReceteListesi f = new FrmMikroReceteListesi { MdiParent = FrmAna.ActiveForm };
            f.Show();
        }
        private static void ReceteListesi() {
            FrmReceteListesi f = new FrmReceteListesi { MdiParent = FrmAna.ActiveForm };
            f.Show();
        }
        private static void ReceteEkle() {
            FrmReceteED f = new FrmReceteED();
            f.ShowDialog();
        }
        private static void ReceteGrupTakimlar() {
            FrmReceteGrupListesi f = new FrmReceteGrupListesi {
                MdiParent = FrmAna.ActiveForm
            };
            f.Show();
        }
        private static void OperasyonKartlari() {
            FrmOperasyonKartlari f = new FrmOperasyonKartlari();
            f.ShowDialog();
        }
        private static void IsIstasyonKartlari() {
            FrmIstasyonKartlari f = new FrmIstasyonKartlari();
            f.ShowDialog();
        }
        private static void UretimEmirleri() {
            FrmUretimEmriListesi f = new FrmUretimEmriListesi {
                MdiParent = FrmAna.ActiveForm
            };
            f.Show();
        }
        private static void OperasyonTakip() {
            FrmOperasyonTakip f = new FrmOperasyonTakip {
                MdiParent = FrmAna.ActiveForm
            };
            f.Show();
        }
        private static void Personelkartlari() {
        }
        private static void UretimGirisi() {
            FrmUretimIstasyonUretimGir f = new FrmUretimIstasyonUretimGir();
            f.ShowDialog();
        }
        private static void IstasyonTakip() {
            FrmUretimIstasyonTakip f = new FrmUretimIstasyonTakip() {
                MdiParent = FrmAna.ActiveForm
            };
            f.Show();
        }
        private static void Operasyonlar() {
            FrmUretimOperasyonList f = new FrmUretimOperasyonList {
                MdiParent = FrmAna.ActiveForm
            };
            f.Show();
        }
        private static void OprerasyonHareketler() {
            FrmUretimOperasyonHareketList f = new FrmUretimOperasyonHareketList {
                MdiParent = FrmAna.ActiveForm
            };
            f.Show();
        }
        private static void OprerasyonDetaylar() {
            FrmUretimOperasyonHareketDetayList f = new FrmUretimOperasyonHareketDetayList {
                MdiParent = FrmAna.ActiveForm
            };
            f.Show();
        }
        private static void IstasyonHareketler() {
            FrmUretimIstasyonHareketList f = new FrmUretimIstasyonHareketList {
                MdiParent = FrmAna.ActiveForm
            };
            f.Show();
        }
        private static void IstasyonDetaylar() {
            FrmUretimIstasyonHareketDetayList f = new FrmUretimIstasyonHareketDetayList {
                MdiParent = FrmAna.ActiveForm
            };
            f.Show();
        }
        private static void UretimTalepler() {
            FrmUretimTalepList f = new FrmUretimTalepList {
                MdiParent = FrmAna.ActiveForm
            };
            f.Show();
        }
        private static void IstasyonHareketleri() {
            FrmIstasyonHareketler f = new FrmIstasyonHareketler {
                MdiParent = FrmAna.ActiveForm
            };
            f.Show();
        }
        private static void IstasyonBekleyenler() {
            FrmIstasyonBekleyenler f = new FrmIstasyonBekleyenler {
                MdiParent = FrmAna.ActiveForm
            };
            f.Show();
        }
        private static void IstasyonFisler() {
            FrmIstasyonFisList f = new FrmIstasyonFisList {
                MdiParent = FrmAna.ActiveForm
            };
            f.Show();
        }
        private static void IstasyonHareketDetaylar() {
            FrmIstasyonHareketDetaylar f = new FrmIstasyonHareketDetaylar {
                MdiParent = FrmAna.ActiveForm
            };
            f.Show();
        }

        private static void IstasyonRaporu() {
            FrmIstasyonRaporu f = new FrmIstasyonRaporu {
                MdiParent = FrmAna.ActiveForm
            };
            f.Show();
        }
      


    }
}