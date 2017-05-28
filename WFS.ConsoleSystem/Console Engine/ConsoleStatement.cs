using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhileFalseStudios.Console
{
    /// <summary>
    /// Returned result of the statement tokeniser.
    /// </summary>
    public struct ConsoleStatement
    {
        /// <summary>
        /// The name of the statement.
        /// </summary>
        public string statementName;

        /// <summary>
        /// The number of arguments in the statement.
        /// </summary>
        public int argumentCount;

        /// <summary>
        /// The actual arguments in the statement.
        /// </summary>
        public string[] arguments;
    }
}
