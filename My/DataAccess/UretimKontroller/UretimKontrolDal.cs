using My.Core.Data;
using My.Entities.UretimKontroller;
using System.Data;

namespace My.DataAccess.UretimKontroller {
    public class UretimKontrolDal : BaseDal<UretimKontrol>, IUretimKontrolDal {
        public UretimKontrolDal(IDbConnection connection) : base(connection) {
        }
    }
}
