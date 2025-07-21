using Verse;
using RimWorld;
using HugsLib;
using System.Collections.Generic;
using System.Linq;
using HugsLib.Settings;

namespace ConfigurableWorkSpeed
{
    public class StatPart_WorkSpeedMultiplier : StatPart
    {
        /// <summary>
        /// StatPart_WorkSpeedMultiplier applies configurable multipliers to work speed-related stats.
        /// - Uses fixed multipliers for core RimWorld stats (defined in ConfigurableWorkSpeedSettings_HugsLib).
        /// - Uses dynamic multipliers for any detected custom StatDefs (from any mod) matching work-related keywords and categories.
        /// - Multipliers are configured via HugsLib settings and are compatible with UINotIncluded for live in-game changes.
        /// - To extend: other mods can add StatDefs with relevant keywords/categories, and this mod will automatically provide sliders for them.
        /// </summary>
        private static readonly Dictionary<string, System.Func<float>> statMultipliers = new()
        {
            { "WorkSpeedGlobal", () => ConfigurableWorkSpeedSettings_HugsLib.WorkSpeedGlobal.Value },
            { "GeneralLaborSpeed", () => ConfigurableWorkSpeedSettings_HugsLib.GeneralLaborSpeed.Value },
            { "ConstructionSpeed", () => ConfigurableWorkSpeedSettings_HugsLib.ConstructionSpeed.Value },
            { "MiningSpeed", () => ConfigurableWorkSpeedSettings_HugsLib.MiningSpeed.Value },
            { "PlantWorkSpeed", () => ConfigurableWorkSpeedSettings_HugsLib.PlantWorkSpeed.Value },
            { "CookingSpeed", () => ConfigurableWorkSpeedSettings_HugsLib.CookingSpeed.Value },
            { "ButcheryFleshSpeed", () => ConfigurableWorkSpeedSettings_HugsLib.ButcheryFleshSpeed.Value },
            { "DrugCookingSpeed", () => ConfigurableWorkSpeedSettings_HugsLib.DrugCookingSpeed.Value },
            { "SculptingSpeed", () => ConfigurableWorkSpeedSettings_HugsLib.SculptingSpeed.Value },
            { "TailoringSpeed", () => ConfigurableWorkSpeedSettings_HugsLib.TailoringSpeed.Value },
            { "SmithingSpeed", () => ConfigurableWorkSpeedSettings_HugsLib.SmithingSpeed.Value },
            { "ResearchSpeed", () => ConfigurableWorkSpeedSettings_HugsLib.ResearchSpeed.Value },
            { "CleaningSpeed", () => ConfigurableWorkSpeedSettings_HugsLib.CleaningSpeed.Value }
        };

        public override void TransformValue(StatRequest req, ref float val)
        {
            try
            {
                float multiplier = 1f;
                // Try fixed multipliers first
                if (statMultipliers.TryGetValue(parentStat.defName, out var getter))
                {
                    multiplier = getter();
                }
                else if (ConfigurableWorkSpeedSettings_HugsLib.StatMultipliers != null)
                {
                    var dynamicEntry = ConfigurableWorkSpeedSettings_HugsLib.StatMultipliers
                        .FirstOrDefault(kvp => kvp.Key.defName == parentStat.defName);
                    if (dynamicEntry.Key != null)
                    {
                        multiplier = dynamicEntry.Value.Value;
                    }
                }
                val *= multiplier;
                Log.Message($"[ConfigurableWorkSpeed] Applied multiplier {multiplier:0.00} to stat {parentStat.defName} (final value: {val:0.00})");
                if (multiplier > 1.5f)
                {
                    Log.Warning($"[ConfigurableWorkSpeed] Multiplier for {parentStat.defName} is set to {multiplier:0.00}. Extreme values may cause instability or crashes.");
                }
            }
            catch (System.Exception ex)
            {
                Log.Error($"[ConfigurableWorkSpeed] Error applying multiplier to stat {parentStat?.defName}: {ex.Message}");
            }
        }

        public override string ExplanationPart(StatRequest req)
        {
            if (statMultipliers.TryGetValue(parentStat.defName, out var getter))
            {
                float mult = getter();
                if (mult != 1f)
                    return $"Configurable Work Speed: x{mult:0.##}";
            }
            // Dynamic stats
            if (ConfigurableWorkSpeedSettings_HugsLib.StatMultipliers != null)
            {
                var dynamicEntry = ConfigurableWorkSpeedSettings_HugsLib.StatMultipliers
                    .FirstOrDefault(kvp => kvp.Key.defName == parentStat.defName);
                if (dynamicEntry.Key != null)
                {
                    float mult = dynamicEntry.Value.Value;
                    if (mult != 1f)
                        return $"Configurable Work Speed: x{mult:0.##}";
                }
            }
            return null;
        }
    }
}
