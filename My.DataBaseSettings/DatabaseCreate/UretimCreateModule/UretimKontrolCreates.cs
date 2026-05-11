using My.DatabaseSettings.Base;
using System.Collections.Generic;

namespace My.DatabaseSettings.DatabaseCreate.UretimCreateModule
{
    public static class UretimKontrolCreates
    {
        private const string _tabloAdi = "UretimKontrol";
        public static void UretimKontrolCreate(this List<string> list)
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
            list.Add(TableCreate.SqlSutun(_tabloAdi, "UrId", TableCreate.Guid(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "UrIId", TableCreate.Guid(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "IstHrId", TableCreate.Guid(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Turu", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Tarih", TableCreate.DateTime(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "IstasyonKodu", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "IstasyonAdi", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "StokKodu", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "StokAdi", TableCreate.VarChar(150), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "OlcumDegeri", TableCreate.Double(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "OlcumDegeri2", TableCreate.Double(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "OlcumDegeri3", TableCreate.Double(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "OlcumDegeri4", TableCreate.Double(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "OlcumDegeri5", TableCreate.Double(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "OlcumDegeriMin", TableCreate.Double(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "OlcumDegeriMax", TableCreate.Double(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "OlcumDegeriMin2", TableCreate.Double(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "OlcumDegeriMax2", TableCreate.Double(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "OlcumDegeriMin3", TableCreate.Double(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "OlcumDegeriMax3", TableCreate.Double(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "OlcumDegeriMin4", TableCreate.Double(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "OlcumDegeriMax4", TableCreate.Double(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "OlcumDegeriMin5", TableCreate.Double(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "OlcumDegeriMax5", TableCreate.Double(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "HataliGiris", TableCreate.SmallInt(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Aciklama", TableCreate.VarChar(250), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "KayitEden", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Degistiren", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "KayitTarihi", TableCreate.DateTime(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "DegistirmeTarihi", TableCreate.DateTime(), ""));
        }
    }
}
