using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using My.DatabaseSettings.Base;
namespace My.DatabaseSettings.DatabaseCreate.UretimCreateModule
{
  public static  class UretimIstasyonHareketCreates
    {
        private const string _tabloAdi = "UretimIstasyonHareket";
        public static void UretimIstasyonHareketCreate(this List<string> list)
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
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Tarih", TableCreate.DateTime(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "UretimMiktari", TableCreate.Double(), "0"));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "FireMiktari", TableCreate.Double(), "0"));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "IptalMiktari", TableCreate.Double(), "0"));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "PersonelKodu", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "PersonelAdi", TableCreate.VarChar(150), "")); 
            list.Add(TableCreate.SqlSutun(_tabloAdi, "KayitEden", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "KayitTarihi", TableCreate.DateTime(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Degistiren", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "DegistirmeTarihi", TableCreate.DateTime(), ""));
            //
            list.Add(TableCreate.SqlSutun(_tabloAdi, "UrId", TableCreate.UniqueIdentifier(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "UrOId", TableCreate.UniqueIdentifier(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "UrOHId", TableCreate.UniqueIdentifier(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "UrOHDId", TableCreate.UniqueIdentifier(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "UrIId", TableCreate.UniqueIdentifier(), "")); 
            list.Add(TableCreate.SqlSutun(_tabloAdi, "RcAId", TableCreate.UniqueIdentifier(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "RcOId", TableCreate.UniqueIdentifier(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "SipId", TableCreate.UniqueIdentifier(), ""));
            
        }
    }
}