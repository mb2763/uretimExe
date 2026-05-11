 
using System;
using My.Core;
using My.Core.Data;

namespace My.Entities.MailAyarlar
{
    [Table("MailSettings")]
    public class MailAyar
    {
        public MailAyar Clone() { return (MailAyar)MemberwiseClone(); }
        [Key] public Guid Id { get; set; }
        public string MailKodu { get; set; }
        public string Host { get; set; }
        public string Port { get; set; }
        public string MailAdres { get; set; }
        public string Pass { get; set; }
        public string DisplayName { get; set; }
        public bool EnableSsl { get; set; }

        public string Konu { get; set; }
        public string Body { get; set; }

        public MailAyar()
        {
            Id = MyGuid.NewGuid();
            MailKodu = "";
            Host = "";
            Port = "";
            MailAdres = "";
            Pass = "";
            DisplayName = "";
            EnableSsl = false;
            Konu = "";
            Body = "";

        }
        public static string GetInsertCode()
        {
            string sql = @" INSERT INTO 
                MailSettings (  MailKodu, Host, Port , MailAdres ,Pass ,DisplayName,EnableSsl,Konu,Body) 
                VALUES       ( @MailKodu, @Host, @Port , @MailAdres ,@Pass ,@DisplayName,@EnableSsl,@Konu,@Body  )";
            return sql;
        }
        public static string GetUpdateCode()
        {
            string sql = @" UPDATE MailSettings SET MailKodu=@MailKodu, Host=@Host, Port =@Port, 
                        MailAdres=@MailAdres, Pass=@Pass ,DisplayName=@DisplayName,EnableSsl=@EnableSsl,
                        Konu= @Konu,Body = @Body
                        WHERE Id=@Id ";
            return sql;
        }
    }
}
