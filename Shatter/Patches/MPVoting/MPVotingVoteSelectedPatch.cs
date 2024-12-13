using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;


namespace Shatter.Patches.MPVotingPatches
{
    [HarmonyPatch(typeof(MPVoting), "VoteSelected")]
    public class MPVotingVoteSelectedPatch
    {
        public static bool Prefix(int OptionID)
        {
            MonoBehaviour.print("Voted " + OptionID);
            System.Random random = new System.Random();
            for (int i = 1; i <= Config.extraVotes; i++)
            {

                // Generate the first part of the number (first 8 digits)
                int firstPart = random.Next(10000000, 99999999); // Ensures 8 digits

                // Generate the second part of the number (last 9 digits)
                long secondPart = random.Next(100000000, 999999999); // Ensures 9 digits

                // Combine the two parts to create a 17-digit number
                string random17DigitNumber = $"{firstPart}{secondPart}";
                GameConnection.ServerConnection.CallRPC("VoteSubmittedRPC", new object[] { "STEAM_" + random17DigitNumber, OptionID });
                Debug.Log("Voted ID " + OptionID.ToString() + " as STEAM_" + random17DigitNumber + " (vote " + i.ToString() + ")");
            }
            return true;
        }
    }
}
