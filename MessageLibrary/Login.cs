using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MessageLibrary
{
    public class Login : Message, IChatProtocol
    {
        public string UserName { get; set; }
        public string Version { get; set; }
        public string StatusMessage { get; set; }
        public bool? LoginSuccessful { get; set; }
        [JsonIgnore]
        public string Prefix => "cmd:";

        public string ToJSON()
        {
            return Prefix + JsonConvert.SerializeObject(this);
        }
    }
}
