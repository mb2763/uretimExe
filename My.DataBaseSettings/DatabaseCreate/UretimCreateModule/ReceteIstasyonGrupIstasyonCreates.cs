using My.DatabaseSettings.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My.DatabaseSettings.DatabaseCreate.UretimCreateModule
{
    public static class ReceteIstasyonGrupIstasyonCreates
    {
        private const string _tabloAdi = "ReceteIstasyonGrupIstasyon";
        public static void ReceteIstasyonGrupIstasyonCreate(this List<string> list)
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
            list.Add(TableCreate.SqlSutun(_tabloAdi, "RcAId", TableCreate.UniqueIdentifier(), "")); 
            list.Add(TableCreate.SqlSutun(_tabloAdi, "GrupKodu", TableCreate.VarChar(50), "")); 
        }
    }
}
