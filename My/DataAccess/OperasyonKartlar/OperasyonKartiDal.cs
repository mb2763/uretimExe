using My.Core.Data;
using My.Entities.OperasyonKartlar;
using System.Data;

namespace My.DataAccess.OperasyonKartlar
{
    public class OperasyonKartiDal : BaseDal<OperasyonKarti>, IOperasyonKartiDal
    {
        public OperasyonKartiDal(IDbConnection connection) : base(connection)
        {
        }
    }
}