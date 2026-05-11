using My.Business.Service.IstasyonKartlar;
using My.Business.Service.IstasyonTakipler;
using My.Core;
using My.Core.Result;
using My.Entities.IstasyonTakipler;
using My.Entities.UretimIstasyonlar;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using Dapper;

namespace My.Business.Manager {
    public class IstasyonHareketManager {
        private readonly DatabaseFactoryMikro _dbMikro;
        private readonly DatabaseFactoryPro _dbPro;
        public IIstasyonTakipHareketService IstHareketService { get; set; }
        public IIstasyonTakipHareketLogService IstHareketLogService { get; set; }
        public IIstasyonTakipStokHareketService IstHareketStokService { get; set; }
        public IIstasyonTakipHareketDetayService IstHareketDetayService { get; set; }
        public IIstasyonKartiService IstKartService { get; set; }
        public IstasyonHareketManager(DatabaseFactoryPro dbPro, DatabaseFactoryMikro dbMikro) {
            _dbPro = dbPro;
            _dbMikro = dbMikro;
            IstHareketService = _dbPro.IstasyonTakipHareket;
            IstHareketLogService = _dbPro.IstasyonTakipHareketLog;
            IstHareketStokService = _dbPro.IstasyonTakipStokHareket;
            IstHareketDetayService = _dbPro.IstasyonTakipHareketDetay;
            IstKartService = _dbPro.IstasyonKarti;
        }


        public IDataResult<IEnumerable<IstasyonTakipBekleyenModel>> GetBekleyenler(string whereAnd = "") {
            var sql = $@"  SELECT UrI.Id AS UrIId,UrI.UrId ,sip.SiparisKodu, sip.Tarih,  UrO.ReceteKodu,UrO.ReceteAdi,siph.StokKodu,siph.StokAdi, 
  sum(coalesce(UrI.PlanlananMiktar,0)) -( sum(coalesce(UrI.UretimMiktari,0)) + sum(coalesce(UrI.FireMiktari,0))) AS KalanMiktar ,
  UrI.IstasyonKodu,UrI.IstasyonAdi,UrO.OperasyonKodu,UrO.OperasyonAdi, 
  sum(coalesce(UrI.PlanlananMiktar,0))as PlanlananMiktar,sum(coalesce(UrI.UretimMiktari,0))as UretimMiktari,  sum(coalesce(UrI.FireMiktari,0))as FireMiktari  ,
  sum(coalesce(UrI.IptalMiktari,0))as IptalMiktari  ,
  sip.TeslimTarihi , UrI.Fason,UrI.FasonCariKodu,UrI.FasonCariUnvani,siph.Parti,siph.Lot, UrI.KayitEden AS TalepEden,siph.Aciklama  
  from UretimIstasyon UrI 
  left outer join UretimOperasyon UrO on UrI.UrOId = UrO.Id
  left outer join Siparis sip  on UrO.SipId = sip.Id
  left outer join SiparisHareket siph  on UrO.SipHId = siph.Id
  left outer join IstasyonTakipHareket TH  on TH.UrIId = UrI.Id
  where UrO.KalanMiktar > 0 AND Th.Id IS null  {whereAnd}
  group by  UrI.Id,UrI.UrId,UrI.IstasyonKodu ,UrI.IstasyonAdi , UrI.Fason, UrI.FasonCariKodu,
  UrI.FasonCariUnvani,UrO.OperasyonKodu,UrO.OperasyonAdi,UrO.ReceteKodu,UrO.ReceteAdi,
  sip.SiparisKodu,sip.Tarih,sip.TeslimTarihi, UrO.KalanMiktar,siph.StokKodu,siph.StokAdi,siph.Parti,siph.Lot , UrI.KayitEden ,siph.Aciklama; ";
            var dt = _dbPro.GenelServis.Query<IstasyonTakipBekleyenModel>(sql, null);
            return dt;
        }
        public IDataResult<IEnumerable<IstasyonTakipBekleyenModel>> GetBekleyenByIstKodu(string Istasyon) {
            var sql = $@"  SELECT UrI.Id AS UrIId,UrI.UrId, sip.SiparisKodu, sip.Tarih,  UrO.ReceteKodu,UrO.ReceteAdi,siph.StokKodu,siph.StokAdi, 
  sum(coalesce(UrI.PlanlananMiktar,0)) -( sum(coalesce(UrI.UretimMiktari,0)) + sum(coalesce(UrI.IptalMiktari,0))) AS KalanMiktar ,
  UrI.IstasyonKodu,UrI.IstasyonAdi,UrO.OperasyonKodu,UrO.OperasyonAdi, 
  sum(coalesce(UrI.PlanlananMiktar,0))as PlanlananMiktar,sum(coalesce(UrI.UretimMiktari,0))as UretimMiktari, sum(coalesce(UrI.FireMiktari,0))as FireMiktari  ,
  sum(coalesce(UrI.IptalMiktari,0))as IptalMiktari  ,
  sip.TeslimTarihi , UrI.Fason,UrI.FasonCariKodu,UrI.FasonCariUnvani,siph.Parti,siph.Lot, UrI.KayitEden AS TalepEden,siph.Aciklama  
  from UretimIstasyon UrI 
  left outer join UretimOperasyon UrO on UrI.UrOId = UrO.Id
  left outer join Siparis sip  on UrO.SipId = sip.Id
  left outer join SiparisHareket siph  on UrO.SipHId = siph.Id
  left outer join IstasyonTakipHareket TH  on TH.UrIId = UrI.Id
  where UrO.KalanMiktar > 0 AND Th.Id IS null and UrI.IstasyonKodu = '{Istasyon}'
  group by  UrI.Id,UrI.UrId,UrI.IstasyonKodu ,UrI.IstasyonAdi , UrI.Fason, UrI.FasonCariKodu,
  UrI.FasonCariUnvani,UrO.OperasyonKodu,UrO.OperasyonAdi,UrO.ReceteKodu,UrO.ReceteAdi,
  sip.SiparisKodu,sip.Tarih,sip.TeslimTarihi, UrO.KalanMiktar,siph.StokKodu,siph.StokAdi,siph.Parti,siph.Lot , UrI.KayitEden,siph.Aciklama; ";
            var dt = _dbPro.GenelServis.Query<IstasyonTakipBekleyenModel>(sql, null);
            return dt;
        }


        public IResult SaveIstasyonTakipHareketDetayUpdate(IstasyonTakipHareketDetay mdl) {
            if (mdl.Id == null || mdl.Id == Guid.Empty) {
                mdl.Id = MyGuid.NewGuid();
            }

            IDbConnection _conn = new SqlConnection(_dbPro.Settings.GetConnectionString());
            if (_conn.State != ConnectionState.Open) {
                _conn.Open();
            }
            IDbTransaction trs = _conn.BeginTransaction();
            try {
                var sql = IstasyonTakipHareketDetay.GetUpdateSqlCode();
                _conn.Execute(sql, mdl, trs); 

                string sql00 = $@"   UPDATE UretimIstasyonHareket SET  
                UretimMiktari   =  @Miktar ,FireMiktari =  @FireMiktar ,IptalMiktari =  @IptalMiktar   WHERE Id=@Id;  "; 
                _conn.Execute(sql00, mdl, trs);

                var sql0 = IstasyonTakipHareket.GetMiktarFireUpdateSqlCode(mdl.IstHrId); ;
                _conn.Execute(sql0, null, trs);

                var sql01 = IstasyonTakipHareket.GetIptalUpdateSqlCodeByUrIId(mdl.UrIId); ;
                _conn.Execute(sql01, null, trs);

                var sql1 = "exec[Uretim_MiktarGuncelle] '" + mdl.UrId + "';";
                _conn.Execute(sql1, null, trs);
                var sql2 = "exec[Uretim_PlanlananGuncelle] '" + mdl.UrId + "';";
                _conn.Execute(sql2, null, trs);
                trs.Commit();
                return new SuccessResult();
            } catch (SqlException e) {

                trs.Dispose();
                return new ErrorResult(e.Message);
            } catch (Exception e) {

                trs.Dispose();
                return new ErrorResult(e.Message);
            } finally {
                trs.Dispose();
                _conn.Dispose();
            }

        }

    }
}
