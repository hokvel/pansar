using System;
using System.Reflection;

using DynModLib;
using Harmony;
using HBS.Logging;
using Newtonsoft.Json;

namespace Pansar
{ 
    public class Settings
    {
        public bool UseCurrentStructure = false;
        public float? HeadMaxArmorOverride = 45;
        public float StructureTotalLoadCapacityFactor = 2.0f;
        public float StructureFrontLoadCapacityFactor = 2.0f;
        public float StructureRearLoadCapacityFactor = 1.0f;
    }

    public static class Control
    {
        public static Mod mod;
        public static Settings settings = new Settings();
        public static ModSettings modSettings = new ModSettings();

        public static void Start(string modDirectory, string json)
        {
            mod = new Mod(modDirectory);
            Logger.SetLoggerLevel(mod.Logger.Name, LogLevel.Log);

            mod.LoadSettings(modSettings);
            try
            {
                Settings customSettings = JsonConvert.DeserializeObject<Settings>(json);
                if (customSettings.StructureRearLoadCapacityFactor < 0 ||
                    customSettings.StructureFrontLoadCapacityFactor < 0 ||
                    customSettings.StructureTotalLoadCapacityFactor < 0)
                {
                    mod.Logger.LogError("load capacity factors must be non-negative");
                }
				else if (customSettings.HeadMaxArmorOverride != null && customSettings.HeadMaxArmorOverride.Value < 0)
				{
                    mod.Logger.LogError("headMaxArmorOverride must be non-negative or null");
				}
                else
                {
                    settings = customSettings;
                    mod.Logger.Log("custom settings applied");
                }
            }
            catch (Exception e)
            {
                mod.Logger.LogError(e);
            }

            var harmony = HarmonyInstance.Create(mod.Name);
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            mod.Logger.Log("loaded " + mod.Name);
        }
    }
}
