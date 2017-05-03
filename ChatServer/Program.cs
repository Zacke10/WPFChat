using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Threading;
using System.Net.Sockets;
using MessageLibrary;

namespace ChatServer
{
    class Program
    {
        static void Main(string[] args)
        {
            //Server myServer = new Server();
            //Thread serverThread = new Thread(myServer.Run);
            //serverThread.Start();
            //serverThread.Join();

            ChatMessage cm = new ChatMessage();
            cm.Body = "hej";
            cm.Sender = "då";
            Console.WriteLine(cm.ToJSON());
        }
    }
}
