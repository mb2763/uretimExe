using My.Core.Data;
using My.Entities.UretimStoklar;
using System.Data;

namespace My.DataAccess.UretimStoklar
{
    public class UretimStokFisDal : BaseDal<UretimStokFis>, IUretimStokFisDal
    {
        public UretimStokFisDal(IDbConnection connection) : base(connection)
        {
        }
    }
}
