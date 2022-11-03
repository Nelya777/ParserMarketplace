using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserControl.Modules.ViewController
{
    public abstract class UiParser : IDisposable
    {
        public abstract void Parse();
        public abstract bool IsVisible();
        public abstract void Dispose();
    }
}
