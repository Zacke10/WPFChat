using MessageLibrary;
using System;
using System.IO;
using System.Net.Sockets;

namespace ChatServer
{
    internal class ClientHandler : IChatController
    {
        public TcpClient TcpClient { get; set; }
        public string UserName { get; set; }
        private Server cServer;
        private MessageManager messManager;

        public void HandleMessage(ChatMessage cMessage)
        {
            if (!String.IsNullOrEmpty(UserName))
            {
                cServer.AddMessageToQueue(cMessage);
                cServer.SendMessage();
            }
        }

        public void HandleLogin(Login login)
        {
            if (cServer.IsValidUsername(login.UserName))
            {
                UserName = login.UserName;
                cServer.SendUsernames();
            }
            else
            {
                UserName = login.UserName + Guid.NewGuid();
                cServer.SendUsernames();
            }
        }



        public ClientHandler(TcpClient c, Server server)
        {
            TcpClient = c;
            cServer = server;
            UserName = "";
            messManager = new MessageManager(this);
        }

        internal void Run()
        {
            try
            {
                string message = "";
                while (true)
                {
                    NetworkStream n = TcpClient.GetStream();
                    BinaryReader bReader = new BinaryReader(n);
                    message = bReader.ReadString();
                    Console.WriteLine(message);
                    messManager.HandleMessage(message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                cServer.DisconnectClient(this);
            }
        }

        public void HandleUsernames(UsernameList usernameList)
        {
            // :)
        }
    }
}