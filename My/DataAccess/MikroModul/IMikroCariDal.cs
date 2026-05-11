using My.Core.Data;
using My.Entities.Mikro;
using System.Collections.Generic;

namespace My.DataAccess.MikroModul
{
    public interface IMikroCariDal : IBaseDal<MikroCari>
    {
        IEnumerable<MikroCari> GetViewListWhere(string whereSql);
    }
}