using My.Core.Data;
using My.Core.Result;
using My.Entities.Models;
using My.Entities.UretimEmirler;
using My.Entities.UretimOperasyonlar;
using My.Entities.UretimStoklar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace My.Business.Manager {
    public class UretimEmriManager {
        private readonly DatabaseFactoryPro _db;

        public UretimEmriManager(DatabaseFactoryPro dbPro) {
            _db = dbPro;
        }

        public IDataResult<UretimEmri> UretimEmriKaydetBySiparis(UretimEmriKayitModelSiparis mdl, bool yenikayit) {
            var rsKod = KodVarmi(mdl.UretimEmri, "IsEmriNo", yenikayit);
            if (!rsKod.Success) return new ErrorDataResult<UretimEmri>(rsKod.Message);
            var con = _db.Siparis.GetConnection();
            if (con.State != ConnectionState.Open) con.Open();
            var trs = con.BeginTransaction();
            try {

                var rsAna = _db.UretimEmri.InsertOrUpdate(mdl.UretimEmri, trs);
                if (!rsAna.Success) {
                    trs.Dispose();
                    return new ErrorDataResult<UretimEmri>(rsAna.Message);
                }
                /* Hareketler  Sil */
                var rsHrDel = _db.UretimOperasyon.Delete(c => c.UrId == mdl.UretimEmri.Id, trs);
                if (!rsHrDel.Success) {
                    trs.Dispose();
                    return new ErrorDataResult<UretimEmri>(rsHrDel.Message);
                }

                /* stoklar Sil  */
                var rsStkDel = _db.UretimStok.Delete(c => c.UrId == mdl.UretimEmri.Id, trs);
                if (!rsStkDel.Success) {
                    trs.Dispose();
                    return new ErrorDataResult<UretimEmri>(rsStkDel.Message);
                }

                /* Hareketler */
                var rsHr = _db.UretimOperasyon.InsertOrUpdate(mdl.UretimOperasyonlar, trs);
                if (!rsHr.Success) {
                    trs.Dispose();
                    return new ErrorDataResult<UretimEmri>(rsHr.Message);
                }

                if (mdl.UretimStoklar.Count > 0) {
                    /* Stoklar */
                    var rsStk = _db.UretimStok.InsertOrUpdate(mdl.UretimStoklar, trs);
                    if (!rsStk.Success) {
                        trs.Dispose();
                        return new ErrorDataResult<UretimEmri>(rsStk.Message);
                    }
                }

                var sqlGuncelle = "exec[Uretim_MiktarGuncelle] '" + mdl.UretimEmri.Id + "';";
                var rsGun = _db.UretimEmri.Execute(sqlGuncelle, null, trs);
                if (!rsGun.Success) {
                    trs.Dispose();
                    return new ErrorDataResult<UretimEmri>(rsGun.Message);
                }
                trs.Commit();
            } catch (Exception e) {
                trs.Dispose();
                return new ErrorDataResult<UretimEmri>(e.Message);
            } finally {
                trs.Dispose();
            }

            return new SuccessDataResult<UretimEmri>(mdl.UretimEmri);
        }

        public virtual IResult KodVarmi<T>(T entity, string kontrolalan, bool yenikayitmi) {
            var tabloadi = ClassExtensions.GetClassTableName(typeof(T));
            var GetId = ClassExtensions.GetClassColumnNameKey(typeof(T));
            var sql2 = $" where  {GetId} <> @{GetId} and {kontrolalan} = @{kontrolalan} ";
            if (yenikayitmi) sql2 = $" where  {kontrolalan} = @{kontrolalan} ";
            var sql = string.Format("Select count(*) From {0}  {1};", tabloadi, sql2);
            var rs = _db.ReceteAna.Query<int>(sql, entity);
            if (!rs.Success) return new ErrorResult(rs.Message);
            if (rs.Data.FirstOrDefault() > 0) throw new Exception("Aynı " + kontrolalan + " Kodla Kayıt Var");
            return new SuccessResult();
        }

        public IResult UretimEmriSil(UretimEmriKayitModelSiparis mdl) {
            var con = _db.Siparis.GetConnection();
            if (con.State != ConnectionState.Open) con.Open();
            var trs = con.BeginTransaction();
            try {
                var rsAna = _db.UretimEmri.Delete(mdl.UretimEmri, trs);
                if (!rsAna.Success) {
                    trs.Dispose();
                    return new ErrorResult(rsAna.Message);
                }

                /* Hareketler */
                var rsHrDel = _db.UretimOperasyon.Delete(c => c.UrId == mdl.UretimEmri.Id, trs);
                if (!rsHrDel.Success) {
                    trs.Dispose();
                    return new ErrorResult(rsHrDel.Message);
                }

                /* stoklar */
                var rsStkDel = _db.UretimStok.Delete(c => c.UrId == mdl.UretimEmri.Id, trs);
                if (!rsStkDel.Success) {
                    trs.Dispose();
                    return new ErrorResult(rsStkDel.Message);
                }

                var sqlGuncelle = "exec[Uretim_MiktarGuncelle] '" + mdl.UretimEmri.Id + "';";
                var rsGun = _db.UretimEmri.Execute(sqlGuncelle, null, trs);
                if (!rsGun.Success) {
                    trs.Dispose();
                    return new ErrorResult(rsGun.Message);
                }
                var sqlSipDurumDegis = "Update Siparis Set Durumu='YeniKayit' where Id='" + mdl.UretimEmri.SipId + "';";
                var rsSipDurumDegis = _db.Siparis.Execute(sqlSipDurumDegis, null, trs);
                if (!rsSipDurumDegis.Success) {
                    trs.Dispose();
                    return new ErrorResult(rsGun.Message);
                }


                trs.Commit();
            } catch (Exception e) {
                trs.Dispose();
                return new ErrorResult(e.Message);
            } finally {
                trs.Dispose();
            }

            return new SuccessResult();
        }

        public IResult UretimOperasyonHareketKaydet(List<UretimOperasyonHareket> UretimOperasyonHareketler) {
            var con = _db.Siparis.GetConnection();
            if (con.State != ConnectionState.Open) con.Open();
            var trs = con.BeginTransaction();
            try {
                /* Hareketler */
                var rsHr = _db.UretimOperasyonHareket.InsertOrUpdate(UretimOperasyonHareketler, trs);
                if (!rsHr.Success) {
                    trs.Dispose();
                    return new ErrorResult(rsHr.Message);
                }

                var urId = UretimOperasyonHareketler.FirstOrDefault().UrId;

                var sqlGuncelle = "exec[Uretim_MiktarGuncelle] '" + urId + "' ;";
                var rsGun1 = _db.UretimEmri.Execute(sqlGuncelle, null, trs);
                if (!rsGun1.Success) {
                    trs.Dispose();
                    return new ErrorResult(rsGun1.Message);
                }
                var sqlGuncelle2 = "exec[Uretim_PlanlananGuncelle] '" + urId + "' ;";
                var rsGun2 = _db.UretimEmri.Execute(sqlGuncelle2, null, trs);
                if (!rsGun2.Success) {
                    trs.Dispose();
                    return new ErrorResult(rsGun2.Message);
                }
                trs.Commit();
            } catch (Exception e) {
                trs.Dispose();
                return new ErrorResult(e.Message);
            } finally {
                trs.Dispose();
            }

            return new SuccessResult();
        }

        public IResult UretimOperasyonHareketSil(Guid? id) {
            var con = _db.Siparis.GetConnection();
            if (con.State != ConnectionState.Open) con.Open();
            var trs = con.BeginTransaction();
            try {
                /* Hareketler */
                var rsHr = _db.UretimOperasyonHareket.Delete(c => c.Id == id, trs);
                if (!rsHr.Success) {
                    trs.Dispose();
                    return new ErrorResult(rsHr.Message);
                }
                var sqlGuncelle = "exec[Uretim_MiktarGuncelle] '" + id + "';";
                var rsGun = _db.UretimEmri.Execute(sqlGuncelle, null, trs);
                if (!rsGun.Success) {
                    trs.Dispose();
                    return new ErrorResult(rsGun.Message);
                }
                trs.Commit();
            } catch (Exception e) {
                trs.Dispose();
                return new ErrorResult(e.Message);
            } finally {
                trs.Dispose();
            }

            return new SuccessResult();
        }

        public IResult UretimeBagliTumHareketleriSil(Guid? urid) {
            var con = _db.Siparis.GetConnection();
            if (con.State != ConnectionState.Open) con.Open();
            var trs = con.BeginTransaction();
            try {
                /* Hareketler */
                var rsHr = _db.UretimOperasyonHareket.Delete(c => c.UrId == urid, trs);
                if (!rsHr.Success) {
                    trs.Dispose();
                    return new ErrorResult(rsHr.Message);
                }

                /* Hareket Detay */
                var rsHrD = _db.UretimOperasyonHareketDetay.Delete(c => c.UrId == urid, trs);
                if (!rsHrD.Success) {
                    trs.Dispose();
                    return new ErrorResult(rsHrD.Message);
                }

                /* Istasyon  */
                var rsIs = _db.UretimIstasyon.Delete(c => c.UrId == urid, trs);
                if (!rsIs.Success) {
                    trs.Dispose();
                    return new ErrorResult(rsIs.Message);
                }

                /* Istasyon Hareket */
                var rsIsH = _db.UretimIstasyonHareket.Delete(c => c.UrId == urid, trs);
                if (!rsIsH.Success) {
                    trs.Dispose();
                    return new ErrorResult(rsIsH.Message);
                }

                /* Istasyon Takip Hareket */
                var rsIsTH = _db.IstasyonTakipHareket.Delete(c => c.UrId == urid, trs);
                if (!rsIsTH.Success) {
                    trs.Dispose();
                    return new ErrorResult(rsIsTH.Message);
                }
                /* Istasyon Takip   Hareket  Detay  */
                var rsIsTSHD = _db.IstasyonTakipHareketDetay.Delete(c => c.UrId == urid, trs);
                if (!rsIsTSHD.Success) {
                    trs.Dispose();
                    return new ErrorResult(rsIsTSHD.Message);
                }
                /* Istasyon Takip Hareket Log */
                var rsIsTHL = _db.IstasyonTakipHareketLog.Delete(c => c.UrId == urid, trs);
                if (!rsIsTHL.Success) {
                    trs.Dispose();
                    return new ErrorResult(rsIsTHL.Message);
                }

                /* Istasyon Takip Stok Hareket   */
                var rsIsTSH = _db.IstasyonTakipStokHareket.Delete(c => c.UrId == urid, trs);
                if (!rsIsTSH.Success) {
                    trs.Dispose();
                    return new ErrorResult(rsIsTSH.Message);
                }

                var rsIsUSFH = _db.IstasyonTakipStokHareket.Delete(c => c.UrId == urid, trs);
                if (!rsIsUSFH.Success) {
                    trs.Dispose();
                    return new ErrorResult(rsIsTSH.Message);
                    }

                var sqlGuncelle = "exec[Uretim_MiktarGuncelle] '" + urid + "';";
                var rsGun = _db.UretimEmri.Execute(sqlGuncelle, null, trs);
                if (!rsGun.Success) {
                    trs.Dispose();
                    return new ErrorResult(rsGun.Message);
                }
                trs.Commit();
            } catch (Exception e) {
                trs.Dispose();
                return new ErrorResult(e.Message);
            } finally {
                trs.Dispose();
            }

            return new SuccessResult();
        }

        public IResult UretimMiktarlariGuncelleVeAyarla(Guid? urid) {
            var con = _db.Siparis.GetConnection();
            if (con.State != ConnectionState.Open) con.Open();
            var trs = con.BeginTransaction();
            try {
                var sqlGuncelle = "exec[Uretim_MiktarGuncelle] '" + urid + "';";
                var rsGun1 = _db.UretimEmri.Execute(sqlGuncelle, null, trs);
                if (!rsGun1.Success) {
                    trs.Dispose();
                    return new ErrorResult(rsGun1.Message);
                }
                var sqlGuncelle2 = "exec[Uretim_PlanlananGuncelle] '" + urid + "';";
                var rsGun2 = _db.UretimEmri.Execute(sqlGuncelle2, null, trs);
                if (!rsGun2.Success) {
                    trs.Dispose();
                    return new ErrorResult(rsGun2.Message);
                }
                trs.Commit();
            } catch (Exception e) {
                trs.Dispose();
                return new ErrorResult(e.Message);
            } finally {
                trs.Dispose();
            }

            return new SuccessResult();
        }
        public IResult UretimDurumGuncelle(Guid? urid) {
            var con = _db.Siparis.GetConnection();
            if (con.State != ConnectionState.Open) con.Open();
            var trs = con.BeginTransaction();
            try {
                var sqlGuncelle = "exec[Uretim_DurumGuncelle] '" + urid + "';";
                var rsGun1 = _db.UretimEmri.Execute(sqlGuncelle, null, trs);
                if (!rsGun1.Success) {
                    trs.Dispose();
                    return new ErrorResult(rsGun1.Message);
                }

                trs.Commit();
            } catch (Exception e) {
                trs.Dispose();
                return new ErrorResult(e.Message);
            } finally {
                trs.Dispose();
            }

            return new SuccessResult();
        }

        public IDataResult<int> UretimOperasyonHareketKayitVarmi(Guid? id) {
            var sql = " select count(*) as Adet from  UretimOperasyonHareketDetay where UrOHId =  '" + id + "';";
            var rsHr = _db.UretimOperasyonHareketDetay.Query<int>(sql, null);
            if (!rsHr.Success) return new ErrorDataResult<int>(rsHr.Message);
            return new SuccessDataResult<int>(rsHr.Data.FirstOrDefault());
        }

        #region Siparisden

        public IDataResult<UretimEmriKayitModelSiparis> GetUretimSiparisNew(Guid? sipId) {
            var mdl = new UretimEmriKayitModelSiparis();
            var rsSip = GetSiparis(sipId);
            if (!rsSip.Success) return new ErrorDataResult<UretimEmriKayitModelSiparis>(rsSip.Message);
            mdl.SiparisModel = rsSip.Data;
            mdl.UretimEmri = new UretimEmri();
            mdl.UretimOperasyonlar = new List<UretimOperasyon>();
            mdl.UretimOperasyonHareketler = new List<UretimOperasyonHareket>();
            mdl.UretimStoklar = new List<UretimStok>();
            mdl.ReceteModeller = new List<ReceteKayitModel>();
            mdl.OperasyonModeller = new List<ReceteOperasyonKayitModel>();
            var varmi = false;

            foreach (var siphr in mdl.SiparisModel.Hareketler)
                foreach (var itm in mdl.SiparisModel.Detaylar.Where(c => c.SipHId == siphr.Id)) {
                    var rcdid = itm.RcDId;

                    varmi = false;
                    foreach (var itmx in mdl.UretimStoklar)
                        if (itm.StokKodu == itmx.StokKodu && itm.Renk == itmx.Renk && itm.Beden == itmx.Beden && itm.Birim == itmx.Birim) {
                            itmx.Miktar += itm.Miktar * siphr.Miktar;
                            itmx.PlanlananMiktar += itm.Miktar * siphr.Miktar;
                            varmi = true;
                        }

                    if (!varmi) {
                        double gec_miktar = 0.0;
                        double fire_miktar = 0.0;
                        gec_miktar = itm.Miktar;
                        var receteDetay1 = _db.ReceteDatay.SelectFirst(c => c.Id == rcdid);
                        if (!receteDetay1.Success) return new ErrorDataResult<UretimEmriKayitModelSiparis>(receteDetay1.Message);
                        if (receteDetay1.Data != null && receteDetay1.Data?.FireYuzde != 0) {
                            fire_miktar = gec_miktar * (receteDetay1.Data.FireYuzde / 100);
                        }
                        gec_miktar = itm.Miktar + fire_miktar;
                        mdl.UretimStoklar.Add(new UretimStok {
                            RcAId = itm.RcAId,
                            RcDId = itm.RcDId,
                            Birim = itm.Birim,
                            Miktar = itm.Miktar * siphr.Miktar,
                            PlanlananMiktar = gec_miktar * siphr.Miktar,
                            KullanilanMiktar = gec_miktar * siphr.Miktar,
                            FireMiktari = 0,
                            Renk = itm.Renk,
                            Beden = itm.Beden,
                            SipId = itm.SipId,
                            SipHId = itm.SipHId,
                            StokKodu = itm.StokKodu,
                            StokAdi = itm.StokAdi,
                            UrId = mdl.UretimEmri.Id,
                            Fiyat = 0,
                            Tutar = 0
                        });
                    }
                }

            /*      */
            var rcAIdler = new List<Guid?>();
            foreach (var itm in mdl.SiparisModel.Hareketler) {
                var id = rcAIdler.Count(c => c == itm.RcAId);
                if (id <= 0) rcAIdler.Add(itm.RcAId);
            }

            foreach (var itm in rcAIdler) {
                var rsm1 = GetRecete(itm);
                if (!rsm1.Success) return new ErrorDataResult<UretimEmriKayitModelSiparis>(rsm1.Message);
                var rsm2 = GetOperasyon(itm);
                if (!rsm2.Success) return new ErrorDataResult<UretimEmriKayitModelSiparis>(rsm2.Message);
                mdl.ReceteModeller.Add(rsm1.Data);
                mdl.OperasyonModeller.Add(rsm2.Data);
            }



            return new SuccessDataResult<UretimEmriKayitModelSiparis>(mdl);
        }

        public IDataResult<UretimEmriKayitModelSiparis> GetUretimSiparisEdit(Guid? urId) {
            var mdl = new UretimEmriKayitModelSiparis();
            mdl.UretimEmri = new UretimEmri();
            var urt = _db.UretimEmri.SelectFirst(c => c.Id == urId);
            if (!urt.Success) return new ErrorDataResult<UretimEmriKayitModelSiparis>(urt.Message);
            mdl.UretimEmri = urt.Data;
            if (mdl.UretimEmri == null) return new ErrorDataResult<UretimEmriKayitModelSiparis>("Üretime Ait Kayıt Bulunamadı.");
            var rsSip = GetSiparis(mdl.UretimEmri.SipId);
            if (!rsSip.Success) return new ErrorDataResult<UretimEmriKayitModelSiparis>(rsSip.Message);
            mdl.SiparisModel = rsSip.Data;
            var urtOpr = _db.UretimOperasyon.SelectList(c => c.UrId == urId);
            if (!urtOpr.Success) return new ErrorDataResult<UretimEmriKayitModelSiparis>(urtOpr.Message);
            mdl.UretimOperasyonlar = urtOpr.Data.ToList();
            var urtIst = _db.UretimOperasyonHareket.GetViewListWhere(" Where UROH.UrId = '" + urId + "';");
            if (!urtIst.Success) return new ErrorDataResult<UretimEmriKayitModelSiparis>(urtIst.Message);
            mdl.UretimOperasyonHareketler = urtIst.Data.ToList();
            var urtStk = _db.UretimStok.SelectList(c => c.UrId == urId);
            if (!urtStk.Success) return new ErrorDataResult<UretimEmriKayitModelSiparis>(urtStk.Message);
            mdl.UretimStoklar = urtStk.Data.ToList();
            /*      */
            var rcAIdler = new List<Guid?>();
            foreach (var itm in mdl.SiparisModel.Hareketler) {
                var idara = rcAIdler.Count(c => c == itm.RcAId);
                if (idara <= 0) rcAIdler.Add(itm.RcAId);
            }

            mdl.ReceteModeller = new List<ReceteKayitModel>();
            mdl.OperasyonModeller = new List<ReceteOperasyonKayitModel>();
            foreach (var itm in rcAIdler) {
                var rsm1 = GetRecete(itm);
                if (!rsm1.Success) return new ErrorDataResult<UretimEmriKayitModelSiparis>(rsm1.Message);
                var rsm2 = GetOperasyon(itm);
                if (!rsm2.Success) return new ErrorDataResult<UretimEmriKayitModelSiparis>(rsm2.Message);
                mdl.ReceteModeller.Add(rsm1.Data);
                mdl.OperasyonModeller.Add(rsm2.Data);
            }

            return new SuccessDataResult<UretimEmriKayitModelSiparis>(mdl);
        }

        public IDataResult<UretimEmriKayitModelSiparis> GetUretimOperasyonSiparisEdit(Guid? oprId) {
            var mdl = new UretimEmriKayitModelSiparis();
            mdl.UretimEmri = new UretimEmri();
            var urtOprAra = _db.UretimOperasyonHareket.SelectFirst(c => c.Id == oprId);
            if (!urtOprAra.Success) return new ErrorDataResult<UretimEmriKayitModelSiparis>(urtOprAra.Message);
            var urId = urtOprAra.Data.UrId;
            var urt = _db.UretimEmri.SelectFirst(c => c.Id == urId);
            if (!urt.Success) return new ErrorDataResult<UretimEmriKayitModelSiparis>(urt.Message);
            mdl.UretimEmri = urt.Data;
            var rsSip = GetSiparis(mdl.UretimEmri.SipId);
            if (!rsSip.Success) return new ErrorDataResult<UretimEmriKayitModelSiparis>(rsSip.Message);
            mdl.SiparisModel = rsSip.Data;
            var urtOpr = _db.UretimOperasyon.SelectList(c => c.UrId == urId);
            if (!urtOpr.Success) return new ErrorDataResult<UretimEmriKayitModelSiparis>(urtOpr.Message);
            mdl.UretimOperasyonlar = urtOpr.Data.ToList();
            var urtIst = _db.UretimOperasyonHareket.GetViewListWhere(" Where UrOH.UrId = '" + urId + "';");
            if (!urtIst.Success) return new ErrorDataResult<UretimEmriKayitModelSiparis>(urtIst.Message);
            mdl.UretimOperasyonHareketler = urtIst.Data.ToList();
            var urtStk = _db.UretimStok.SelectList(c => c.UrId == urId);
            if (!urtStk.Success) return new ErrorDataResult<UretimEmriKayitModelSiparis>(urtStk.Message);
            mdl.UretimStoklar = urtStk.Data.ToList();
            /*      */
            var rcAIdler = new List<Guid?>();
            foreach (var itm in mdl.SiparisModel.Hareketler) {
                var idara = rcAIdler.Count(c => c == itm.RcAId);
                if (idara <= 0) rcAIdler.Add(itm.RcAId);
            }
            mdl.ReceteModeller = new List<ReceteKayitModel>();
            mdl.OperasyonModeller = new List<ReceteOperasyonKayitModel>();
            foreach (var itm in rcAIdler) {
                var rsm1 = GetRecete(itm);
                if (!rsm1.Success) return new ErrorDataResult<UretimEmriKayitModelSiparis>(rsm1.Message);
                var rsm2 = GetOperasyon(itm);
                if (!rsm2.Success) return new ErrorDataResult<UretimEmriKayitModelSiparis>(rsm2.Message);
                mdl.ReceteModeller.Add(rsm1.Data);
                mdl.OperasyonModeller.Add(rsm2.Data);
            }

            return new SuccessDataResult<UretimEmriKayitModelSiparis>(mdl);
        }

        #endregion

        #region Baglantilar

        public IDataResult<SiparisKayitModel> GetSiparis(Guid? id) {
            var mdl = new SiparisKayitModel();
            var sip = _db.Siparis.SelectFirst(c => c.Id == id);
            if (!sip.Success) return new ErrorDataResult<SiparisKayitModel>(sip.Message);
            mdl.Siparis = sip.Data;
            var sipHr = _db.SiparisHareket.SelectList(c => c.SipId == id);
            if (!sipHr.Success) return new ErrorDataResult<SiparisKayitModel>(sipHr.Message);
            mdl.Hareketler = sipHr.Data.ToList();
            var sipDet = _db.SiparisHareketDetay.SelectList(c => c.SipId == id);
            if (!sipDet.Success) return new ErrorDataResult<SiparisKayitModel>(sipDet.Message);
            mdl.Detaylar = sipDet.Data.ToList();
            return new SuccessDataResult<SiparisKayitModel>(mdl);
        }

        public IDataResult<ReceteKayitModel> GetRecete(Guid? id) {
            var mdl = new ReceteKayitModel();
            var recete = _db.ReceteAna.SelectFirst(c => c.Id == id);
            if (!recete.Success) return new ErrorDataResult<ReceteKayitModel>(recete.Message);
            mdl.Recete = recete.Data;
            var receteDetay = _db.ReceteDatay.SelectList(c => c.RcAId == id);
            if (!receteDetay.Success) return new ErrorDataResult<ReceteKayitModel>(receteDetay.Message);
            mdl.ReceteDetaylar = receteDetay.Data.ToList();
            var receteStok = _db.ReceteStok.SelectList(c => c.RcAId == id);
            if (!receteStok.Success) return new ErrorDataResult<ReceteKayitModel>(receteStok.Message);
            mdl.ReceteStoklar = receteStok.Data.ToList();
            return new SuccessDataResult<ReceteKayitModel>(mdl);
        }

        public IDataResult<ReceteOperasyonKayitModel> GetOperasyon(Guid? rcAId) {
            var mdl = new ReceteOperasyonKayitModel();
            var recete = _db.ReceteAna.SelectFirst(c => c.Id == rcAId);
            if (!recete.Success) return new ErrorDataResult<ReceteOperasyonKayitModel>(recete.Message);
            mdl.Recete = recete.Data;
            var operasyon = _db.ReceteOperasyon.SelectList(c => c.RcAId == rcAId);
            if (!operasyon.Success) return new ErrorDataResult<ReceteOperasyonKayitModel>(operasyon.Message);
            mdl.Operasyonlar = operasyon.Data.ToList();
            var istasyon = _db.ReceteIstasyon.SelectList(c => c.RcAId == rcAId);
            if (!istasyon.Success) return new ErrorDataResult<ReceteOperasyonKayitModel>(istasyon.Message);
            mdl.Istasyonlar = istasyon.Data.ToList();
            var receteCari = _db.ReceteIstasyonCari.SelectList(c => c.RcAId == rcAId);
            if (!receteCari.Success) return new ErrorDataResult<ReceteOperasyonKayitModel>(receteCari.Message);
            mdl.Cariler = receteCari.Data.ToList();
            return new SuccessDataResult<ReceteOperasyonKayitModel>(mdl);
        }

        #endregion
    }
}