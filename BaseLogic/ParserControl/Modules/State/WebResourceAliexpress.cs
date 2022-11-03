using ParserControl.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserControl.Modules.State
{
    public struct WebResourceAliexpress : IWebResource
    {
        public string Url => "https://aliexpress.ru/";
        public string Name => "Aliexpress";
    }
}
