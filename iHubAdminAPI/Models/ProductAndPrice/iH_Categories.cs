using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iHubAdminAPI.Models;
using System.Data.Entity;

namespace iHubAdminAPI.Models.ProductAndPrice
{

  
    [Table("iH_Categories")]
    public class iH_Categories
    {

        [Key]
        public Int32 ID { get; set; }
        public string CategoryName { get; set; }
        public Int32 ParentID { get; set; }
        public string Image { get; set; }
        public Int32? Status { get; set; }
        public Int16 CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
        public Int16 UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string Description { get; set; }
        public decimal? BookingPercentage { get; set; }
        public string AliasNames { get; set; }
        public Int32? Priority { get; set; }
        public Boolean? Is_LeafNode { get; set; }
        public decimal? BuyerPercentage { get; set; }
        public Int16? Top_Category_Id { get; set; }
        public Int16? Is_Batch_Number { get; set; }
        public Int16? Is_Manufacture_Date { get; set; }
        public Int16? Is_Expiry_Date { get; set; }
        public Int16? Is_IMEI_Number { get; set; }
        public iH_Categories()
        {
            this.SubCategories = new HashSet<iH_Categories>();
        }
        public int? MOQ { get; set; }
        [ForeignKey("ParentID")]
        //public virtual iH_Categorieslist MasterCategory { get; set; }
        public virtual ICollection<iH_Categories> SubCategories { get; set; }
        public List<VMModelsForCategory> Menulist { get; set; }
        public bool IsLeaf
        {
            get
            {
                return this.SubCategories.Count == 0;
            }
        }
    }
   
}