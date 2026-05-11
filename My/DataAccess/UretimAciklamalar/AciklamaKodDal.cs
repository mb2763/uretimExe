using My.Core.Data;
using My.Entities.UretimAciklamalar;
using System.Data;

namespace My.DataAccess.UretimAciklamalar {
    public class AciklamaKodDal : BaseDal<AciklamaKod>, IAciklamaKodDal {
        public AciklamaKodDal(IDbConnection connection) : base(connection) {
        }
    }
}
