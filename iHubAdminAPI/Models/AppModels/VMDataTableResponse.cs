using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace iHubAdminAPI.Models
{
    public class VMDataTableResponse
    {
        public List<Dictionary<string, string>> Resultset { get; set; }
        public List<Dictionary<string, string>> Addressset { get; set; }
        public int TotalCount { get; set; }
        public string sName { get; set; }
        public List<Dictionary<string, string>> Addressset2 { get; set; }
        public List<Dictionary<string, string>> Addressset3 { get; set; }
        public List<Dictionary<string, string>> Addressset4 { get; set; }
        public List<Dictionary<string, string>> Addressset5 { get; set; }
        public List<Dictionary<string, object>> Resultset1 { get; set; }
        public List<Dictionary<string, string>> Addressset1 { get; set; }
    }
}