using System.Net.Sockets;

namespace ChatServer
{
    internal class ClientHandler
    {
        public TcpClient TcpClient { get; set; }
        public string UserName { get; set; }
        private Server cServer;

        public ClientHandler(TcpClient c, Server server)
        {
            TcpClient = c;
            cServer = server;
            UserName = "";
        }
    }
}