using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace iHubAdminAPI.Models
{
  

    [Table("CM_Coupons")]
    public class Coupons
    {
        [Key]
        public int CouponID { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        //[Remote("IsExistCategory", "CreateCoupon", HttpMethod = "POST", AdditionalFields = "CouponID", ErrorMessage = "Name already exists. Please enter a different Name.")]
        public string Code { get; set; }
        public string Type { get; set; }
        public string CouponDescription { get; set; }
        [Required]
        public decimal Value { get; set; }
        // public decimal MaxDiscountAmount { get; set; }
        //[RegularExpression("^\\d*[0-9]\\d*$", ErrorMessage = "Enter Only Positive Numbers")]
        //public decimal IntervalAmount { get; set; }
        public decimal MinOf { get; set; }
        public decimal MaxOf { get; set; }
        public string ValidFrom { get; set; }
        public string ValidTo { get; set; }
        public int StatusTypeID { get; set; }
        public int Status { get; set; }
        [Required]
        public int NumberOfTimes { get; set; }
        public string ProductIDsNew { get; set; }
        public string CategoryIDs { get; set; }
        public string BrandsIDS { get; set; }
        public decimal CartValue { get; set; }
        public string FreeproductIDS { get; set; }
        public string CouponType { get; set; }
        public string CouponGRoupChange { get; set; }
        public string Domains { get; set; }
        public string StoreIDS { get; set; }
        public int FreeProductsNumberoftimes { get; set; }
        public string UsersType { get; set; }
        public string IsApplied { get; set; }
        public string Product_id { get; set; }
        public string ClassName { get; set; }
        public int NoOfCoupons { get; set; }
        public int GenerateCoupon { get; set; }
        public string CouponIDs { get; set; }
        public int Couptimes { get; set; }
        public int Couptimesperuser { get; set; }
        public int GroupId { get; set; }
        public int Group_Id { get; set; }
        public string Generatenewdate { get; set; }
        public int ParTFranch { get; set; }
        public string DomainID { get; set; }
        public string FranchiseID { get; set; }
        public string CorporateID { get; set; }


        public bool IsUser { get; set; }
        public bool IsVisible { get; set; }
        public bool IsGroup { get; set; }
        public bool IsExclusive { get; set; }
        public bool IsCashback { get; set; }
        public string CouponBasedOn { get; set; }
        //public string CouponDependentOn { get; set; }
        public int ProviderID { get; set; }
        public bool IsProvider { get; set; }
        public string ItemIDs { get; set; }
        public int CreatedByID { get; set; }
        public DateTime CreatedOn { get; set; }
        public int UpdatedByID { get; set; }
        public DateTime UpdatedOn { get; set; }
        [NotMapped]
        public string UserIDs { get; set; }
        [NotMapped]
        public List<string> ItmeValues { get; set; }
        [NotMapped]
        public string ProviderName { get; set; }
        [NotMapped]
        public List<string> UserName { get; set; }
        [NotMapped]
        public int NumberOfTimesUsed { get; set; }
        [NotMapped]
        public List<LoginUsers> Users { get; set; }
        [NotMapped]
        public List<LoginUsers> CouponCount { get; set; }
        [NotMapped]
        public bool UserSpecifyWithoutUser { get; set; }
        [NotMapped]
        public int NumberOfCoupons { get; set; }
        public string CustomerMobileNumber { get; set; }
        public string IhubCouponType { get; set; }
        public string StoreID { get; set; }
        public string Unitname { get; set; }
        public int Pagesize { get; set; }
        public int Page_Index { get; set; }
    }

    [Table("CM_Coupon_User_Validations")]
    public class Coupon_User_Validations
    {
        [Key]
        public int ID { get; set; }
        public int CouponID { get; set; }
        public long UserID { get; set; }
        public int NumberOfTimesUsed { get; set; }
        public DateTime? LastUsedDate { get; set; }
    }
    public class LoginUsers
    {
        public int NumberOfTimesUsed { get; set; }
        public string UserName { get; set; }
    }
    public class Raffle
    {
        public string MobileNumber { get; set; }
        public int OrderId { get; set; }
        public string Order_DateFrom { get; set; }
        public string Order_DateTo { get; set; }
        public int Page_Size { get; set; }
        public int Page_Index { get; set; }
        public string Product_id { get; set; }
    }
    [Table("iH_RaffleCoupons")]
    public class RffleCoupon
    {
        [Key]
        public Int16 Raffle_ID { get; set; }
        public int Order_Id  { get; set; }
        public string Raffle_Code { get; set; }
        public DateTime Created_Date { get; set; }
        
    }
}