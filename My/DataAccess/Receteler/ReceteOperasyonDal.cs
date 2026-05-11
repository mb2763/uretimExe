using My.Core.Data;
using My.Entities.Receteler;
using System.Data;

namespace My.DataAccess.Receteler
{
    public class ReceteOperasyonDal : BaseDal<ReceteOperasyon>, IReceteOperasyonDal
    {
        public ReceteOperasyonDal(IDbConnection connection) : base(connection)
        {
        }
    }
}