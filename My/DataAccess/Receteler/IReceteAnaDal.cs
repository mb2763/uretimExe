using My.Core.Data;
using My.Entities.Receteler;
using System.Collections.Generic;

namespace My.DataAccess.Receteler
{
    public interface IReceteAnaDal : IBaseDal<ReceteAna>
    {
        IEnumerable<ReceteAna> GetListWhere(string where);
    }
}