using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MotoSimEG_VRC;

namespace WinModConnect.MotoSimManager
{
    public enum ROBOSIGNAL
    {
        S,
        L,
        U,
        R,
        B,
        T
    }
    public class MotoSimJointSignal<T>
    {
        public string Name { get; set; }
        public T Value { get; set; }
    }
}
