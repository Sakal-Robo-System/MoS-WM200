using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MotoSimEG_VRC;
using WinModConnect.Logger;
using System.Threading;

namespace WinModConnect.MotoSimManager
{
    public class MotoSimController
    {
        public IController m_motoController = null;

        public delegate void delegateWinMODSignalReceived(string signalName, float signalValue);
        public event delegateWinMODSignalReceived OnWinMODSignalReceived;

        public delegate void delegateIOWinMODSignalReceived(string signalName, bool signalValue);
        public event delegateIOWinMODSignalReceived OnIOWinMODSignalReceived;

        Thread trd = null;

        Thread trd1 = null;

        public MotoSimController(IController m_motoController)
        {
            this.m_motoController = m_motoController;
            Name = m_motoController.Name;

        }

        public string Name { get; set; }
        public List<MotoSimRobot> Robots { get; set; } = new List<MotoSimRobot>();

        public List<MotoSimIOSignal<int>> IOSignals { get; set; } = new List<MotoSimIOSignal<int>>();

        public void ReadSignals(int cycleRate)
        {
            Logs.WrtieLogs("MotoSimController", "ReadSignals", "start thread");

            trd = new Thread(this.StartThreadReadRoboSignalsMoto);
            trd.Start();
            trd1 = new Thread(this.StartThreadReadIOSignals);
            trd1.Start(cycleRate);
        }

        private void StartThreadReadIOSignals(object arg)
        {
            //   IOSignals
            Logs.WrtieLogs("MotoSimController", "  ", "StartThreadReadIOSignals");
            var io = m_motoController.IO;
            var ioSignalOutput = IOSignals.Where(x => x.Type == TYPE.OUTPUT).ToList();
            try
            {
                while (true)
                {
                    foreach (var ioSig in ioSignalOutput)
                    {
                        bool val = io.GetData(ioSig.Name_int) == 0 ? false : true;

                        OnIOWinMODSignalReceived?.Invoke(ioSig.Name, val);
                    }
                    Thread.Sleep((int)arg);
                }
            }
            catch (Exception ex)
            {
                Logs.WrtieLogs("MotoSimController ", "StartThreadReadIOSignals ", ex.Message);
            }
        }

        private void StartThreadReadRoboSignalsMoto()
        {
            try
            {
                Logs.WrtieLogs("MotoSimController", "  ", "StartThreadReadRoboSignalsMoto");
                while (true)
                {
                    int z = 0;
                    foreach (var x in Robots)
                    {
                        Pulse angle;
                        angle.data = new double[(int)DefSize.PULSE_DATA];
                        angle = m_motoController.Robots[z].GetAngle();

                        for (int y = 0; y < 6; y++)
                        {
                            float roboPos = (float)angle.data[y];
                            OnWinMODSignalReceived?.Invoke(x.JointSignals[y].Name, roboPos);
                        }
                        z++;
                        Thread.Sleep(10);
                    }
                }
            }
            catch (Exception ex)
            {
                Logs.WrtieLogs("MotoSimController ", "StartThreadReadRoboSignalsMoto ", ex.Message);
            }
        }

        public void StopMotoSIMDataExchange(bool statusVal)
        {

            if (statusVal == false)
            {
                trd.Abort();
                trd1.Abort();
            }
        }
    }
}
