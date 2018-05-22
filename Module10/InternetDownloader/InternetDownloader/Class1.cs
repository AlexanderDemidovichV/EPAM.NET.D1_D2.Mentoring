using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogProvider;

namespace InternetDownloader
{
    public static class Class1
    {
        public static void Met()
        {
            var logger = NLogProvider.GetLogger("mySuperLogger", CultureInfo.CurrentCulture);
            logger.Info("hello");
        }
    }
}
