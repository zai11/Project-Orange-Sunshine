using System;
using System.IO;
using System.Collections.Generic;

namespace ProjectOrangeSunshine.Shared
{
    public static class Logger
    {

        private static readonly string logPath = @"./log.txt";

        private static readonly List<string> lines = new();

        public static void WriteLog()
        {
            using StreamWriter writer = File.AppendText(logPath);
            lines.ForEach((line) => writer.WriteLine(line));
        }

        public static void LogClientError(int errorCode, string message)
        {
            string error = "[client] ERROR " + errorCode.ToString() + ": " + message;
            lines.Add(error);
        }

        public static void LogClientError(string message)
        {
            string error = "[client] ERROR: " + message;
            lines.Add(error);
        }

        public static void LogServerError(int errorCode, string message)
        {
            string error = "[server] ERROR " + errorCode.ToString() + ": " + message;
            lines.Add(error);
        }

        public static void LogServerError(string message)
        {
            string error = "[server] ERROR: " + message;
            lines.Add(error);
        }

        public static void LogClientWarning(string message)
        {
            string warning = "[client] WARNING: " + message;
            lines.Add(warning);
        }

        public static void LogServerWarning(string message)
        {
            string warning = "[server] WARNING: " + message;
            lines.Add(warning);
        }

        public static void LogClientMessage(string message)
        {
            message = "[client] MESSAGE: " + message;
            lines.Add(message);
        }

        public static void LogServerMessage(string message)
        {
            message = "[server] MESSAGE: " + message;
            lines.Add(message);
        }
    }
}
