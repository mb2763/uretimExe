using My.Core.Data;
using My.Core.Logger;
using My.Core.Result;
using My.Entities.Templer;
using System;
using System.Collections.Generic;
using System.Data;

namespace My.Business.Service.Templer {
    public class TempSiparisUretimMiktarService : BaseServiceCore<TempSiparisUretimMiktar>, ITempSiparisUretimMiktarService {
        public TempSiparisUretimMiktarService(IDbConnection dbConnection, ILogManager log) : base(dbConnection, log) {

        }
        public IDataResult<IEnumerable<TempSiparisUretimMiktar>> GetTempSiparisUretimMiktarBySipId(Guid? sipId, string kullanici) {
            string sql = $@"
BEGIN  
DECLARE  @kullanici varchar(2000); 
DECLARE  @sipid UNIQUEIDENTIFIER;
SET @kullanici='{kullanici}';
SET @sipid='{sipId}'; 
DELETE FROM TempSiparisUretimMiktar where Kullanici =@kullanici; 
DECLARE  @turu varchar(50); 
SET @turu='select'; 
 /* */ 
 INSERT INTO TempSiparisUretimMiktar
  SELECT NEWID() AS Id,@turu, @kullanici,  
   Ur.SiparisKodu AS IsEmriKodu, Ur.IsEmriNo, UrO.Sira,UrO.ReceteKodu,UrO.ReceteAdi,UrO.OperasyonKodu,UrO.OperasyonAdi, 
   sum(IstHr.PlanlananMiktar ) AS IST_PlanlananMiktar,sum( IstHr.UretimMiktari ) AS IST_UretimMiktari, sum(IstHr.FireMiktari ) AS IST_FireMiktari,sum( IstHr.IptalMiktari  ) AS IST_IptalMiktari, 
   IstHr.UrId, IstHr.UrIId ,UrI.UrOId,   UrI.RcAId, UrI.RcOId ,UrO.SipId,UrO.SipHId 
   FROM   IstasyonTakipHareket IstHr 
   LEFT OUTER JOIN UretimIstasyon UrI ON IstHr.UrIId = UrI.Id
   LEFT OUTER JOIN UretimOperasyon UrO ON UrO.Id = UrI.UrOId
   LEFT OUTER JOIN UretimEmri Ur ON IstHr.UrId = Ur.Id 
   WHERE Ur.SipId=@sipid
   GROUP BY UrO.Sira,UrO.ReceteKodu,UrO.ReceteAdi,UrO.OperasyonKodu,UrO.OperasyonAdi, Ur.IsEmriNo,Ur.SiparisKodu,
   IstHr.UrId, IstHr.UrIId ,UrI.UrOId,   UrI.RcAId, UrI.RcOId, UrO.SipId,UrO.SipHId ; 
/* */ 
  INSERT INTO TempSiparisUretimMiktar 
  SELECT NEWID(),@turu,@kullanici,  Ur.SiparisKodu AS IsEmriKodu, Ur.IsEmriNo, UrO.Sira,UrO.ReceteKodu,UrO.ReceteAdi,UrO.OperasyonKodu,UrO.OperasyonAdi, 
  sum(UrO.PlanlananMiktar ) AS OPR_PlanlananMiktar,sum( UrO.UretimMiktari ) AS OPR_UretimMiktari, sum(UrO.FireMiktari ) AS OPR_FireMiktari,sum( UrO.IptalMiktari  ) AS OPR_IptalMiktari, 
  UrO.UrId,CAST(0x0 AS UNIQUEIDENTIFIER) AS   UrIId ,UrO.Id AS UrOId,   UrO.RcAId, UrO.RcOId ,UrO.SipId,UrO.SipHId 
  FROM    UretimOperasyon UrO 
  LEFT OUTER JOIN UretimEmri Ur ON UrO.UrId = Ur.Id 
  WHERE UrO.Id NOT IN ((SELECT UrOId FROM TempSiparisUretimMiktar )) AND Ur.SipId=@sipid
  GROUP BY  Ur.IsEmriNo,Ur.SiparisKodu  , UrO.Sira,UrO.ReceteKodu,UrO.ReceteAdi,UrO.OperasyonKodu,UrO.OperasyonAdi, 
  UrO.UrId,UrO.Id, UrO.RcAId, UrO.RcOId ,UrO.SipId,UrO.SipHId ; 
  /* */
  SET @turu='update'; 
  INSERT INTO TempSiparisUretimMiktar
  SELECT NEWID(),@turu, HR.Kullanici,   HR.IsEmriKodu,HR.IsEmriNo,999999 AS Sira,  HR.ReceteKodu,HR.ReceteAdi, 'Toplam' AS OperasyonKodu, 'Toplam' AS OperasyonAdi,
  Max(HR.PlanlananMiktar) AS PlanlananMiktar,
  (SELECT SUM(UretimMiktari) FROM TempSiparisUretimMiktar WHERE Kullanici=@kullanici AND  SipId=@sipid and SipHId=  HR.SipHId   AND  Sira = 
  (SELECT MAX(sira) FROM TempSiparisUretimMiktar WHERE Kullanici=@kullanici AND  SipId=@sipid AND SipHId =  HR.SipHId )) AS UretimMiktari,
  sum( HR.FireMiktari) AS FireMiktari,sum(HR.IptalMiktari) AS IptalMiktari,
  HR.UrId,CAST(0x0 AS UNIQUEIDENTIFIER) AS   UrIId,CAST(0x0 AS UNIQUEIDENTIFIER) AS   UrOId ,    HR.RcAId,CAST(0x0 AS UNIQUEIDENTIFIER) AS   RcOId, HR.SipId,HR.SipHId 
  FROM TempSiparisUretimMiktar HR 
  WHERE HR.Kullanici=@kullanici and HR.SipId=@sipid  
  GROUP BY  HR.Kullanici,  HR.IsEmriKodu,HR.IsEmriNo,  HR.ReceteKodu,HR.ReceteAdi,  HR.UrId,    HR.RcAId, HR.SipId,HR.SipHId  
  ORDER BY  HR.SipId, HR.SipHId  
  /* */ 
  MERGE SiparisHareket AS SHR 
  USING(SELECT * FROM TempSiparisUretimMiktar WHERE  Kullanici=@kullanici AND  SipId=@sipid AND Turu=@turu  ) AS Tmp   ON SHR.Id=Tmp.SipHId 
  WHEN MATCHED THEN
  UPDATE SET    SHR.UretimMiktari = Tmp.UretimMiktari,  SHR.FireMiktari   = Tmp.FireMiktari,  SHR.IptalMiktari  = Tmp.IptalMiktari  ; 
  /* SELECT SipH.* FROM SiparisHareket SipH      LEFT OUTER JOIN Siparis Sip ON SipH.SipId=Sip.Id   WHERE  Sip.Id=@sipid */ 
   SELECT HR.Id,Hr.Turu,HR.Kullanici,   
   HR.IsEmriKodu,HR.IsEmriNo, HR.Sira,HR.ReceteKodu,HR.ReceteAdi,HR.OperasyonKodu,HR.OperasyonAdi,  
   SipH.Miktar AS SipMiktar ,
   HR.PlanlananMiktar,HR.UretimMiktari,HR.FireMiktari,HR.IptalMiktari,
   HR.UrId,HR.UrIId ,HR.UrOId,   HR.RcAId, HR.RcOId ,HR.SipId,HR.SipHId 
   FROM TempSiparisUretimMiktar HR 
   LEFT OUTER JOIN SiparisHareket SipH ON SipH.Id=HR.SipHId 
   WHERE HR.Kullanici=@kullanici and HR.SipId=@sipid 
   ORDER BY  HR.SipId, HR.SipHId, HR.Sira   
  END";
            var rs = Query<TempSiparisUretimMiktar>(sql, null);
            return rs;
        }

        public IDataResult<IEnumerable<TempSiparisUretimMiktar>> GetTempSiparisUretimMiktarBySipKodu(string sipkodu, string kullanici) {
            string sql = $@"
BEGIN  
DECLARE  @kullanici varchar(2000);
DECLARE  @sipkodu varchar(50);
SET @kullanici='{kullanici}'; 
SET @sipkodu='{sipkodu}'; 
  DELETE FROM TempSiparisUretimMiktar where Kullanici =@kullanici;  
 DECLARE  @turu varchar(50);
 SET @turu='select'; 
 /* */ 
  INSERT INTO TempSiparisUretimMiktar
  SELECT NEWID() AS Id,@turu, @kullanici,  
  Ur.SiparisKodu AS IsEmriKodu, Ur.IsEmriNo, UrO.Sira,UrO.ReceteKodu,UrO.ReceteAdi,UrO.OperasyonKodu,UrO.OperasyonAdi, 
   sum(IstHr.PlanlananMiktar ) AS IST_PlanlananMiktar,sum( IstHr.UretimMiktari ) AS IST_UretimMiktari, sum(IstHr.FireMiktari ) AS IST_FireMiktari,sum( IstHr.IptalMiktari  ) AS IST_IptalMiktari, 
   IstHr.UrId, IstHr.UrIId ,UrI.UrOId,   UrI.RcAId, UrI.RcOId ,UrO.SipId,UrO.SipHId 
   FROM   IstasyonTakipHareket IstHr 
   LEFT OUTER JOIN UretimIstasyon UrI ON IstHr.UrIId = UrI.Id
   LEFT OUTER JOIN UretimOperasyon UrO ON UrO.Id = UrI.UrOId
   LEFT OUTER JOIN UretimEmri Ur ON IstHr.UrId = Ur.Id 
   WHERE Ur.SiparisKodu=@sipkodu
   GROUP BY UrO.Sira,UrO.ReceteKodu,UrO.ReceteAdi,UrO.OperasyonKodu,UrO.OperasyonAdi, Ur.IsEmriNo,Ur.SiparisKodu,
   IstHr.UrId, IstHr.UrIId ,UrI.UrOId,   UrI.RcAId, UrI.RcOId, UrO.SipId,UrO.SipHId ; 
  /* */ 
  INSERT INTO TempSiparisUretimMiktar 
  SELECT NEWID(),@turu,@kullanici,  Ur.SiparisKodu AS IsEmriKodu, Ur.IsEmriNo, UrO.Sira,UrO.ReceteKodu,UrO.ReceteAdi,UrO.OperasyonKodu,UrO.OperasyonAdi, 
  sum(UrO.PlanlananMiktar ) AS OPR_PlanlananMiktar,sum( UrO.UretimMiktari ) AS OPR_UretimMiktari, sum(UrO.FireMiktari ) AS OPR_FireMiktari,sum( UrO.IptalMiktari  ) AS OPR_IptalMiktari, 
  UrO.UrId,CAST(0x0 AS UNIQUEIDENTIFIER) AS   UrIId ,UrO.Id AS UrOId,   UrO.RcAId, UrO.RcOId ,UrO.SipId,UrO.SipHId 
  FROM    UretimOperasyon UrO 
  LEFT OUTER JOIN UretimEmri Ur ON UrO.UrId = Ur.Id 
  WHERE UrO.Id NOT IN ((SELECT UrOId FROM TempSiparisUretimMiktar )) AND Ur.SiparisKodu=@sipkodu 
  GROUP BY  Ur.IsEmriNo,Ur.SiparisKodu  , UrO.Sira,UrO.ReceteKodu,UrO.ReceteAdi,UrO.OperasyonKodu,UrO.OperasyonAdi, 
  UrO.UrId,UrO.Id, UrO.RcAId, UrO.RcOId ,UrO.SipId,UrO.SipHId ; 
  /* */
  SET @turu='update'; 
  INSERT INTO TempSiparisUretimMiktar
  SELECT NEWID(),@turu, HR.Kullanici,   HR.IsEmriKodu,HR.IsEmriNo,999999 AS Sira,  HR.ReceteKodu,HR.ReceteAdi, 'Toplam' AS OperasyonKodu, 'Toplam' AS OperasyonAdi,
  Max(HR.PlanlananMiktar) AS PlanlananMiktar,
  (SELECT SUM(UretimMiktari) FROM TempSiparisUretimMiktar WHERE Kullanici=@kullanici AND  IsEmriKodu=@sipkodu and  SipHId=  HR.SipHId   AND  Sira= (SELECT MAX(sira) FROM TempSiparisUretimMiktar WHERE SipHId=  HR.SipHId )) AS UretimMiktari,
  sum( HR.FireMiktari) AS FireMiktari,sum(HR.IptalMiktari) AS IptalMiktari,
  HR.UrId,CAST(0x0 AS UNIQUEIDENTIFIER) AS   UrIId,CAST(0x0 AS UNIQUEIDENTIFIER) AS   UrOId ,    HR.RcAId,CAST(0x0 AS UNIQUEIDENTIFIER) AS   RcOId, HR.SipId,HR.SipHId 
  FROM TempSiparisUretimMiktar HR 
  WHERE HR.Kullanici=@kullanici and HR.IsEmriKodu=@sipkodu  
  GROUP BY  HR.Kullanici,  HR.IsEmriKodu,HR.IsEmriNo,  HR.ReceteKodu,HR.ReceteAdi,  HR.UrId,    HR.RcAId, HR.SipId,HR.SipHId  
  ORDER BY  HR.SipId, HR.SipHId  
 /* */ 
  MERGE SiparisHareket AS SHR 
  USING(SELECT * FROM TempSiparisUretimMiktar WHERE  Kullanici=@kullanici AND  IsEmriKodu=@sipkodu AND Turu=@turu  ) AS Tmp   ON SHR.Id=Tmp.SipHId 
  WHEN MATCHED THEN
  UPDATE SET    SHR.UretimMiktari = Tmp.UretimMiktari,  SHR.FireMiktari   = Tmp.FireMiktari,  SHR.IptalMiktari  = Tmp.IptalMiktari  ; 
  /* SELECT SipH.* FROM SiparisHareket SipH     LEFT OUTER JOIN Siparis Sip ON SipH.SipId=Sip.Id    WHERE  Sip.SiparisKodu=@sipkodu */ 
   SELECT HR.Id,Hr.Turu,HR.Kullanici,   
   HR.IsEmriKodu,HR.IsEmriNo, HR.Sira,HR.ReceteKodu,HR.ReceteAdi,HR.OperasyonKodu,HR.OperasyonAdi,  
   SipH.Miktar AS SipMiktar ,
   HR.PlanlananMiktar,HR.UretimMiktari,HR.FireMiktari,HR.IptalMiktari,
   HR.UrId,HR.UrIId ,HR.UrOId,   HR.RcAId, HR.RcOId ,HR.SipId,HR.SipHId 
   FROM TempSiparisUretimMiktar HR 
   LEFT OUTER JOIN SiparisHareket SipH ON SipH.Id=HR.SipHId 
   WHERE HR.Kullanici=@kullanici and HR.IsEmriKodu=@sipkodu 
   ORDER BY  HR.SipId, HR.SipHId, HR.Sira  
   END ";
            var rs = Query<TempSiparisUretimMiktar>(sql, null);
            return rs;
        }


    }
}
