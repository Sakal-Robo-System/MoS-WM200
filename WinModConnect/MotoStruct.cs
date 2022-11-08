using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MotoSimEG_VRC;

namespace WinModConnect
{
    class MotoStruct
    {
        public PartData PartData
        {
            get
            {
                PartData data;
                data.data = new double[(int)DefSize.PART_DATA];
                return data;
            }
        }
        public Position Position
        {
            get
            {
                Position data;
                data.data = new double[(int)DefSize.POSITION_DATA];
                return data;
            }
        }
        public Pulse Pulse
        {
            get
            {
                Pulse data;
                data.data = new double[(int)DefSize.PULSE_DATA];
                return data;
            }
        }
        public ToolData ToolData
        {
            get
            {
                ToolData data;
                data.data = new double[(int)DefSize.TOOL_POSITION];
                data.gravity = new double[(int)DefSize.TOOL_GRAVITY];
                data.inertia = new double[(int)DefSize.TOOL_INERTIA];
                data.weight = 0;
                return data;
            }
        }
        public UserFrameData UserFrameData
        {
            get
            {
                UserFrameData data;
                data.data = new double[(int)DefSize.USERFRAME_DATA];
                return data;
            }
        }
    }
}
