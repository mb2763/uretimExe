using My.Core;
using My.Core.Result;
using My.Entities.Ayarlar;
using My.Entities.Mikro;
using My.Entities.StokDepoRaflar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace My.Business.Manager {


    public class MikroKayitManager {
        private readonly DatabaseFactoryMikro _dbMikro;
        private DatabaseFactoryPro _dbPro;
        public MikroKayitManager(DatabaseFactoryPro dbPro, DatabaseFactoryMikro dbMikro) {
            _dbPro = dbPro;
            _dbMikro = dbMikro;
        }
        public IDataResult<IEnumerable<MikroStokMaliyet>> GetMikroStokMaliyetListWhere(string whereSql) {
            try {
                return _dbMikro.Stoklar.GetMikroStokMaliyetListWhere(whereSql);
            } catch (Exception e) {
                return new ErrorDataResult<IEnumerable<MikroStokMaliyet>>(e.Message);
            }
        }
        public IDataResult<int> GetStokHareketEvrakSira(string seri, short sth_tip, short sth_cins, short sth_evraktip) {
            try {
                var con = (SqlConnection)_dbMikro.GenelServis.Dal.Connection;
                var evrak = 0;
                var evrakNo = 0;
                var qry = $"SELECT ISNULL(MAX(sth_evrakno_sira),0) as [evrakno] FROM STOK_HAREKETLERI WHERE sth_tip={sth_tip}   " +
                    $" AND sth_cins={sth_cins} AND sth_evraktip={sth_evraktip} AND sth_evrakno_seri='" + seri.Trim() + "'";
                var cmd = new SqlCommand(qry, con);
                cmd.CommandType = CommandType.Text;
                con.Open();
                var objReader = cmd.ExecuteReader();
                while (objReader.Read()) evrak = Convert.ToInt32(objReader["evrakno"].ToString());
                con.Close();
                evrakNo = evrak + 1;
                return new SuccessDataResult<int>(evrakNo);
            } catch (Exception e) {
                return new ErrorDataResult<int>(e.Message);
            }
        }
        public IDataResult<string> StokHareketKaydet(List<MikroStokHareketleri> _model, List<StokDepoRaf> depoRaf =null) {
            List<MikroBedenHareketleri> renkBeden = new List<MikroBedenHareketleri>();
            List<MikroPartiLot> partiLot = new List<MikroPartiLot>();
            string don = "";
            /*Renk Beden Parti Ayarla*/
            foreach (var itm in _model) {
                itm.sth_special3 = "AKT";
                /*Renk Beden Ayar*/
                if (!string.IsNullOrEmpty(itm.Beden) || !string.IsNullOrEmpty(itm.Renk)) {
                    var rs0 = GetRenkBeden(itm);
                    if (!rs0.Success) {
                        return new ErrorDataResult<string>(rs0.Message);
                    }
                    if (rs0.Data != null) {
                        renkBeden.Add(rs0.Data);
                    }
                }
                if (itm.UretimTarihi == null) {
                    itm.UretimTarihi = DateTime.Now;
                }
                if (itm.SonKullanmaTarihi == null) {
                    itm.SonKullanmaTarihi = DateTime.Now;
                }
                if (!string.IsNullOrEmpty(itm.sth_parti_kodu)) {
                    if (itm.sth_lot_no == null || itm.sth_lot_no == 0) {
                        itm.sth_lot_no = 1;
                    }
                }
                /* Parti Lot Ayar */
                if (itm.sth_tip == 0 && !string.IsNullOrEmpty(itm.sth_parti_kodu) && itm.UretimTarihi != null) {
                    var f = partiLot.Find(c => c.pl_partikodu == itm.sth_parti_kodu && c.pl_lotno == itm.sth_lot_no);
                    if (f == null) {
                        var rs01 = GetPartiLot(itm);
                        if (!rs01.Success) {
                            return new ErrorDataResult<string>(rs01.Message);
                        }
                        if (rs01.Data != null) {
                            var rsss = PartiLotDataOnceGirilmismi(itm.sth_stok_kod, itm.sth_parti_kodu, itm.sth_lot_no);
                            if (rsss.IsError) {
                                return new ErrorDataResult<string>(rsss.Message);
                            }
                            partiLot.Add(rs01.Data);
                        }
                    }
                }
            }
            /*Kaydı Başlat*/
            var con = _dbMikro.GenelServis.GetConnection();
            if (con.State != ConnectionState.Open) con.Open();
            var trs = con.BeginTransaction();
            try {
                var sql0 = _dbMikro.StokHareketleri.GetInsertOrUpdateSqlWithId();
                var rs1 = _dbMikro.StokHareketleri.Execute(sql0, _model, trs);
                if (!rs1.Success) {
                    trs.Dispose();
                    return new ErrorDataResult<string>(rs1.Message);
                }
                don = rs1.Data;
                if (renkBeden.Count > 0) {
                    var sql = MikroBedenHareketleri.GetInsertSqlCode();
                    var rs2 = _dbMikro.StokHareketleri.Execute(sql, renkBeden, trs);
                    if (!rs2.Success) {
                        trs.Dispose();
                        return new ErrorDataResult<string>(rs2.Message);
                    }
                    don = rs2.Data;
                }
                if (partiLot.Count > 0) {
                    var sql3 = MikroPartiLot.GetInsertSqlCode();
                    var rs3 = _dbMikro.StokHareketleri.Execute(sql3, partiLot, trs);
                    if (!rs3.Success) {
                        trs.Dispose();
                        return new ErrorDataResult<string>(rs3.Message);
                    }
                    don = rs3.Data;
                }
                if (depoRaf!=null && depoRaf.Count > 0) {
                    var sql4 = StokDepoRaf.GetInsertSqlCode();
                    var rs4 = _dbMikro.StokHareketleri.Execute(sql4, depoRaf, trs);
                    if (!rs4.Success) {
                        trs.Dispose();
                        return new ErrorDataResult<string>(rs4.Message);
                    }
                    don = rs4.Data;
                }
                trs.Commit();
                return new SuccessDataResult<string>(don);
            } catch (Exception e) {
                trs.Dispose();
                return new ErrorDataResult<string>(e.Message);
            } finally {
                trs.Dispose();
            }
        }
        public IDataResult<string> PartiLotDataOnceGirilmismi(string stokkodu, string partiNo, int? lotno) {
            string sqlParti = $@"  Select * FROM PARTILOT WHERE  pl_stokkodu='{stokkodu}' and pl_partikodu ='{partiNo}' and  pl_lotno = {lotno}   ; ";
            var rs2 = _dbMikro.PartiLotlar.Query<MikroPartiLot>(sqlParti, null);
            if (!rs2.Success) {
                return new ErrorDataResult<string>(rs2.Message);
            }
            if (rs2.Data.Count() > 0) {
                return new ErrorDataResult<string>("Parti No Daha Önce Kayıt Edilmiş");
            }
            return new SuccessDataResult<string>("Ok");
        }
        public IDataResult<string> DeleteMikroAktarilanFisBySeriSira(string seri, string sira) {
            var con = _dbMikro.GenelServis.GetConnection();
            if (con.State != ConnectionState.Open) con.Open();
            var trs = con.BeginTransaction();
            try {
                string sqlBeden = $@"  DELETE FROM BEDEN_HAREKETLERI WHERE BdnHar_Guid IN (
                          Select bh.BdnHar_Guid FROM BEDEN_HAREKETLERI bh 
                          LEFT OUTER JOIN STOK_HAREKETLERI sh ON bh.BdnHar_Har_uid=sh.sth_Guid 
                          WHERE sth_evrakno_seri='{seri}' and sth_evrakno_sira='{sira}' ) ; ";

                var rs2 = _dbMikro.StokHareketleri.Execute(sqlBeden, null, trs);
                if (!rs2.Success) {
                    trs.Dispose();
                    return new ErrorDataResult<string>(rs2.Message);
                }
                string sqlPartiLot = $@"    DELETE FROM PARTILOT  WHERE pl_Guid IN (   
                   Select pl.pl_Guid  FROM PARTILOT pl   
                   LEFT OUTER JOIN STOK_HAREKETLERI sh ON  sh.sth_belge_no =pl.pl_aciklama  
                   WHERE sth_evrakno_seri='{seri}' and sth_evrakno_sira='{sira}'  and pl.pl_ozelkod3= 'AKT') ;   ";

                var rsParLot = _dbMikro.StokHareketleri.Execute(sqlPartiLot, null, trs);
                if (!rsParLot.Success) {
                    trs.Dispose();
                    return new ErrorDataResult<string>(rsParLot.Message);
                }

                string sql = MikroStokHareketAktarilanFisler.GetDeleteSqlCodeBySeriSira(seri, sira) + ";";
                var rs1 = _dbMikro.StokHareketleri.Execute(sql, null, trs);
                if (!rs1.Success) {
                    trs.Dispose();
                    return new ErrorDataResult<string>(rs1.Message);
                }

                //  string sqlPartiLot = $@"  DELETE FROM PARTILOT  WHERE pl_Guid IN (    Select pl.pl_Guid  FROM PARTILOT pl   LEFT OUTER JOIN STOK_HAREKETLERI sh ON 
                //  sh.sth_stok_kod =pl.pl_stokkodu AND pl.pl_partikodu=sh.sth_parti_kodu AND pl.pl_lotno=sh.sth_lot_no 
                //  WHERE sth_evrakno_seri='{seri}' and sth_evrakno_sira='{sira}' and pl_aciklama = 'AKT') ; ";
                //  var rs3 = _dbMikro.StokHareketleri.Execute(sqlPartiLot, null, trs); if (!rs3.Success) { trs.Dispose(); return new ErrorDataResult<string>(rs3.Message); }

                trs.Commit();
                return new SuccessDataResult<string>("Ok");
            } catch (Exception e) {
                trs.Dispose();
                return new ErrorDataResult<string>(e.Message);
            } finally {
                trs.Dispose();
            }
        }
        public IDataResult<string> DeleteMikroAktarilanFisByBelgeNo(string belgeNo) {
            var con = _dbMikro.GenelServis.GetConnection();
            if (con.State != ConnectionState.Open) con.Open();
            var trs = con.BeginTransaction();
            try {
                string sqlBeden = $@"  DELETE FROM BEDEN_HAREKETLERI WHERE BdnHar_Guid IN (
                          Select bh.BdnHar_Guid FROM BEDEN_HAREKETLERI bh 
                          LEFT OUTER JOIN STOK_HAREKETLERI sh ON bh.BdnHar_Har_uid=sh.sth_Guid 
                          WHERE sth_belge_no='{belgeNo}' ) ; ";

                var rs2 = _dbMikro.StokHareketleri.Execute(sqlBeden, null, trs);
                if (!rs2.Success) {
                    trs.Dispose();
                    return new ErrorDataResult<string>(rs2.Message);
                }

                string sql = MikroStokHareketAktarilanFisler.GetDeleteSqlCodeByBelgeNo(belgeNo) + ";";
                var rs1 = _dbMikro.StokHareketleri.Execute(sql, null, trs);
                if (!rs1.Success) {
                    trs.Dispose();
                    return new ErrorDataResult<string>(rs1.Message);
                }


                trs.Commit();
                return new SuccessDataResult<string>("Ok");
            } catch (Exception e) {
                trs.Dispose();
                return new ErrorDataResult<string>(e.Message);
            } finally {
                trs.Dispose();
            }
        }
        private IDataResult<MikroBedenHareketleri> GetRenkBeden(MikroStokHareketleri sth) {
            #region Stok Tip Kontrolü
            int TakipTip = 0;
            int RBTakipTip = 0;

            try {
                var con = (SqlConnection)_dbMikro.GenelServis.Dal.Connection;
                var qry = @" SELECT  sto_detay_takip AS [TakipTip]  ,
CASE  WHEN    sto_detay_takip=1 THEN 'Parti' 
      WHEN    sto_detay_takip=2 THEN 'PartiLot' 
      WHEN    sto_detay_takip=3 THEN 'SeriNo'
      ELSE '' END AS TakipTipAd ,  
CASE  WHEN sto_renkDetayli=1    AND sto_bedenli_takip=0 THEN 1 
      WHEN sto_renkDetayli=0    AND sto_bedenli_takip=1 THEN 2 
      WHEN sto_renkDetayli=1    AND sto_bedenli_takip=1 THEN 3
      ELSE 0 END AS  [RBTakipTip] ,
CASE  WHEN sto_renkDetayli=1    AND sto_bedenli_takip=0 THEN 'Renk' 
      WHEN sto_renkDetayli=0    AND sto_bedenli_takip=1 THEN 'Beden' 
      WHEN sto_renkDetayli=1    AND sto_bedenli_takip=1 THEN 'RenkBeden'
      ELSE '' END AS RBTakipTipAd  
     , sto_Guid,  sto_kod,  sto_isim,    sto_cins
 FROM  dbo.STOKLAR   
  WHERE sto_kod='" + sth.sth_stok_kod.Trim() + "'";

                var cmd = new SqlCommand(qry, con);
                cmd.CommandType = CommandType.Text;
                con.Open();
                SqlDataReader stokTipSorguobjReader = cmd.ExecuteReader();
                while (stokTipSorguobjReader.Read()) {
                    TakipTip = Convert.ToInt32(stokTipSorguobjReader["TakipTip"].ToString());
                    RBTakipTip = Convert.ToInt32(stokTipSorguobjReader["RBTakipTip"].ToString());
                }
                con.Close();
            } catch (Exception e) {
                return new ErrorDataResult<MikroBedenHareketleri>(e.Message);
            }

            int bedenNo = 0;
            int renkNo = 0;
            #endregion

            if (RBTakipTip == 1) //RENK TAKİPLİ ÜRÜN
            {
                var rsrnk = GetRenkNo(sth.sth_stok_kod, sth.Renk);
                if (!rsrnk.Success) {
                    return new ErrorDataResult<MikroBedenHareketleri>(rsrnk.Message);
                }
                else if (rsrnk.Data <= 0) {
                    return new SuccessDataResult<MikroBedenHareketleri>(null);
                }
                else {
                    renkNo = rsrnk.Data;
                }
                #region Renk Hareketleri
                int katsayi = renkNo;
                int renkHareketNo = 1;
                if (katsayi > 1) {
                    renkHareketNo = (((katsayi * 40) - 40) + 1);
                }
                MikroBedenHareketleri renk = new MikroBedenHareketleri();
                renk.BdnHar_Guid = MyGuid.NewGuid();
                renk.BdnHar_DBCno = 0;
                renk.BdnHar_Spec_Rec_no = 0;
                renk.BdnHar_iptal = false;
                renk.BdnHar_fileid = 113;
                renk.BdnHar_hidden = false;
                renk.BdnHar_kilitli = false;
                renk.BdnHar_degisti = false;
                renk.BdnHar_checksum = 0;
                renk.BdnHar_create_user = 999;
                renk.BdnHar_create_date = DateTime.Now;
                renk.BdnHar_lastup_user = 999;
                renk.BdnHar_lastup_date = DateTime.Now;
                renk.BdnHar_special1 = "";
                renk.BdnHar_special2 = "";
                renk.BdnHar_special3 = "AKT";
                renk.BdnHar_Tipi = 11;
                renk.BdnHar_Har_uid = sth.sth_Guid;
                renk.BdnHar_BedenNo = Convert.ToInt16(renkHareketNo);
                renk.BdnHar_HarGor = sth.sth_miktar;
                renk.BdnHar_KnsIsGor = 0;
                renk.BdnHar_KnsFat = 0;
                renk.BdnHar_TesMik = 0;
                renk.BdnHar_rezervasyon_miktari = 0;
                renk.BdnHar_rezerveden_teslim_edilen = 0;
                return new SuccessDataResult<MikroBedenHareketleri>(renk);
                #endregion
            }
            else if (RBTakipTip == 2) //BEDEN TAKİPLİ ÜRÜN
            {
                var rsbdn = GetBedenNo(sth.sth_stok_kod, sth.Beden);
                if (!rsbdn.Success) {
                    return new ErrorDataResult<MikroBedenHareketleri>(rsbdn.Message);
                }
                else if (rsbdn.Data <= 0) {
                    return new SuccessDataResult<MikroBedenHareketleri>(null);
                }
                else {
                    bedenNo = rsbdn.Data;
                }
                #region Beden Hareketleri
                MikroBedenHareketleri beden = new MikroBedenHareketleri();
                beden.BdnHar_Guid = MyGuid.NewGuid();
                beden.BdnHar_DBCno = 0;
                beden.BdnHar_Spec_Rec_no = 0;
                beden.BdnHar_iptal = false;
                beden.BdnHar_fileid = 113;
                beden.BdnHar_hidden = false;
                beden.BdnHar_kilitli = false;
                beden.BdnHar_degisti = false;
                beden.BdnHar_checksum = 0;
                beden.BdnHar_create_user = 999;
                beden.BdnHar_create_date = DateTime.Now;
                beden.BdnHar_lastup_user = 999;
                beden.BdnHar_lastup_date = DateTime.Now;
                beden.BdnHar_special1 = "";
                beden.BdnHar_special2 = "";
                beden.BdnHar_special3 = "AKT";
                beden.BdnHar_Tipi = 11;
                beden.BdnHar_Har_uid = sth.sth_Guid;
                beden.BdnHar_BedenNo = Convert.ToInt16(bedenNo);
                beden.BdnHar_HarGor = sth.sth_miktar;
                beden.BdnHar_KnsIsGor = 0;
                beden.BdnHar_KnsFat = 0;
                beden.BdnHar_TesMik = 0;
                beden.BdnHar_rezervasyon_miktari = 0;
                beden.BdnHar_rezerveden_teslim_edilen = 0;
                return new SuccessDataResult<MikroBedenHareketleri>(beden);
                #endregion
            }
            else if (RBTakipTip == 3) //RENK VE BEDEN TAKİPLİ ÜRÜN
            {

                var rsrnk = GetRenkNo(sth.sth_stok_kod, sth.Renk);
                if (!rsrnk.Success) {
                    return new ErrorDataResult<MikroBedenHareketleri>(rsrnk.Message);
                }
                else if (rsrnk.Data <= 0) {
                    return new SuccessDataResult<MikroBedenHareketleri>(null);
                }
                else {
                    renkNo = rsrnk.Data;
                }
                var rsbdn = GetBedenNo(sth.sth_stok_kod, sth.Beden);
                if (!rsbdn.Success) {
                    return new ErrorDataResult<MikroBedenHareketleri>(rsbdn.Message);
                }
                else if (rsbdn.Data <= 0) {
                    return new SuccessDataResult<MikroBedenHareketleri>(null);
                }
                else {
                    bedenNo = rsbdn.Data;
                }

                #region Beden ve Renk Hareketleri
                int renkKatsayi = renkNo;
                int bedenKatsayi = bedenNo;
                int renkBedenHareketNo = 1;
                if (renkKatsayi > 1 && bedenKatsayi > 1) {
                    renkBedenHareketNo = (((renkKatsayi * 40) - 40) + 2);
                }
                MikroBedenHareketleri renkBeden = new MikroBedenHareketleri();
                renkBeden.BdnHar_Guid = MyGuid.NewGuid();
                renkBeden.BdnHar_DBCno = 0;
                renkBeden.BdnHar_Spec_Rec_no = 0;
                renkBeden.BdnHar_iptal = false;
                renkBeden.BdnHar_fileid = 113;
                renkBeden.BdnHar_hidden = false;
                renkBeden.BdnHar_kilitli = false;
                renkBeden.BdnHar_degisti = false;
                renkBeden.BdnHar_checksum = 0;
                renkBeden.BdnHar_create_user = 999;
                renkBeden.BdnHar_create_date = DateTime.Now;
                renkBeden.BdnHar_lastup_user = 999;
                renkBeden.BdnHar_lastup_date = DateTime.Now;
                renkBeden.BdnHar_special1 = "";
                renkBeden.BdnHar_special2 = "";
                renkBeden.BdnHar_special3 = "AKT";
                renkBeden.BdnHar_Tipi = 11;
                renkBeden.BdnHar_Har_uid = sth.sth_Guid;
                renkBeden.BdnHar_BedenNo = Convert.ToInt16(renkBedenHareketNo);
                renkBeden.BdnHar_HarGor = sth.sth_miktar;
                renkBeden.BdnHar_KnsIsGor = 0;
                renkBeden.BdnHar_KnsFat = 0;
                renkBeden.BdnHar_TesMik = 0;
                renkBeden.BdnHar_rezervasyon_miktari = 0;
                renkBeden.BdnHar_rezerveden_teslim_edilen = 0;
                return new SuccessDataResult<MikroBedenHareketleri>(renkBeden);
                #endregion
            }
            return new SuccessDataResult<MikroBedenHareketleri>(null);
        }
        private IDataResult<MikroPartiLot> GetPartiLot(MikroStokHareketleri sth) {
            #region Stok Tip Kontrolü
            int TakipTip = 0;
            int RBTakipTip = 0;

            try {
                var con = (SqlConnection)_dbMikro.GenelServis.Dal.Connection;
                var qry = @" SELECT  sto_detay_takip AS [TakipTip] ,
CASE  WHEN    sto_detay_takip=1 THEN 'Parti' 
      WHEN    sto_detay_takip=2 THEN 'PartiLot' 
      WHEN    sto_detay_takip=3 THEN 'SeriNo'
      ELSE '' END AS TakipTipAd ,  
CASE  WHEN sto_renkDetayli=1    AND sto_bedenli_takip=0 THEN 1 
      WHEN sto_renkDetayli=0    AND sto_bedenli_takip=1 THEN 2 
      WHEN sto_renkDetayli=1    AND sto_bedenli_takip=1 THEN 3
      ELSE 0 END AS  [RBTakipTip] ,
CASE  WHEN sto_renkDetayli=1    AND sto_bedenli_takip=0 THEN 'Renk' 
      WHEN sto_renkDetayli=0    AND sto_bedenli_takip=1 THEN 'Beden' 
      WHEN sto_renkDetayli=1    AND sto_bedenli_takip=1 THEN 'RenkBeden'
      ELSE '' END AS RBTakipTipAd  
     , sto_Guid,  sto_kod,  sto_isim,    sto_cins
 FROM  dbo.STOKLAR   
  WHERE sto_kod='" + sth.sth_stok_kod.Trim() + "'";

                var cmd = new SqlCommand(qry, con);
                cmd.CommandType = CommandType.Text;
                con.Open();
                SqlDataReader stokTipSorguobjReader = cmd.ExecuteReader();
                while (stokTipSorguobjReader.Read()) {
                    TakipTip = Convert.ToInt32(stokTipSorguobjReader["TakipTip"].ToString());
                    RBTakipTip = Convert.ToInt32(stokTipSorguobjReader["RBTakipTip"].ToString());
                }
                con.Close();
            } catch (Exception e) {
                return new ErrorDataResult<MikroPartiLot>(e.Message);
            }
            #endregion
            if (TakipTip == 1) /*PARTİ KOD TAKİPLİ ÜRÜN*/
           {
                #region   Hareketleri 
                MikroPartiLot prt = new MikroPartiLot();
                prt.pl_partikodu = sth.sth_parti_kodu;
                prt.pl_lotno = sth.sth_lot_no;
                prt.pl_stokkodu = sth.sth_stok_kod;
                prt.pl_ozelkod3 = "AKT";
                prt.pl_aciklama = sth.sth_belge_no;
                prt.pl_create_user = sth.sth_create_user;
                prt.pl_lastup_user = sth.sth_create_user;
                prt.pl_create_date = sth.sth_create_date;
                prt.pl_lastup_date = sth.sth_create_date;
                prt.pl_son_kullanim_tar = sth.SonKullanmaTarihi;
                prt.pl_uretim_tar = sth.UretimTarihi;
                return new SuccessDataResult<MikroPartiLot>(prt);
                #endregion
            }
            else if (TakipTip == 2) /*PARTİ VE LOT TAKİPLİ ÜRÜN*/
            {
                MikroPartiLot prt = new MikroPartiLot();
                prt.pl_partikodu = sth.sth_parti_kodu;
                prt.pl_lotno = sth.sth_lot_no;
                prt.pl_stokkodu = sth.sth_stok_kod;
                prt.pl_ozelkod3 = "AKT";
                prt.pl_aciklama = sth.sth_belge_no;
                prt.pl_create_user = sth.sth_create_user;
                prt.pl_lastup_user = sth.sth_create_user;
                prt.pl_create_date = sth.sth_create_date;
                prt.pl_lastup_date = sth.sth_create_date;
                prt.pl_son_kullanim_tar = sth.SonKullanmaTarihi;
                prt.pl_uretim_tar = sth.UretimTarihi;
            }
            else if (TakipTip == 3) /*SERİ NO TAKİPLİ ÜRÜN*/{ }
            else { }

            return new SuccessDataResult<MikroPartiLot>(null);
        }
        private string GetPropertyValue(string PropName, MikroStokRenk ent) {
            return ent.GetType().GetProperty(PropName) == null
                ? ""
                : ent.GetType().GetProperty(PropName).GetValue(ent, null).ToString();
        }
        private string GetPropertyValue(string PropName, MikroStokBeden ent) {
            return ent.GetType().GetProperty(PropName) == null
                ? ""
                : ent.GetType().GetProperty(PropName).GetValue(ent, null).ToString();
        }
        private IDataResult<int> GetRenkNo(string stokkodu, string renk) {
            var rsRenk = _dbMikro.Stoklar.GetRenkByStokKodu(stokkodu);
            if (!rsRenk.Success) {
                return new ErrorDataResult<int>(rsRenk.Message);
            }
            if (rsRenk.Data == null) {
                return new SuccessDataResult<int>(-1);
            }
            int renkNo = 0;
            for (var i = 1; i <= 60; i++) {
                var renkAd = GetPropertyValue("rnk_kirilim_" + i, rsRenk.Data);
                if (!string.IsNullOrEmpty(renkAd))
                    if (renkAd == renk) {
                        renkNo = i; break;
                    }
            }
            return new SuccessDataResult<int>(renkNo);
        }
        private IDataResult<int> GetBedenNo(string stokkodu, string beden) {
            var rsBeden = _dbMikro.Stoklar.GetBedenByStokKodu(stokkodu);
            if (!rsBeden.Success) {
                return new ErrorDataResult<int>(rsBeden.Message);
            }
            if (rsBeden.Data == null) {
                return new SuccessDataResult<int>(-1);
            }
            int bedenNo = 0;
            for (var i = 1; i <= 40; i++) {
                var bedenAd = GetPropertyValue("bdn_kirilim_" + i, rsBeden.Data);
                if (!string.IsNullOrEmpty(bedenAd))
                    if (bedenAd == beden) {
                        bedenNo = i; break;
                    }
            }
            return new SuccessDataResult<int>(bedenNo);
        }
        public IDataResult<IEnumerable<int>> StokHareketIdKayitEdilmismi(Guid id) {
            try {
                string sql = $@" SELECT  COUNT(*)  FROM STOK_HAREKETLERI where sth_Guid='{id}'";
                var rs = _dbMikro.StokHareketleri.Query<int>(sql, null);
                return rs;
            } catch (Exception e) {
                return new ErrorDataResult<IEnumerable<int>>(e.Message);
            }
        }
        public IDataResult<IEnumerable<MikroStokHareketAktarilanFisler>> GetMikroAktarilanFisByBelgeNo(string belgeNo) {
            try {
                string sql = MikroStokHareketAktarilanFisler.GetSelectSqlCodeByBelgeNo(belgeNo, " AND NOT (  sth_tip = 2 and    sth_cins = 6 and sth_evraktip = 2 ) ");
                var rs = _dbMikro.StokHareketleri.Query<MikroStokHareketAktarilanFisler>(sql, null);
                return rs;
            } catch (Exception e) {
                return new ErrorDataResult<IEnumerable<MikroStokHareketAktarilanFisler>>(e.Message);
            }
        }

    }
}