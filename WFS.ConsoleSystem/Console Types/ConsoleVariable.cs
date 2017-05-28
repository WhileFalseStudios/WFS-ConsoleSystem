using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhileFalseStudios.Console
{
    /// <summary>
    /// A variable stored in the console system.
    /// </summary>
    /// <typeparam name="T">The numeric type stored by the variable.</typeparam>
    public class ConsoleVariable<T> : ConsoleBase where T : IConvertible
    {
        /// <summary>
        /// Constructs a new console variable, with an initial <typeparamref name="T"/> value.
        /// </summary>
        /// <param name="n">The name of the variable.</param>
        /// <param name="v">The value of the variable.</param>
        /// <param name="h">The help text for the variable.</param>
        /// <param name="f">The flags for the variable.</param>
        public ConsoleVariable(string n, T v, string h = "", ConsoleFlag f = 0) : base(n, v.ToString(), h, f)
        {
            cachedValue = v;
        }

        /// <summary>
        /// Gets/sets the value of this console variable (depending on argument count).
        /// </summary>
        /// <param name="args">The arguments parsed by the console.</param>
        public override void Invoke(params string[] args)
        {
            if (args.Length == 0)
            {
                ConsoleSystem.Log("{0}: {1}", LogVerbosity.Normal, name, value.ToString());
            }
            else
            {
                if (HasFlags(ConsoleFlag.DebugOnly))
                {
                    //Return if debug disabled.
                    if (!ConsoleSystem.debugMode)
                        return;
                }

                if (HasFlags(ConsoleFlag.Cheat))
                {
                    //Return if cheats disabled.
                    if (!ConsoleSystem.cheatsMode)
                        return;
                }

                try
                {
                    value = ParseStringValue<T>(args[0]);
                }
                catch
                {
                    ConsoleSystem.Log("Invalid value for {0}: {1} (must be {2})", LogVerbosity.Warning, name, args[0], typeof(T).Name);
                    return;
                }
            }
        }

        protected T cachedValue;

        /// <summary>
        /// The value of this variable.
        /// </summary>
        new public virtual T value
        {
            get
            {
                return cachedValue;
            }

            set
            {
                base.value = value.ToString();
                cachedValue = value;
            }
        }

        public override string GetTypeString()
        {
            return typeof(T).Name;
        }

        public override string GetConfigString()
        {
            return string.Format("{0} {1} \"{2}\"", name, ConsoleSystem.VarSetChar, value.ToString());
        }
    }

    public interface IEnumVariable
    {
        string[] GetEnumValues();
    }

    /// <summary>
    /// Enum-specific implementation of the <see cref="ConsoleVariable{T}"/> class.
    /// </summary>
    /// <typeparam name="T">The enum type. An exception will be thrown if the type is not an enum.</typeparam>
    public class ConsoleEnumVariable<T> : ConsoleVariable<T>, IEnumVariable where T : IComparable, IFormattable, IConvertible
    {
        public ConsoleEnumVariable(string n, T v, string h, ConsoleFlag f) : base(n, v, h, f)
        {
            if (!typeof(T).IsEnum)
                throw new NotSupportedException("Type for ConsoleEnumVariable must be an enum.");
        }

        /// <summary>
        /// Parses the string as the enum.
        /// </summary>
        /// <typeparam name="T2">Type hack because of an virtual function. T2 should be <typeparamref name="T"/></typeparam>
        /// <param name="v">The enum string to parse.</param>
        /// <returns>The enum type.</returns>
        /// <exception cref="InvalidCastException">Throws when the enum parse fails.</exception>
        public override T2 ParseStringValue<T2>(string v)
        {
            //TODO: can we do the enum check here instead?
            try
            {
                return (T2)Enum.Parse(typeof(T2), v);               
            }
            catch
            {
                throw new InvalidCastException("Enum parse failed.");
            }
        }

        public override T value
        {
            get
            {
                return cachedValue;
            }
            set
            {
                var max = Enum.GetValues(typeof(T)).Cast<T>().Max();
                var min = Enum.GetValues(typeof(T)).Cast<T>().Min();
                if (value.CompareTo(max) > 0)
                    cachedValue = max;
                else if (value.CompareTo(min) < 0)
                    cachedValue = min;
                else
                    cachedValue = value;
            }
        }

        public override string GetConfigString()
        {
            return string.Format("{0} {1} \"{2}\"", name, ConsoleSystem.VarSetChar, Enum.GetName(typeof(T), value));
        }

        public string[] GetEnumValues()
        {
            return Enum.GetNames(typeof(T));
        }
    }

    /// <summary>
    /// A variable stored in the console system, with a callback when its value changes.
    /// </summary>
    /// <typeparam name="T">The numeric type stored by the variable.</typeparam>
    public class ConsoleCallbackVariable<T> : ConsoleVariable<T> where T : IConvertible
    {
        Action func;

        public ConsoleCallbackVariable(string n, T v, Action a, string h = "", ConsoleFlag f = 0) : base(n, v, h, f)
        {
            func = a;
        }

        public override void Invoke(params string[] args)
        {
            base.Invoke(args);
            if (args.Length > 0)
            {
                func.Invoke();
            }
        }
    }
}
