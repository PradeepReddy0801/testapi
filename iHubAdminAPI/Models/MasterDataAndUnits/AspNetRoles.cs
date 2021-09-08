using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace iHubAdminAPI.Models.MasterDataAndUnits
{
    [Table("AspNetRoles")]
    public class AspNetRoles
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}