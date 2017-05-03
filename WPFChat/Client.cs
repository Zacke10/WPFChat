using MessageLibrary;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WPFChat
{
    class Client
    {
        private TcpClient client;
        BlockingCollection<ChatMessage> messageCollection = new BlockingCollection<ChatMessage>();
        public Client()
        {
            //HACK komma åt chattfönstret..
        }
        public void Start()
        {
            client = new TcpClient("127.0.0.1", 5000);

            Thread listenerThread = new Thread(Listen);
            listenerThread.Start();

            Thread senderThread = new Thread(Send);
            senderThread.Start();

            senderThread.Join();
            listenerThread.Join();
        }

        public void AddMessageToSend(ChatMessage messageToSend)
        {
            messageCollection.Add(messageToSend);
        }

        public void Listen()
        {
            string message = "";

            try
            {
                while (true)
                {
                    NetworkStream n = client.GetStream();
                    message = new BinaryReader(n).ReadString();
                    Console.WriteLine("Other: " + message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void DisplayInChatWindow(ChatMessage message)
        {
            
        }

        public void Send()
        {
            try
            {
                while (true)
                {
                    ChatMessage temp = messageCollection.Take();
                    NetworkStream n = client.GetStream();
                    BinaryWriter w = new BinaryWriter(n);
                    w.Write(temp.ToJSON());
                    w.Flush();
                }

                client.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}

