using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinModConnect.Mappers
{

    public class ErrorMessagesMapping
    {
        public ErrorMessagesMapping(string errorCode, string errorMessage)
        {
            this.errorCode = errorCode;
            this.errorMessage = errorMessage;
        }
        public string errorCode { get; set; }
        public string errorMessage { get; set; }
    }
}
