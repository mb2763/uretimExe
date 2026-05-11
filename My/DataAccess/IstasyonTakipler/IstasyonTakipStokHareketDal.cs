using Dapper;
using My.Core.Data;
using My.Entities.IstasyonTakipler;
using My.Entities.Models;
using System;
using System.Collections.Generic;
using System.Data;

namespace My.DataAccess.IstasyonTakipler
{
    public class IstasyonTakipStokHareketDal : BaseDal<IstasyonTakipStokHareket>, IIstasyonTakipStokHareketDal
    {
        public IstasyonTakipStokHareketDal(IDbConnection connection) : base(connection)
        {

        }

        public IEnumerable<IstasyonTakipStokHareket> GetViewListWhere(string whereSql)
        {
            var sql =
                @"  select *  from IstasyonTakipStokHareket   " + whereSql + "  ";

            var data = Connection.Query<IstasyonTakipStokHareket>(sql);
            return data;
        }
       public IEnumerable<IstasyonTakipStokHareketKullanilan> GetViewListKullanimWhere(string andwhereSql)
        {

            //     var sql = @"  SELECT IstSHr.StokKodu,IstSHr.StokAdi,IstSHr.Birim,IstSHr.Renk,IstSHr.Beden, ''  AS Parti, ''  AS Lot 
            //,SH.Miktar AS SipAdet, UrST.PlanlananMiktar 
            //, UrST.PlanlananMiktar/ SH.Miktar AS Carpan
            //,sum(coalesce(IstSHr.KullanilanMiktar,0))   as KullanilanMiktar
            //,sum(coalesce(IstSHr.IptalMiktari,0)) as IptalMiktari
            //,sum(coalesce(IstSHr.FireMiktari,0)) as FireMiktari, 
            //IstSHr. SipId,IstSHr.SipHId  
            //from IstasyonTakipStokHareket IstSHr 
            //LEFT OUTER JOIN UretimIstasyon UrI ON IstSHr.UrIId = UrI.Id
            //LEFT OUTER JOIN UretimOperasyon UrO ON UrI.UrOId = UrO.Id   
            //LEFT OUTER JOIN UretimStok UrST   ON IstSHr.UrSTId = UrST.Id  
            //LEFT OUTER JOIN SiparisHareket SH ON SH.Id=UrST.SipHId
            //where UrO.Sira=1  " + andwhereSql + @"    
            //group by  IstSHr.StokKodu,IstSHr.StokAdi,IstSHr.Birim,IstSHr.Renk,IstSHr.Beden ,IstSHr.SipId ,IstSHr.SipHId 
            // ,SH.Miktar ,UrST.Miktar,UrST.PlanlananMiktar   ";
            var sql = @"  SELECT  HRD.StokKodu, HRD.StokAdi,  
                  HRD.SipAdet, 
                  HRD.Carpan,ROUND( SUM( HRD.StokMiktar),2) AS StokMiktar,
                  ROUND(SUM( HRD.StokFireMiktar),2) AS StokFireMiktar, ROUND(SUM(HRD.StokIptalMiktar),2) AS StokIptalMiktar, 
                  HRD.Birim, HRD.Renk, HRD.Beden, HRD.Parti, HRD.Lot,  
                  SUM(HRD.UretimMiktar) AS UretimMiktari, SUM(HRD.FireMiktar) AS  UretimFireMiktari, SUM(HRD.IptalMiktar) AS UretimIptalMiktari, 
                  HRD.SipId, HRD.SipHId 
                  FROM IstasyonTakipStokHareketDetay HRD  
                  WHERE 1=1  " + andwhereSql + @" 
                  GROUP BY    HRD.StokKodu, HRD.StokAdi,HRD.SipAdet,HRD.Carpan,  HRD.Birim, HRD.Renk, HRD.Beden, HRD.Parti, HRD.Lot, HRD.SipId, HRD.SipHId   
                    ";
            var data = Connection.Query<IstasyonTakipStokHareketKullanilan>(sql);
            return data;
        }
   
     public IEnumerable<IstasyonTakipStokHareketKullanilan> GetViewListKullanimWherePartiLot(string andwhereSql)
        { 
        var sql = @"  SELECT  HRD.StokKodu, HRD.StokAdi,  
                  HRD.SipAdet, 
                  HRD.Carpan,ROUND( SUM( HRD.StokMiktar),2) AS StokMiktar,
                  ROUND(SUM( HRD.StokFireMiktar),2) AS StokFireMiktar, ROUND(SUM(HRD.StokIptalMiktar),2) AS StokIptalMiktar, 
                  HRD.Birim, HRD.Renk, HRD.Beden, HRD.Parti, HRD.Lot,  
                  SUM(HRD.UretimMiktar) AS UretimMiktari, SUM(HRD.FireMiktar) AS  UretimFireMiktari, SUM(HRD.IptalMiktar) AS UretimIptalMiktari, 
                  HRD.SipId, HRD.SipHId 
                  FROM IstasyonTakipStokHareketDetay HRD  
                  WHERE 1=1  " + andwhereSql + @" 
                  GROUP BY    HRD.StokKodu, HRD.StokAdi,HRD.SipAdet,HRD.Carpan,  HRD.Birim, HRD.Renk, HRD.Beden, HRD.Parti, HRD.Lot, HRD.SipId, HRD.SipHId   
                    ";

            var data =  Query<IstasyonTakipStokHareketKullanilan>(sql,null);
            return data;
        }
        public IEnumerable<IstasyonTakipStokHareketKullanilan> GetViewListKullanimWhereMalKabul(string andwhereSql) {
            var sql = @"  SELECT UrST.StokKodu,UrST.StokAdi,SipH.Miktar AS SipAdet, UrST.PlanlananMiktar/ SipH.Miktar AS Carpan  
,SipH.UretimMiktari * (UrST.PlanlananMiktar/ SipH.Miktar) AS StokMiktar
,SipH.FireMiktari * (UrST.PlanlananMiktar/ SipH.Miktar) AS StokFireMiktar
,SipH.IptalMiktari * (UrST.PlanlananMiktar/ SipH.Miktar) AS StokIptalMiktar  
,SipH.UretimMiktari, SipH.FireMiktari AS  UretimFireMiktari, SipH.IptalMiktari AS UretimIptalMiktari
,UrST.Birim,UrST.Renk,UrST.Beden ,'' AS Parti ,'' AS Lot
 ,UrST.SipId, UrST.SipHId 
FROM UretimStok UrST 
LEFT OUTER JOIN SiparisHareket SipH ON SipH.Id=UrST.SipHId 
WHERE 1=1  " + andwhereSql + @" 
                    ";

            var data = Query<IstasyonTakipStokHareketKullanilan>(sql, null);
            return data;
        }
    public IEnumerable<MalKabulFisKullanilanStokModel> GetViewListKullanimMalKabulFis(Guid?  sipid) {
            var sql = $@" SELECT  DSH.StokKodu, DSH.StokAdi, sum(COALESCE( DSH.CikisMiktar,0)) AS Miktar, DSH.Birimi,DSH.PartiNo, DSH.LotNo  
,'' as Renk,'' as Beden,URF.UrId,UR.SipId,  DSH.FisId 
FROM dbo.UretimStokFisHareket DSH 
left OUTER JOIN  UretimStokFis URF ON DSH.FisId = URF.Id
LEFT OUTER JOIN  UretimEmri Ur ON URF.UrId = Ur.Id   
WHERE DSH.GirisCikis=1 and Ur.SipId='{sipid}'
GROUP BY  DSH.StokKodu, DSH.StokAdi, DSH.Birimi, DSH.PartiNo, DSH.LotNo,URF.UrId,UR.SipId,  DSH.FisId  
                    ";

            var data = Query<MalKabulFisKullanilanStokModel>(sql, null);
            return data;
        }

 

        public IEnumerable<IstasyonTakipStokHareket> GetStokHareketByUrIId(Guid urIId)
        {
            var sql = IstasyonTakipStokHareket.GetStokHareketByUrIId(urIId); ;

            var data = Connection.Query<IstasyonTakipStokHareket>(sql);
            return data;
        }


    }
}
        //public IEnumerable<IstasyonTakipStokHareketKullanilan> GetViewListKullanimWherePartiLot1(string andwhereSql)
        //{ 
        //var sql = @"   select IstSHr.StokKodu,IstSHr.StokAdi,IstSHr.Birim,IstSHr.Renk,IstSHr.Beden,coalesce(IstSHr.Parti,'') AS Parti,coalesce(IstSHr.Lot,'') AS Lot 
        //            ,SH.Miktar AS SipAdet, UrST.PlanlananMiktar 
        //            , UrST.PlanlananMiktar/ SH.Miktar AS Carpan
        //            ,sum(coalesce(IstSHr.KullanilanMiktar,0))   as KullanilanMiktar
        //            ,sum(coalesce(IstSHr.IptalMiktari,0)) as IptalMiktari
        //            ,sum(coalesce(IstSHr.FireMiktari,0)) as FireMiktari, 
        //            IstSHr.SipId,IstSHr.SipHId  
        //            from IstasyonTakipStokHareket IstSHr 
        //            LEFT OUTER JOIN UretimIstasyon UrI ON IstSHr.UrIId = UrI.Id
        //            LEFT OUTER JOIN UretimOperasyon UrO ON UrI.UrOId = UrO.Id   
        //            LEFT OUTER JOIN UretimStok UrST   ON IstSHr.UrSTId = UrST.Id  
        //            LEFT OUTER JOIN SiparisHareket SH ON SH.Id=UrST.SipHId
        //            where UrO.Sira=1  " + andwhereSql + @"  
        //            group by  IstSHr.StokKodu,IstSHr.StokAdi,IstSHr.Birim,IstSHr.Renk,IstSHr.Beden,coalesce(IstSHr.Parti,'')  ,coalesce(IstSHr.Lot,''),IstSHr.SipId ,IstSHr.SipHId 
        //             ,SH.Miktar ,UrST.Miktar,UrST.PlanlananMiktar   ";

        //    var data =  Query<IstasyonTakipStokHareketKullanilan>(sql,null);
        //    return data;
        //}