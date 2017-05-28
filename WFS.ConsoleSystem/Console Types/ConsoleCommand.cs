using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhileFalseStudios.Console
{
    /// <summary>
    /// Interface for the console commands.
    /// </summary>
    public interface IConsoleCommand { }

    /// <summary>
    /// No parameter console command.
    /// </summary>
    public class ConsoleCommand : ConsoleBase, IConsoleCommand
    {
        Action func;

        /// <summary>
        /// Constructs a new no-parameter console command.
        /// </summary>
        /// <param name="n">The name of the command.</param>
        /// <param name="value">The method bound to this command.</param>
        /// <param name="h">The help text for the command.</param>
        /// <param name="f">The flags for the command.</param>
        public ConsoleCommand(string n, Action value, string h = "", ConsoleFlag f = 0) : base(n, value.Method.Name, h, f)
        {
            func = value;
        }

        /// <summary>
        /// 0 parameters.
        /// </summary>
        public override int parameterCount { get { return 0; } }

        /// <summary>
        /// Call the method bound to this console command.
        /// </summary>
        /// <param name="args">The string arguments parsed by the console.</param>
        public override void Invoke(params string[] args)
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

            func.Invoke();
        }
    }

    /// <summary>
    /// 1 parameter console command.
    /// </summary>
    /// <typeparam name="T">The parameter type.</typeparam>
    public class ConsoleCommand<T> : ConsoleBase, IConsoleCommand where T : IConvertible
    {
        Action<T> func;

        /// <summary>
        /// Instantiates a one parameter console command.
        /// </summary>
        /// <param name="n">The name of the command.</param>
        /// <param name="value">The callback for the command.</param>
        /// <param name="h">The help text for the command.</param>
        /// <param name="f">Flags for the command.</param>
        public ConsoleCommand(string n, Action<T> value, string h = "", ConsoleFlag f = 0) : base(n, value.Method.Name, h, f)
        {
            func = value;
        }

        /// <summary>
        /// 1 parameter.
        /// </summary>
        public override int parameterCount { get { return 1; } }

        public override void Invoke(params string[] args)
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

            T v1;
            try
            {
                if (args.Length < 1)
                    v1 = default(T);
                else
                    v1 = ParseStringValue<T>(args[0]);
            }
            catch
            {
                ConsoleSystem.Log("Invalid value for parameter 1: {0} (must be {1})", LogVerbosity.Warning, args[0], typeof(T).Name);
                return;
            }

            func.Invoke(v1);
        }

        public override string GetTypeString()
        {
            return typeof(T).Name;
        }
    }

    /// <summary>
    /// 2 parameter console command.
    /// </summary>
    /// <typeparam name="T">The 1st parameter type.</typeparam>
    /// <typeparam name="T2">The 2nd parameter type.</typeparam>
    public class ConsoleCommand<T, T2> : ConsoleBase, IConsoleCommand where T : IConvertible where T2 : IConvertible
    {
        Action<T, T2> func;

        /// <summary>
        /// Instanitates a new two paremeter console command.
        /// </summary>
        public ConsoleCommand(string n, Action<T, T2> value, string h = "", ConsoleFlag f = 0) : base(n, value.Method.Name, h, f)
        {
            func = value;
        }

        /// <summary>
        /// 2 parameters.
        /// </summary>
        public override int parameterCount { get { return 2; } }

        public override void Invoke(params string[] args)
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

            T v1;
            T2 v2;
            try
            {
                if (args.Length < 1)
                    v1 = default(T);
                else
                    v1 = ParseStringValue<T>(args[0]);
            }
            catch
            {
                ConsoleSystem.Log("Invalid value for parameter 1: {0} (must be {1})", LogVerbosity.Warning, args[0], typeof(T).Name);
                return;
            }

            try
            {
                if (args.Length < 2)
                    v2 = default(T2);
                else
                    v2 = ParseStringValue<T2>(args[1]);
            }
            catch
            {
                ConsoleSystem.Log("Invalid value for parameter 2: {0} (must be {1})", LogVerbosity.Warning, args[1], typeof(T2).Name);
                return;
            }

            func.Invoke(v1, v2);
        }

        public override string GetTypeString()
        {
            return string.Format("{0}, {1}", typeof(T).Name, typeof(T2).Name);
        }
    }

    /// <summary>
    /// 3 parameter console command.
    /// </summary>
    /// <typeparam name="T">1st parameter type.</typeparam>
    /// <typeparam name="T2">2nd parameter type.</typeparam>
    /// <typeparam name="T3">3rd parameter type.</typeparam>
    public class ConsoleCommand<T, T2, T3> : ConsoleBase, IConsoleCommand where T : IConvertible where T2 : IConvertible where T3 : IConvertible
    {
        Action<T, T2, T3> func;

        public ConsoleCommand(string n, Action<T, T2, T3> value, string h = "", ConsoleFlag f = 0) : base(n, value.Method.Name, h, f)
        {
            func = value;
        }

        /// <summary>
        /// 3 parameters.
        /// </summary>
        public override int parameterCount { get { return 3; } }

        public override void Invoke(params string[] args)
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

            T v1;
            T2 v2;
            T3 v3;
            try
            {
                if (args.Length < 1)
                    v1 = default(T);
                else
                    v1 = ParseStringValue<T>(args[0]);
            }
            catch
            {
                ConsoleSystem.Log("Invalid value for parameter 1: {0} (must be {1})", LogVerbosity.Warning, args[0], typeof(T));
                return;
            }

            try
            {
                if (args.Length < 2)
                    v2 = default(T2);
                else
                    v2 = ParseStringValue<T2>(args[1]);
            }
            catch
            {
                ConsoleSystem.Log("Invalid value for parameter 2: {0} (must be {1})", LogVerbosity.Warning, args[1], typeof(T2));
                return;
            }

            try
            {
                if (args.Length < 3)
                    v3 = default(T3);
                else
                    v3 = ParseStringValue<T3>(args[2]);
            }
            catch
            {
                ConsoleSystem.Log("Invalid value for parameter 3: {0} (must be {1})", LogVerbosity.Warning, args[1], typeof(T3));
                return;
            }

            func.Invoke(v1, v2, v3);
        }

        public override string GetTypeString()
        {
            return string.Format("{0}, {1}, {2}", typeof(T).Name, typeof(T2).Name, typeof(T3).Name);
        }
    }

    /// <summary>
    /// Console command accepting an unlimited number of string arguments.
    /// </summary>
    public class ConsoleCommandGeneric : ConsoleBase, IConsoleCommand
    {
        Action<string[]> func;

        /// <summary>
        /// Instantiates a new generic command.
        /// </summary>
        /// <param name="n">The name of the command.</param>
        /// <param name="v">The callback for the command.</param>
        /// <param name="h">The help text for the command.</param>
        /// <param name="f">The flags for the command.</param>
        public ConsoleCommandGeneric(string n, Action<string[]> v, string h = "", ConsoleFlag f = 0) : base(n, v.Method.Name, h, f)
        {
            func = v;
        }

        /// <summary>
        /// Invokes the callback function.
        /// </summary>
        /// <param name="args"></param>
        public override void Invoke(params string[] args)
        {
            func.Invoke(args);
        }

        public override string GetTypeString()
        {
            return typeof(string[]).Name;
        }
    }
}
