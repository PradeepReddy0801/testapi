using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iHubAdminAPI.Models
{
    public class VMMessage
    {
        public string UniqueID { get; set; }
        public string SenderID { get; set; }
        public string ReceiverID { get; set; }
        public string ReceiverName { get; set; }
        public bool isGroupMessage { get; set; }
        public string OrgnizationID { get; set; }
        public string Text { get; set; }
        public string SenderName { get; set; }
        public string AttachementName { get; set; }
        public string FilePath { get; set; }
        public bool IsAttachement { get; set; }
        public string AttachementType { get; set; }
        public long ForwardFrom { get; set; }
        public long ReplyFor { get; set; }
        public bool IsForwadable { get; set; }
        public bool IsLocation { get; set; }
        public int RequestID { get; set; }
        [NotMapped]
        public string sReceivePath { get; set; }
    }
    public class VMChat
    {
        public string GroupID { get; set; }
        public bool IsGroup { get; set; }
        public List<VMChatMessages> Messages { get; set; }
        public int RequestID { get; set; }
    }
    public class VMChatMessages
    {
        public long id { get; set; }
        public long msgid { get; set; }
        public string senderName { get; set; }
        public string SenderID { get; set; }
        public string msg { get; set; }
        public string time { get; set; }
        public bool isRead { get; set; }
        public string guid { get; set; }
        public string receiver { get; set; }
        public int status { get; set; }
        public int uId { get; set; }
        public bool IsAttachement { get; set; }
        public bool isdownloaded { get; set; }
        public string AttachementType { get; set; }
        public string FileDisplayName { get; set; }
        public string Filepath { get; set; }
        public string ThumbnailPath { get; set; }
        public string Size { get; set; }
        public string Duration { get; set; }
        public bool IsForwadable { get; set; }
        public long ReplyFor { get; set; }
        public long ForwardFrom { get; set; }
        public string ReplyText { get; set; }
        public string ReplyThumb { get; set; }
    }
    public class VMCustomResponse
    {
        public List<string> lstString;

        public long ID { get; set; }
        public string PropertyName { get; set; }
        public string groupID { get; set; }
        public string ResponseMessage { get; set; }
        public bool IsException { get; set; }
        public bool Status { get; set; }
        public string sID { get; set; }
        public bool IsExceed { get; set; }
        public string ExceptionMessage { get; set; }
        public bool IsAttachement { get; set; }
        public string strUdf1 { get; set; }
        public string strUdf2 { get; set; }
        public long longUdf3 { get; set; }
        public string ReplyText { get; set; }
        public string ReplyThumb { get; set; }
        public string isGroup { get; set; }
        public int RequestID { get; set; }
        public string senderID { get; set; }
        public string senderName { get; set; }
    }

    public class Chat_Messages
    {
        public long ID { get; set; }
        public long ForwardFrom { get; set; }
        public long ReplyFor { get; set; }
        public bool isGroupMessage { get; set; }
        public bool IsForwadable { get; set; }
        public string SenderID { get; set; }
        public string SenderName { get; set; }
        public string ReceiverID { get; set; }
        public string MessageType { get; set; }
        public string Message { get; set; }
        public string Filepath { get; set; }
        public string OrganizationID { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool isSent { get; set; }
        public DateTime? SentDate { get; set; }
        public bool isRead { get; set; }
        public DateTime? ReadDate { get; set; }
        public int StatusTypeID { get; set; }
        public bool IsUploaded { get; set; }
        public string FileDisplayName { get; set; }
        public bool IsAttachement { get; set; }
        public bool IsLocation { get; set; }
    }

    public class Chat_MessageDetails
    {
        public long ID { get; set; }
        public long MessageID { get; set; }
        public long ForwardFrom { get; set; }
        public long ReplyFor { get; set; }
        public string ReplyText { get; set; }
        public string ReplyThumb { get; set; }
        public string ReplyFilePath { get; set; }
        public bool IsForwadable { get; set; }
        public bool isGroupMessage { get; set; }
        public string SenderID { get; set; }
        public string SenderName { get; set; }
        public string Message { get; set; }
        public string ReceiverGroupID { get; set; }
        public string ReceiverUserID { get; set; }
        public DateTime? SentDate { get; set; }
        public DateTime? ReadDate { get; set; }
        public bool isSent { get; set; }
        public bool isRead { get; set; }
        public int StatusTypeID { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Filepath { get; set; }
        public string FileDisplayName { get; set; }
        public bool IsAttachement { get; set; }
        public string ThumbnailPath { get; set; }
        public string AttachementType { get; set; }
        public bool IsUploaded { get; set; }
        public string Size { get; set; }
        public string Duration { get; set; }
        public bool IsLocation { get; set; }
        public bool isProcessed { get; set; }
        public int RequestID { get; set; }
    }

    public class Chat_AppEvents
    {
        public int ID { get; set; }
        public string Type { get; set; }
        public string GroupID { get; set; }
        public string GroupName { get; set; }
        public string UserID { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ExecutedDate { get; set; }
    }
}
