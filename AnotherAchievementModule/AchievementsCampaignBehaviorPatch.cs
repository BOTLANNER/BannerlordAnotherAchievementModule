

using System;
using System.Linq;
using System.Reflection;

using HarmonyLib;

using StoryMode;
using StoryMode.GameComponents.CampaignBehaviors;

using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;

namespace AnotherAchievementModule
{
    [HarmonyPatch(typeof(AchievementsCampaignBehavior))]
    static class AchievementsCampaignBehaviorPatch
    {
        static MethodInfo CheckIfModulesAreDefaultMethod = typeof(AchievementsCampaignBehavior).GetMethod("CheckIfModulesAreDefault", BindingFlags.Instance | BindingFlags.NonPublic);
        static AchievementsCampaignBehavior singleton;

        static bool CheckIfModulesAreDefault(this AchievementsCampaignBehavior __instance)
        {
            return (bool) CheckIfModulesAreDefaultMethod.Invoke(__instance, null);
        }

        //[HarmonyPrefix]
        //[HarmonyPatch(MethodType.Constructor)]
        //internal static bool Ctor(ref AchievementsCampaignBehavior __instance)
        //{
        //    if (singleton != null)
        //    {
        //        __instance = singleton;
        //        return false;
        //    }
        //    singleton = __instance;
        //    return true;
        //}

        [HarmonyFinalizer]
        [HarmonyPatch("RegisterEvents")]
        static Exception? RegisterEventsFinalizer(Exception __exception)
        {
            Exception? _Exception;
            if (__exception is NullReferenceException && StoryModeManager.Current == null && Main.Settings!.AchievementsInSandbox)
            {
                _Exception = null;
            }
            else
            {
                _Exception = __exception;
            }
            return _Exception;
        }

        [HarmonyPrefix]
        [HarmonyPatch("CheckAchievementSystemActivity")]

        public static bool CheckAchievementSystemActivity(ref bool __result, ref bool ____deactivateAchievements, ref AchievementsCampaignBehavior __instance)
        {
            //____deactivateAchievements = false;
            //__result = true;
            bool flag;
            flag = (!__instance.CheckIfModulesAreDefault() || Game.Current.CheatMode ? false : !____deactivateAchievements);
            if (!flag)
            {
                if (!__instance.CheckIfModulesAreDefault())
                {
                    Debug.Print("Achievements were disabled because !CheckIfModulesAreDefault", 0, Debug.DebugColor.DarkRed, 17592186044416L);
                }
                if (Game.Current.CheatMode)
                {
                    Debug.Print("Achievements were disabled because Game.Current.CheatMode", 0, Debug.DebugColor.DarkRed, 17592186044416L);
                }
                if (____deactivateAchievements)
                {
                    Debug.Print("Achievements were disabled because _deactivateAchievements was true", 0, Debug.DebugColor.DarkRed, 17592186044416L);
                }
            }
            __result = flag || Main.Settings!.Achievements;
            return false;
        }




        [HarmonyPrefix]
        [HarmonyPatch("CheckIfModulesAreDefault")]
        private static bool CheckIfModulesAreDefault(ref bool __result, ref bool ____deactivateAchievements, ref AchievementsCampaignBehavior __instance)
        {
            bool flag = Campaign.Current.PreviouslyUsedModules.All<string>((string x) =>
            {
                if (x.Equals("Native", StringComparison.OrdinalIgnoreCase) || x.Equals("SandBoxCore", StringComparison.OrdinalIgnoreCase) || x.Equals("CustomBattle", StringComparison.OrdinalIgnoreCase) || x.Equals("SandBox", StringComparison.OrdinalIgnoreCase) || x.Equals("Multiplayer", StringComparison.OrdinalIgnoreCase) || x.Equals("BirthAndDeath", StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
                return x.Equals("StoryMode", StringComparison.OrdinalIgnoreCase);
            });
            if (!flag)
            {
                Debug.Print("Achievements are disabled! !CheckIfModulesAreDefault:", 0, Debug.DebugColor.DarkRed, 17592186044416L);
                foreach (string previouslyUsedModule in Campaign.Current.PreviouslyUsedModules)
                {
                    Debug.Print(previouslyUsedModule, 0, Debug.DebugColor.DarkRed, 17592186044416L);
                }
            }
            __result = flag;
            return false;
        }


        [HarmonyPrefix]
        [HarmonyPatch("DeactivateAchievements")]
        private static bool DeactivateAchievements(ref bool ____deactivateAchievements, ref AchievementsCampaignBehavior __instance, bool showMessage = true)
        {
            ____deactivateAchievements = !Main.Settings!.Achievements;
            CampaignEventDispatcher.Instance.RemoveListeners(__instance);
            if (showMessage)
            {
                if (____deactivateAchievements)
                {
                    MBInformationManager.AddQuickInformation(new TextObject("{=Z9mcDuDi}Achievements are disabled!", null), 0, null, ""); 
                }
                else
                {
                    MBInformationManager.AddQuickInformation(new TextObject("[AnotherAchievementModule] Achievements still enabled 🙂!", null), 0, null, "");
                }
            }
            return false;
        }

        [HarmonyPostfix]
        [HarmonyPatch("OnSessionLaunched")]
        private static void OnSessionLaunched(ref bool ____deactivateAchievements,CampaignGameStarter campaignGameStarter)
        {
            if (!____deactivateAchievements)
            {
                MBInformationManager.AddQuickInformation(new TextObject("[Another Achievement Module] Achievements still enabled!", null), 0, null, "");
            }
        }
    }
}
