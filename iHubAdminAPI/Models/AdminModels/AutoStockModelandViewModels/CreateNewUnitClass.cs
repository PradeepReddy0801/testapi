using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iHubAdminAPI.Models.AutoStockModelandViewModels
{
    public class CreateNewUnitClass
    {
        public int ID { get; set; }
        public string ClassName { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public int Priority { get; set; }
        public decimal Budget { get; set; }
        public decimal ProductSellingMinPrice {  get; set; }
        public decimal ProductSellingMaxPrice { get; set; }
        public int DCStockAlertQuantity { get; set; }
        public int ProductSellingMaxQuantity { get; set; }
        public int LocationID { get; set; }
        public int CategoryID { get; set; }
        public int BrandMinProductsQuantity { get; set; }
    }
}