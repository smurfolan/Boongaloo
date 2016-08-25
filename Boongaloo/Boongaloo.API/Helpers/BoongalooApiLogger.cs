using System;
using System.Reflection;
using log4net;

namespace Boongaloo.API.Helpers
{
    public static class BoongalooApiLogger
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        
        public static void LogInfo(string message)
        {
            Log.Info(message);
        }

        public static void LogError(string message, Exception ex)
        {
            Log.Error(message, ex);
        }
    }
}