using My.Core.Data;
using My.Entities.UretimAciklamalar;
using System.Data;

namespace My.DataAccess.UretimAciklamalar {
    public class AciklamaDegerDal : BaseDal<AciklamaDeger>, IAciklamaDegerDal {
        public AciklamaDegerDal(IDbConnection connection) : base(connection) {
        }
    }
}
