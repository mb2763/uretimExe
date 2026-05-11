using My.Core.Data;
using My.Core.Logger;
using My.Core.Result;
using My.DataAccess.MikroModul;
using My.Entities.Mikro;
using System;
using System.Collections.Generic;

namespace My.Business.Service.MikroModul
{
    public class MikroStokService : BaseService<MikroStok>, IMikroStokService
    {
        private readonly IMikroStokDal _dal;
        private readonly ILogManager _ilogger;

        public MikroStokService(IMikroStokDal dal, ILogManager ilogger) : base(dal, ilogger)
        {
            _dal = dal;
            _ilogger = ilogger;
        }

        public IDataResult<IEnumerable<MikroStok>> GetViewListWhere(string whereSql, string stokGrubKodu)
        {
            try
            {
                var r = _dal.GetViewListWhere(whereSql, stokGrubKodu);
                return new SuccessDataResult<IEnumerable<MikroStok>>(r);
            }
            catch (Exception e)
            {
                _ilogger.Error(e, e.Message, _dal.GetType().Name, "");
                return new ErrorDataResult<IEnumerable<MikroStok>>(e.Message);
            }
        }

         public IDataResult<IEnumerable<MikroStokMaliyet>> GetMikroStokMaliyetListWhere(string whereSql)
        {
            try
            {
                var r = _dal.GetMikroStokMaliyetListWhere(whereSql );
                return new SuccessDataResult<IEnumerable<MikroStokMaliyet>>(r);
            }
            catch (Exception e)
            {
                _ilogger.Error(e, e.Message, _dal.GetType().Name, "");
                return new ErrorDataResult<IEnumerable<MikroStokMaliyet>>(e.Message);
            }
        }

    
        public IDataResult<IEnumerable<MikroStokRenk>> GetRenkListWhere(string wheresql)
        {
            try
            {
                var r = _dal.GetRenkListWhere(wheresql);
                return new SuccessDataResult<IEnumerable<MikroStokRenk>>(r);
            }
            catch (Exception e)
            {
                _ilogger.Error(e, e.Message, _dal.GetType().Name, "");
                return new ErrorDataResult<IEnumerable<MikroStokRenk>>(e.Message);
            }
        }  
    
        public IDataResult<IEnumerable<MikroStokBeden>> GetBedenListWhere(string wheresql)
        {
            try
            {
                var r = _dal.GetBedenListWhere(wheresql);
                return new SuccessDataResult<IEnumerable<MikroStokBeden>>(r);
            }
            catch (Exception e)
            {
                _ilogger.Error(e, e.Message, _dal.GetType().Name, "");
                return new ErrorDataResult<IEnumerable<MikroStokBeden>>(e.Message);
            }
        }

        public IDataResult<MikroStokRenk> GetRenkByKodu(string renkKodu) {
            try {
                var r = _dal.GetRenkByKodu(renkKodu);
                return new SuccessDataResult<MikroStokRenk>(r);
            }
            catch (Exception e) {
                _ilogger.Error(e, e.Message, _dal.GetType().Name, "");
                return new ErrorDataResult<MikroStokRenk>(e.Message);
            }
        }
        public IDataResult<MikroStokBeden> GetBedenByKodu(string renkKodu) {
            try {
                var r = _dal.GetBedenByKodu(renkKodu);
                return new SuccessDataResult<MikroStokBeden>(r);
            }
            catch (Exception e) {
                _ilogger.Error(e, e.Message, _dal.GetType().Name, "");
                return new ErrorDataResult<MikroStokBeden>(e.Message);
            }
        }

        public IDataResult<MikroStokRenk> GetRenkByStokKodu(string stokKodu) {
            try {
                var r = _dal.GetRenkByStokKodu(stokKodu);
                return new SuccessDataResult<MikroStokRenk>(r);
            }
            catch (Exception e) {
                _ilogger.Error(e, e.Message, _dal.GetType().Name, "");
                return new ErrorDataResult<MikroStokRenk>(e.Message);
            }
        }
        public IDataResult<MikroStokBeden> GetBedenByStokKodu(string stokKodu) {
            try {
                var r = _dal.GetBedenByStokKodu(stokKodu);
                return new SuccessDataResult<MikroStokBeden>(r);
            }
            catch (Exception e) {
                _ilogger.Error(e, e.Message, _dal.GetType().Name, "");
                return new ErrorDataResult<MikroStokBeden>(e.Message);
            }
        }
 
        public IDataResult<List<string>> GetStokKategoriler( ) {
            try {
                var r = _dal.GetStokKategoriler( );
                return new SuccessDataResult<List<string>>(r);
            }
            catch (Exception e) {
                _ilogger.Error(e, e.Message, _dal.GetType().Name, "");
                return new ErrorDataResult<List<string>>(e.Message);
            }
        }
    }
}