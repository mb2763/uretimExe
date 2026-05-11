using My.Core.Data;
using My.Core.Result;
using My.Entities.Models;
using My.Entities.Raporlar;
using My.Entities.Receteler;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace My.Business.Manager {
    public class ReceteManager {
        private readonly DatabaseFactoryPro _db;

        public ReceteManager(DatabaseFactoryPro dbPro) {
            _db = dbPro;
        }

        public ReceteKayitModel GetReceteKayit() {
            return new ReceteKayitModel();
        }
        public IDataResult<IEnumerable<ReceteDetay>> GetReceteDetayByReceteKodu(string recetekodu) {
            var recete = _db.ReceteAna.SelectFirst(c => c.ReceteKodu == recetekodu);
            if (!recete.Success) return new ErrorDataResult<IEnumerable<ReceteDetay>>(recete.Message);
            var rec = recete.Data;
            var receteDetay = _db.ReceteDatay.SelectList(c => c.RcAId == rec.Id);
            if (!receteDetay.Success) return new ErrorDataResult<IEnumerable<ReceteDetay>>(receteDetay.Message);

            return receteDetay;
        }

        public IDataResult<ReceteKayitModel> GetReceteKayit(Guid? id) {
            var mdl = new ReceteKayitModel();
            var recete = _db.ReceteAna.SelectFirst(c => c.Id == id);
            if (!recete.Success) return new ErrorDataResult<ReceteKayitModel>(recete.Message);
            mdl.Recete = recete.Data;
            var receteDetay = _db.ReceteDatay.SelectList(c => c.RcAId == id);
            if (!receteDetay.Success) return new ErrorDataResult<ReceteKayitModel>(receteDetay.Message);
            mdl.ReceteDetaylar = receteDetay.Data.ToList().OrderBy(c => c.ReceteSira).ToList();
            var receteStok = _db.ReceteStok.SelectList(c => c.RcAId == id);
            if (!receteStok.Success) return new ErrorDataResult<ReceteKayitModel>(receteStok.Message);
            mdl.ReceteStoklar = receteStok.Data.ToList();
            var receteBagIst = _db.ReceteyeBagliIstasyon.SelectList(c => c.RcAId == id);
            if (!receteBagIst.Success) return new ErrorDataResult<ReceteKayitModel>(receteBagIst.Message);
            mdl.ReceteyeBagliIstasyonlar = receteBagIst.Data.ToList();
            var receteStokRenkBedenler = _db.ReceteStokRenkBeden.SelectList(c => c.RcAId == id);
            if (!receteStokRenkBedenler.Success) return new ErrorDataResult<ReceteKayitModel>(receteStokRenkBedenler.Message);
            mdl.ReceteStokRenkBedenler = receteStokRenkBedenler.Data.ToList();
            return new SuccessDataResult<ReceteKayitModel>(mdl);
        }
        public virtual IResult ReceteStokKoduDahaonceGirilmismi(Guid? rcGuid, string entegreStokKodu) {

            var sql = $@" 
            declare @guid uniqueidentifier;
            declare @entegreStokKodu varchar(50) 
            set @guid='{rcGuid}';
            set @entegreStokKodu='{entegreStokKodu}';
            select count(*) as Adet from ReceteAna where Id <> @guid and EntegreStokKodu=@entegreStokKodu";

            var rs = _db.ReceteAna.Query<int>(sql, null);
            if (!rs.Success) return new ErrorResult(rs.Message);
            if (rs.Data.FirstOrDefault() > 0) return new ErrorResult("" + entegreStokKodu + " stok kodu ile  daha önce reçete kayıt edilmiş \naynı kod ile tekrar reçete kayıt edilemez.");
            return new SuccessResult();
        }

        public virtual IDataResult<IEnumerable<ReceteKullanilanStokModel>> GetReceteKullanilanStokList(string wheresql) {

            var sql = $@" 
   SELECT Rd.Id ,Cinsi ,ra.ReceteKodu,ra.ReceteAdi
   ,ReceteSira ,StokKullan ,StokTuru ,StokAnaGrup ,StokAltGrup
   ,VarsayilanStokKodu ,VarsayilanStokAdi ,Birim ,Renk ,Miktar
   ,Rd.Aciklama ,SiparisdeGosterme ,RcAId ,OperasyonMaliyet ,Beden
   ,Ebat ,Gram ,Olcu ,FireYuzde
   FROM ReceteDetay rd left outer join ReceteAna ra on rd.RcAId=ra.Id " + wheresql;
            var rs = _db.ReceteAna.Query<ReceteKullanilanStokModel>(sql, null);
            return rs;
        }

      public virtual IDataResult<IEnumerable<ReceteRaporuModel>> GetReceteGenelRaporuList(string wheresql) {

            var sql = $@" 
  IF OBJECT_ID('tempdb..#TempReceteRaporu') IS NOT NULL
    Truncate TABLE #TempReceteRaporu
    /*DROP TABLE #TempReceteRaporu*/
 ELSE
  CREATE TABLE #TempReceteRaporu(FisSira int, ReceteSira int, ReceteKodu NVARCHAR(150), ReceteAdi NVARCHAR(255),Cinsi int ,  CinsAdi NVARCHAR(150), 
    StokKodu  NVARCHAR(150), StokAdi NVARCHAR(255),  Birim NVARCHAR(50), 
     Miktar float,FireYuzde float, RcAId uniqueidentifier,RcDId UNIQUEIDENTIFIER )
   
 DECLARE @ReceteKodu NVARCHAR(150), @ReceteAdi NVARCHAR(255),@Cinsi int ,  @CinsAdi NVARCHAR(150), 
     @StokKodu  NVARCHAR(150), @StokAdi NVARCHAR(255),  @Birim NVARCHAR(50), 
     @Miktar float,@FireYuzde float, @RcAId uniqueidentifier,@RcDId UNIQUEIDENTIFIER,@FisSira INT 
DECLARE ReceteRaporCursor CURSOR
FOR     
SELECT  ReceteKodu, ReceteAdi,StokCinsiKodu AS Cinsi, Upper(StokCinsiAdi) AS CinsAdi,  EntegreStokKodu AS StokKodu, EntegreStokAdi AS StokAdi, EntegreBirim ,0,0,Id AS RcAId 
FROM ReceteAna {wheresql}

SET @FisSira=0;
 
OPEN ReceteRaporCursor 
FETCH NEXT FROM ReceteRaporCursor INTO  @ReceteKodu , @ReceteAdi ,@Cinsi  ,  @CinsAdi ,    @StokKodu , @StokAdi ,  @Birim ,   @Miktar ,@FireYuzde , @RcAId  
/* İşlem başarılı ise @@FETCH_STATUS değişken değeri '0' ise işlem başarılıdır ve bir sonraki kayıt var demektir. */
WHILE @@FETCH_STATUS = 0
 BEGIN
SET @FisSira = @FisSira +1;
 INSERT INTO #TempReceteRaporu (FisSira ,ReceteSira,ReceteKodu , ReceteAdi ,Cinsi , CinsAdi , StokKodu , StokAdi , Birim , Miktar ,FireYuzde , RcAId  ) 
        VALUES(@FisSira,0,@ReceteKodu , @ReceteAdi ,@Cinsi,@CinsAdi ,@StokKodu , @StokAdi ,  @Birim ,   @Miktar ,@FireYuzde , @RcAId   ) ;   
 INSERT INTO #TempReceteRaporu
  SELECT @FisSira AS FisSira,  RD.ReceteSira,'' AS ReceteKodu, '' AS ReceteAdi  ,TmpST.CinsKodu AS Cinsi,TmpST.Cinsi AS CinsAdi ,
  RD.VarsayilanStokKodu AS StokKodu, RD.VarsayilanStokAdi AS StokAdi, RD.Birim,RD.Miktar,RD.FireYuzde,RD.RcAId, RD.Id AS RcDId 
  FROM ReceteDetay RD LEFT OUTER JOIN TempMikroStok TmpST ON RD.VarsayilanStokKodu =TmpST.StokKodu 
  WHERE RcAId= @RcAId; 
 FETCH NEXT FROM ReceteRaporCursor INTO @ReceteKodu , @ReceteAdi ,@Cinsi  ,  @CinsAdi ,    @StokKodu , @StokAdi ,  @Birim ,   @Miktar ,@FireYuzde , @RcAId  
END  
CLOSE ReceteRaporCursor 
DEALLOCATE ReceteRaporCursor 
Select FisSira , ReceteSira , ReceteKodu , ReceteAdi ,Cinsi  ,  CinsAdi , 
     StokKodu , StokAdi ,  Birim , 
     Miktar ,FireYuzde ,Miktar * (1+(FireYuzde/100)) AS FireliMiktar  ,RcAId,RcDId   
FROM #TempReceteRaporu  
ORDER BY FisSira,ReceteSira
    "   ;
            var rs = _db.ReceteAna.Query<ReceteRaporuModel>(sql, null);
            return rs;
        }


        public virtual IDataResult<int> GetRafOmru(Guid? rcaId) {
            var sql = $@"    select coalesce(RafOmru,0) as RafOmru  from ReceteAna WITH (NOLOCK) where Id='{rcaId}'";
            var rs = _db.ReceteAna.QueryFirst<int>(sql, null);
            return rs;
        }
        public virtual IResult ReceteDetayFireYuzdeGuncelle(List<ReceteDetay> dty) {
            var sql = $@" UPDATE ReceteDetay SET FireYuzde=@FireYuzde where Id =@Id; ";
            var rs = _db.ReceteDatay.Execute(sql, dty);
            if (!rs.Success) return new ErrorResult(rs.Message);
            return rs;
        }

        public IDataResult<ReceteKayitModel> ReceteKaydet(ReceteKayitModel mdl, bool yenikayit) {
            var rsKod = KodVarmi(mdl.Recete, "ReceteKodu", yenikayit);
            if (!rsKod.Success) return new ErrorDataResult<ReceteKayitModel>(rsKod.Message);
            var con = _db.ReceteAna.GetConnection();
            if (con.State != ConnectionState.Open) con.Open();
            var trs = con.BeginTransaction();
            try {

                var rsAna = _db.ReceteAna.InsertOrUpdate(mdl.Recete, trs);
                if (!rsAna.Success) {
                    trs.Rollback();
                    trs.Dispose();
                    return new ErrorDataResult<ReceteKayitModel>(rsAna.Message);
                }

                /* detaylar Sil */

                var rsDIDel = _db.ReceteDatay.Delete(c => c.RcAId == mdl.Recete.Id, trs);
                if (!rsDIDel.Success) {
                    trs.Rollback();
                    trs.Dispose();
                    return new ErrorDataResult<ReceteKayitModel>(rsDIDel.Message);
                }
                /* stoklar Sil */
                var rsSIDel = _db.ReceteStok.Delete(c => c.RcAId == mdl.Recete.Id, trs);
                if (!rsSIDel.Success) {
                    trs.Rollback();
                    trs.Dispose();
                    return new ErrorDataResult<ReceteKayitModel>(rsSIDel.Message);
                }


                /* Bag Ist Sil */
                var rsBIDel = _db.ReceteyeBagliIstasyon.Delete(c => c.RcAId == mdl.Recete.Id, trs);
                if (!rsBIDel.Success) {
                    trs.Rollback();
                    trs.Dispose();
                    return new ErrorDataResult<ReceteKayitModel>(rsBIDel.Message);
                }

                /* Renk Beden Sil */
                var rsSRBDel = _db.ReceteStokRenkBeden.Delete(c => c.RcAId == mdl.Recete.Id, trs);
                if (!rsSRBDel.Success) {
                    trs.Rollback();
                    trs.Dispose();
                    return new ErrorDataResult<ReceteKayitModel>(rsSRBDel.Message);
                } 
                /* detaylar  */
                var rsDI = _db.ReceteDatay.InsertOrUpdate(mdl.ReceteDetaylar, trs);
                if (!rsDI.Success) {
                    trs.Rollback();
                    trs.Dispose();
                    return new ErrorDataResult<ReceteKayitModel>(rsDI.Message);
                }

                if (mdl.ReceteStoklar != null && mdl.ReceteStoklar.Count > 0) {
                    /* stoklar */
                    var rsSI = _db.ReceteStok.InsertOrUpdate(mdl.ReceteStoklar, trs);
                    if (!rsSI.Success) {
                        trs.Rollback();
                        trs.Dispose();
                        return new ErrorDataResult<ReceteKayitModel>(rsSI.Message);
                    }
                }

                if (mdl.ReceteyeBagliIstasyonlar != null && mdl.ReceteyeBagliIstasyonlar.Count > 0) {
                    /* Baglı istasyonlar */
                    var rsBI = _db.ReceteyeBagliIstasyon.InsertOrUpdate(mdl.ReceteyeBagliIstasyonlar, trs);
                    if (!rsBI.Success) {
                        trs.Rollback();
                        trs.Dispose();
                        return new ErrorDataResult<ReceteKayitModel>(rsBI.Message);
                    }
                }
                if (mdl.ReceteStokRenkBedenler != null && mdl.ReceteStokRenkBedenler.Count > 0) {
                    /* stok renk beden ler */
                    var rsSRBI = _db.ReceteStokRenkBeden.InsertOrUpdate(mdl.ReceteStokRenkBedenler, trs);
                    if (!rsSRBI.Success) {
                        trs.Rollback();
                        trs.Dispose();
                        return new ErrorDataResult<ReceteKayitModel>(rsSRBI.Message);
                    }
                }
                trs.Commit();
            } catch (Exception e) {
                trs.Rollback();
                trs.Dispose();
                return new ErrorDataResult<ReceteKayitModel>(e.Message);
            } finally {
                trs.Dispose();
            }

            return new SuccessDataResult<ReceteKayitModel>(mdl);
        }

        public virtual IResult KodVarmi<T>(T entity, string kontrolalan, bool yenikayitmi) {
            var tabloadi = ClassExtensions.GetClassTableName(typeof(T));
            var GetId = ClassExtensions.GetClassColumnNameKey(typeof(T));
            var sql2 = $" where  {GetId} <> @{GetId} and {kontrolalan} = @{kontrolalan} ";
            if (yenikayitmi) sql2 = $" where  {kontrolalan} = @{kontrolalan} ";
            var sql = string.Format("Select count(*) From {0}  {1};", tabloadi, sql2);
            var rs = _db.ReceteAna.Query<int>(sql, entity);
            if (!rs.Success) return new ErrorResult(rs.Message);
            if (rs.Data.FirstOrDefault() > 0) return new ErrorResult("Aynı " + kontrolalan + " Kodla Kayıt Var");
            return new SuccessResult();
        }

        public IResult ReceteSil(ReceteKayitModel mdl) {
            var rsad = _db.ReceteOperasyon.SelectList(c => c.RcAId == mdl.Recete.Id);
            if (!rsad.Success) return new ErrorResult(rsad.Message);
            if (rsad.Data != null && rsad.Data.Count() > 0)
                return new ErrorResult("Reçeteye bağlı Operasyonlar var Kayıt silinemez.");
            var con = _db.ReceteAna.GetConnection();
            if (con.State != ConnectionState.Open) con.Open();
            var trs = con.BeginTransaction();
            try {
                var rsAna = _db.ReceteAna.Delete(mdl.Recete, trs);
                if (!rsAna.Success) {
                    trs.Dispose();
                    return new ErrorResult(rsAna.Message);
                }

                /* detaylar */
                var rsDI = _db.ReceteDatay.Delete(c => c.RcAId == mdl.Recete.Id, trs);
                if (!rsDI.Success) {
                    trs.Dispose();
                    return new ErrorResult(rsDI.Message);
                }

                /* stoklar */
                var rsSI = _db.ReceteStok.Delete(c => c.RcAId == mdl.Recete.Id, trs);
                if (!rsSI.Success) {
                    trs.Dispose();
                    return new ErrorResult(rsSI.Message);
                }
                /* Bag Ist Sil */
                var rsBIDel = _db.ReceteyeBagliIstasyon.Delete(c => c.RcAId == mdl.Recete.Id, trs);
                if (!rsBIDel.Success) {
                    trs.Dispose();
                    return new ErrorResult(rsBIDel.Message);
                }

                /* Renk Beden Sil */
                var rsSRBDel = _db.ReceteStokRenkBeden.Delete(c => c.RcAId == mdl.Recete.Id, trs);
                if (!rsSRBDel.Success) {
                    trs.Dispose();
                    return new ErrorResult(rsSRBDel.Message);
                }
                trs.Commit();
            } catch (Exception e) {
                trs.Dispose();
                return new ErrorResult(e.Message);
            } finally {
                trs.Dispose();
            }

            return new SuccessResult();
        }

        public IResult ReceteSilKontrol(Guid? id) {
            var rs = _db.Siparis.QueryProc<KayitSilKontrolModel>("Recete_Sil_Kontrol", new { Id = id });
            if (!rs.Success) return new ErrorResult(rs.Message);
            var data = rs.Data.FirstOrDefault();
            if (data != null && data.Adet > 0)
                return new ErrorResult($"Kayıda bağlı  ( {data.Mesaj} ) hareketler var kayıt silinemez.");
            return new SuccessResult();
        }
    }
}