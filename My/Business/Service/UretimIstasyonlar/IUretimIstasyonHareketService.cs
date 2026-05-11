using My.Core.Data;
using My.Core.Result;
using My.Entities.UretimIstasyonlar;
using System.Collections.Generic;

namespace My.Business.Service.UretimIstasyonlar
{
    public interface IUretimIstasyonHareketService : IBaseService< UretimIstasyonHareket>
    {
        IDataResult<IEnumerable< UretimIstasyonHareket>> GetViewListWhere(string whereSql);
    }
}