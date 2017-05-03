using MessageLibrary;
using System;
using System.IO;
using System.Net.Sockets;

namespace ChatServer
{
    internal class ClientHandler
    {
        public TcpClient TcpClient { get; set; }
        public string UserName { get; set; }
        private Server cServer;
        private MessageManager messManager;

        public void HandleChatMessage(ChatMessage cMessage)
        {
            cServer.AddMessageToQueue(cMessage);
            cServer.Broadcast();
        }

        public void HandleLogin(Login login)
        {

        }

        public ClientHandler(TcpClient c, Server server)
        {
            TcpClient = c;
            cServer = server;
            UserName = "";
            messManager = new MessageManager(HandleChatMessage, HandleLogin);
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
            catch (Exception)
            {
                throw;
            }
        }



    }
}