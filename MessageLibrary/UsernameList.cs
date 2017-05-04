using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MessageLibrary
{
    public class UsernameList : IChatProtocol
    {
        public List<string> Usernames { get; set; }
        [JsonIgnore]
        public string Prefix => "unm:";

        public string ToJSON()
        {
            return Prefix + JsonConvert.SerializeObject(this);
        }
    }
}
