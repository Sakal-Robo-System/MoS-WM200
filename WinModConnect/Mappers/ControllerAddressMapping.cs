using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinModConnect.Mappers
{

    class ControllerAddressMapping
    {
        public ControllerAddressMapping(string type, int startInputAddr, int endInputAddr, int starOutputAddr, int endOutputAddr)
        {
            this.controllerType = type;
            this.startInputAddr = startInputAddr;
            this.endInputAddr = endInputAddr;
            this.starOutputAddr = starOutputAddr;
            this.endOutputAddr = endOutputAddr;
        }
        public string controllerType { get; set; }
        public int startInputAddr { get; set; }
        public int endInputAddr { get; set; }
        public int starOutputAddr { get; set; }
        public int endOutputAddr { get; set; }
    }
}
