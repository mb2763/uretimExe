using My.Core.Data;
using My.Entities.UretimStoklar;
using System.Data;

namespace My.DataAccess.UretimStoklar
{
    public class UretimStokDal : BaseDal<UretimStok>, IUretimStokDal
    {
        public UretimStokDal(IDbConnection connection) : base(connection)
        {
        }
    }
}