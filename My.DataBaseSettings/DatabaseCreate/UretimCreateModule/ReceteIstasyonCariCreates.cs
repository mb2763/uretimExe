using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using My.DatabaseSettings.Base;

namespace My.DatabaseSettings.DatabaseCreate.UretimCreateModule
{
    public static class ReceteIstasyonCariCreates
    {
        private const string _tabloAdi = "ReceteIstasyonCari";
        public static void ReceteIstasyonCariCreate(this List<string> list)
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
            list.Add(TableCreate.SqlSutun(_tabloAdi, "CariKodu", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "CariUnvani", TableCreate.VarChar(150), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "RcAId", TableCreate.UniqueIdentifier(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "RcOId", TableCreate.UniqueIdentifier(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "RcIstId", TableCreate.UniqueIdentifier(), ""));
           
        }

    }

}
