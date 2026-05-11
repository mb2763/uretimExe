using My.Core.Data;
using My.Entities.ReceteIstasyonlar;
using System.Data;

namespace My.DataAccess.ReceteIstasyonlar
{
    public class ReceteIstasyonDal : BaseDal<ReceteIstasyon>, IReceteIstasyonDal
    {
        public ReceteIstasyonDal(IDbConnection connection) : base(connection)
        {
        }
    }
}