using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace iHubAdminAPI.Models
{
    [Table("MAIL_SMS_Settings")]
    public class MailSMSSettings
    {
        [Key]
        public int ID { get; set; }
        public int iDomainID { get; set; }
        public string sTemplateType { get; set; }
        public string sType { get; set; }
        public string sSubject { get; set; }
        public string sFrom { get; set; }
        public string sTrackhere { get; set; }
        public string sHelpcenter { get; set; }
        public string sMobileNumber { get; set; }
        public string sEmail { get; set; }
        public string sBody { get; set; }
        public string sUrl { get; set; }
        public Boolean SMS { get; set; }
        public Boolean MAIL { get; set; }
        public string sdltid { get; set; }
    }
}