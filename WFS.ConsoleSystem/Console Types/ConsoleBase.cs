using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhileFalseStudios.Console
{
    /// <summary>
    /// Flags used by the console system for controlling functionality.
    /// </summary>
    [Flags]
    public enum ConsoleFlag
    {
        /// <summary>
        /// The console object can only be used when cheats are enabled.
        /// </summary>
        Cheat = 1 << 0,
        /// <summary>
        /// [<see cref="ConsoleVariable{T}"/> only] The value of this console object will be written to the config file.
        /// </summary>
        Archive = 1 << 1,
        /// <summary>
        /// The console object can only be used in developer mode.
        /// </summary>
        DebugOnly = 1 << 2,

        /// <summary>
        /// Allow this console object to be deregistered at runtime.
        /// </summary>
        AllowDeregister = 1 << 3,
    }

    /// <summary>
    /// Base type for all console objects.
    /// </summary>
    public class ConsoleBase
    {
        /// <summary>
        /// The name of this console object. This is what is entered into the console to call it, written to config file etc.
        /// </summary>
        public string name { get; }

        /// <summary>
        /// The help string for this console object. This is displayed to the user with help commands.
        /// </summary>
        public string helpText { get; }

        /// <summary>
        /// The internal value of this object. This is unlikely to be used externally.
        /// </summary>
        public string value { get; set; }

        /// <summary>
        /// The <see cref="ConsoleFlag"/> bitmask possessed by this console object.
        /// </summary>
        public ConsoleFlag flags { get; }

        /// <summary>
        /// Constructs a new console object. This should never be called directly (subclasses call this in their constructors).
        /// </summary>
        /// <param name="n">The name of the object.</param>
        /// <param name="v">The value of the object.</param>
        /// <param name="h">The help text for the object.</param>
        /// <param name="f">The flags for the object.</param>
        protected ConsoleBase(string n, string v, string h = "", ConsoleFlag f = 0)
        {
            name = n.ToLower();
            value = v;
            helpText = h;
            flags = f;

            ConsoleSystem.RegisterConsoleObject(this);
        }

        /// <summary>
        /// Call the method bound to this console object.
        /// </summary>
        /// <param name="args">The arguments parsed by the console.</param>
        public virtual void Invoke(params string[] args) { }

        /// <summary>
        /// Checks whether the object has the specified flags in its bitmask.
        /// </summary>
        /// <param name="fCheck">The flag bitmask to check against.</param>
        /// <returns></returns>
        public bool HasFlags(ConsoleFlag fCheck)
        {
            return (flags & fCheck) == fCheck;
        }

        /// <summary>
        /// Converts the string to the specified numeric type.
        /// </summary>
        /// <typeparam name="T">The numeric type to parse.</typeparam>
        /// <param name="v">The string value to parse.</param>
        /// <returns>The parsed object.</returns>
        /// <exception cref="InvalidCastException">Thrown when cast does not succeed.</exception>
        public virtual T ParseStringValue<T>(string v) where T : IConvertible
        {
            try
            {
                return (T)Convert.ChangeType(v, typeof(T));
            }
            catch
            {
                throw new InvalidCastException("Conversion failed.");
            }
        }

        /// <summary>
        /// The number of perameters on this ConsoleCommand.
        /// </summary>
        public virtual int parameterCount { get; }

        /// <summary>
        /// The parameter types used in this object.
        /// </summary>
        /// <returns></returns>
        public virtual string GetTypeString()
        {
            return "None";
        }

        /// <summary>
        /// The written config string for the archived objects.
        /// </summary>
        /// <returns></returns>
        public virtual string GetConfigString()
        {
            return string.Empty;
        }
    }
}
