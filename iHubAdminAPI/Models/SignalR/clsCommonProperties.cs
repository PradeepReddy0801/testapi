using System;

namespace iHubAdminAPI.Models
{
    public class clsCommonProperties
    {

        public string CreatedByID { get; set; }
        public string ModifiedByID { get; set; }
        //[DefaultValue(typeof(DateTime),DateTime.Now)]
        public DateTime CreatedTime
        {
            get
            {
                return (_createdBy == DateTime.MinValue) ? DateTime.Now : _createdBy;
            }
            set { _createdBy = value; }
        }
        //[DefaultValue(DateTime.Now)]
        public DateTime ModifiedTime
        {
            get
            {
                return (_createdBy == DateTime.MinValue) ? DateTime.Now : _createdBy;
            }
            set { _createdBy = value; }
        }
        [System.ComponentModel.DefaultValue(10)]
        public int? StatusTypeID { get; set; }

        private DateTime _createdBy = DateTime.MinValue;

        // public string Description { get; set; }

        //public string CreatedByName { get; set; }
        //public string CreatedBySYSID { get; set; }
        //public string ModifiedByName { get; set; }
    }
}