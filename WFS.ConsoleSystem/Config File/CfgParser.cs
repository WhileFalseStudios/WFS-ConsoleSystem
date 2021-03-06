﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace WhileFalseStudios.Console
{
    /// <summary>
    /// Parser for config files.
    /// </summary>
    public static class ConfigParser
    {
        /// <summary>
        /// How verbose the written config files are.
        /// </summary>
        public enum OutputVerbosity
        {
            /// <summary>
            /// Full line breaks and comments.
            /// </summary>
            Pretty,
            /// <summary>
            /// Minimal line breaks and comments.
            /// </summary>
            Condensed,

            /// <summary>
            /// Minimal line breaks and no comments.
            /// </summary>
            Undocumented,
        }

        /// <summary>
        /// The file type used for config files. Defaults to ".cfg"
        /// </summary>
        public static string ConfigExtension = ".cfg";

        /// <summary>
        /// The style to use for comments in config files. Defaults to "//"
        /// </summary>
        public static string ConfigCommentStyle = "//";

        /// <summary>
        /// List of directories the console will search for config files.
        /// </summary>
        public static List<string> configFilePaths { get; }

        /// <summary>
        /// The file to automatically load config data from.
        /// </summary>
        public static string primaryConfigFile = "system.cfg";

        /// <summary>
        /// User's config file.
        /// </summary>
        public static string userConfigFile = "autoexec.cfg";

        static ConfigParser()
        {
            configFilePaths = new List<string>();
        }

        /// <summary>
        /// Loads and executes the specified cfg file if it exists.
        /// </summary>
        /// <param name="file"></param>
        public static void ExecuteConfigFile(string file)
        {
            if (Path.GetExtension(file) != ConfigExtension)
                return;

            ParseConfig(LoadConfig(file));
        }

        /// <summary>
        /// Parse a set of lines from a config file.
        /// </summary>
        /// <param name="lines">The lines from the config to parse.</param>
        public static void ParseConfig(IEnumerable<string> lines)
        {
            foreach (string line in lines)
            {
                string lineTrimmed = line.Trim();
                if (lineTrimmed.IsNullOrWhiteSpace() || lineTrimmed.StartsWith(ConfigCommentStyle))
                    continue;

                //TODO: line execution.
                ConsoleSystem.Evaluate(lineTrimmed);
            }
        }

        /// <summary>
        /// Write a config file to the disk.
        /// </summary>
        /// <param name="items">Objects to write.</param>
        /// <param name="file">The file to write to.</param>
        /// <param name="ml">Sets the output verbosity.</param>
        public static void WriteConfig(string file, List<ConsoleBase> items, OutputVerbosity ml = OutputVerbosity.Condensed)
        {
            if (configFilePaths.Count > 0)
            {
                List<string> commands = new List<string>()
                {
                    string.Format("{0} This file is automatically generated. Do not edit as changes will be lost on next run.", ConfigCommentStyle),                    
                };
                
                switch (ml)
                {
                    case OutputVerbosity.Pretty:
                        items.ForEach(x => commands.Add(string.Format("{0} {1}: {2}\n{3}\n", ConfigCommentStyle, x.name, x.helpText, x.GetConfigString())));
                        break;
                    case OutputVerbosity.Condensed:
                        items.ForEach(x => commands.Add(string.Format("{0}{1}\n{2}", ConfigCommentStyle, x.helpText, x.GetConfigString())));
                        break;
                    case OutputVerbosity.Undocumented:
                        items.ForEach(x => commands.Add(x.GetConfigString()));
                        break;
                }

                foreach (var path in configFilePaths)
                {
                    try
                    {
                        Directory.CreateDirectory(path);
                        File.WriteAllLines(Path.Combine(path, file), commands.ToArray());
                        break;
                    }
                    catch
                    {
                        continue;
                    }
                }                
            }        
        }

        /// <summary>
        /// Loads the lines in a config file.
        /// </summary>
        /// <param name="file">The name of the file to search for.</param>
        /// <returns>The lines in an array, or an array containing <see cref="string.Empty"/> if no file can be read.</returns>
        public static string[] LoadConfig(string file)
        {
            foreach (var path in configFilePaths)
            {
                string cfgPath = Path.Combine(path, file);
                try
                {
                    if (File.Exists(cfgPath))
                    {
                        return File.ReadAllLines(cfgPath);
                    }
                    else
                        continue;
                }
                catch
                {
                    continue;
                }
            }

            return new string[] { string.Empty };
        }

        static bool IsNullOrWhiteSpace(this string value)
        {
            return string.IsNullOrEmpty(value) || value.Trim().Length == 0;
        }
    }
}
