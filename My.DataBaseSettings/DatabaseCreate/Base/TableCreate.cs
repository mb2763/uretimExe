 

namespace My.DatabaseSettings.Base
{

    public static class TableCreate
    {

        public static string SqlTablo(string tabloAdi, string idSutunAdi)
        {
            return $@"
            IF NOT  EXISTS (SELECT 1   FROM  INFORMATION_SCHEMA.TABLES   WHERE TABLE_TYPE='BASE TABLE'   AND TABLE_NAME='{tabloAdi}') 
            CREATE TABLE {tabloAdi} ( {idSutunAdi}  UNIQUEIDENTIFIER  PRIMARY KEY DEFAULT (NEWSEQUENTIALID()) ); ";

        } 
        public static string SqlTabloStringId(string tabloAdi, string idSutunAdi, int length=50)
        {
            return $@"
            IF NOT  EXISTS (SELECT 1   FROM  INFORMATION_SCHEMA.TABLES   WHERE TABLE_TYPE='BASE TABLE'   AND TABLE_NAME='{tabloAdi}') 
            CREATE TABLE {tabloAdi} ( {idSutunAdi}  VARCHAR({length})  PRIMARY KEY DEFAULT convert(varchar({length}), NEWID()) ); "; 
        }
    public static string SqlTabloIntId(string tabloAdi, string idSutunAdi,string turu=" VARCHAR(50) ")
        {
            return $@"
            IF NOT  EXISTS (SELECT 1   FROM  INFORMATION_SCHEMA.TABLES   WHERE TABLE_TYPE='BASE TABLE'   AND TABLE_NAME='{tabloAdi}') 
            CREATE TABLE {tabloAdi} ( {idSutunAdi}  INT  PRIMARY KEY IDENTITY ); "; 
        }

        public static string SqlSutun(string tabloAdi, string SutunAdi, string verituru, string defaultvalue = "")
        {
             
            string SUTUN_REPLACE_DEFAULT_DROP_QUERY = @" 
                   DECLARE @default_name varchar(256);
                   SELECT @default_name = [name] FROM sys.default_constraints WHERE parent_object_id=OBJECT_ID('{0}') AND COL_NAME(parent_object_id, parent_column_id)='{1}';
                   IF COALESCE(@default_name,'')<>'' BEGIN   EXEC('ALTER TABLE {0} DROP CONSTRAINT ' + @default_name);    END   ";

            string defValueIns = "";
            string defValueUpDrop = "";
            string defValueUpAdd = "";
            if (!string.IsNullOrEmpty(defaultvalue))
            {
                string dfName = $"[DF_{tabloAdi}_{SutunAdi}_Cons]"; 
                defValueIns = $"BEGIN ALTER TABLE {tabloAdi} ADD  CONSTRAINT {dfName}  DEFAULT ({defaultvalue}) FOR [{SutunAdi}] END;";
                defValueUpDrop = string.Format(SUTUN_REPLACE_DEFAULT_DROP_QUERY, tabloAdi, SutunAdi, defaultvalue);
                defValueUpAdd = $" BEGIN ALTER TABLE {tabloAdi} ADD DEFAULT  ({defaultvalue}) FOR [{SutunAdi}] END; ";

            }
            var donen = $@"  IF EXISTS(SELECT 1  FROM INFORMATION_SCHEMA.COLUMNS  WHERE TABLE_NAME = '{tabloAdi}' AND COLUMN_NAME = '{SutunAdi}')
            BEGIN   {defValueUpDrop}
             ALTER TABLE {tabloAdi} ALTER COLUMN {SutunAdi} {verituru} ;{defValueUpAdd}
             END
            else  BEGIN ALTER TABLE {tabloAdi} ADD {SutunAdi} {verituru} ; {defValueIns} END ";

            return donen;

        }
         
        public static string VarChar(int genislik)
        {

            return $"varchar({genislik})";
        }
        public static string String(int genislik)
        {

            return $"varchar({genislik})";
        }
        public static string VarCharMax() {

            return $"varchar(MAX)";
        } 
        public static string StringMax() {
            return $"varchar(MAX)";
        }
        public static string Int()
        {
            return $"int";
        }
        public static string Float()
        {
            return $"float";
        }
        public static string Double()
        {
            return $"float";
        }
        public static string SmallInt()
        {
            return $"smallint";
        }
        public static string DateTime()
        {
            return $"datetime";
        }
        public static string Guid()
        {
            return $"uniqueidentifier";
        }
        public static string UniqueIdentifier()
        {
            return $"uniqueidentifier";
        }
    }
}
