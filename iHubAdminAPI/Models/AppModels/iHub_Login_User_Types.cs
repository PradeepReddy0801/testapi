using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace iHubAdminAPI.Models.AppModels
{
    [Table("iHub_Login_User_Types")]
    public class iHub_Login_User_Types
    {
        public int ID { get; set; }
        public string User_Name { get; set; }
    }
}