using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace iHubAdminAPI.Models.ProductAndPrice
{
    [Table("iHub_EMI_Orders")]
    public class iHub_EMI_Orders
    {
        [Key]
        public int Order_ID { get; set; }
        public Int16 Tenure{get;set;}
    }
}