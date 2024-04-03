using System;
using System.Diagnostics;
using System.Threading;

namespace Cryogatt.RFID.Trace
{
    /// <summary>
    ///     Static Log class for event logging supporting ERROR, INFORMATION and DEBUG.
    /// </summary>
    public static class Log
    {
        #region Static Constructor

        /// <summary>
        ///     Static constructor invoked before Log is accessed, attaches the event listeners.
        /// </summary>
        static Log()
        {
            // Add all trace source listeners
            TraceListener errorListener = new TextWriterTraceListener(CLogsPath + @"error.log", "errorlog")
            {
                Filter = new EventTypeFilter(SourceLevels.Information)
            };
            CTraceSource.Listeners.Add(errorListener);
            TraceListener debugListener = new TextWriterTraceListener(CLogsPath + @"debug.log", "debuglog")
            {
                Filter = new EventTypeFilter(SourceLevels.Verbose)
            };
            CTraceSource.Listeners.Add(debugListener);

            // Debug is enabled
            IsDebugEnabled = true;
        }

        #endregion Static Constructor

        #region Public Properties

        /// <summary>
        ///     Returns true if debug is enabled, false otherwise TODO: read from App/Web config
        /// </summary>
        public static bool IsDebugEnabled { get; set; }

        #endregion Public Properties

        #region Private Methods

        /// <summary>
        ///     Formats the output message with the time and location of the log invocation.
        /// </summary>
        /// <param name="message">The message passed in by the log invoker</param>
        /// <returns></returns>
        private static string Format(string message)
        {
            string output;
            try
            {
                // Form the output string
                output = DateTime.UtcNow.ToString("yyyy/MM/dd HH:mm:ss.fff") + " : ";
                output += Thread.CurrentThread.ManagedThreadId + " : ";

                // Obtain stack frame for 2 calls up, i.e. the source of the Log.x call
                var stackTrace = new StackTrace();
                if (stackTrace.FrameCount > 2)
                {
                    var stackFrame = stackTrace.GetFrame(2);

                    output += stackFrame.GetMethod().DeclaringType.Name + " : ";
                    output += stackFrame.GetMethod() + " : ";
                }

                // Append log messge to output
                output += message;
            }
            catch (Exception)
            {
                output = message;
            }

            return output;
        }

        #endregion Private Methods

        #region Private Data

        /// <summary>
        ///     The TraceSource whose name must match that in the overall application App.config file.
        /// </summary>
        private static readonly TraceSource CTraceSource = new TraceSource("Cryogatt.RFID.Trace");

        /// <summary>
        ///     The sub-folder within common application data (typically C:\ProgramData) where the log files will be written.
        /// </summary>
        private const string CProgramDataSubFolder = @"\Cryogatt\RFID";

        /// <summary>
        ///     The full path to the log folder.
        /// </summary>
        private static readonly string CLogsPath =
            Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + CProgramDataSubFolder +
            @"\Logs\";

        #endregion Private Data

        #region Public Methods

        /// <summary>
        ///     Output a debug message, providing debug is enabled, to the debug log.
        /// </summary>
        /// <param name="message">The debug message</param>
        public static void Debug(string message)
        {
            if (IsDebugEnabled) CTraceSource.TraceEvent(TraceEventType.Verbose, 0, Format(message));
        }

        /// <summary>
        ///     Output an information message to the event and debug logs.
        /// </summary>
        /// <param name="message">The informational message.</param>
        public static void Information(string message)
        {
            // Write information trace event to the trace source
            CTraceSource.TraceEvent(TraceEventType.Information, 0, Format(message));
        }

        /// <summary>
        ///     Output an error message to the event and debug logs.
        /// </summary>
        /// <param name="message">The error message</param>
        public static void Error(string message)
        {
            // Write error trace event to the trace source
            CTraceSource.TraceEvent(TraceEventType.Error, 0, Format(message));
        }

        #endregion Public Methods
    }
}