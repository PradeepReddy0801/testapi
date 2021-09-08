using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iHubAdminAPI.Models
{
    public class Consignments
    {
        //public Int64 ID { get; set; }
        //public string ConsignmentName { get; set; }
        public string LogisticPackets { get; set; }
        public string CreatedDate { get; set; }
        public string StartDate { get; set; }
        public string ReachedDate { get; set; }
        public Int64? Status { get; set; }
        public string VehicleDetails { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        //public Int32? NumberOfBoxes { get; set; }
        public string SourceName { get; set; }
        public string DestinationName { get; set; }
        public string Inventory_Product_IDs { get; set; }
        public Int16? From_UnitID { get; set; }
        public Int16? To_UnitID { get; set; }
        //public string Remarks { get; set; }
        //public DateTime? Last_Updated_Time { get; set; }

        public string Cons_Name { get; set; }
        public Int32? Cons_Status { get; set; }
        public string Source { get; set; }
        public string Destination { get; set; }
        public string CreateDate { get; set; }
        public string Source_UnitID { get; set; }
        public string Dest_UnitID { get; set; }

        public Int32 Consignment_ID { get; set; }
        public string Consignment_Name { get; set; }
        public Nullable<DateTime> Created_Date { get; set; }
        //public string Created_Date { get; set; }
        //public DateTime? Start_Date { get; set; }
        //public DateTime? Reached_Date { get; set; }
        public Int16 Consignment_Status { get; set; }
        public Int16 Source_Unit_Id { get; set; }
        public Int16 Destination_Unit_Id { get; set; }
        //public string Inventory_Product_IDs { get; set; }
        public string Inventory_Shrinkage_Ids { get; set; }
        public Int16 NumberOfBoxes { get; set; }
        public string Remarks { get; set; }
        public int Created_By { get; set; }
        public int Updated_By { get; set; }
        public string Vehicle_Details { get; set; }
        public string Consignment_TrackDetails_Json { get; set; }
        //public DateTime? Last_Updated_Time { get; set; }
        public int Pagesize { get; set; }
        public int Pageindex { get; set; }
        public int DomainID { get; set; }
        public string Source_Name { get; set; }
        public string Destination_Name { get; set; }


        public List<Consignments> Resultset { get; set; }
        // public IQueryable<ResultsTable> rlist { get; set; }
        public int TotalCount { get; set; }

    }
}