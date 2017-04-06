using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;
using log4net.Config;
using System.Reflection;


namespace TSApi
{
    public class Logger
    {
        private static readonly ILog logger =
           LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);



        static Logger()
        {

            XmlConfigurator.Configure();
        }

        public static void Debug(string s)
        {
            logger.Debug(s);
        }

        public static void Error(string s)
        {
            logger.Error(s);
        }



        public static void Error(string s, Exception e)
        {
            logger.Error(s, e);
        }

        public static void Info(string s)
        {
            logger.Info(s);
        }
    }


}