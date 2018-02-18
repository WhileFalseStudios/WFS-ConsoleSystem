using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhileFalseStudios.Console
{
    /// <summary>
    /// Console command that can bind multiple <see cref="Action"/> instances.
    /// </summary>
    public class ConsoleMulticastCommand : ConsoleBase, IConsoleCommand
    {
        List<Action> actionList = new List<Action>();

        /// <summary>
        /// Constructs a new multicast console command with no parameters.
        /// </summary>
        /// <param name="n">The name of the command.</param>
        /// <param name="h">The help string of the command.</param>
        /// <param name="f"></param>
        public ConsoleMulticastCommand(string n, string h = "", ConsoleFlag f = 0) : base(n, "none (multicast)", h, f)
        {
            
        }

        /// <summary>
        /// Bind an object's method to this command.
        /// </summary>
        /// <param name="func"></param>
        public void Add(Action func)
        {
            if (!actionList.Contains(func))
            {
                actionList.Add(func);
            }
        }

        /// <summary>
        /// Unbind an object's method from this command.
        /// </summary>
        /// <param name="func"></param>
        public void Remove(Action func)
        {
            if (actionList.Contains(func))
            {
                actionList.Remove(func);
            }
        }

        /// <summary>
        /// 0 parameters.
        /// </summary>
        public override int parameterCount { get { return 0; } }

        /// <summary>
        /// Invoke all bound actions.
        /// </summary>
        /// <param name="args"></param>
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

            foreach (var act in actionList)
            {
                act.Invoke();
            }
        }
    }

    /// <summary>
    /// Console command that can bind multiple <see cref="Action"/> instances.
    /// </summary>
    public class ConsoleMulticastCommand<T> : ConsoleBase, IConsoleCommand where T : IConvertible
    {
        List<Action<T>> actionList = new List<Action<T>>();

        /// <summary>
        /// Constructs a new multicast console command with 1 parameter.
        /// </summary>
        /// <param name="n">The name of the command.</param>
        /// <param name="h">The help string of the command.</param>
        /// <param name="f"></param>
        public ConsoleMulticastCommand(string n, string h = "", ConsoleFlag f = 0) : base(n, "none (multicast)", h, f)
        {

        }

        /// <summary>
        /// Bind an object's method to this command.
        /// </summary>
        /// <param name="func"></param>
        public void Add(Action<T> func)
        {
            if (!actionList.Contains(func))
            {
                actionList.Add(func);
            }
        }

        /// <summary>
        /// Unbind an object's method from this command.
        /// </summary>
        /// <param name="func"></param>
        public void Remove(Action<T> func)
        {
            if (actionList.Contains(func))
            {
                actionList.Remove(func);
            }
        }

        /// <summary>
        /// 0 parameters.
        /// </summary>
        public override int parameterCount { get { return 1; } }

        public override string GetTypeString()
        {
            return typeof(T).Name;
        }

        /// <summary>
        /// Invoke all bound actions.
        /// </summary>
        /// <param name="args"></param>
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

            foreach (var act in actionList)
            {
                act.Invoke(v1);
            }
        }
    }

    /// <summary>
    /// Console command that can bind multiple <see cref="Action"/> instances.
    /// </summary>
    public class ConsoleMulticastCommand<T, T2> : ConsoleBase, IConsoleCommand where T : IConvertible where T2 : IConvertible
    {
        List<Action<T, T2>> actionList = new List<Action<T, T2>>();

        /// <summary>
        /// Constructs a new multicast console command with 2 parameters.
        /// </summary>
        /// <param name="n">The name of the command.</param>
        /// <param name="h">The help string of the command.</param>
        /// <param name="f"></param>
        public ConsoleMulticastCommand(string n, string h = "", ConsoleFlag f = 0) : base(n, "none (multicast)", h, f)
        {

        }

        /// <summary>
        /// Bind an object's method to this command.
        /// </summary>
        /// <param name="func"></param>
        public void Add(Action<T, T2> func)
        {
            if (!actionList.Contains(func))
            {
                actionList.Add(func);
            }
        }

        /// <summary>
        /// Unbind an object's method from this command.
        /// </summary>
        /// <param name="func"></param>
        public void Remove(Action<T, T2> func)
        {
            if (actionList.Contains(func))
            {
                actionList.Remove(func);
            }
        }

        /// <summary>
        /// 0 parameters.
        /// </summary>
        public override int parameterCount { get { return 2; } }

        public override string GetTypeString()
        {
            return string.Format("{0}, {1}", typeof(T).Name, typeof(T2).Name);
        }

        /// <summary>
        /// Invoke all bound actions.
        /// </summary>
        /// <param name="args"></param>
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

            T2 v2;
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

            foreach (var act in actionList)
            {
                act.Invoke(v1, v2);
            }
        }
    }

    /// <summary>
    /// Console command that can bind multiple <see cref="Action"/> instances.
    /// </summary>
    public class ConsoleMulticastCommand<T, T2, T3> : ConsoleBase, IConsoleCommand where T : IConvertible where T2 : IConvertible where T3 : IConvertible
    {
        List<Action<T, T2, T3>> actionList = new List<Action<T, T2, T3>>();

        /// <summary>
        /// Constructs a new multicast console command with 3 parameters.
        /// </summary>
        /// <param name="n">The name of the command.</param>
        /// <param name="h">The help string of the command.</param>
        /// <param name="f"></param>
        public ConsoleMulticastCommand(string n, string h = "", ConsoleFlag f = 0) : base(n, "none (multicast)", h, f)
        {

        }

        /// <summary>
        /// Bind an object's method to this command.
        /// </summary>
        /// <param name="func"></param>
        public void Add(Action<T, T2, T3> func)
        {
            if (!actionList.Contains(func))
            {
                actionList.Add(func);
            }
        }

        /// <summary>
        /// Unbind an object's method from this command.
        /// </summary>
        /// <param name="func"></param>
        public void Remove(Action<T, T2, T3> func)
        {
            if (actionList.Contains(func))
            {
                actionList.Remove(func);
            }
        }

        /// <summary>
        /// 0 parameters.
        /// </summary>
        public override int parameterCount { get { return 3; } }

        public override string GetTypeString()
        {
            return string.Format("{0}, {1}, {2}", typeof(T).Name, typeof(T2).Name, typeof(T3).Name);
        }

        /// <summary>
        /// Invoke all bound actions.
        /// </summary>
        /// <param name="args"></param>
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

            T2 v2;
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

            T3 v3;
            try
            {
                if (args.Length < 3)
                    v3 = default(T3);
                else
                    v3 = ParseStringValue<T3>(args[2]);
            }
            catch
            {
                ConsoleSystem.Log("Invalid value for parameter 3: {0} (must be {1})", LogVerbosity.Warning, args[2], typeof(T3).Name);
                return;
            }

            foreach (var act in actionList)
            {
                act.Invoke(v1, v2, v3);
            }
        }
    }

    /// <summary>
    /// Console command that can bind multiple <see cref="Action"/> instances.
    /// </summary>
    public class ConsoleMulticastCommandGeneric : ConsoleBase, IConsoleCommand
    {
        List<Action<string[]>> actionList = new List<Action<string[]>>();

        /// <summary>
        /// Constructs a new multicast console command with generic string parameters.
        /// </summary>
        /// <param name="n">The name of the command.</param>
        /// <param name="h">The help string of the command.</param>
        /// <param name="f"></param>
        public ConsoleMulticastCommandGeneric(string n, string h = "", ConsoleFlag f = 0) : base(n, "none (multicast)", h, f)
        {

        }

        /// <summary>
        /// Bind an object's method to this command.
        /// </summary>
        /// <param name="func"></param>
        public void Add(Action<string[]> func)
        {
            if (!actionList.Contains(func))
            {
                actionList.Add(func);
            }
        }

        /// <summary>
        /// Unbind an object's method from this command.
        /// </summary>
        /// <param name="func"></param>
        public void Remove(Action<string[]> func)
        {
            if (actionList.Contains(func))
            {
                actionList.Remove(func);
            }
        }

        /// <summary>
        /// Invoke all bound actions.
        /// </summary>
        /// <param name="args"></param>
        public override void Invoke(params string[] args)
        {
            foreach (var act in actionList)
            {
                act.Invoke(args);
            }
        }

        public override string GetTypeString()
        {
            return typeof(string[]).Name;
        }
    }
}
