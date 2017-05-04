using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                    ChatMessage cm = Newtonsoft.Json.JsonConvert.DeserializeObject<ChatMessage>(message.Substring(4));
                    chatController.HandleMessage(cm);
                }
                else if (message.StartsWith("cmd:"))
                {
                    Login cm = Newtonsoft.Json.JsonConvert.DeserializeObject<Login>(message.Substring(4));
                    chatController.HandleLogin(cm);
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
