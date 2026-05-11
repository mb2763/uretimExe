using My.Core.Data;
using My.Entities.UretimTalepler;
using System.Data;

namespace My.DataAccess.UretimTalepler
{
    public class UretimTalepDal : BaseDal<UretimTalep>, IUretimTalepDal
    {
        public UretimTalepDal(IDbConnection connection) : base(connection)
        {
        }


    }
}