using MIU;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shatter.Commands
{
    internal class ConfigCommands
    {
        [ConsoleCommand("shattercfg", "Configure Shatter", "[string key] [string? value]", false, false)]
        public static string ShatterCfg(params string[] args)
        {
            if (args.Length == 0)
            {
                return "Usage:\n\tshattercfg [key] {value}\n\n\tConfig options:\n\t\treadyToWin : Controls wether the level shall end on MarbleController->IsReadyToWin call. Setting this to true skips gems (which will still be visible).\n\t\tforceEndPad : Controls wether the level shall end on EndPad collision. Only use if readyToWin does not work\n\t\textraVotes : Configures the number of extra votes to broadcast every time a vote is submitted in Multipalyer\n\t\tsave : Saves the config\n\t\tload : Loads the config";
            }
            if (args.Length == 1)
            {
                switch (args[0])
                {
                    case "readyToWin": return $"readyToWin: {Config.forceReadyToWin}";
                    case "forceEndpad": return $"forceEndPad: {Config.forceEndPad}";
                    case "extraVotes": return $"extraVotes: {Config.extraVotes}";

                    case "save": { Config.SaveConfig(); return "Config saved!"; }
                    case "load": { Config.Init(); return "Config loaded!"; }
                    default: return "Invalid config option \"{args[0]}\"";
                }
            }
            switch (args[0])
            {
                case "readyToWin":
                    {
                        if (args[1].ToLower() == "true")
                        {
                            Config.forceReadyToWin = true;
                        }
                        else
                        {
                            Config.forceReadyToWin = false;
                        }
                        return $"readyToWin: {Config.forceReadyToWin}";
                    }
                case "forceEndPad":
                    {
                        if (args[1].ToLower() == "true")
                        {
                            Config.forceEndPad = true;
                        }
                        else
                        {
                            Config.forceEndPad = false;
                        }
                        return $"forceEndPad: {Config.forceEndPad}";
                    }
                case "extraVotes":
                    {
                        try
                        {
                            int extraVotes = int.Parse(args[1]);
                            Config.extraVotes = extraVotes;
                            return $"forceEndPad: {Config.extraVotes}";
                        }
                        catch (Exception e)
                        {
                            return "Error: Value is not an integer";
                        }
                    }
                default: return "Invalid config option \"{args[0]}\"";
            }
        }
    }
}
