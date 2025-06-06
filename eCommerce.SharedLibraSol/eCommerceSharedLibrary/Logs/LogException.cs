﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;
namespace eCommerceSharedLibrary.Logs
{
    // Cấu hình Serilog log các Exception thông qua File, Console và Debug 
    public static class LogException
    {
        public static void LogExceptions(Exception ex)
        {
            LogToFile(ex.Message);
            LogToConsole(ex.Message);
            LogToDebugger(ex.Message);
        }
        public static void LogToFile(string message) => Log.Information(message);
        public static void LogToConsole(string message) => Log.Warning(message);
        public static void LogToDebugger(string message) => Log.Debug(message);
    }
}
