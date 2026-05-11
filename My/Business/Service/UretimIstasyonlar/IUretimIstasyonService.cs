using My.Core.Data;
using My.Core.Result;
using My.Entities.UretimIstasyonlar;
using System.Collections.Generic;

namespace My.Business.Service.UretimIstasyonlar
{
    public interface IUretimIstasyonService : IBaseService<UretimIstasyon>
    {
        IDataResult<IEnumerable<UretimIstasyon>> GetViewListWhere(string whereSql);
    }
}