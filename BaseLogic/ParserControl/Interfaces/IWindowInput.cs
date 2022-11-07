using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParserControl.Interfaces
{
    public interface IWindowInput
    {
        void ParseWindow(params string[] parseButtonString);
        void DataInput(string inputString);
        void PressEnter();
        bool IsInputPossible { get; }

        string Text { get; }
    }
}
