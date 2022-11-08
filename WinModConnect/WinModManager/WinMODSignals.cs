using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WMCommLib;

namespace WinModConnect.WinModManager
{
    class WinMODSignals<T>
    {
        public string Name { get; set; }
        public T Value { get; set; }
        public eWMCommType Type { get; set; }
    }
}
