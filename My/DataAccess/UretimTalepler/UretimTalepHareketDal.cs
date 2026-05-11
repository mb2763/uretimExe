using My.Core.Data;
using My.Entities.UretimTalepler;
using System.Data;

namespace My.DataAccess.UretimTalepler
{
    public class UretimTalepHareketDal : BaseDal<UretimTalepHareket>, IUretimTalepHareketDal
    {
        public UretimTalepHareketDal(IDbConnection connection) : base(connection)
        {
        }


    }
}