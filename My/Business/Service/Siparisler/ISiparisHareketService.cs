using My.Core.Data;
using My.Core.Result;
using My.Entities.Siparisler;
using System.Collections.Generic;

namespace My.Business.Service.Siparisler
{
    public interface ISiparisHareketService : IBaseService<SiparisHareket>
    {
        IDataResult<IEnumerable<SiparisHareket>> GetViewListKalanMiktarliWhere(string whereSql);
        IDataResult<IEnumerable<SiparisHareketModel>> GetViewListWhere(string whereSql);
        IDataResult<IEnumerable<SiparisHareket>> GetViewListWhere2(string whereSql);
    }
}
