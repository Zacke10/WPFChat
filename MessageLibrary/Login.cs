using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageLibrary
{
    class Login
    {
        public string UserName { get; set; }
        public string Version { get; set; }
        public string StatusMessage { get; set; }
        public bool? LoginSuccessful { get; set; }
    }
}
