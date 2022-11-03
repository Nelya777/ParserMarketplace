using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserControl.Interfaces
{
    public interface IModule : IDisposable
    {
        IModule InitModule();
    }
}
