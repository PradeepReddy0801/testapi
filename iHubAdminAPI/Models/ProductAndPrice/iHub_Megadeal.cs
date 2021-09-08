using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iHubAdminAPI.Models.ProductAndPrice
{
    [Table("iHub_MegaDeals")]
    public class iHub_MegaDeals
    {
        [Key]
        public int MegaDeal_ID { get; set; }
        public decimal Deal_Price { get; set; }
        public DateTime Deal_ValidFrom { get; set; }
        public DateTime Deal_ValidTo { get; set; }
        public byte Status { get; set; }
        public int Product_ID { get; set; }
    }
}