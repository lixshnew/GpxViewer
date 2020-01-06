using System;
using System.Collections;
using System.Collections.Specialized;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Windows.Forms;

using OY.TotalCommander.TcPluginInterface;
using OY.TotalCommander.TcPluginInterface.Lister;
using System.Diagnostics;


namespace OY.TotalCommander.TcPlugins.GpxViewer
{
    public class GpxViewer : ListerPlugin
    {

        public ListerControl lc;
        public GpxViewer(StringDictionary pluginSettings) : base(pluginSettings)
        {
            if (String.IsNullOrEmpty(Title))
                Title = ".NET GpxViewer";
            DetectString = "EXT=\"GPX\"";
        }
        
        public override object Load(string fileToLoad, ShowFlags showFlags)
        {
            lc = null;
            if (!String.IsNullOrEmpty(fileToLoad))
            {
                lc = new ListerControl(fileToLoad);
                FocusedControl = lc.webBrowser;
            }
            
            return lc;
        }
        
        public override ListerResult LoadNext(object control, string fileToLoad, ShowFlags showFlags)
        {
            return ListerResult.OK;
        }

        public override void CloseWindow(object control)
        {
            lc.Dispose(); // make focus back to TC , strange!
        }

        public override ListerResult SearchText(object control, string searchString, SearchParameter searchParameter)
        {
            return ListerResult.OK;
        }

        public override ListerResult SendCommand(object control, ListerCommand command, ShowFlags parameter)
        {
            return ListerResult.OK;
        }

        public override ListerResult Print(object control, string fileToPrint, string defPrinter,
                PrintFlags printFlags, PrintMargins pMargins)
        {
            return ListerResult.OK;
        }

        public override int NotificationReceived(object control, int message, int wParam, int lParam)
        {
            // do nothing
            return 0;
        }

        public override Bitmap GetPreviewBitmap(string fileToLoad, int width, int height, byte[] contentBuf)
        {
            Bitmap bitmap = new Bitmap(width, height);
            return bitmap;
        }

        public override ListerResult SearchDialog(object control, bool findNext)
        {
            return ListerResult.OK;
        }



        public override void OnTcTrace(TraceLevel level, string text)
        {

        }
        
    }
}
