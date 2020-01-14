using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Reflection;
using System.IO;


namespace OY.TotalCommander.TcPlugins.GpxViewer
{
    //[System.Runtime.InteropServices.ComVisibleAttribute(true)]
    public partial class ListerControl: UserControl
    {
        
        private string fileName;

        // get html string from resource file, not used finally
        private string GetHtml()
        {
            Assembly assembly = Assembly.GetAssembly(GetType());
            Stream myStream = assembly.GetManifestResourceStream("OY.TotalCommander.TcPlugins.GpxViewer.map.html");

            if(myStream==null)
            {
                MessageBox.Show("Resource is null");
                return null;
            }

            Encoding encode = System.Text.Encoding.GetEncoding("UTF-8");
            StreamReader myStreamReader = new StreamReader(myStream, encode);
         
            return myStreamReader.ReadToEnd();
        }

        public string GetGpxString()
        {
            return File.ReadAllText(fileName, Encoding.UTF8);
        }

        public static void Delay(int milliSecond)
        {
            int start = Environment.TickCount;
            while (Math.Abs(Environment.TickCount - start) < milliSecond)
            {
                Application.DoEvents();
            }
        }

        public ListerControl(string name)
        {
            fileName = name;
            InitializeComponent();
            this.webBrowser.Document.Click += new System.Windows.Forms.HtmlElementEventHandler(this.webBrowser_click);
        }

        private void webBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            //MessageBox.Show("webBrower load done");
            webBrowser.Focus();
            
        }

        private void  webBrowser_click(object sender, HtmlElementEventArgs e)
        {
            webBrowser.Focus();
           
        }

        private void ListerControl_Load(object sender, EventArgs e)
        {
            
            webBrowser.ObjectForScripting = this;


            // get html stream object from resource, webBrowser use it directly.
            Assembly assembly = Assembly.GetAssembly(GetType());
            Stream myStream = assembly.GetManifestResourceStream("OY.TotalCommander.TcPlugins.GpxViewer.map.html");
            if (myStream!=null)
            {
                // !!! 
                //  This is a workaround here, it seems webBrowser is asynchronous sytem, it need some delay here,
                //  otherwise, the web will not be displayed in the lister form. I found it by adding a messagebox. 
                // !!!
                //MessageBox.Show("loading");
                Delay(20);
                webBrowser.DocumentStream = myStream;
              
            }
            else
            {
                MessageBox.Show("Resource is NULL");
            }
              
            //webBrowser.Navigate("file:///c:/Users/lixinshe/source/repos/GpxViewer/map.html");

        }

    }
}
