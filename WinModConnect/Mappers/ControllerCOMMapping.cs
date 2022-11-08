using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinModConnect.Mappers
{
    public class ControllerCOMMapping
    {
        public ControllerCOMMapping(string controllerName, string comElementName)
        {
            this.ControllerName = controllerName;
            this.COMElementName = comElementName;
        }
        public string ControllerName { get; set; }
        public string COMElementName { get; set; }
    }
}
