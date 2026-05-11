using My.Business.Service.Geneller;
using My.Business.Service.UretimIstasyonlar;
using My.Core.Result;
using My.Entities.UretimIstasyonlar;
using System;
using System.Linq;

namespace My.Business.Manager
{
    public class UretimIstasyonManager
    {
        private readonly DatabaseFactoryPro _db;
        private readonly IUretimIstasyonHareketService _srv;
        private readonly IGenelService _srvGnl;
        private readonly IUretimIstasyonService _srvIstasyon;

        public UretimIstasyonManager(DatabaseFactoryPro dbPro)
        {
            _db = dbPro;
            _srv = _db.UretimIstasyonHareket;
            _srvIstasyon = _db.UretimIstasyon;
            _srvGnl = _db.GenelServis;
        }

        public IDataResult<UretimIstasyonHareket> GetUretimIstasyonHareket(Guid? id)
        {
            var rsIH = _srv.GetViewListWhere(" where UrIH.Id ='" + id + "'");
            if (!rsIH.Success) return new ErrorDataResult<UretimIstasyonHareket>(rsIH.Message);

            return new SuccessDataResult<UretimIstasyonHareket>(rsIH.Data.FirstOrDefault());
        }

        public IDataResult<UretimIstasyon> GetUretimIstasyon(Guid? id)
        {
            var rsI = _srvIstasyon.GetViewListWhere(" where UrI.Id ='" + id + "'");
            if (!rsI.Success) return new ErrorDataResult<UretimIstasyon>(rsI.Message);
            return new SuccessDataResult<UretimIstasyon>(rsI.Data.FirstOrDefault());
        }

        public IResult UretimIstasyonHareketKaydet(UretimIstasyonHareket mdl)
        {
            var rs = _srv.InsertOrUpdate(mdl);
            if (rs.Success)
            {
                var sql = "exec[Uretim_MiktarGuncelle] '" + mdl.UrId + "';";
                var rsGun1 = _srvGnl.Execute(sql, null);
                if (!rsGun1.Success)
                {
                    return new ErrorResult(rsGun1.Message);
                }
                var sql2 = "exec[Uretim_PlanlananGuncelle] '" + mdl.UrId + "';";
                var rsGun2 = _srvGnl.Execute(sql2, null);
                if (!rsGun2.Success)
                {

                    return new ErrorResult(rsGun2.Message);
                }
                return new SuccessResult();
            }
            else
            {
                return new ErrorResult(rs.Message);
            } 
        }

        public IResult UretimIstasyonHareketSil(UretimIstasyonHareket mdl)
        {
            var rs = _srv.Delete(mdl);
            if (rs.Success)
            {
                var sql = "exec[Uretim_MiktarGuncelle] '" + mdl.UrId + "';";
                var rsGun1 = _srvGnl.Execute(sql, null);
                if (!rsGun1.Success)
                {

                    return new ErrorResult(rsGun1.Message);
                }
                var sql2 = "exec[Uretim_PlanlananGuncelle] '" + mdl.UrId + "';";
                var rsGun2 = _srvGnl.Execute(sql2, null);
                if (!rsGun2.Success)
                {

                    return new ErrorResult(rsGun2.Message);
                }
                return new SuccessResult();
            }
            else
            {
                return new ErrorResult(rs.Message);
            }

        }
    }
}