using My.Business.Service.UretimIstasyonlar;
using My.Business.Service.UretimOperasyonlar;
using My.Core.Result;
using My.Entities.UretimIstasyonlar;
using My.Entities.UretimOperasyonlar;
using System;
using System.Collections.Generic;
using System.Data;

namespace My.Business.Manager
{
    public class UretimOperasyonManager
    {
        private readonly DatabaseFactoryPro _db;
        private readonly IUretimIstasyonService _srvIst;
        private readonly IUretimOperasyonHareketService _srvOprHrk;
        private readonly IUretimOperasyonHareketDetayService _srvOprHrkDetay;

        public UretimOperasyonManager(DatabaseFactoryPro dbPro)
        {
            _db = dbPro;
            _srvIst = _db.UretimIstasyon;
            _srvOprHrkDetay = _db.UretimOperasyonHareketDetay;
            _srvOprHrk = _db.UretimOperasyonHareket;
        }

        public IDataResult<bool> KaydetNormal(List<UretimIstasyon> ist, UretimOperasyonHareketDetay Detay, List<UretimIstasyon> silinenler)
        {
            IDbConnection con = _srvIst.GetConnection();
            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }
            IDbTransaction trs = con.BeginTransaction();
            try
            {
                var oprDetGuid = Detay.Id;
                var rsd = _srvOprHrkDetay.InsertOrUpdate(Detay, trs);
                if (!rsd.Success)
                {
                    trs.Dispose();
                    return new ErrorDataResult<bool>(rsd.Message);
                }

                var rsDel = _srvIst.Delete(c => c.UrOHDId == oprDetGuid, trs);
                if (!rsDel.Success)
                {
                    trs.Dispose();
                    return new ErrorDataResult<bool>(rsDel.Message);
                }

                if (ist.Count > 0)
                {
                    var rs = _srvIst.InsertOrUpdate(ist, trs);
                    if (!rs.Success)
                    {
                        trs.Dispose();
                        return new ErrorDataResult<bool>(rs.Message);
                    }
                }

                foreach (var itm in silinenler)
                {
                    string sqlIstTakpHareketSil = $@" 
                                                    Delete From IstasyonTakipHareket          WHERE UrIId='{itm.Id}';  
                                                    Delete From IstasyonTakipHareketDetay     WHERE UrIId='{itm.Id}'; 
                                                    Delete From IstasyonTakipHareketLog       WHERE UrIId='{itm.Id}'; 
                                                    Delete From IstasyonTakipStokHareket      WHERE UrIId='{itm.Id}'; 
                                                   
                                                    ";
                    var rIsTkpSil = _srvIst.Execute(sqlIstTakpHareketSil, null, trs);
                    if (!rIsTkpSil.Success)
                    {
                        trs.Dispose();
                        return new ErrorDataResult<bool>(rIsTkpSil.Message);

                    }
                }

                var sqlGuncelle = "exec  [Uretim_MiktarGuncelle] '" + Detay.UrId.ToString() + "' ; ";
                var r3 = _srvIst.Execute(sqlGuncelle, null, trs);
                if (!r3.Success)
                {
                    trs.Dispose();
                    return new ErrorDataResult<bool>(r3.Message);

                }
                var sqlGuncelle2 = "exec  [Uretim_PlanlananGuncelle] '" + Detay.UrId.ToString() + "' ; ";
                var r4 = _srvIst.Execute(sqlGuncelle2, null, trs);
                if (!r4.Success)
                {
                    trs.Dispose();
                    return new ErrorDataResult<bool>(r4.Message);

                }
                trs.Commit();
            }
            catch (Exception ex)
            {
                trs.Dispose();
                return new ErrorDataResult<bool>(ex.Message);
            }
            finally
            {
                trs.Dispose();
            }
            return new SuccessDataResult<bool>(true);
        }
        public IDataResult<bool> KaydetSonraki(List<UretimIstasyon> ist, UretimOperasyonHareketDetay Detay, UretimOperasyonHareket HareketYeni)
        {
            IDbConnection con = _srvIst.GetConnection();
            if (con.State != ConnectionState.Open)
            {
                con.Open();
            }
            IDbTransaction trs = con.BeginTransaction();
            try
            {
                var rshr = _srvOprHrk.Insert(HareketYeni, trs);
                if (!rshr.Success)
                {
                    trs.Dispose();
                    return new ErrorDataResult<bool>(rshr.Message);

                }

                var oprDetGuid = Detay.Id;
                Detay.UrOHId = HareketYeni.Id;
                var rsd = _srvOprHrkDetay.InsertOrUpdate(Detay, trs);
                if (!rsd.Success)
                {
                    trs.Dispose();
                    return new ErrorDataResult<bool>(rsd.Message);

                }
                foreach (var itm in ist)
                {
                    itm.UrOHId = HareketYeni.Id;
                }

                var rsDel = _srvIst.Delete(c => c.UrOHDId == oprDetGuid, trs);
                if (!rsDel.Success)
                {
                    trs.Dispose();
                    return new ErrorDataResult<bool>(rsDel.Message);
                }

                if (ist.Count > 0)
                {
                    var rs = _srvIst.InsertOrUpdate(ist, trs);
                    if (!rs.Success)
                    {
                        trs.Dispose();
                        return new ErrorDataResult<bool>(rs.Message);

                    }
                }

                var sqlGeuncelle = " exec  [Uretim_MiktarGuncelle] '" + Detay.UrId.ToString() + "' ; ";
                var r3 = _srvIst.Execute(sqlGeuncelle, null, trs);
                if (!r3.Success)
                {
                    trs.Dispose();
                    return new ErrorDataResult<bool>(r3.Message);

                }
                trs.Commit();
            }
            catch (Exception ex)
            {
                trs.Dispose();
                return new ErrorDataResult<bool>(ex.Message);
            }
            finally
            {
                trs.Dispose();
            }
            return new SuccessDataResult<bool>(true);
        }


    }
}
