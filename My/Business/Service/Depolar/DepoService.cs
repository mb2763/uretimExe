using My.Core.Data;
using My.Core.Logger;
using My.Core.Result;
using My.Entities.Depolar;
using System.Data;
using System.Linq;

namespace My.Business.Service.Depolar {
    public class DepoService : BaseServiceCore<Depo>, IDepoService {
        public DepoService(IDbConnection dbConnection, ILogManager log) : base(dbConnection, log) { 
        
        }
         
        public IDataResult<int> GetCount() {
            var rs = SelectListWhere();
            if (rs.IsError) {
                return new ErrorDataResult<int>(rs.Message);
            }
            return new SuccessDataResult<int>(rs.Data.Count());
        }

    }
}
