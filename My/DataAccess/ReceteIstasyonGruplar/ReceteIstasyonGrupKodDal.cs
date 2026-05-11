using My.Core.Data;
using My.Entities.ReceteIstasyonGruplar;
using System.Data;

namespace My.DataAccess.ReceteIstasyonGruplar
{
    public class ReceteIstasyonGrupKodDal : BaseDal<ReceteIstasyonGrupKod>, IReceteIstasyonGrupKodDal
    {
        public ReceteIstasyonGrupKodDal(IDbConnection connection) : base(connection)
        {
        }
    }
}
