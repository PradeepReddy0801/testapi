using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace iHubAdminAPI.Models.AppModels
{
    [Table("iH_Stores_Keeper")]
    public class iHub_StoreAssistant_Agents
    {
        [Key]
        public Int16 Store_Keeper_ID { get; set; }
        public string Store_Keeper_UserName { get; set; }
        public int Store_User_Type { get; set; }
        public int Store_User_Status { get; set; }
        public DateTime Last_Updated_Date { get; set; }
        public Int16 Store_Keeper_User_Id { get; set; }
    }
}