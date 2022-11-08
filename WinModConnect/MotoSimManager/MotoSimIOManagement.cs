using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MotoSimEG_VRC;
using System.Windows.Forms;
using WinModConnect.Logger;
using Newtonsoft.Json;
using WinModConnect.Mappers;
using System.IO;
using System.Reflection;

namespace WinModConnect.MotoSimManager
{

    class MotoSimIOManagement
    {
        MotoSimEG_VRC.Application motoApp = null;

        private Cell motoCell = null;

        private List<MotoSimController> Controllers { get; set; } = new List<MotoSimController>();

        private List<string> ControllersName { get; set; } = null;

        public event MotoSimController.delegateWinMODSignalReceived OnWinMODSignalReceivedFromController;

        public event MotoSimController.delegateIOWinMODSignalReceived OnWinMODIOSignalReceivedFromController;

        public MotoSimIOManagement(MotoSimEG_VRC.Application app)
        {
            motoApp = app;
            motoCell = motoApp.Cell;
        }

        public string Export()
        {
            Logs.WrtieLogs("", "MotoSimIOManagement.cs", "Export ");

            try
            {
                FolderBrowserDialog sfd = new FolderBrowserDialog();
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    //Export IO data into tab delemenated file. - logic for creating tab files.
                    System.IO.Directory.CreateDirectory(sfd.SelectedPath);

                    var serialNo = 1;
                    foreach (var a in Controllers)
                    {
                        string path = sfd.SelectedPath + "\\" + a.Name + ".txt";
                        string content = "Line Number" + "\t" + "Symbol" + "\t" + "Address" + "\t" + "Type" + "\t" + "default value" + "\t" + "Comment" + "\t" + "Definition internal Format" + "\t" +
                            "Definition external Format" + "\t" + "YM_Controller" + "\t" + "YM_Unit" + "\t" + "YM_Bus" + "\t" + "YM_Signal" + "\t" + "YM_Mapping" + "\t" + "signal name" +
                            "\t" + "signal type" + "\r\n";
                        System.IO.File.AppendAllText(path, content);
                        content = "";

                        foreach (var controllerMoto in a.IOSignals)
                        {
                            if (controllerMoto.Type == TYPE.OUTPUT)
                            {
                                content = serialNo.ToString() + "\t" + controllerMoto.Name + "\t" + " " + "\t" + "BM" + "\t" + " " + "\t" + "" + "\t" + " " + "\t" +
                                    " " + "\t" + a.Name + "\t" + "" + "\t" + "" + "\t" + controllerMoto.Name + "\t" + controllerMoto.Name_int + "\t" + controllerMoto.Name +
                                    "\t" + "BO" + "\r\n";
                                System.IO.File.AppendAllText(path, content);
                                content = "";
                            }
                            else if (controllerMoto.Type == TYPE.INPUT)
                            {
                                content = serialNo.ToString() + "\t" + controllerMoto.Name + "\t" + " " + "\t" + "BM" + "\t" + " " + "\t" + "" + "\t" + " " + "\t" +
                                   " " + "\t" + a.Name + "\t" + "" + "\t" + "" + "\t" + controllerMoto.Name + "\t" + controllerMoto.Name_int + "\t" + controllerMoto.Name +
                                   "\t" + "BI" + "\r\n";
                                System.IO.File.AppendAllText(path, content);
                                content = "";
                            }
                            serialNo++;
                        }

                        foreach (var c in a.Robots)
                        {
                            for (int m = 0; m < c.JointSignals.Count; m++)
                            {
                                var r = c.JointSignals[m];

                                content = serialNo.ToString() + "\t" + r.Name + "\t" + " " + "\t" + "AM" + "\t" + " " + "\t" + " " + "\t" + "DIMENSIONLESS[] +/ -1.000.000,00 SI *" + "\t" +
                             " " + "\t" + a.Name + "\t" + c.Name + "\t" + "" + "\t" + (ROBOSIGNAL)m + "\t" + "" + "\t" + r.Name +
                             "\t" + "AO" + "\r\n";
                                System.IO.File.AppendAllText(path, content);
                                content = "";

                                serialNo++;
                            }
                        }
                    }

                }
                return motoCell.Controllers.Count + " files with MotoSIM Controller Names saved at " + sfd.SelectedPath;
            }
            catch (Exception ex)
            {
                return "Error saving the I/O Signal files.";
            }

        }

        public List<String> GetControllerListName()
        {
            if (ControllersName == null)
            {
                ControllersName = new List<string>();
                foreach (var name in Controllers)
                {
                    ControllersName.Add(name.Name);
                }
            }

            return ControllersName;
        }

        public void Configure()
        {

            Logs.WrtieLogs("", "MotoSimIOManagement.cs", "Configure ");

            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\JSON" + $"\\AddressMapping";
            string addr = System.IO.File.ReadAllText(path);
            List<ControllerAddressMapping> controAddrList = JsonConvert.DeserializeObject<List<ControllerAddressMapping>>(addr);

            for (int x = 0; x < motoCell.Controllers.Count; x++)
            {

                Logs.WrtieLogs("", "MotoSimIOManagement.cs", motoCell.Controllers[x].Name);
                MotoSimController m = new MotoSimController(motoCell.Controllers[x]);
                string conName = "";
                foreach (var addrMap in controAddrList)
                {
                    if (motoCell.Controllers[x].Name.Contains(addrMap.controllerType))
                    {
                        conName = addrMap.controllerType;
                    }
                }

                if (conName != null)
                {
                    ControllerAddressMapping controAddr = controAddrList.SingleOrDefault(y => y.controllerType == conName);

                    for (var num = controAddr.starOutputAddr; num <= controAddr.endOutputAddr; num++)
                    {
                        var num1 = num % 10;
                        if (num1 != 8 && num1 != 9)
                        {
                            var io = motoCell.Controllers[x].IO;
                            MotoSimIOSignal<int> ioSig = new MotoSimIOSignal<int>()
                            {
                                Name = motoCell.Controllers[x].Name + "." + num.ToString(),
                                Value = io.GetData(num),
                                Name_int = num,
                                Type = TYPE.OUTPUT
                            };

                            m.IOSignals.Add(ioSig);
                        }
                    }

                    for (var num2 = controAddr.startInputAddr; num2 <= controAddr.endInputAddr; num2++)
                    {
                        var num3 = num2 % 10;
                        if (num3 != 8 && num3 != 9)
                        {
                            var io = motoCell.Controllers[x].IO;
                            MotoSimIOSignal<int> ioSig = new MotoSimIOSignal<int>()
                            {
                                Name = motoCell.Controllers[x].Name + "." + num2.ToString(),
                                Value = io.GetData(num2),
                                Name_int = num2,
                                Type = TYPE.INPUT

                            };
                            m.IOSignals.Add(ioSig);
                        }
                    }
                }

                //Robot
                for (int roboC = 0; roboC < motoCell.Controllers[x].Robots.Count; roboC++)
                {
                    Logs.WrtieLogs("MotoSimIOManagement.cs", "Configure---inside robot", motoCell.Controllers[x].Robots[roboC].Name);
                    MotoSimRobot robo = new MotoSimRobot()
                    {
                        Name = motoCell.Controllers[x].Robots[roboC].Name
                    };

                    Pulse angle;
                    angle.data = new double[(int)DefSize.PULSE_DATA];
                    //IRobot robot = app.Cell.Controllers[0].Robots[0];
                    angle = motoCell.Controllers[x].Robots[roboC].GetAngle();

                    for (int roboSig = 0; roboSig < 6; roboSig++)
                    {
                        string sigName = motoCell.Controllers[x].Name + "." + motoCell.Controllers[x].Robots[roboC].Name + "." + (ROBOSIGNAL)roboSig;
                        MotoSimJointSignal<float> rS = new MotoSimJointSignal<float>()
                        {
                            Name = sigName,
                            Value = (float)angle.data[roboSig],
                            // angle = angle
                        };
                        robo.JointSignals.Add(rS);
                    }

                    m.Robots.Add(robo);

                }
                Controllers.Add(m);
            }

        }

        public void generateJSONControllersAddress()
        {
            List<ControllerAddressMapping> addrList = new List<ControllerAddressMapping>();

            ControllerAddressMapping addrMap = new ControllerAddressMapping("YRC1000", 20010, 25127, 30010, 35127);

            ControllerAddressMapping addrMap1 = new ControllerAddressMapping("YRC1000micro", 20010, 21287, 30010, 31287);

            ControllerAddressMapping addrMap2 = new ControllerAddressMapping("DX200", 20010, 25127, 30010, 35127);

            ControllerAddressMapping addrMap3 = new ControllerAddressMapping("DX100", 20010, 22567, 30010, 32567);

            ControllerAddressMapping addrMap4 = new ControllerAddressMapping("FS100", 20010, 21287, 30010, 31287);

            ControllerAddressMapping addrMap5 = new ControllerAddressMapping("NX100", 20010, 21287, 30010, 31287);

            addrList.Add(addrMap);
            addrList.Add(addrMap1);
            addrList.Add(addrMap2);
            addrList.Add(addrMap3);
            addrList.Add(addrMap4);
            addrList.Add(addrMap5);

            //Save all config into json file.

            string addrListJSON = JsonConvert.SerializeObject(addrList);

            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\JSON" + $"\\AddressMapping";
            System.IO.File.WriteAllText(path, addrListJSON);

            string addr = System.IO.File.ReadAllText(path);
            List<ControllerAddressMapping> abc = JsonConvert.DeserializeObject<List<ControllerAddressMapping>>(addr);
        }

        public void startMototSIMSignalExchange(int cycleRate)
        {
            Logs.WrtieLogs("MotoSimIOManagement", "startMototSIMSignalExchange", " ");
            List<ControllerCOMMapping> abc = GenericStaticClass.getMapping();

            foreach (var x in Controllers)
            {
                var con = abc.SingleOrDefault(y => y.ControllerName == x.Name);
                if (con != null)
                {
                    Logs.WrtieLogs("MotoSimIOManagement", "StartMotoSIMExchange()", "events added ");
                    x.OnWinMODSignalReceived += receiveSignalFromMotoSIM;
                    x.OnIOWinMODSignalReceived += receiveIOSignalFromMotoSIM;
                    x.ReadSignals(cycleRate);
                }
            }
        }

        public void stopMotoSIMSignalExchange()
        {
            List<ControllerCOMMapping> abc = GenericStaticClass.getMapping();
            foreach (var x in Controllers)
            {
                var con = abc.SingleOrDefault(y => y.ControllerName == x.Name);
                if (con != null)
                {
                    Logs.WrtieLogs("MotoSimIOManagement", "StartMotoSIMExchange()", "events remove ");
                    x.StopMotoSIMDataExchange(false);
                    x.OnWinMODSignalReceived -= receiveSignalFromMotoSIM;
                    x.OnIOWinMODSignalReceived -= receiveIOSignalFromMotoSIM;
                }
            }
        }

        public void receiveSignalFromMotoSIM(string name, float value)
        {
            OnWinMODSignalReceivedFromController?.Invoke(name, value);
        }

        public void receiveIOSignalFromMotoSIM(string name, bool value)
        {
            OnWinMODIOSignalReceivedFromController?.Invoke(name, value);
        }

        public void SendDataToMototSIM(string controllerName, string signal, bool value)
        {
            MotoSimController mContr = Controllers.SingleOrDefault(x => x.Name == controllerName);

            var io = mContr.m_motoController.IO;

            if (mContr != null)
            {
                var ioSignal = mContr.IOSignals.SingleOrDefault(y => y.Name == signal && y.Type == TYPE.INPUT);
                if (ioSignal != null)
                {
                    if (value == true)
                        io.SetData(ioSignal.Name_int, 1);
                    else
                        io.SetData(ioSignal.Name_int, 0);
                }
            }
        }
    }
}
