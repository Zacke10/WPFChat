using MessageLibrary;
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
    public partial class MainWindow : Window
    {
        Client client;
        List<ChatMessage> chatList = new List<ChatMessage>();
        public MainWindow()
        {
            InitializeComponent();
            client = new Client( c=> { chatList.Add(c); DisplayMessages(); } , this);
        }

        public void DisplayMessages()
        {
            chatBox.Clear();
            foreach (var x in chatList)
            {
                chatBox.AppendText($"{x.Sender}: {x.Body} \n");

            }
        }

        private void sendMessageButton_Click(object sender, RoutedEventArgs e)
        {
            ChatMessage messageToSend = new ChatMessage() { Body = messageText.Text };
            client.AddMessageToSend(messageToSend);
            messageText.Clear();

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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Thread clientThread = new Thread(client.Start);
            clientThread.Start(GetServerInfo());
            
        }
    }
}
