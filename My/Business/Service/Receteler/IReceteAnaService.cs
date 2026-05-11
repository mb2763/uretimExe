using My.Core.Data;
using My.Core.Result;
using My.Entities.Receteler;
using System.Collections.Generic;

namespace My.Business.Service.Receteler
{
    public interface IReceteAnaService : IBaseService<ReceteAna>
    {
        IDataResult<IEnumerable<ReceteAna>> GetListWhere(string where);
    }
}