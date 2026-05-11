using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using My.Core.Data;
using My.Core.Result;
using My.Entities.Models;
using My.Entities.Raporlar;
using My.Entities.Siparisler;
using My.Entities.Templer;

namespace My.Business.Manager {
    public class SiparisManager {
        private readonly DatabaseFactoryPro _db;
        private readonly DatabaseFactoryMikro _dbMikro;

        public SiparisManager(DatabaseFactoryPro dbPro, DatabaseFactoryMikro dbMikro)
        {
            _db = dbPro;
            _dbMikro = dbMikro;
        }
        public SiparisKayitModel GetSiparis() {
            return new SiparisKayitModel();
        }

        public IDataResult<SiparisKayitModel> GetSiparis(Guid? id) {
            var mdl = new SiparisKayitModel();

            var sipSql = @"Select Top 1 * ,
                    (SELECT TOP 1 IsEmriNo FROM UretimEmri WHERE SipId=s.Id ) as IsEmriNo 
                    From Siparis S where Id='" + id + "';";

            // var sip = _db.Siparis.SelectFirst(c => c.Id == id);
            var sip = _db.Siparis.QueryFirst<Siparis>(sipSql, null); 
            if (!sip.Success) return new ErrorDataResult<SiparisKayitModel>(sip.Message);
            mdl.Siparis = sip.Data;
            var sipHr = _db.SiparisHareket.GetViewListWhere2(" where SH.SipId ='"+id+"'");
            if (!sipHr.Success) return new ErrorDataResult<SiparisKayitModel>(sipHr.Message);
            mdl.Hareketler = sipHr.Data.ToList();
            var sipDet = _db.SiparisHareketDetay.SelectList(c => c.SipId == id);
            if (!sipDet.Success) return new ErrorDataResult<SiparisKayitModel>(sipDet.Message);
            mdl.Detaylar = sipDet.Data.ToList();
            return new SuccessDataResult<SiparisKayitModel>(mdl);
        }

        public IDataResult<SiparisKayitModel> SiparisKaydet(SiparisKayitModel mdl, bool yenikayit) {
            var rsKod = KodVarmi(mdl.Siparis, "SiparisKodu", yenikayit);
            if (!rsKod.Success) return new ErrorDataResult<SiparisKayitModel>(rsKod.Message);
            var con = _db.Siparis.GetConnection();
            if (con.State != ConnectionState.Open) con.Open();
            var trs = con.BeginTransaction();
            try {
                var rsAna = _db.Siparis.InsertOrUpdate(mdl.Siparis, trs);
                if (!rsAna.Success) {
                    trs.Dispose();
                    return new ErrorDataResult<SiparisKayitModel>(rsAna.Message);
                }
                /* Hareketler  Sil */
                var rsHrDel = _db.SiparisHareket.Delete(c => c.SipId == mdl.Siparis.Id, trs);
                if (!rsHrDel.Success) {
                    trs.Dispose();
                    return new ErrorDataResult<SiparisKayitModel>(rsHrDel.Message);
                }
                /* detaylar  Sil */
                var rsDetDel = _db.SiparisHareketDetay.Delete(c => c.SipId == mdl.Siparis.Id, trs);
                if (!rsDetDel.Success) {
                    trs.Dispose();
                    return new ErrorDataResult<SiparisKayitModel>(rsDetDel.Message);
                }

                /* Hareketler */
                var rsHr = _db.SiparisHareket.InsertOrUpdate(mdl.Hareketler, trs);
                if (!rsHr.Success) {
                    trs.Dispose();
                    return new ErrorDataResult<SiparisKayitModel>(rsHr.Message);
                }

                /* detaylar */
                var rsDt = _db.SiparisHareketDetay.InsertOrUpdate(mdl.Detaylar, trs);
                if (!rsDt.Success) {
                    trs.Dispose();
                    return new ErrorDataResult<SiparisKayitModel>(rsDt.Message);
                }
                trs.Commit();
            } catch (Exception e) {
                trs.Dispose();
                return new ErrorDataResult<SiparisKayitModel>(e.Message);
            } finally {
                trs.Dispose();
            }

            return new SuccessDataResult<SiparisKayitModel>(mdl);
        }
        public IDataResult<bool> SipariseBagliUretimVarmi(Guid? sipid) {
            var sip = _db.UretimEmri.SelectFirst(c => c.SipId == sipid);
            if (!sip.Success) return new ErrorDataResult<bool>(sip.Message);
            if (sip.Data != null) return new SuccessDataResult<bool>(true);
            return new SuccessDataResult<bool>(false);
        }
        public IDataResult<string> GetOperasyonKoduByRcAId(Guid? rcaId) {
            string sql = " SELECT RO.OperasyonKodu FROM ReceteOperasyon RO  " +
                " WHERE RO.Sira= 1 AND RO.RcAId =@RcAId";
            var sip = _db.ReceteOperasyon.Query<string>(sql, new { RcAId = rcaId });
            if (!sip.Success) return new ErrorDataResult<string>(sip.Message);
            if (sip.Data != null) return new SuccessDataResult<string>(sip.Data.FirstOrDefault());
            return new SuccessDataResult<string>("");
        }


        public IResult SiparisEntGuncelle(Guid? sipId, string seri, string sira, string sonseri = "", string sonsira = "", int ent = 1, int kapandi=1 ) {
            var con = _db.Siparis.GetConnection();
            if (con.State != ConnectionState.Open) con.Open();
            var trs = con.BeginTransaction();
            try {
                var sql =
                    $"update Siparis set Ent={ent}, EntSeri ='{seri}' , EntSira ='{sira}',EntKayitSeri='{sonseri}',EntKayitSira= '{sonsira}',Kapandi= {kapandi} where Id='{sipId}' ;";
                var rsAna = _db.Siparis.Execute(sql, null, trs);
                if (!rsAna.Success) {
                    trs.Dispose();
                    return new ErrorResult(rsAna.Message);
                }

                var sql2 =
                    $" update SiparisHareket set  Ent={ent}, EntSeri ='{seri}' , EntSira ='{sira}' where SipId='{sipId}' ;";
                var rsHr = _db.Siparis.Execute(sql, null, trs);
                if (!rsHr.Success) {
                    trs.Dispose();
                    return new ErrorResult(rsHr.Message);
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
        public IResult SiparisIptalEntGuncelle(Guid? sipId, string seri, string sira ) {
            var con = _db.Siparis.GetConnection();
            if (con.State != ConnectionState.Open) con.Open();
            var trs = con.BeginTransaction();
            try {
                var sql =
                    $"update Siparis set EntIptal=1, EntIptalSeri ='{seri}' , EntIptalSira ='{sira}'  where Id='{sipId}' ;";
                var rsAna = _db.Siparis.Execute(sql, null, trs);
                if (!rsAna.Success) {
                    trs.Dispose();
                    return new ErrorResult(rsAna.Message);
                }

                //var sql2 =   $"update SiparisHareket set  Ent=1, EntSeri ='{seri}' , EntSira ='{sira}' where SipId='{sipId}' ;";
                //var rsHr = _db.Siparis.Execute(sql, null, trs);
                //if (!rsHr.Success) {
                //    trs.Dispose();
                //    return new ErrorResult(rsHr.Message);
                //}

                trs.Commit();
            } catch (Exception e) {
                trs.Dispose();
                return new ErrorResult(e.Message);
            } finally {
                trs.Dispose();
            }

            return new SuccessResult();
        }
        public virtual IResult KodVarmi<T>(T entity, string kontrolalan, bool yenikayitmi) {
            var tabloadi = ClassExtensions.GetClassTableName(typeof(T));
            var GetId = ClassExtensions.GetClassColumnNameKey(typeof(T));
            var sql2 = $" where  {GetId} <> @{GetId} and {kontrolalan} = @{kontrolalan} ";
            if (yenikayitmi) sql2 = $" where  {kontrolalan} = @{kontrolalan} ";
            var sql = string.Format("Select count(*) From {0}  {1};", tabloadi, sql2);
            var rs = _db.Siparis.Query<int>(sql, entity);
            if (!rs.Success) return new ErrorResult(rs.Message);
            if (rs.Data.FirstOrDefault() > 0) throw new Exception("Aynı " + kontrolalan + " Kodla Kayıt Var");
            return new SuccessResult();
        }

        public IResult SiparisSil(SiparisKayitModel mdl) {
            var con = _db.Siparis.GetConnection();
            if (con.State != ConnectionState.Open) con.Open();
            var trs = con.BeginTransaction();
            try {
                var rsAna = _db.Siparis.Delete(mdl.Siparis, trs);
                if (!rsAna.Success) {
                    trs.Dispose();
                    return new ErrorResult(rsAna.Message);
                }

                /* Hareketler */
                var rsHrDel = _db.SiparisHareket.Delete(c => c.SipId == mdl.Siparis.Id, trs);
                if (!rsHrDel.Success) {
                    trs.Dispose();
                    return new ErrorResult(rsHrDel.Message);
                }

                /* detaylar */
                var rsDetDel = _db.SiparisHareketDetay.Delete(c => c.SipId == mdl.Siparis.Id, trs);
                if (!rsDetDel.Success) {
                    trs.Dispose();
                    return new ErrorResult(rsDetDel.Message);
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

        public IResult SiparisSilKontrol(Guid? id) {
            var rs = _db.Siparis.QueryProc<KayitSilKontrolModel>("Siparis_Sil_Kontrol", new { Id = id });
            if (!rs.Success) return new ErrorResult(rs.Message);
            var data = rs.Data.FirstOrDefault();
            if (data != null && data.Adet > 0)
                return new ErrorResult($"Kayıda bağlı  ( {data.Mesaj} ) hareketler var kayıt silinemez.");
            return new SuccessResult();
        }
        public IDataResult<IEnumerable<TempSiparisUretimMiktar>> GetTempSiparisUretimMiktarBySipId(Guid? sipId, string kullanici) {
            var rs = _db.TempSiparisUretimMiktar.GetTempSiparisUretimMiktarBySipId(sipId, kullanici); 
            return rs;
        }

         public IDataResult<IEnumerable<TempSiparisUretimMiktar>> GetTempSiparisUretimMiktarBySipKodu(string sipkodu, string kullanici) {
            var rs = _db.TempSiparisUretimMiktar.GetTempSiparisUretimMiktarBySipKodu(sipkodu, kullanici); 
            return rs;
        }
        public IDataResult<KaliteRaporModel> GetKaliteYazdir(Guid? id)
        {
            var dbName = _dbMikro.Settings.Database;
            string sql = @" select DISTINCT SiparisKodu as IsEmriNo,SH.StokKodu as ParcaNo,SH.StokAdi as ParcaAdi,

                S.Tarih as IsEmriTarihi,S.TeslimTarihi as TeslimTarihi,SH.Miktar as TalepEdilenMiktar ,
				SH.Parti + '-' + SH.Lot as LotNo ,A.StokKodu as MalzemeKodu,A.StokAdi as MalzemeAdi,
				A.StokMiktar as MalzemeMiktar,A.Parti + '-' + A.Lot as MalzemeLotNo,
				YAPIS.StokKodu as MalzemeYapisKodu ,YAPIS.StokAdi as MalzemeYapisAdi,
				YAPIS.StokMiktar as MalzemeYapistiriciMiktar ,YAPIS.Parti + '-' + YAPIS.Lot as MalzemYapistiriciLotNo,RA.AmbalajSekli  From Siparis S
INNER JOIN SiparisHareket SH ON SH.SipId = S.Id
INNER JOIN ReceteAna RA ON SH.RcAId=RA.Id
INNER JOIN(SELECT HRD.StokKodu, HRD.StokAdi,
                  HRD.SipAdet,
                  HRD.Carpan, ROUND(SUM(HRD.StokMiktar),2) AS StokMiktar,
                  ROUND(SUM(HRD.StokFireMiktar), 2) AS StokFireMiktar, ROUND(SUM(HRD.StokIptalMiktar), 2) AS StokIptalMiktar,
                  HRD.Birim, HRD.Renk, HRD.Beden, HRD.Parti, HRD.Lot,  
                  SUM(HRD.UretimMiktar) AS UretimMiktari, SUM(HRD.FireMiktar) AS UretimFireMiktari, SUM(HRD.IptalMiktar) AS UretimIptalMiktari,
                  HRD.SipId, HRD.SipHId
                  FROM IstasyonTakipStokHareketDetay HRD

                  LEFT OUTER JOIN [MikroDB_V16_FEZA].[dbo].[STOKLAR] S ON HRD.StokKodu = S.sto_kod

                  LEFT OUTER JOIN [MikroDB_V16_FEZA].[dbo].[STOKLAR_USER] U ON S.sto_Guid = U.Record_uid
                  WHERE 1 = 1 and U.HAMMADDE_CINSI<>1
                  GROUP BY    HRD.StokKodu, HRD.StokAdi,HRD.SipAdet,HRD.Carpan,  HRD.Birim, HRD.Renk, HRD.Beden, HRD.Parti, HRD.Lot, HRD.SipId, HRD.SipHId ) A ON S.Id = A.SipId
INNER JOIN(SELECT HRD.StokKodu, HRD.StokAdi,
                  HRD.SipAdet,
                  HRD.Carpan, ROUND(SUM(HRD.StokMiktar),2) AS StokMiktar,
                  ROUND(SUM(HRD.StokFireMiktar), 2) AS StokFireMiktar, ROUND(SUM(HRD.StokIptalMiktar), 2) AS StokIptalMiktar,
                  HRD.Birim, HRD.Renk, HRD.Beden, HRD.Parti, HRD.Lot,  
                  SUM(HRD.UretimMiktar) AS UretimMiktari, SUM(HRD.FireMiktar) AS UretimFireMiktari, SUM(HRD.IptalMiktar) AS UretimIptalMiktari,
                  HRD.SipId, HRD.SipHId
                  FROM IstasyonTakipStokHareketDetay HRD

                  LEFT OUTER JOIN[MikroDB_V16_FEZA].[dbo].[STOKLAR] S ON HRD.StokKodu = S.sto_kod

                  LEFT OUTER JOIN[MikroDB_V16_FEZA].[dbo].[STOKLAR_USER] U ON S.sto_Guid = U.Record_uid
                  WHERE 1 = 1 and U.HAMMADDE_CINSI = 1
                  GROUP BY    HRD.StokKodu, HRD.StokAdi,HRD.SipAdet,HRD.Carpan,  HRD.Birim, HRD.Renk, HRD.Beden, HRD.Parti, HRD.Lot, HRD.SipId, HRD.SipHId ) YAPIS ON S.Id = YAPIS.SipId WHERE 1=1 and S.Id=@SipId";
            var sip = _db.Siparis.Query<KaliteRaporModel>(sql, new { SipId = id });
            if (!sip.Success) return new ErrorDataResult<KaliteRaporModel>(sip.Message);
            if (sip.Data != null) return new SuccessDataResult<KaliteRaporModel>(sip.Data.FirstOrDefault());
            return new SuccessDataResult<KaliteRaporModel>();
        }





    }
}