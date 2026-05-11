using My.Core.Data;
using My.Entities.ReceteGruplar;
using System.Data;

namespace My.DataAccess.ReceteGruplar
{
    public class ReceteGrupDetayDal : BaseDal<ReceteGrupDetay>, IReceteGrupDetayDal
    {
        public ReceteGrupDetayDal(IDbConnection connection) : base(connection)
        {
        }
    }
}