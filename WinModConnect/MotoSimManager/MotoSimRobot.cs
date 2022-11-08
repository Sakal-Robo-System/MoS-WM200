using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinModConnect.MotoSimManager
{
    public class MotoSimRobot
    {
        public List<MotoSimJointSignal<float>> JointSignals { get; set; } = new List<MotoSimJointSignal<float>>();
        public string Name { get; set; }
    }
}
