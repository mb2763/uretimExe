using My.DatabaseSettings.Base;
using System.Collections.Generic;

namespace My.DatabaseSettings.DatabaseCreate.UretimCreateModule {
    public static class UretimEmriCreates {
        private const string _tabloAdi = "UretimEmri";
        public static void UretimEmriCreate(this List<string> list) {
            list.CreateTable("Id");
            list.CreateColumns();
        }

        static void CreateTable(this List<string> list, string idAdi = "Id") {
            var sql = TableCreate.SqlTablo(_tabloAdi, idAdi);
            list.Add(sql);
        }

        static void CreateColumns(this List<string> list) {

            list.Add(TableCreate.SqlSutun(_tabloAdi, "Turu", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Durumu", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "IsEmriNo", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "ReceteKodu", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "ReceteAdi", TableCreate.VarChar(150), ""));

            list.Add(TableCreate.SqlSutun(_tabloAdi, "SiparisKodu", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "SiparisCariKodu", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "SiparisCariUnvani", TableCreate.VarChar(150), ""));

            list.Add(TableCreate.SqlSutun(_tabloAdi, "BaslangicTarihi", TableCreate.DateTime(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "BitisTarihi", TableCreate.DateTime(), ""));

            list.Add(TableCreate.SqlSutun(_tabloAdi, "Aciklama", TableCreate.VarChar(500), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "IstasyonGrupKodu", TableCreate.VarChar(50), ""));

            // list.Add(TableCreate.SqlSutun(_tabloAdi, "PlanlananMiktar", TableCreate.Double(), ""));
            // list.Add(TableCreate.SqlSutun(_tabloAdi, "UretimMiktari", TableCreate.Double(), ""));
            // list.Add(TableCreate.SqlSutun(_tabloAdi, "FireMiktari", TableCreate.Double(), ""));
            // list.Add(TableCreate.SqlSutun(_tabloAdi, "SiparisMiktar", TableCreate.Double(), ""));

            list.Add(TableCreate.SqlSutun(_tabloAdi, "HesaplananMaliyet", TableCreate.Double(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "StokMaliyet", TableCreate.Double(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "OperasyonMaliyetFiyat", TableCreate.Double(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "OperasyonMaliyet", TableCreate.Double(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "IstasyonMaliyetFiyat", TableCreate.Double(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "IstasyonMaliyet", TableCreate.Double(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "NetMaliyet", TableCreate.Double(), ""));

            list.Add(TableCreate.SqlSutun(_tabloAdi, "Kapandi", TableCreate.SmallInt(), ""));

            list.Add(TableCreate.SqlSutun(_tabloAdi, "KayitEden", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "KayitTarihi", TableCreate.DateTime(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Degistiren", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "DegistirmeTarihi", TableCreate.DateTime(), ""));

            list.Add(TableCreate.SqlSutun(_tabloAdi, "RcAId", TableCreate.UniqueIdentifier(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "SipId", TableCreate.UniqueIdentifier(), ""));
           

        }
    }
}
