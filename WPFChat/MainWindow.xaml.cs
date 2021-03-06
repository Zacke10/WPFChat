﻿using MessageLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFChat
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IChatController
    {
        Client client;
        private const string BroadcastMessage = "Everyone";
        List<ChatMessage> chatList = new List<ChatMessage>();

        public MainWindow()
        {
            InitializeComponent();
            client = new Client(this);
            EnabledComponent(false);
            serverIP.Text = "127.0.0.1";
            serverPort.Text = "5000";
            userName.Text = "Charles";


        }

        private void EnabledComponent(bool isConnected)
        {
            chatBox.IsEnabled = isConnected;
            listBoxUsers.IsEnabled = chatBox.IsEnabled;
            sendMessageButton.IsEnabled = chatBox.IsEnabled;
            messageText.IsEnabled = chatBox.IsEnabled;

            serverIP.IsEnabled = !isConnected;
            serverPort.IsEnabled = serverIP.IsEnabled;
            userName.IsEnabled = serverIP.IsEnabled;
            connectButton.IsEnabled = serverIP.IsEnabled;
        }

        private void EnabledComponentBadUsername()
        {
            chatBox.IsEnabled = false;
            listBoxUsers.IsEnabled = false;
            sendMessageButton.IsEnabled = false;
            messageText.IsEnabled = false;

            serverIP.IsEnabled = false;
            serverPort.IsEnabled = false;
            userName.IsEnabled = true;
            connectButton.IsEnabled = true;
        }


        public void DisplayMessages()
        {
            chatBox.Clear();
            foreach (var chatMessage in chatList)
            {
                if (chatMessage.Recipients == null)
                    chatBox.AppendText($"{chatMessage.Sender}: {chatMessage.Body} \n");
                else
                    chatBox.AppendText($"{chatMessage.Sender} (PRIVATE): {chatMessage.Body} \n");

            }
        }

        private void sendMessageButton_Click(object sender, RoutedEventArgs e)
        {

            //TODO kolla namn listan och skapa recipents
            if (listBoxUsers.SelectedIndex != -1)
            {
                string sendTo = listBoxUsers.SelectedItem.ToString() == BroadcastMessage ? null : listBoxUsers.SelectedItem.ToString();
                List<string> recipients = sendTo != null ? new List<string>() { sendTo } : null;
                ChatMessage messageToSend = new ChatMessage()
                {
                    Sender = userName.Text,
                    Body = messageText.Text,
                    Recipients = recipients
                };
                client.AddToSendQueue(messageToSend);
                messageText.Clear();
            }
        }

        public ServerInfo GetServerInfo()
        {
            string sIP = serverIP.Text;
            string sPort = serverPort.Text;
            string uName = userName.Text;
            return new ServerInfo()
            {
                ServerIP = sIP,
                ServerPort = Int32.Parse(sPort),
                Username = uName
            };

        }

        private void connectButton_Click(object sender, RoutedEventArgs e)
        {
            if (connectButton.Content.ToString() == "Connect")
            {
                Thread clientThread = new Thread(client.Start);
                client.AddToSendQueue(new Login() { UserName = userName.Text });
                clientThread.IsBackground = true;
                clientThread.Start(GetServerInfo());

            }
            else
            {
                client?.AddToSendQueue(new Login() { UserName = userName.Text });
            }
        }

        public void HandleMessage(ChatMessage message)
        {
            Dispatcher.Invoke(() =>
            {
                chatList.Add(message);
                DisplayMessages();
            });
        }

        public void HandleLogin(Login login)
        {
            if (login.LoginSuccessful ?? false)
            {
                Dispatcher.Invoke(() =>
                {
                    EnabledComponent(true);
                    connectButton.Content = "Connect";
                });
            }
            else
            {
                Dispatcher.Invoke(() =>
                {
                    EnabledComponentBadUsername();
                    connectButton.Content = "Set username";
                    MessageBox.Show("Bad username");
                });
            }
        }

        public void HandleUsernames(UsernameList usernameList)
        {
            usernameList.Usernames.Sort();
            Dispatcher.Invoke(() =>
            {
                listBoxUsers.Items.Clear();
                listBoxUsers.Items.Add(BroadcastMessage);
                foreach (var user in usernameList.Usernames)
                {
                    listBoxUsers.Items.Add(user);
                }
                listBoxUsers.SelectedIndex = 0;
            });


        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
