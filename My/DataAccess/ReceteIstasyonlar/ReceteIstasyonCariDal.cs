using My.Core.Data;
using My.Entities.ReceteIstasyonlar;
using System.Data;

namespace My.DataAccess.ReceteIstasyonlar
{
    public class ReceteIstasyonCariDal : BaseDal<ReceteIstasyonCari>, IReceteIstasyonCariDal
    {
        public ReceteIstasyonCariDal(IDbConnection connection) : base(connection)
        {
        }
    }
}