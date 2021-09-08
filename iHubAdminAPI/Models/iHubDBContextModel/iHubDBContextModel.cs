using iHubAdminAPI.Models.ProductAndPrice;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;
using iHubAdminAPI.Models.AppModels;
namespace iHubAdminAPI.Models
{
    public class iHubDBContext : DbContext
    {
        public class chatContext : DbContext
        {
            public chatContext() : base("name=DefaultConnection") { }
            public DbSet<User> Users { get; set; }
            public DbSet<Connection> Connections { get; set; }
        }
        public iHubDBContext() : base("name=DefaultConnection") { }
        public DbSet<iH_Master_Locations> iH_Master_Locations { get; set; }
        public DbSet<iHub_Products> iHub_Products { get; set; }
        public DbSet<iH_Categories> iH_Categories { get; set; }
        public DbSet<iHub_MegaDeals> iHub_MegaDeals { get; set; }
        public DbSet<iHub_Units_DC_WH_ST> iHub_Units_DC_WH_ST { get; set; }
        public DbSet<Coupons> Coupons { get; set; }
        public DbSet<Coupon_User_Validations> Coupon_User_Validations { get; set; }
        public DbSet<RffleCoupon> RffleCoupon { get; set; }
        public DbSet<iHub_EMI_Orders> iHub_EMI_Orders { get; set; }
        public DbSet<iHub_Login_User_Types> iHub_Login_User_Types { get; set; }
        public DbSet<AspNetUsers> AspNetUsers { get; set; }
        public DbSet<iHub_StoreAssistant_Agents> iHub_StoreAssistant_Agents { get; set; }
        public DbSet<UserBasket_Products> UserBasket_Products { get; set; }
        public DbSet<iHub_Domains> iHub_Domains { get; set; }
        public DbSet<iHub_Inventory_Products> iHub_Inventory_Products { get; set; }
        public DbSet<Chat_Messages> Chat_Messages { get; set; }
        public DbSet<Chat_MessageDetails> Chat_MessageDetails { get; set; }
        public DbSet<Chat_AppEvents> Chat_AppEvents { get; set; }
        public DbSet<Group> Group { get; set; }
        public DbSet<GroupUser> GroupUser { get; set; }
        public DbSet<iHub_Menu_Strip> iHub_Menu_Strip { get; set; }
        public DbSet<iD_BottomLinks> iD_BottomLinks { get; set; }
        public DbSet<User_Packages> User_Packages { get; set; }
        public DbSet<iHub_Product_Ordered_Locations> iHub_Product_Ordered_Locations { get; set; }
        public DbSet<DomainsDistConfiguration> DomainsDistData { get; set; }
        public DbSet<DomainsMasterData> DomainsMasterData { get; set; }
        public DbSet<MailSMSSettings> MailSMSSettings { get; set; }
        public DbSet<UserLogins> UserLogins { get; set; }
        public DbSet<UserContacts> UserContacts { get; set; }
        public DbSet<iHub_RoleSubscriptions> iHub_RoleSubscriptions { get; set; }
        public DbSet<iHub_User_Subscriptions> iHub_User_Subscriptions { get; set; }
        public DbSet<Special_Request_Details> Special_Request_Details { get; set; }
        public DbSet<iD_Orders_Main> iD_Orders_Main { get; set; }
        public DbSet<Customer_Special_Requested_Products> Customer_Special_Requested_Products { get; set; }
        public DbSet<CacheTransactions> CacheTransactions { get; set; }
    }
    [Table("iD_Orders_Main")]
    //[DataContract(IsReference = true)]
    //[JsonObject(IsReference = false)]
    public class iD_Orders_Main
    {
        [Key]
        public int Orders_Main_ID { get; set; }
        public int Order_Number { get; set; }
        public int Buyer_Id { get; set; }
        public decimal Total_Sale_Amount { get; set; }
        public decimal Total_Paid_Amount { get; set; }
        public decimal Total_Due_Amount { get; set; }

    }
    public class iH_Master_Locations
    {
        iHubDBContext dbContext = new iHubDBContext();
        [Key]
        public int Location_ID { get; set; }
        public string AliasNames { get; set; }
        public int ParentID { get; set; }
        public iH_Master_Locations()
        {
            this.SubCategories = new HashSet<iH_Master_Locations>();
        }
        public int? Location_ParentID { get; set; }
        [ForeignKey("Location_ParentID")]
        public virtual iH_Master_Locations MasterCategory { get; set; }
        public virtual ICollection<iH_Master_Locations> SubCategories { get; set; }

        public bool IsLeaf
        {
            get
            {
                return this.SubCategories.Count == 0;
            }
        }

        public string Location_Name { get; set; }
        public int ID { get; internal set; }
    }

    public class UserBasket_Products
    {
        [Key]
        public int UserBasket_Product_ID { get; set; }
        public int UserBasketPakage_ID { get; set; }
        public int Product_ID { get; set; }
        public int Quantity { get; set; }
        public string Product_Name { get; set; }
        public bool isFreeProduct { get; set; }
        public int Cart_ID { get; set; }
        public int Order_Details_ID { get; set; }
        public int Order_ID { get; set; }
    }
    public class iHub_Domains
    {
        [Key]
        public int Site_ID { get; set; }
        public string Category_ID { get; set; }
        public string Search_FileName { get; set; }
        public string SearchKeyword { get; set; }
        public string sName { get; set; }
    }

    public class iHub_Inventory_Products
    {
        [Key]
        public int Inventory_Product_ID { get; set; }
        public int Product_Id { get; set; }
        public int Inventory_Product_Status { get; set; }
        public int Order_Id { get; set; }
        public int Consignment_Id { get; set; }
    }

    public class User_Packages
    {
        [Key]
        public int User_PackageID { get; set; }
        public int Package_ID { get; set; }
        public int Cart_ID { get; set; }
        public int Buyer_ID { get; set; }

    }

    public class iHub_Product_Ordered_Locations
    {
        [Key]
        public int Ordered_Location_ID { get; set; }
        public int Ordered_Product_Details_ID { get; set; }
        public Int16 Ordered_Product_Location_Status { get; set; }
        public int OrderNumber { get; set; }

    }

    [Table("iHub_RoleSubscriptions")]
    public class iHub_RoleSubscriptions
    {
        [Key]
        public int Role_Subscription_ID { get; set; }
        public int RoleID { get; set; }
        public int Notification_TypeID { get; set; }

    }

    [Table("iHub_User_Subscriptions")]
    public class iHub_User_Subscriptions
    {
        [Key]
        public int User_Subscription_ID { get; set; }
        public int UserID { get; set; }
        public int Notification_TypeID { get; set; }
        public int FK_Role_Subscription_ID { get; set; }

    }

    public class UserLogins
    {
        [Key]
        public string AspNetUserId { get; set; }
        public int UnitID { get; set; }
        public int UserID { get; set; }
        public string noHiphenuserID { get; set; }
        public string RoleName { get; set; }
    }

    [Table("CacheTransactions")]
    public class CacheTransactions
    {
        [Key]
        public int CacheTransactionID { get; set; }
        public string CacheKey { get; set; }
        public string Type { get; set; }
        public bool Status { get; set; }
        public DateTime Last_Updated_DateTime { get; set; }
    }
}