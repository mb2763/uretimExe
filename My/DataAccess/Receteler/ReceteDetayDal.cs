using My.Core.Data;
using My.Entities.Receteler;
using System.Data;

namespace My.DataAccess.Receteler
{
    public class ReceteDetayDal : BaseDal<ReceteDetay>, IReceteDetayDal
    {
        public ReceteDetayDal(IDbConnection connection) : base(connection)
        {
        }
    }
}