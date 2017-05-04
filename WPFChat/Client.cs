﻿using MessageLibrary;
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
        Action<ChatMessage> addMessage;
        BlockingCollection<ChatMessage> messageCollection = new BlockingCollection<ChatMessage>();
        MainWindow mainWindow;

        public Client(Action<ChatMessage> messageAdder, MainWindow w)
        {
            addMessage = messageAdder;
            mainWindow = w;
        }
        public void Start(object info)
        {
            ServerInfo serverInfo = info as ServerInfo;
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
            string message = "";

            try
            {
                while (true)
                {
                    NetworkStream n = client.GetStream();
                    message = new BinaryReader(n).ReadString();
                    ChatMessage cM = new ChatMessage() { Body = message };
                    Console.WriteLine("Other: " + message);
                    mainWindow.Dispatcher.Invoke(new Action(() => addMessage(cM)));
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

