using My.Core.Data;
using My.Entities.ReceteGruplar;
using System.Data;

namespace My.DataAccess.ReceteGruplar
{
    public class ReceteGrupDal : BaseDal<ReceteGrup>, IReceteGrupDal
    {
        public ReceteGrupDal(IDbConnection connection) : base(connection)
        {
        }
    }
}