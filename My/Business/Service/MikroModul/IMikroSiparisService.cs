using My.Core.Data;
using My.Core.Result;
using My.Entities.Mikro;
using System.Collections.Generic;

namespace My.Business.Service.MikroModul
{
    public interface IMikroSiparisService : IBaseService<MikroSiparis>
    {
        IDataResult<IEnumerable<MikroSiparis>> GetViewListWhere(string whereSql);
    }
}