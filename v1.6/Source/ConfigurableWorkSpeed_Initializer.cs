using HugsLib;
using HugsLib.Settings;

namespace ConfigurableWorkSpeed
{
    public class ConfigurableWorkSpeedMod : ModBase
    {
        public override string ModIdentifier => "ConfigurableWorkSpeed";

        public override void DefsLoaded()
        {
            ConfigurableWorkSpeedSettings_HugsLib.Initialize(Settings);
        }
    }
}