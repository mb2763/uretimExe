using My.Core.Data;
using My.Entities.ReceteIstasyonGruplar;
using System.Data;

namespace My.DataAccess.ReceteIstasyonGruplar
{
    public class ReceteIstasyonGrupOperasyonDal : BaseDal<ReceteIstasyonGrupOperasyon>, IReceteIstasyonGrupOperasyonDal
    {
        public ReceteIstasyonGrupOperasyonDal(IDbConnection connection) : base(connection)
        {
        }
    }
}
