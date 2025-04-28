using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.DAL.SignarHub
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(string senderType, string messageText, int studentId, int instructorId)
        {
            await Clients.All.SendAsync("ReceiveMessage", senderType, messageText, studentId, instructorId, DateTime.Now);
        }
    }
}
