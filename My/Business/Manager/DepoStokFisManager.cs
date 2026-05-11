using My.Core;
using My.Core.Result;
using My.Entities.DepoStoklar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Linq;

namespace My.Business.Manager {
    public class DepoStokFisManager {
        private DatabaseFactoryPro _dbPro;
        public DepoStokFisManager(DatabaseFactoryPro dbPro) {
            _dbPro = dbPro;
        }
        public IDataResult<IEnumerable<DepoStokFis>> GetDepoStokFisListWhere(string whereSql) {
            try {
                string sql = DepoStokFis.GetSelectSqlCode() + whereSql;
                return _dbPro.GenelServis.Query<DepoStokFis>(whereSql, null);
            } catch (Exception e) {
                return new ErrorDataResult<IEnumerable<DepoStokFis>>(e.Message);
            }
        }
        
        public IDataResult<IEnumerable<DepoStok>> GetDepoStok(List<DepoStokHareket> hr) {
            try {
                string insql = "";
                var klis1 = hr.GroupBy(c => c.StokKodu).ToList();
                foreach (var itm in klis1) {
                    if (string.IsNullOrEmpty(insql)) {
                        insql = $"'{itm.Key}'";
                    }
                    else {
                        insql = insql + $",'{itm.Key}'";
                    }
                }
                string sql = DepoStok.GetSelectSqlCode() + $" where StokKodu IN ({insql}) ;";
                var rs = _dbPro.GenelServis.Query<DepoStok>(sql, null);
                return rs;
            } catch (Exception e) {
                return new ErrorDataResult<IEnumerable<DepoStok>>(e.Message);
            }
        }
    
        public IDataResult<DepoStokFis> KaydetDepoStokFis(DepoStokFis mdl, List<DepoStokHareket> list, bool yenikayit, bool cikis = false) {
            if (mdl.Id == Guid.Empty) {
                mdl.Id = MyGuid.NewGuid();
            }

            foreach (var itm in list) {
                itm.FisId = mdl.Id;
                if (itm.Id == Guid.Empty) {
                    itm.Id = MyGuid.NewGuid();
                }
            }
            var rsEvrakKayit = EvrakKayitEdilmismi(mdl.FisTuru, mdl.FisSeri, mdl.FisNo, mdl.Id);
            if (!rsEvrakKayit.Success) {
                return new ErrorDataResult<DepoStokFis>(rsEvrakKayit.Message);
            }

            if (rsEvrakKayit.Data > 0) {
                return new ErrorDataResult<DepoStokFis>("FisSeri : " + mdl.FisSeri + " FisNo : " + mdl.FisNo.ToString() + " nolu Evrak Daha Once Kayıt Edilmiş");
            } 
            var con = _dbPro.GenelServis.GetConnection();
            if (con.State != ConnectionState.Open) con.Open();
            var trs = con.BeginTransaction();
            try {
                var sqlAna = DepoStokFis.GetInsertSqlCode(); 
                var rsAna = _dbPro.GenelServis.Execute(sqlAna, mdl, trs);
                if (!rsAna.Success) {
                    trs.Dispose();
                    return new ErrorDataResult<DepoStokFis>(rsAna.Message);
                }
                var sqlHrSilAyar = " update DepoStokHareket set Sil = 2 where FisId = '" + mdl.Id + "' ; ";
                var rsHrSilAyar = _dbPro.GenelServis.Execute(sqlHrSilAyar, null, trs);
                if (!rsHrSilAyar.Success) {
                    trs.Dispose();
                    return new ErrorDataResult<DepoStokFis>(rsHrSilAyar.Message);
                }

                foreach (var itm in list) {
                    itm.FisId = mdl.Id;
                    itm.Tarih = mdl.Tarih;
                    itm.Sil = 0;
                    if (string.IsNullOrEmpty(itm.BarGui)) {
                        itm.BarGui = Guid.NewGuid().ToString().ToUpper().Replace("-", "");
                    }
                }
                /* detaylar */
                var sqlDetay = DepoStokHareket.GetInsertSqlCode();
                var rsDI = _dbPro.GenelServis.Execute(sqlDetay, list, trs);
                if (!rsDI.Success) {
                    trs.Dispose();
                    return new ErrorDataResult<DepoStokFis>(rsDI.Message);
                }
                var sqlTemizle = " Delete From  DepoStokHareket where Sil= 2 and FisId ='" + mdl.Id + "' ; ";
                var rsHrSil = _dbPro.GenelServis.Execute(sqlTemizle, null, trs);
                if (!rsHrSil.Success) {
                    trs.Dispose();
                    return new ErrorDataResult<DepoStokFis>(rsHrSil.Message);
                }
                var sqlSiraGuncelle = @" 
                DECLARE @varId UNIQUEIDENTIFIER
                DECLARE @varGuid UNIQUEIDENTIFIER
                DECLARE @varSira INT
                DECLARE cur CURSOR FAST_FORWARD READ_ONLY LOCAL FOR
                    SELECT SH.Id,SH.StGuid, SH.Sira FROM DepoStokHareket SH WHERE coalesce(SH.Sira,0) = 0;
                OPEN cur
                FETCH NEXT FROM cur INTO @varId, @varGuid, @varSira
                WHILE @@FETCH_STATUS = 0 BEGIN
                    UPDATE DepoStokHareket SET Sira = (SELECT max(Sira) + 1 FROM DepoStokHareket SHU WHERE SHU.StGuid = @varGuid ) WHERE Id = @varId;
                FETCH NEXT FROM cur INTO @varId, @varGuid, @varSira
                END
                    CLOSE cur
                DEALLOCATE cur; ";

                var rsHrSiraGuncelle = _dbPro.GenelServis.Execute(sqlSiraGuncelle, null, trs);
                if (!rsHrSiraGuncelle.Success) {
                    trs.Dispose();
                    return new ErrorDataResult<DepoStokFis>(rsHrSiraGuncelle.Message);
                }
                trs.Commit();
                StokSiraAyarla(mdl.Id);
            } catch (Exception e) {
                trs.Dispose();
                return new ErrorDataResult<DepoStokFis>(e.Message);
            } finally {
                trs.Dispose();
            }
            return new SuccessDataResult<DepoStokFis>(mdl);
        }
        IDataResult<int> EvrakKayitEdilmismi(string fisTuru, string fisSeri, int fisNo, Guid? id) {
            string sql = @" SELECT count(*) AS Adet FROM DepoStokFis FS 
            WHERE FS.FisTuru = '" + fisTuru + "' AND FS.FisSeri = '" + fisSeri +
                         "' AND FS.FisNo = " + fisNo + " AND Id <>  '" + id + "' ;";

            var rsSel = _dbPro.GenelServis.Query<int>(sql, null);
            if (!rsSel.Success) return new ErrorDataResult<int>(rsSel.Message);

            return new SuccessDataResult<int>(rsSel.Data.FirstOrDefault());
        }

        private IResult StokSiraAyarla(Guid? fisId) {
            string sqlSifiraDusenler = @" 
                     SELECT * FROM ( SELECT SH.StGuid,SH.Sira ,
                     (SELECT TOP 1 SUM(COALESCE(SH1.GirisMiktar,0)) - SUM(COALESCE(SH1.CikisMiktar,0)) From DepoStokHareket SH1 WHERE SH1.StGuid=SH.StGuid AND SH1.Sira=SH.Sira ) AS Kalan,
                     0 AS Cikan
                     FROM DepoStokHareket SH 
                     LEFT OUTER JOIN DepoStokFis FS ON FS.Id=SH.FisId
                     WHERE FS.Id= '" + fisId + @"'  
                     GROUP  BY SH.StGuid,SH.Sira )HR
                     WHERE HR.Kalan <= 0 ";

            string sqlSirala = @"   SELECT * FROM ( 
   SELECT  SH.StGuid, SH.Sira,SUM(COALESCE(SH.GirisMiktar,0))-SUM(COALESCE(SH.CikisMiktar,0)) AS Kalan,0 AS Cikan,SH.Sira AS SiraEski,SH.BarGui
   FROM DepoStokHareket SH 
   WHERE SH.StGuid  ='@GuidDegisecek'
   GROUP  BY SH.StGuid,SH.Sira,SH.BarGui)HR
   WHERE HR.Kalan > 0
   ORDER BY HR.StGuid,HR.Sira ";

            var rsSel = _dbPro.GenelServis.Query<DepoStokCikisSiraKontrolModel>(sqlSifiraDusenler, null);
            if (!rsSel.Success) return new ErrorResult(rsSel.Message);
            foreach (var itm in rsSel.Data) {
                var sqlEksiyeDusur = " update DepoStokHareket set Sira =-1 where StGuid ='" + itm.StGuid + "' and Sira =" + itm.Sira + ";";
                var rsUpd = _dbPro.GenelServis.Execute(sqlEksiyeDusur, null);
                if (!rsUpd.Success) return new ErrorResult(rsUpd.Message);
                var sqlSirala2 = sqlSirala.Replace("@GuidDegisecek", itm.StGuid.ToString());
                var rsSelSirala =
                    _dbPro.GenelServis.Query<DepoStokCikisSiraKontrolModel>(sqlSirala2, null);
                if (!rsSelSirala.Success) return new ErrorResult(rsSelSirala.Message);
                int sira = 0;
                foreach (var itm2 in rsSelSirala.Data) {
                    itm2.Sira = ++sira;
                }

                var sqlUpdateYeniSira = " update DepoStokHareket set Sira =@Sira where BarGui=@BarGui;";
                var rsUpdYeniSira = _dbPro.GenelServis.Execute(sqlUpdateYeniSira, rsSelSirala.Data);
                if (!rsUpdYeniSira.Success) return new ErrorResult(rsUpdYeniSira.Message);
            }

            return new SuccessResult();
        }
        public IResult SilDepoStokFis(Guid? idSi) {
            var con = _dbPro.GenelServis.GetConnection();
            if (con.State != ConnectionState.Open) con.Open();
            var trs = con.BeginTransaction();
            try {
                var rsAna = _dbPro.GenelServis.Execute($@"DELETE FROM DepoStokFis where Id='{idSi}'",null, trs);
                if (!rsAna.Success) {
                    trs.Dispose();
                    return new ErrorResult(rsAna.Message);
                }
                /* detaylar */
                var rsDI = _dbPro.GenelServis.Execute($@"DELETE FROM DepoStokHareket where FisId='{idSi}'", null, trs); 
                if (!rsDI.Success) {
                    trs.Dispose();
                    return new ErrorResult(rsDI.Message);
                }
                trs.Commit();
                StokSiraAyarla(idSi);
            } catch (Exception e) {
                trs.Dispose();
                return new ErrorResult(e.Message);
            } finally {
                trs.Dispose();
            }
            return new SuccessResult();
        }

        public IDataResult<int> FisNoAl(string islemTuru, string seri) {
            var sql = @" SELECT coalesce(FIS.FisNo,0) + 1 AS FisNo From ( 
            Select Max(coalesce(FisNo,0))  AS FisNo FROM DepoStokFis DSF 
            WHERE DSF.FisTuru = '" + islemTuru + "' AND DSF.FisSeri = '" + seri + "')  FIS; ";
            var rsSel = _dbPro.GenelServis.Query<int>(sql, null);
            if (!rsSel.Success) return new ErrorDataResult<int>(rsSel.Message);
            return new SuccessDataResult<int>(rsSel.Data.FirstOrDefault());
        }
    }
}



//public IDataResult<IEnumerable<DepoStokHareket>> GetDepoStokHareketListWhere(string whereSql) {
//    try {
//        string sql = DepoStokHareket.GetSelectSqlCode() + whereSql;
//        return _dbPro.GenelServis.Query<DepoStokHareket>(sql, null);
//    } catch (Exception e) {
//        return new ErrorDataResult<IEnumerable<DepoStokHareket>>(e.Message);
//    }
//}
//public IDataResult<IEnumerable<DepoStok>> GetDepoStok(string whereSql) {
//    try {
//        string sql = DepoStok.GetSelectSqlCode() + whereSql;
//        return _dbPro.GenelServis.Query<DepoStok>(sql, null);
//    } catch (Exception e) {
//        return new ErrorDataResult<IEnumerable<DepoStok>>(e.Message);
//    }
//}