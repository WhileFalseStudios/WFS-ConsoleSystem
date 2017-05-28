using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.IO;
using WhileFalseStudios.Console;

namespace ConsoleTestApp
{
    class Program
    {
        static ConsoleCommand quitCmd = new ConsoleCommand("quit", C_Quit, "Quits the program");
        static ConsoleCommand onlyInCheatsCmd = new ConsoleCommand("cheattest", C_Cheats, "Checks whether cheats works.", ConsoleFlag.Cheat);
        static ConsoleCallbackVariable<bool> cheats = new ConsoleCallbackVariable<bool>("cheats", false, () => { ConsoleSystem.cheatsMode = cheats.value; }, "enables cheats.");
        static ConsoleCallbackVariable<bool> debug = new ConsoleCallbackVariable<bool>("debug", false, () => { ConsoleSystem.debugMode = debug.value; }, "enables debug mode.");

        static ConsoleCommand<string, int, bool> aBigCommand3 = new ConsoleCommand<string, int, bool>("dev_bigcommandtest3", C_BigTest3, "Tests multi parameter commands.");
        static ConsoleCommand<float, int> aBigCommand2 = new ConsoleCommand<float, int>("dev_bigcommandtest2", C_BigTest2, "Tests multi parameter commands.");
        static ConsoleCommandGeneric genericCommand = new ConsoleCommandGeneric("dev_genericcommand", C_Generic, "Tests generic command");

        static ConsoleVariable<string> dev_stringVar = new ConsoleVariable<string>("dev_stringvar", "Test", "Test variable for strings.", ConsoleFlag.Archive);
        static ConsoleVariable<int> dev_intVar = new ConsoleVariable<int>("dev_intvar", 10, "Test variable for ints.", ConsoleFlag.Archive);
        static ConsoleVariable<float> dev_floatVar = new ConsoleVariable<float>("dev_floatvar", 10.0f, "Test variable for floats.", ConsoleFlag.Archive);
        static ConsoleVariable<bool> dev_boolVar = new ConsoleVariable<bool>("dev_boolvar", true, "Test variable for booleans.", ConsoleFlag.Archive);
        static ConsoleEnumVariable<ConsoleColor> dev_enumVar = new ConsoleEnumVariable<ConsoleColor>("dev_enumvar", ConsoleColor.Black, "Test variable for enums.", ConsoleFlag.Archive);

        static bool wantsToQuit = false;

        static void Main(string[] args)
        {
            Console.Title = "Console Test App";
            ConfigParser.configFilePaths.Add(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location));
            ConsoleSystem.Initialize();
            LogText("Type \"list\" for a list of commands.", ConsoleColor.Cyan);
            while (!wantsToQuit)
            {
                var lines = ConsoleSystem.GetAllLoggedText();
                foreach (var line in lines)
                {
                    switch (line.verbosity)
                    {
                        case LogVerbosity.Normal:
                            LogText(line.message);
                            break;
                        case LogVerbosity.Warning:
                            LogText(line.message, ConsoleColor.Yellow);
                            break;
                        case LogVerbosity.Error:
                            LogText(line.message, ConsoleColor.Red);
                            break;
                    }
                }

                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("> ");
                string input = Console.ReadLine();
                Console.ForegroundColor = ConsoleColor.Gray;
                ConsoleSystem.Evaluate(input);
            }
        }

        static void LogText(string text, ConsoleColor c = ConsoleColor.White)
        {
            Console.ForegroundColor = c;
            Console.WriteLine(text);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        static void C_Quit()
        {
            wantsToQuit = true;
            ConsoleSystem.SaveArchiveVariables(ConfigParser.OutputVerbosity.Pretty);
        }

        static void C_Cheats()
        {
            ConsoleSystem.Log("Cheats is on.");
        }

        static void C_BigTest3(string s, int i, bool b)
        {
            if (s == null)
            {
                ConsoleSystem.Log("String argument required.", LogVerbosity.Warning);
                return;
            }

            ConsoleSystem.Log("A string: {0}, An int: {1}, A bool: {2}", LogVerbosity.Normal, s, i, b);
        }

        static void C_BigTest2(float f, int i)
        {
            ConsoleSystem.Log("A float: {0}, An int: {1}",LogVerbosity.Normal , f, i);
        }

        static void C_Generic(params string[] args)
        {
            string accum = "You typed ";

            if (args.Length > 0)
            {
                foreach (string s in args)
                {
                    accum += s + " ";
                }
            }
            else
            {
                accum += "nothing.";
            }

            accum = accum.Trim();
            ConsoleSystem.Log(accum);
        }
    }
}
