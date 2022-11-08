using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WMCommLib;
using WinModConnect.Logger;
using System.Runtime.InteropServices;
using System.Threading;

namespace WinModConnect.WinModManager
{
    class WinMODCOMElement : IWMCommElementNotify
    {
        public readonly IWMCommElement m_pCommElement = null;                                  // Interface to an object thats represents a WinMOD COM-Element

        public readonly string m_sCOMElement = null;

        bool m_bConnected = false;                                                      // Member for determining if COM-Element opened
        bool m_bCommError = false;
        bool m_bIsReadyToRun = false;                                                   // Flag which is used to check if object ready to run data exchange
        bool m_bCommIOWarning = false;

        //Input signals(our input WinMOD output)
        public List<WinMODSignals<IWMCommSignal>> m_inputSignals = new List<WinMODSignals<IWMCommSignal>>();

        //Output signals(our output WinMOD input)
        public List<WinMODSignals<IWMCommSignal>> m_outputSignals = new List<WinMODSignals<IWMCommSignal>>();

        const uint m_uConOptionsDef = (uint)(eWMCommOpt.eWMCommOpt_NotifySignals | eWMCommOpt.eWMCommOpt_NotifyRun | eWMCommOpt.eWMCommOpt_NotifyClose);

        public delegate void delegateOnCOMSignalReceived(string signalName, bool signalValue);
        public event delegateOnCOMSignalReceived OnCOMSignalReceived;

        Thread trd;
        public WinMODCOMElement(string sCOMElement, IWMCommElement pCommElement)
        {
            Logs.WrtieLogs("CWMCommElement()", "Constructor", sCOMElement);
            m_sCOMElement = sCOMElement;
            m_pCommElement = pCommElement;
        }
        public bool IsReadyToRun
        {
            get { return m_bIsReadyToRun; }
        }

        public bool ConnectCOMElement()                                                                 // Method for open COM-Element 
        {
            try
            {
                if (m_pCommElement != null)
                {
                    Logs.WrtieLogs("", "CWMCommElement()", "ConnectCOMElement");
                    // Open COM-Element and set default notify flags - See all possible options in WMCommLib.eWMCommOpt
                    m_pCommElement.OpenElement(m_sCOMElement, m_uConOptionsDef);
                    m_bConnected = true;
                    m_bCommError = false;
                    return true;
                }
            }
            catch (COMException ex)
            {
                m_bCommError = true;

                DisconnectCOMElement();
            }
            catch (Exception ex)
            {
                m_bCommError = true;

                DisconnectCOMElement();
            }
            return false;
        }

        public int OnRunInit()
        {
            try
            {
                Logs.WrtieLogs("CWMCommElement", "OnRunInit()", " ");
                m_bCommIOWarning = false;
                m_bIsReadyToRun = false;

                // Initialize connection between COM-Element signals and signals of the client process!!!
                int nOk = PrepareRunData();
                if (nOk >= 0)                                                                               // Check if signals connections initialized without errors
                {
                    m_bIsReadyToRun = true;
                    uint uOptions = m_uConOptionsDef;
                    uOptions |= (uint)eWMCommOpt.eWMCommOpt_AdviseData;
                    m_pCommElement.SetConOptions(uOptions);

                    return nOk;
                }
            }
            catch (Exception ex)
            {
                Logs.WrtieLogs("WinMODCOMElement::OnRunInit ", ex.ToString(), WinMODComm.GetHResultString(ex));
            }
            return -1;
        }

        int PrepareRunData()
        {
            Logs.WrtieLogs("", "PrepareRunData", "Start point");
            if (m_pCommElement != null)
            {
                // Read in all signals of the connected COM-Element
                m_pCommElement.GetElementSignals();

                List<string> mappedSignals = new List<string>(); // List for save mapped signals

                // Iterate over all outputs and create connection objects if exists a corresponding process signal (same name and same signal type)
                uint nCommOutputCount = m_pCommElement.GetOutputCount();

                for (uint i = 0; i < nCommOutputCount; i++)
                {
                    IWMCommSignal pOutputSignal = m_pCommElement.GetOutput(i);
                    string sSignalName = pOutputSignal.GetName();
                    // Determine signal type and get the corresponding signal interface
                    switch (pOutputSignal.GetType())
                    {
                        case eWMCommType.eWMCommType_Fix:
                            {
                                if (!mappedSignals.Contains(sSignalName))
                                {
                                    WinMODSignals<IWMCommSignal> a = new WinMODSignals<IWMCommSignal>();
                                    a.Name = sSignalName;
                                    a.Value = pOutputSignal;
                                    a.Type = eWMCommType.eWMCommType_Fix;

                                    m_outputSignals.Add(a);
                                    mappedSignals.Add(sSignalName);
                                }
                                break;
                            }
                        case eWMCommType.eWMCommType_Bin:
                            {

                                if (!mappedSignals.Contains(sSignalName))
                                {
                                    IWMCommSignalBin pBinaryOutputSignal = (IWMCommSignalBin)pOutputSignal; // get the typed signal interface
                                    WinMODSignals<IWMCommSignal> a = new WinMODSignals<IWMCommSignal>();
                                    a.Name = sSignalName;
                                    a.Value = pOutputSignal;
                                    a.Type = eWMCommType.eWMCommType_Bin;

                                    m_outputSignals.Add(a);
                                    mappedSignals.Add(sSignalName);
                                }
                                break;

                            }
                    }
                }

                mappedSignals.Clear();

                // Iterate over all inputs and create connection objects if exists a corresponding process signal (same name and same signal type)
                uint nCommInputCount = m_pCommElement.GetInputCount();
                Logs.WrtieLogs("", "PrepareRunData", "inputCount " + nCommInputCount);
                for (uint i = 0; i < nCommInputCount; i++)
                {
                    IWMCommSignal pInputSignal = m_pCommElement.GetInput(i);
                    string sSignalName = pInputSignal.GetName();
                    // Determine signal type and get the corresponding signal interface
                    switch (pInputSignal.GetType())
                    {
                        case eWMCommType.eWMCommType_Fix:
                            {
                                if (!mappedSignals.Contains(sSignalName))
                                {
                                    IWMCommSignalFix pAnalogOutputSignal = (IWMCommSignalFix)pInputSignal;
                                    WinMODSignals<IWMCommSignal> a = new WinMODSignals<IWMCommSignal>();
                                    a.Name = sSignalName;
                                    a.Value = pInputSignal;
                                    a.Type = eWMCommType.eWMCommType_Fix;

                                    m_inputSignals.Add(a);
                                    mappedSignals.Add(sSignalName);
                                }
                                break;
                            }
                        case eWMCommType.eWMCommType_Bin:
                            {
                                if (!mappedSignals.Contains(sSignalName))
                                {
                                    IWMCommSignalBin pBinaryInputSignal = (IWMCommSignalBin)pInputSignal;
                                    bool z = pBinaryInputSignal.GetValue();

                                    WinMODSignals<IWMCommSignal> a = new WinMODSignals<IWMCommSignal>();
                                    a.Name = sSignalName;
                                    a.Value = pInputSignal;
                                    a.Type = eWMCommType.eWMCommType_Bin;

                                    m_inputSignals.Add(a);
                                    mappedSignals.Add(sSignalName);
                                }
                                break;
                            }
                    }
                }

                return 0;
            }
            return -1;
        }

        public void OnCommNotify(uint nElementEvent)        // Method that reacts to specific COM-Element-states within WinMOD 
        {
            Logs.WrtieLogs("", "CWMCommElement()", "OnCommNotify");
            if (m_pCommElement != null)
            {
                Logs.WrtieLogs("", "CWMCommElement()", "OnCommNotify...1");
                /* Not implemented notify state!
                if (((eWMCommOpt)nElementEvent).HasFlag(eWMCommOpt.eWMCommOpt_NotifyName))
                {
                    Debug.WriteLine("------------ Notify Element Name ------------");
                }
                */

                if (((eWMCommOpt)nElementEvent).HasFlag(eWMCommOpt.eWMCommOpt_NotifySignals))
                {
                    //Debug.WriteLine("------------ Notify Element Signals ------------");
                    Logs.WrtieLogs("The signal configuration of the COM-Element has changed! Please export the signal list out of the client and reimport the list into the needed COM-Element within WinMOD!", "", "");
                    Disconnect(true);
                }

                if (((eWMCommOpt)nElementEvent).HasFlag(eWMCommOpt.eWMCommOpt_NotifyRun))
                {
                    if (((eWMCommOpt)nElementEvent).HasFlag(eWMCommOpt.eWMCommOpt_StateRun))
                        Logs.WrtieLogs("COM-Element ", m_sCOMElement, " running.");
                    else
                        Logs.WrtieLogs("COM-Element ", m_sCOMElement, " stopped.");
                }

                if (((eWMCommOpt)nElementEvent).HasFlag(eWMCommOpt.eWMCommOpt_AdviseData))
                {
                    //  Logs.WrtieLogs("------------ Advise Element: Signal data has changed! ------------","","");

                }

                if (((eWMCommOpt)nElementEvent).HasFlag(eWMCommOpt.eWMCommOpt_NotifyClose))
                {
                    //Debug.WriteLine("------------ Notify Element: COM-Element closed! ------------");

                    m_bCommError = true;
                    //  Debug.WriteLine("Connection to the COM-Element " + m_sCOMElement + " within WinMOD has been closed unexpectedly! Please verify the state of the COM-Element in WinMOD!");
                    //OnCommElementClosed();
                    Disconnect(true);
                }
            }
        }

        void DisconnectCOMElement()                                                                      // Method for close COM-Element 
        {
            try
            {
                if (m_pCommElement != null && m_bConnected)
                {
                    m_pCommElement.CloseElement();
                }
            }
            catch (Exception ex)
            {
                Logs.WrtieLogs("WinMODCOMElement", "Disconnect COM-Element error: ", ex.Message + WinMODComm.GetHResultString(ex));
            }
            finally
            {
                m_bConnected = false;
            }
        }

        public void OnCommElementClosed()
        {
            //m_pCommElement = null;
            m_bConnected = false;
        }

        public void Reset(bool bCommElementClosed = false)
        {
            try
            {
                m_bIsReadyToRun = false;

                if (bCommElementClosed)
                    OnCommElementClosed();

                SetCommNotifyMode(false);
                DisconnectCOMElement();

                // Remove all connection objects here
                m_inputSignals.Clear();
                m_outputSignals.Clear();
            }
            catch (Exception ex)
            {
                Logs.WrtieLogs("WinMODCOMElement::Reset error: ", ex.ToString(), WinMODComm.GetHResultString(ex));
            }
        }

        public bool SetCommNotifyMode(bool bAdviseData)                                                  // Method for activate/deactivate advice data notification 
        {
            if (m_pCommElement != null && m_bConnected)
            {
                Logs.WrtieLogs("", "CWMCommElement()", "SetCommNotifyMode");
                uint uOptions = m_uConOptionsDef;
                if (bAdviseData)
                    uOptions |= (uint)eWMCommOpt.eWMCommOpt_AdviseData;

                try
                {
                    m_pCommElement.SetConOptions(uOptions);
                    return true;
                }
                catch (Exception ex)
                {
                    Logs.WrtieLogs("WinMODCOMElement::SetCommNotifyMode ", ex.ToString(), WinMODComm.GetHResultString(ex));
                }
            }
            return false;
        }

        public void ReadSignals(int cycleRate)
        {
            try
            {
                Logs.WrtieLogs("WinMODCOMElements", "ReadSignals", "start thread");
                trd = new Thread(this.StartThreadReadSignals);
                trd.Start(cycleRate);
            }
            catch (Exception ex)
            {

            }
        }

        public void StopReadSignals()
        {
            trd.Abort();
        }

        private void StartThreadReadSignals(object arg)
        {
            try
            {
                while (true)
                {
                    foreach (WinMODSignals<IWMCommSignal> sig in m_inputSignals)
                    {
                        m_pCommElement.ReadData();
                        eWMCommType a = sig.Type;
                        if (a == eWMCommType.eWMCommType_Bin)
                        {
                            IWMCommSignalBin b = (IWMCommSignalBin)sig.Value;
                            bool finalV = b.GetValue();
                            OnCOMSignalReceived(b.GetName(), finalV);
                        }
                        else if (a == eWMCommType.eWMCommType_Fix)
                        {
                            IWMCommSignalFix b = (IWMCommSignalFix)sig.Value;
                            double finalV = b.GetValue();
                        }

                    }
                    int zz = (int)arg;
                    Thread.Sleep((int)arg);
                }
            }
            catch (Exception ex)
            {

            }
        }

        public void Disconnect(bool bConnectionIsShutdown = false)
        {
            try
            {
                Reset(bConnectionIsShutdown);
            }
            catch (Exception ex)
            {

            }
        }
    }
}
