using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace iHubAdminAPI.Models
{
    [Table("Domain_District_Configuration")]
    public class DomainsDistConfiguration
    {
        [Key]
        public int ID { get; set; }
        public int DomainID { get; set; }
        public int DistID { get; set; }
        public int CityID { get; set; }
        public Boolean bShowDistrict { get; set; }
    }
}