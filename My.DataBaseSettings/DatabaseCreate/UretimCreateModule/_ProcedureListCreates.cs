using System;
using System.Collections.Generic; 
namespace My.DatabaseSettings.DatabaseCreate.UretimCreateModule
{
    public static class _ProcedureListCreates
    {
        public static void Procedure_Create(this List<string> list)
        { 

             list.Uretim_MiktarGuncelle();
             list.Uretim_DurumGuncelle();
             list.Uretim_PlanlananGuncelle(); 
             list.Uretim_SonrakiIstasyonaGonder(); 
             list.Siparis_Sil_Kontrol();
             list.Recete_Sil_Kontrol();
             list.Operasyon_Sil_Kontrol();
             list.Istasyon_Sil_Kontrol();
             list.ReceteOperasyon_Tablo_AdGuncelle();
             list.ReceteIstasyon_Tablo_AdGuncelle();
             list.ReceteAna_Tablo_AdGuncelle();
             
        }

        /*---------------------*/
       
        static void Uretim_MiktarGuncelle(this List<string> list)
        {
            string sql = @" 
      CREATE OR ALTER PROCEDURE [dbo].[Uretim_MiktarGuncelle]  
         @urid    uniqueidentifier   
            AS   
         BEGIN  SET NOCOUNT ON;  
			  begin  UPDATE UrI SET  UrI.UretimMiktari=HRK.UretimMiktari,UrI.FireMiktari=HRK.FireMiktari,UrI.IptalMiktari=HRK.IptalMiktari FROM UretimIstasyon UrI INNER JOIN (
			  SELECT UrI1.Id as UrIId, sum(coalesce( UrIH1.UretimMiktari,0 ))as UretimMiktari , sum( coalesce( UrIH1.FireMiktari,0 ))as FireMiktari   , sum( coalesce( UrIH1.IptalMiktari,0 ))as IptalMiktari        
			   FROM  UretimIstasyon UrI1 left outer join   UretimIstasyonHareket UrIH1  on UrI1.Id =UrIH1.UrIId 
			   where UrI1.UrId =@urid  group by  UrI1.Id 
			  ) HRK ON UrI.Id = HRK.UrIId  ;
			  end
			  begin   UPDATE UrOHD SET UrOHD.UretimMiktari=HRK.UretimMiktari,UrOHD.FireMiktari=HRK.FireMiktari,UrOHD.IptalMiktari=HRK.IptalMiktari FROM UretimOperasyonHareketDetay UrOHD INNER JOIN ( 
			  SELECT UrOHD1.Id as UrOHDId,  sum(coalesce( UrIH1.UretimMiktari,0 ))as  UretimMiktari , sum( coalesce( UrIH1.FireMiktari,0 ))as FireMiktari  , sum( coalesce( UrIH1.IptalMiktari,0 ))as IptalMiktari      
			  FROM UretimOperasyonHareketDetay UrOHD1 left outer join   UretimIstasyonHareket UrIH1 on UrOHD1.Id =UrIH1.UrOHDId 
			  where UrOHD1.UrId = @urid  group by  UrOHD1.Id 
			  ) HRK ON UrOHD.Id = HRK.UrOHDId  ;
			  end
			  begin  UPDATE UrOH SET  UrOH.IslemdekiMiktar=HRK.IslemdekiMiktar, UrOH.UretimMiktari=HRK.UretimMiktari,UrOH.FireMiktari=HRK.FireMiktari,UrOH.IptalMiktari=HRK.IptalMiktari FROM UretimOperasyonHareket UrOH INNER JOIN (
			   SELECT UrOH1.Id as UrOHId,sum(coalesce( UrOHD1.IslemdekiMiktar,0 ))as IslemdekiMiktar , sum(coalesce( UrOHD1.UretimMiktari,0 ))as UretimMiktari , sum( coalesce( UrOHD1.FireMiktari,0 ))as FireMiktari,    
          sum( coalesce( UrOHD1.IptalMiktari,0 ))as IptalMiktari  
			   FROM UretimOperasyonHareket UrOH1  left outer join UretimOperasyonHareketDetay UrOHD1  on UrOH1.Id =UrOHD1.UrOHId  
			   where UrOH1.UrId = @urid  group by  UrOH1.Id 
			   ) HRK ON UrOH.Id = HRK.UrOHId  ;
			  END
      
			  begin  UPDATE UrO SET   UrO.UretimMiktari=HRK.UretimMiktari,UrO.FireMiktari=HRK.FireMiktari,UrO.IptalMiktari=HRK.IptalMiktari FROM UretimOperasyon UrO INNER JOIN (
			  SELECT UrO1.Id as UrOId,  sum(coalesce( UrOH1.UretimMiktari,0 ))as UretimMiktari , sum( coalesce( UrOH1.FireMiktari,0 ))as FireMiktari , sum( coalesce( UrOH1.IptalMiktari,0 ))as IptalMiktari      
			   FROM UretimOperasyon UrO1 left outer join  UretimOperasyonHareket UrOH1   on UrO1.Id =UrOH1.UrOId  
			   where UrO1.UrId = @urid  group by  UrO1.Id
			  ) HRK ON UrO.Id = HRK.UrOId  ;
			  END
        BEGIN 
        MERGE UretimOperasyon AS OPR 
        USING(SELECT UR.Id,   coalesce((SELECT UretimMiktari FROM UretimOperasyon WHERE SipHId=UR.SipHId AND Sira = UR.Sira-1),UR.PlanlananMiktar) AS PlanlananMiktar 
        FROM   dbo.UretimOperasyon UR WHERE UR.Sira > 1 AND UR.UrId=@urid  ) AS Tmp   ON OPR.Id=Tmp.Id 
        WHEN MATCHED THEN
        UPDATE SET    OPR.PlanlananMiktar = Tmp.PlanlananMiktar   ; 
        END
	      begin UPDATE UretimOperasyonHareket set  KalanMiktar=coalesce( PlanlananMiktar,0 )-(coalesce(UretimMiktari,0 )+coalesce(IptalMiktari,0 )+coalesce(FireMiktari,0 ))   where UrId = @urid    end
	      begin UPDATE UretimOperasyon        set  KalanMiktar=coalesce( PlanlananMiktar,0 )-(coalesce(UretimMiktari,0 )+coalesce(IptalMiktari,0 )+coalesce(FireMiktari,0 ))   where UrId = @urid    end 
           
           begin UPDATE UO SET  UO.Durumu=HRK.Durumu FROM UretimOperasyon UO INNER JOIN ( 
				  Select UO1.Id as UrOId,IIF(Hr1.PlanlananMiktar > 0 and Hr1.KalanMiktar <=0, 'Hazir', IIF(Hr1.IslemdekiMiktar > 0, 'Uretimde','Beklemede') ) as Durumu FROM
			   	 UretimOperasyon UO1 left outer join 
				 (  select UrOId,sum(coalesce( UrOH1.PlanlananMiktar,0 ))as PlanlananMiktar , sum(coalesce( UrOH1.IslemdekiMiktar,0 ))as IslemdekiMiktar ,   sum(coalesce( UrOH1.UretimMiktari,0 ))as UretimMiktari  , sum(coalesce( UrOH1.KalanMiktar,0 ))as KalanMiktar 
			     From UretimOperasyonHareket UrOH1 where UrId=@urid  group By UrOId)  Hr1 on UO1.Id=Hr1.UrOId WHERE UO1.UrId=@urid
				) HRK ON UO.Id = HRK.UrOId 
				end  
			
      begin UPDATE ur SET  ur.Durumu=HRK.Durumu FROM UretimEmri ur INNER JOIN (
				  select TOP 1 UrId,Durumu From (select UrId,Durumu ,CASE Durumu WHEN 'Uretimde' THEN 2 WHEN 'Hazir' THEN 3 ELSE 1  END as DurumKontrol 
			      From UretimOperasyon where UrId=@urid group By Durumu,UrId) H order By DurumKontrol 
				 ) HRK ON ur.Id = HRK.UrId END
         
         begin UPDATE sip SET  sip.Durumu=HRK.Durumu FROM Siparis sip INNER JOIN (
				 select TOP 1 SipId,Durumu From (select SipId,Durumu From UretimEmri where Id=@urid group By Durumu,SipId) H  
				 ) HRK ON sip.Id = HRK.SipId end
              begin delete from  UretimOperasyonHareketDetay where UrId=@urid  and PlanlananMiktar <= 0 and UretimMiktari <= 0 end
		      END  
          ";
            list.Add(sql);
        }
        static void Uretim_DurumGuncelle(this List<string> list) {
            string sql = @" 
      CREATE OR ALTER PROCEDURE [dbo].[Uretim_DurumGuncelle]  
         @urid    uniqueidentifier   
        AS   
        BEGIN  SET NOCOUNT ON;   
          begin UPDATE UretimOperasyonHareket set  KalanMiktar=coalesce( PlanlananMiktar,0 )-(coalesce(UretimMiktari,0 )+coalesce(IptalMiktari,0 )+coalesce(FireMiktari,0 ))   where UrId = @urid    END
	      begin UPDATE UretimOperasyon        set  KalanMiktar=coalesce( PlanlananMiktar,0 )-(coalesce(UretimMiktari,0 )+coalesce(IptalMiktari,0 )+coalesce(FireMiktari,0 ))   where UrId = @urid    END 
 
         begin UPDATE UO SET  UO.Durumu=HRK.Durumu FROM UretimOperasyon UO INNER JOIN ( 
				  Select UO1.Id as UrOId,IIF(Hr1.PlanlananMiktar > 0 and Hr1.KalanMiktar <=0, 'Hazir', IIF(Hr1.IslemdekiMiktar > 0, 'Uretimde','Beklemede') ) as Durumu FROM
			   	 UretimOperasyon UO1 left outer join 
				 (  select UrOId,sum(coalesce( UrOH1.PlanlananMiktar,0 ))as PlanlananMiktar , sum(coalesce( UrOH1.IslemdekiMiktar,0 ))as IslemdekiMiktar ,   sum(coalesce( UrOH1.UretimMiktari,0 ))as UretimMiktari  , sum(coalesce( UrOH1.KalanMiktar,0 ))as KalanMiktar 
			     From UretimOperasyonHareket UrOH1 where UrId=@urid  group By UrOId)  Hr1 on UO1.Id=Hr1.UrOId WHERE UO1.UrId=@urid
				) HRK ON UO.Id = HRK.UrOId 
				end  

			  begin UPDATE ur SET  ur.Durumu=HRK.Durumu FROM UretimEmri ur INNER JOIN (
			  select TOP 1 UrId,Durumu From (select UrId,Durumu ,CASE Durumu WHEN 'Uretimde' THEN 2 WHEN 'Hazir' THEN 3 ELSE 1  END as DurumKontrol 
			      From UretimOperasyon where UrId=@urid group By Durumu,UrId) H order By DurumKontrol 
				 ) HRK ON ur.Id = HRK.UrId end
              begin UPDATE sip SET  sip.Durumu=HRK.Durumu FROM Siparis sip INNER JOIN (
			  select TOP 1 SipId,Durumu From (select SipId,Durumu From UretimEmri where Id=@urid group By Durumu,SipId) H  
				 ) HRK ON sip.Id = HRK.SipId end
             
         begin delete from  UretimOperasyonHareketDetay where UrId=@urid  and PlanlananMiktar <= 0 and UretimMiktari <= 0 END
		    
        END       
          ";
            list.Add(sql);
        }
        static void Uretim_PlanlananGuncelle(this List<string> list)
        {
            string sql = @" 
      CREATE OR ALTER PROCEDURE dbo.Uretim_PlanlananGuncelle 
         @uridsi    uniqueidentifier   
       AS   
       BEGIN  
       BEGIN 
       SET NOCOUNT ON;  
       DECLARE @id UNIQUEIDENTIFIER , 
       @planlananMiktar float, @UretimMiktari float, @islemdekiMiktar float, @fireMiktari float, @iptalMiktari float,@kalanMiktar float,
       @urId UNIQUEIDENTIFIER,@urOId UNIQUEIDENTIFIER,  @rcaId UNIQUEIDENTIFIER , @rcoId UNIQUEIDENTIFIER ,@siphId UNIQUEIDENTIFIER ,
       @sira int  ; 
       /*  operasyon hareketlerini yoksa olustur  */
       DECLARE curs CURSOR DYNAMIC /*Cursor'a dinamik özellik kazandırıyoruz.*/ SCROLL GLOBAL FOR 
       SELECT Id,PlanlananMiktar,UretimMiktari,FireMiktari,IptalMiktari,KalanMiktar,UrId,RcAId,RcOId,Sira FROM UretimOperasyon  where UrId = @uridsi  ;  
       OPEN curs
       /* Sıradaki veri yakalanıyor ve hafızaya alınıyor. O anki veri ' Cursor' tarafından temsil ediliyor ve kolon değerleri ilgili değişkenlere sıralı bir şekilde atanıyor.*/
        FETCH NEXT FROM curs INTO @id, @planlananMiktar, @UretimMiktari, @fireMiktari,@iptalMiktari, @kalanMiktar,@urId,@rcaId,@rcoId, @sira
       /* İşlem başarılı ise @@FETCH_STATUS değişken değeri '0' ise işlem başarılıdır ve bir sonraki kayıt var demektir.*/
       WHILE @@FETCH_STATUS = 0
         BEGIN
           if(@sira=1)   
       	begin  IF NOT EXISTS(SELECT Id FROM  UretimOperasyonHareket where UrOId=@id) begin 
         INSERT INTO UretimOperasyonHareket ( PlanlananMiktar , UretimMiktari , IslemdekiMiktar ,FireMiktari ,IptalMiktari,KalanMiktar ,BaslangicTarihi ,BitisTarihi ,Sira,KayitEden,KayitTarihi ,UrId ,UrOId ,RcAId ,RcOId ,SipId  ) 
                                       select PlanlananMiktar,0,0,0,0,PlanlananMiktar, CURRENT_TIMESTAMP,null,Sira,'OtomatikKayit',CURRENT_TIMESTAMP,  UrId ,Id ,RcAId ,RcOId ,SipId  From UretimOperasyon where Id =@id;
       	end end
          if(@sira > 1)   
       	begin  IF NOT EXISTS(SELECT Id FROM  UretimOperasyonHareket where UrOId=@id) begin 
         INSERT INTO UretimOperasyonHareket 
           ( PlanlananMiktar , UretimMiktari , IslemdekiMiktar ,FireMiktari,IptalMiktari ,KalanMiktar ,BaslangicTarihi ,BitisTarihi ,Sira,KayitEden,KayitTarihi , UrId ,UrOId ,RcAId ,RcOId ,SipId  ) 
           select 0,0,0,0,0,0, CURRENT_TIMESTAMP,null,Sira,'OtomatikKayit',CURRENT_TIMESTAMP,    UrId ,Id ,RcAId ,RcOId ,SipId  From UretimOperasyon where Id =@id;
       	end end  
          /*  PRINT cast(@planlananMiktar as varchar(30)) + ' _ ' + cast(@UretimMiktari  as varchar(30)) ; */ 
          FETCH NEXT FROM curs INTO @id, @planlananMiktar, @UretimMiktari, @fireMiktari,@iptalMiktari,@kalanMiktar,@urId,@rcaId,@rcoId, @sira  /* Sıradaki veriye geçilir ve tekrardan cursor üzerinden kolon değerleri ilgili değişkenlere atanır.*/ 
          END 
       CLOSE curs/* Cursor kapatılıyor.*/ 
       DEALLOCATE curs;/* Tarafımıza ayrılan hafızayı boşaltıyoruz.*/
       /*  
       planlama miktarlarını detaydan guncelle bir oncekinin  biten adedi   sonrakinin planlanan adedi
       */
        DECLARE curs2 CURSOR    DYNAMIC /*Cursor'a dinamik özellik kazandırıyoruz.*/ SCROLL GLOBAL FOR
        SELECT Id,PlanlananMiktar,UretimMiktari,FireMiktari,IptalMiktari,KalanMiktar,UrId, RcAId,RcOId,SipHId,Sira FROM UretimOperasyon where UrId = @uridsi    ; 
        OPEN curs2
         FETCH NEXT FROM curs2 INTO @id, @planlananMiktar, @UretimMiktari, @fireMiktari,@iptalMiktari,@kalanMiktar,@urId, @rcaId,@rcoId,@siphId, @sira
        /* İşlem başarılı ise @@FETCH_STATUS değişken değeri '0' ise işlem başarılıdır ve bir sonraki kayıt var demektir.*/
        WHILE @@FETCH_STATUS = 0
         BEGIN
            if(@UretimMiktari > 0)
       	   begin IF EXISTS(SELECT Id FROM  UretimOperasyon  where  UrId = @uridsi and RcAId =@rcaId and SipHId =@siphId and Sira = @sira +1)
       	   begin   SELECT @urOId=Id FROM  UretimOperasyon  where  UrId = @uridsi and RcAId =@rcaId and SipHId =@siphId and Sira = @sira +1 ;
       	        Update UretimOperasyonHareket Set PlanlananMiktar = @UretimMiktari ,KalanMiktar=@UretimMiktari  where UrOId=@urOId;  
         	 end end
       	 if(@UretimMiktari <= 0)
       	  begin IF EXISTS(SELECT Id FROM  UretimOperasyon  where  UrId = @uridsi and RcAId =@rcaId and SipHId =@siphId and Sira = @sira +1)
       	  begin   SELECT @urOId=Id FROM  UretimOperasyon  where   UrId = @uridsi and RcAId =@rcaId and SipHId =@siphId and Sira = @sira +1 ;
       	        Update UretimOperasyonHareket Set PlanlananMiktar = 0 ,KalanMiktar=0  where UrOId=@urOId;  
       	 end end
         FETCH NEXT FROM curs2 INTO @id, @planlananMiktar, @UretimMiktari, @fireMiktari,@iptalMiktari,@kalanMiktar,@urId, @rcaId,@rcoId,@siphId, @sira  /* Sıradaki veriye geçilir ve tekrardan cursor üzerinden kolon değerleri ilgili değişkenlere atanır.*/ 
         END
        /* Cursor kapatılıyor.*/
        CLOSE curs2
        /* Tarafımıza ayrılan hafızayı boşaltıyoruz.*/
        DEALLOCATE curs2
     END
      begin EXEC  ('EXEC [Uretim_SonrakiIstasyonaGonder] '''+@uridsi+' ;'''); END
       /*  BEGIN  EXEC [Uretim_SonrakiIstasyonaGonder]  @uridsi ; END*/
      END      
       ";
            list.Add(sql);
        }
        static void Uretim_SonrakiIstasyonaGonder(this List<string> list)
        {
            string sql = @" 
      CREATE OR ALTER PROCEDURE dbo.Uretim_SonrakiIstasyonaGonder @urid1 UNIQUEIDENTIFIER
AS
BEGIN
  SET NOCOUNT ON;
  DECLARE @planlananMiktar FLOAT
         ,@islemdekiMiktar FLOAT
         ,@uretimMiktari FLOAT
         ,@fireMiktari FLOAT
         ,@iptalMiktari FLOAT
         ,@kalanMiktar FLOAT
         ,@urId UNIQUEIDENTIFIER
         ,@urOId UNIQUEIDENTIFIER
         ,@urOHId UNIQUEIDENTIFIER
         ,@urOHDId UNIQUEIDENTIFIER
         ,@rcaId UNIQUEIDENTIFIER
         ,@rcoId UNIQUEIDENTIFIER
         ,@sipId UNIQUEIDENTIFIER
         ,@siphId UNIQUEIDENTIFIER
         ,@ur_planlananMiktar FLOAT
         ,@ur_uretimMiktari FLOAT
         ,@ur_fireMiktari FLOAT
         ,@ur_iptalMiktari FLOAT
         ,@uretilecekMiktar FLOAT
         ,@istGrubu varchar(50)
         ,@operasyonKodu varchar(50)
         ,@istasyonKodu varchar(50)
         ,@istasyonAdi varchar(150)
		 ,@onceUretilenMiktari FLOAT;
  SET @urId = @urid1;
  /*  operasyon hareketlerini yoksa olustur  */
  DECLARE curs CURSOR DYNAMIC /*Cursor'a dinamik özellik kazandırıyoruz.*/ SCROLL GLOBAL FOR SELECT
    UROH.PlanlananMiktar
   ,UROH.IslemdekiMiktar
   ,UROH.UretimMiktari
   ,UROH.FireMiktari
   ,UROH.IptalMiktari
   ,UROH.KalanMiktar
   ,COALESCE(UROH.PlanlananMiktar, 0) - COALESCE(UROH.IslemdekiMiktar, 0)
   ,COALESCE(UrMik.PlanlananMiktar, 0)
   ,COALESCE(UrMik.UretimMiktari, 0)
   ,COALESCE(UrMik.FireMiktari, 0)
   ,COALESCE(UrMik.IptalMiktari, 0)
   ,UROH.UrId
   ,UROH.UrOId
   ,UROH.Id
   ,UROH.RcAId
   ,UROH.RcOId
   ,UROH.SipId
   ,UR.IstasyonGrupKodu
   ,URO.OperasyonKodu
   ,GROP.IstasyonKodu
   ,GROP.IstasyonAdi
  FROM UretimOperasyonHareket UROH
  LEFT OUTER JOIN UretimOperasyon URO  ON UROH.UrOId = URO.Id
  LEFT OUTER JOIN UretimEmri UR ON UROH.UrId = UR.Id
  LEFT OUTER JOIN ReceteIstasyonGrupOperasyon GROP ON GROP.GrupKodu = UR.IstasyonGrupKodu AND GROP.OperasyonKodu=URO.OperasyonKodu
  LEFT OUTER JOIN ((SELECT
      UI1.UrOHId
     ,SUM(COALESCE(UI1.PlanlananMiktar, 0)) AS PlanlananMiktar
     ,SUM(COALESCE(UI1.UretimMiktari, 0)) AS UretimMiktari
     ,SUM(COALESCE(UI1.FireMiktari, 0)) AS FireMiktari
     ,SUM(COALESCE(UI1.IptalMiktari, 0)) AS IptalMiktari
    FROM UretimOperasyonHareketDetay UI1
    GROUP BY UI1.UrOHId)) AS UrMik
    ON UrMik.UrOHId = UROH.Id
  LEFT OUTER JOIN ReceteAna RCA
    ON UROH.RcAId = RCA.Id
  WHERE RCA.IstasyonGruplamaKullan = 1
  AND UROH.PlanlananMiktar > 0
  AND UROH.PlanlananMiktar > UROH.IslemdekiMiktar
  AND UROH.UrId = @urId;
  OPEN curs
  /* Sıradaki veri yakalanıyor ve hafızaya alınıyor. O anki veri ' Cursor' tarafından temsil ediliyor ve kolon değerleri ilgili değişkenlere sıralı bir şekilde atanıyor.*/
  FETCH NEXT FROM curs INTO @planlananMiktar, @islemdekiMiktar, @uretimMiktari, @fireMiktari, @iptalMiktari, @kalanMiktar, @uretilecekMiktar, @ur_planlananMiktar,
  @ur_uretimMiktari, @ur_fireMiktari, @ur_iptalMiktari, @urId, @urOId, @urOHId, @rcaId, @rcoId, @sipId,@istGrubu,@operasyonKodu,@istasyonKodu,@istasyonAdi
  /* İşlem başarılı ise @@FETCH_STATUS değişken değeri '0' ise işlem başarılıdır ve bir sonraki kayıt var demektir.*/
  WHILE @@FETCH_STATUS = 0
  BEGIN
   Select @onceUretilenMiktari = coalesce(Sum(coalesce(PlanlananMiktar,0)),0) From UretimOperasyonHareketDetay where UrOHId=@urOHId;
  IF ((@uretilecekMiktar )  > 0)
  BEGIN
    SET @urOHDId = NEWID();

    INSERT INTO UretimOperasyonHareketDetay (Id, Turu, Tarih, PlanlananMiktar, IslemdekiMiktar, UretimMiktari, FireMiktari, IptalMiktari, KayitEden, UrId, UrOId, UrOHId, RcAId, RcOId, SipId)
      SELECT
        @urOHDId
       ,'Acilis'
       ,CURRENT_TIMESTAMP
       ,(@planlananMiktar - @onceUretilenMiktari)
       ,(@planlananMiktar - @onceUretilenMiktari)
       ,0
       ,0
       ,0
       ,'OtomatikKayit'
       ,@urId
       ,@urOId
       ,@urOHId
       ,@rcaId
       ,@rcoId
       ,@sipId;

    
    INSERT INTO UretimIstasyon (IstasyonKodu, IstasyonAdi, PlanlananMiktar, UretimMiktari, FireMiktari, IptalMiktari, BaslangicTarihi,Fason,
                FasonCariKodu, FasonCariUnvani, KayitEden, UrId, UrOId, UrOHId, UrOHDId, RcAId, RcOId, SipId)
      SELECT  
        @istasyonKodu
       ,@istasyonAdi
       ,(@planlananMiktar - @onceUretilenMiktari)
       ,0
       ,0
       ,0
       ,CURRENT_TIMESTAMP
       ,0,'',''
       ,'OtomatikKayit'
       ,@urId
       ,@urOId
       ,@urOHId
       ,@urOHDId
       ,@rcaId
       ,@rcoId
       ,@sipId  ;

    UPDATE UretimOperasyonHareket  SET IslemdekiMiktar = @planlananMiktar  WHERE Id = @urOHId;
  END

  IF (@uretilecekMiktar > 1
    AND @uretilecekMiktar < 0)   /* yedek kasıtlı mantık hatası yapıldı */
  BEGIN
    IF NOT EXISTS (SELECT
          Id
        FROM UretimOperasyonHareketDetay
        WHERE UrOHId = @urOHId)
    BEGIN
      INSERT INTO UretimOperasyonHareketDetay (Turu, Tarih )
        SELECT
          'Acilis'
         ,CURRENT_TIMESTAMP ;
    END
  END

  FETCH NEXT FROM curs INTO @planlananMiktar, @islemdekiMiktar, @uretimMiktari, @fireMiktari, @iptalMiktari, @kalanMiktar, @uretilecekMiktar, @ur_planlananMiktar,
  @ur_uretimMiktari, @ur_fireMiktari, @ur_iptalMiktari, @urId, @urOId, @urOHId, @rcaId, @rcoId, @sipId,@istGrubu,@operasyonKodu,@istasyonKodu,@istasyonAdi
  /* Sıradaki veriye geçilir ve tekrardan cursor üzerinden kolon değerleri ilgili değişkenlere atanır.*/
  END
  CLOSE curs/* Cursor kapatılıyor.*/
  DEALLOCATE curs;/* Ayrılan hafızayı boşaltıyoruz.*/

END 
  ";
            list.Add(sql);
        }
        /*---------------------*/
        static void ReceteOperasyon_Tablo_AdGuncelle(this List<string> list)
        {
            string sql = @" CREATE OR ALTER PROCEDURE [dbo].[ReceteOperasyon_Tablo_AdGuncelle]  
                        @id    uniqueidentifier
                        AS BEGIN SET NOCOUNT ON; 	  
                      	update UretimOperasyon       set OperasyonKodu   = (select OperasyonKodu From ReceteOperasyon R where R.Id =[UretimOperasyon].RcOId) where RcOId = @id; 
						update UretimOperasyon       set OperasyonAdi   = (select OperasyonAdi From ReceteOperasyon R where R.Id =[UretimOperasyon].RcOId) where RcOId = @id; 
						update ReceteIstasyon        set OperasyonKodu   = (select OperasyonKodu From ReceteOperasyon R where R.Id =[ReceteIstasyon].RcOId) where RcOId = @id; 
						update ReceteIstasyon        set OperasyonAdi   = (select OperasyonAdi From ReceteOperasyon R where R.Id =[ReceteIstasyon].RcOId) where RcOId = @id; 
						END";
            list.Add(sql);
        }
        static void ReceteIstasyon_Tablo_AdGuncelle(this List<string> list)
        {
            string sql = @" Create Or ALTER     PROCEDURE [dbo].[ReceteIstasyon_Tablo_AdGuncelle] 
                        @id    uniqueidentifier
                         AS BEGIN SET NOCOUNT ON; 	  
						update UretimIstasyon      set IstasyonKodu   = (select IstasyonKodu From ReceteIstasyon R where R.Id =UretimIstasyon.RcIstId) where RcIstId = @id; 
						update UretimIstasyon      set IstasyonAdi   = (select IstasyonAdi From ReceteIstasyon R where R.Id =UretimIstasyon.RcIstId) where RcIstId = @id; 
						END";
            list.Add(sql);
        }
        static void ReceteAna_Tablo_AdGuncelle(this List<string> list)
        {
            string sql = @" Create Or ALTER     PROCEDURE [dbo].[ReceteAna_Tablo_AdGuncelle]  
                            @id    uniqueidentifier
                          AS BEGIN SET NOCOUNT ON; 	  
                        update ReceteGrupDetay      set ReceteKodu   = (select ReceteKodu From ReceteAna R where R.Id =ReceteGrupDetay.RcAId) where RcAId = @id; 
						update ReceteGrupDetay      set ReceteAdi   = (select ReceteAdi From ReceteAna R where R.Id =ReceteGrupDetay.RcAId) where RcAId = @id; 						
						update UretimOperasyon      set ReceteKodu   = (select ReceteKodu From ReceteAna R where R.Id =UretimOperasyon.RcAId) where RcAId = @id; 
						update UretimOperasyon      set ReceteAdi   = (select ReceteAdi From ReceteAna R where R.Id =UretimOperasyon.RcAId) where RcAId = @id; 					 
						END";
            list.Add(sql);
        }
        /*---------------------*/
        /*---------------------*/
        static void Siparis_Sil_Kontrol(this List<string> list)
        {
            string sql = @" 
  Create Or ALTER     PROCEDURE [dbo].[Siparis_Sil_Kontrol]  
  @Id uniqueidentifier
  AS 
  declare @UrAdet int set @UrAdet=0; 
  declare @TopAdet int set @TopAdet=0;
  declare @Mesaj varchar(8000) Set @Mesaj='';
  BEGIN SET NOCOUNT ON; 	  
                select @UrAdet=Count(*)  From UretimEmri where SipId = @Id;
			IF( coalesce(@UrAdet,0) > 0 )
			begin 
			set @Mesaj = @Mesaj +'Üretim Emri';
			set @TopAdet =@TopAdet + coalesce(@UrAdet,0); 
			end 
			select @TopAdet as Adet , @Mesaj as Mesaj;			    
  END";
            list.Add(sql);
        }
        static void Recete_Sil_Kontrol(this List<string> list)
        {
            string sql = @"
  Create Or ALTER PROCEDURE [dbo].[Recete_Sil_Kontrol]  
  @Id uniqueidentifier
  AS 
  declare @UrAdet int set @UrAdet=0; 
  declare @TopAdet int set @TopAdet=0;
  declare @Mesaj varchar(8000) Set @Mesaj='';
  BEGIN SET NOCOUNT ON; 	  
            select @UrAdet=Count(*)  From UretimEmri where RcAId = @Id;
			IF( coalesce(@UrAdet,0) > 0 )
			begin 
			set @Mesaj = @Mesaj +'Üretim Emri';
			set @TopAdet =@TopAdet + coalesce(@UrAdet,0); 
			end 
			select @TopAdet as Adet , @Mesaj as Mesaj;			    
  END";
            list.Add(sql);
        }
        static void Operasyon_Sil_Kontrol(this List<string> list)
        {
            string sql = @"
  Create Or ALTER PROCEDURE [dbo].[Operasyon_Sil_Kontrol]  
  @Id uniqueidentifier
  AS 
  declare @UrAdet int set @UrAdet=0; 
  declare @TopAdet int set @TopAdet=0;
  declare @Mesaj varchar(8000) Set @Mesaj='';
  BEGIN SET NOCOUNT ON; 	  
            select @UrAdet=Count(*)  From UretimOperasyon where RcOId = @Id;
			IF( coalesce(@UrAdet,0) > 0 )
			begin 
			set @Mesaj = @Mesaj +'Uretim Operasyon';
			set @TopAdet =@TopAdet + coalesce(@UrAdet,0); 
			end 
			select @TopAdet as Adet , @Mesaj as Mesaj;			    
  END";
            list.Add(sql);
        }
        static void Istasyon_Sil_Kontrol(this List<string> list)
        {
            string sql = @"
  Create Or ALTER    PROCEDURE [dbo].[Istasyon_Sil_Kontrol]  
  @Id uniqueidentifier
  AS 
  declare @UrAdet int set @UrAdet=0; 
  declare @TopAdet int set @TopAdet=0;
  declare @Mesaj varchar(8000) Set @Mesaj='';
  BEGIN SET NOCOUNT ON; 	  
            select @UrAdet=Count(*)  From UretimIstasyon where RcIstId = @Id;
			IF( coalesce(@UrAdet,0) > 0 )
			begin 
			set @Mesaj = @Mesaj +'Uretim Istasyon';
			set @TopAdet =@TopAdet + coalesce(@UrAdet,0); 
			end 
			select @TopAdet as Adet , @Mesaj as Mesaj;			    
  END";
            list.Add(sql);
        }
         
    }
}