using Dapper;
using My.Core.Data;
using My.Entities.Geneller;
using My.Entities.Mikro;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace My.DataAccess.Geneller
{
    public class GenelDal : BaseDal<Genel>, IGenelDal
    {
        public GenelDal(IDbConnection connection) : base(connection)
        {
        }

        public IEnumerable<string> GrupListesi(string tabloadi, string sutun)
        {
            var sql = " Select " + sutun + " as Kodu From " + tabloadi + " group by " + sutun + ";";

            return Connection.Query<string>(sql);
        }
      

        public string GetEvrakNo(string kodu)
        {
            var sql = @"           
           declare @Kodu varchar(25);  declare @Ara varchar(35);  declare @GnId uniqueidentifier;
           begin set @Ara = '" + kodu + @"';
           if exists(select Id from AyarSayac sy1 where (sy1.Kodu = @Ara)) 
           begin
               select top 1 @GnId = s1.Id ,@Kodu = coalesce(s1.BasinaEkle, '') + RIGHT(REPLICATE('0', 22) + cast(coalesce(s1.Verilecek, 1000) as varchar(22)), coalesce(s1.BasamakSayisi, 5))
           from AyarSayac s1   where (s1.Kodu = @Ara) 
           update AyarSayac set Verilecek = Verilecek + 1 where(Id = @GnId);
           select @Kodu as Kodu end
               ELSE    BEGIN select '' as Kodu; end end  ";
            var data = Connection.Query<Genel>(sql).FirstOrDefault();
            if (data == null) return "";
            if (data.Kodu == null) return "";
            return data.Kodu;
        }
    }
}