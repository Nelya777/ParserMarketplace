using ParserControl.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserControl.Modules.ViewController
{
    public class StringViewPathComparer : IEqualityComparer<IStringViewPath>
    {
        public bool Equals(IStringViewPath x, IStringViewPath y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null)) return false;
            if (ReferenceEquals(y, null)) return false;
            if (x.GetType() != y.GetType()) return false;
            return x.Path == y.Path;
        }

        public int GetHashCode(IStringViewPath obj)
        {
            return (obj.Path != null ? obj.Path.GetHashCode() : 0);
        }
    }
}
