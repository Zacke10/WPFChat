using System;
using System.Collections.Generic;
using MessageLibrary;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.IO;
using System.Collections.Concurrent;

namespace ChatServer
{
    internal class Server
    {
        List<ClientHandler> clients = new List<ClientHandler>();
        ConcurrentQueue<ChatMessage> chatQueue = new ConcurrentQueue<ChatMessage>();

        public void AddMessageToQueue(ChatMessage cMessage)
        {
            chatQueue.Enqueue(cMessage);
        }

        public void DisconnectClient(ClientHandler client)
        {


            lock (clients)
            {
                clients.Remove(client);
                Console.WriteLine("User: " + client.UserName + " has left the building...");
                //Broadcast(client, "User: " + client.UserName + " has left the building...");
            }
        }
        public void Run()
        {
            TcpListener listener = new TcpListener(IPAddress.Any, 5000);
            Console.WriteLine("Server up and running, waiting for messages...");

            try
            {
                listener.Start();

                while (true)
                {
                    TcpClient c = listener.AcceptTcpClient();
                    ClientHandler newClient = new ClientHandler(c, this);
                    clients.Add(newClient);

                    Thread clientThread = new Thread(newClient.Run);
                    clientThread.Start();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (listener != null)
                    listener.Stop();
            }
        }
        public void Broadcast()
        {
            ChatMessage cm;
            while (chatQueue.TryDequeue(out cm))
            {
                string messageSerialized = cm.ToJSON();
                foreach (ClientHandler tmpClient in clients)
                {
                    if (tmpClient.UserName != cm.Sender)
                    {
                        NetworkStream n = tmpClient.TcpClient.GetStream();
                        BinaryWriter w = new BinaryWriter(n);
                        w.Write(messageSerialized);
                        w.Flush();
                    }
                    else if (clients.Count == 1)
                    {
                        NetworkStream n = tmpClient.TcpClient.GetStream();
                        BinaryWriter w = new BinaryWriter(n);
                        w.Write("Sorry, you are alone...");
                        w.Flush();
                    }
                }
            }
        }
    }
}
