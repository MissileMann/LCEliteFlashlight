using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LCEliteFlashlight
{
    class EliteFlashlightConfig
    {
        public readonly ConfigEntry<int> EliteFlashlightPrice;
        internal ManualLogSource mls;

        public EliteFlashlightConfig(ConfigFile cfg)
        {
            mls.LogInfo("Elite Flashlight config generating");
            cfg.SaveOnConfigSet = false;

            EliteFlashlightPrice = cfg.Bind("Elite-flashlight Price", "Elite-flashlight Price", 50, "What value you want the Elite-flashlight to cost in the shop.");

            // Get rid of old settings from the config file that are not used anymore
            ClearOrphanedEntries(cfg);
            // We need to manually save since we disabled `SaveOnConfigSet` earlier
            cfg.Save();
            // And finally, we re-enable `SaveOnConfigSet` so changes to our config
            // entries are written to the config file automatically from now on
            cfg.SaveOnConfigSet = true;
        }

        static void ClearOrphanedEntries(ConfigFile cfg)
        {
            // Find the private property `OrphanedEntries` from the type `ConfigFile`
            PropertyInfo orphanedEntriesProp = AccessTools.Property(typeof(ConfigFile), "OrphanedEntries");
            // And get the value of that property from our ConfigFile instance
            var orphanedEntries = (Dictionary<ConfigDefinition, string>)orphanedEntriesProp.GetValue(cfg);
            // And finally, clear the `OrphanedEntries` dictionary
            orphanedEntries.Clear();
        }
    }
}
