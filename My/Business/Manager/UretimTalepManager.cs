using My.Business.Service.Geneller;
using My.Business.Service.UretimTalepler;
using My.Core.Result;
using My.Entities.Models;
using My.Entities.UretimTalepler;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace My.Business.Manager {
    public class UretimTalepManager {
        private readonly DatabaseFactoryPro _db;
        private IUretimTalepService _srv;
        private IUretimTalepHareketService _srvHrk;
        private readonly IGenelService _srvGnl;
        public UretimTalepManager(DatabaseFactoryPro dbPro) {
            _db = dbPro;
            _srv = _db.UretimTalep;
            _srvHrk = _db.UretimTalepHareket;
            _srvGnl = _db.GenelServis;
        }
        public UretimTalep GetTalepNew() {
            return new UretimTalep();
        }
        public IDataResult<UretimTalep> GetTalep(Guid? id) {

            var sip = _db.UretimTalep.SelectFirst(c => c.UrtTlpId == id);
            if (!sip.Success) return new ErrorDataResult<UretimTalep>(sip.Message);
            return new SuccessDataResult<UretimTalep>(sip.Data);
        }
        public IDataResult<List<UretimTalepHareket>> GetTalepHareketler(Guid? urtTlpId) {
            var sip = _db.UretimTalepHareket.SelectList(c => c.UrtTlpId == urtTlpId);
            if (!sip.Success) return new ErrorDataResult<List<UretimTalepHareket>>(sip.Message);
            return new SuccessDataResult<List<UretimTalepHareket>>(sip.Data.ToList());
        }
        public IDataResult<string> Kaydet(UretimTalep mdl, List<UretimTalepHareket> hareketler ) {
           
            var con = _db.UretimTalep.GetConnection();
            if (con.State != ConnectionState.Open) con.Open();
            var trs = con.BeginTransaction();
            try {
                var rsAna = _db.UretimTalep.InsertOrUpdate(mdl, trs);
                if (!rsAna.Success) {
                    trs.Dispose();
                    return new ErrorDataResult<string>(rsAna.Message);
                }
                /* Hareketler  Sil */
                var rsHrDel = _db.UretimTalepHareket.Delete(c => c.UrtTlpId == mdl.UrtTlpId, trs);
                if (!rsHrDel.Success) {
                    trs.Dispose();
                    return new ErrorDataResult<string>(rsHrDel.Message);
                } 
                /* Hareketler */
                var rsHr = _db.UretimTalepHareket.InsertOrUpdate(hareketler, trs);
                if (!rsHr.Success) {
                    trs.Dispose();
                    return new ErrorDataResult<string>(rsHr.Message);
                } 
                trs.Commit();
            }
            catch (Exception e) {
                trs.Dispose();
                return new ErrorDataResult<string>(e.Message);
            }
            finally {
                trs.Dispose();
            }

            return new SuccessDataResult<string>();
        }
        public IResult   Sil(UretimTalep mdl) {
            var con = _db.UretimTalep.GetConnection();
            if (con.State != ConnectionState.Open) con.Open();
            var trs = con.BeginTransaction();
            try {
                var rsAna = _db.UretimTalep.Delete(mdl, trs);
                if (!rsAna.Success) {
                    trs.Dispose();
                    return new ErrorResult(rsAna.Message);
                }

                /* Hareketler */
                var rsHrDel = _db.UretimTalepHareket.Delete(c => c.UrtTlpId == mdl.UrtTlpId, trs);
                if (!rsHrDel.Success) {
                    trs.Dispose();
                    return new ErrorResult(rsHrDel.Message);
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

    }
}
