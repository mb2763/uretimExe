using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using My.DatabaseSettings.Base;

namespace My.DatabaseSettings.DatabaseCreate.UretimCreateModule
{
    public static class ReceteGrupDetayCreates
    {
        private const string _tabloAdi = "ReceteGrupDetay";
        public static void ReceteGrupDetayCreate(this List<string> list)
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
            list.Add(TableCreate.SqlSutun(_tabloAdi, "ReceteKodu", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "ReceteAdi", TableCreate.VarChar(150), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Miktar", TableCreate.Float(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Aciklama", TableCreate.VarChar(500), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "RcGId", TableCreate.UniqueIdentifier(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "RcAId", TableCreate.UniqueIdentifier(), ""));  
            
        }
    }
}
