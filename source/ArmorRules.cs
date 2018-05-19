using System;

using BattleTech;

namespace Pansar
{

    public static class ArmorRules
    {
        public static float MaxFrontArmor(LocationDef locationDef, LocationLoadoutDef loadout, float currentRearArmor)
        {
            if (locationDef.Location == ChassisLocations.Head && Control.settings.HeadMaxArmorOverride != null)
            {
                return Control.settings.HeadMaxArmorOverride.Value;
            }
            else
            {
                float baseStructure = Control.settings.UseCurrentStructure ? loadout.CurrentInternalStructure : locationDef.InternalStructure;
                float maxFront = baseStructure * Control.settings.StructureFrontLoadCapacityFactor;
				float maxTotal = baseStructure * Control.settings.StructureTotalLoadCapacityFactor - currentRearArmor;
				return Math.Max(0, Math.Min(maxFront, maxTotal));
            }
        }

        public static float MaxRearArmor(LocationDef locationDef, LocationLoadoutDef loadout, float currentFrontArmor)
        {
            float baseStructure = Control.settings.UseCurrentStructure ? loadout.CurrentInternalStructure : locationDef.InternalStructure;
            float maxRear;
            if (locationDef.Location == ChassisLocations.CenterTorso || locationDef.Location == ChassisLocations.LeftTorso || locationDef.Location == ChassisLocations.RightTorso)
            {
                maxRear = baseStructure * Control.settings.StructureRearLoadCapacityFactor;
            }
            else
            {
                maxRear = 0;
            }
            float maxTotal = baseStructure * Control.settings.StructureTotalLoadCapacityFactor - currentFrontArmor;
            return Math.Max(0, Math.Min(maxRear, maxTotal));
        }
    }
}