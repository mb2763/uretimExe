using My.Core.Data;
using My.Core.Result;
using My.Entities.Depolar;

namespace My.Business.Service.Depolar {
    public interface IDepoService : IBaseService<Depo> {
        IDataResult<int> GetCount();

    }
}
