using My.DatabaseSettings.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My.DatabaseSettings.DatabaseCreate.TempCreateModule {
    public static class TempSiparisUretimMiktarCreates {
        private const string _tabloAdi = "TempSiparisUretimMiktar";
        public static void TempSiparisUretimMiktarCreate(this List<string> list) {
            list.CreateTable("Id");
            list.CreateColumns();
        }

        static void CreateTable(this List<string> list, string idAdi = "Id") {
            var sql = TableCreate.SqlTablo(_tabloAdi, idAdi);
            list.Add(sql);
        }

        static void CreateColumns(this List<string> list) {
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Turu", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Kullanici", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "IsEmriKodu", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "IsEmriNo", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Sira", TableCreate.Int(), "0"));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "ReceteKodu", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "ReceteAdi", TableCreate.VarChar(150), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "OperasyonKodu", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "OperasyonAdi", TableCreate.VarChar(150), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "PlanlananMiktar", TableCreate.Float(), "0"));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "UretimMiktari", TableCreate.Float(), "0"));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "FireMiktari", TableCreate.Float(), "0"));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "IptalMiktari", TableCreate.Float(), "0"));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "UrId", TableCreate.UniqueIdentifier(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "UrIId", TableCreate.UniqueIdentifier(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "UrOId", TableCreate.UniqueIdentifier(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "RcAId", TableCreate.UniqueIdentifier(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "RcOId", TableCreate.UniqueIdentifier(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "SipId", TableCreate.UniqueIdentifier(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "SipHId", TableCreate.UniqueIdentifier(), ""));

        }

    }
}
