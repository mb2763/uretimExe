using My.Core.Data;
using My.Core.Result;
using My.Entities.Mikro;
using System.Collections.Generic;

namespace My.Business.Service.MikroModul
{
    public interface IMikroSiparisHareketService : IBaseService<MikroSiparisHareket>
    {
        IDataResult<IEnumerable<MikroSiparisHareket>> GetViewListWhere(string whereSql);
        IDataResult<IEnumerable<MikroSiparisHareket>> GetViewListSeriSira(string seri, string sira);
    }
}