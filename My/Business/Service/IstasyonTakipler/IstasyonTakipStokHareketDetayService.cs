using My.Core.Data;
using My.Core.Logger;
using My.Core.Result;
using My.Entities.IstasyonTakipler;
using System;
using System.Collections.Generic;
using System.Data;

namespace My.Business.Service.IstasyonTakipler {
    public class IstasyonTakipStokHareketDetayService : BaseServiceCore<IstasyonTakipStokHareketDetay>, IIstasyonTakipStokHareketDetayService {
        public IstasyonTakipStokHareketDetayService(IDbConnection dbConnection, ILogManager log) : base(dbConnection, log) { }

        public IDataResult<string> DetaylarGuncelleToplu() {
            var sql = $@"
 MERGE  IstasyonTakipStokHareketDetay AS Tar 
 USING(
 SELECT  ITHD.Id AS IstHrDId,SIP.SiparisKodu AS IsEmriKodu,SIPH.StokKodu AS UrtStokKodu,SIPH.StokAdi AS UrtStokAdi,
 coalesce(ITSH.StokKodu,'') AS StokKodu, coalesce(ITSH.StokAdi,'') AS StokAdi, ITHD.Turu,  
 SIPH.Miktar AS SipAdet , ISNULL(ITSH.Birim,'') AS Birim,  ISNULL(ITSH.Renk,'') AS Renk,ISNULL( ITSH.Beden,'') AS Beden, ISNULL(ITSH.Parti,'') AS Parti,
 ISNULL(ITSH.Lot,'') AS Lot,  
 ITHD.Miktar AS UretimMiktar, ITHD.FireMiktar, ISNULL(ITHD.IptalMiktar,0) AS IptalMiktar   
,ITHD.Miktar *(ISNULL(UrST.PlanlananMiktar,0)/ SIPH.Miktar) AS StokMiktar
,ITHD.FireMiktar *(ISNULL(UrST.PlanlananMiktar,0)/ SIPH.Miktar) AS StokFireMiktar
,ISNULL(ITHD.IptalMiktar,0) *(ISNULL(UrST.PlanlananMiktar,0)/ SIPH.Miktar) AS StokIptalMiktar
, ISNULL(UrST.PlanlananMiktar,0) / SIPH.Miktar AS Carpan    
, ITHD.UrId ,ITHD.UrIId ,ITSH.UrSTId  
 ,ITHD.IstHrId,UrO.SipId,UrO.SipHId
 FROM  IstasyonTakipHareketDetay ITHD
 LEFT OUTER JOIN UretimIstasyon UrI ON ITHD.UrIId = UrI.Id
 LEFT OUTER JOIN UretimOperasyon UrO ON UrI.UrOId = UrO.Id   
 LEFT OUTER JOIN SiparisHareket SIPH ON SIPH.Id=UrO.SipHId
 LEFT OUTER JOIN Siparis  SIP ON SIP.Id=UrO.SipId
 LEFT OUTER JOIN UretimStok UrST ON UrO.SipHId = UrST.SipHId    
 LEFT OUTER JOIN   (
 SELECT DISTINCT ITSH1.StokKodu,ITSH1.StokAdi,ITSH1.Birim ,ITSH1.Renk ,ITSH1.Beden ,ITSH1.Parti ,ITSH1.Lot ,ITSH1.UrSTId,ITSH1.SipHId 
 FROM IstasyonTakipStokHareket ITSH1 LEFT OUTER JOIN UretimIstasyon UrI1 ON ITSH1.UrIId = UrI1.Id  LEFT OUTER JOIN UretimOperasyon UrO1 ON UrI1.UrOId = UrO1.Id  
 WHERE UrO1.Sira=1 
 ) ITSH ON SIPH.Id = ITSH.SipHId AND ITSH.UrSTId=UrST.Id   
 WHERE UrO.Sira=1 and ITHD.Turu IN ('UretimBitis','MamulGiris','FireMamulGiris','UretimBitis')  
 ) AS Tmp  ON (Tar.IstHrDId=Tmp.IstHrDId AND Tar.SipHId=Tmp.SipHId AND Tar.StokKodu=Tmp.StokKodu  ) WHEN NOT MATCHED BY TARGET 
 THEN INSERT ( IstHrDId ,IsEmriKodu ,UrtStokKodu ,UrtStokAdi ,StokKodu ,StokAdi ,Turu ,SipAdet ,Birim ,Renk ,Beden ,
 Parti ,Lot ,UretimMiktar ,FireMiktar ,IptalMiktar ,Carpan ,StokMiktar ,StokFireMiktar ,StokIptalMiktar ,
 UrId ,UrIId ,UrSTId ,IstHrId ,SipId ,SipHId) 
 VALUES(  Tmp.IstHrDId ,Tmp.IsEmriKodu ,Tmp.UrtStokKodu ,Tmp.UrtStokAdi ,Tmp.StokKodu ,Tmp.StokAdi ,Tmp.Turu ,Tmp.SipAdet ,Tmp.Birim ,Tmp.Renk ,Tmp.Beden ,
 Tmp.Parti ,Tmp.Lot , Tmp.UretimMiktar ,Tmp.FireMiktar ,Tmp.IptalMiktar ,Tmp.Carpan ,Tmp.StokMiktar ,Tmp.StokFireMiktar ,Tmp.StokIptalMiktar ,
 Tmp.UrId ,Tmp.UrIId ,Tmp.UrSTId ,Tmp.IstHrId ,Tmp.SipId ,Tmp.SipHId ) 
 WHEN MATCHED THEN 
 UPDATE SET  Tar.IsEmriKodu = Tmp.IsEmriKodu ,Tar.UrtStokKodu = Tmp.UrtStokKodu ,Tar.UrtStokAdi = Tmp.UrtStokAdi ,Tar.StokKodu = Tmp.StokKodu ,
 Tar.StokAdi = Tmp.StokAdi ,Tar.Turu = Tmp.Turu ,Tar.SipAdet = Tmp.SipAdet ,Tar.Birim = Tmp.Birim ,Tar.Renk = Tmp.Renk ,
 Tar.Beden = Tmp.Beden ,Tar.Parti = Tmp.Parti ,Tar.Lot = Tmp.Lot ,Tar.UretimMiktar = Tmp.UretimMiktar ,Tar.FireMiktar = Tmp.FireMiktar ,
 Tar.IptalMiktar = Tmp.IptalMiktar ,Tar.Carpan = Tmp.Carpan ,Tar.StokMiktar = Tmp.StokMiktar ,Tar.StokFireMiktar = Tmp.StokFireMiktar ,
 Tar.StokIptalMiktar = Tmp.StokIptalMiktar ,Tar.UrId = Tmp.UrId ,Tar.UrIId = Tmp.UrIId ,Tar.UrSTId = Tmp.UrSTId ,
 Tar.IstHrId = Tmp.IstHrId ,Tar.SipId = Tmp.SipId ,Tar.SipHId = Tmp.SipHId  ; 
";

            var rs = Execute(sql, null);
            return rs;
        }
        public IDataResult<string> DetaylarGuncelleBySipId(Guid? sipId) {
            var sql = $@"
MERGE  IstasyonTakipStokHareketDetay AS Tar 
 USING(
 SELECT  ITHD.Id AS IstHrDId,SIP.SiparisKodu AS IsEmriKodu,SIPH.StokKodu AS UrtStokKodu,SIPH.StokAdi AS UrtStokAdi,
 coalesce(ITSH.StokKodu,'') AS StokKodu, coalesce(ITSH.StokAdi,'') AS StokAdi, ITHD.Turu,  
 SIPH.Miktar AS SipAdet , ISNULL(ITSH.Birim,'') AS Birim,  ISNULL(ITSH.Renk,'') AS Renk,ISNULL( ITSH.Beden,'') AS Beden, ISNULL(ITSH.Parti,'') AS Parti,
 ISNULL(ITSH.Lot,'') AS Lot,  
 ITHD.Miktar AS UretimMiktar, ITHD.FireMiktar, ISNULL(ITHD.IptalMiktar,0) AS IptalMiktar   
,ITHD.Miktar *(ISNULL(UrST.PlanlananMiktar,0)/ SIPH.Miktar) AS StokMiktar
,ITHD.FireMiktar *(ISNULL(UrST.PlanlananMiktar,0)/ SIPH.Miktar) AS StokFireMiktar
,ISNULL(ITHD.IptalMiktar,0) *(ISNULL(UrST.PlanlananMiktar,0)/ SIPH.Miktar) AS StokIptalMiktar
, ISNULL(UrST.PlanlananMiktar,0) / SIPH.Miktar AS Carpan    
, ITHD.UrId ,ITHD.UrIId ,ITSH.UrSTId  
 ,ITHD.IstHrId,UrO.SipId,UrO.SipHId
 FROM  IstasyonTakipHareketDetay ITHD
 LEFT OUTER JOIN UretimIstasyon UrI ON ITHD.UrIId = UrI.Id
 LEFT OUTER JOIN UretimOperasyon UrO ON UrI.UrOId = UrO.Id   
 LEFT OUTER JOIN SiparisHareket SIPH ON SIPH.Id=UrO.SipHId
 LEFT OUTER JOIN Siparis  SIP ON SIP.Id=UrO.SipId
 LEFT OUTER JOIN UretimStok UrST ON UrO.SipHId = UrST.SipHId    
 LEFT OUTER JOIN   (
 SELECT DISTINCT ITSH1.StokKodu,ITSH1.StokAdi,ITSH1.Birim ,ITSH1.Renk ,ITSH1.Beden ,ITSH1.Parti ,ITSH1.Lot ,ITSH1.UrSTId,ITSH1.SipHId 
 FROM IstasyonTakipStokHareket ITSH1 LEFT OUTER JOIN UretimIstasyon UrI1 ON ITSH1.UrIId = UrI1.Id  LEFT OUTER JOIN UretimOperasyon UrO1 ON UrI1.UrOId = UrO1.Id  
 WHERE UrO1.Sira=1 
 ) ITSH ON SIPH.Id = ITSH.SipHId AND ITSH.UrSTId=UrST.Id   
 WHERE UrO.Sira=1 and ITHD.Turu IN ('UretimBitis','MamulGiris','FireMamulGiris','UretimBitis')   AND SIP.Id='{sipId}'
 ) AS Tmp  ON (Tar.IstHrDId=Tmp.IstHrDId AND Tar.SipHId=Tmp.SipHId AND Tar.StokKodu=Tmp.StokKodu  ) WHEN NOT MATCHED BY TARGET 
 THEN INSERT ( IstHrDId ,IsEmriKodu ,UrtStokKodu ,UrtStokAdi ,StokKodu ,StokAdi ,Turu ,SipAdet ,Birim ,Renk ,Beden ,
 Parti ,Lot ,UretimMiktar ,FireMiktar ,IptalMiktar ,Carpan ,StokMiktar ,StokFireMiktar ,StokIptalMiktar ,
 UrId ,UrIId ,UrSTId ,IstHrId ,SipId ,SipHId) 
 VALUES(  Tmp.IstHrDId ,Tmp.IsEmriKodu ,Tmp.UrtStokKodu ,Tmp.UrtStokAdi ,Tmp.StokKodu ,Tmp.StokAdi ,Tmp.Turu ,Tmp.SipAdet ,Tmp.Birim ,Tmp.Renk ,Tmp.Beden ,
 Tmp.Parti ,Tmp.Lot , Tmp.UretimMiktar ,Tmp.FireMiktar ,Tmp.IptalMiktar ,Tmp.Carpan ,Tmp.StokMiktar ,Tmp.StokFireMiktar ,Tmp.StokIptalMiktar ,
 Tmp.UrId ,Tmp.UrIId ,Tmp.UrSTId ,Tmp.IstHrId ,Tmp.SipId ,Tmp.SipHId ) 
 WHEN MATCHED THEN 
 UPDATE SET  Tar.IsEmriKodu = Tmp.IsEmriKodu ,Tar.UrtStokKodu = Tmp.UrtStokKodu ,Tar.UrtStokAdi = Tmp.UrtStokAdi ,Tar.StokKodu = Tmp.StokKodu ,
 Tar.StokAdi = Tmp.StokAdi ,Tar.Turu = Tmp.Turu ,Tar.SipAdet = Tmp.SipAdet ,Tar.Birim = Tmp.Birim , 
 Tar.UretimMiktar = Tmp.UretimMiktar ,Tar.FireMiktar = Tmp.FireMiktar ,
 Tar.IptalMiktar = Tmp.IptalMiktar ,Tar.Carpan = Tmp.Carpan ,Tar.StokMiktar = Tmp.StokMiktar ,Tar.StokFireMiktar = Tmp.StokFireMiktar ,
 Tar.StokIptalMiktar = Tmp.StokIptalMiktar ,Tar.UrId = Tmp.UrId ,Tar.UrIId = Tmp.UrIId ,Tar.UrSTId = Tmp.UrSTId ,
 Tar.IstHrId = Tmp.IstHrId ,Tar.SipId = Tmp.SipId ,Tar.SipHId = Tmp.SipHId  ; ";

            var rs = Execute(sql, null);
            return rs;
        }

        /// <summary>
        /// IstasyonTakipStokHareketDetay ITHD,TempMikroStok TMPS
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public IDataResult<IEnumerable<IstasyonTakipStokHareketDetayToplu>> GetListViewInKategoriToplu(string andwhere, string altandwhere) {
           
            string sql = $@"
  SELECT * FROM (SELECT 1 AS SR, ITSHD.StokKodu, ITSHD.StokAdi, ITSHD.Birim, ITSHD.Renk, ITSHD.Beden,ITSHD.Parti, ITSHD.Lot ,
  SUM(ISNULL(ITSHD.UretimMiktar,0)) AS UretimMiktar, 
  SUM(ISNULL(ITSHD.FireMiktar,0)) AS FireMiktar, 
  SUM(ISNULL(ITSHD.IptalMiktar,0)) AS IptalMiktar, 
  SUM(ISNULL(ITSHD.StokMiktar,0)) AS StokMiktar, 
  SUM(ISNULL(ITSHD.StokFireMiktar,0)) AS StokFireMiktar, 
  SUM(ISNULL(ITSHD.StokIptalMiktar ,0)) AS  StokIptalMiktar,
  TMPS.KategoriKodu,TMPS.KategoriAdi, TMPS.KaliteKontrolKodu,TMPS.KaliteKontrolAdi, TMPS.ReyonKodu,TMPS.ReyonAdi
  FROM  IstasyonTakipStokHareketDetay ITSHD 
  LEFT OUTER JOIN TempMikroStok TMPS ON ITSHD.StokKodu = TMPS.StokKodu 
  LEFT OUTER JOIN IstasyonTakipHareketDetay ITHD ON ITHD.Id = ITSHD.IstHrDId  
  where 1=1   {andwhere}  
  GROUP BY ITSHD.StokKodu, ITSHD.StokAdi, ITSHD.Birim, ITSHD.Renk, ITSHD.Beden,ITSHD.Parti, ITSHD.Lot, TMPS.KategoriKodu,TMPS.KategoriAdi, TMPS.KaliteKontrolKodu,TMPS.KaliteKontrolAdi, TMPS.ReyonKodu,TMPS.ReyonAdi  
  UNION ALL  
  SELECT 2 AS S,  ITHD.StokKodu, ITHD.StokAdi, TMPS.Birim1 AS Birim , ITHD.Renk, ITHD.Beden,  ITHD.Parti, ITHD.Lot, 
  0 AS UretimMiktar,    0 AS FireMiktar, 0 AS IptalMiktar,  0 AS StokMiktar,SUM(ISNULL(ITHD.FireMiktar,0))  AS StokFireMiktar, 0 AS StokIptalMiktar,   
  TMPS.KategoriKodu,TMPS.KategoriAdi, TMPS.KaliteKontrolKodu,TMPS.KaliteKontrolAdi, TMPS.ReyonKodu,TMPS.ReyonAdi
  FROM IstasyonTakipHareketDetay ITHD 
  LEFT OUTER JOIN TempMikroStok TMPS ON ITHD.StokKodu = TMPS.StokKodu   
  LEFT OUTER JOIN IstasyonTakipHareket ITH ON ITHD.IstHrId=ITH.Id
  WHERE  ITHD.Turu='FireStokGiris' {altandwhere}
  GROUP BY ITHD.StokKodu, ITHD.StokAdi, TMPS.Birim1, ITHD.Renk, ITHD.Beden,ITHD.Parti, ITHD.Lot, TMPS.KategoriKodu,TMPS.KategoriAdi, TMPS.KaliteKontrolKodu,TMPS.KaliteKontrolAdi, TMPS.ReyonKodu,TMPS.ReyonAdi
  ) HRK ORDER BY  HRK.StokAdi,SR ";
            
            var rs = Query<IstasyonTakipStokHareketDetayToplu>(sql, null);
            return rs;
        }

        /// <summary>
        /// IstasyonTakipStokHareketDetay ITHD,TempMikroStok TMPS
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public IDataResult<IEnumerable<IstasyonTakipStokHareketDetayDetayli>> GetListViewInKategoriDetayli(string andwhere,string altandwhere) {
           

            string sql = $@"  SELECT  HRK.IsEmriKodu, HRK.IsEmriNo, HRK.UrtStokKodu, HRK.UrtStokAdi, HRK.StokKodu, HRK.StokAdi, 
  HRK.Birim, HRK.Renk, HRK.Beden, 
  SUM(ISNULL(HRK.StokMiktar,0)) AS StokMiktar, 
  SUM(ISNULL(HRK.StokFireMiktar,0)) AS StokFireMiktar, 
  SUM(ISNULL(HRK.StokIptalMiktar ,0)) AS  StokIptalMiktar,
  SUM(ISNULL(HRK.UretimMiktar,0)) AS UretimMiktar, 
  SUM(ISNULL(HRK.FireMiktar,0)) AS FireMiktar, 
  SUM(ISNULL(HRK.IptalMiktar,0)) AS IptalMiktar,  
HRK.KategoriKodu,HRK.KategoriAdi, HRK.KaliteKontrolKodu,HRK.KaliteKontrolAdi , HRK.ReyonKodu,HRK.ReyonAdi
FROM (   
SELECT   ITSHD.Id, ITSHD.IsEmriKodu, ITSHD.UrtStokKodu, ITSHD.UrtStokAdi, ITSHD.StokKodu, ITSHD.StokAdi, ITSHD.Turu, 
 ITSHD.Birim, ITSHD.Renk, ITSHD.Beden, 
 ITSHD.StokMiktar, ITSHD.StokFireMiktar, ITSHD.StokIptalMiktar,
 ITSHD.UretimMiktar, ITSHD.FireMiktar, ITSHD.IptalMiktar, ITSHD.SipAdet,ITSHD.Carpan,
 TMPS.KategoriKodu,TMPS.KategoriAdi, TMPS.KaliteKontrolKodu,TMPS.KaliteKontrolAdi, TMPS.ReyonKodu,TMPS.ReyonAdi,
 ITHD.Tarih,'Fis' AS Tip,TMPS.Birim1 ,TMPS.Birim2 ,TMPS.Birim3 ,TMPS.Birim4 
 ,Ur.IsEmriNo
 /* ,ITSHD.UrId, ITSHD.UrIId, ITSHD.UrSTId, ITSHD.IstHrId, ITSHD.IstHrDId, ITSHD.SipId, ITSHD.SipHId */ 
FROM  IstasyonTakipStokHareketDetay ITSHD 
LEFT OUTER JOIN TempMikroStok TMPS ON ITSHD.StokKodu = TMPS.StokKodu 
LEFT OUTER JOIN IstasyonTakipHareketDetay ITHD ON ITHD.Id = ITSHD.IstHrDId 
LEFT OUTER JOIN UretimEmri UR ON ITSHD.UrId = UR.Id   where 1=1 {andwhere}
UNION ALL  
SELECT ITHD.Id, ITH.SiparisKodu AS IsEmriKodu , ITH.ReceteKodu AS UrtStokKodu, ITH.ReceteAdi AS UrtStokAdi, ITHD.StokKodu, ITHD.StokAdi, ITHD.Turu, 
 TMPS.Birim1,coalesce( ITHD.Renk,''),coalesce( ITHD.Beden,''), 
 0 AS StokMiktar,ITHD.FireMiktar AS StokFireMiktar, 0 AS StokIptalMiktar,
  0 AS UretimMiktar,  0 AS FireMiktar, 0 AS IptalMiktar, 0 AS SipAdet ,0 AS Carpan,
 TMPS.KategoriKodu,TMPS.KategoriAdi, TMPS.KaliteKontrolKodu,TMPS.KaliteKontrolAdi, TMPS.ReyonKodu,TMPS.ReyonAdi,
 ITHD.Tarih ,'Fire' AS Tip,TMPS.Birim1 ,TMPS.Birim2 ,TMPS.Birim3 ,TMPS.Birim4
 ,Ur.IsEmriNo 
 FROM IstasyonTakipHareketDetay ITHD 
 LEFT OUTER JOIN TempMikroStok TMPS ON ITHD.StokKodu = TMPS.StokKodu   
 LEFT OUTER JOIN IstasyonTakipHareket ITH ON ITHD.IstHrId=ITH.Id
 LEFT OUTER JOIN UretimEmri UR ON ITHD.UrId = UR.Id 
 WHERE  ITHD.Turu='FireStokGiris'  {altandwhere}
) HRK 
 GROUP BY HRK.IsEmriKodu, HRK.UrtStokKodu, HRK.UrtStokAdi, HRK.StokKodu, HRK.StokAdi, 
  HRK.Birim, HRK.Renk, HRK.Beden, HRK.KategoriKodu,HRK.KategoriAdi, HRK.KaliteKontrolKodu,HRK.KaliteKontrolAdi, HRK.ReyonKodu,HRK.ReyonAdi
  , HRK.IsEmriNo
 ORDER BY HRK.UrtStokAdi,HRK.StokAdi ";
            var rs = Query<IstasyonTakipStokHareketDetayDetayli>(sql, null);
            return rs;
        }

    }
}
