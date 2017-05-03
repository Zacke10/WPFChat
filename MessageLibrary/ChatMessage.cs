using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MessageLibrary
{
    public class ChatMessage : Message, IChatProtocol
    {
        public string Sender { get; set; }
        public List<string> Recipients { get; set; }
        public string Body { get; set; }
        [JsonIgnore]
        public string Prefix => "msg:";
        public string ToJSON()
        {
            return Prefix + JsonConvert.SerializeObject(this);
        }
    }
}
