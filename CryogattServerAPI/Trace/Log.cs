using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Web.Configuration;

namespace CryogattServerAPI.Trace
{
    public class Log
    {
        private static TraceSource m_traceSource = new TraceSource("Cryogatt");
        public static string C_PROGRAM_DATA_SUB_FOLDER = @"\Cryogatt\CryogattAPI";
        public static string C_DEBUG_LOG_PATH = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + C_PROGRAM_DATA_SUB_FOLDER + @"\Logs\";

        public static void AddListeners()
        {
            // Add all trace source listeners
            m_traceSource.Listeners.Add(new TextWriterTraceListener(C_DEBUG_LOG_PATH + @"error.log", "errorlog"));
            m_traceSource.Listeners["errorlog"].Filter = new System.Diagnostics.EventTypeFilter(SourceLevels.Error);
            m_traceSource.Listeners.Add(new TextWriterTraceListener(C_DEBUG_LOG_PATH + @"debug.log", "debuglog"));
            m_traceSource.Listeners["debuglog"].Filter = new System.Diagnostics.EventTypeFilter(SourceLevels.Verbose);
        }

        public static void RemoveListeners()
        {
            // Remove all trace source listeners
            m_traceSource.Listeners.Remove("errorlog");
            m_traceSource.Listeners.Remove("debuglog");
        }

        public static void Error(string message)
        {
            // Write error trace event to the trace source
            m_traceSource.TraceEvent(TraceEventType.Error, 0, Format(message));
        }

        public static void Debug(string message)
        {
            bool debug = false;
            debug = Convert.ToBoolean(WebConfigurationManager.AppSettings["DebugLoggingEnabled"]);
            if (debug)
            {
                // Write debug trace event to the trace source
                m_traceSource.TraceEvent(TraceEventType.Verbose, 0, Format(message));
            }
        }

        // Formats the message with "timecode: class.methodname: " + message
        private static string Format(string message)
        {
            string output = "";
            try
            {
                // Obtain stack frame for 2 calls up, i.e. the source of the Log.x call
                StackTrace stackTrace = new StackTrace();
                StackFrame[] stackFrames = stackTrace.GetFrames();
                StackFrame stackFrame = stackFrames[2];

                // Form the output string
                output = DateTime.UtcNow.ToString("yyyy/MM/dd HH:mm:ss.fff") + ": ";
                output += stackFrame.GetMethod().DeclaringType.Name + ": " + stackFrame.GetMethod().ToString() + ": ";
                output += message;
            }
            catch (Exception)
            {
                output = message;
            }
            return output;
        }
    }
}
