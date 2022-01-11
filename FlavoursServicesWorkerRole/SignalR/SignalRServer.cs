using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Hosting;
using Microsoft.AspNet.SignalR;
using System.ComponentModel;

namespace SignalR
{
    public  class SignalRServer
    {
        private IDisposable _signalR;
        private BindingList<ClientItem> _clients = new BindingList<ClientItem>();
        private BindingList<string> _groups = new BindingList<string>();

        public SignalRServer()
        {


            

            //Register to static hub events
            SimpleHub.ClientConnected += SimpleHub_ClientConnected;
            SimpleHub.ClientDisconnected += SimpleHub_ClientDisconnected;
            SimpleHub.ClientNameChanged += SimpleHub_ClientNameChanged;
            SimpleHub.ClientJoinedToGroup += SimpleHub_ClientJoinedToGroup;
            SimpleHub.ClientLeftGroup += SimpleHub_ClientLeftGroup;
            SimpleHub.MessageReceived += SimpleHub_MessageReceived;
        }

        public void Start(string url)
        {
            _signalR = WebApp.Start<Startup>(url);
        }

        private void SimpleHub_ClientConnected(string clientId)
        {
            //Add client to our clients list
            _clients.Add(new ClientItem() { Id = clientId, Name = clientId });

        }

        private void SimpleHub_ClientDisconnected(string clientId)
        {
            //Remove client from the list

            var client = _clients.FirstOrDefault(x => x.Id == clientId);
            if (client != null)
                _clients.Remove(client);
            writeToLog($"Client disconnected:{clientId}");
        }

        private void SimpleHub_ClientNameChanged(string clientId, string newName)
        {
            //Update the client's name if it exists

            var client = _clients.FirstOrDefault(x => x.Id == clientId);
            if (client != null)
                client.Name = newName;

            writeToLog($"Client name changed. Id:{clientId}, Name:{newName}");
        }

        private void SimpleHub_ClientJoinedToGroup(string clientId, string groupName)
        {
            //Only add the groups name to our groups list

            var group = _groups.FirstOrDefault(x => x == groupName);
            if (group == null)
                _groups.Add(groupName);


            writeToLog($"Client joined to group. Id:{clientId}, Group:{groupName}");
        }

        private void SimpleHub_ClientLeftGroup(string clientId, string groupName)
        {
            writeToLog($"Client left group. Id:{clientId}, Group:{groupName}");
        }

        private void SimpleHub_MessageReceived(string senderClientId, string message)
        {
            //One of the clients sent a message, log it

            string clientName = _clients.FirstOrDefault(x => x.Id == senderClientId)?.Name;

            writeToLog($"{clientName}:{message}");

        }

   
    

        private void SendToClient(ClientItem clientItem,string message)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<SimpleHub>();

            //Microsoft.AspNet.SignalR.Hubs.ConnectionIdProxy connection=
            hubContext.Clients.Client(clientItem.Id).addMessage("SERVER", message);
            
        }

        private void writeToLog(string log)
        {
          
        }
    }
}
