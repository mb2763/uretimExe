using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace My.DatabaseSettings.DatabaseCreate.DepoKabulCreateModule {
    public static class Create_ProcedureList
    {
        public static void Procedure_Create(this List<string> list)
        {
            
            /*    */
            
        }

        static void Recete_Guid_IdGuncelle(this List<string> list)
        {
            string sql = @"Create Or ALTER   PROCEDURE [dbo].[Recete_Guid_IdGuncelle]  @guid    uniqueidentifier
                         AS BEGIN SET NOCOUNT ON; 
                         	  update ReceteDetay set RcAId = (select Id From ReceteAna R where R.RcAGuid =[ReceteDetay].RcAGuid) where RcAGuid = @guid;  
                              update ReceteStok  set RcAId = (select Id From ReceteAna R where R.RcAGuid =[ReceteStok].RcAGuid) where RcAGuid =  @guid; 
                              update ReceteStok Set RcDId = (Select Id From ReceteDetay D where D.RcAGuid =[ReceteStok].RcAGuid and D.RcDGuid =[ReceteStok].RcDGuid)  where RcAGuid =  @guid;
                         END";
            list.Add(sql);
        }

    }
}