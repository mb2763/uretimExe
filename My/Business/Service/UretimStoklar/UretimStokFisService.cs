using My.Core.Data;
using My.Core.Logger;
using My.Core.Result;
using My.DataAccess.UretimStoklar;
using My.Entities.DepoStoklar;
using My.Entities.UretimStoklar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace My.Business.Service.UretimStoklar {
    public class UretimStokFisService : BaseService<UretimStokFis>, IUretimStokFisService {
        private IUretimStokFisDal _dal;
        private ILogManager _ilogger;

        public UretimStokFisService(IUretimStokFisDal dal, ILogManager ilogger) : base(dal, ilogger) {
            _dal = dal;
            _ilogger = ilogger;
        }

        public IDataResult<UretimStokFisModel> GetFis(Guid id) {
            try {
                string sql = UretimStokFisModel.GetSelectSqlCode($" where UF.Id='{id}' ");
                var rs = _dal.Query<UretimStokFisModel>(sql, null);
                if (rs == null || rs.Count() <= 0) {
                    return new ErrorDataResult<UretimStokFisModel>("Kayit Bulunamadı");
                }
                return new SuccessDataResult<UretimStokFisModel>(rs.FirstOrDefault());

            } catch (Exception ex) {
                return new ErrorDataResult<UretimStokFisModel>(ex.Message);

            }
        }
        public IDataResult<UretimStokFisModel> GetFisFirst(string sor) {
            try {
                string sql = UretimStokFisModel.GetSelectSqlCode(sor);
                var rs = _dal.Query<UretimStokFisModel>(sql, null);
                if (rs == null || rs.Count() <= 0) {
                    return new ErrorDataResult<UretimStokFisModel>("Kayit Bulunamadı");
                }
                return new SuccessDataResult<UretimStokFisModel>(rs.FirstOrDefault());

            } catch (Exception ex) {
                return new ErrorDataResult<UretimStokFisModel>(ex.Message);
            }
        }
        public IDataResult<List<UretimStokFisModel>> GetFisList(string sor = "") {
            try {
                string sql = UretimStokFisModel.GetSelectSqlCode(sor);
                var rs = _dal.Query<UretimStokFisModel>(sql, null);
                return new SuccessDataResult<List<UretimStokFisModel>>(rs.ToList());
            } catch (Exception ex) {
                return new ErrorDataResult<List<UretimStokFisModel>>(ex.Message);
            }
        }

        public IDataResult<UretimStokFisHareket> GetStokHareketFirst(string sor) {
            try {
                string sql = UretimStokFisHareket.GetSelectSqlCode(sor);
                var rs = _dal.Query<UretimStokFisHareket>(sql, null);
                if (rs == null || rs.Count() <= 0) {
                    return new ErrorDataResult<UretimStokFisHareket>("Kayit Bulunamadı");
                }
                return new SuccessDataResult<UretimStokFisHareket>(rs.FirstOrDefault());

            } catch (Exception ex) {
                return new ErrorDataResult<UretimStokFisHareket>(ex.Message);
            }
        }
        public IDataResult<List<UretimStokFisHareket>> GetStokHareketList(string sor = "") {
            try {
                string sql = UretimStokFisHareket.GetSelectSqlCode(sor);
                var rs = _dal.Query<UretimStokFisHareket>(sql, null);
                return new SuccessDataResult<List<UretimStokFisHareket>>(rs.ToList());
            } catch (Exception ex) {
                return new ErrorDataResult<List<UretimStokFisHareket>>(ex.Message);
            }
        }
        public IDataResult<List<UretimStokFisHareket>> GetStokHareketByFisId(Guid? fisId) {
            try {
                string sql = UretimStokFisHareket.GetSelectSqlCodeByFisId(fisId);
                var rs = _dal.Query<UretimStokFisHareket>(sql, null);
                return new SuccessDataResult<List<UretimStokFisHareket>>(rs.ToList());

            } catch (Exception ex) {
                return new ErrorDataResult<List<UretimStokFisHareket>>(ex.Message);
            }

        }

        public IDataResult<string> FisOnaySave(Guid? fisId, string durumu, string usercode) {
            try {
                string sql = UretimStokFis.GetOnayUpdSqlCode(fisId, durumu, usercode);
                var rs = _dal.Execute(sql, null);

                return new SuccessDataResult<string>(rs);

            } catch (Exception ex) {
                return new ErrorDataResult<string>(ex.Message);
            }

        }


        public IDataResult<string> FisSil(Guid? fisId) {
            string don = "";
            var con = GetConnection();
            if (con.State != ConnectionState.Open) con.Open();
            var trs = con.BeginTransaction();
            try {
                string sqlFis = $" Delete From UretimStokFis where Id='{fisId}'";
                string sqlHrk = $" Delete From UretimStokFisHareket where FisId='{fisId}'";
                string sqlHrk2 = $" Delete From DepoStokHareket where FisId='{fisId}'";
                var rs = _dal.Execute(sqlFis, null, trs);
                var rs2 = _dal.Execute(sqlHrk, null, trs);
                var rs3 = _dal.Execute(sqlHrk2, null, trs);
                don = rs + " - " + rs2 + " - " +rs3;
                trs.Commit();
            } catch (Exception e) {
                trs.Dispose();
                return new ErrorDataResult<string>(e.Message);
            } finally {
                trs.Dispose();
            }
            return new SuccessDataResult<string>(don);
        }


        public IDataResult<string> StokKoduGuncelle(string eskiKod, string yeniKod) {
            string don = "";
            var con = GetConnection();
            if (con.State != ConnectionState.Open) con.Open();
            var trs = con.BeginTransaction();
            try {
                string sqlFis = $" Update DepoStokHareket Set StokKodu='{yeniKod}' where StokKodu='{eskiKod}'";
                string sqlHrk = $" Update UretimStok Set StokKodu='{yeniKod}' where StokKodu='{eskiKod}'";
                string sqlHrk2 = $" Update DepoStok Set StokKodu='{yeniKod}' where StokKodu='{eskiKod}'";
                string sqlHrk3 = $" Update DepoKabulIrsaliyeHareket Set StokKodu='{yeniKod}' where StokKodu='{eskiKod}'";
                string sqlHrk4 = $" Update DepoStokBarkod Set StokKodu='{yeniKod}' where StokKodu='{eskiKod}'";
                string sqlHrk5 = $" Update ReceteDetay Set VarsayilanStokKodu='{yeniKod}' where VarsayilanStokKodu='{eskiKod}'";
                string sqlHrk6 = $" Update SiparisHareket Set StokKodu='{yeniKod}' where StokKodu='{eskiKod}'";
                string sqlHrk7 = $" Update SiparisHareketDetay Set StokKodu='{yeniKod}' where StokKodu='{eskiKod}'";
                string sqlHrk8 = $" Update UretimStokFisHareket Set StokKodu='{yeniKod}' where StokKodu='{eskiKod}'";
                var rs = _dal.Execute(sqlFis, null, trs);
                var rs2 = _dal.Execute(sqlHrk, null, trs);
                var rs3 = _dal.Execute(sqlHrk2, null, trs);
                var rs4 = _dal.Execute(sqlHrk3, null, trs);
                var rs5 = _dal.Execute(sqlHrk4, null, trs);
                var rs6 = _dal.Execute(sqlHrk5, null, trs);
                var rs7 = _dal.Execute(sqlHrk6, null, trs);
                var rs8 = _dal.Execute(sqlHrk7, null, trs);
                var rs9 = _dal.Execute(sqlHrk8, null, trs);
                don = rs + " - " + rs2 + " - " + rs3;
                trs.Commit();
                } catch (Exception e) {
                trs.Dispose();
                return new ErrorDataResult<string>(e.Message);
                } finally {
                trs.Dispose();
                }
            return new SuccessDataResult<string>(don);
            }

        }
}
