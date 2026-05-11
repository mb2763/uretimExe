using My.Business.Service.Geneller;
using My.Core.Result;
using My.Entities.Models;
using System.Collections.Generic;

namespace My.Business.Manager
{
    public class UretimIstasyonTakipManager
    {
        private readonly DatabaseFactoryPro _db;
        private readonly IGenelService _srvGnl;
        public UretimIstasyonTakipManager(DatabaseFactoryPro dbPro)
        {
            _db = dbPro;
            _srvGnl = _db.GenelServis;
        } 
        public IDataResult<IEnumerable<UretimIstasyonTakipModel>> GetTakipList()
        {
            var sql = @" select   coalesce(UrI.IstasyonKodu,'') as IstasyonKodu,coalesce(UrI.IstasyonAdi,'') as IstasyonAdi,UrO.OperasyonKodu,UrO.OperasyonAdi, UrO.ReceteKodu,UrO.ReceteAdi,
  UrI.Fason,UrI.FasonCariKodu,UrI.FasonCariUnvani,
  sum(coalesce(UrI.PlanlananMiktar,0))as PlanlananMiktar,sum(coalesce(UrI.UretimMiktari,0))as UretimMiktari,  sum(coalesce(UrI.FireMiktari,0))as FireMiktari   ,
  sum(coalesce(UrI.IptalMiktari,0))as IptalMiktari   
  from UretimIstasyon UrI 
  left outer join UretimOperasyon UrO on UrI.UrOId = UrO.Id
  where UrO.KalanMiktar > 0  
  group by UrI.IstasyonKodu ,UrI.IstasyonAdi , UrI.Fason, UrI.FasonCariKodu,UrI.FasonCariUnvani,UrO.OperasyonKodu,UrO.OperasyonAdi,UrO.ReceteKodu,UrO.ReceteAdi;";
            return _srvGnl.Query<UretimIstasyonTakipModel>(sql, null);
        }
     public IDataResult<IEnumerable<UretimIstasyonTakipModel>> GetTakipListUretimKodlu()
        {
            var sql = @" select  Sip.SiparisKodu AS IsEmriKodu,UrO.IsEmriNo, UrI.IstasyonKodu,UrI.IstasyonAdi,UrO.OperasyonKodu,UrO.OperasyonAdi, UrO.ReceteKodu,UrO.ReceteAdi,
  UrI.Fason,UrI.FasonCariKodu,UrI.FasonCariUnvani,
  sum(coalesce(UrI.PlanlananMiktar,0))as PlanlananMiktar,sum(coalesce(UrI.UretimMiktari,0))as UretimMiktari,  sum(coalesce(UrI.FireMiktari,0))as FireMiktari   ,
  sum(coalesce(UrI.IptalMiktari,0))as IptalMiktari   
  from UretimIstasyon UrI 
  left outer join UretimOperasyon UrO on UrI.UrOId = UrO.Id
  left outer join Siparis Sip on UrI.SipId = Sip.Id
  where UrO.KalanMiktar > 0  
  group by Sip.SiparisKodu,UrO.IsEmriNo, UrI.IstasyonKodu ,UrI.IstasyonAdi , UrI.Fason, UrI.FasonCariKodu,UrI.FasonCariUnvani,UrO.OperasyonKodu,UrO.OperasyonAdi,UrO.ReceteKodu,UrO.ReceteAdi;";
            return _srvGnl.Query<UretimIstasyonTakipModel>(sql, null);
        }

        public IDataResult<IEnumerable<UretimIstasyonTakipDetayModel>> GetTakipDetayList(string istasyonKodu, int fason)
        {
            var sql =
                $@" select  Sip.SiparisKodu AS IsEmriKodu,UrO.IsEmriNo,  UrI.IstasyonKodu,UrI.IstasyonAdi,UrO.OperasyonKodu,UrO.OperasyonAdi,UrO.ReceteKodu,UrO.ReceteAdi
   ,UrI.Fason, UrI.FasonCariKodu,UrI.FasonCariUnvani,
   coalesce(UrI.PlanlananMiktar,0) as PlanlananMiktar, coalesce(UrI.UretimMiktari,0) as UretimMiktari,   coalesce(UrI.FireMiktari,0) as FireMiktari ,
   coalesce(UrI.IptalMiktari,0) as IptalMiktari ,
   UrI.BaslangicTarihi
   from UretimIstasyon UrI
   left outer join UretimOperasyon UrO on UrI.UrOId= UrO.Id
   left outer join Siparis Sip on UrI.SipId = Sip.Id
   where UrO.KalanMiktar > 0 and UrI.IstasyonKodu ='{istasyonKodu}' and Coalesce(UrI.Fason,0) = {fason}";
            return _srvGnl.Query<UretimIstasyonTakipDetayModel>(sql, null);
        }
        public IDataResult<IEnumerable<UretimIstasyonTakipDetayModel>> GetTakipDetayList1111(string istasyonKodu, int fason)
        {
            var sql =
                $@" select    UrI.IstasyonKodu,UrI.IstasyonAdi,UrO.OperasyonKodu,UrO.OperasyonAdi,UrO.ReceteAdi,UrI.Fason, 
   UrI.FasonCariKodu,UrI.FasonCariUnvani,
   coalesce(UrI.PlanlananMiktar,0) as PlanlananMiktar, coalesce(UrI.UretimMiktari,0) as UretimMiktari,   coalesce(UrI.FireMiktari,0) as FireMiktari ,
   coalesce(UrI.IptalMiktari,0) as IptalMiktari ,
   UrI.BaslangicTarihi
   from UretimIstasyon UrI
   left outer join UretimOperasyon UrO on UrI.UrOId= UrO.Id
   where UrO.KalanMiktar > 0 and UrI.IstasyonKodu ='{istasyonKodu}' and Coalesce(UrI.Fason,0) = {fason}";
            return _srvGnl.Query<UretimIstasyonTakipDetayModel>(sql, null);
        }
    }
}