using My.Business.Service.IstasyonBakimlar;
using My.Business.Service.IstasyonKartlar;
using My.Core.Result;
using My.Entities.IstasyonBakimlar;
using System;
using System.Collections.Generic;
using System.Data;

namespace My.Business.Manager
{

    public class IstasyonBakimManager
    {
        private readonly DatabaseFactoryMikro _dbMikro;
        private readonly DatabaseFactoryPro _dbPro;
        public IIstasyonBakimService IstBakimService { get; set; }
        public IIstasyonBakimParcaService IstParcaService { get; set; }
        public IIstasyonKartiService IstKartService { get; set; }
        public IstasyonBakimManager(DatabaseFactoryPro dbPro, DatabaseFactoryMikro dbMikro)
        {
            _dbPro = dbPro;
            _dbMikro = dbMikro;
            IstBakimService = _dbPro.IstasyonBakim;
            IstParcaService = _dbPro.IstasyonBakimParca;
            IstKartService = _dbPro.IstasyonKarti;
        }

        public IDataResult<string> Kaydet(IstasyonBakim mdl, List<IstasyonBakimParca> parcalar)
        {

            var con = _dbPro.IstasyonBakim.GetConnection();
            if (con.State != ConnectionState.Open) con.Open();
            var trs = con.BeginTransaction();
            try
            {
                var rsAna = _dbPro.IstasyonBakim.InsertOrUpdate(mdl, trs);
                if (!rsAna.Success)
                {
                    trs.Dispose();
                    return new ErrorDataResult<string>(rsAna.Message);
                }
                /* Hareketler  Sil */
                var rsHrDel = _dbPro.IstasyonBakimParca.Delete(c => c.IstBakId == mdl.Id, trs);
                if (!rsHrDel.Success)
                {
                    trs.Dispose();
                    return new ErrorDataResult<string>(rsHrDel.Message);
                }
                /* Hareketler */
                var rsHr = _dbPro.IstasyonBakimParca.InsertOrUpdate(parcalar, trs);
                if (!rsHr.Success)
                {
                    trs.Dispose();
                    return new ErrorDataResult<string>(rsHr.Message);
                }
                trs.Commit();
            }
            catch (Exception e)
            {
                trs.Dispose();
                return new ErrorDataResult<string>(e.Message);
            }
            finally
            {
                trs.Dispose();
            }

            return new SuccessDataResult<string>();
        }
        public IResult Sil(IstasyonBakim mdl)
        {
            var con = _dbPro.IstasyonBakim.GetConnection();
            if (con.State != ConnectionState.Open) con.Open();
            var trs = con.BeginTransaction();
            try
            {
                var rsAna = _dbPro.IstasyonBakim.Delete(mdl, trs);
                if (!rsAna.Success)
                {
                    trs.Dispose();
                    return new ErrorResult(rsAna.Message);
                }

                /* Hareketler */
                var rsHrDel = _dbPro.IstasyonBakimParca.Delete(c => c.IstBakId == mdl.Id, trs);
                if (!rsHrDel.Success)
                {
                    trs.Dispose();
                    return new ErrorResult(rsHrDel.Message);
                }


                trs.Commit();
            }
            catch (Exception e)
            {
                trs.Dispose();
                return new ErrorResult(e.Message);
            }
            finally
            {
                trs.Dispose();
            }

            return new SuccessResult();
        }
    }
}
