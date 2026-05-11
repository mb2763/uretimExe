using My.DatabaseSettings.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My.DatabaseSettings.DatabaseCreate.DepoKabulCreateModule
{
    public static class CreateDepoKontrolIadeSevk
    {

        private const string _tabloAdi = "DepoKontrolIadeSevk";
        public static void DepoKontrolIadeSevkCreate(this List<string> list)
        {
            list.CreateTable("Id");
            list.CreateColumns();
            list.AddData();
        }

        static void CreateTable(this List<string> list, string idAdi = "Id")
        {
            var sql = TableCreate.SqlTablo(_tabloAdi, idAdi);
            list.Add(sql);
        }

        static void CreateColumns(this List<string> list)
        {
           
            list.Add(TableCreate.SqlSutun(_tabloAdi, "CariKodu", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "CariAdi", TableCreate.VarChar(150), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "FisNo", TableCreate.Int(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "IrsSeri", TableCreate.VarChar(25), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "IrsSira", TableCreate.Int(), "0")); 
            list.Add(TableCreate.SqlSutun(_tabloAdi, "IrsTarih", TableCreate.DateTime(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "SevkTarih", TableCreate.DateTime(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "IrsDepoNo", TableCreate.Int(), "0"));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "IadeDepoNo", TableCreate.Int(), "0"));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Aciklama", TableCreate.VarChar(255), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "KayitEden", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Degistiren", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "KayitTarihi", TableCreate.DateTime(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "DegistirmeTarihi", TableCreate.DateTime(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "IrsBelgeNo", TableCreate.VarChar(50), ""));
        }
        static void AddData(this List<string> list)
        {
            //Guid id1 = Guid.Parse("936FF5FD-1333-11ED-8F7E-E4AAEA428528");
            //Guid id2 = Guid.Parse("936FF5FE-1333-11ED-8F7E-E4AAEA428528");

            //string sql = @" 
            //  IF NOT EXISTS (SELECT 1 FROM " + _tabloAdi + @" WHERE Id = '" + id1 + @"') 
            //  begin INSERT [dbo].[" + _tabloAdi + @"] (Id,DepoKodu,DepoAdi ) VALUES ('" + id1 + @"','01','Merkez Depo' ) end ; 

            //";

            //list.Add(sql);
        }
    }
}
