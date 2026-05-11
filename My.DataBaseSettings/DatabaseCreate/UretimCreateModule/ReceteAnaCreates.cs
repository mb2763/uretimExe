using My.DatabaseSettings.Base;
using System.Collections.Generic;

namespace My.DatabaseSettings.DatabaseCreate.UretimCreateModule
{
    public static class ReceteAnaCreates
    {
        private const string _tabloAdi = "ReceteAna";
        public static void ReceteAnaCreate(this List<string> list)
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
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Grubu", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "ReceteKodu", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "ReceteAdi", TableCreate.VarChar(150), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Aciklama", TableCreate.VarChar(500), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "AmbalajSekli", TableCreate.VarChar(500), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "EntegreStokKodu", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "EntegreStokAdi", TableCreate.VarChar(150), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "EntegreBirim", TableCreate.VarChar(35), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "ModelKodu", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "StokCinsiKodu", TableCreate.SmallInt(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "StokCinsiAdi", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "HaziriSonrakiIstasyonaGonder", TableCreate.SmallInt(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "IstasyonGruplamaKullan", TableCreate.SmallInt(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "AparatZorunlu", TableCreate.SmallInt(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "OlcumZorunlu", TableCreate.SmallInt(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "RafOmru", TableCreate.Int(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "KayitEden", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Degistiren", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "KayitTarihi", TableCreate.DateTime(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "DegistirmeTarihi", TableCreate.DateTime(), ""));

        }
    }
}
