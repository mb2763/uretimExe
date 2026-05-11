using My.DatabaseSettings.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace My.DatabaseSettings.DatabaseCreate.GenelCreateModule {
    public class CreateStok {



   
    private static string _tabloAdi = "Stok";
    private static string _idAdi = "StokId";
    public static void AddSql(List<string> list) {
        CreateTable(list);
        CreateColumns(list);
        AddData(list);
    }
    static void CreateTable(List<string> list) {
        var sql = TableCreate.SqlTablo(_tabloAdi, _idAdi);
        list.Add(sql);
    }
    static void CreateColumns(List<string> list) {
        list.Add(TableCreate.SqlSutun(_tabloAdi, "StokKodu", TableCreate.VarChar(25), ""));
        list.Add(TableCreate.SqlSutun(_tabloAdi, "StokAdi", TableCreate.VarChar(150), ""));
        list.Add(TableCreate.SqlSutun(_tabloAdi, "Barkodu", TableCreate.VarChar(150), ""));
        list.Add(TableCreate.SqlSutun(_tabloAdi, "Birimi", TableCreate.VarChar(25), ""));
        list.Add(TableCreate.SqlSutun(_tabloAdi, "Aciklamasi", TableCreate.VarChar(255), ""));
        list.Add(TableCreate.SqlSutun(_tabloAdi, "Grup", TableCreate.VarChar(50), ""));
        list.Add(TableCreate.SqlSutun(_tabloAdi, "AraGrup", TableCreate.VarChar(50), ""));
        list.Add(TableCreate.SqlSutun(_tabloAdi, "AltGrup", TableCreate.VarChar(50), ""));
        list.Add(TableCreate.SqlSutun(_tabloAdi, "AlisKdv", TableCreate.Int(), "0"));
        list.Add(TableCreate.SqlSutun(_tabloAdi, "SatisKdv", TableCreate.Int(), "0"));
        list.Add(TableCreate.SqlSutun(_tabloAdi, "Marka", TableCreate.VarChar(50), ""));
        list.Add(TableCreate.SqlSutun(_tabloAdi, "Model", TableCreate.VarChar(50), ""));
        list.Add(TableCreate.SqlSutun(_tabloAdi, "Model", TableCreate.VarChar(50), ""));
        list.Add(TableCreate.SqlSutun(_tabloAdi, "Aktif", TableCreate.SmallInt(), "1"));
    }
    static void AddData(List<string> list) {

        }
    }
}
