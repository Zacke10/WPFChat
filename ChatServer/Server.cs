using System;
using System.Collections.Generic;
using MessageLibrary;
using System.Net.Sockets;

namespace ChatServer
{
    internal class Server
    {
        List<ClientHandler> clients = new List<ClientHandler>();
        Queue<ChatMessage> chatQueue = new Queue<ChatMessage>();

        internal void Run()
        {
            //try
            //{
            //    string message = "";
            //    while (true)
            //    {
            //        NetworkStream n = TcpClient.GetStream();
            //        message = new BinaryReader(n).ReadString();
            //        if (UserName == "")
            //        {
            //            UserName = message;
            //        }

            //        if (message.Contains('/'))
            //        {
            //            string[] tmpMessage = message.Split('/');
            //            myServer.Privatecast(this, tmpMessage[0], "From: " + tmpMessage[0] + " Message: " + tmpMessage[1]);
            //        }
            //        else
            //        {
            //            myServer.Broadcast(this, UserName + ": " + message);
            //        }
            //        Console.WriteLine(message);
            //    }
            //}
            //catch (Exception)
            //{

            //    throw;
            //}
        }
    }
}