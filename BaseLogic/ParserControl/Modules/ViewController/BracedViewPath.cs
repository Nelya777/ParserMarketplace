using ParserControl.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserControl.Modules.ViewController
{
    public class BracedViewPath : IStringViewPath
    {
        public string Path { get; }

        public BracedViewPath(string path)
        {
            Path = path;
        }
    }
}
