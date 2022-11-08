using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WMCommLib;
using System.Runtime.InteropServices;
using WinModConnect.Logger;

namespace WinModConnect.WinModManager
{
    class WinMODComm : IWMCommConnectNotify
    {
        IWMCommConnect m_pCommItf = null;
        IWMCommRecv m_pCommRecv = null;
        IWMCommSend m_pCommSend = null;
        IWMCommAdvise m_pCommAdviceItf = null;

        readonly byte[] c_keyClient = new byte[]         // Public key - WinMOD Comm SDK: WinMOD SDK V1.0:
       {
          /*  0xB4,0xCB,0x00,0x00,0x80,0x00,0x00,0x00,0xA2,0xF8,0x2A,0xFD,0xFB,0x50,0x3A,0x3E,
            0xAD,0xC6,0x1E,0xDE,0xFD,0x28,0x64,0xC2,0x16,0xDE,0x9A,0xD7,0xDC,0x10,0x24,0xC9,
            0xD2,0x0B,0x55,0xAD,0x53,0xE8,0xA7,0xDE,0x0C,0x95,0x9B,0xE8,0xDE,0x6C,0x38,0xAA,
            0x8F,0xD4,0xC0,0x9B,0xFA,0xF5,0xFD,0x16,0xD9,0x8E,0x64,0x13,0x1E,0x76,0x4A,0xD7,
            0xF7,0xC0,0xA5,0x2D,0x5D,0x5D,0x58,0x61,0x2D,0x5F,0x02,0x74,0xF8,0x91,0x3C,0x90,
            0x8B,0xD6,0x53,0x5C,0x13,0xCA,0xFC,0xDB,0xCE,0x9E,0x4A,0x1E,0x1D,0x43,0x23,0xEF,
            0x00,0xDA,0xA5,0xAF,0x41,0x86,0xF9,0x50,0x26,0xC7,0x47,0xFA,0x0D,0x48,0x96,0x94,
            0xF4,0x3B,0x23,0xAF,0x7A,0x5D,0xB6,0xF4,0x45,0xA7,0x27,0x71,0xFF,0x20,0x3D,0x38,
            0x91,0x65,0x5B,0xC5,0xC2,0x5B,0x8B,0xBD,0x5F,0x1C,0x3E,0x70,0xEB,0xDA,0xE9,0xEC */
            0x34, 0x47, 0x00, 0x00, 0xD4, 0x00, 0x00, 0x00,  0xE7, 0x1D, 0xC0, 0x19, 0x93, 0x35, 0xE2, 0xCA,
            0x8E, 0xD7, 0xE4, 0xFF, 0xEB, 0xF8, 0x16, 0xCB,  0x24, 0x52, 0x7E, 0x7B, 0xA5, 0x8B, 0x75, 0x4D,
            0xA3, 0x1F, 0xF3, 0xA1, 0xF4, 0xC1, 0x69, 0xDA,  0xAF, 0x63, 0xA5, 0xBC, 0x01, 0xDC, 0x8A, 0x74,
            0xF3, 0x34, 0x2B, 0xC2, 0x1B, 0x90, 0x75, 0x7C,  0xE5, 0xAF, 0x68, 0x05, 0x21, 0x38, 0x8F, 0x76,
            0x3B, 0xF9, 0x07, 0xBC, 0xF7, 0x4C, 0xAA, 0x59,  0xC1, 0xB4, 0x0F, 0x02, 0xFE, 0x48, 0x0E, 0xFE,
            0x4A, 0xE7, 0x34, 0xFC, 0xD8, 0xBB, 0xEC, 0xD0,  0xE7, 0x34, 0x14, 0xB2, 0x80, 0x63, 0xD6, 0x09,
            0xE4, 0x4E, 0x22, 0x06, 0x38, 0x7C, 0x58, 0xBE,  0x4D, 0x97, 0xB1, 0x92, 0x54, 0xBF, 0x5B, 0xD1,
            0xEC, 0x91, 0x53, 0x82, 0x9D, 0x62, 0x8F, 0x4B,  0x7F, 0x51, 0xB9, 0xEC, 0xB2, 0x3B, 0xBF, 0x66,
            0x76, 0xF9, 0x21, 0x6C, 0x0C, 0x7A, 0x3B, 0xEF,  0x3F, 0x89, 0x94, 0x94, 0x04, 0x5B, 0xFF, 0x54,
            0x44, 0x0A, 0x60, 0xEC, 0x2B, 0x86, 0x33, 0xDC,  0x97, 0x4A, 0x7E, 0xD6, 0x60, 0x2B, 0x23, 0xB0,
            0xE0, 0xF7, 0x3F, 0x43, 0x2C, 0xB4, 0xDB, 0x8E,  0x61, 0x48, 0x0C, 0x0F, 0x8C, 0x1E, 0xF6, 0x28,
            0xD0, 0xC5, 0xEF, 0x44, 0xE3, 0xC9, 0x3F, 0x05,  0x49, 0x66, 0x99, 0xD5, 0x25, 0x8A, 0xE3, 0xFA,
            0x28, 0xAE, 0xB2, 0xCA, 0x34, 0x80, 0x49, 0x83,  0x1C, 0x13, 0x5D, 0x24, 0x48, 0xB6, 0x62, 0x1E,
            0x30, 0xF9, 0xBF, 0x92, 0x8B, 0xAA, 0xBC, 0x5D,  0xCB, 0xDD, 0x9A, 0x45, 0xAD, 0x04, 0xA8, 0x16,
            0xC8, 0xEA, 0xC1, 0xB5, 0x0C, 0x0E, 0x17, 0x00

       };

        bool m_bCommConnected = false;                                      // Member for determining if Comm-state is connected

        const uint m_uNotifyStates = (uint)(eWMCommState.eWMCommStateConnect | eWMCommState.eWMCommStateRecv | eWMCommState.eWMCommStateError);

        public readonly List<string> m_COMElements = new List<string>();    // Member to store the last requested COM-Element list (see ReadComElementList())

        public event EventHandler<EventArgs> ComElementListChanged;         // Event to notify after read requested COM-Element list from current loaded WinMOD project

        public event EventHandler<EventArgs> ConnectionStateChanged;        // When the connection state changes

        public WinMODComm()
        {
            try
            {
                m_pCommItf = new WMCommClass();
                if (m_pCommItf != null)
                {
                    Logs.WrtieLogs("WinMODComm", "WinMODComm()", "inside constructor");
                    // Create send & receive buffers (For exchanging telegrams with WinMOD)
                    m_pCommSend = m_pCommItf.CreateSendBuffer();
                    m_pCommRecv = m_pCommItf.CreateRecvBuffer();

                    // Get advice interface from main WMCommClass
                    m_pCommAdviceItf = (IWMCommAdvise)m_pCommItf;

                    // Register this object as callback target (interface IWMCommConnectNotify), get notified when comm-states change
                    // Parameters:                   #Object, #notify states, #Notify type (multi-thread, single, async)               
                    m_pCommAdviceItf.AdviseConnectItf(this, m_uNotifyStates, eWMCommNotify.eWMCommNotify_SingleThreadAsync);

                }
            }
            catch (Exception ex)
            {
                Logs.WrtieLogs("WinMODComm", "Constructor()", ex.Message);
            }
        }

        public bool IsConnected()
        {
            return m_bCommConnected;
        }

        // Method that reacts to specific Comm-states of the WinMOD connection (see IWMCommConnectNotify)
        public void OnCommNotify(uint nCommStates, uint nDeltaStates)
        {
            Logs.WrtieLogs("CWMComm", "OnCommNotify()", "start OnCommNotify");
            if (m_pCommItf != null)
            {
                // If Comm-State = Connect
                if (((eWMCommState)nDeltaStates).HasFlag(eWMCommState.eWMCommStateConnect))
                {
                    if (((eWMCommState)nCommStates).HasFlag(eWMCommState.eWMCommStateConnect))
                    {
                        Logs.WrtieLogs("WinMODComm", "OnCommNotify()", "setting m_bCommConnected = true");
                        m_bCommConnected = true;
                    }
                    else
                    {
                        m_bCommConnected = false;
                        Logs.WrtieLogs("WinMODComm", "OnCommNotify()", "setting m_bCommConnected = false");
                    }

                    ConnectionStateChanged?.Invoke(this, EventArgs.Empty);
                }

                // If Comm-State = Receiving
                if (((eWMCommState)nCommStates).HasFlag(eWMCommState.eWMCommStateRecv))
                {
                    try
                    {
                        uint nSAP;                                                                                  // SAP of received telegram
                        uint nInvokeID;                                                                             // Invoke ID of received telegram

                        while ((nSAP = m_pCommRecv.RecvTelegram(out nInvokeID)) != 0)                 // Read Telegrams as long as the receive buffer is empty!
                        {
                            switch (nSAP)
                            {
                                case (uint)(eWMCommSAP.eWMCommSAP_ReadData | eWMCommSAP.eWMCommSAP_ResponseFlag):
                                    {
                                        Logs.WrtieLogs("WinMODComm", "OnCommNotify()", "--- SAP: Read Data ---");

                                        break;
                                    }

                                case (uint)(eWMCommSAP.eWMCommSAP_QueryLicense | eWMCommSAP.eWMCommSAP_ResponseFlag):
                                    {
                                        Logs.WrtieLogs("WinMODComm", "OnCommNotify()", "--- SAP: Queried License ---");

                                        break;
                                    }

                                case (uint)(eWMCommSAP.eWMCommSAP_GetElementList | eWMCommSAP.eWMCommSAP_ResponseFlag):
                                    {
                                        Logs.WrtieLogs("WinMODComm", "OnCommNotify()", "Get COM Element List");

                                        ReadComElementList();                                           // Reads requested COM-Element list from WinMOD 
                                        OnComElementListChanged(EventArgs.Empty);
                                        break;
                                    }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Logs.WrtieLogs("WinMODComm", "OnCommNotify()", "Exception while Comm-State Receiving: " + ex.ToString() + GetHResultString(ex));
                    }
                }

                // If Comm-State = Error
                if (((eWMCommState)nCommStates).HasFlag(eWMCommState.eWMCommStateError) && ((eWMCommState)nDeltaStates).HasFlag(eWMCommState.eWMCommStateError))
                {

                    m_bCommConnected = false;

                    // Use try-catch-block here so that wie can relay the COM-Exception from GetCommError
                    try
                    {
                        m_pCommItf.GetCommError();
                    }
                    catch (COMException ex)
                    {
                        Logs.WrtieLogs("WinMODComm", "OnCommNotify()", "Comm-State Error occured: " + ex.Message + GetHResultString(ex));
                    }
                }

            }
        }

        public void GetElementList()
        {
            Logs.WrtieLogs("WinMODComm", "CWMComm.cs", "GetElementList()");
            if (IsConnected() && m_pCommSend != null)
                m_pCommSend.SendTelegram(0, (uint)eWMCommSAP.eWMCommSAP_GetElementList); // Send Get_Element_List telegram
        }

        private void ReadComElementList()                           // Method for reading the requested COM-Element list from WinMOD 
        {
            m_COMElements.Clear();                                  // Clear old list

            uint uCount = m_pCommRecv.ReadUInt32();                 // #1: Read Element count                        
            Logs.WrtieLogs("WinMODComm ", "ReadComElementList()", "" + uCount);
            for (uint i = 0; i < uCount; i++)                       // #2: Read Element list
            {
                string strName = m_pCommRecv.ReadBSTR();            // Read string from telegram (COM-Element name)
                                                                    // Debug.WriteLine("COM-Element: \"" + strName + "\"");
                Logs.WrtieLogs("WinMODComm ", "COM-Element: ", strName);
                m_COMElements.Add(strName);
            }
        }

        private void OnComElementListChanged(EventArgs e)
        {
            ComElementListChanged?.Invoke(this, e);

        }

        public void ConnectToWinMOD(string hostno, ushort portno, int timeout)
        {
            try
            {
                // Trying to connect to WinMOD
                if (m_pCommItf != null)
                {
                    Logs.WrtieLogs("WinMODComm.cs ", "ConnectToWinMOD()", "");
                    m_pCommItf.Connect(ref c_keyClient[0], Convert.ToUInt32(c_keyClient.Length), hostno, portno, Convert.ToUInt32(timeout));
                }
            }
            catch (Exception ex)
            {
                string res = GetHResultString(ex);
                Logs.WrtieLogs("WinMODComm ", "ConnectToWinMOD ", ex.Message + GetHResultString(ex));
                throw; //+ GetHResultString(ex));
                       // Debug.WriteLine("GetCommError: " + GetCommError());
            }

        }

        public void DisconnectWinMOD()
        {
            try
            {
                if (m_pCommItf != null)
                {
                    try
                    {
                        Logs.WrtieLogs("WinMODComm ", "DisconnectWinMOD ", "Disconnect from winmod");
                        m_pCommItf.Disconnect();
                    }
                    catch (Exception ex)
                    {
                        // Ignore errors while disconnecting communication
                    }
                }

                if (m_pCommSend != null)
                    m_pCommSend.ResetTelegram();
            }
            catch (Exception ex)
            {
                Logs.WrtieLogs("WinMODComm", "DisconnectWinMOD", ex.Message + GetHResultString(ex));
                // throw;
            }
        }

        public IWMCommConnect GetCommItf()
        {
            return m_pCommItf;
        }

        public IWMCommAdvise GetCommAdvice()
        {
            return m_pCommAdviceItf;
        }

        public static string GetHResultString(Exception ex)
        {
            string strHResult = string.Empty;
            if (ex != null)
                strHResult = " (HResult: 0x" + ex.HResult.ToString("X8") + ")";
            return strHResult;
        }

    }
}
