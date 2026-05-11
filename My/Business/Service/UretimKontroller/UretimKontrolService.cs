using My.Core.Data;
using My.Core.Logger;
using My.Core.Result;
using My.DataAccess.UretimKontroller;
using My.Entities.UretimIstasyonlar;
using My.Entities.UretimKontroller;
using System.Collections.Generic;
using System;
using System.Linq;

namespace My.Business.Service.UretimKontroller {
    public class UretimKontrolService : BaseService<UretimKontrol>, IUretimKontrolService {
        private readonly IUretimKontrolDal _dal;
        private readonly ILogManager _ilogger;

        public UretimKontrolService(IUretimKontrolDal dal, ILogManager ilogger) : base(dal, ilogger) {
            _dal = dal;
            _ilogger = ilogger;
        }

        public IDataResult<IEnumerable<UretimKontrol>> GetViewListWhere(string whereSql) {
            try {
      //          string yeni = @" SELECT   UK.* ,Ur.IsEmriNo,Ur.SiparisKodu AS IsEmriKodu,K.Adi+' '+K.Soyadi as  Kullanici,
      //              RU.OlcumDegeriMin,RU.OlcumDegeriMax,RU.OlcumDegeriMin2,RU.OlcumDegeriMax2,RU.OlcumDegeriMin3,RU.OlcumDegeriMax3,UR.RcAId
      //                   FROM  UretimKontrol UK 
      //                  LEFT OUTER JOIN UretimEmri UR ON UK.UrId = UR.Id
						//LEFT OUTER JOIN UretimOperasyon UP ON UR.Id=UP.UrId
      //                  LEFT OUTER JOIN Kullanici K ON UK.KayitEden =K.KullaniciAdi
						//INNER  JOIN ReceteOperasyon RU ON UP.RcAId=RU.RcAId
						//where 1=1  ";
                string sql = @" SELECT   UK.* ,Ur.IsEmriNo,Ur.SiparisKodu AS IsEmriKodu,K.Adi+' '+K.Soyadi as  Kullanici
                         FROM  UretimKontrol UK 
                        LEFT OUTER JOIN UretimEmri UR ON UK.UrId = UR.Id
                        LEFT OUTER JOIN Personel K ON UK.KayitEden =K.Kodu " + whereSql; 
                var r = _dal.Query<UretimKontrol>(sql,null);
                return new SuccessDataResult<IEnumerable<UretimKontrol>>(r.ToList());
            } catch (Exception e) {
                _ilogger.Error(e, e.Message, _dal.GetType().Name, "");
                return new ErrorDataResult<IEnumerable<UretimKontrol>>(e.Message);
            }
        }
    }
}
