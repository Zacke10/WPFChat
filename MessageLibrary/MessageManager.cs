using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MessageLibrary
{
    public class MessageManager

    {
        private IChatController chatController;

        public MessageManager(IChatController chatController)
        {
            this.chatController = chatController;
        }
        /// <summary>
        /// Hanterar meddelande och skickar till motsvarande action
        /// </summary>
        /// <param name="message"></param>
        public void HandleMessage(string message)
        {
            try
            {
                if (message.StartsWith("msg:"))
                {
                    ChatMessage cm = JsonConvert.DeserializeObject<ChatMessage>(message.Substring(4));
                    chatController.HandleMessage(cm);
                }
                else if (message.StartsWith("cmd:"))
                {
                    Login cm = JsonConvert.DeserializeObject<Login>(message.Substring(4));
                    chatController.HandleLogin(cm);
                }
                else if (message.StartsWith("unm:"))
                {
                    UsernameList ul = JsonConvert.DeserializeObject<UsernameList>(message.Substring(4));
                    chatController.HandleUsernames(ul);
                }
                else
                {
                    //do nothing, just chill
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
