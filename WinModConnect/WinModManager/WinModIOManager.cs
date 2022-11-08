using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinModConnect.Mappers;
using WMCommLib;
using WinModConnect.Logger;
using System.Runtime.InteropServices;

namespace WinModConnect.WinModManager
{
    class WinModIOManager
    {
        public string IPAddress { get; set; }
        public string Port { get; set; }
        public int Timeout { get; set; }
        public WinMODComm winModApp { get; set; }

        List<WinMODCOMElement> winCOMElements = null;

        List<string> WinCOMElementsName = new List<string>();

        public event WinMODCOMElement.delegateOnCOMSignalReceived OnCOMSignalReceivedFromCOMElement;

        //to notify the list of COM elements
        public event EventHandler<EventArgs> WinMODCOMElementsListUpdate;

        public event EventHandler<EventArgs> ConnectionStateChangedUpdate;        // When the connection state changes

        public WinModIOManager()
        {
            winModApp = new WinMODComm();

            winCOMElements = new List<WinMODCOMElement>();

            winModApp.ConnectionStateChanged += OnConnectionStateChanged;

            winModApp.ComElementListChanged += FillComElementList;
        }


        public void Connect(string hostAddr, int portNo, int timeout)
        {
            try
            {
                bool bConnect = !winModApp.IsConnected();
                if (bConnect)
                {
                    winModApp.ConnectToWinMOD(hostAddr, (ushort)portNo, timeout);
                }
                else
                {
                    //  CommDisconnect();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void DisconnectWinMOD()
        {
            try
            {
                bool bConnect = winModApp.IsConnected();
                if (bConnect)
                {
                    winModApp.DisconnectWinMOD();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void GetCOMElements()
        {
            if (winModApp.IsConnected())
                winModApp.GetElementList();
        }

        public List<string> GetCOMElementsName()
        {
            return WinCOMElementsName;
        }

        private void FillComElementList(object sender, EventArgs e)
        {
            WinCOMElementsName.Clear();

            foreach (var eName in winModApp.m_COMElements)
                WinCOMElementsName.Add(eName);

            WinMODCOMElementsListUpdate?.Invoke(this, e);
        }

        public void GenerateCOMElements()
        {
            try
            {
                var c = winModApp.m_COMElements.Count();

                if (winCOMElements != null)
                    winCOMElements.Clear();

                foreach (var com in winModApp.m_COMElements)
                {
                    IWMCommElement pCommElement = null;
                    // Create an element instance.....xyz.....creating a COM element
                    pCommElement = winModApp.GetCommItf().CreateElement();

                    //  Logs.WrtieLogs("WinModIOManager", "FillComElementList", pCommElement.GetElementName());

                    WinMODCOMElement comE = new WinMODCOMElement(com, pCommElement);

                    // Register this object as callback target for interface IWMCommElementNotify, see passed WMCommLib.eWMCommOpt flags in OpenElement(...)
                    // Parameters:                            #Object, #COM-Element, #Notify type (multi-thread, single, async)               
                    winModApp.GetCommAdvice().AdviseElementItf(comE, pCommElement, eWMCommNotify.eWMCommNotify_SingleThreadAsync);

                    winCOMElements.Add(comE);

                }
            }
            catch (Exception ex)
            {
                Logs.WrtieLogs("WinModIOManager", "GenerateCOMElements ", ex.Message + WinMODComm.GetHResultString(ex));
            }
        }


        public bool OpenCOMElement()
        {
            foreach (var pConn in winCOMElements)
            {
                if (pConn.ConnectCOMElement())
                {
                    int iResult = pConn.OnRunInit();
                }
                if (!pConn.IsReadyToRun)
                    pConn.Disconnect();
            }
            return true;
        }



        public void OnConnectionStateChanged(object sender, EventArgs e)
        {
            ConnectionStateChangedUpdate?.Invoke(this, e);
        }

        public void SendData(string comName, string signalName, float value, bool value1)
        {
            WinMODCOMElement comEle = winCOMElements.SingleOrDefault(x => x.m_sCOMElement == comName);
            WinMODSignals<IWMCommSignal> sig = comEle.m_outputSignals.SingleOrDefault(x => x.Name == signalName);
            try
            {

                if (sig.Type == eWMCommType.eWMCommType_Fix)
                {
                    lock (comEle.m_pCommElement)
                    {
                        comEle.m_pCommElement.WriteData();
                        IWMCommSignalFix b = (IWMCommSignalFix)sig.Value;
                        b.SetRange(-1000000.0, 1000000.0, true);

                        b.SetValue(value);
                    }
                }
                else if (sig.Type == eWMCommType.eWMCommType_Bin)
                {
                    lock (comEle.m_pCommElement)
                    {
                        comEle.m_pCommElement.WriteData();
                        IWMCommSignalBin b = (IWMCommSignalBin)sig.Value;
                        b.SetValue(value1);
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public void StartSignalExchange(int cycleRate)
        {
            try
            {
                List<ControllerCOMMapping> abc = GenericStaticClass.getMapping();
                foreach (var com in winCOMElements)
                {
                    var comE = abc.SingleOrDefault(y => y.COMElementName == com.m_sCOMElement);
                    if (comE != null)
                    {
                        Logs.WrtieLogs("WinModIOManager", "StartWinMODExchange()", "events added ");
                        com.OnCOMSignalReceived += ReceiveSignalVlaue;
                        com.ReadSignals(cycleRate);
                    }
                }
            }
            catch (Exception ex)
            {
                Logs.WrtieLogs("WinModIOManager::StartWinMODExchange()", ex.Message, WinMODComm.GetHResultString(ex));
                throw;
            }

        }

        public void StopSignalExchange()
        {
            try
            {
                List<ControllerCOMMapping> abc = GenericStaticClass.getMapping();
                foreach (var com in winCOMElements)
                {
                    var comE = abc.SingleOrDefault(y => y.COMElementName == com.m_sCOMElement);
                    if (comE != null)
                    {
                        Logs.WrtieLogs("WinModIOManager", "StartWinMODExchange()", "events removed ");
                        com.StopReadSignals();
                        com.OnCOMSignalReceived -= ReceiveSignalVlaue;
                    }
                    com.Disconnect();
                }
            }
            catch (Exception ex)
            {
                Logs.WrtieLogs("WinModIOManager::StopSignalExchange()", ex.Message, WinMODComm.GetHResultString(ex));
                throw;
            }


        }

        public void StopDataExchange()
        {
            foreach (var pConn in winCOMElements)
            {
                pConn.Disconnect();
            }
        }

        public void ReceiveSignalVlaue(string signalName, bool signalValue)
        {
            OnCOMSignalReceivedFromCOMElement?.Invoke(signalName, signalValue);
        }

    }
}
