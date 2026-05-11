using Dapper;
using My.Core.Data;
using My.Entities.Mikro;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace My.DataAccess.MikroModul {
    public class MikroStokDal : BaseDal<MikroStok>, IMikroStokDal {
        public MikroStokDal(IDbConnection connection) : base(connection) {
        }

        //public override MikroStok SelectFirstWhere(string whereSql = "", string columnList = "*", IDbTransaction transaction = null)
        //{
        //    string sql = @"SELECT   TOP 100 PERCENT S.sto_Guid As StGuid,S.sto_kod AS StokKodu, S.sto_isim AS StokAdi,
        //    sto_anagrup_kod AS Grup /* ANA GRUP */  , sto_altgrup_kod as AltGrup, 
        //    dbo.fn_DovizIsmi(dbo.fn_StokFiyatDovizCinsi(sto_kod, 1, 0, 1)) AS Dvz, /* DVZ */
        //    s.sto_birim1_ad as Birim,   S.sto_renk_kodu As RenkKodu,S.sto_beden_kodu as BedenKodu,
        //    S.sto_cins as StokCinsi, sto_plu_no as PluNo ,
        //    dbo.fn_StokSatisFiyati(sto_kod, 1, 0, 1) AS Fiyati
        //    /* dbo.fn_StokSatisFiyati (sto_kod, 1, 0,1) AS [TOPTAN FİYATI]   ,
        //      dbo.fn_StokSatisFiyati (sto_kod, 2, 0,1) AS [PERAKENDE FİYATI]  ,
        //      dbo.fn_StokSatisFiyati (sto_kod, 4, 0,1) AS [Vadeli Fiyat]   ,
        //      dbo.fn_StokSatisFiyati (sto_kod, 3, 0,1) AS [Diger FİYATI]  FİYAT  ,*/
        //    FROM STOKLAR S " + whereSql;
        //    // --where S.sto_cins = 1 /* 1 hammadde,4 mamül ,0  ticari mal */"
        //    if (transaction == null)
        //    {
        //        if (Connection.State != ConnectionState.Open) Connection.Open();
        //    }
        //    var data = Connection.Query<MikroStok>(sql, null, transaction: transaction).FirstOrDefault();
        //    if (transaction == null)
        //    {
        //        if (Connection.State == ConnectionState.Open) Connection.Close();
        //    }
        //    return data;
        //}

        public IEnumerable<MikroStok> GetViewListWhere(string whereSql, string stokGrubKodu) {
            var sql = $@"SELECT   TOP 100 PERCENT S.sto_Guid As StGuid,S.sto_kod AS StokKodu, S.sto_isim AS StokAdi,
            {stokGrubKodu} AS AnaGrup /* sto_anagrup_kod ANA GRUP */  , sto_altgrup_kod as AltGrup, 
            dbo.fn_DovizIsmi(dbo.fn_StokFiyatDovizCinsi(sto_kod, 1, 0, 1)) AS Dvz, /* DVZ */
            s.sto_birim1_ad as Birim,   S.sto_renk_kodu As RenkKodu,S.sto_beden_kodu as BedenKodu,
            coalesce( S.sto_cins,0) as StokCinsi, sto_plu_no as PluNo ,
            dbo.fn_StokSatisFiyati(sto_kod, 1, 0, 1) AS Fiyati,
            coalesce(S.sto_model_kodu,'') as ModelKodu
            /* dbo.fn_StokSatisFiyati (sto_kod, 1, 0,1) AS [TOPTAN FİYATI]   ,
              dbo.fn_StokSatisFiyati (sto_kod, 2, 0,1) AS [PERAKENDE FİYATI]  ,
              dbo.fn_StokSatisFiyati (sto_kod, 4, 0,1) AS [Vadeli Fiyat]   ,
              dbo.fn_StokSatisFiyati (sto_kod, 3, 0,1) AS [Diger FİYATI]  FİYAT  ,*/
            FROM STOKLAR S  WITH (NOLOCK) " + whereSql;
            // --where S.sto_cins = 1 /* 1 hammadde,4 mamül ,0  ticari mal */"

            if (Connection.State != ConnectionState.Open) Connection.Open();

            var data = Connection.Query<MikroStok>(sql);

            if (Connection.State == ConnectionState.Open) Connection.Close();

            return data;
        }
   public IEnumerable<MikroStokMaliyet> GetMikroStokMaliyetListWhere(string whereSql ) {
            var sql = $@"    SELECT   TOP 100 PERCENT S.sto_Guid As StGuid,S.sto_kod AS StokKodu,  S.sto_isim AS StokAdi,
            s.sto_birim1_ad as Birim, S.sto_renk_kodu As RenkKodu,S.sto_beden_kodu as BedenKodu,
            coalesce( S.sto_cins,0) as StokCinsi,
            coalesce( STU.STANDARTMALIYET,'0') as STANDARTMALIYET, coalesce( STU.RECETEMALIYET ,'0') AS RECETEMALIYET
            FROM STOKLAR S WITH (NOLOCK) 
            LEFT JOIN STOKLAR_USER STU ON Record_uid=sto_Guid " + whereSql;
           
            if (Connection.State != ConnectionState.Open) Connection.Open();

            var data = Connection.Query<MikroStokMaliyet>(sql);

            if (Connection.State == ConnectionState.Open) Connection.Close();

            return data;
        }



        public IEnumerable<MikroStokRenk> GetRenkListWhere(string wheresql) {
            var sql = @"SELECT    *   FROM STOK_RENK_TANIMLARI   WITH (NOLOCK)   " + wheresql + ";";
            if (Connection.State != ConnectionState.Open) Connection.Open();
            var data = Connection.Query<MikroStokRenk>(sql);
            if (Connection.State == ConnectionState.Open) Connection.Close();
            return data;
        }


        public IEnumerable<MikroStokBeden> GetBedenListWhere(string wheresql) {
            var sql = @"SELECT    *   FROM STOK_BEDEN_TANIMLARI  WITH (NOLOCK)    " + wheresql + ";";
            if (Connection.State != ConnectionState.Open) Connection.Open();
            var data = Connection.Query<MikroStokBeden>(sql);
            if (Connection.State == ConnectionState.Open) Connection.Close();
            return data;
        }

        public MikroStokRenk GetRenkByKodu(string renkKodu) {
            var sql = @"SELECT    *   FROM STOK_RENK_TANIMLARI  WITH (NOLOCK)   where rnk_kodu ='" + renkKodu + "'";
            if (Connection.State != ConnectionState.Open) Connection.Open();
            var data = Connection.Query<MikroStokRenk>(sql).FirstOrDefault();
            if (Connection.State == ConnectionState.Open) Connection.Close();
            return data;
        }
        public MikroStokBeden GetBedenByKodu(string bedenKodu) {
            var sql = @"SELECT    *   FROM STOK_BEDEN_TANIMLARI  WITH (NOLOCK)   where bdn_kodu ='" + bedenKodu + "'";
            if (Connection.State != ConnectionState.Open) Connection.Open();
            var data = Connection.Query<MikroStokBeden>(sql).FirstOrDefault();
            if (Connection.State == ConnectionState.Open) Connection.Close();
            return data;
        }

        public MikroStokRenk GetRenkByStokKodu(string stokKodu) {
            var sql = @"DECLARE @kodu VARCHAR(25); 
              SELECT TOP 1 @kodu = sto_renk_kodu FROM STOKLAR  WITH (NOLOCK)  WHERE sto_kod='" + stokKodu + @"'; 
              SELECT    *   FROM STOK_RENK_TANIMLARI  r  WITH (NOLOCK)  WHERE r.rnk_kodu=@kodu ;  ";
            if (Connection.State != ConnectionState.Open) Connection.Open();
            var data = Connection.Query<MikroStokRenk>(sql).FirstOrDefault();
            if (Connection.State == ConnectionState.Open) Connection.Close();
            return data;
        }
        public MikroStokBeden GetBedenByStokKodu(string stokKodu) {
            var sql = @" DECLARE @kodu VARCHAR(25); 
              SELECT TOP 1 @kodu = sto_beden_kodu FROM STOKLAR  WITH (NOLOCK)  WHERE sto_kod='" + stokKodu + @"'; 
              SELECT    *   FROM STOK_BEDEN_TANIMLARI b  WITH (NOLOCK)  WHERE b.bdn_kodu=@kodu ;";
            if (Connection.State != ConnectionState.Open) Connection.Open();
            var data = Connection.Query<MikroStokBeden>(sql).FirstOrDefault();
            if (Connection.State == ConnectionState.Open) Connection.Close();
            return data;
        }
   public List<string> GetStokKategoriler( ) {
            var sql = @" select ktg_isim as StokKategori  FROM dbo.STOK_KATEGORILERI   WITH (NOLOCK) order by ktg_isim ;";
            if (Connection.State != ConnectionState.Open) Connection.Open();
            var data = Connection.Query<string>(sql).ToList();
            if (Connection.State == ConnectionState.Open) Connection.Close();
            return data;
        }

    }
}