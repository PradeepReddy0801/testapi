using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace iHubAdminAPI.Models
{
    [Table("Domains_Master_Configuration")]
    public class DomainsMasterData
    {
        [Key]
        public int ID { get; set; }
        public int DomainID { get; set; }
        public Boolean bMinShopValue { get; set; }
        public int iMinShopValue { get; set; }
        public string sAlertMessage { get; set; }
        public Boolean bDeliveryCharges { get; set; }
        public int iDCValue1 { get; set; }
        public int iDCValue2 { get; set; }
        public string sContactNumber { get; set; }
        public string sAlternateContactNumber { get; set; }
        public string sEmail { get; set; }
        public string sAddress { get; set; }
        public string sHeaderMessage { get; set; }
        public string sLogo { get; set; }
        public string sLogoTagLine { get; set; }
        public string sPageTitle { get; set; }
        public string sPaymentGateway { get; set; }       
		public int iDCDaysFrom { get; set; }
        public int iDCDaysTo { get; set; }
        public int iVDDaysFrom { get; set; }
        public int iVDDaysTo { get; set; }
        public int iWHDaysFrom { get; set; }
        public int iWHDaysTo { get; set; }
        public int iShowLocationType { get; set; }
    }
}