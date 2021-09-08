using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace iHubAdminAPI.Models
{
    [Table("Buyer_Categories")]
    public class Cls_Buyer_Categories
    {
        [Key]
        public int ID { get; set; }
        public int Buyer_ID { get; set; }
        public int CatID { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
    }
    [Table("Vendor_Categories")]
    public class Cls_Vendor_Categories
    {
        [Key]
        public int ID { get; set; }
        public int Vendor_ID { get; set; }
        public int CatID { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public string PIDsWithLP { get; set; }
        
    }

    [Table("Vendor_Categories_Products")]
    public class Cls_Vendor_Categories_Products
    {
        [Key]
        public int VCPID { get; set; }
        public int ID { get; set; }
        public int ProductID { get; set; }
        public decimal LandingPrice { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
    }

}