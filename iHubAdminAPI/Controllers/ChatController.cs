
using iHubAdminAPI.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace iHubAdminAPI.Controllers
{
    public class ChatController
    {
        iHubDBContext dbContext = new iHubDBContext();
        public VMCustomResponse AddMessage(VMMessage model)
        {
            try
            {
                DateTime currDate = DateTime.Now;
                Chat_Messages message = new Chat_Messages();
                string Filepath = "", ThumbnailPath = "", FilePath = model.FilePath, AttaType = "";
                message.MessageType = model.IsAttachement ? "Attach" : "Text";
                message.Filepath = FilePath;
                message.IsAttachement = model.IsAttachement;
                message.isGroupMessage = model.isGroupMessage;
                message.OrganizationID = model.OrgnizationID;
                message.SenderID = model.SenderID.Replace("-", "");
                message.SenderName = model.SenderName;
                message.ReceiverID = model.ReceiverID.Replace("-", "");
                message.Message = model.Text;
                message.FileDisplayName = model.AttachementName;
                message.ForwardFrom = model.ForwardFrom;
                message.ReplyFor = model.ReplyFor;
                string ReplyText = "", ReplyThumb = "", ReplyFilePath = "";
                if (model.ReplyFor > 0)
                {
                    var md = dbContext.Chat_MessageDetails.Where(a => a.MessageID == model.ReplyFor).FirstOrDefault();
                    ReplyText = md.Message;
                    ReplyThumb = md.ThumbnailPath;
                    ReplyFilePath = md.Filepath;
                }
                message.IsForwadable = model.IsForwadable;
                message.IsUploaded = false;
                message.CreatedDate = currDate;
                message.isSent = false;
                message.isRead = false;
                message.StatusTypeID = 10;
                message.IsLocation = model.IsLocation;
                dbContext.Chat_Messages.Add(message);
                dbContext.SaveChanges();
                List<string> lstgroupUsers = new List<string>();
                string imgtpe = model.Text;
                string[] types = imgtpe.Split('.');
                if (model.ForwardFrom > 0)
                {
                    var forwadMessage = dbContext.Chat_MessageDetails.Where(async => async.MessageID == model.ForwardFrom).FirstOrDefault();
                    message.MessageType = forwadMessage.AttachementType;
                    Filepath = forwadMessage.Filepath;
                    AttaType = forwadMessage.AttachementType;
                    ThumbnailPath = forwadMessage.ThumbnailPath;
                    FilePath = Filepath;
                    message.IsAttachement = forwadMessage.IsAttachement;
                    model.Text = forwadMessage.Message;
                    model.AttachementName = forwadMessage.FileDisplayName;
                }
                else
                {
                    if (!string.IsNullOrEmpty(model.AttachementName))
                    {
                        Filepath = model.FilePath + message.ID + model.AttachementName.Substring(model.AttachementName.IndexOf("."));
                        AttaType = ServiceUtil.AttachmentType(model.AttachementName.Substring(model.AttachementName.LastIndexOf(".")));
                        if (AttaType == "Video" || AttaType == "Image")

                            //ThumbnailPath = model.FilePath + message.ID + "_Thumb.png";
                            ThumbnailPath = model.FilePath + message.ID + "_Thumb." + types[1];
                        else
                            ThumbnailPath = model.FilePath + message.ID + model.AttachementName.Substring(model.AttachementName.IndexOf("."));
                        FilePath = Filepath;
                    }
                    message.MessageType = model.IsAttachement ? "Attach" : "Text";
                    message.Filepath = FilePath;
                    message.IsAttachement = model.IsAttachement;
                }
                List<Chat_MessageDetails> messageDetails = new List<Chat_MessageDetails>();
                if (message.isGroupMessage)
                {
                    Guid id = (Guid)TypeDescriptor.GetConverter(typeof(Guid)).ConvertFromInvariantString(message.ReceiverID);
                    lstgroupUsers = dbContext.GroupUser.Where(a => a.GroupId == id).Select(a => a.AspNetUserId).ToList();//&& a.AspNetUserId != message.SenderID
                    if (message.IsAttachement == true && lstgroupUsers.Count() == 0)
                    {
                        if (dbContext.UserLogins.Where(x => x.noHiphenuserID == message.SenderID).Select(z => z.RoleName).FirstOrDefault().ToString() == "salesteamuser" &&
                        dbContext.UserLogins.Where(x => x.noHiphenuserID == message.ReceiverID).Select(z => z.RoleName).FirstOrDefault().ToString() == "iStoreAdmin")
                        {
                            var proc = (from gu in dbContext.UserContacts
                                        join us in dbContext.UserLogins on gu.ContactUserID equals us.noHiphenuserID
                                        where gu.Status == 10 && !gu.IsChannel && gu.GroupName == model.RequestID.ToString() && us.RoleName == "procurementuser"
                                        select new VMGroupUser { GroupName = gu.GroupName, UserID = gu.ContactUserID, Name = us.AspNetUserId }).FirstOrDefault();
                            lstgroupUsers.Add(proc.UserID);
                        }
                        lstgroupUsers.Add(message.ReceiverID); lstgroupUsers.Add(message.SenderID);
                    }
                }
                else
                {
                    if (dbContext.UserLogins.ToList().Where(x => x.noHiphenuserID == message.SenderID).Select(z => z.RoleName).FirstOrDefault().ToString() == "salesteamuser" &&
                        dbContext.UserLogins.ToList().Where(x => x.noHiphenuserID == message.ReceiverID).Select(z => z.RoleName).FirstOrDefault().ToString() == "iStoreAdmin")
                    {
                        var proc = (from gu in dbContext.UserContacts
                                    join us in dbContext.UserLogins on gu.ContactUserID equals us.noHiphenuserID
                                    where gu.Status == 10 && !gu.IsChannel && gu.GroupName == model.RequestID.ToString() && us.RoleName == "procurementuser"
                                    select new VMGroupUser { GroupName = gu.GroupName, UserID = gu.ContactUserID, Name = us.AspNetUserId }).FirstOrDefault();
                        lstgroupUsers.Add(proc.UserID);
                    }
                    lstgroupUsers.Add(message.ReceiverID); lstgroupUsers.Add(message.SenderID);
                }
                foreach (string user in lstgroupUsers)
                {
                    Chat_MessageDetails details = new Chat_MessageDetails();
                    details.MessageID = message.ID;
                    details.isGroupMessage = message.isGroupMessage;
                    details.SenderID = message.SenderID;
                    details.Message = message.Message;
                    details.SenderName = model.SenderName;
                    details.ReceiverGroupID = message.ReceiverID;
                    details.ReceiverUserID = user.Replace("-", ""); ;
                    details.Filepath = Filepath;
                    details.ForwardFrom = model.ForwardFrom;
                    details.ReplyFor = model.ReplyFor;
                    details.IsForwadable = model.IsForwadable;
                    details.IsUploaded = false;
                    details.Size = "";
                    details.Duration = "";
                    details.FileDisplayName = model.AttachementName;
                    details.IsAttachement = model.IsAttachement;
                    details.ThumbnailPath = ThumbnailPath;
                    details.AttachementType = AttaType;
                    details.ReplyText = ReplyText;
                    details.ReplyThumb = ReplyThumb;
                    details.ReplyFilePath = ReplyFilePath;
                    details.IsLocation = model.IsLocation;
                    message.IsUploaded = false;
                    details.isProcessed = false;
                    details.RequestID = model.RequestID;
                    if (dbContext.UserLogins.Where(x => x.noHiphenuserID == details.ReceiverUserID).Select(c => c.RoleName).FirstOrDefault() == "iStoreAdmin")
                    {
                        details.isProcessed = false;
                    }
                    if (details.SenderID == details.ReceiverUserID)
                    {
                        details.isSent = details.isRead = true;
                        details.SentDate = details.ReadDate = DateTime.Now;
                    }
                    else
                    {
                        details.isSent = false;
                        details.isRead = false;
                    }
                    details.CreatedDate = currDate;
                    details.StatusTypeID = 10;
                    messageDetails.Add(details);
                }
                dbContext.Chat_MessageDetails.AddRange(messageDetails);
                dbContext.SaveChanges();
                //var updateId = dbContext.GroupUser.Where(x => x.GroupId.ToString().Replace("-","") == model.ReceiverID && x.AspNetUserId.Replace("-", "") == model.SenderID).FirstOrDefault();
                //updateId.LastMessageID = message.ID;
                //dbContext.SaveChanges();

                return new VMCustomResponse()
                {
                    ID = message.ID,
                    IsException = false,
                    ResponseMessage = "Success",
                    lstString = lstgroupUsers,
                    PropertyName = FilePath,
                    sID = AttaType,
                    groupID = ThumbnailPath,
                    ReplyText = ReplyText,
                    ReplyThumb = ReplyThumb
                };
            }
            catch (Exception ex)
            {
                throw ex;
                //return new VMCustomResponse() { ID = 0, IsException = true, ResponseMessage = "Error Occured ! Try again later", ExceptionMessage = ex.Message };
            }
        }

        public List<Chat_AppEvents> CheckUserActiveEvents(string UserID)
        {
            try
            {
                var events = dbContext.Chat_AppEvents.Where(a => a.UserID == UserID && !a.Status).ToList();
                return events;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public VMCustomResponse UpdateEventStatus(int eventID)
        {
            try
            {
                var eve = dbContext.Chat_AppEvents.Find(eventID);
                if (eve != null)
                {
                    eve.Status = true;
                    eve.CreatedDate = DateTime.Now;
                    dbContext.SaveChanges();
                }
                return new VMCustomResponse() { ID = eventID, IsException = false, ResponseMessage = "Success" };
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public VMChat GetMessagesByGuid(string groupID, string UserID, long LastMessageID, bool IsGroup)
        {
            VMChat c = new VMChat();
            c.GroupID = groupID;
            c.IsGroup = IsGroup;
            if (IsGroup)
            {
                c.Messages = dbContext.Chat_MessageDetails
                    .Where(a => a.MessageID > LastMessageID && a.ReceiverGroupID == groupID && a.ReceiverUserID.Replace(@"-", "") == UserID
                    && a.isGroupMessage && a.StatusTypeID == 10).Select(x => new
                    {
                        ID = x.ID,
                        MessageID = x.MessageID,
                        SenderName = x.SenderName,
                        Message = x.Message,
                        SenderID = x.SenderID,
                        IsRead = x.isRead,
                        SentDate = x.CreatedDate,
                        IsAttachement = x.IsAttachement,
                        FileDisplayName = x.FileDisplayName,
                        Filepath = x.Filepath,
                        ThumbnailPath = x.ThumbnailPath,
                        AttachementType = x.AttachementType,
                        Size = x.Size,
                        Duration = x.Duration,
                        IsForwadable = x.IsForwadable,
                        ReplyFor = x.ReplyFor,
                        ForwardFrom = x.ForwardFrom,
                        ReplyText = x.ReplyText,
                        ReplyThumb = x.ReplyThumb,
                        //  IsLocation = x.IsLocation

                    }).ToList().Select(x => new VMChatMessages
                    {
                        id = 1,
                        msgid = x.MessageID,
                        msg = x.Message,
                        senderName = x.SenderName,
                        SenderID = x.SenderID,
                        isRead = x.IsRead,
                        guid = groupID,
                        isdownloaded = false,
                        receiver = "",
                        status = 1,
                        uId = 0,
                        time = x.SentDate.ToString("dd MMM yyyy hh:mm:ss tt"),
                        IsAttachement = x.IsAttachement,
                        FileDisplayName = x.FileDisplayName,
                        Filepath = x.Filepath,
                        ThumbnailPath = x.ThumbnailPath,
                        AttachementType = x.AttachementType,
                        Size = x.Size,
                        Duration = x.Duration,
                        IsForwadable = x.IsForwadable,
                        ReplyFor = x.ReplyFor,
                        ForwardFrom = x.ForwardFrom,
                        ReplyText = x.ReplyText,
                        ReplyThumb = x.ReplyThumb,
                        //IsLocation = x.IsLocation
                    }).ToList();
                return c;
            }
            else
            {
                groupID = groupID.Replace("-", "");
                c.Messages = dbContext.Chat_MessageDetails
                    .Where(a => a.MessageID > LastMessageID &&
                    ((a.SenderID == groupID && a.ReceiverGroupID == UserID) ||
                    (a.SenderID == UserID && a.ReceiverGroupID == groupID))
                    && !a.isGroupMessage && a.StatusTypeID == 10).Select(x => new
                    {
                        ID = x.ID,
                        MessageID = x.MessageID,
                        SenderName = x.SenderName,
                        Message = x.Message,
                        SenderID = x.SenderID,
                        IsRead = x.isRead,
                        SentDate = x.CreatedDate,
                        IsAttachement = x.IsAttachement,
                        FileDisplayName = x.FileDisplayName,
                        Filepath = x.Filepath,
                        ThumbnailPath = x.ThumbnailPath,
                        AttachementType = x.AttachementType,
                        Size = x.Size,
                        Duration = x.Duration,
                        IsForwadable = x.IsForwadable,
                        ReplyFor = x.ReplyFor,
                        ForwardFrom = x.ForwardFrom,
                        ReplyText = x.ReplyText,
                        ReplyThumb = x.ReplyThumb,
                        //  IsLocation = x.IsLocation
                    }).ToList().Select(x => new VMChatMessages
                    {
                        id = 1,
                        msgid = x.MessageID,
                        msg = x.Message,
                        senderName = x.SenderName,
                        SenderID = UserID,
                        isRead = x.IsRead,
                        guid = groupID,
                        receiver = "",
                        status = 1,
                        isdownloaded = false,
                        uId = 0,
                        time = x.SentDate.ToString("dd MMM yyyy hh:mm:ss tt"),
                        IsAttachement = x.IsAttachement,
                        FileDisplayName = x.FileDisplayName,
                        Filepath = x.Filepath,
                        ThumbnailPath = x.ThumbnailPath,
                        AttachementType = x.AttachementType,
                        Size = x.Size,
                        Duration = x.Duration,
                        IsForwadable = x.IsForwadable,
                        ReplyFor = x.ReplyFor,
                        ForwardFrom = x.ForwardFrom,
                        ReplyText = x.ReplyText,
                        ReplyThumb = x.ReplyThumb,
                        //IsLocation = x.IsLocation
                    }).ToList();
                return c;

            }
        }

        public List<string> GetGroupUsers(long ID, string groupID, bool IsGroup, string SenderID, string size, string duration)
        {
            try
            {
                dbContext.Chat_MessageDetails.Where(x => x.MessageID == ID).ToList().ForEach(a =>
                {
                    a.IsUploaded = true;
                    a.Size = size;
                    a.Duration = duration;
                });
                dbContext.SaveChanges();

                if (IsGroup)
                {
                    Guid id = (Guid)TypeDescriptor.GetConverter(typeof(Guid)).ConvertFromInvariantString(groupID);
                    return dbContext.GroupUser.Where(a => a.GroupId == id).Select(a => a.AspNetUserId).ToList();//&& a.AspNetUserId != SenderID
                }
                else
                {
                    return new List<string>() { groupID, SenderID };
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}