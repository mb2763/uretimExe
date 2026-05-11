

using My.Core.Result;
using My.Entities.Raporlar.IstasyonRaporlari;
using System.Linq;

namespace My.Business.Manager {
    public class IstasyonRaporManager {
        private readonly DatabaseFactoryPro _db;

        public IstasyonRaporManager(DatabaseFactoryPro dbPro) {
            _db = dbPro;
        }
         
        /// <summary>
        /// 
        /// </summary>
        /// <param name="andSorgu">HR IstasyonHareketDetay  IST IstasyonHareket</param>
        /// <returns></returns>
        public virtual IDataResult<IstasyonRaporModel> GetIstasyonRapor(string andSorgu) {

            var sqlHrk = IstasyonRaporHareketModel.GetSelectSqlCode(andSorgu);
            var sqlTop = IstasyonRaporToplamModel.GetSelectSqlCode(andSorgu); 
            var rs1 = _db.GenelServis.Query<IstasyonRaporHareketModel>(sqlHrk, null);
            if (!rs1.Success) return new ErrorDataResult<IstasyonRaporModel>(rs1.Message); 
            var rs2 = _db.GenelServis.Query<IstasyonRaporToplamModel>(sqlTop, null);
            if (!rs2.Success) return new ErrorDataResult<IstasyonRaporModel>(rs2.Message); 
            var mdl = new IstasyonRaporModel();
            mdl.Hareketler = rs1.Data.ToList();
            mdl.Toplamlar = rs2.Data.ToList(); 
            return new SuccessDataResult<IstasyonRaporModel>(mdl);
        }
         
    }
}
