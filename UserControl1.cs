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

        public int i = 0;
        public int j = 0;

        public double[] lats;
        public double[] lons;

        public int total_count;

        public double GetLat()
        {
            double v = lats[i];
            i++;
            return v;

        }
        public double GetLon()
        {
            double v = lons[j];
            j++;
            return v;
        }

        public int GetCount()
        {
            return total_count;
        }

        private double FindMean(double[] d, int cnt)
        {
            int i;
            double min, max;

            if (cnt == 0) return 0;

            min = max = d[0];

            for (i = 0; i < cnt; i++)
            {
                if (d[i] > max) max = d[i];
                if (d[i] < min) min = d[i];
            }
            return (max + min) / 2;
        }

        public double GetCenterLat()
        {

            return FindMean(lats, total_count);
        }


        public double GetCenterLon()
        {
            return FindMean(lons, total_count);
        }

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
            XmlDocument doc = new XmlDocument();
    
            doc.Load(fileName);
            XmlElement rootElem = doc.DocumentElement;

            XmlNodeList trkpts = rootElem.GetElementsByTagName("trkpt");

            lats = new double[trkpts.Count];
            lons = new double[trkpts.Count];

            total_count = 0;
            foreach (XmlNode node in trkpts)
            {

                string lon = ((XmlElement)node).GetAttribute("lon");
                string lat = ((XmlElement)node).GetAttribute("lat");

                double[] coo = wgs2bd(Convert.ToDouble(lat), Convert.ToDouble(lon));


                lats[total_count] = coo[0];
                lons[total_count] = coo[1];

                total_count++;
            }
            
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




        //[start]coordinate convertion is from blog :   https://blog.csdn.net/zyh_1988/article/details/80389411
        
        
        static double pi = 3.14159265358979324;
        static double a = 6378245.0;
        static double ee = 0.00669342162296594323;
        public static double x_pi = 3.14159265358979324 * 3000.0 / 180.0;

        static double[] wgs2gcj(double lat, double lon)
        {
            double dLat = transformLat(lon - 105.0, lat - 35.0);
            double dLon = transformLon(lon - 105.0, lat - 35.0);
            double radLat = lat / 180.0 * pi;
            double magic = Math.Sin(radLat);
            magic = 1 - ee * magic * magic;
            double sqrtMagic = Math.Sqrt(magic);
            dLat = (dLat * 180.0) / ((a * (1 - ee)) / (magic * sqrtMagic) * pi);
            dLon = (dLon * 180.0) / (a / sqrtMagic * Math.Cos(radLat) * pi);
            double mgLat = lat + dLat;
            double mgLon = lon + dLon;
            double[] loc = { mgLat, mgLon };
            return loc;
        }
        static double[] wgs2bd(double lat, double lon)
        {
            double[] wgs_gcj = wgs2gcj(lat, lon);
            double[] gcj_bd = gcj2bd(wgs_gcj[0], wgs_gcj[1]);
            return gcj_bd;
        }
        public static double[] gcj2bd(double lat, double lon)
        {
            double x = lon, y = lat;
            double z = Math.Sqrt(x * x + y * y) + 0.00002 * Math.Sin(y * x_pi);
            double theta = Math.Atan2(y, x) + 0.000003 * Math.Cos(x * x_pi);
            double bd_lon = z * Math.Cos(theta) + 0.0065;
            double bd_lat = z * Math.Sin(theta) + 0.006;
            return new double[] { bd_lat, bd_lon };
        }

        public static double[] bd2gcj(double lat, double lon)
        {
            double x = lon - 0.0065, y = lat - 0.006;
            double z = Math.Sqrt(x * x + y * y) - 0.00002 * Math.Sin(y * x_pi);
            double theta = Math.Atan2(y, x) - 0.000003 * Math.Cos(x * x_pi);
            double gg_lon = z * Math.Cos(theta);
            double gg_lat = z * Math.Sin(theta);
            return new double[] { gg_lat, gg_lon };
        }
        private static double transformLat(double lat, double lon)
        {
            double ret = -100.0 + 2.0 * lat + 3.0 * lon + 0.2 * lon * lon + 0.1 * lat * lon + 0.2 * Math.Sqrt(Math.Abs(lat));
            ret += (20.0 * Math.Sin(6.0 * lat * pi) + 20.0 * Math.Sin(2.0 * lat * pi)) * 2.0 / 3.0;
            ret += (20.0 * Math.Sin(lon * pi) + 40.0 * Math.Sin(lon / 3.0 * pi)) * 2.0 / 3.0;
            ret += (160.0 * Math.Sin(lon / 12.0 * pi) + 320 * Math.Sin(lon * pi / 30.0)) * 2.0 / 3.0;
            return ret;
        }

        private static double transformLon(double lat, double lon)
        {
            double ret = 300.0 + lat + 2.0 * lon + 0.1 * lat * lat + 0.1 * lat * lon + 0.1 * Math.Sqrt(Math.Abs(lat));
            ret += (20.0 * Math.Sin(6.0 * lat * pi) + 20.0 * Math.Sin(2.0 * lat * pi)) * 2.0 / 3.0;
            ret += (20.0 * Math.Sin(lat * pi) + 40.0 * Math.Sin(lat / 3.0 * pi)) * 2.0 / 3.0;
            ret += (150.0 * Math.Sin(lat / 12.0 * pi) + 300.0 * Math.Sin(lat / 30.0 * pi)) * 2.0 / 3.0;
            return ret;
        }

        
        //[end] coordinate convertion is from blog :   https://blog.csdn.net/zyh_1988/article/details/80389411

    }
}
