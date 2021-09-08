using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iHubAdminAPI.Models
{
    public class VMGroupUser
    {
        public Guid? GroupID { get; set; }
        public List<string> UserIDs { get; set; }
        public List<VMDropDown> Groups { get; set; }
        public List<VMDropDown> Columns { get; set; }
        public List<VMTableColumns> Masters { get; set; }
        public Guid? ChannelID { get; set; }

        public string Action { get; set; }
        public string OrganizationID { get; set; }


        public string AspNetUserId { get; set; }
        public int TypeID { get; set; }

        public string GroupName { get; set; }
        public string Name { get; set; }
        public string Avatar { get; set; }
        public int? GroupChatMaster_Id { get; set; }
        public Guid Guid { get; set; }
        public string UserID { get; set; }
        public List<string> Devices { get; set; }

        public List<VMDropDown> AddedGroups { get; set; }
        public List<VMDropDown> RemovedGroups { get; set; }

        // public VMApplicationUser ModifiedUser { get; set; }
       
        public long LastMessageID { get; set; }
        public VMCustomResponse Response { get; set; }
        public bool IsGroup { get; set; }
        public bool IsChannel { get; set; }
    }
    public class VMUserGroups {
        public string UserID { get; set; }
        public List<Guid> GroupIDs { get; set; }
       // public List<VMRule> MappedGroups { get; set; }
        public List<VMDropDown> Groups { get; set; }
    }

    public class VMDropDown
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public long ParentID { get; set; }
        public Guid sID { get; set; }
    }

    public class VMTableColumns
    {
        public long ID { get; set; }
        public string ColumnName { get; set; }
        public string DisplayName { get; set; }
        public long? ParentColumnID { get; set; }
        public long? HirarchyID { get; set; }
        //public int DisplayIndex { get; set; }
        public int Type { get; set; }
        public string Value { get; set; }
        public long lValue { get; set; }
        public int columnIndex { get; set; }
        public bool IsInHierarchy { get; set; }
        public long Order { get; set; }
        public List<VMDropDown> MasterDatas { get; set; }
    }

    public class VMPayload
    {
        public string Type { get; set; }
        public string Action { get; set; }
        public string groupID { get; set; }
        public long MessageID { get; set; }
        public bool IsGroup { get; set; }
        public string UserID { get; set; }
        public int RequestID { get; set; }
    }
}
