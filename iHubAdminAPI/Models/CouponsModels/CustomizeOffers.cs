using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iHubAdminAPI.Models.CouponsModels
{
    public class CustomizeOffers
    {
        public string UnitName { get; set; }
        public string MobileNumber { get; set; }
        public DateTime? OfferDateFrom { get; set; }
        public DateTime? OfferDateTo { get; set; }
        public int OfferStatus { get; set; }
        public int OfferType { get; set; }
        public int Offerid { get; set; }
        public int UnitID { get; set; }
        public decimal MinCartValue { get; set; }
        public decimal OfferAmount { get; set; }
        public int Pagesize { get; set; }
        public int Pageindex { get; set; }
        //Fields For Adding Company
        public int ID { get; set; }
        public string CompanyName { get; set; }
        public int BusinessTypeId { get; set; }
        public string DirectorName { get; set; }
        public string MailId { get; set; }
        public string Address { get; set; }
        public string Contact_Person_Name { get; set; }
        public string Contact_Person_MobileNumber { get; set; }
        public string Contact_Person_MailId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeMobileNumber { get; set; }
        public string EmployeeDesignation { get; set; }
        public string EmployeeMailid { get; set; }
        public int EmployeeStatus { get; set; }
        public int ApplicablePercentage { get; set; }
        public string CatIds { get; set; }
        public int TimesOfUse { get; set; }
        public int Applicableon { get; set; }

        public string Flag { get; set; }
        public string Corporate_DiscountDateFrom { get; set; }
        public string Corporate_DiscountDateTo { get; set; }
    }
}