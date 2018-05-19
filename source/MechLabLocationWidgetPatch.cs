using System;
using System.Reflection;

using BattleTech;
using BattleTech.UI;
using Harmony;
using UnityEngine;

namespace Pansar
{
    [HarmonyPatch(typeof(MechLabLocationWidget), "OnFrontArmorAdd")]
    public static class MechLabLocationWidgetOnFrontArmorAddPatch
    {
        static bool Prefix(MechLabLocationWidget __instance)
        {
            LocationDef locationDef = ReflectionUtils.GetChassisLocationDef(__instance);
            int mod = Mathf.FloorToInt(__instance.currentArmor) % 5;
            float delta = Math.Min(
                ArmorRules.MaxFrontArmor(locationDef, __instance.loadout, 0) - __instance.currentArmor,
                UnityGameInstance.BattleTechGame.MechStatisticsConstants.ARMOR_PER_STEP - mod);

            if (delta > 0)
            {
                __instance.currentArmor += delta;
                __instance.maxRearArmor = ArmorRules.MaxRearArmor(locationDef, __instance.loadout, __instance.currentArmor);
                float rearOverflow = __instance.currentRearArmor - __instance.maxRearArmor;
                if (rearOverflow > 0)
                {
                    __instance.currentRearArmor -= rearOverflow;
                    __instance.maxArmor += rearOverflow;
                }
                __instance.ModifyArmor(false, 0, true);
            }
            return false;
        }
    }

    [HarmonyPatch(typeof(MechLabLocationWidget), "OnFrontArmorSubtract")]
    public static class MechLabLocationWidgetOnFrontArmorSubstractPatch
    {
        static bool Prefix(MechLabLocationWidget __instance)
        {
            int mod = Mathf.FloorToInt(__instance.currentArmor) % 5;
            float delta = Math.Min(__instance.currentArmor, mod == 0 ? UnityGameInstance.BattleTechGame.MechStatisticsConstants.ARMOR_PER_STEP : mod);

            if (delta > 0)
            {
                LocationDef locationDef = ReflectionUtils.GetChassisLocationDef(__instance);
                __instance.currentArmor -= delta;
                __instance.maxRearArmor = ArmorRules.MaxRearArmor(locationDef, __instance.loadout, __instance.currentArmor);
                __instance.ModifyArmor(false, 0, true);
            }
            return false;
        }
    }


    [HarmonyPatch(typeof(MechLabLocationWidget), "OnRearArmorAdd")]
    public static class MechLabLocationWidgetOnRearArmorAddPatch
    {
        static bool Prefix(MechLabLocationWidget __instance)
        {
            LocationDef locationDef = ReflectionUtils.GetChassisLocationDef(__instance);
            int mod = Mathf.FloorToInt(__instance.currentRearArmor) % 5;
            float delta = Math.Min(
                ArmorRules.MaxRearArmor(locationDef, __instance.loadout, 0) - __instance.currentRearArmor, 
                UnityGameInstance.BattleTechGame.MechStatisticsConstants.ARMOR_PER_STEP - mod);

            if (delta > 0)
            {
                __instance.currentRearArmor += delta;
                __instance.maxArmor = ArmorRules.MaxFrontArmor(locationDef, __instance.loadout, __instance.currentRearArmor);
                float overflow = __instance.currentArmor - __instance.maxArmor;
                if (overflow > 0)
                {
                    __instance.currentArmor -= overflow;
                    __instance.maxRearArmor += overflow;
                }
                __instance.ModifyArmor(true, 0, true);
            }
            return false;
        }
    }

    [HarmonyPatch(typeof(MechLabLocationWidget), "OnRearArmorSubtract")]
    public static class MechLabLocationWidgetOnRearArmorSubstractPatch
    {
        static bool Prefix(MechLabLocationWidget __instance)
        {
            int mod = Mathf.FloorToInt(__instance.currentRearArmor) % 5;
            float delta = Math.Min(__instance.currentRearArmor, mod == 0 ? UnityGameInstance.BattleTechGame.MechStatisticsConstants.ARMOR_PER_STEP : mod);
            if (delta > 0)
            {
                LocationDef locationDef = ReflectionUtils.GetChassisLocationDef(__instance);
                __instance.currentRearArmor -= delta;
                __instance.maxArmor = ArmorRules.MaxFrontArmor(locationDef, __instance.loadout, __instance.currentRearArmor);
                __instance.ModifyArmor(true, 0, true);
            }
            return false;
        }
    }


    [HarmonyPatch(typeof(MechLabLocationWidget), "SetData")]
    public static class MechLabLocationWidgetSetDataPatch
    {
        static void Postfix(MechLabLocationWidget __instance, LocationLoadoutDef loadout)
        {
            LocationDef locationDef = ReflectionUtils.GetChassisLocationDef(__instance);

            __instance.currentArmor = Math.Min(loadout.CurrentArmor, ArmorRules.MaxFrontArmor(locationDef, __instance.loadout, 0));
            __instance.currentRearArmor = Math.Min(loadout.CurrentRearArmor, ArmorRules.MaxRearArmor(locationDef, __instance.loadout, __instance.currentArmor));

            __instance.maxArmor = ArmorRules.MaxFrontArmor(locationDef, __instance.loadout, __instance.currentRearArmor);
            __instance.maxRearArmor = ArmorRules.MaxRearArmor(locationDef, __instance.loadout, __instance.currentArmor);
            __instance.ModifyArmor(false, 0, false);
        }
    }
}