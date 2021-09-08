using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iHubAdminAPI.Models.MasterDataAndUnits
{
    public class VMiHubUnitsModel
    {
        public int Sub_Unit_ID { get; set; }
        public int Unit_Hierarchy_Level_ID { get; set; }
        public string Unit_Type { get; set; }
        public string Unit_Name { get; set; }
        public string Email { get; set; }
        public string Phone_Number { get; set; }
        public string ContactName { get; set; }
        public string AddressLine_One { get; set; }
        public string AddressLine_Two { get; set; }
        public int VillageLocation_ID { get; set; }
        public string Suggested_UserName { get; set; }
        public string Unit_Additional_Data { get; set; }
        public string Password { get; set; }
        public string Order_By { get; set; }
        public int Page_Index { get; set; }
        public int Page_Size { get; set; }
        public string BusinessDetails { get; set; }
        public string LocationDetails { get; set; }
        public string DocumentDetalis { get; set; }
        public string Gender { get; set; }
        public string Date_Of_Birth { get; set; }
        public string Qualification { get; set; }
        public int Address_Proof_ID { get; set; }
        public string Address_Proof_Id_Number { get; set; }
        public int isWHcumStore { get; set; }
        public int RoleId { get; set; }
        public string Address_Proof_ID_employee { get; set; }
    }
}