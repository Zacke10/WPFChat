using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageLibrary
{
    public class MessageManager
        
    {
        private Action<ChatMessage> messageHandler;
        private Action<Login> statusHandler;

        public MessageManager(Action<ChatMessage> chatMessage, Action<Login> login)
        {
            messageHandler = chatMessage;
            statusHandler = login;
        }
        /// <summary>
        /// Hanterar meddelande och skickar till motsvarande action
        /// </summary>
        /// <param name="message"></param>
        public void HandleMessage(string message)
        {

        }

    }
}
