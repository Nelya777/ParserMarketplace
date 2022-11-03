using ParserControl.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserControl.Modules.State
{
    public struct WebResourceWildberries : IWebResource
    {
        public string Url => "https://www.wildberries.ru/";
        public string Name => "Wildberries";
    }
}
