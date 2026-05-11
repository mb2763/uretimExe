using My.Core.Data;
using My.Core.Result;
using My.Entities.ReceteIstasyonGruplar;

namespace My.Business.Service.ReceteIstasyonGruplar
{
    public interface IReceteIstasyonGrupKodService : IBaseService<ReceteIstasyonGrupKod>
    {
        IResult KodVarmi<T>(T entity, string kontrolalan, bool yenikayitmi);
    }
}
