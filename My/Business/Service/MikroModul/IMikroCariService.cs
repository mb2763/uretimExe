using My.Core.Data;
using My.Core.Result;
using My.Entities.Mikro;
using System.Collections.Generic;

namespace My.Business.Service.MikroModul
{
    public interface IMikroCariService : IBaseService<MikroCari>
    {
        IDataResult<IEnumerable<MikroCari>> GetViewListWhere(string whereSql);
    }
}