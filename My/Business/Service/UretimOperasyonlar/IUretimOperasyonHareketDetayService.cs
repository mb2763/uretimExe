using My.Core.Data;
using My.Core.Result;
using My.Entities.UretimOperasyonlar;
using System.Collections.Generic;

namespace My.Business.Service.UretimOperasyonlar
{
    public interface IUretimOperasyonHareketDetayService : IBaseService<UretimOperasyonHareketDetay>
    {
        IDataResult<IEnumerable<UretimOperasyonHareketDetay>> GetViewListWhere(string whereSql);
    }
}