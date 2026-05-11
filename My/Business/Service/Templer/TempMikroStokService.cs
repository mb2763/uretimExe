using My.Core.Data;
using My.Core.Logger;
using My.Core.Result;
using My.Entities.Templer; 
using System.Collections.Generic;
using System.Data; 

namespace My.Business.Service.Templer {
    public class TempMikroStokService : BaseServiceCore<TempMikroStok>, ITempMikroStokService {
        public TempMikroStokService(IDbConnection dbConnection, ILogManager log) : base(dbConnection, log) {

        }

      public  IDataResult<IEnumerable<TempMikroStok>>  MikroStokGuncelle(string mikroDb) {
            var sql = $@"  
  Declare @trh DateTime;  
  Declare @trhid UNIQUEIDENTIFIER;
  Declare @suan DateTime; 
  Declare @fark int;
  Declare @modul VARCHAR(50);
  DECLARE @suredakika int;
  SET @modul='TempMikroStok';
  SET @suredakika=3;

 SET @suan= GETDATE();
  Select top 1 @trhid =Id,	@trh = Tarih From  TempSonGuncelleme WHERE Modul = @modul ; 
  begin
    if (@trh is null or @trhid is null)  
     begin  
	   insert into  TempSonGuncelleme(Modul,Tarih)values (@modul,'2000-01-01');
	   Select top 1 @trhid =Id,	@trh = Tarih From  TempSonGuncelleme WHERE Modul = @modul; 
      end
    end
	SELECT @fark= DATEDIFF(MINUTE,@trh,@suan) ;

SET NOCOUNT ON;
 if	(@fark > @suredakika)
begin

MERGE TempMikroStok AS Tar 
 USING( 
 SELECT st.sto_kod AS StokKodu,st.sto_isim AS StokAdi,st.sto_cins AS CinsKodu , 
 coalesce(ST.sto_kategori_kodu,'') AS KategoriKodu, coalesce(sk.ktg_isim,'') AS KategoriAdi,
 coalesce(ST.sto_kalkon_kodu,'') AS KaliteKontrolKodu, coalesce(skk.KKon_ismi,'') AS KaliteKontrolAdi,
 coalesce(ST.sto_reyon_kodu,'') AS ReyonKodu, coalesce(ryn.ryn_ismi,'') AS ReyonAdi,
 st.sto_birim1_ad AS Birim1,st.sto_birim2_ad AS Birim2,st.sto_birim3_ad AS Birim3,st.sto_birim4_ad AS Birim4,
 ST.sto_birim1_katsayi AS Katsayi1,ST.sto_birim2_katsayi AS Katsayi2,ST.sto_birim3_katsayi AS Katsayi3,ST.sto_birim4_katsayi AS Katsayi4,
 st.sto_create_date as CreateDate,st.sto_lastup_date AS EditDate,
 sto_detay_takip AS TakipTip,
CASE  WHEN sto_detay_takip=1 THEN 'Parti'  WHEN sto_detay_takip=2 THEN 'PartiLot'   WHEN sto_detay_takip=3 THEN 'SeriNo'  ELSE '' END AS TakipTipAd , 
CASE  WHEN sto_renkDetayli=1    AND sto_bedenli_takip=0 THEN 1    WHEN sto_renkDetayli=0    AND sto_bedenli_takip=1 THEN 2 
      WHEN sto_renkDetayli=1    AND sto_bedenli_takip=1 THEN 3    ELSE 0 END AS RBTakipTip ,
CASE  WHEN sto_renkDetayli=1    AND sto_bedenli_takip=0 THEN 'Renk'      WHEN sto_renkDetayli=0    AND sto_bedenli_takip=1 THEN 'Beden' 
      WHEN sto_renkDetayli=1    AND sto_bedenli_takip=1 THEN 'RenkBeden' ELSE '' END AS RBTakipTipAd  ,
                 {mikroDb}.dbo.fn_StokCins(sto_cins) AS Cinsi 
            FROM {mikroDb}.dbo.STOKLAR ST  WITH (NOLOCK)
 left OUTER JOIN {mikroDb}.dbo.STOK_KATEGORILERI sk  WITH (NOLOCK) ON sk.ktg_kod = ST.sto_kategori_kodu 
 left OUTER JOIN {mikroDb}.dbo.STOK_REYONLARI ryn  WITH (NOLOCK) ON ST.sto_reyon_kodu = ryn.ryn_kod
 left OUTER JOIN {mikroDb}.dbo.STOK_KALITE_KONTROL_TANIMLARI skk  WITH (NOLOCK) ON skk.KKon_kod = ST.sto_kalkon_kodu 
 ) AS Tmp ON (Tar.StokKodu=Tmp.StokKodu )
 WHEN NOT MATCHED BY TARGET 
 THEN INSERT ( StokKodu ,StokAdi ,CinsKodu ,Cinsi ,KategoriKodu ,KategoriAdi ,KaliteKontrolKodu,KaliteKontrolAdi,ReyonKodu,ReyonAdi,Birim1 ,Birim2 ,Birim3 ,Birim4 ,
 Katsayi1 ,Katsayi2 ,Katsayi3 ,Katsayi4 ,CreateDate ,EditDate,TakipTip,TakipTipAd,RBTakipTip,RBTakipTipAd) 
 VALUES(Tmp.StokKodu ,Tmp.StokAdi ,Tmp.CinsKodu ,Tmp.Cinsi ,Tmp.KategoriKodu ,Tmp.KategoriAdi ,Tmp.KaliteKontrolKodu ,Tmp.KaliteKontrolAdi ,Tmp.ReyonKodu ,Tmp.ReyonAdi ,
 Tmp.Birim1 ,Tmp.Birim2 ,Tmp.Birim3 ,Tmp.Birim4 ,
 Tmp.Katsayi1 ,Tmp.Katsayi2 ,Tmp.Katsayi3 ,Tmp.Katsayi4 ,Tmp.CreateDate ,Tmp.EditDate, Tmp.TakipTip  ,TakipTipAd, Tmp.RBTakipTip ,RBTakipTipAd) 
 WHEN MATCHED AND Tar.StokKodu = Tmp.StokKodu AND  (Tar.EditDate <> Tmp.EditDate  ) 
 THEN UPDATE SET StokKodu = Tmp.StokKodu,StokAdi = Tmp.StokAdi ,CinsKodu = Tmp.CinsKodu ,Cinsi = Tmp.Cinsi 
 ,KategoriKodu = Tmp.KategoriKodu   ,KategoriAdi = Tmp.KategoriAdi 
 ,KaliteKontrolKodu = Tmp.KaliteKontrolKodu   ,KaliteKontrolAdi = Tmp.KaliteKontrolAdi 
 ,ReyonKodu = Tmp.ReyonKodu   ,ReyonAdi = Tmp.ReyonAdi 
 ,Birim1 = Tmp.Birim1 ,Birim2 = Tmp.Birim2 ,Birim3 = Tmp.Birim3 ,Birim4 = Tmp.Birim4 
 ,Katsayi1 = Tmp.Katsayi1 ,Katsayi2 = Tmp.Katsayi2 ,Katsayi3 = Tmp.Katsayi3 ,Katsayi4 = Tmp.Katsayi4  
 ,CreateDate = Tmp.CreateDate ,EditDate = Tmp.EditDate ,TakipTip=Tmp.TakipTip ,TakipTipAd=Tmp.TakipTipAd 
 ,RBTakipTip=Tmp.RBTakipTip,RBTakipTipAd=Tmp.RBTakipTipAd 
 
 WHEN NOT MATCHED BY SOURCE  THEN UPDATE SET 
 KategoriKodu = ''   ,KategoriAdi =''  ,KaliteKontrolKodu = ''   ,KaliteKontrolAdi = ''  ,ReyonKodu = ''   ,ReyonAdi = ''  ;   
 
 update TempSonGuncelleme Set Tarih=@suan where Id=@trhid;

END

";

            var rs = Query<TempMikroStok>(sql,null);
            return rs;

        }

        public IDataResult<IEnumerable<TempMikroStokKategori>> MikroStokKategoriGuncelle(string mikroDb) {
            var sql = $@" 
  Declare @trh DateTime;  
  Declare @trhid UNIQUEIDENTIFIER;
  Declare @suan DateTime; 
  Declare @fark int;
  Declare @modul VARCHAR(50);
  DECLARE @suredakika int;
  SET @modul='TempMikroStokKategori';
  SET @suredakika=1;

 SET @suan= GETDATE();
  Select top 1 @trhid =Id,	@trh = Tarih From  TempSonGuncelleme WHERE Modul = @modul ; 
  begin
    if (@trh is null or @trhid is null)  
     begin  
	   insert into  TempSonGuncelleme(Modul,Tarih)values (@modul,'2000-01-01');
	   Select top 1 @trhid =Id,	@trh = Tarih From  TempSonGuncelleme WHERE Modul = @modul; 
      end
    end
	SELECT @fark= DATEDIFF(MINUTE,@trh,@suan) ;

SET NOCOUNT ON;
 if	(@fark > @suredakika)
begin


 MERGE  TempMikroStokKategori AS Tar 
 USING(SELECT ktg_Guid AS Id,'STOK_KATEGORI' AS Turu, ktg_kod AS KategoriKodu, ktg_isim AS KategoriAdi,
 ktg_create_date as CreateDate, ktg_lastup_date AS EditDate from {mikroDb}.dbo.STOK_KATEGORILERI ) AS Tmp 
 ON (Tar.Id=Tmp.Id )
 WHEN NOT MATCHED BY TARGET 
 THEN INSERT (Id ,Turu,KategoriKodu,KategoriAdi,CreateDate, EditDate) 
 VALUES(Tmp.Id,Tmp.Turu,Tmp.KategoriKodu,Tmp.KategoriAdi,Tmp.CreateDate,Tmp.EditDate ) 
 WHEN MATCHED AND Tar.EditDate <> Tmp.EditDate AND Tar.KategoriKodu = Tmp.KategoriKodu
 THEN UPDATE SET Tar.KategoriKodu = Tmp.KategoriKodu ,Tar.KategoriAdi = Tmp.KategoriAdi ,Tar.EditDate = Tmp.EditDate ;  

 MERGE  TempMikroStokKategori AS Tar 
 USING(SELECT KKon_Guid AS Id,'KALITE_KONTROL' AS Turu, KKon_kod AS KategoriKodu, KKon_ismi AS KategoriAdi,
 KKon_create_date as CreateDate, KKon_lastup_date AS EditDate from {mikroDb}.dbo.STOK_KALITE_KONTROL_TANIMLARI ) AS Tmp 
 ON (Tar.Id=Tmp.Id )
 WHEN NOT MATCHED BY TARGET 
 THEN INSERT (Id ,Turu,KategoriKodu,KategoriAdi,CreateDate, EditDate) 
 VALUES(Tmp.Id,Tmp.Turu,Tmp.KategoriKodu,Tmp.KategoriAdi,Tmp.CreateDate,Tmp.EditDate ) 
 WHEN MATCHED AND Tar.EditDate <> Tmp.EditDate AND Tar.KategoriKodu = Tmp.KategoriKodu
 THEN UPDATE SET Tar.KategoriKodu = Tmp.KategoriKodu ,Tar.KategoriAdi = Tmp.KategoriAdi ,Tar.EditDate = Tmp.EditDate ; 

MERGE  TempMikroStokKategori AS Tar 
USING(SELECT ryn_Guid AS Id,'STOK_REYONLAR' AS Turu, ryn_kod AS KategoriKodu, ryn_ismi AS KategoriAdi,
ryn_create_date as CreateDate, ryn_lastup_date AS EditDate from {mikroDb}.dbo.STOK_REYONLARI ) AS Tmp 
ON (Tar.Id=Tmp.Id )
WHEN NOT MATCHED BY TARGET 
THEN INSERT (Id ,Turu,KategoriKodu,KategoriAdi,CreateDate, EditDate) 
VALUES(Tmp.Id,Tmp.Turu,Tmp.KategoriKodu,Tmp.KategoriAdi,Tmp.CreateDate,Tmp.EditDate ) 
WHEN MATCHED AND Tar.EditDate <> Tmp.EditDate AND Tar.KategoriKodu = Tmp.KategoriKodu
THEN UPDATE SET Tar.KategoriKodu = Tmp.KategoriKodu ,Tar.KategoriAdi = Tmp.KategoriAdi ,Tar.EditDate = Tmp.EditDate ;

 update TempSonGuncelleme Set Tarih=@suan where Id=@trhid;

END

";

            var rs = Query<TempMikroStokKategori>(sql, null);
            return rs;

        } 
        public IDataResult<IEnumerable<TempMikroStokKategori>> GetStokKategoriListKaliteKontrol( ) {
            var sql = $@" Select * From  TempMikroStokKategori  where Turu='KALITE_KONTROL' order by KategoriAdi  ";

            var rs = Query<TempMikroStokKategori>(sql, null);
            return rs;

        }
        public IDataResult<IEnumerable<TempMikroStokKategori>> GetStokKategoriListStokKategori() {
            var sql = $@" Select * From  TempMikroStokKategori  where Turu='STOK_KATEGORI'  order by KategoriAdi ";

            var rs = Query<TempMikroStokKategori>(sql, null);
            return rs;

        }
        public IDataResult<IEnumerable<TempMikroStokKategori>> GetStokReyonList () {
            var sql = $@" Select * From  TempMikroStokKategori  where Turu='STOK_REYONLAR'  order by KategoriAdi ";

            var rs = Query<TempMikroStokKategori>(sql, null);
            return rs;

        }
    }
}
