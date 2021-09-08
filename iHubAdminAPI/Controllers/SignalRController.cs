
using AspNet.Identity.SQLDatabase;
using iHubAdminAPI.ChatHub;
using iHubAdminAPI.Models;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using static iHubAdminAPI.Models.iHubDBContext;

namespace iHubAdminAPI.Controllers
{
    [RoutePrefix("api/SignalR")]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class SignalRController : ApiController
    {
        internal static SqlDependency dependency = null;
        public SQLDatabase _database;
        ChatController chatController = new ChatController();
        iHubDBContext dbContext = new iHubDBContext();

        public SignalRController()
        {
            _database = new SQLDatabase();
        }

        [HttpPost]
        [Route("SendMessage")]
        public IHttpActionResult SendMessage(VMMessage vmModel)
        {
            try
            {
                string tempReciverID = vmModel.ReceiverID;
                if (!vmModel.isGroupMessage)
                    vmModel.ReceiverID = vmModel.ReceiverID.Replace("-", "");

                if (string.IsNullOrEmpty(vmModel.Text) && vmModel.IsAttachement)
                {
                    vmModel.Text = "1 Attachement";
                }
                vmModel.FilePath = "";
                DateTime currDate = DateTime.Now;
                // var update = dbContext.Special_Request_Details.Where(x => x.RequestID == vmModel.RequestID).FirstOrDefault();
                //update.LastUpdatedDate = currDate;
                // dbContext.SaveChanges();
                if (vmModel.IsAttachement && vmModel.isGroupMessage)
                {
                    vmModel.FilePath = "Attachement/GroupAttach/" + currDate.Year + "/" + currDate.Month + "/" + vmModel.ReceiverID + "/";
                }
                if (vmModel.IsAttachement && !vmModel.isGroupMessage)
                {
                    vmModel.FilePath = "Attachement/UserAttach/" + currDate.Year + "/" + currDate.Month + "/" + vmModel.SenderID + "/";
                }
                if (string.IsNullOrEmpty(vmModel.AttachementName))
                    vmModel.AttachementName = "";
                var response = chatController.AddMessage(vmModel);
                string thumbpath = response.groupID;
                if (!vmModel.IsAttachement || (vmModel.IsForwadable && vmModel.IsAttachement))
                {
                    if (response.PropertyName != null && response.PropertyName != "")
                    {
                        string folderpath = response.PropertyName.Substring(0, response.PropertyName.LastIndexOf("/"));
                        string physicalPath = HttpContext.Current.Server.MapPath("~/" + folderpath);
                        if (!System.IO.File.Exists(physicalPath))
                        {
                            Directory.CreateDirectory(physicalPath);
                        }
                        string Destination = HttpContext.Current.Server.MapPath("~/" + response.PropertyName);
                        string Source = HttpContext.Current.Server.MapPath("~/" + vmModel.sReceivePath);
                        System.IO.File.Copy(Source, Destination);
                    }

                    if ((vmModel.ForwardFrom > 0 && vmModel.IsAttachement))
                        vmModel.IsAttachement = true;
                    response.lstString = response.lstString.Distinct().ToList();
                    if (!ServiceUtil.IslstNullOrEmpty(response.lstString))
                    {

                        using (var db = new chatContext())
                        {
                            var hubContext = GlobalHost.ConnectionManager.GetHubContext<StoreChattinghub>();
                            foreach (var reciever in response.lstString)
                            {
                                var user = db.Users.Find(reciever.Replace(@"-", ""));
                                if (user == null)
                                {
                                    //Clients.Caller.showErrorMessage("Could not find that user.");
                                }
                                else
                                {
                                    db.Entry(user).Collection(u => u.Connections).Query().Where(c => c.Connected == true).Load();

                                    if (user.Connections == null)
                                    {

                                        //pushnotification Logic
                                        try
                                        {

                                            //ServiceUtil.SendNotification(user.UserName, vmModel.SenderName + ":" + vmModel.Text);
                                        }
                                        catch (Exception ex)
                                        {

                                        }
                                    }
                                    else
                                    {
                                        if (dbContext.UserLogins.Where(x => x.noHiphenuserID == vmModel.SenderID).Select(z => z.RoleName).FirstOrDefault().ToString() == "salesteamuser" &&
                                            dbContext.UserLogins.Where(x => x.noHiphenuserID == vmModel.ReceiverID).Select(z => z.RoleName).FirstOrDefault().ToString() == "iStoreAdmin" &&
                                            dbContext.UserLogins.Where(x => x.noHiphenuserID == reciever).Select(z => z.RoleName).FirstOrDefault().ToString() == "procurementuser")
                                        {
                                            foreach (var connection in user.Connections)
                                            {
                                                hubContext.Clients.Client(connection.ConnectionID)
                                                    .addNewMessage(vmModel.Text, vmModel.ReceiverID,
                                                    vmModel.SenderName, tempReciverID, vmModel.RequestID,
                                                    vmModel.isGroupMessage,
                                                    DateTime.Now.ToString("dd MMM yyyy hh:mm:ss tt"),
                                                    response.ID,
                                                    vmModel.IsAttachement,
                                                    response.PropertyName,
                                                    vmModel.AttachementName,
                                                    thumbpath,
                                                    response.sID, 0, 0, vmModel.ReplyFor, response.ReplyText, response.ReplyThumb, 0);
                                                //logger.Info(vmModel.SenderName + " Sends SignalR event to  :" + user.UserName);
                                            }
                                        }
                                        else
                                        {
                                            foreach (var connection in user.Connections)
                                            {
                                                hubContext.Clients.Client(connection.ConnectionID)
                                                    .addNewMessage(vmModel.Text, vmModel.SenderID,
                                                    vmModel.SenderName, tempReciverID, vmModel.RequestID,
                                                    vmModel.isGroupMessage,
                                                    DateTime.Now.ToString("dd MMM yyyy hh:mm:ss tt"),
                                                    response.ID,
                                                    vmModel.IsAttachement,
                                                    response.PropertyName,
                                                    vmModel.AttachementName,
                                                    thumbpath,
                                                    response.sID, 0, 0, vmModel.ReplyFor, response.ReplyText, response.ReplyThumb, 0);
                                                //logger.Info(vmModel.SenderName + " Sends SignalR event to  :" + user.UserName);
                                            }
                                        }

                                    }
                                }
                            }
                        }
                    }
                }
                return Ok(new VMCustomResponse()
                {
                    ID = response.ID,
                    sID = vmModel.UniqueID,
                    groupID = tempReciverID,
                    IsException = false,
                    RequestID = vmModel.RequestID,
                    ResponseMessage = "Success",
                    IsAttachement = vmModel.IsAttachement,
                    PropertyName = response.PropertyName,//file path
                    ExceptionMessage = response.sID,//attachement Type
                    IsExceed = vmModel.isGroupMessage,
                    strUdf1 = thumbpath,
                    strUdf2 = vmModel.AttachementName,
                    longUdf3 = vmModel.ReplyFor,
                    ReplyText = response.ReplyText,
                    ReplyThumb = response.ReplyThumb
                });
            }
            catch (Exception ex)
            {
                //logger.Info("SendMessage:" + ex.Message);
                return Ok(new VMCustomResponse() { ID = 0, IsException = true, sID = vmModel.UniqueID, groupID = vmModel.ReceiverID, ResponseMessage = "Error Occured ! Try again later", ExceptionMessage = ex.Message });
            }
        }


        [HttpPost]
        [Route("CreateChannel")]
        public VMCustomResponse CreateChannel(VMGroupUser vmModel)
        {
            try
            {
                var cntgroup = dbContext.Group.Where(a => a.GroupName == vmModel.GroupName).Count();
                if (cntgroup > 0)
                {
                    return new VMCustomResponse() { IsException = false, ResponseMessage = "Channel Already Exist" };
                }
                else if (vmModel.UserIDs.Count > 0)
                {
                    Group grp = new Group();
                    grp.GroupName = vmModel.GroupName;
                    grp.ChannelOwner = vmModel.UserID;
                    grp.CreatedByID = grp.ModifiedByID = vmModel.UserID;
                    grp.CreatedTime = grp.ModifiedTime = DateTime.Now;
                    grp.StatusTypeID = 10;
                    grp.GroupId = Guid.NewGuid();
                    grp.NohiphenGroupID = grp.GroupId.ToString().Replace("-", "");
                    dbContext.Group.Add(grp);
                    dbContext.SaveChanges();
                    List<GroupUser> grpUsers = new List<GroupUser>();
                    GroupUser u = new GroupUser();
                    foreach (var user in vmModel.UserIDs)
                    {
                        u = new GroupUser();
                        u.GroupId = grp.GroupId;
                        u.ModifiedByID = u.CreatedByID = vmModel.UserID;
                        u.CreatedTime = u.ModifiedTime = DateTime.Now;
                        u.AspNetUserId = user;
                        u.StatusTypeID = 10;
                        u.GroupName = vmModel.GroupName;
                        grpUsers.Add(u);
                        dbContext.GroupUser.Add(u);
                        dbContext.SaveChanges();
                    }
                    //u = new GroupUser();
                    //u.GroupId = grp.GroupId;
                    //u.ModifiedByID = u.CreatedByID = vmModel.UserID;
                    //u.CreatedTime = u.ModifiedTime = DateTime.Now;
                    //u.AspNetUserId = vmModel.UserID;
                    //u.StatusTypeID = 10;
                    //grpUsers.Add(u);
                    //dbContext.GroupUser.AddRange(grpUsers);
                    //dbContext.SaveChanges();

                    return new VMCustomResponse() { IsException = false, sID = grp.GroupId.ToString(), lstString = vmModel.UserIDs };
                }
                return new VMCustomResponse() { IsException = false };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [Route("AddContactsToUsers")]
        public VMCustomResponse AddContactsToUsers(VMGroupUser user)
        {
            try
            {
                List<UserContacts> lstContacts = new List<UserContacts>();
                List<Chat_AppEvents> lstEvents = new List<Chat_AppEvents>();
                Special_Request_Details srd = new Special_Request_Details();
                foreach (var contact in user.UserIDs)
                {
                    UserContacts con = new UserContacts();
                    con.IsChannel = false;
                    con.Status = 10;
                    con.ContactUserID = contact.Replace("-", "");
                    var iContactUserID = con.ContactUserID;
                    con.UserID = user.UserID.ToString().Replace("-", "");
                    con.CreatedBy = con.UpdatedBy = "superadmin@inativetech.com";
                    con.UpdatedOn = con.CreatedOn = DateTime.Now;
                    con.GroupName = user.GroupName;
                    lstContacts.Add(con);
                    Chat_AppEvents eve = new Chat_AppEvents();
                    eve.CreatedDate = DateTime.Now;
                    eve.GroupID = con.ContactUserID;
                    eve.GroupName = dbContext.UserLogins.Where(a => a.noHiphenuserID == iContactUserID).SingleOrDefault().RoleName;
                    eve.UserID = con.UserID;
                    eve.Status = false;
                    eve.Type = "ContactAdded";
                    lstEvents.Add(eve);

                    con = new UserContacts();
                    con.IsChannel = false;
                    con.Status = 10;
                    con.ContactUserID = user.UserID.Replace("-", "");
                    con.UserID = contact.Replace("-", "");
                    con.GroupName = user.GroupName;
                    con.CreatedBy = con.UpdatedBy = "superadmin@inativetech.com";
                    con.UpdatedOn = con.CreatedOn = DateTime.Now;
                    lstContacts.Add(con);
                    eve = new Chat_AppEvents();
                    eve.CreatedDate = DateTime.Now;
                    eve.GroupID = con.ContactUserID;
                    eve.UserID = con.UserID;
                    eve.Status = false;
                    eve.GroupName = dbContext.UserLogins.Where(a => a.noHiphenuserID == iContactUserID).SingleOrDefault().RoleName;
                    eve.Type = "ContactAdded";
                    lstEvents.Add(eve);
                    srd.UserIDs = srd.UserIDs + "," + contact.Replace("-", "");
                }
                srd.UserIDs = srd.UserIDs + "," + user.UserID.Replace("-", "");
                srd.UserIDs = srd.UserIDs.Substring(1);
                srd.RequestID = Convert.ToInt32(user.GroupName);
                srd.CreatedDate = srd.ModifiedDate = srd.LastUpdatedDate = DateTime.Now;
                srd.Status = 1;
                dbContext.Special_Request_Details.Add(srd);
                dbContext.UserContacts.AddRange(lstContacts);
                dbContext.Chat_AppEvents.AddRange(lstEvents);
                dbContext.SaveChanges();
                return new VMCustomResponse() { IsException = false };
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet]
        [Route("GetGuidForUser")]
        public IHttpActionResult GetGuidForUser(int userid)
        {
            var inList = dbContext.UserLogins.Where(x => x.UserID == userid).FirstOrDefault();
            if (inList == null)
            {
                UserLogins login = new UserLogins();
                login.UserID = userid;
                login.AspNetUserId = Guid.NewGuid().ToString();
                login.noHiphenuserID = login.AspNetUserId.Replace("-", "");
                dbContext.UserLogins.Add(login);
                dbContext.SaveChanges();
                return Ok(login.AspNetUserId);
            }
            else
            {
                return Ok(inList.AspNetUserId);
            }

        }

        [HttpGet]
        [Route("GetGroupsByUserID")]
        public List<VMGroupUser> GetGroupsByUserID(string noHiphenuserID)
        {
            try
            {
                List<VMGroupUser> contacts = new List<VMGroupUser>();
                var contacts1 = (from gu in dbContext.UserContacts.Where(m => m.UserID == noHiphenuserID.Replace("-", ""))
                                 join us in dbContext.UserLogins on gu.ContactUserID equals us.noHiphenuserID
                                 where gu.Status == 10 && !gu.IsChannel
                                 select new VMGroupUser { GroupName = gu.GroupName, UserID = gu.ContactUserID, Name = us.AspNetUserId }).ToList();
                //contacts = contacts1;
                var sRoleName = dbContext.UserLogins.ToList().Where(m => m.AspNetUserId == noHiphenuserID).ToList().Select(z => z.RoleName).FirstOrDefault();
                if (sRoleName != null && sRoleName == "procurementuser")
                //if (dbContext.UserLogins.Where(x => x.AspNetUserId == noHiphenuserID).Select(z => z.RoleName).FirstOrDefault().ToString() == "procurementuser")
                {
                    foreach (var item in contacts1)
                    {
                        contacts.Add(item);
                        VMGroupUser store = new VMGroupUser();
                        store = (from gu in dbContext.UserContacts.Where(m => m.UserID == item.UserID.ToString())
                                 join us in dbContext.UserLogins on gu.ContactUserID equals us.noHiphenuserID
                                 where gu.Status == 10 && !gu.IsChannel && gu.GroupName == item.GroupName && us.RoleName == "iStoreAdmin"
                                 select new VMGroupUser { GroupName = gu.GroupName, UserID = gu.ContactUserID, Name = us.AspNetUserId }).FirstOrDefault();
                        contacts.Add(store);
                    }
                }
                else
                {
                    contacts = contacts1;
                }
                foreach (var item in contacts)
                {
                    item.Guid = (Guid)TypeDescriptor.GetConverter(typeof(Guid)).ConvertFromInvariantString(item.UserID);
                }
                //var groupContacts = (from gu in dbContext.GroupUser.Where(m => m.AspNetUserId == noHiphenuserID)
                //                     join us in dbContext.UserLogins on gu.AspNetUserId equals us.AspNetUserId
                //                     join grp in dbContext.Group on gu.GroupId equals grp.GroupId
                //                     where grp.StatusTypeID == 10 && gu.StatusTypeID == 10
                //                     select new VMGroupUser { GroupName = grp.GroupName, Guid = gu.GroupId, Name = us.UserID.ToString(), IsGroup = true }).ToList();
                return contacts;
            }
            catch
            {
                throw;
            }
        }

        [HttpGet]
        [Route("GetGUID")]
        public IHttpActionResult GetGUID()
        {
            try
            {
                var cmd = "select asp.Id,anr.Name as RoleName from AspNetUsers asp join AspNetUserRoles rol on rol.UserId = asp.Id inner join AspNetRoles anr on rol.RoleId = anr.Id where rol.RoleId in (35, 36, 8)";
                Dictionary<string, object> parameter1 = new Dictionary<string, object> { };
                var res = _database.SelectQuery(cmd, parameter1);
                foreach (var item in res)
                {
                    UserLogins login = new UserLogins();
                    login.UserID = Convert.ToInt32(item["Id"]);
                    login.AspNetUserId = Guid.NewGuid().ToString();
                    login.noHiphenuserID = login.AspNetUserId.Replace("-", "");
                    login.RoleName = item["RoleName"];
                    dbContext.UserLogins.Add(login);
                    dbContext.SaveChanges();
                }
                return Ok(1);
            }
            catch
            {
                throw;
            }
        }

        [Route("Payload")]
        [HttpPost]
        public IHttpActionResult Payload(VMPayload model)
        {
            try
            {
                string userID = model.UserID.Replace("-", "");

                SetIsSendStatusForMessage_Details(model.MessageID, userID, model.groupID);
                SetIsSendStatusForMessage(model.MessageID);

                return Ok();
            }
            catch (Exception ex)
            {
                return Ok("InternalServer Error");
            }
        }

        public void UpdateSendStatus(long MessageID, string UserID, string GroupID)
        {
            SetIsSendStatusForMessage_Details(MessageID, UserID, GroupID);
            SetIsSendStatusForMessage(MessageID);
        }
        public void SetIsSendStatusForMessage_Details(long MessageID, string UserID, string GroupID)
        {
            try
            {
                var msg_details = dbContext.Chat_MessageDetails.Where(a => a.MessageID <= MessageID && !a.isSent && a.ReceiverGroupID == GroupID && a.ReceiverUserID == UserID && a.StatusTypeID == 10);
                if (msg_details != null)
                {
                    foreach (var detail in msg_details)
                    {

                        detail.isSent = true;
                        detail.SentDate = DateTime.Now;
                    }
                    dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {

            }
        }

        public void SetIsSendStatusForMessage(long MessageID)
        {
            try
            {
                var cnt = dbContext.Chat_MessageDetails.Where(a => a.MessageID == MessageID && !a.isSent).Count();
                if (cnt == 0)
                {
                    var msg = dbContext.Chat_Messages.Find(MessageID);
                    if (msg != null)
                    {
                        msg.isSent = true;
                        msg.SentDate = DateTime.Now;
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        [HttpPost]
        [Route("GetUserMessagesByGroup")]
        public VMChat GetUserMessagesByGroup(VMPayload model)
        {
            try
            {
                VMChat c = new VMChat();
                c.GroupID = model.groupID;
                c.IsGroup = model.IsGroup;
                c.RequestID = model.RequestID;
                if (model.IsGroup)
                {
                    c.Messages = dbContext.Chat_MessageDetails
                        .Where(a => a.MessageID > model.MessageID && a.ReceiverGroupID == model.groupID && a.ReceiverUserID == model.UserID
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
                            ReplyThumb = x.ReplyThumb
                        }).ToList().Select(x => new VMChatMessages
                        {
                            id = 1,
                            msgid = x.MessageID,
                            msg = x.Message,
                            senderName = x.SenderName,
                            SenderID = x.SenderID,
                            isRead = x.IsRead,
                            guid = model.groupID,
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
                            ReplyThumb = x.ReplyThumb
                        }).ToList();
                    return c;
                }
                else
                {
                    if (dbContext.UserLogins.ToList().Where(x => x.noHiphenuserID == model.groupID).Select(z => z.RoleName).FirstOrDefault().ToString() == "iStoreAdmin")
                    {
                        var getchat = (from gu in dbContext.UserContacts
                                       join us in dbContext.UserLogins on gu.ContactUserID equals us.noHiphenuserID
                                       where gu.Status == 10 && !gu.IsChannel && gu.GroupName == model.RequestID.ToString() && us.RoleName == "salesteamuser"
                                       select new VMGroupUser { GroupName = gu.GroupName, UserID = gu.ContactUserID, Name = us.AspNetUserId }).FirstOrDefault();
                        model.UserID = getchat.UserID;
                    }
                    model.groupID = model.groupID.Replace("-", "");
                    //c.Messages = dbContext.Chat_MessageDetails.ToList().Where(a => a.MessageID > model.MessageID && a.RequestID == model.RequestID &&
                    c.Messages = dbContext.Chat_MessageDetails.ToList().Where(a => a.RequestID == model.RequestID &&
                        ((a.SenderID == model.groupID && a.ReceiverGroupID == model.UserID) || (a.SenderID == model.UserID && a.ReceiverGroupID == model.groupID))
                        && a.StatusTypeID == 10).Select(x => new
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
                            ReplyThumb = x.ReplyThumb
                        }).ToList().Select(x => new VMChatMessages
                        {
                            id = 1,
                            msgid = x.MessageID,
                            msg = x.Message,
                            senderName = x.SenderName,
                            SenderID = model.UserID,
                            isRead = x.IsRead,
                            guid = model.groupID,
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
                            ReplyThumb = x.ReplyThumb
                        }).ToList();
                    return c;
                }
            }
            catch
            {
                throw;
            }
        }

        [HttpGet]
        [Route("ReqirementOSCount")]
        public void ReqirementOSCount()
        {
            try
            {
                if (dependency == null)
                {
                    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        con.Open();

                        SqlCommand command = new SqlCommand();
                        command = new SqlCommand(@"SELECT ID FROM [dbo].[Chat_MessageDetails]", con);
                        //DataTable dt = new DataTable();
                        dependency = new SqlDependency(command);
                        dependency.OnChange += new
                        OnChangeEventHandler((sender, e) => dependency_OnChangesave(sender, e));

                        if (con.State == ConnectionState.Closed)
                            con.Open();
                        command.ExecuteReader();


                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                //Common.SaveErrorLog("ErrorLog: XiSignalR ReqirementOSCount method" + ex.ToString(), sDatabase);
                throw ex;
            }
        }

        private void dependency_OnChangesave(object sender, SqlNotificationEventArgs e)
        {
            if (dependency != null)
            {
                dependency.OnChange -= new OnChangeEventHandler((Sender, r) => dependency_OnChangesave(sender, e));
                dependency = null;
            }
            if (e.Info == SqlNotificationInfo.Insert)
            {
                //Call the EntireSignalR method because Count gain From the SP
                List<string> oSender = new List<string>();
                oSender.Add("salesteamuser");
                oSender.Add("procurementuser");
                oSender.Add("iStoreAdmin");
                foreach (var item in oSender)
                {
                    sendMessage_By_LastMessageID(item);
                }
                ReqirementOSCount();

                //IHubContext context = GlobalHost.ConnectionManager.GetHubContext<StoreChattinghub>();
                //context.Clients.All.EntireSolution(count);
            }
        }

        public IHttpActionResult sendMessage_By_LastMessageID(string sender)
        {
            using (var db = new chatContext())
            {
                var hubContext = GlobalHost.ConnectionManager.GetHubContext<StoreChattinghub>();

                var messages = dbContext.Chat_MessageDetails.Where(x => x.isProcessed == false && x.SenderName == sender).ToList();
                foreach (var reciever in messages)
                {
                    reciever.isProcessed = true;
                    dbContext.SaveChanges();

                    var user = db.Users.Find(reciever.ReceiverUserID.Replace(@"-", ""));
                    if (user == null)
                    {
                        //Clients.Caller.showErrorMessage("Could not find that user.");
                    }
                    else
                    {
                        db.Entry(user).Collection(u => u.Connections).Query().Where(c => c.Connected == true).Load();

                        if (user.Connections == null)
                        {
                            //pushnotification Logic
                            try
                            {
                                //ServiceUtil.SendNotification(user.UserName, vmModel.SenderName + ":" + vmModel.Text);
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                        else
                        {
                            foreach (var connection in user.Connections)
                            {
                                hubContext.Clients.Client(connection.ConnectionID)
                                    .addNewMessage(reciever.Message, reciever.SenderID,
                                    reciever.SenderName, reciever.ReceiverGroupID, reciever.RequestID,
                                    false,
                                    DateTime.Now.ToString("dd MMM yyyy hh:mm:ss tt"),
                                    reciever.ID,
                                    reciever.IsAttachement,
                                    reciever.Filepath,
                                    "",
                                    reciever.ThumbnailPath,
                                    reciever.AttachementType, 0, 0, reciever.ReplyFor, reciever.ReplyText, reciever.ReplyThumb, 0);
                                //logger.Info(vmModel.SenderName + " Sends SignalR event to  :" + user.UserName);
                            }
                        }
                    }
                }
            }
            return Ok();
        }

        // [AllowAnonymous]
        [HttpPost]
        [Route("UploadAttachement")]
        public IHttpActionResult UploadAttachement()
        {
            try
            {
                HttpPostedFile file = HttpContext.Current.Request.Files[0];
                var data = HttpContext.Current.Request.Form[0];
                VMCustomResponse model = JsonConvert.DeserializeObject<VMCustomResponse>(data);
                string str = string.Empty;
                string type = string.Empty;
                string folderpath = model.PropertyName.Substring(0, model.PropertyName.LastIndexOf("/"));
                string physicalPath = HttpContext.Current.Server.MapPath("~/" + folderpath);
                if (!System.IO.File.Exists(physicalPath))
                {
                    Directory.CreateDirectory(physicalPath);
                }
                string ExteName = Path.GetExtension(file.FileName); //file.FileName;
                string imgPath = HttpContext.Current.Server.MapPath("~/" + model.PropertyName);
                file.SaveAs(imgPath);
                string size = "", duration = "";
                //result = PointsRepository.UpdateBlackspotImage(HistoryID, BSHistoryID, imageName);
                string SenderID = model.senderID;
                string SenderName = model.senderName;
                var lstUser = chatController.GetGroupUsers(model.ID, model.groupID, model.IsExceed, SenderID, size, duration);
                using (var db = new chatContext())
                {
                    var hubContext = GlobalHost.ConnectionManager.GetHubContext<StoreChattinghub>();
                    foreach (var reciever in lstUser)
                    {
                        var user = db.Users.Find(reciever.Replace("-", ""));
                        if (user == null)
                        {
                            //Clients.Caller.showErrorMessage("Could not find that user.");
                        }
                        else
                        {
                            db.Entry(user).Collection(u => u.Connections).Query().Where(c => c.Connected == true).Load();

                            if (user.Connections == null)
                            {
                                //pushnotification Logic
                                try
                                {
                                    // ServiceUtil.SendNotification(user.UserName, SenderName + ": Attachement");
                                }
                                catch (Exception ex)
                                {
                                }
                            }
                            else
                            {
                                foreach (var connection in user.Connections)
                                {
                                    hubContext.Clients.Client(connection.ConnectionID)
                                        .addNewMessage(model.strUdf2, SenderID,
                                        SenderName, model.groupID, model.RequestID,
                                        model.IsExceed,
                                        DateTime.Now.ToString("dd MMM yyyy hh:mm:ss tt"),
                                        model.ID,
                                        true,
                                        model.PropertyName,
                                        model.strUdf2,
                                        model.strUdf1,
                                        model.ExceptionMessage, size, duration, model.longUdf3, model.ReplyText, model.ReplyThumb);
                                }
                            }
                        }
                    }
                }
                return Ok(new VMCustomResponse() { IsException = false });
            }
            catch (Exception ex)
            {
                return Ok(new VMCustomResponse() { ID = 0, IsException = true, ExceptionMessage = ex.Message });
            }
        }

        private Image ScaleImage(Bitmap image, int maxWidth, int maxHeight)
        {
            var ratioX = (double)maxWidth / image.Width;
            var ratioY = (double)maxHeight / image.Height;
            var ratio = Math.Min(ratioX, ratioY);
            var newWidth = (int)(image.Width * ratio);
            var newHeight = (int)(image.Height * ratio);
            var newImage = new Bitmap(newWidth, newHeight);
            using (var graphics = Graphics.FromImage(newImage))
                graphics.DrawImage(image, 0, 0, newWidth, newHeight);
            return newImage;
        }

        private ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        [HttpPost]
        public string DownloadTextFile(string sPath, string sFile)
        {
            string user = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string sPath1 = Path.Combine(user, "Downloads");
            string FilePath = sPath1 + "\\" + sFile;
            WebRequest req = WebRequest.Create(sPath);
            using (WebResponse resp = req.GetResponse())
            {
                using (Stream stream = resp.GetResponseStream())
                {

                    FileStream fs = new FileStream(FilePath, FileMode.Create, FileAccess.Write);
                    byte[] buffer = new byte[resp.ContentLength];
                    stream.Read(buffer, 0, (int)resp.ContentLength);
                    fs.Write(buffer, 0, (int)resp.ContentLength);
                    fs.Close();

                    byte[] byteInfo = System.IO.File.ReadAllBytes(FilePath);
                    //System.IO.File.Delete(FilePath);
                    string contentType = MimeMapping.GetMimeMapping(FilePath);
                    var fileContentResult = new System.Web.Mvc.FileContentResult(byteInfo.ToArray(), contentType);
                    //fileContentResult.FileDownloadName = sName;
                    fileContentResult.FileDownloadName = FilePath;

                }
            }
            return null;
        }

    }
}