using Dapper;
using My.Core.Data;
using My.Entities.IstasyonTakipler;
using System.Collections.Generic;
using System.Data;

namespace My.DataAccess.IstasyonTakipler {
    public class IstasyonTakipHareketDetayDal : BaseDal<IstasyonTakipHareketDetay>, IIstasyonTakipHareketDetayDal {
        public IstasyonTakipHareketDetayDal(IDbConnection connection) : base(connection) {
        }
        public IEnumerable<IstasyonTakipHareketDetay> GetViewListWhere(string whereSql) {
            var sql = @"  select IstHD.*,
IstHr.SiparisKodu ,IstHr.IstasyonKodu,IstHr.IstasyonAdi,IstHr.OperasyonKodu,IstHr.OperasyonAdi,IstHr.ReceteKodu,IstHr.ReceteAdi 
,iif([Turu]='FireStokGiris',
  (Select Top 1 Birim From UretimStok where StokKodu =IstHD.StokKodu and UrId=IstHD.[UrId] ),
  (Select Top 1 [EntegreBirim] From ReceteAna where [EntegreStokKodu] =IstHD.StokKodu)) as Birim2
  ,coalesce(Prs.Adi,'') +' '+ coalesce(Prs.Soyadi,'') AS AdiSoyadi 
 ,(Select Durumu From UretimEmri where Id=IstHD.UrId) as Durumu
from IstasyonTakipHareketDetay IstHD 
left outer join IstasyonTakipHareket IstHr ON IstHD.IstHrId = IstHr.Id
left OUTER JOIN Personel PRS ON prs.Kodu=IstHD.Personel
 " + whereSql + "  ";
            var data = Connection.Query<IstasyonTakipHareketDetay>(sql);
            return data;
        }
         
        public IEnumerable<IstasyonTakipHareketDetay> GetViewListStokFire(string andwhereSql) {

  //          var sql = @" SELECT * FROM  IstasyonTakipHareketDetay IstHD 
  //LEFT OUTER JOIN UretimIstasyon UrI ON IstHD.UrIId = UrI.Id
  //WHERE IstHD.Turu='FireStokGiris'    " + andwhereSql + @"   ";  
            
            var sql = @" SELECT  (Select Top 1 Birim From UretimStok   where  UrId =  UrI.UrId and StokKodu= IstHD.StokKodu  ) as Birim  ,IstHD.*  
FROM  IstasyonTakipHareketDetay IstHD 
LEFT OUTER JOIN UretimIstasyon UrI ON IstHD.UrIId = UrI.Id 
  WHERE IstHD.Turu='FireStokGiris'   " + andwhereSql + @"   "; 
            var data = Connection.Query<IstasyonTakipHareketDetay>(sql);
            return data;
        }


    }
}
