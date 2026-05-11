
CREATE OR ALTER FUNCTION dbo.fn_by_Stok_Son5_Giris_Fiyati
(
       @StokKodu NVARCHAR (25),
       @ReferTarih DATETIME, 
       @ReturType TINYINT /*0:Son giriş fiyatı, 1: Ortalama giriş fiyatı , 2:Sayim Fisi Fiyat ,3:Devir Giriş Fiyati, 4:Standart Maliyet Fiyati   */ 
	   
)
RETURNS FLOAT
AS
BEGIN
DECLARE @Deger FLOAT,   @Miktar FLOAT, @RetVAL FLOAT=0.0  


IF @ReturType=0
BEGIN
SELECT TOP 1 @Deger= dbo.fn_StokHareketNetDeger(SH.sth_tutar, SH.sth_iskonto1, SH.sth_iskonto2, SH.sth_iskonto3, SH.sth_iskonto4, SH.sth_iskonto5, SH.sth_iskonto6, SH.sth_masraf1, SH.sth_masraf2, SH.sth_masraf3, SH.sth_masraf4, 
                         SH.sth_otvtutari, SH.sth_oivtutari, SH.sth_tip, 0, SH.sth_har_doviz_kuru, SH.sth_alt_doviz_kuru, SH.sth_stok_doviz_kuru) , 
						 @Miktar=sth_miktar FROM STOK_HAREKETLERI SH WITH (NOLOCK) 
						 WHERE SH.sth_tip = 0  AND  sth_cins NOT IN (9,15) AND sth_evraktip IN (3,13) AND sth_normal_iade=0 AND  sth_stok_kod=@StokKodu AND   sth_miktar > 0 AND
					       (@ReferTarih IS NULL OR sth_tarih<=@ReferTarih) 
ORDER BY sth_tarih DESC
END
ELSE IF @ReturType=1 
BEGIN
Select @Deger=SUM(coalesce(Tutar,0)),@Miktar=SUM(coalesce(Miktar,0)) From ( 
SELECT TOP 5  dbo.fn_StokHareketNetDeger(SH.sth_tutar, SH.sth_iskonto1, SH.sth_iskonto2, SH.sth_iskonto3, SH.sth_iskonto4, SH.sth_iskonto5, SH.sth_iskonto6, 
				SH.sth_masraf1, SH.sth_masraf2, SH.sth_masraf3, SH.sth_masraf4,  SH.sth_otvtutari, SH.sth_oivtutari, 
				SH.sth_tip, 0, SH.sth_har_doviz_kuru, SH.sth_alt_doviz_kuru, SH.sth_stok_doviz_kuru)   as Tutar,
				 sth_miktar as Miktar
FROM STOK_HAREKETLERI SH WITH (NOLOCK)
WHERE sth_tip = 0 AND    sth_cins NOT IN (9,15) AND  sth_evraktip IN (3,13) AND  sth_normal_iade=0 AND  sth_stok_kod=@StokKodu AND  sth_miktar > 0 AND
	  (@ReferTarih IS NULL OR sth_tarih<=@ReferTarih)  
	  order by sth_tarih desc)HR2
END
ELSE IF @ReturType=2 
BEGIN
Select @Deger=SUM(coalesce(Tutar,0)),@Miktar=SUM(coalesce(Miktar,0)) From ( 
SELECT TOP 1  dbo.fn_StokHareketNetDeger(SH.sth_tutar, SH.sth_iskonto1, SH.sth_iskonto2, SH.sth_iskonto3, SH.sth_iskonto4, SH.sth_iskonto5, SH.sth_iskonto6, 
				SH.sth_masraf1, SH.sth_masraf2, SH.sth_masraf3, SH.sth_masraf4,  SH.sth_otvtutari, SH.sth_oivtutari, 
				SH.sth_tip, 0, SH.sth_har_doviz_kuru, SH.sth_alt_doviz_kuru, SH.sth_stok_doviz_kuru)   as Tutar,
				 sth_miktar as Miktar
FROM STOK_HAREKETLERI SH WITH (NOLOCK)
WHERE sth_tip = 0 AND   sth_cins  IN (10) AND sth_evraktip IN (12) AND    sth_normal_iade=0 AND  sth_stok_kod=@StokKodu AND  sth_miktar > 0 AND
	  (@ReferTarih IS NULL OR sth_tarih<=@ReferTarih)  
	  order by sth_tarih desc)HR3
END
ELSE IF @ReturType=3 
BEGIN
Select @Deger=SUM(coalesce(Tutar,0)),@Miktar=SUM(coalesce(Miktar,0)) From ( 
SELECT TOP 1  dbo.fn_StokHareketNetDeger(SH.sth_tutar, SH.sth_iskonto1, SH.sth_iskonto2, SH.sth_iskonto3, SH.sth_iskonto4, SH.sth_iskonto5, SH.sth_iskonto6, 
				SH.sth_masraf1, SH.sth_masraf2, SH.sth_masraf3, SH.sth_masraf4,  SH.sth_otvtutari, SH.sth_oivtutari, 
				SH.sth_tip, 0, SH.sth_har_doviz_kuru, SH.sth_alt_doviz_kuru, SH.sth_stok_doviz_kuru)   as Tutar,
				 sth_miktar as Miktar
FROM STOK_HAREKETLERI SH WITH (NOLOCK)
WHERE  sth_tip = 0 AND  sth_cins IN (11) AND sth_evraktip IN (12) AND     sth_normal_iade=0 AND  sth_stok_kod=@StokKodu AND  sth_miktar > 0 AND
	  (@ReferTarih IS NULL OR sth_tarih<=@ReferTarih)  
	  order by sth_tarih desc)HR4
END
ELSE IF @ReturType=4 
BEGIN
Select @Deger=SUM(coalesce(Tutar,0)),@Miktar=SUM(coalesce(Miktar,0)) From ( 
SELECT TOP 1 coalesce(S.sto_standartmaliyet,0) AS Tutar ,1 AS Miktar   
FROM STOKLAR S  WITH (NOLOCK)
WHERE   S.sto_kod=@StokKodu  )HR5
END

IF @Deger IS NULL SET @Deger=0.0
IF @Miktar IS NULL SET @Miktar=0.0
IF @Miktar>0 SET @RetVAL=@Deger/@Miktar
RETURN @RetVAL
END
GO





CREATE OR ALTER FUNCTION dbo.fn_by_Stok_Son5_Cikis_Fiyati
(
       @StokKodu NVARCHAR (25), 
	     @ReferTarih  DATETIME,
       @ReturType TINYINT /*0:Son cikis fiyatı, 1: Ortalama cikis fiyatı ,2: */
)
RETURNS FLOAT
AS
BEGIN
DECLARE @Deger FLOAT,
             @Miktar FLOAT,
             @RetVAL FLOAT=0.0
IF @ReturType=0
BEGIN
SELECT TOP 1   @Deger= dbo.fn_StokHareketNetDeger(SH.sth_tutar, SH.sth_iskonto1, SH.sth_iskonto2, SH.sth_iskonto3, SH.sth_iskonto4, SH.sth_iskonto5, SH.sth_iskonto6, 
			      SH.sth_masraf1, SH.sth_masraf2, SH.sth_masraf3, SH.sth_masraf4,   SH.sth_otvtutari, SH.sth_oivtutari, 
				  SH.sth_tip, 0, SH.sth_har_doviz_kuru, SH.sth_alt_doviz_kuru, SH.sth_stok_doviz_kuru) , 
			   @Miktar=sth_miktar FROM STOK_HAREKETLERI SH WITH (NOLOCK) 
		     WHERE sth_evraktip IN (1,4) AND  sth_cins NOT IN (9,15) AND sth_normal_iade=0 AND  sth_stok_kod=@StokKodu AND    sth_miktar > 0 AND
				   (@ReferTarih IS NULL OR sth_tarih<=@ReferTarih) 
ORDER BY sth_tarih DESC
END
ELSE  
BEGIN


Select @Deger=SUM(coalesce(Tutar,0)),@Miktar=SUM(coalesce(Miktar,0)) From ( 
SELECT TOP 5 dbo.fn_StokHareketNetDeger(SH.sth_tutar, SH.sth_iskonto1, SH.sth_iskonto2, SH.sth_iskonto3, SH.sth_iskonto4, SH.sth_iskonto5, SH.sth_iskonto6, 
				SH.sth_masraf1, SH.sth_masraf2, SH.sth_masraf3, SH.sth_masraf4,  SH.sth_otvtutari, SH.sth_oivtutari, 
				SH.sth_tip, 0, SH.sth_har_doviz_kuru, SH.sth_alt_doviz_kuru, SH.sth_stok_doviz_kuru)   as Tutar,
				 sth_miktar as Miktar
FROM STOK_HAREKETLERI SH WITH (NOLOCK)
WHERE sth_evraktip IN (1,4) AND   sth_cins NOT IN (9,15) AND    sth_normal_iade=0 AND  sth_stok_kod=@StokKodu AND  sth_miktar > 0 AND
	  (@ReferTarih IS NULL OR sth_tarih<=@ReferTarih) 
	  order by sth_tarih desc ) HR2
	   
END
IF @Deger IS NULL SET @Deger=0.0
IF @Miktar IS NULL SET @Miktar=0.0
IF @Miktar>0 SET @RetVAL=@Deger/@Miktar
RETURN @RetVAL
END

GO 
 
  
 CREATE   NONCLUSTERED INDEX By_IDX_StokFiyatListe
 ON [dbo].[STOK_SATIS_FIYAT_LISTELERI] ([sfiyat_listesirano],[sfiyat_deposirano])
 INCLUDE ([sfiyat_stokkod],[sfiyat_odemeplan],[sfiyat_fiyati],[sfiyat_doviz]) 
 
 go

 