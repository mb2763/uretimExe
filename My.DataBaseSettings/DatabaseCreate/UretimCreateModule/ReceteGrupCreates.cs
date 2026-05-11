using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using My.DatabaseSettings.Base;

namespace My.DatabaseSettings.DatabaseCreate.UretimCreateModule
{
    public static class ReceteGrupCreates
    {
        private const string _tabloAdi = "ReceteGrup";
        public static void ReceteGrupCreate(this List<string> list)
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
            list.Add(TableCreate.SqlSutun(_tabloAdi, "ReceteGrupKodu", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Aciklama", TableCreate.VarChar(150), ""));
             
        }
    }
}