using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace iHubAdminAPI.Models
{
    public class VMResponseForSp
    {
        public List<Dictionary<string, object>> Resultset { get; set; }
        public List<Dictionary<string, object>> ResultsetTwo { get; set; }
        public List<Dictionary<string, object>> ResultsetThree { get; set; }
    }
}