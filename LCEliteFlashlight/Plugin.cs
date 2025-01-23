using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using LethalLib.Modules;
using LethalLib;

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

        AssetBundle EliteFlashlightAsset;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }

            mls = BepInEx.Logging.Logger.CreateLogSource(modGUID);

            string sAssemblyLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            EliteFlashlightAsset = AssetBundle.LoadFromFile(Path.Combine(sAssemblyLocation, "eliteflashlightbundle"));
            if (EliteFlashlightAsset == null )
            {
                mls.LogError("Failed to load EliteFlashlight assets");
                return;
            }

            Item Missile_EliteFlashlight = EliteFlashlightAsset.LoadAsset<Item>("assets/EliteFlashlightItem.asset");
            if (Missile_EliteFlashlight == null )
            {
                mls.LogError("Failed to load Elite-flashlight item");
                return;
            }

            NetworkPrefabs.RegisterNetworkPrefab(Missile_EliteFlashlight.spawnPrefab);
            Items.RegisterItem(Missile_EliteFlashlight);

            int elitePrice = 50;
            TerminalNode eliteTerminalNode = EliteFlashlightAsset.LoadAsset<TerminalNode>("assets/iTerminalNode.asset");
            Items.RegisterShopItem(Missile_EliteFlashlight, null, null, eliteTerminalNode, elitePrice);

            mls.LogInfo("Elite Flashlight has loaded");
            harmony.PatchAll();
        }
    }
}
