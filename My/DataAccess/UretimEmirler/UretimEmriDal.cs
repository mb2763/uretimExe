using My.Core.Data;
using My.Entities.UretimEmirler;
using System.Data;

namespace My.DataAccess.UretimEmirler
{
    public class UretimEmriDal : BaseDal<UretimEmri>, IUretimEmriDal
    {
        public UretimEmriDal(IDbConnection connection) : base(connection)
        {
        }
    }
}