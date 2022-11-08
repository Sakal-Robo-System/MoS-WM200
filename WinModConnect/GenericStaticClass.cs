using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using WinModConnect.Mappers;

namespace WinModConnect
{
    public static class GenericStaticClass
    {
        static List<ControllerCOMMapping> controllerMap = null;

        static List<ErrorMessagesMapping> errorMap = null;
        public static List<ControllerCOMMapping> getMapping()
        {
            return controllerMap;
        }

        public static void resetMapping()
        {
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\JSON" + $"\\COMControllerMapping";
            if (File.Exists(path))
            {
                string addr = System.IO.File.ReadAllText(path);
                controllerMap = JsonConvert.DeserializeObject<List<ControllerCOMMapping>>(addr);
            }
        }

        public static List<ErrorMessagesMapping> getErrorMapping()
        {
            return errorMap;
        }

        public static void readErrorMapping()
        {
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\JSON" + $"\\ErrorListJSON";
            if (File.Exists(path))
            {
                string addr = System.IO.File.ReadAllText(path);
                errorMap = JsonConvert.DeserializeObject<List<ErrorMessagesMapping>>(addr);
            }
        }
    }
}
