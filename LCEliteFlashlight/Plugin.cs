using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCEliteFlashlight
{
    [BepInPlugin(modGUID,modName, modVersion)]
    [BepInDependency(LethalLib.Plugin.ModGUID)]
    public class EliteFlashlightBase : BaseUnityPlugin
    {
        private const string modGUID = "Missile.LCEliteFlashlight";
        private const string modName = "LCEliteFlashlight";
        private const string modVersion = "1.0.0.0";

        private readonly Harmony harmony = new Harmony(modGUID);

        private static EliteFlashlightBase Instance;

        internal ManualLogSource mls;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }

            mls = BepInEx.Logging.Logger.CreateLogSource(modGUID);

            mls.LogInfo("Elite Flashlight has awakened");

            harmony.PatchAll();
        }
    }
}
