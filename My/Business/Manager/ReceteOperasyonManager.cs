using System;
using System.Data;
using System.Linq;
using My.Core.Result;
using My.Entities.Models;

namespace My.Business.Manager {
    public class ReceteOperasyonManager {
        private readonly DatabaseFactoryPro _db;

        public ReceteOperasyonManager(DatabaseFactoryPro dbPro) {
            _db = dbPro;
        }

        public IDataResult<ReceteOperasyonKayitModel> GetOperasyonKayitYeni(Guid? rcAId) {
            var mdl = new ReceteOperasyonKayitModel();
            var recete = _db.ReceteAna.SelectFirst(c => c.Id == rcAId);
            if (!recete.Success) return new ErrorDataResult<ReceteOperasyonKayitModel>(recete.Message);
            mdl.Recete = recete.Data;
            return new SuccessDataResult<ReceteOperasyonKayitModel>(mdl);
        }

        public IDataResult<ReceteOperasyonKayitModel> GetOperasyonKayitEdit(Guid? rcAId) {
            var mdl = new ReceteOperasyonKayitModel();
            var recete = _db.ReceteAna.SelectFirst(c => c.Id == rcAId);
            if (!recete.Success) return new ErrorDataResult<ReceteOperasyonKayitModel>(recete.Message);
            mdl.Recete = recete.Data;

            var operasyon = _db.ReceteOperasyon.SelectList(c => c.RcAId == rcAId);
            if (!operasyon.Success) return new ErrorDataResult<ReceteOperasyonKayitModel>(operasyon.Message);
            mdl.Operasyonlar = operasyon.Data.ToList();

            var istasyon = _db.ReceteIstasyon.SelectList(c => c.RcAId == rcAId);
            if (!istasyon.Success) return new ErrorDataResult<ReceteOperasyonKayitModel>(istasyon.Message);
            mdl.Istasyonlar = istasyon.Data.ToList();

            var receteCari = _db.ReceteIstasyonCari.SelectList(c => c.RcAId == rcAId);
            if (!receteCari.Success) return new ErrorDataResult<ReceteOperasyonKayitModel>(receteCari.Message);
            mdl.Cariler = receteCari.Data.ToList();

            return new SuccessDataResult<ReceteOperasyonKayitModel>(mdl);
        }

        public IResult OperasyonKaydet(ReceteOperasyonKayitModel mdl, bool yenikayit) {
            var con = _db.ReceteOperasyon.GetConnection();
            if (con.State != ConnectionState.Open) con.Open();
            var trs = con.BeginTransaction();
            try {

                var rsAnaSil = _db.ReceteOperasyon.Delete(c => c.RcAId == mdl.Recete.Id, trs);
                if (!rsAnaSil.Success) {
                    trs.Dispose();
                    return new ErrorResult(rsAnaSil.Message);
                }

                var rsAna = _db.ReceteOperasyon.InsertOrUpdate(mdl.Operasyonlar, trs);
                if (!rsAna.Success) {
                    trs.Dispose();
                    return new ErrorResult(rsAna.Message);
                }
                /* istasyonlar Sil  */
                var rsDIDel = _db.ReceteIstasyon.Delete(c => c.RcAId == mdl.Recete.Id, trs);
                if (!rsDIDel.Success) {
                    trs.Dispose();
                    return new ErrorResult(rsDIDel.Message);
                }

                /* cariler Sil */
                var rsSIDel = _db.ReceteIstasyonCari.Delete(c => c.RcAId == mdl.Recete.Id, trs);
                if (!rsSIDel.Success) {
                    trs.Dispose();
                    return new ErrorResult(rsSIDel.Message);
                }
                if (mdl.Istasyonlar?.Count > 0) {
                    /* istasyon */
                    var rsDI = _db.ReceteIstasyon.InsertOrUpdate(mdl.Istasyonlar, trs);
                    if (!rsDI.Success) {
                        trs.Dispose();
                        return new ErrorResult(rsDI.Message);
                    }
                }
                if (mdl.Cariler?.Count > 0) {
                    /* cari */
                    if (mdl.Cariler.Count > 0) {
                        var rsSI = _db.ReceteIstasyonCari.InsertOrUpdate(mdl.Cariler, trs);
                        if (!rsSI.Success) {
                            trs.Dispose();
                            return new ErrorResult(rsSI.Message);
                        }
                    }
                }

                

                if (!yenikayit) {
                    foreach (var itm in mdl.Operasyonlar) {
                        var sqlGuncelle = @"  
                       exec  [" + "ReceteOperasyon_Tablo_AdGuncelle] '" + itm.Id.ToString().ToUpper() + @"';";
                        var rsGun = _db.ReceteOperasyon.Execute(sqlGuncelle, null, trs);
                        if (!rsGun.Success) {
                            trs.Dispose();
                            return new ErrorResult(rsGun.Message);
                        }
                    }

                    foreach (var itm in mdl.Istasyonlar) {
                        var sqlGuncelle = @"   
                        exec  [" + "ReceteIstasyon_Tablo_AdGuncelle] '" + itm.Id.ToString().ToUpper() + @"';";
                        var rsGun = _db.ReceteOperasyon.Execute(sqlGuncelle, null, trs);
                        if (!rsGun.Success) {
                            trs.Dispose();
                            return new ErrorResult(rsGun.Message);
                        }
                    }
                }

                trs.Commit();
            }
            catch (Exception e) {
                trs.Dispose();
                return new ErrorResult(e.Message);
            }
            finally {
                trs.Dispose();
            }

            return new SuccessResult();
        }

        public IResult OperasyonSil(ReceteOperasyonKayitModel mdl) {
            var con = _db.ReceteOperasyon.GetConnection();
            if (con.State != ConnectionState.Open) con.Open();
            var trs = con.BeginTransaction();
            try {
                var rsAna = _db.ReceteOperasyon.Delete(c => c.RcAId == mdl.Recete.Id, trs);
                if (!rsAna.Success) {
                    trs.Dispose();
                    return new ErrorResult(rsAna.Message);
                }

                /* istasyonlar */
                var rsDIDel = _db.ReceteIstasyon.Delete(c => c.RcAId == mdl.Recete.Id, trs);
                if (!rsDIDel.Success) {
                    trs.Dispose();
                    return new ErrorResult(rsDIDel.Message);
                }

                /* cariler */
                var rsSIDel = _db.ReceteIstasyonCari.Delete(c => c.RcAId == mdl.Recete.Id, trs);
                if (!rsSIDel.Success) {
                    trs.Dispose();
                    return new ErrorResult(rsSIDel.Message);
                }

                trs.Commit();
            }
            catch (Exception e) {
                trs.Dispose();
                return new ErrorResult(e.Message);
            }
            finally {
                trs.Dispose();
            }

            return new SuccessResult();
        }

        public IResult OperasyonSilKontrol(Guid? id) {
            var rs = _db.Siparis.QueryProc<KayitSilKontrolModel>("Operasyon_Sil_Kontrol", new { Id = id });
            if (!rs.Success) return new ErrorResult(rs.Message);
            var data = rs.Data.FirstOrDefault();
            if (data != null && data.Adet > 0)
                return new ErrorResult($"Kayıda bağlı  ( {data.Mesaj} ) hareketler var kayıt silinemez.");
            return new SuccessResult();
        }

        public IResult IstasyonSilKontrol(Guid? id) {
            var rs = _db.Siparis.QueryProc<KayitSilKontrolModel>("Istasyon_Sil_Kontrol", new { Id = id });
            if (!rs.Success) return new ErrorResult(rs.Message);
            var data = rs.Data.FirstOrDefault();
            if (data != null && data.Adet > 0)
                return new ErrorResult($"Kayıda bağlı  ( {data.Mesaj} ) hareketler var kayıt silinemez.");
            return new SuccessResult();
        }
    }
}