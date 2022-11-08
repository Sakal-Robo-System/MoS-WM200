using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Runtime.InteropServices;
using MotoSimEG_VRC;
using WinModConnect.MotoSimManager;
using WinModConnect.Logger;
using WinModConnect.WinModManager;


namespace WinModConnect
{
    /// <summary>
    /// Summary description for WinModConnect.
    /// </summary>
    [Guid("e24e280b-b837-475d-946b-45b56ca1cf06")]
    public class MotoSimPlugin : IMotoSimPlugin
    {
        #region Local Variables
        MotoSimEG_VRC.Application app = null;   // MotoSimEG-VRC object
        IRibbonMgr iRibbon = null;              // RibbonManager object
        int PluginID = 0;                       // Unique ID assigned by the MotoSimEG-VRC
        MotoStruct MotoSt = null;               // Structure initialization object

        // TODO: Please add a new variable for each category
        int GrpID1 = 0;                         // Category ID of the ribbon

        WinMODConnectorForm plgForm;
        #endregion

        #region Initialization/Termination processing
        public MotoSimPlugin()
        {
        }

        // Plug-in connection processing
        public void ConnectToMotoSim(object MotoSimObj, int cookie)
        {
            Logs.OpenFile();
            app = (MotoSimEG_VRC.Application)MotoSimObj;
            PluginID = cookie;
            app.ScreenUpdating = 1;             // This code should not be changed
            MotoSt = new MotoStruct();

            AddRibbons();                       // Ribbon menu construction
            AttachEventHandlers();              // Event handler registration

            plgForm = new WinMODConnectorForm(app);

            return;
        }

        // Plug-in cutting process
        public void DisconnectFromMotoSim()
        {
            RemoveRibbons();                    // Ribbon menu Remove
            //DetachEventHandlers();              // Event handler Detach 

            Marshal.ReleaseComObject(iRibbon);
            iRibbon = null;
            Marshal.ReleaseComObject(app);
            app = null;

            GC.Collect();
            GC.WaitForPendingFinalizers();
            return;
        }
        #endregion

        // TODO: Please add the construction / removal process of the ribbon menu.
        #region UI Methods
        // Ribbon menu construction
        bool AddRibbons()
        {
            iRibbon = app.RibbonManager;
            // Create a file icon from the resource
            BitmapIcon iBmp = new BitmapIcon();

            string smallIconFile = iBmp.CreateFile(Properties.Resources.WinmodSmall3);
            string largeIconFile = iBmp.CreateFile(Properties.Resources.WinmodLarge3);


            GrpID1 = iRibbon.AddCategory(PluginID, "WinMOD MS200", smallIconFile, largeIconFile);
            Logs.WrtieLogs("", "AddRibbons", "GrpID1 " + GrpID1);
            iRibbon.AddButton(PluginID, GrpID1, "", "Connect to WinMOD", "", "", 0, 0, "NewIO", "EnableFunc1");

            // Delete icon file
            iBmp.CleanFiles();
            return true;
        }

        // Ribbon menu Remove
        bool RemoveRibbons()
        {
            iRibbon.RemoveCategory(PluginID, GrpID1);
            //iRibbon.RemoveCategory(PluginID, GrpID2);
            return true;
        }
        #endregion

        // TODO: Please implement the command handler for the ribbon button that you added.
        #region UI Callbacks
        //========================================
        // Command handler of the ribbon button
        //========================================



        public void NewIO()
        {
            plgForm.ShowDialog();
        }



        public void CallbackFuncX() { }

        //========================================
        // Ribbon button use determination function
        //========================================
        public int EnableFunc1()
        {
            if (app.Cell == null)
                return 0;
            return 1;
        }  //Enable


        public int EnableFuncX() { return 0; }  //Disable
        #endregion

        // TODO: If you handles the event, please register an event handler
        #region Event Methods
        // Event handler registration
        bool AttachEventHandlers()
        {
            try
            {
                app.DestroyNotify += new IAppEvents_DestroyNotifyEventHandler(OnDestroyNotify);
                app.FileOpenNotify += new IAppEvents_FileOpenNotifyEventHandler(OnFileOpenNotify);
                app.FileCloseNotify += new IAppEvents_FileCloseNotifyEventHandler(OnFileCloseNotify);
                app.FileSaveNotify += new IAppEvents_FileSaveNotifyEventHandler(OnFileSaveNotify);
                app.JobPlaybackStart += new IAppEvents_JobPlaybackStartEventHandler(OnJobPlaybackStart);
                app.JobPlaybackEnd += new IAppEvents_JobPlaybackEndEventHandler(OnJobPlaybackEnd);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        // Event handler Detach 
        bool DetachEventHandlers()
        {
            try
            {
                app.DestroyNotify -= new IAppEvents_DestroyNotifyEventHandler(OnDestroyNotify);
                app.FileOpenNotify -= new IAppEvents_FileOpenNotifyEventHandler(OnFileOpenNotify);
                app.FileCloseNotify -= new IAppEvents_FileCloseNotifyEventHandler(OnFileCloseNotify);
                app.FileSaveNotify -= new IAppEvents_FileSaveNotifyEventHandler(OnFileSaveNotify);
                app.JobPlaybackStart -= new IAppEvents_JobPlaybackStartEventHandler(OnJobPlaybackStart);
                app.JobPlaybackEnd -= new IAppEvents_JobPlaybackEndEventHandler(OnJobPlaybackEnd);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }
        #endregion

        // TODO: Please implement the event handler that you added.
        #region Event Handlers
        public void OnDestroyNotify()
        {
            //MessageBox.Show("Event MotoSimEG-VRC Close");
        }
        public void OnFileOpenNotify()
        {
            //MessageBox.Show("Event Cell Open");
        }
        public void OnFileCloseNotify()
        {
            //MessageBox.Show("Event Cell Close");
        }
        public void OnFileSaveNotify()
        {
            //MessageBox.Show("Event File Save");
        }
        public void OnJobPlaybackStart()
        {
            //MessageBox.Show("Event Playback Start");
        }
        public void OnJobPlaybackEnd()
        {
            //MessageBox.Show("Event Playback End");
        }
        #endregion
    }
}
