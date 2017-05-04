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
        BlockingCollection<ChatMessage> messageCollection = new BlockingCollection<ChatMessage>();
        MainWindow mainWindow;
        public string UserName { get; set; }

        private MessageManager messageManager;

        public Client(MainWindow w)
        {
            mainWindow = w;
            messageManager = new MessageManager(w);
        }

        public void Start(object info)
        {
            ServerInfo serverInfo = info as ServerInfo;
            UserName = serverInfo.Username;
            try
            {
                client = new TcpClient(serverInfo.ServerIP, serverInfo.ServerPort);

                Thread listenerThread = new Thread(Listen);
                listenerThread.Start();

                Thread senderThread = new Thread(Send);
                senderThread.Start();

                senderThread.Join();
                listenerThread.Join();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void AddMessageToSend(ChatMessage messageToSend)
        {
            messageCollection.Add(messageToSend);
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

        public void DisplayInChatWindow(ChatMessage message)
        {

        }

        public void Send()
        {
            try
            {
                NetworkStream n = client.GetStream();
                BinaryWriter w = new BinaryWriter(n);
                Login login = new Login() {UserName = UserName};
                w.Write(login.ToJSON());
                w.Flush();

                while (true)
                {
                    ChatMessage temp = messageCollection.Take();
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

