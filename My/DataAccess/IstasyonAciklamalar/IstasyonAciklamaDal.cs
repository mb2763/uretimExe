using My.Core.Data;
using My.Entities.IstasyonAciklamalar;
using System.Data;

namespace My.DataAccess.IstasyonAciklamalar {
    public class IstasyonAciklamaDal : BaseDal<IstasyonAciklama>, IIstasyonAciklamaDal {
        public IstasyonAciklamaDal(IDbConnection connection) : base(connection) {
        }
    }
}
