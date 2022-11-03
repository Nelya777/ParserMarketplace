using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserControl.Modules.ViewController
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class BracedViewPathAttribute : Attribute
    {
        public string Path { get; }

        public BracedViewPathAttribute(string paths)
        {
            Path = paths;
        }
    }
}
