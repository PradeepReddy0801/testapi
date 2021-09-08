using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;

using System.Data.Entity;
using Microsoft.AspNet.SignalR;
using iHubAdminAPI.Models;
using static iHubAdminAPI.Models.iHubDBContext;

namespace iHubAdminAPI.ChatHub
{
    //[Authorize]
    public class StoreChattinghub : Hub
    {
            
        public override Task OnConnected()
        {
            try
            {
                using (var db = new chatContext())
                {
                    string UserID = Context.QueryString["UserID"];
                    var user = db.Users.Include("Connections")
                        .SingleOrDefault(u => u.UserName == UserID);

                    if (user == null)
                    {
                        user = new User
                        {
                            UserName = UserID,
                            Connections = new List<Connection>()
                        };
                        db.Users.Add(user);
                    }
                    else
                    {
                        user.Connections= new List<Connection>();
                    }

                    user.Connections.Add(new Connection
                    {
                        ConnectionID = Context.ConnectionId,
                        UserAgent = Context.Request.Headers["User-Agent"],
                        Connected = true,
                        //CreatedOn = DateTime.Now
                    });
                    db.SaveChanges();
                }
                return base.OnConnected();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public override Task OnDisconnected(bool stopCalled)
        {
            using (var db = new chatContext())
            {
                var connection = db.Connections.Find(Context.ConnectionId);
                if (connection != null)
                    db.Connections.Remove(connection);
                db.SaveChanges();
            }
            return base.OnDisconnected(stopCalled);
        }


        public async Task JoinRoom(string roomName)
        {
            await Groups.Add(Context.ConnectionId, roomName);
        }
    }
}