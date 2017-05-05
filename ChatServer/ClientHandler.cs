using MessageLibrary;
using System;
using System.Collections.Generic;
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
            if (!String.IsNullOrEmpty(UserName) && UserName == cMessage.Sender)
            {
                cServer.AddMessageToQueue(cMessage);
                cServer.SendMessage();
            }
        }

        public void HandleLogin(Login login)
        {
            Login loginInfo = new Login();

            if (cServer.IsValidUsername(login.UserName))
            {
                UserName = login.UserName;
                cServer.SendUsernames();
                login.LoginSuccessful = true;
                HandleMessage(new ChatMessage()
                {
                    Recipients = new List<string> { login.UserName },
                    Body = $"Welcome to the chat service {login.UserName}!",
                    Sender = "ChatServer!"    
                });
            }
            else
            {
                login.LoginSuccessful = false;
                login.StatusMessage = $"{login.UserName} is not a valid username";
            }
            cServer.ExecuteSend(login.ToJSON(), this);

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