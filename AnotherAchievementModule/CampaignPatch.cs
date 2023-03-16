

using System;
using System.Collections.Generic;

using HarmonyLib;

using TaleWorlds.CampaignSystem;

namespace AnotherAchievementModule
{
    [HarmonyPatch(typeof(Campaign))]
    static class CampaignPatch
    {
        private readonly static HashSet<string> _allowedModules;

        static CampaignPatch()
        {
            HashSet<string> strs = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            strs.Add("Native");
            strs.Add("SandBoxCore");
            strs.Add("CustomBattle");
            strs.Add("SandBox");
            strs.Add("Multiplayer");
            strs.Add("BirthAndDeath");
            strs.Add("StoryMode");
            _allowedModules = strs;
        }

        [HarmonyPrefix]
        [HarmonyPatch("DetermineModules")]

        private static bool DetermineModules(ref List<string> ____previouslyUsedModules)
        {
            if (____previouslyUsedModules == null)
            {
                ____previouslyUsedModules = new List<string>();
            }
            string[] moduleNames = SandBoxManager.Instance.ModuleManager.ModuleNames;
            for (int i = 0; i < (int) moduleNames.Length; i++)
            {
                string str = moduleNames[i];
                if (!____previouslyUsedModules.Contains(str))
                {
                    ____previouslyUsedModules.Add(str);
                }
            }

            if (Main.Settings!.ReEnableAchievementsInSaves)
            {
                ____previouslyUsedModules.RemoveAll((string x) => !_allowedModules.Contains(x)); 
            }
            return false;
        }
    }
}
