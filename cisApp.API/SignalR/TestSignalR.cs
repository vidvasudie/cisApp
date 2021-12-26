using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace cisApp.API.SignalR
{
    public class TestSignalR : Hub
    {
        static List<UserAndConnectionId> _CurrentConnection = new List<UserAndConnectionId>();

        public override Task OnConnectedAsync()
        {
            string userId = Context.GetHttpContext().Request.Query["userId"];

            UserAndConnectionId newConnecition = new UserAndConnectionId()
            {
                User = userId,
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

        public async Task BoardcastMessage(string name, string message)
        {
            // Call the addNewMessageToPage method to update clients.

            string messageString = "";
            foreach(var item in _CurrentConnection)
            {
                messageString = messageString + "\n" + "conntectionid = " + item.ConnectionId + ", userid=" + item.User;
            }

            await Clients.All.SendAsync("BoardcastMessage", name, message + " - connections = " + messageString);
            await Clients.Client(name).SendAsync("BoardcastMessage", name, "specific to client" + name);
        }

        public class UserAndConnectionId
        {
            public string User { get; set; }
            public string ConnectionId { get; set; }
        }
    }
}
