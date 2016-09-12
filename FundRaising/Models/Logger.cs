using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace FundRaising.Models
{
    public class Logger
    {

        public static Logger _Instance;

        public static Logger Instance
        {
            get
            {
                if(_Instance==null)
                {
                    _Instance = new Logger();
                }
                return _Instance;
            }
        }


        public string FilePath
        {
            get
            {
                
                string path = HttpContext.Current.Server.MapPath("/Data/Error.txt");
                //FileStream fs = new FileStream(path, FileMode.Append, FileAccess.Write);
                return path;
            }

        }

        public void Log(string message)
        {
            try
            {
                string path = HttpContext.Current.Server.MapPath("/Data/Log.txt");
                FileStream fs = new FileStream(path, FileMode.Append, FileAccess.Write);
                StreamWriter sw = new StreamWriter(FilePath, true);
                sw.WriteLine("\n-------------------------------Log--------------------------------------\n");

                sw.WriteLine("Message Logged At: " + DateTime.Now);
                sw.WriteLine("Message Details:  " + message);
                sw.Flush();
                sw.Close();
            }
            catch
            {

            }
            
        }


        public void LogException(Exception ex)
        {
            StreamWriter sw = new StreamWriter(FilePath,true);
            sw.WriteLine("\n-------------------------------Log--------------------------------------\n");

            sw.WriteLine("Message Logged At: " + DateTime.Now);
            sw.WriteLine("Exception Details: \n \n \n Message : " + ex.Message);

            sw.WriteLine("\n\nException Source:  " + ex.Source);

            sw.WriteLine("\n\nException StackTrace:  " + ex.StackTrace);
            sw.Flush();
            sw.Close();
        }



    }
}