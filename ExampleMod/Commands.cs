using MIU;
using UnityEngine;

namespace ExampleMod
{
    internal class Commands
    {
        [ConsoleCommand(null, null, null, false, false, description = "Prints hello world or hello [name]")]
        public static string example(params string[] args)
        {
            if (args.Length == 0)
            {
                return "Hello, World!";
            }
            return "Hello, " + args[0];
        }
    }
}
