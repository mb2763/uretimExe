using My.Core.Data;
using My.Entities.ReceteStoklar;
using System.Data;

namespace My.DataAccess.ReceteStoklar
{
    public class ReceteStokDal : BaseDal<ReceteStok>, IReceteStokDal
    {
        public ReceteStokDal(IDbConnection connection) : base(connection)
        {
        }
    }
}