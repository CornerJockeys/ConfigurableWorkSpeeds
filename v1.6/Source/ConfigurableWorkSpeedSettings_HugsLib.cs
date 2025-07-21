using Verse;
using RimWorld;
using HugsLib;
using HugsLib.Settings;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace ConfigurableWorkSpeed
{
    public static class ConfigurableWorkSpeedSettings_HugsLib
    {
        // ...existing code...
        // All fields, methods, and helper functions go here
        // ...existing code...
        private static readonly float MinValue = 0.0f;
        private static readonly float MaxValue = 5.0f;
        private static readonly float StepValue = 0.05f;

        private static void SetCustomDrawer(SettingHandle<float> handle)
        {
            handle.CustomDrawer = rect =>
            {
                float value = handle.Value;
                float labelWidth = 60f;
                float buttonWidth = 30f;
                Rect minusRect = new Rect(rect.x, rect.y, buttonWidth, rect.height);
                Rect valueRect = new Rect(rect.x + buttonWidth, rect.y, labelWidth, rect.height);
                Rect plusRect = new Rect(rect.x + buttonWidth + labelWidth, rect.y, buttonWidth, rect.height);
                bool changed = false;
                if (Widgets.ButtonText(minusRect, "-", true, true))
                {
                    value = Mathf.Max(MinValue, (float)System.Math.Round(value - StepValue, 2));
                    changed = true;
                }
                Widgets.Label(valueRect, value.ToString("F2"));
                if (Widgets.ButtonText(plusRect, "+", true, true))
                {
                    value = Mathf.Min(MaxValue, (float)System.Math.Round(value + StepValue, 2));
                    changed = true;
                }
                if (changed) handle.Value = value;
                return false;
            };
        }
        public static Dictionary<StatDef, SettingHandle<float>> StatMultipliers;
        public static SettingHandle<float> WorkSpeedGlobal;
        public static SettingHandle<float> GeneralLaborSpeed;
        public static SettingHandle<float> ConstructionSpeed;
        public static SettingHandle<float> MiningSpeed;
        public static SettingHandle<float> PlantWorkSpeed;
        public static SettingHandle<float> CookingSpeed;
        public static SettingHandle<float> ButcheryFleshSpeed;
        public static SettingHandle<float> DrugCookingSpeed;
        public static SettingHandle<float> SculptingSpeed;
        public static SettingHandle<float> TailoringSpeed;
        public static SettingHandle<float> SmithingSpeed;
        public static SettingHandle<float> ResearchSpeed;
        public static SettingHandle<float> CleaningSpeed;
        public static void Initialize(ModSettingsPack settings)
        {
            Log.Message("[ConfigurableWorkSpeed] Initializing settings...");
            StatMultipliers = FilterAndRegisterStats(null, settings);
            Log.Message($"[ConfigurableWorkSpeed] Registered {StatMultipliers.Count} work speed-related stats for configuration.");
            WorkSpeedGlobal = settings.GetHandle(
                "WorkSpeedGlobal",
                "Global Work Speed",
                "Multiplier for all work speed (Recommended: 0.0 – 5.0)",
                1.0f
            );
            SetCustomDrawer(WorkSpeedGlobal);

            GeneralLaborSpeed = settings.GetHandle(
                "GeneralLaborSpeed",
                "General Labor Speed",
                "Multiplier for general labor speed (Recommended: 0.0 – 5.0)",
                1.0f
            );
            SetCustomDrawer(GeneralLaborSpeed);

            ConstructionSpeed = settings.GetHandle(
                "ConstructionSpeed",
                "Construction Speed",
                "Multiplier for construction speed (Recommended: 0.0 – 5.0)",
                1.0f
            );
            SetCustomDrawer(ConstructionSpeed);

            MiningSpeed = settings.GetHandle(
                "MiningSpeed",
                "Mining Speed",
                "Multiplier for mining speed (Recommended: 0.0 – 5.0)",
                1.0f
            );
            SetCustomDrawer(MiningSpeed);

            PlantWorkSpeed = settings.GetHandle(
                "PlantWorkSpeed",
                "Plant Work Speed",
                "Multiplier for plant work speed (Recommended: 0.0 – 5.0)",
                1.0f
            );
            SetCustomDrawer(PlantWorkSpeed);

            CookingSpeed = settings.GetHandle(
                "CookingSpeed",
                "Cooking Speed",
                "Multiplier for cooking speed (Recommended: 0.0 – 5.0)",
                1.0f
            );
            SetCustomDrawer(CookingSpeed);

            ButcheryFleshSpeed = settings.GetHandle(
                "ButcheryFleshSpeed",
                "Butchery Flesh Speed",
                "Multiplier for butchery flesh speed (Recommended: 0.0 – 5.0)",
                1.0f
            );
            SetCustomDrawer(ButcheryFleshSpeed);

            DrugCookingSpeed = settings.GetHandle(
                "DrugCookingSpeed",
                "Drug Cooking Speed",
                "Multiplier for drug cooking speed (Recommended: 0.0 – 5.0)",
                1.0f
            );
            SetCustomDrawer(DrugCookingSpeed);

            SculptingSpeed = settings.GetHandle(
                "SculptingSpeed",
                "Sculpting Speed",
                "Multiplier for sculpting speed (Recommended: 0.0 – 5.0)",
                1.0f
            );
            SetCustomDrawer(SculptingSpeed);

            TailoringSpeed = settings.GetHandle(
                "TailoringSpeed",
                "Tailoring Speed",
                "Multiplier for tailoring speed (Recommended: 0.0 – 5.0)",
                1.0f
            );
            SetCustomDrawer(TailoringSpeed);

            SmithingSpeed = settings.GetHandle(
                "SmithingSpeed",
                "Smithing Speed",
                "Multiplier for smithing speed (Recommended: 0.0 – 5.0)",
                1.0f
            );
            SetCustomDrawer(SmithingSpeed);

            ResearchSpeed = settings.GetHandle(
                "ResearchSpeed",
                "Research Speed",
                "Multiplier for research speed (Recommended: 0.0 – 5.0)",
                1.0f
            );
            SetCustomDrawer(ResearchSpeed);

            CleaningSpeed = settings.GetHandle(
                "CleaningSpeed",
                "Cleaning Speed",
                "Multiplier for cleaning speed (Recommended: 0.0 – 5.0)",
                1.0f
            );
            SetCustomDrawer(CleaningSpeed);
        }
        private static readonly string[] ValidCategories = new[] {
            "PawnWork", "PawnEfficiency", "Basics", "Production", "Work", "Crafting", "Drugs", "Medical", "StatsReport"
        };

        private static readonly string[] RelevantKeywords = new[] {
            "speed", "work", "efficiency", "labor", "construction", "mining", "plant", "cooking", "butchery", "drug", "sculpting", "tailoring", "smithing", "research", "cleaning"
        };

        public static Dictionary<StatDef, SettingHandle<float>> FilterAndRegisterStats(ModBase mod, ModSettingsPack settings)
        {
            var result = new Dictionary<StatDef, SettingHandle<float>>();

            foreach (var stat in DefDatabase<StatDef>.AllDefs)
            {
                string defNameLower = stat.defName.ToLower();

                // Must contain one of the keywords
                bool keywordMatch = RelevantKeywords.Any(k => defNameLower.Contains(k));
                if (!keywordMatch) continue;

                // Optional: restrict by category
                if (stat.category != null && !ValidCategories.Contains(stat.category.defName)) continue;

                // Create the setting using positional arguments
                var handle = settings.GetHandle<float>(
                    settingName: $"Multiplier_{stat.defName}",
                    title: stat.label.CapitalizeFirst(),
                    description: $"Multiplier for {stat.label.CapitalizeFirst()} (Default: 100%)",
                    defaultValue: 1.0f
                );

                Log.Message(string.Format("[ConfigurableWorkSpeed] Registered stat: {0} ({1})", stat.defName, stat.label));
                result[stat] = handle;
            }

            return result;
        }
    }
}