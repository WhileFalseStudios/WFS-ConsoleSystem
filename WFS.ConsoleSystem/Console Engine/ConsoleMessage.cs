using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhileFalseStudios.Console
{
    /// <summary>
    /// Message container.
    /// </summary>
    public struct ConsoleMessage
    {
        /// <summary>
        /// The message.
        /// </summary>
        public string message;
        /// <summary>
        /// Verbosity of the message. Just describes if the message is an error, warning etc. It is up to you to handle this if you wish.
        /// </summary>
        public LogVerbosity verbosity;
    }

    public enum LogVerbosity
    {
        Normal,
        Warning,
        Error
    }
}
