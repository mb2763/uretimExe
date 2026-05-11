using My.Business.Service.Geneller;
using My.Business.Service.UretimTalepler;
using My.Core.Result;
using My.Entities.Models;
using System;
using System.Collections.Generic;
using System.Security.Policy;

namespace My.Business.Manager {
    public class UretimTakipManagerV2 {
        private readonly DatabaseFactoryPro _db; 
        private readonly IGenelService _srvGnl;

        public UretimTakipManagerV2(DatabaseFactoryPro dbPro) {
            _db = dbPro; 
            _srvGnl = _db.GenelServis;
        }

        public IDataResult<IEnumerable<UretimTakipModelV2>> GetTakipList() {
            var sql = @"Select * From  (
                        select UrO.OperasyonKodu, UrO.OperasyonAdi,UrO.ReceteAdi, coalesce(Sip.SiparisKodu,'') as SiparisKodu,coalesce(sip.CariUnvani,'') as CariUnvani, 		 
						 sum(coalesce(UrOH.PlanlananMiktar,0))as PlanlananMiktar  
                        , sum(coalesce(UrOH.UretimMiktari,0))as UretimMiktari
                        , sum(coalesce(UrOH.IslemdekiMiktar,0))as IslemdekiMiktar 
                        , sum(coalesce(UrOH.FireMiktari,0))as FireMiktari 
                        , sum(coalesce(UrOH.IptalMiktari,0))as IptalMiktari 
                        , sum(coalesce(UrOH.KalanMiktar,0))as KalanMiktar 
                         from UretimOperasyonHareket UrOH 
                         left outer join UretimOperasyon UrO on UrO.Id=UrOH.UrOId 
                         left outer join UretimEmri Ur on Ur.Id=UrO.UrId 
                         left outer join Siparis Sip on Sip.Id=UrO.SipId 
                          where UrO.Durumu <> 'Hazir' and coalesce(Ur.Kapandi,0) = 0 and coalesce(UrOH.KalanMiktar,0) > 0 
                          group by UrO.OperasyonKodu, UrO.OperasyonAdi,UrO.ReceteAdi,UrO.Durumu,coalesce(Sip.SiparisKodu,''),coalesce(sip.CariUnvani,''))HR ";
            return _srvGnl.Query<UretimTakipModelV2>(sql, null);
        }

        public IDataResult<IEnumerable<UretimTakipDetayModelV2>> GetTakipDetayList(string operasyon, string durumu,string sipid="") {
            /* and coalesce(UrOH.UretimMiktari,0) > 0 */ /* and coalesce(UrOH.KalanMiktar,0) > 0 */
            var sql = "";
            string sipidSql = "";
            if (!string.IsNullOrEmpty(sipid)) {
                sipidSql=   " and Sip.Id= '" + sipid + "'";
            }
            if (durumu == "Uretimde")
                sql = @"
            select UrOHD.Id ,Uro.Durumu, UrO.OperasyonKodu,UrO.OperasyonAdi,UrO.ReceteKodu,UrO.ReceteAdi, Sip.SiparisKodu ,Sip.CariKodu,Sip.CariUnvani 
              , coalesce(UrOHD.PlanlananMiktar,0) as PlanlananMiktar  ,  coalesce(UrOHD.UretimMiktari,0) as UretimMiktari ,coalesce(UrOHD.IslemdekiMiktar,0)as IslemdekiMiktar
              , coalesce(UrOHD.FireMiktari,0) as FireMiktari ,coalesce(UrOHD.IptalMiktari,0) as IptalMiktari ,  coalesce(UrOHD.IslemdekiMiktar,0) - coalesce(UrOHD.UretimMiktari,0) as KalanMiktar 
              from UretimOperasyonHareketDetay UrOHD 
			  left outer join UretimOperasyonHareket UrOH on UrOH.Id=UrOHD.UrOHId 
              left outer join UretimOperasyon UrO on UrO.Id=UrOHD.UrOId  
              left outer join UretimEmri Ur on Ur.Id=UrO.UrId 
              left outer join Siparis Sip on Sip.Id=UrO.SipId 
             where UrO.Durumu <> 'Hazir' and coalesce(Ur.Kapandi,0) = 0 and UrO.OperasyonKodu ='" + operasyon + "' " + sipidSql+
                      @"  and coalesce(UrOH.IslemdekiMiktar,0) > 0 and coalesce(UrOH.KalanMiktar,0) > 0  ; ";
            else
                sql =
                    @"select UrOH.Id ,Uro.Durumu, UrO.OperasyonKodu,UrO.OperasyonAdi,UrO.ReceteKodu,UrO.ReceteAdi, Sip.SiparisKodu ,Sip.CariKodu,Sip.CariUnvani 
              , coalesce(UrOH.PlanlananMiktar,0) as PlanlananMiktar  ,  coalesce(UrOH.UretimMiktari,0) as UretimMiktari ,coalesce(UrOH.IslemdekiMiktar,0)as IslemdekiMiktar
              , coalesce(UrOH.FireMiktari,0) as FireMiktari  , coalesce(UrOH.IptalMiktari,0) as IptalMiktari ,  coalesce(UrOH.KalanMiktar,0) as KalanMiktar 
              from UretimOperasyonHareket UrOH 
              left outer join UretimOperasyon UrO on UrO.Id=UrOH.UrOId 
              left outer join UretimEmri Ur on Ur.Id=UrO.UrId 
              left outer join Siparis Sip on Sip.Id=UrO.SipId 
             where UrO.Durumu <> 'Hazir' and coalesce(Ur.Kapandi,0) = 0 and UrO.OperasyonKodu ='" + operasyon + "' " + sipidSql +
                    @" and coalesce(UrOH.KalanMiktar,0) > 0 ;";

            return _srvGnl.Query<UretimTakipDetayModelV2>(sql, null);
        }

        public IDataResult<IEnumerable<UretimTakipDetayModelV2>> GetTakipDetayListAll() {
            var sql =
                @" Select  UrOH.Id  as Id,Uro.Durumu,UrO.OperasyonKodu,UrO.OperasyonAdi,UrO.ReceteKodu,UrO.ReceteAdi,
                   UrO.PlanlananMiktar,UrO.UretimMiktari,coalesce(UrOH.IslemdekiMiktar,0)as IslemdekiMiktar,UrO.FireMiktari,UrO.IptalMiktari,UrO.KalanMiktar 
               , Sip.SiparisKodu ,Sip.CariKodu,Sip.CariUnvani 
               From (select UrO2.* From ( select UrId   From UretimOperasyon  where KalanMiktar > 0 Group By UrId ) UrO1
               left outer join UretimOperasyon UrO2  on  UrO1.UrId = UrO2.UrId ) UrO
               left outer join UretimOperasyonHareket UrOH  on  UrOH.UrOId = UrO.Id
               left outer join Siparis Sip on Sip.Id=UrO.SipId  ;";
            return _srvGnl.Query<UretimTakipDetayModelV2>(sql, null);
        }

        public IDataResult<IEnumerable<OperasyonKalanKontrolModel>> GetSiparisKalanKontrol(Guid? sipId) {
            var sql = @"declare @UrId uniqueidentifier set @UrId = '" + sipId + @"' ;
            select UrId,UrOId,RcaId,RcOId,Sira,
              sum(coalesce(PlanlananMiktar,0)) as PlanlananMiktar  ,
              sum(coalesce(UretimMiktari,0)) as UretimMiktari  ,
              sum(coalesce(IslemdekiMiktar,0)) as IslemdekiMiktar  ,
              sum(coalesce(FireMiktari,0)) as FireMiktari  ,
              sum(coalesce(IptalMiktari,0)) as IptalMiktari  ,
              sum(coalesce(KalanMiktar,0)) as KalanMiktar    
              from UretimOperasyonHareket where UrId =@UrId  
            group by UrId,UrOId,RcaId,RcOId,Sira ;";
            return _srvGnl.Query<OperasyonKalanKontrolModel>(sql, null);
        }
    }
}