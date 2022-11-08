using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WinModConnect.MotoSimManager;
using WinModConnect.WinModManager;
using WinModConnect.Mappers;
using Newtonsoft.Json;
using System.IO;
using System.Reflection;
using System.Threading;
using WinModConnect.Logger;
using System.Runtime.InteropServices;

namespace WinModConnect
{
    public partial class WinMODConnectorForm : Form
    {
        MotoSimEG_VRC.Application appm;

        MotoSimIOManagement motoSimIOManagement = null;

        WinModIOManager winModIOMgr = null;

        List<string> COMelementsName = new List<string>();

        List<ControllerCOMMapping> controllerCOMMap = null;

        List<string> controllersName = null;

        bool isDataExchangeRunning = false;

        bool isWinMODConnected = false;

        public WinMODConnectorForm(MotoSimEG_VRC.Application app)
        {
            appm = app;
            InitializeComponent();
            controllerCOMMap = new List<ControllerCOMMapping>();

            GenericStaticClass.readErrorMapping();

            MappinglistView.Columns.Add("MotoSIM Controllers", 250, HorizontalAlignment.Left);
            MappinglistView.Columns.Add("WinMOD COM Elements", 250, HorizontalAlignment.Left);

            LogslistView.Columns.Add("Message", 350, HorizontalAlignment.Left);
            LogslistView.Columns.Add("Timestamp", 150, HorizontalAlignment.Left);

        }


        public void ShowLogsInListView(string logMsg)
        {
            LogslistView.Items.Add(new ListViewItem(new string[] { logMsg, DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss tt") }));
        }

        private void ExportSignals_Click(object sender, EventArgs e)
        {

            if (motoSimIOManagement != null)
            {
                string msg = motoSimIOManagement.Export();

                ShowLogsInListView(msg);

                ShowLogsInListView("Files contain I/O Signals. Import in respective COM element in WinMOD project.");

            }

        }

        private void StartDataExchange_Click(object sender, EventArgs e)
        {
            try
            {
                if (isDataExchangeRunning == false)
                {
                    winModIOMgr.GenerateCOMElements();

                    var status = winModIOMgr.OpenCOMElement();

                    if (status == true)
                    {
                        Logs.WrtieLogs("WinMODConnectorForm", "StartDataExchange()", "Data Exchange Started ");
                        var x = CycleRate.Text;
                        motoSimIOManagement.startMototSIMSignalExchange(int.Parse(x));

                        //signals from winMOD to MototSIM
                        winModIOMgr.StartSignalExchange(int.Parse(x));

                        isDataExchangeRunning = true;

                        ShowLogsInListView("Started data exchange between MotoSIM and WinMOD");
                        ShowLogsInListView("Start MotoSIM simulation to exhange data.");
                        groupBox2.Enabled = false;
                        StartDataExchange.Text = "Stop";
                    }
                }
                else
                {
                    isDataExchangeRunning = false;
                    motoSimIOManagement.stopMotoSIMSignalExchange();
                    winModIOMgr.StopSignalExchange();
                    ShowLogsInListView("Stopped data exchange betwee MotoSIM and WinMOD");
                    Logs.WrtieLogs("WinMODConnectorForm", "StopDataExchange()", "Data Exchange stopped ");
                    groupBox2.Enabled = true;
                    StartDataExchange.Text = "Start";
                }
            }
            catch (Exception ex)
            {
                string str = WinMODComm.GetHResultString(ex);

                string newstr = str.Substring(str.IndexOf("0x"));
                string newstr1 = newstr.Remove(newstr.Length - 1);
                List<ErrorMessagesMapping> errM = GenericStaticClass.getErrorMapping();
                if (str != null && errM != null)
                {
                    var msg = errM.SingleOrDefault(x => x.errorCode == newstr1);
                    if (msg != null)
                        ShowLogsInListView(msg.errorMessage);
                }
                else
                {
                    ShowLogsInListView(ex.Message);
                }
            }

        }

        //signals received from WinMOD
        public void WinMODSignalReceived(string signal, bool value)
        {
            try
            {
                string contro = signal;
                int index = contro.IndexOf('.');
                contro = contro.Substring(0, index);
                motoSimIOManagement.SendDataToMototSIM(contro, signal, value);
            }
            catch (Exception ex)
            {
                ShowLogsInListView("MotoSIM error Please restart it.");
            }
        }

        //signals received from MotoSIM
        public void MotoSIMIOSignalReceived(string sigName, bool sigValue)
        {
            try
            {
                List<ControllerCOMMapping> abc = GenericStaticClass.getMapping();
                string com = sigName;
                int index = com.IndexOf('.');
                com = com.Substring(0, index);

                var comSig = abc.SingleOrDefault(z => z.ControllerName == com);

                if (comSig != null)
                    winModIOMgr.SendData(comSig.COMElementName, sigName, 0, sigValue);
            }
            catch (Exception ex)
            {
                string str = WinMODComm.GetHResultString(ex);

                string newstr = str.Substring(str.IndexOf("0x"));
                string newstr1 = newstr.Remove(newstr.Length - 1);
                List<ErrorMessagesMapping> errM = GenericStaticClass.getErrorMapping();
                if (str != null && errM != null)
                {
                    var msg = errM.SingleOrDefault(x => x.errorCode == newstr1);
                    if (msg != null)
                        ShowLogsInListView(msg.errorMessage);
                }
            }

        }

        public void MotoSIMSignalReceived(string sigName, float sigValue)
        {
            try
            {
                List<ControllerCOMMapping> abc = GenericStaticClass.getMapping();
                string com = sigName;
                int index = com.IndexOf('.');
                com = com.Substring(0, index);

                var comSig = abc.SingleOrDefault(z => z.ControllerName == com);

                if (comSig != null)
                    winModIOMgr.SendData(comSig.COMElementName, sigName, sigValue, false);
            }
            catch (Exception ex)
            {
                string str = WinMODComm.GetHResultString(ex);

                string newstr = str.Substring(str.IndexOf("0x"));
                string newstr1 = newstr.Remove(newstr.Length - 1);
                List<ErrorMessagesMapping> errM = GenericStaticClass.getErrorMapping();
                if (str != null && errM != null)
                {
                    var msg = errM.SingleOrDefault(x => x.errorCode == newstr1);
                    if (msg != null)
                        ShowLogsInListView(msg.errorMessage);
                }
            }

        }


        private void PluginForm_Load(object sender, EventArgs e)
        {
            try
            {
                if (motoSimIOManagement == null)
                {
                    motoSimIOManagement = new MotoSimIOManagement(appm);

                    motoSimIOManagement.Configure();

                    controllersName = motoSimIOManagement.GetControllerListName();

                    foreach (var cName in controllersName)
                    {
                        motoSIMNames.Items.Add(cName);
                    }

                    motoSimIOManagement.OnWinMODSignalReceivedFromController += MotoSIMSignalReceived;
                    motoSimIOManagement.OnWinMODIOSignalReceivedFromController += MotoSIMIOSignalReceived;
                }
            }
            catch (Exception ex)
            {
                ShowLogsInListView("MotoSIM error Please restart it.");
            }
        }

        private void addMapping_Click(object sender, EventArgs e)
        {
            var obj = motoSIMNames.SelectedItem;
            string controllerName = Convert.ToString(obj);

            var obj1 = winMODNames.SelectedItem;
            string comElementName = Convert.ToString(obj1);

            if (controllerName == "" || comElementName == "")
                return;

            ControllerCOMMapping map = new ControllerCOMMapping(controllerName, comElementName);

            controllerCOMMap.Add(map);

            MappinglistView.Items.Add(new ListViewItem(new string[] { controllerName, comElementName }));

            if (motoSIMNames.Items.Count == 1)
            {
                motoSIMNames.Text = string.Empty;
                motoSIMNames.Items.Clear();
            }
            else
            {
                motoSIMNames.Items.Remove(obj);
            }
            if (winMODNames.Items.Count == 1)
            {
                winMODNames.Text = string.Empty;
                winMODNames.Items.Clear();
            }
            else
            {
                motoSIMNames.Items.Remove(obj);
                winMODNames.Items.Remove(obj1);               
            }

            SaveMapping.Enabled = true;

        }

        private void GetCOMElements_Click(object sender, EventArgs e)
        {
            try
            {
                winModIOMgr.GetCOMElements();
            }
            catch (Exception ex)
            {
                ShowLogsInListView("Error extracting WinMOD COM element names");
            }
        }

        private void FillCOMElementListUI(object sender, EventArgs e)
        {
            try
            {
                COMelementsName.Clear();

                var comList = winModIOMgr.GetCOMElementsName();
                foreach (var x in comList)
                    COMelementsName.Add(x);

                ShowLogsInListView("Successfully extracted WinMOD COM element names.");

                ResetMappingSettings();
            }
            catch (Exception ex)
            {
                ShowLogsInListView("Error extracting WinMOD COM element names");
            }
        }

        private void connectToWinMOD_Click(object sender, EventArgs e)
        {
            try
            {
                if (winModIOMgr == null)
                {
                    winModIOMgr = new WinModIOManager();
                    winModIOMgr.ConnectionStateChangedUpdate += ConnectionStateChangedUpdateUI;
                    winModIOMgr.WinMODCOMElementsListUpdate += FillCOMElementListUI;
                    winModIOMgr.OnCOMSignalReceivedFromCOMElement += WinMODSignalReceived;
                }

                if (isWinMODConnected == false)
                {
                    string host = comHostTextBox.Text;
                    int port = int.Parse(comPortTextBox.Text);
                    winModIOMgr.Connect(host, port, int.Parse(comTimeoutTextBox.Text));
                }
                else
                {
                    if (isDataExchangeRunning == true)
                    {
                        isDataExchangeRunning = false;
                        motoSimIOManagement.stopMotoSIMSignalExchange();
                        winModIOMgr.StopSignalExchange();
                        StartDataExchange.Text = "Start";
                    }
                    winModIOMgr.DisconnectWinMOD();
                }

            }
            catch (Exception ex)
            {
                string str = WinMODComm.GetHResultString(ex);
                string newstr = str.Substring(str.IndexOf("0x"));
                string newstr1 = newstr.Remove(newstr.Length - 1);
                List<ErrorMessagesMapping> errM = GenericStaticClass.getErrorMapping();
                if (str != null && errM != null)
                {
                    var msg = errM.SingleOrDefault(x => x.errorCode == newstr1);
                    if (msg != null)
                        ShowLogsInListView(msg.errorMessage);
                }
            }
        }

        private void ConnectionStateChangedUpdateUI(object sender, EventArgs e)
        {
            if (isWinMODConnected == false)
            {
                connectToWinMOD.Text = "Disconnect";
                groupBox2.Enabled = true;
                ShowLogsInListView("Connection to WinMOD established.");
                isWinMODConnected = true;
                comTimeoutTextBox.Enabled = false;
            }
            else
            {
                if (isDataExchangeRunning == true)
                {
                    isDataExchangeRunning = false;
                    motoSimIOManagement.stopMotoSIMSignalExchange();
                    winModIOMgr.StopSignalExchange();
                    StartDataExchange.Text = "Start";
                }
                connectToWinMOD.Text = "Connect";
                groupBox2.Enabled = false;
                COMelementsName.Clear();
                ResetMappingSettings();
                isWinMODConnected = false;
                comTimeoutTextBox.Enabled = true;

                ShowLogsInListView("Disconnected from WinMOD.");
            }
        }

        private void SaveMapping_Click(object sender, EventArgs e)
        {
            try
            {
                string addrListJSON = JsonConvert.SerializeObject(controllerCOMMap);

                string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\JSON" + $"\\COMControllerMapping";
                if (File.Exists(path))
                {
                    File.Delete(path);
                }

                System.IO.File.WriteAllText(path, addrListJSON);

                GenericStaticClass.resetMapping();

                ShowLogsInListView("Saved MotoSIM Controller and WinMOD COM mapping.");

                DataExchange.Enabled = true;
            }
            catch (Exception ex)
            {
                ShowLogsInListView("Error saving the mapping.");
            }
        }

        private void ResetMapping_Click(object sender, EventArgs e)
        {
            try
            {
                ResetMappingSettings();

                string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\JSON" + $"\\COMControllerMapping";
                if (File.Exists(path))
                {
                    File.Delete(path);
                }

                ShowLogsInListView("Removed MotoSIM Controller and WinMOD COM mapping.");
            }
            catch (Exception ex)
            {
                ShowLogsInListView("Error removing the mapping.");
            }
        }

        private void ResetMappingSettings()
        {
            winMODNames.Items.Clear();

            motoSIMNames.Items.Clear();

            controllerCOMMap.Clear();

            MappinglistView.Clear();

            SaveMapping.Enabled = false;

            DataExchange.Enabled = false;

            MappinglistView.Columns.Add("MotoSIM Controllers", 250, HorizontalAlignment.Left);
            MappinglistView.Columns.Add("WinMOD COM Elements", 250, HorizontalAlignment.Left);

            if (COMelementsName.Count != 0)
            {
                foreach (var cName in COMelementsName)
                {
                    winMODNames.Items.Add(cName);
                }
            }

            if (controllersName.Count != 0)
            {
                foreach (var cName in controllersName)
                {
                    motoSIMNames.Items.Add(cName);
                }
            }
        }

        public void ErrorMapping(object sender, EventArgs ez)
        {
            ErrorMessagesMapping e = new ErrorMessagesMapping("0x80070102", "Error connecting to WinMOD! Synchronization of communication failed.");

            ErrorMessagesMapping e1 = new ErrorMessagesMapping("0x80042741", "Error connecting to WinMOD! Requested address is invalid.");

            ErrorMessagesMapping e2 = new ErrorMessagesMapping("0x80042AF9", "Error connecting to WinMOD! Unknown destination address.");

            ErrorMessagesMapping e3 = new ErrorMessagesMapping("0x8004274C", "Error connecting to WinMOD! Connection timed out.");

            ErrorMessagesMapping e4 = new ErrorMessagesMapping("0x8004274D", "Error connecting to WinMOD! Requested service not available.");

            ErrorMessagesMapping e5 = new ErrorMessagesMapping("0x80044102", "Error connecting to WinMOD! Connection timed out.");

            ErrorMessagesMapping e6 = new ErrorMessagesMapping("0x8004C001", "Error connecting to WinMOD! Maximum connections exceeded.");

            ErrorMessagesMapping e7 = new ErrorMessagesMapping("0x8004C002", "Error connecting to WinMOD! Refused by WinMOD IP address filter.");

            ErrorMessagesMapping e8 = new ErrorMessagesMapping("0x8004C004", "Invalid license. Check the WinMOD license and software version.");

            ErrorMessagesMapping e9 = new ErrorMessagesMapping("0x80042775", "Connection cancelled by the destination computer.");

            ErrorMessagesMapping e10 = new ErrorMessagesMapping("0x800704E3", "Error sending or receiving data from WinMOD.");

            ErrorMessagesMapping e11 = new ErrorMessagesMapping("0x80040154", "Error connecting to WinMOD! WMCommLib library is not registered.");

            ErrorMessagesMapping e12 = new ErrorMessagesMapping("0x8007007E", "Error connecting to WinMOD! WMCommLib.dll does not exist in the specified path");

            ErrorMessagesMapping e13 = new ErrorMessagesMapping("E_INVALIDARG", "WinMOD Error: Invalid arguments");

            ErrorMessagesMapping e14 = new ErrorMessagesMapping("E_OUTOFMEMORY", "Failed to allocate necessary memory");

            ErrorMessagesMapping e15 = new ErrorMessagesMapping("E_ACCESSDENIED", "MotoSIM Error: General access denied error");

            ErrorMessagesMapping e16 = new ErrorMessagesMapping("E_FAIL", "MotoSIM Error: Unspecified failure");

            ErrorMessagesMapping e17 = new ErrorMessagesMapping("E_INVALIDARG", "MotoSIM Error: One or more arguments are not vaild");

            ErrorMessagesMapping e18 = new ErrorMessagesMapping("E_NOINTERFACE", "MotoSIM Error: No such interface supported");

            ErrorMessagesMapping e19 = new ErrorMessagesMapping("E_POINTER", "MotoSIM Error: Pointer that is not valid");

            ErrorMessagesMapping e20 = new ErrorMessagesMapping("E_UNEXPECTED", "MotoSIM Error: Unexpected failure");

            List<ErrorMessagesMapping> errList = new List<ErrorMessagesMapping>();

            errList.Add(e);
            errList.Add(e1);
            errList.Add(e2);
            errList.Add(e3);
            errList.Add(e4);
            errList.Add(e5);
            errList.Add(e6);
            errList.Add(e7);
            errList.Add(e8);
            errList.Add(e9);
            errList.Add(e10);
            errList.Add(e11);
            errList.Add(e12);
            errList.Add(e13);
            errList.Add(e14);
            errList.Add(e15);
            errList.Add(e16);
            errList.Add(e17);
            errList.Add(e18);
            errList.Add(e19);
            errList.Add(e20);

            string errListJSON = JsonConvert.SerializeObject(errList);

            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\JSON" + $"\\ErrorListJSON";
            System.IO.File.WriteAllText(path, errListJSON);

            string err = System.IO.File.ReadAllText(path);
            List<ErrorMessagesMapping> abc = JsonConvert.DeserializeObject<List<ErrorMessagesMapping>>(err);

        }

    }
}
