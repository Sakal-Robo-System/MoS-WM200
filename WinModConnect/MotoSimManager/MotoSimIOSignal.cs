using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinModConnect.MotoSimManager
{
    public enum TYPE
    {
        INPUT,
        OUTPUT
    }
    public class MotoSimIOSignal<T>
    {
        public string Name { get; set; }
        public T Value { get; set; }
        public TYPE Type { get; set; }
        public int Name_int { get; set; }

    }
}
