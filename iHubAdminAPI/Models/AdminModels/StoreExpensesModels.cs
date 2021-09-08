using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iHubAdminAPI.Models
{
    public class VMGetAllStoreExpenses
    {
        public string Unit_Name { get; set; }
        public int status { get; set; }
        public string expensedate { get; set; }
        public string expensestype { get; set; }
        // public int expenses_id { get; set; }
        public int pagesize { get; set; }
        public int pageindex { get; set; }
        public int Expenses_ID { get; set; }
        public int Expensed_UnitID { get; set; }
        public decimal Expensed_Amount { get; set; }
        public string Expensed_On { get; set; }
        public string Other_Expenses { get; set; }
        public DateTime Expensed_Date { get; set; }
        public int Expens_Status { get; set; }
        public string Expenses_Description { get; set; }
        public int Totalcount { get; set; }

        public int Hierarchy_Level { get; set; }
        public string Unit_Type { get; set; }
        public string Role_Name { get; set; }
        public int Source_Unit_ID { get; set; }
        public int UnitID { get; set; }
        public int CategoryID { get; set; }
        public string ProductName { get; set; }
        // public int SKU { get; set; }
        public Int16 iHub_Unit_ID { get; set; }
        public Int16 Distribution_Channel_ID { get; set; }
        public int iHub_Product_ID { get; set; }
        public string Product_Name { get; set; }
        public string Product_Series { get; set; }
        public int Qty { get; set; }
        public int Previous_UnitID { get; set; }

        public int Source_UnitID { get; set; }
        public int Dest_UnitID { get; set; }
        public int productid { get; set; }
        public int Consignment_ID { get; set; }
        public string Cons_Name { get; set; }
        public DateTime Created_Date { get; set; }
        public string Consignment_Name { get; set; }
        public int Previous_Unit_Id { get; set; }
        public int Unit_Id { get; set; }
        public string Source_Name { get; set; }
        public string Destination_Name { get; set; }
        public Int16 Inventory_Product_Status { get; set; }
        public int unitid { get; set; }
        public Int16? Unit_Hierarchy_Level { get; set; }


        //intrastore
        public int OrderID { get; set; }
        public string OrderDate { get; set; }
        public int Status { get; set; }
        public string Source { get; set; }
        public string Destination { get; set; }
        public int Order_Id { get; set; }
        public DateTime Order_Date { get; set; }
        public string SKU { get; set; }
        public int count { get; set; }


    }
}