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
    [BepInDependency("FlipMods.ReservedItemSlotCore", BepInDependency.DependencyFlags.SoftDependency)]
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

        internal static EliteFlashlightConfig BoundConfig { get; private set; } = null!;

        #pragma warning disable IDE0051 // Remove unused private members
        void Awake()
        #pragma warning restore IDE0051 // Remove unused private members
        {
            mls = BepInEx.Logging.Logger.CreateLogSource(modGUID);
            if (Instance == null)
            {
                Instance = this;
            }
            BoundConfig = new EliteFlashlightConfig(base.Config);

            
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

            if(BoundConfig.EliteFlashlightPrice.Value > 0) { 
                elitePrice = BoundConfig.EliteFlashlightPrice.Value;
            }
            mls.LogInfo("Elite Flashlight set to " + elitePrice + " credits");

            TerminalNode eliteTerminalNode = EliteFlashlightAsset.LoadAsset<TerminalNode>("assets/iTerminalNode.asset");
            Items.RegisterShopItem(Missile_EliteFlashlight, null, null, eliteTerminalNode, elitePrice);

            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("FlipMods.ReservedFlashlightSlot"))
            {
                ReservedItemSlotsCompat.AddItemsToReservedItemSlots();
                mls.LogInfo("Elite Flashlight and Flashlight slot compat loaded");
            }
            

            mls.LogInfo("Elite Flashlight has fully loaded");
            harmony.PatchAll();
        }
    }
}
