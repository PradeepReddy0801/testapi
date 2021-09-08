using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace iHubAdminAPI.Models.AppModels
{  
    [Table("AspNetUsers")]
    public class AspNetUsers
    {
        [Key]
        public int Id { get; set; }
        public string UserName { get; set; }
        public Boolean LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public DateTime Last_Updated_Datetime { get; set; }
        public string PhoneNumber { get; set; }
    }
}