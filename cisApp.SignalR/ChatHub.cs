using cisApp.Function;
using cisApp.SignalR.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace cisApp.SignalR
{
    public class ChatHub : Hub
    {
        static List<UserAndConnectionId> _CurrentConnection = new List<UserAndConnectionId>();

        public override Task OnConnectedAsync()
        {
            string userId = Context.GetHttpContext().Request.Query["userId"];

            UserAndConnectionId newConnecition = new UserAndConnectionId()
            {
                UserId = Guid.Parse(userId),
                ConnectionId = Context.ConnectionId
            };

            _CurrentConnection.Add(newConnecition);

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {

            try
            {
                _CurrentConnection = _CurrentConnection.Where(o => o.ConnectionId != Context.ConnectionId).ToList();
            }
            catch (Exception ex)
            {

            }

            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessage(Guid recieverId, string message, List<Guid> imgs)
        {
            // Call the addNewMessageToPage method to update clients.
            var userAndCon = _CurrentConnection.Where(o => o.ConnectionId == Context.ConnectionId).FirstOrDefault();

            if (userAndCon != null)
            {
                var messages = GetChatMessage.Get.MockChatMessageModel(userAndCon.UserId, recieverId, message, imgs);

                if (message != null)
                {
                    foreach (var item in messages)
                    {
                        var sendTo = _CurrentConnection.Where(o => o.UserId == item.RecieverId).ToList();
                        if (sendTo.Count > 0)
                        {
                           foreach (var send in sendTo)
                            {
                                await Clients.Client(send.ConnectionId).SendAsync("SendMessage", item);
                            }
                        }
                    }
                }
            }
            //string messageString = "";
            //foreach (var item in _CurrentConnection)
            //{
            //    messageString = messageString + "\n" + "conntectionid = " + item.ConnectionId + ", userid=" + item.User;
            //}

            //await Clients.All.SendAsync("BoardcastMessage", name, message + " - connections = " + messageString);
            //await Clients.Client(name).SendAsync("BoardcastMessage", name, "specific to client" + name);
        }
    }
}
