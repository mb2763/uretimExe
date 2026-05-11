using My.Core.Data;
using My.Entities.ReceteIstasyonGruplar;
using System.Data;

namespace My.DataAccess.ReceteIstasyonGruplar
{
    public class ReceteIstasyonGrupIstasyonDal : BaseDal<ReceteIstasyonGrupIstasyon>, IReceteIstasyonGrupIstasyonDal
    {
        public ReceteIstasyonGrupIstasyonDal(IDbConnection connection) : base(connection)
        {
        }
    }
}
