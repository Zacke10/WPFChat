﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageLibrary
{
    public interface IChatProtocol 
    {
        string ToJSON();
        string Prefix { get;}

    }
}
