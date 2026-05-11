using My.Core.Data;
using My.Core.Logger;
using My.Core.Result;
using My.DataAccess.Personeller;
using My.Entities.Personeller;
using System;
using System.Linq;

namespace My.Business.Service.Personeller
{
    public class PersonelService : BaseService<Personel>, IPersonelService
    {
        private IPersonelDal _dal;
        private ILogManager _ilogger;

        public PersonelService(IPersonelDal dal, ILogManager ilogger) : base(dal, ilogger)
        {
            _dal = dal;
            _ilogger = ilogger;
        }

        public IDataResult<string> PersonelKaydet(Personel personel, bool yenikayitmi)
        {
            try
            {
                var rsVar = KodVarmi<Personel>(personel, nameof(Personel.Kodu), yenikayitmi);
                if (!rsVar.Success)
                {
                    return new ErrorDataResult<string>(rsVar.Message);
                }

                string sql = Personel.GetInsOrUpdCode();
                var rs = _dal.Execute(sql, personel);
                return new SuccessDataResult<string>();
            }
            catch (Exception ex)
            {
                _ilogger.Error(ex.Message, "PersonelService.PersonelKaydet");
                return new ErrorDataResult<string>(ex.Message);
            }
        }
        public IDataResult<string> PersonelKaydetSifresiz(Personel personel, bool yenikayitmi)
        {
            try
            {
                var rsVar = KodVarmi<Personel>(personel, nameof(Personel.Kodu), yenikayitmi);
                if (!rsVar.Success)
                {
                    return new ErrorDataResult<string>(rsVar.Message);
                }
                string sql = Personel.GetUpdCodeSifresiz();
                var rs = _dal.Execute(sql, personel);
                return new SuccessDataResult<string>();
            }
            catch (Exception ex)
            {
                _ilogger.Error(ex.Message, "PersonelService.PersonelKaydetSifresiz");
                return new ErrorDataResult<string>(ex.Message);
            }
        }
        public string Sifrele64(string yazi1, string yazihash = "mikroconnect")
        {
            try
            {
                string yazi = yazi1 + yazihash;
                System.Security.Cryptography.MD5CryptoServiceProvider MD5şifreleyici = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] baytlar = System.Text.ASCIIEncoding.Default.GetBytes(yazi);
                byte[] hash = MD5şifreleyici.ComputeHash(baytlar);
                double kapasite = (hash.Length * 2 + (hash.Length / (double)8));
                System.Text.StringBuilder sb = new System.Text.StringBuilder((int)kapasite);
                int I;
                var loopTo = hash.Length - 1;
                for (I = 0; I <= loopTo; I++)
                    sb.Append(BitConverter.ToString(hash, I, 1));
                return sb.ToString().TrimEnd(new char[] { ' ' });
            }
            catch // (Exception ex)
            {
                return "";
            }
        }

        public virtual IResult KodVarmi<T>(T entity, string kontrolalan, bool yenikayitmi)
        {
            try
            {
                var tabloadi = ClassExtensions.GetClassTableName(typeof(T));
                var GetId = ClassExtensions.GetClassColumnNameKey(typeof(T));
                var sql2 = $" where  {GetId} <> @{GetId} and {kontrolalan} = @{kontrolalan} ";
                if (yenikayitmi) sql2 = $" where  {kontrolalan} = @{kontrolalan} ";
                var sql = string.Format("Select count(*) From {0}  {1};", tabloadi, sql2);
                var rs = _dal.Query<int>(sql, entity);
                if (rs.FirstOrDefault() > 0) return new ErrorResult("Aynı " + kontrolalan + " - Kodla Kayıt Var");
                return new SuccessResult();
            }
            catch (Exception ex)
            {
                return new ErrorResult(ex.Message);
            }

        }
    }
}