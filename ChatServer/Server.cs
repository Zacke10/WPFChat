using System;
using System.Collections.Generic;
using MessageLibrary;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.IO;
using System.Collections.Concurrent;
using System.Linq;

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
            }
            SendUsernames();
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

        public bool IsValidUsername(string username)
        {
            lock (clients)
            {
                return clients.Where(c => !String.IsNullOrEmpty(c.UserName) && c.UserName == username).Count() == 0;
            }
        }

        public void SendUsernames()
        {
            lock (clients)
            {
                UsernameList ul = new UsernameList()
                {
                    Usernames = clients.Select(c => c.UserName).ToList()
                };
                string message = ul.ToJSON();

                foreach (var client in clients)
                {

                    ExecuteSend(message, client);
                }
            }
        }

        public void SendMessage()
        {
            ChatMessage cm;
            while (chatQueue.TryDequeue(out cm))
            {
                string messageSerialized = cm.ToJSON();
                lock (clients)
                {
                    IEnumerable<ClientHandler> tmpClients;

                    if (cm.Recipients == null)
                    {
                        tmpClients = clients;
                    }
                    else
                    {
                        tmpClients = clients.Where(c => cm.Recipients.Exists(s => s == c.UserName) || cm.Sender == c.UserName);
                    }

                    foreach (ClientHandler tmpClient in tmpClients)
                    {
                        ExecuteSend(messageSerialized, tmpClient);
                    }
                }
            }
        }

        public void ExecuteSend(string messageSerialized, ClientHandler tmpClient)
        {
            NetworkStream n = tmpClient.TcpClient.GetStream();
            BinaryWriter w = new BinaryWriter(n);
            w.Write(messageSerialized);
            w.Flush();
            Console.WriteLine("Message has been sent to client");
        }
    }
}
