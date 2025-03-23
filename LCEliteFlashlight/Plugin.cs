using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.IO;
using System.Reflection;
using UnityEngine;
using LethalLib.Modules;
using LethalLib;
using BepInEx.Bootstrap;

namespace LCEliteFlashlight
{
    [BepInDependency("FlipMods.ReservedItemSlotCore", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("hlb.lightutility", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInPlugin(modGUID,modName, modVersion)]
    [BepInDependency(Plugin.ModGUID)]
    public class EliteFlashlightBase : BaseUnityPlugin
    {
        private const string modGUID = "Missile.LCEliteFlashlight";
        private const string modName = "LCEliteFlashlight";
        private const string modVersion = "1.1.0";

        private readonly Harmony harmony = new Harmony(modGUID);

        private static EliteFlashlightBase Instance;

        internal ManualLogSource mls;

        AssetBundle EliteFlashlightAsset;

        internal static EliteFlashlightConfig BoundConfig { get; private set; } = null!;

        #pragma warning disable IDE0051 // Remove unused private members
        void Awake()
        #pragma warning restore IDE0051 // Remove unused private members
        {
            int elitePrice = 50;


            mls = BepInEx.Logging.Logger.CreateLogSource(modGUID);
            if (Instance == null)
            {
                Instance = this;
            }
            BoundConfig = new EliteFlashlightConfig(base.Config);

            //get asset from assetbundle
            string sAssemblyLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            EliteFlashlightAsset = AssetBundle.LoadFromFile(Path.Combine(sAssemblyLocation, "eliteflashlightbundle"));
            if (EliteFlashlightAsset == null )
            {
                mls.LogError("Failed to load EliteFlashlight assets");
                return;
            }

            
            //create the flashlight item
            Item Missile_EliteFlashlight = EliteFlashlightAsset.LoadAsset<Item>("assets/EliteFlashlightItem.asset");
            if (Missile_EliteFlashlight == null)
            {
                mls.LogError("Failed to load Elite-flashlight item");
                return;
            }

            NetworkPrefabs.RegisterNetworkPrefab(Missile_EliteFlashlight.spawnPrefab);
            Items.RegisterItem(Missile_EliteFlashlight);


            //set up shop item and price
            if(BoundConfig.EliteFlashlightPrice.Value > 0) { 
                elitePrice = BoundConfig.EliteFlashlightPrice.Value;
            }
            mls.LogInfo("Elite Flashlight set to " + elitePrice + " credits");

            TerminalNode eliteTerminalNode = EliteFlashlightAsset.LoadAsset<TerminalNode>("assets/iTerminalNode.asset");
            Items.RegisterShopItem(Missile_EliteFlashlight, null, null, eliteTerminalNode, elitePrice);

            //compats
            if (Chainloader.PluginInfos.ContainsKey("FlipMods.ReservedFlashlightSlot"))
            {
                ReservedItemSlotsCompat.AddItemsToReservedItemSlots();
                mls.LogInfo("Elite Flashlight and Flashlight slot compat loaded");
            }
            
            if (Chainloader.PluginInfos.ContainsKey("hlb.lightutility"))
            {
                Missile_EliteFlashlight.spawnPrefab.GetComponent<FlashlightItem>().flashlightTypeID = 4;
                mls.LogInfo("Elite Flashlight and light utility compat loaded");
            }

            mls.LogInfo("Elite Flashlight has fully loaded");
            harmony.PatchAll();
        }
    }
}
