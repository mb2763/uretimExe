using My.Core.Data;
using My.Entities.Mikro;
using System.Collections.Generic;

namespace My.DataAccess.MikroModul
{
    public interface IMikroSiparisDal : IBaseDal<MikroSiparis>
    {
        IEnumerable<MikroSiparis> GetViewListWhere(string whereSql = "");
    }
}