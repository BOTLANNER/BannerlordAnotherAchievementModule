
using HarmonyLib;

using SandBox;

using StoryMode;
using StoryMode.GameComponents.CampaignBehaviors;

using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;

namespace AnotherAchievementModule
{
    [HarmonyPatch(typeof(SandBoxSubModule))]
    static class SandBoxSubModulePatch
    {

        [HarmonyPostfix]
        [HarmonyPatch("InitializeGameStarter")]

        private static void InitializeGameStarter(Game game, IGameStarter gameStarterObject)
        {
            if (gameStarterObject is CampaignGameStarter campaignStarter && !(game.GameType is CampaignStoryMode) && Main.Settings!.AchievementsInSandbox)
            {
                campaignStarter.AddBehavior(new AchievementsCampaignBehavior()); 
            }
        }
    }
}
