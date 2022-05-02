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
        }

        public async Task ReadMessage(Guid recieverId)
        {
            var userAndCon = _CurrentConnection.Where(o => o.ConnectionId == Context.ConnectionId).FirstOrDefault();

            if (userAndCon != null)
            {
                var chatGroup = GetChatGroup.Get.GetById(recieverId);

                if (chatGroup != null)
                {
                    var chatGroupUsers = GetChatGroup.Get.GetUserByGroupId(chatGroup.ChatGroupId.Value);

                    DateTime readDead = DateTime.Now;

                    foreach(var item in chatGroupUsers)
                    {
                        if (item.UserId != userAndCon.UserId)
                        {
                            ChatReadObj obj = new ChatReadObj()
                            {
                                UserId = item.UserId,
                                TargetChat = recieverId,
                                ReadDate = readDead
                            };

                            var sendTo = _CurrentConnection.Where(o => o.UserId == item.UserId).ToList();
                            if (sendTo.Count > 0)
                            {
                                foreach (var send in sendTo)
                                {
                                    await Clients.Client(send.ConnectionId).SendAsync("ReadMessage", obj);
                                }
                            }
                        }
                    }
                }
                else
                {
                    DateTime readDead = DateTime.Now;
                    ChatReadObj obj = new ChatReadObj()
                    {
                        UserId = userAndCon.UserId,
                        TargetChat = userAndCon.UserId,
                        ReadDate = readDead
                    };

                    var sendTo = _CurrentConnection.Where(o => o.UserId == recieverId).ToList();
                    if (sendTo.Count > 0)
                    {
                        foreach (var send in sendTo)
                        {
                            await Clients.Client(send.ConnectionId).SendAsync("ReadMessage", obj);
                        }
                    }
                }
            }
        }

        public class ChatReadObj
        {
            public Guid UserId { get; set; }
            public Guid TargetChat { get; set; }
            public DateTime ReadDate { get; set; }
        }
    }
}
