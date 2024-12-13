using MIU;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Shatter.Commands
{
    internal class Level
    {
        [ConsoleCommand("pos", "Get yor current position", null, false, false)]
        public static string Position()
        {
            GamePlayManager gamePlayManager = GamePlayManager.Get(true);
            if (gamePlayManager == null) return "Failed to get (GamePlayManager)gamePlayManager";
            MarbleManager marbleManager = gamePlayManager.manager;
            if (marbleManager == null) return "Failed to get (MarbleManager)marbleManager";
            MarbleController player = marbleManager.Player;
            if (player == null) return "Failed to get (MarbleController)player";
            var pos = player.GetPosition();
            return $"Vector3D(x: {pos.x}; y: {pos.y}; z: {pos.z})";
        }

        [ConsoleCommand("startLevel", "Starts the current level (this also works in menus)")]
        public static string StartLevel()
        {
            GlobalContext.Invoke<PlayLevel>(new PlayLevel.Configuration() { });
            return "Playing level...";
        }

        [ConsoleCommand("deRewind", "Removes the rewind flag from the current level", null, false, false)]
        public static string DeRewind(params string[] shouldUseRaw)
        {
            MarbleManager.usedRewind = false;
            return "No more rewind!";

        }

        [ConsoleCommand("win", "Wins the current level for you", null, false, false)]
        public static string Win()
        {
            GamePlayManager gamePlayManager = GamePlayManager.Get(true);
            if (gamePlayManager == null) return "Failed to get (GamePlayManager)gamePlayManager";
            MarbleManager marbleManager = gamePlayManager.manager;
            if (marbleManager == null) return "Failed to get (MarbleManager)marbleManager";
            MarbleController player = marbleManager.Player;
            if (player == null) return "Failed to get (MarbleController)player";
            FieldInfo finishClipField = typeof(MarbleController).GetField("finishClip", BindingFlags.NonPublic | BindingFlags.Instance);
            if (finishClipField == null) return "Failed to get the finishClip field";

            // Retrieving the value of finishClip
            var finishClip = finishClipField.GetValue(player) as PlayClips;
            if (finishClip == null) return "Failed to get (AudioClip)finishClip";

            // Playing the finishClip with the specified parameters
            finishClip.Play(player.GetOneShotAudioSource(), 1f, 1f, true);
            GamePlayManager.Get(true).FinishPlay(player);
            GamePlayManager.Get(true).StopPlay();

            return "The level has been completed successfully!";
        }
    }
}
