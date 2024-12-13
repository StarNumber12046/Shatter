using MIU;
using Parse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Shatter.Commands
{
    internal class Multiplayer
    {
        [ConsoleCommand("vote", "Submits a vote with an optional custom User ID. Passing \"random\" as the UserID will generate a random one", "[string voteId] [string? UserID]", false, false, description = "Submit a vote with a random UID (useful for botting MP)")]
        public static string vote(params string[] args)
        {
            if (args.Length == 0) return "Missing argument: voteId";
            Debug.Log("Submitting vote to server: " + args[0]);
            if (args.Length == 1)
            {
                GameConnection.ServerConnection.CallRPC("VoteSubmittedRPC", new object[] { ParseUser.CurrentUser.Username, args[0] });
                return "Voted for " + args[0] + " as " + ParseUser.CurrentUser.Username;
            }
            if (args[1] == "random")
            {

                System.Random random = new System.Random();
                int firstPart = random.Next(10000000, 99999999); // Ensures 8 digits
                long secondPart = random.Next(100000000, 999999999); // Ensures 9 digits
                string RandomSteamID = $"{firstPart}{secondPart}";
                GameConnection.ServerConnection.CallRPC("VoteSubmittedRPC", new object[] { $"STEAM_{RandomSteamID}", args[0] });
                return "Voted for " + args[0] + " as STEAM_" + RandomSteamID;
            }
            GameConnection.ServerConnection.CallRPC("VoteSubmittedRPC", new object[] { $"STEAM_{args[1]}", args[0] });
            return "Voted ID " + args[0] + " as STEAM_" + args[1];

        }
    }
}
