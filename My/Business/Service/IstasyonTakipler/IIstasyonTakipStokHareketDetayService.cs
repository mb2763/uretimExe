using My.Core.Data;
using My.Core.Result;
using My.Entities.IstasyonTakipler;
using System;
using System.Collections;
using System.Collections.Generic;

namespace My.Business.Service.IstasyonTakipler {
    public interface IIstasyonTakipStokHareketDetayService : IBaseService<IstasyonTakipStokHareketDetay> {

        /// <summary>
        /// IstasyonTakipStokHareketDetay ITHD,TempMikroStok TMPS
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        IDataResult<IEnumerable<IstasyonTakipStokHareketDetayToplu>> GetListViewInKategoriToplu(string andwhere, string altandwhere);
        /// <summary>
        /// IstasyonTakipStokHareketDetay ITHD,TempMikroStok TMPS
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        IDataResult<IEnumerable<IstasyonTakipStokHareketDetayDetayli>> GetListViewInKategoriDetayli(string andwhere, string altandwhere);
      
        IDataResult<string> DetaylarGuncelleToplu();
        IDataResult<string> DetaylarGuncelleBySipId(Guid? sipId);
    }
}
