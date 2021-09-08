using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iHubAdminAPI.Models
{
    public class Vm_RoleCategories
    {
        public Int16 Top_Category_Id { get; set; }
        public bool Is_LeafNode { get; set; }
        public bool Status { get; set; }
        public bool TaskAssigned { get; set; }
        public int TaskID { get; set; }
        public int iHub_Product_ID { get; set; }
        public decimal Landing_Price { get; set; }
        public int ParentTaskID { get; set; }
        public String TaskName { get; set; }
        public String RoleID { get; set; }
        public int MOQ { get; set; }
       // public int CatID { get; set; }
        public List<VM_PIDsWithLP> PIDsWithLP { get; set; }
        public List<Vm_RoleCategories> Categories { get; set; }
    }
    public class VM_PIDsWithLP
    {
        public int iHub_Product_ID { get; set; }
        public decimal Landing_Price { get; set; }
        public int CatID { get; set; }
        public decimal Selling_Price { get; set; }
        public decimal MRP { get; set; }
        public string ProductCode { get; set; }
        public string Product_Name { get; set; }
        public int HSN_Code { get; set; }
        public decimal GST_Percentage { get; set; }
        public int Inventory_Count { get; set; }
        public int Count { get; set; }

    }

}
