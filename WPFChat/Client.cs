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
using System.Windows;
using System.Windows.Threading;

namespace WPFChat
{
    class Client
    {
        private TcpClient client;
        BlockingCollection<IChatProtocol> messageCollection = new BlockingCollection<IChatProtocol>();
        MainWindow mainWindow;

        private MessageManager messageManager;

        public Client(MainWindow w)
        {
            mainWindow = w;
            messageManager = new MessageManager(w);
        }

        public void Start(object info)
        {
            ServerInfo serverInfo = info as ServerInfo;
            try
            {
                client = new TcpClient(serverInfo.ServerIP, serverInfo.ServerPort);

                Thread listenerThread = new Thread(Listen);
                listenerThread.IsBackground = true;
                listenerThread.Start();

                Thread senderThread = new Thread(Send);
                senderThread.IsBackground = true;
                senderThread.Start();

                senderThread.Join();
                listenerThread.Join();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

 
        public void AddToSendQueue(IChatProtocol data)
        {
            messageCollection.Add(data);
        }

        public void Listen()
        {
            try
            {
                string message = "";
                while (true)
                {
                    NetworkStream n = client.GetStream();
                    message = new BinaryReader(n).ReadString();
                    messageManager.HandleMessage(message);
                    Console.WriteLine("Other: " + message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void Send()
        {
            try
            {
                NetworkStream n = client.GetStream();
                BinaryWriter w = new BinaryWriter(n);

                while (true)
                {
                    IChatProtocol temp = messageCollection.Take();
                    n = client.GetStream();
                    w = new BinaryWriter(n);
                    w.Write(temp.ToJSON());
                    w.Flush();
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                client.Close();
            }
        }
    }
}

