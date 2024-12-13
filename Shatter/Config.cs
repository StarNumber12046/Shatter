using I2.Loc.SimpleJSON;
using System.IO;
using System.Reflection;
using System;
using UnityEngine;

namespace Shatter
{
    public class Config
    {
        public static void Init()
        {
            bool flag = File.Exists(Config.GetConfigPath());
            if (flag)
            {
                Config.ReadConfig();
            }
            if (!flag)
            {
                Config.SaveConfig();
            }
        }

        public static string GetConfigPath()
        {
            return Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), CONFIG_FILE_NAME);
        }

        private static void ReadConfig()
        {
            if (!File.Exists(Config.GetConfigPath()))
            {
                return;
            }
            JSONNode jsonnode;
            try
            {
                jsonnode = JSON.Parse(File.ReadAllText(Config.GetConfigPath()));
            }
            catch (Exception ex)
            {
                MonoBehaviour.print("Couldn't load Shatter config!");
                Debug.LogException(ex);
                return;
            }

            if (jsonnode["forceEndPad"] != null)
            {
                Config.forceEndPad = jsonnode["forceEndPad"].AsBool;
            }

            if (jsonnode["forceReadyToWin"] != null)
            {
                Config.forceReadyToWin = jsonnode["forceReadyToWin"].AsBool;
            }

            if (jsonnode["extraVotes"] != null)
            {
                Config.extraVotes = jsonnode["extraVotes"].AsInt;
            }
        }

        public static void SaveConfig()
        {
            JSONNode jsonnode = new JSONClass
            {
                {
                    "forceEndPad",
                    new JSONData(Config.forceEndPad)
                },
                {
                    "forceReadyToWin",
                    new JSONData(Config.forceReadyToWin)
                },
                {
                    "extraVotes",
                    new JSONData(Config.extraVotes)
                }
            };
            File.WriteAllText(Config.GetConfigPath(), jsonnode.ToString());
        }

        // Constants and Fields
        public const string CONFIG_FILE_NAME = "shatter.json";

        public static bool forceEndPad = false;

        public static bool forceReadyToWin = false;

        public static int extraVotes = 0;
    }
}
