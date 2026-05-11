
using System.Collections.Generic;
using My.DatabaseSettings.Base;

namespace My.DatabaseSettings.DatabaseCreate.UretimCreateModule
{
    public static class ReceteOperasyonCreates
    {
        private const string _tabloAdi = "ReceteOperasyon";
        public static void ReceteOperasyonCreate(this List<string> list)
        {
            list.CreateTable("Id");
            list.CreateColumns();
        }
        static void CreateTable(this List<string> list, string idAdi = "Id")
        {
            var sql = TableCreate.SqlTablo(_tabloAdi, idAdi);
            list.Add(sql);
        }
        static void CreateColumns(this List<string> list)
        {
            list.Add(TableCreate.SqlSutun(_tabloAdi, "OperasyonKodu", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "OperasyonAdi", TableCreate.VarChar(150), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "MaliyetFiyat", TableCreate.Double(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Sira", TableCreate.Int(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "UretimSure", TableCreate.Double(), "0"));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "KullanilacakAparat", TableCreate.String(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "OlcumDegeriMin", TableCreate.Double(), "0"));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "OlcumDegeriMax", TableCreate.Double(), "0"));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "OlcumDegeriMin2", TableCreate.Double(), "0"));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "OlcumDegeriMax2", TableCreate.Double(), "0"));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "OlcumDegeriMin3", TableCreate.Double(), "0"));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "OlcumDegeriMax3", TableCreate.Double(), "0"));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "OlcumDegeriMin4", TableCreate.Double(), "0"));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "OlcumDegeriMax4", TableCreate.Double(), "0"));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "OlcumDegeriMin5", TableCreate.Double(), "0"));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "OlcumDegeriMax5", TableCreate.Double(), "0"));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "RcAId", TableCreate.UniqueIdentifier(), ""));
        }
    }
}