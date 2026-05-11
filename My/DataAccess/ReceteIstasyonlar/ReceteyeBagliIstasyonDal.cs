using My.Core.Data;
using My.Entities.ReceteIstasyonlar;
using System.Data;

namespace My.DataAccess.ReceteIstasyonlar
{
    public class ReceteyeBagliIstasyonDal : BaseDal<ReceteyeBagliIstasyon>, IReceteyeBagliIstasyonDal
    {
        public ReceteyeBagliIstasyonDal(IDbConnection connection) : base(connection)
        {
        }
    }
}
