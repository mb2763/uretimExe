using My.Core.Data;
using My.Core.Logger;
using My.Core.Result;
using My.DataAccess.IstasyonTakipler;
using My.Entities.IstasyonTakipler;
using My.Entities.Models;
using System;
using System.Collections.Generic;

namespace My.Business.Service.IstasyonTakipler
{
    public class IstasyonTakipStokHareketService : BaseService<IstasyonTakipStokHareket>, IIstasyonTakipStokHareketService
    {
        private IIstasyonTakipStokHareketDal _dal;
        private ILogManager _ilogger;

        public IstasyonTakipStokHareketService(IIstasyonTakipStokHareketDal dal, ILogManager ilogger) : base(dal, ilogger)
        {
            _dal = dal;
            _ilogger = ilogger;
        } 
        public IDataResult<IEnumerable<IstasyonTakipStokHareket>> GetViewListWhere(string whereSql)
        {
            try
            {
                var r = _dal.GetViewListWhere(whereSql);
                return new SuccessDataResult<IEnumerable<IstasyonTakipStokHareket>>(r);
            }
            catch (Exception e)
            {
                _ilogger.Error(e, e.Message, _dal.GetType().Name, "");
                return new ErrorDataResult<IEnumerable<IstasyonTakipStokHareket>>(e.Message);
            }
        }
         public IDataResult<IEnumerable<IstasyonTakipStokHareketKullanilan>> GetViewListKullanimWhere(string andwhereSql)
        {
            try
            {
                var r = _dal.GetViewListKullanimWhere(andwhereSql);
                return new SuccessDataResult<IEnumerable<IstasyonTakipStokHareketKullanilan>>(r);
            }
            catch (Exception e)
            {
                _ilogger.Error(e, e.Message, _dal.GetType().Name, "");
                return new ErrorDataResult<IEnumerable<IstasyonTakipStokHareketKullanilan>>(e.Message);
            }
        }
          public IDataResult<IEnumerable<IstasyonTakipStokHareketKullanilan>> GetViewListKullanimWherePartiLot(string andwhereSql)
        {
            try
            {
                var r = _dal.GetViewListKullanimWherePartiLot(andwhereSql);
                return new SuccessDataResult<IEnumerable<IstasyonTakipStokHareketKullanilan>>(r);
            }
            catch (Exception e)
            {
                _ilogger.Error(e, e.Message, _dal.GetType().Name, "");
                return new ErrorDataResult<IEnumerable<IstasyonTakipStokHareketKullanilan>>(e.Message);
            }
        }
          public IDataResult<IEnumerable<IstasyonTakipStokHareketKullanilan>> GetViewListKullanimWhereMalKabul(string andwhereSql)
        {
            try
            {
                var r = _dal.GetViewListKullanimWhereMalKabul(andwhereSql);
                return new SuccessDataResult<IEnumerable<IstasyonTakipStokHareketKullanilan>>(r);
            }
            catch (Exception e)
            {
                _ilogger.Error(e, e.Message, _dal.GetType().Name, "");
                return new ErrorDataResult<IEnumerable<IstasyonTakipStokHareketKullanilan>>(e.Message);
            }
        }
       public IDataResult<IEnumerable<MalKabulFisKullanilanStokModel>> GetViewListKullanimMalKabulFis(Guid? sipid)
        {
            
            try
            {
                var r = _dal.GetViewListKullanimMalKabulFis(sipid);
                return new SuccessDataResult<IEnumerable<MalKabulFisKullanilanStokModel>>(r);
            }
            catch (Exception e)
            {
                _ilogger.Error(e, e.Message, _dal.GetType().Name, "");
                return new ErrorDataResult<IEnumerable<MalKabulFisKullanilanStokModel>>(e.Message);
            }
        }
        public IDataResult<IEnumerable<IstasyonTakipStokHareket>> GetStokHareketByUrIId(Guid urIId)
        {
            try
            {
                var r = _dal.GetStokHareketByUrIId(urIId);
                return new SuccessDataResult<IEnumerable<IstasyonTakipStokHareket>>(r);
            }
            catch (Exception e)
            {
                _ilogger.Error(e, e.Message, _dal.GetType().Name, "");
                return new ErrorDataResult<IEnumerable<IstasyonTakipStokHareket>>(e.Message);
            }
        }


    }
}
