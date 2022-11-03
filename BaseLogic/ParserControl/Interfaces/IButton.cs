using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserControl.Interfaces
{
    public interface IButton
    {
        void ParseButton(params string[] parseButtonString);
        void Press();

        bool IsPressPossible { get; }

        string Text { get; }
    }
}
