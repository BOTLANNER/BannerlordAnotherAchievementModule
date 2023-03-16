using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using HarmonyLib;

using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace AnotherAchievementModule
{
    public class Main : MBSubModuleBase
    {
        /* Semantic Versioning (https://semver.org): */
        public static readonly int SemVerMajor = 1;
        public static readonly int SemVerMinor = 0;
        public static readonly int SemVerPatch = 0;
        public static readonly string? SemVerSpecial = null;
        private static readonly string SemVerEnd = (SemVerSpecial is not null) ? "-" + SemVerSpecial : string.Empty;
        public static readonly string Version = $"{SemVerMajor}.{SemVerMinor}.{SemVerPatch}{SemVerEnd}";

        public static readonly string Name = typeof(Main).Namespace;
        public const string DisplayName = "Another Achievement Module"; // to be shown to humans in-game
        public static readonly string HarmonyDomain = "com.b0tlanner.bannerlord." + Name.ToLower();

        internal static readonly Color ImportantTextColor = Color.FromUint(0x00F16D26); // orange

        internal static Settings? Settings;

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();
            var harmony = new Harmony(HarmonyDomain);
            harmony.PatchAll();
        }

        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            if (Settings.Instance is not null && Settings.Instance != Settings)
            {
                Settings = Settings.Instance;

                // register for settings property-changed events
                Settings.PropertyChanged += Settings_OnPropertyChanged;
            }
            InformationManager.DisplayMessage(new InformationMessage($"Loaded {DisplayName}", ImportantTextColor));
        }

        private void Settings_OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            InformationManager.DisplayMessage(new InformationMessage($"{DisplayName} - Settings Updated", ImportantTextColor));
        }
    }
}
