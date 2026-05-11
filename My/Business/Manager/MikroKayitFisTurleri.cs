using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My.Business.Manager {
    public enum MikroFisGirisTurleri {
        StokVirmanFisi,
        UretimHareketFisi,
        SayimDepoGirisFisi,
        UretimdenGirisFisi,
        //StokAcilisFisiDepoGiris,
    }
    public enum MikroFisCikisTurleri {
        StokVirmanFisi,
        UretimHareketFisi,
        SarfDepoCikisFisi,
        UretimeCikisFisi,
        FireCikisFisi,
        //StokAcilisFisiDepoCikis,
    }

    public enum MikroAyarFisTurleri {
        UretimUrunGirisFisi,
        UretimStokCikisFisi,

        UretimUrunFireCikisFisi,
        UretimStokFireCikisFisi,


        DepoSevkFisi,
        SarfCikisFisi,
        FireGirisFisi,
        HizliUretimFisi
    }
    public class MikroKayitFisTurleri {

        public static List<string> GetUretimUrunGirisFisiTuruList() {
            List<string> lis = new List<string>();
            lis.Add(MikroFisGirisTurleri.StokVirmanFisi.ToString());
            lis.Add(MikroFisGirisTurleri.UretimHareketFisi.ToString());
            //lis.Add(MikroFisGirisTurleri.StokAcilisFisiDepoGiris.ToString());
            lis.Add(MikroFisGirisTurleri.SayimDepoGirisFisi.ToString());
            lis.Add(MikroFisGirisTurleri.UretimdenGirisFisi.ToString());
            return lis;
        }
        public static List<string> GetUretimStokCikisFisiTuruList() {
            List<string> lis = new List<string>();
            lis.Add(MikroFisCikisTurleri.StokVirmanFisi.ToString());
            lis.Add(MikroFisCikisTurleri.UretimHareketFisi.ToString());
            //lis.Add(MikroFisCikisTurleri.StokAcilisFisiDepoCikis.ToString());
            lis.Add(MikroFisCikisTurleri.SarfDepoCikisFisi.ToString());
            lis.Add(MikroFisCikisTurleri.UretimeCikisFisi.ToString());
            return lis;
        }

        public static List<string> GetUretimUrunFireCikisFisiTuruList() {
            List<string> lis = new List<string>();
            lis.Add(MikroFisCikisTurleri.StokVirmanFisi.ToString());
            lis.Add(MikroFisCikisTurleri.UretimHareketFisi.ToString());
            //lis.Add(MikroFisGirisTurleri.StokAcilisFisiDepoGiris.ToString());
            lis.Add(MikroFisCikisTurleri.FireCikisFisi.ToString());
            lis.Add(MikroFisCikisTurleri.SarfDepoCikisFisi.ToString());
            lis.Add(MikroFisCikisTurleri.UretimeCikisFisi.ToString());
            return lis;
        }
        public static List<string> GetUretimStokFireCikisFisiTuruList() {
            List<string> lis = new List<string>();
            lis.Add(MikroFisCikisTurleri.StokVirmanFisi.ToString());
            lis.Add(MikroFisCikisTurleri.UretimHareketFisi.ToString());
            //lis.Add(MikroFisCikisTurleri.StokAcilisFisiDepoCikis.ToString());
            lis.Add(MikroFisCikisTurleri.FireCikisFisi.ToString());
            lis.Add(MikroFisCikisTurleri.SarfDepoCikisFisi.ToString());
            lis.Add(MikroFisCikisTurleri.UretimeCikisFisi.ToString());
            return lis;
        }


        public static List<string> GetSarfCikisFisiTuruList() {
            List<string> lis = new List<string>();
            lis.Add(MikroFisCikisTurleri.StokVirmanFisi.ToString());
            lis.Add(MikroFisCikisTurleri.SarfDepoCikisFisi.ToString());
            return lis;
            //lis.Add(MikroFisCikisTurleri.StokAcilisFisiDepoCikis.ToString());
        }
        public static List<string> GetFireGirisFisiTuruList() {
            List<string> lis = new List<string>();
            lis.Add(MikroFisCikisTurleri.StokVirmanFisi.ToString());
            lis.Add(MikroFisCikisTurleri.FireCikisFisi.ToString());
            lis.Add(MikroFisCikisTurleri.SarfDepoCikisFisi.ToString());
            return lis;
            //lis.Add(MikroFisCikisTurleri.StokAcilisFisiDepoCikis.ToString());
        }
        public static List<string> GetHizliUretimFisiTuruList() {
            List<string> lis = new List<string>();
            lis.Add(MikroFisCikisTurleri.StokVirmanFisi.ToString());
            lis.Add(MikroFisCikisTurleri.UretimHareketFisi.ToString());
            return lis;
        }

        public static List<string> GetAyarFisTurleriList() {
            List<string> lis = new List<string>();

            lis.Add(MikroAyarFisTurleri.UretimUrunGirisFisi.ToString());
            lis.Add(MikroAyarFisTurleri.UretimStokCikisFisi.ToString());
            lis.Add(MikroAyarFisTurleri.UretimUrunFireCikisFisi.ToString());
            lis.Add(MikroAyarFisTurleri.UretimStokFireCikisFisi.ToString());
            lis.Add(MikroAyarFisTurleri.DepoSevkFisi.ToString());
            lis.Add(MikroAyarFisTurleri.SarfCikisFisi.ToString());
            lis.Add(MikroAyarFisTurleri.FireGirisFisi.ToString());
            lis.Add(MikroAyarFisTurleri.HizliUretimFisi.ToString());
            return lis;
        }

        public static MikroFisCikisTurleri GetMikroCikisFisiTuru(string turu) {
            if (MikroFisCikisTurleri.StokVirmanFisi.ToString() == turu) {
                return MikroFisCikisTurleri.StokVirmanFisi;
            }
            //else if (MikroFisCikisTurleri.StokAcilisFisiDepoCikis.ToString() == turu) {
            //    return MikroFisCikisTurleri.StokAcilisFisiDepoCikis;
            //}

            else if (MikroFisCikisTurleri.SarfDepoCikisFisi.ToString() == turu) {
                return MikroFisCikisTurleri.SarfDepoCikisFisi;
            }
            else if (MikroFisCikisTurleri.UretimeCikisFisi.ToString() == turu) {
                return MikroFisCikisTurleri.UretimeCikisFisi;
            }
            else if (MikroFisCikisTurleri.UretimHareketFisi.ToString() == turu) {
                return MikroFisCikisTurleri.UretimHareketFisi;
            }
            else if (MikroFisCikisTurleri.FireCikisFisi.ToString() == turu) {
                return MikroFisCikisTurleri.FireCikisFisi;
            }
            else {
                throw new Exception(turu + " Cikis Fiş Türü Bulunamadı");
            }
        }
        public static MikroFisGirisTurleri GetMikroGirisFisiTuru(string turu) {
            if (MikroFisGirisTurleri.StokVirmanFisi.ToString() == turu) {
                return MikroFisGirisTurleri.StokVirmanFisi;
            }
            //else if (MikroFisGirisTurleri.StokAcilisFisiDepoGiris.ToString() == turu) {
            //    return MikroFisGirisTurleri.StokAcilisFisiDepoGiris;
            //}

            else if (MikroFisGirisTurleri.SayimDepoGirisFisi.ToString() == turu) {
                return MikroFisGirisTurleri.SayimDepoGirisFisi;
            }
            else if (MikroFisGirisTurleri.UretimdenGirisFisi.ToString() == turu) {
                return MikroFisGirisTurleri.UretimdenGirisFisi;
            }
            else if (MikroFisGirisTurleri.UretimHareketFisi.ToString() == turu) {
                return MikroFisGirisTurleri.UretimHareketFisi;
            }
            else {
                throw new Exception(turu + " Giris Fiş Türü Bulunamadı");
            }
        }
        public static MikroAyarFisTurleri GetMikroAyarFisTuru(string turu) {

            if (MikroAyarFisTurleri.UretimUrunGirisFisi.ToString() == turu) {
                return MikroAyarFisTurleri.UretimUrunGirisFisi;
            }
            else if (MikroAyarFisTurleri.UretimStokCikisFisi.ToString() == turu) {
                return MikroAyarFisTurleri.UretimStokCikisFisi;
            }

            else if (MikroAyarFisTurleri.UretimStokFireCikisFisi.ToString() == turu) {
                return MikroAyarFisTurleri.UretimStokFireCikisFisi;
            }
            else if (MikroAyarFisTurleri.UretimUrunFireCikisFisi.ToString() == turu) {
                return MikroAyarFisTurleri.UretimUrunFireCikisFisi;
            }
            else if (MikroAyarFisTurleri.DepoSevkFisi.ToString() == turu) {
                return MikroAyarFisTurleri.DepoSevkFisi;
            }
            else if (MikroAyarFisTurleri.SarfCikisFisi.ToString() == turu) {
                return MikroAyarFisTurleri.SarfCikisFisi;
            }
            else if (MikroAyarFisTurleri.FireGirisFisi.ToString() == turu) {
                return MikroAyarFisTurleri.FireGirisFisi;
            }
            else if (MikroAyarFisTurleri.HizliUretimFisi.ToString() == turu) {
                return MikroAyarFisTurleri.HizliUretimFisi;
            }
            else {
                throw new Exception("Ayar Fiş Türü Bulunamadı");
            }
        }
        public static MikroFisGirisTurleri GetHizliUretimFisiTuru(string turu) {
            if (MikroFisGirisTurleri.StokVirmanFisi.ToString() == turu) {
                return MikroFisGirisTurleri.StokVirmanFisi;
            }
            else if (MikroFisGirisTurleri.UretimHareketFisi.ToString() == turu) {
                return MikroFisGirisTurleri.UretimHareketFisi;
            }
            else {
                throw new Exception(turu + " Hizli Uretim Fiş Türü Bulunamadı");
            }
        }

    }
}
