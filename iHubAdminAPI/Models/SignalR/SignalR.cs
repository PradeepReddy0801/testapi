using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iHubAdminAPI.Models
{
    public class User
    {
        [Key]
        public string UserName { get; set; }
        public ICollection<Connection> Connections { get; set; }
    }

    public class Connection
    {
        public string ConnectionID { get; set; }
        public string UserAgent { get; set; }
        public bool Connected { get; set; }
        //public DateTime CreatedOn { get; set; }
    }



    [Table("Groups")]
    public class Group : clsCommonProperties
    //   public class Group 
    {
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid GroupId { get; set; }
        public string GroupName { set; get; }
        public string Description { set; get; }
        public string ChannelOwner { get; set; }
        public string NohiphenGroupID { get; set; }
    }

    public class GroupUser : clsCommonProperties
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }
        public string AspNetUserId { get; set; }
        public string Name { get; set; }
        public string Devices { get; set; }
        public Guid GroupId { get; set; }
        public string GroupName { get; set; }
        public long LastMessageID { get; set; }

        [ForeignKey("GroupId")]
        public virtual Group Groups { get; set; }

    }

    public class UserContacts
    {
        public long ID { get; set; }
        public string UserID { get; set; }
        public string ContactUserID { get; set; }
        public int Status { get; set; }
        public bool IsChannel { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public string GroupName { get; set; }

    }

    public class Special_Request_Details
    {
        [Key]
        public int Details_ID { get; set; }
        public int RequestID { get; set; }
        public string UserIDs { get; set; }
        public DateTime CreatedDate { get; set; }
        public int Status { get; set; }
        public DateTime ModifiedDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }
    }

    [Table("iH_Customer_Special_Requested_Products")]
    public class Customer_Special_Requested_Products
    {
        [Key]
        public int Customer_Requested_ID { get; set; }
        public string Customer_Name { get; set; }
        public string Customer_Mobile_Number { get; set; }
        public string Product_Name { get; set; }
        public decimal Requested_Quantity { get; set; }
        public int Customer_Request_Status { get; set; }
        public string Description { get; set; }
        public DateTime Requested_Date { get; set; }
        public int Unit_Id { get; set; }
        public DateTime Last_Updated_Time { get; set; }
        public int UserID { get; set; }
        public int User_Type { get; set; }
        public int iCategoryID { get; set; }
    }
}
