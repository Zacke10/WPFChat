using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MessageLibrary
{
    class ChatMessage : IChatProtocol
    {
        public string Sender { get; set; }
        public List<string> Recipients { get; set; }
        public string Body { get; set; }

        public string ToJSON()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
