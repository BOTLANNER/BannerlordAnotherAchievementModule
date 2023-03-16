using System.Collections.Generic;

using MCM.Abstractions.Attributes;
using MCM.Abstractions.Attributes.v2;
#if MCM_v5
using MCM.Abstractions.Base.Global;
#else
using MCM.Abstractions.Settings.Base.Global;
#endif

namespace AnotherAchievementModule
{
    public class Settings : AttributeGlobalSettings<Settings>
    {
        public override string Id => $"{Main.Name}_v1";
        public override string DisplayName => Main.DisplayName;
        public override string FolderName => Main.Name;
        public override string FormatType => "json";

        private const string Achievements_Hint = "Allows achievements in modded games. [ Default: ON ]";

        [SettingPropertyBool("Allow Achievements", HintText = Achievements_Hint, RequireRestart = true, Order = 0, IsToggle = false)]
        [SettingPropertyGroup("Achievement Settings", GroupOrder = 0)]
        public bool Achievements { get; set; } = true;

        private const string AchievementsInSandbox_Hint = "Adds achievements in Sandbox game mode. [ Default: ON ]";
        
        [SettingPropertyBool("Achievements In Sandbox", HintText = AchievementsInSandbox_Hint, RequireRestart = true, Order = 1, IsToggle = false)]
        [SettingPropertyGroup("Achievement Settings", GroupOrder = 0)]
        public bool AchievementsInSandbox { get; set; } = true;

        private const string ReEnableAchievementsInSaves_Hint = "Re-enables achievements in saves that had achievements already disabled. [ Default: OFF ]";

        [SettingPropertyBool("Re-enable Achievements in Saves", HintText = ReEnableAchievementsInSaves_Hint, RequireRestart = true, Order = 2, IsToggle = false)]
        [SettingPropertyGroup("Achievement Settings", GroupOrder = 0)]
        public bool ReEnableAchievementsInSaves { get; set; } = false;

        private const string EnableAchievementConsoleCommands_Hint = "Enables console commands to manage achievements. Use 'another_achievement.help' for commands. [ Default: ON ]";

        [SettingPropertyBool("Enable Achievement Console Commands", HintText = EnableAchievementConsoleCommands_Hint, RequireRestart = true, Order = 3, IsToggle = false)]
        [SettingPropertyGroup("Achievement Settings", GroupOrder = 0)]
        public bool EnableAchievementConsoleCommands { get; set; } = true;
    }
}
