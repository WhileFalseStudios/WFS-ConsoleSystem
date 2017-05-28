using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhileFalseStudios.Console
{
    /// <summary>
    /// The actual console that stores info. Only 1 of these may exist as it is static.
    /// </summary>
    public static class ConsoleSystem
    {
        static Dictionary<string, ConsoleBase> objects = new Dictionary<string, ConsoleBase>();
        static List<ConsoleMessage> outputStack = new List<ConsoleMessage>();

        /// <summary>
        /// The character used for quotes in the console.
        /// </summary>
        public const char QuoteChar = '"';

        /// <summary>
        /// The character used for argument separation in the console.
        /// </summary>
        public const char ArgSeperatorChar = ',';

        /// <summary>
        /// The character used for setting variables in the console.
        /// </summary>
        public const char VarSetChar = '=';

        /// <summary>
        /// The character used for running commands in the console.
        /// </summary>
        public const char CommandChar = ':';

        /// <summary>
        /// Should the console run debug mode commands and variables?
        /// </summary>
        public static bool debugMode { get; set; }

        /// <summary>
        /// Should the console run cheats mode commands and variables?
        /// </summary>
        public static bool cheatsMode { get; set; }

        static ConsoleCommand<string> helpCommand = new ConsoleCommand<string>("help", C_Help, "Displays the help text for the specified console object.");
        static ConsoleCommand<string> findCommand = new ConsoleCommand<string>("find", C_Find, "Searches for console objects with the specified search string in their names.");
        static ConsoleCommand<string> searchCommand = new ConsoleCommand<string>("search", C_Find, "Alias for find.");
        static ConsoleCommand<string> echoCommand = new ConsoleCommand<string>("echo", C_Echo, "Prints the specified text to the console.");
        static ConsoleCommand<string> printCommand = new ConsoleCommand<string>("print", C_Echo, "Alias for echo.");
        static ConsoleCommand<string> execCommand = new ConsoleCommand<string>("exec", C_Exec, "Executes the specified config file.");
        static ConsoleCommand<string> getEnums = new ConsoleCommand<string>("list_values", C_GetEnums, "Lists all possible values for an enum variable.");
        static ConsoleCommand listObjects = new ConsoleCommand("list", C_List, "Lists all console objects registered.");

        internal static void RegisterConsoleObject(ConsoleBase o)
        {
            if (!objects.ContainsKey(o.name))
            {
                objects.Add(o.name, o);
            }
            else
            {
                //TODO: Throw/log an error
                Log("Console object named {0} already defined.", LogVerbosity.Error, o.name);
            }
        }

        /// <summary>
        /// Initializes the console system.
        /// </summary>
        public static void Initialize()
        {
            ConfigParser.ExecuteConfigFile(ConfigParser.primaryConfigFile);
            ConfigParser.ExecuteConfigFile(ConfigParser.userConfigFile);
        }

        /// <summary>
        /// Saves any variables flagged as archivable.
        /// </summary>
        public static void SaveArchiveVariables(ConfigParser.OutputVerbosity ml)
        {
            List<ConsoleBase> flaggedObjects = new List<ConsoleBase>();
            foreach (var o in objects.Values)
            {
                if (o.HasFlags(ConsoleFlag.Archive))
                {
                    if ((o.HasFlags(ConsoleFlag.DebugOnly) && !debugMode))
                        continue;

                    flaggedObjects.Add(o);
                }
            }

            ConfigParser.WriteConfig(ConfigParser.primaryConfigFile, flaggedObjects, ml);
        }

        /// <summary>
        /// Remove a console variable from the console system, if it is flagged as <see cref="ConsoleFlag.AllowDeregister"/>
        /// </summary>
        /// <param name="o"></param>
        public static void DeregisterConsoleObject(ConsoleBase o)
        {
            if (o.HasFlags(ConsoleFlag.AllowDeregister))
            {
                objects.Remove(o.name);
            }
        }

        /// <summary>
        /// Gets the <typeparamref name="T"/> with the specified name. If it is not found or not of the specified type, null is returned.
        /// </summary>
        /// <typeparam name="T">The console object type to find.</typeparam>
        /// <param name="name">The console object to find.</param>
        /// <returns>The <typeparamref name="T"/> if found, otherwise null.</returns>
        public static T GetConsoleObject<T>(string name) where T : ConsoleBase
        {
            if (objects.ContainsKey(name))
            {
                if (objects[name] is T)
                {
                    return (T)objects[name];
                }
                else
                {
                    return null;
                }
            }
            else return null;
        }

        /// <summary>
        /// Gets the autocomplete options for the specified text.
        /// </summary>
        /// <param name="search">The string to search for.</param>
        /// <returns>An array of all the results.</returns>
        public static string[] GetAutocompleteText(string search)
        {
            var items = objects.Keys.Where(x =>
            {
                bool b1 = x.Contains(search);
                if (debugMode)
                {
                    return b1 && objects[x].HasFlags(ConsoleFlag.DebugOnly);
                }
                else return b1;
            });

            return items.ToArray();
        }

        /// <summary>
        /// Run a single-line console query.
        /// </summary>
        /// <param name="command">The query to run. This is where user input should be passed to the console.</param>
        public static void Evaluate(string command)
        {
            //Log(command); //Echo it back.
            var statement = TokenizeStatement(command);
            var o = GetConsoleObject<ConsoleBase>(statement.statementName);
            if (o != null && !(o.HasFlags(ConsoleFlag.DebugOnly) && !debugMode))
            {
                o.Invoke(statement.arguments);
            }
            else
            {
                Log("The specified console object \"{0}\" could not be found.", LogVerbosity.Warning, statement.statementName);
            }
        }

        static ConsoleStatement TokenizeStatement(string statement)
        {
            bool isInString = false;
            int setter = 0;
            string command = string.Empty;
            List<string> args = new List<string>();
            string currentArg = string.Empty;

            foreach (char c in statement)
            {
                if (c == QuoteChar) //Quotes, set flag and ignore.
                {
                    isInString = !isInString;
                    continue;
                }

                if (isInString)
                {
                    currentArg += c; //Add it, no matter what it is.
                }
                else
                {
                    if (char.IsWhiteSpace(c))
                        continue;

                    switch (c)
                    {
                        case ArgSeperatorChar:
                            if (setter == 2)
                            {
                                args.Add(currentArg);
                                currentArg = string.Empty;
                            }
                            else
                            {
                                currentArg += c;
                            }
                            break;
                        case CommandChar:
                            if (setter == 0)
                            {
                                command = currentArg;
                                currentArg = string.Empty;
                                setter = 2;
                            }
                            else
                            {
                                currentArg += c;
                            }
                            break;
                        case VarSetChar:
                            if (setter == 0)
                            {
                                command = currentArg;
                                currentArg = string.Empty;
                                setter = 1;
                            }
                            else
                            {
                                currentArg += c;
                            }
                            break;
                        default:
                            currentArg += c;
                            break;
                    }
                }
            }

            if (command != string.Empty)
            {
                args.Add(currentArg);
            }
            else if (currentArg != string.Empty)
            {
                command = currentArg;
            }

            ConsoleStatement s = new ConsoleStatement() { statementName = command.ToLower(), argumentCount = args.Count, arguments = args.ToArray() };
            return s;
        }

        /// <summary>
        /// Log a formatted message to the output stack.
        /// </summary>
        /// <param name="msg">The message format to log.</param>
        /// <param name="args">The arguments to the message.</param>
        public static void Log(string msg, LogVerbosity v = LogVerbosity.Normal, params object[] args)
        {
            string finalText = string.Format(msg, args);
            ConsoleMessage cm = new ConsoleMessage() { message = finalText, verbosity = v };
            outputStack.Add(cm);
        }

        /// <summary>
        /// Gets all the lines logged since the last call to this method. They are popped from the stack in the process.
        /// </summary>
        /// <returns>All the lines since the last method call.</returns>
        public static ConsoleMessage[] GetAllLoggedText()
        {
            var lines = outputStack.ToArray();
            outputStack.Clear();
            return lines;
        }

        static void C_Help(string cmd)
        {
            if (cmd == null)
            {
                Log("Argument required.", LogVerbosity.Warning);
                return;
            }

            if (objects.ContainsKey(cmd))
            {
                var o = objects[cmd];
                if (o.HasFlags(ConsoleFlag.DebugOnly) && !debugMode) //Hide debug commands in normal mode.
                {
                    Log("Console object not found: {0}", LogVerbosity.Warning, cmd);
                }
                else
                {
                    Log("{0}: {1}\nParameters: {2}", LogVerbosity.Normal, o.name, o.helpText, o.GetTypeString());
                }                
            }
            else
            {
                Log("Console object not found: {0}", LogVerbosity.Warning, cmd);
            }
        }

        static void C_Find(string cmd)
        {
            if (cmd == null)
            {
                Log("Argument required.", LogVerbosity.Warning);
                return;
            }

            Log("Found console objects containing {0}:", LogVerbosity.Normal, cmd);
            foreach (var o in objects)
            {
                if (o.Key.Contains(cmd))
                {
                    if (o.Value.HasFlags(ConsoleFlag.DebugOnly) && !debugMode)
                        continue;

                    Log(o.Value.name);
                }
            }
        }

        static void C_Echo(string cmd)
        {
            if (cmd == null)
            {
                Log("Argument required.", LogVerbosity.Warning);
                return;
            }

            Log(cmd);
        }

        static void C_Exec(string cmd)
        {
            if (cmd == null)
            {
                Log("Argument required.", LogVerbosity.Warning);
                return;
            }

            ConfigParser.ExecuteConfigFile(cmd);
        }

        static void C_GetEnums(string cmd)
        {
            if (cmd == null)
            {
                Log("Argument required.", LogVerbosity.Warning);
                return;
            }

            var o = GetConsoleObject<ConsoleBase>(cmd);
            if (o != null)
            {
                if (o is IEnumVariable)
                {
                    var v = o as IEnumVariable;
                    var values = v.GetEnumValues();
                    Log("Enum values for {0}:", LogVerbosity.Normal, o.name);
                    foreach (string val in values)
                    {
                        Log(val);
                    }
                }
                else
                {
                    Log("{0} is not an enum variable.", LogVerbosity.Warning, o.name);
                }
            }
            else
            {
                Log("Object {0} not found.", LogVerbosity.Warning, cmd);
            }
        }

        static void C_List()
        {
            Log("Listing all console objects:");
            int counter = 0;
            foreach (ConsoleBase o in objects.Values)
            {
                if (o.HasFlags(ConsoleFlag.DebugOnly) && !debugMode)
                    continue;

                Log("\n{0}: {1} ({2})\n{3}", LogVerbosity.Normal, o.name, o.GetType().Name.Split('`')[0], o.GetTypeString(), o.helpText); //Cleans up the ugly generic reflected names.
                counter++;
            }

            Log("{0} objects found.", LogVerbosity.Normal, counter);
        }
    }
}
