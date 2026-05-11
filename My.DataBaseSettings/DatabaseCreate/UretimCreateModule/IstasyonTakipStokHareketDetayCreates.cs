using My.DatabaseSettings.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My.DatabaseSettings.DatabaseCreate.UretimCreateModule {
    public static class IstasyonTakipStokHareketDetayCreates {
        private const string _tabloAdi = "IstasyonTakipStokHareketDetay";
        private const string _idAdi = "Id";
        public static void IstasyonTakipStokHareketDetayCreate(this List<string> list) {
            list.CreateTable(_idAdi);
            list.CreateColumns();
            list.AddData();
        }
        static void CreateTable(this List<string> list, string idAdi = _idAdi) {
            var sql = TableCreate.SqlTablo(_tabloAdi, idAdi);
            list.Add(sql);
        }
        static void CreateColumns(this List<string> list) {
            list.Add(TableCreate.SqlSutun(_tabloAdi, "IsEmriKodu", TableCreate.VarChar(35), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "UrtStokKodu", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "UrtStokAdi", TableCreate.VarChar(150), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "StokKodu", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "StokAdi", TableCreate.VarChar(150), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Turu", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "SipAdet", TableCreate.Double(), "0"));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Birim", TableCreate.VarChar(35), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Renk", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Beden", TableCreate.VarChar(50), "")); 
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Parti", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Lot", TableCreate.VarChar(50), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "UretimMiktar", TableCreate.Double(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "FireMiktar", TableCreate.Double(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "IptalMiktar", TableCreate.Double(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "Carpan", TableCreate.Double(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "StokMiktar", TableCreate.Double(), "0"));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "StokFireMiktar", TableCreate.Double(), "0"));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "StokIptalMiktar", TableCreate.Double(), "0")); 
            list.Add(TableCreate.SqlSutun(_tabloAdi, "UrId", TableCreate.UniqueIdentifier(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "UrIId", TableCreate.UniqueIdentifier(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "UrSTId", TableCreate.UniqueIdentifier(), "")); 
            list.Add(TableCreate.SqlSutun(_tabloAdi, "IstHrId", TableCreate.UniqueIdentifier(), "")); 
            list.Add(TableCreate.SqlSutun(_tabloAdi, "IstHrDId", TableCreate.UniqueIdentifier(), "")); 
            list.Add(TableCreate.SqlSutun(_tabloAdi, "SipId", TableCreate.UniqueIdentifier(), ""));
            list.Add(TableCreate.SqlSutun(_tabloAdi, "SipHId", TableCreate.UniqueIdentifier(), ""));
           
            
        }
        static void AddData(this List<string> list) {

        }
    }
}
