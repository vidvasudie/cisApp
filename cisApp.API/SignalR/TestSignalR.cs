using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace cisApp.API.SignalR
{
    public class TestSignalR : Hub
    {
        public async Task BoardcastMessage(string name, string message)
        {
            // Call the addNewMessageToPage method to update clients.
            await Clients.All.SendAsync("BoardcastMessage", name, message);
        }
    }
}
