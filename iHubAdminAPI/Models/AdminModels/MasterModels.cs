using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace iHubAdminAPI.Models
{
  
    //=========================Model For Master 
    public class VMModelsForMaster

    {
        //to Update and edit MasterData
        public int? ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public int Createdby { get; set; }
        public int Updatedby { get; set; }
        public int StoreID { get; set; }
        //to Get Masters Location
        public int ParentID { get; set; }
        public int Parent_Master_ID { get; set; }
        public string Searchtext { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int Sub_Unit_ID { get; set; }
        public string Unit_Name { get; set; }
        public string Email { get; set; }
        public string Phone_Number { get; set; }
        public string ContactName { get; set; }
        public string AddressLine_One { get; set; }
        public string AddressLine_Two { get; set; }
        public int VillageLocation_ID { get; set; }
        public string Suggested_UserName { get; set; }
        public string Unit_Additional_Data { get; set; }
        public int isWHcumStore { get; set; }
        public string Password { get; set; }
        public string TableName { get; set; }
        public string ProductName { get; set; }
        public string SKU { get; set; }
        public int Unit_ID { get; set; }
        public string Unit_Type { get; set; }
        public string Type { get; set; }
        public int OrderID { get; set; }
        public string orderdatefrom { get; set; }
        public string orderdateto { get; set; }
        public int Pagesize { get; set; }
        public string expensedate { get; set; }
        public string expensestype { get; set; }
        public int pagesize { get; set; }
        public int pageindex { get; set; }
        public string JsonData { get; set; }
        public int ClusterFranchiseID { get; set; }
        public string FilterJson { get; set; }
        public int UserType { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string UsertypeName { get; set; }
        public string LocationType { get; set; }
        // To Save Save and Update Default_Messages

        public string Message { get; set; }

        //
        // Second payment
        public int StoreOTP { get; set; }
        public int Notification_ID { get; set; }
        public int ProductID { get; set; }
        public string Payment_Mode_Type { get; set; }
        public double Amount { get; set; }
        public string pamentjson { get; set; }
        public double Use_Wallet_Amount { get; set; }

        //------------cash Deposits------------------------
        public decimal Cash_Amount { get; set; }
        public int Cluster_ID { get; set; }
        public string Cluster_Name { get; set; }
        public string LocationIDs { get; set; }
        public string CreatedDate { get; set; }
        public string LastUpdateDate { get; set; }
        public int UnitId { get; set; }
        public string Date { get; set; }
        public int year { get; set; }
        public int month { get; set; }
        public int day { get; set; }
    }
    public class CatNamesList
    {
        public string AliasNames { get; internal set; }
        public int ID { get; set; }
        public string Name { get; set; }
        public int ParentID { get; set; }
        public string GSTPercentage { get; set; }
    }
}