using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Steamworks;

using TaleWorlds.AchievementSystem;
using TaleWorlds.Library;
using TaleWorlds.PlatformService.Steam;

using static TaleWorlds.Library.CommandLineFunctionality;

namespace AnotherAchievementModule
{
    public class ConsoleCommands
    {

        static Dictionary<string, Dictionary<string, int>> achievements = new()
        {
            { "Against all odds", new Dictionary<string, int> { { "DefeatedSuperiorForce", 1 } } },
            { "Apple of my eye", new Dictionary<string, int> { { "NumberOfChildrenBorn", 1 } } },
            { "Bannerlord", new Dictionary<string, int> { { "AssembledDragonBanner", 1 } } },
            { "Best served cold", new Dictionary<string, int> { { "BestServedCold", 1 } } },
            { "Butcher of Calradia", new Dictionary<string, int> { { "KillCountCaptain", 10000 } } },
            { "Butterlord", new Dictionary<string, int> { { "ButtersInInventoryCount", 100 } } },
            { "Catch", new Dictionary<string, int> { { "KillsWithBoulder", 1 } } },
            { "Crackshot", new Dictionary<string, int> { { "FarthestHeadShot", 200 } } },
            { "Crowdfunded", new Dictionary<string, int> { { "WonTournamentCount", 100 } } },
            { "Crush your enemies", new Dictionary<string, int> { { "DefeatedTroopCount", 10000 } } },
            { "Duelist", new Dictionary<string, int> { { "RadagosDefeatedInDuel", 1 } } },
            { "Dynasty", new Dictionary<string, int> { { "ReachedClanTierSix", 1 } } },
            { "Entrepreneur", new Dictionary<string, int> { { "HasOwnedCaravanAndWorkshop", 1 } } },
            { "Explorer", new Dictionary<string, int> { { "EnteredEverySettlement", 1 } } },
            { "Fat Cat", new Dictionary<string, int> { { "TotalTradeProfit", 1000000 } } },
            { "Freedom!", new Dictionary<string, int> { { "BarbarianVictory", 1 } } },
            { "God of the Arena", new Dictionary<string, int> { { "LeaderOfTournament", 1 } } },
            { "Great Granny", new Dictionary<string, int> { { "GreatGranny", 1 } } },
            { "Head hunter", new Dictionary<string, int> { { "KillsWithRangedHeadshots", 100 } } },
            { "Heartbreaker", new Dictionary<string, int> { { "Hearthbreaker", 1 } } },
            { "Horde breaker", new Dictionary<string, int> { { "DefeatedArmyWhileAloneCount", 1 } } },
            { "I can do it", new Dictionary<string, int> { { "CapturedATownAloneCount", 1 } } },
            { "I spit on your grave", new Dictionary<string, int> { { "ExecutedLordRelation100", 1 } } },
            { "Jack of All Trades", new Dictionary<string, int> { { "SatisfiedJackOfAllTrades", 1 } } },
            { "King Solomon", new Dictionary<string, int> { { "MaxDailyIncome", 10000 } } },
            { "Kingslayer", new Dictionary<string, int> { { "KingOrQueenKilledInBattle", 1 } } },
            { "Know your enemy", new Dictionary<string, int> { { "SuccessfulBattlesAgainstArmyCount", 100 } } },
            { "Lance-a-lot", new Dictionary<string, int> { { "KillsWithCouchedLance", 500 } } },
            { "Landlord", new Dictionary<string, int> { { "OwnedFortificationCount", 1 } } },
            { "Lawbringer", new Dictionary<string, int> { { "ClearedHideoutCount", 1 } } },
            { "Lawmaker", new Dictionary<string, int> { { "ProposedAndWonAPolicy", 1 } } },
            { "Long live the Empire", new Dictionary<string, int> { { "ImperialVictory", 1 } } },
            { "Mastery", new Dictionary<string, int> { { "HighestSkillValue", 300 } } },
            { "Minor Clan", new Dictionary<string, int> { { "NumberOfChildrenBorn", 7 } } },
            { "Mounted Archery", new Dictionary<string, int> { { "KillsWithRangedMounted", 500 } } },
            { "My way", new Dictionary<string, int> { { "CreatedKingdomCount", 1 } } },
            { "Real Estate", new Dictionary<string, int> { { "SuccessfulSiegeCount", 100 } } },
            { "Ride it like you stole it", new Dictionary<string, int> { { "StoleHorseFromAliveEnemy", 1 } } },
            { "Roadkill", new Dictionary<string, int> { { "KillsWithHorseCharge", 100 } } },
            { "Slice n dice", new Dictionary<string, int> { { "KillsWithChainAttack", 10 } } },
            { "Strike!", new Dictionary<string, int> { { "MaxMultiKillsWithSingleMangonelShot", 3 } } },
            { "Supreme Emperor", new Dictionary<string, int> { { "OwnedFortificationCount", 120 } } },
            { "Swordbearer", new Dictionary<string, int> { { "HighestTierSwordCrafted", 6 } } },
            { "The king is pleased", new Dictionary<string, int> { { "MaxDailyTributeGain", 1000 } } },
            { "This Is Our Land", new Dictionary<string, int> { { "RepelledSiegeAssaultCount", 1 } } },
            { "This is Sparta!", new Dictionary<string, int> { { "PushedSomeoneOffLedge", 1 } } },
            { "Trained", new Dictionary<string, int> { { "FinishedTutorial", 1 } } },
            { "Undercover", new Dictionary<string, int> { { "CompletedAnIssueInHostileTown", 1 } } },
            { "Veni vidi vici", new Dictionary<string, int> { { "ClansUnderPlayerKingdomCount", 21 } } },
            { "What have the Romans ever done for us", new Dictionary<string, int> { { "CompletedAllProjects", 1 } } },
        };

        [CommandLineArgumentFunction("help", "another_achievement")]
        public static string Help(List<string> args)
        {
            string helpResponse = @"
Commands:

another_achievement.help                                       - Shows console command usage
another_achievement.unlockall                                  - Unlocks all achievements
another_achievement.list                                       - List all achievement names for use with another_achievement.unlock or another_achievement.setstat
another_achievement.unlock      achievementName                - Unlocks specified achievement (Call another_achievement.list for all supported achievements)
another_achievement.remove      achievementName                - [STEAM ONLY SUPPORTED] Re-locks specified achievement (Call another_achievement.list for all supported achievements)
another_achievement.setstat     achievementName statValue      - Sets specified achievement value (Only if value is greater than current value, call another_achievement.list for all supported achievements)
another_achievement.getstat     achievementName                - Gets specified achievement value (Call another_achievement.list for all supported achievements)
another_achievement.liststats                                  - Lists all achievement stat values and unlock status


";
            //Task<UserStatsReceived_t> ListStats()
            //{
            //    return Task.Run(() =>
            //    {
            //        var t = new TaskCompletionSource<UserStatsReceived_t>();
            //        var _userStatsReceivedT = Callback<UserStatsReceived_t>.Create(new Callback<UserStatsReceived_t>.DispatchDelegate((UserStatsReceived_t param) =>
            //        {
            //            t.TrySetResult(param);
            //        }));
            //        SteamUserStats.RequestCurrentStats();

            //        return t.Task;
            //    });
            //}

            //var stats = ListStats().Result;
            //stats.m_eResult.
            //var achievementsCount = stee.rec;
            //for (uint i = 0; i < achievementsCount; i++)
            //{
            //    helpResponse += "\r\n";
            //    helpResponse += SteamUserStats.GetAchievementName(i);
            //}

            return helpResponse;
        }

        //[CommandLineArgumentFunction("enable", "another_achievement")]
        //public static string Enable(List<string> args)
        //{
        //    if (!Main.Settings!.Achievements)
        //    {
        //        Main.Settings!.Achievements = true;
        //        return "Achievements enabled";
        //    }
        //    return "Achievements already enabled";
        //}

        //[CommandLineArgumentFunction("disable", "another_achievement")]
        //public static string Disable(List<string> args)
        //{
        //    if (Main.Settings!.Achievements)
        //    {
        //        Main.Settings!.Achievements = false;
        //        return "Achievements disabled";
        //    }
        //    return "Achievements already disabled";
        //}

        [CommandLineArgumentFunction("liststats", "another_achievement")]
        public static string ListStats(List<string> args)
        {
            try
            {
                if (Main.Settings!.Achievements)
                {
                    string result = "";
                    foreach (var item in achievements)
                    {
                        result += "\r\n";
                        string achievement = item.Key;
                        bool steamUnlocked = false;
                        try
                        {
                            if (AchievementManager.AchievementService is SteamAchievementService steamAchievementService)
                            {
                                SteamUserStats.GetAchievement(achievement, out steamUnlocked);
                            }
                        }
                        catch (System.Exception e)
                        {
                            //Ignore
                        }

                        var statRequirements = achievements[achievement];
                        foreach (var stat in statRequirements)
                        {
                            var currentValue = AchievementManager.GetStat(stat.Key).Result;
                            result += $"{achievement} - {( steamUnlocked || currentValue >= stat.Value ? "Unlocked" : stat.Value == 1 ? "Locked" : currentValue.ToString())}";
                            result += "\r\n";
                        }
                    }
                    return result;
                }
                return "Achievements are disabled";
            }
            catch (System.Exception e)
            {
                return e.ToString();
            }


        }

        [CommandLineArgumentFunction("list", "another_achievement")]
        public static string List(List<string> args)
        {
            try
            {
                if (Main.Settings!.Achievements)
                {
                    string result = "";
                    foreach (var item in achievements)
                    {
                        result += "\r\n";
                        result += item.Key;
                    }
                    return result;
                }
                return "Achievements are disabled";
            }
            catch (System.Exception e)
            {
                return e.ToString();
            }


        }

        [CommandLineArgumentFunction("unlock", "another_achievement")]
        public static string Unlock(List<string> args)
        {
            try
            {
                if (Main.Settings!.Achievements)
                {
                    if (args.Count > 0)
                    {
                        string achievement = ArgsToString(args).Replace("\"", "");

                        return UpdateInternal(achievement);
                    }
                    else
                    {
                        return Help(args);
                    }
                }
                return "Achievements are disabled";
            }
            catch (System.Exception e)
            {
                return e.ToString();
            }


        }

        [CommandLineArgumentFunction("remove", "another_achievement")]
        public static string Remove(List<string> args)
        {
            try
            {
                if (Main.Settings!.Achievements)
                {
                    if (args.Count > 0)
                    {
                        string achievement = ArgsToString(args).Replace("\"", "");

                        return UpdateInternal(achievement, true);
                    }
                    else
                    {
                        return Help(args);
                    }
                }
                return "Achievements are disabled";
            }
            catch (System.Exception e)
            {
                return e.ToString();
            }


        }

        [CommandLineArgumentFunction("setstat", "another_achievement")]
        public static string SetStat(List<string> args)
        {
            try
            {
                if (Main.Settings!.Achievements)
                {
                    if (args.Count > 1)
                    {
                        string achievement = ArgsToString(args.Take(args.Count - 1).ToList()).Replace("\"", "");
                        string stringValue = args[args.Count - 1];
                        if (!int.TryParse(stringValue, out var value))
                        {
                            return $"Value '{stringValue}' is not a valid number!";
                        }


                        if (!achievements.TryGetValue(achievement, out var statRequirements))
                        {
                            return $"Achievement '{achievement}' not found!";
                        }
                        bool failures = false;
                        foreach (var item in statRequirements)
                        {
                            var currentValue = AchievementManager.GetStat(item.Key).Result;
                            if (currentValue >= value)
                            {
                                return $"Achievement stat is already: {currentValue}. Ignoring command";
                            }
                            failures = !AchievementManager.SetStat(item.Key, value) || failures;
                        }
                        if (failures)
                        {
                            return $"Encountered failures when attempting to set achievement '{achievement}' to stat '{value}'";
                        }
                        return $"Set achievement '{achievement}' to stat '{value}'";
                    }
                    else
                    {
                        return Help(args);
                    }
                }
                return "Achievements are disabled";
            }
            catch (System.Exception e)
            {
                return e.ToString();
            }

        }

        [CommandLineArgumentFunction("getstat", "another_achievement")]
        public static string GetStat(List<string> args)
        {
            try
            {
                if (Main.Settings!.Achievements)
                {
                    if (args.Count > 0)
                    {
                        string achievement = ArgsToString(args).Replace("\"", "");

                        if (!achievements.TryGetValue(achievement, out var statRequirements))
                        {
                            return $"Achievement '{achievement}' not found!";
                        }

                        foreach (var item in statRequirements)
                        {
                            var currentValue = AchievementManager.GetStat(item.Key).Result;
                            if (currentValue >= item.Value)
                            {
                                return $"Achievement '{achievement}' is: 'Unlocked'";
                            }
                            else if (item.Value == 1)
                            {
                                return $"Achievement '{achievement}' is: 'Locked'";
                            }
                            return $"Achievement '{achievement}' stat is: '{currentValue}'";
                        }
                    }
                    else
                    {
                        return Help(args);
                    }
                }
                return "Achievements are disabled";
            }
            catch (System.Exception e)
            {
                return e.ToString();
            }
        }

        [CommandLineArgumentFunction("unlockall", "another_achievement")]
        public static string UnlockAll(List<string> args)
        {
            try
            {
                if (Main.Settings!.Achievements)
                {
                    string result = "";
                    foreach (var item in achievements)
                    {
                        result += "\r\n";
                        try
                        {
                            result += UpdateInternal(item.Key);
                        }
                        catch (System.Exception e)
                        {
                            result += e.ToString();
                        }
                    }
                    return result;
                }
                return "Achievements are disabled";
            }
            catch (System.Exception e)
            {
                return e.ToString();
            }


        }

        private static string UpdateInternal(string achievement, bool remove = false)
        {
            try
            {
                bool success = false;
                if (remove)
                {
                    if (!achievements.TryGetValue(achievement, out var statRequirements))
                    {
                        return $"Achievement '{achievement}' not found!";
                    }
                    foreach (var item in statRequirements)
                    {
                        success = AchievementManager.SetStat(item.Key, 0);
                    }
                    try
                    {
                        if (AchievementManager.AchievementService is SteamAchievementService steamAchievementService)
                        {
                            success = SteamUserStats.ClearAchievement(achievement);
                            if (success && SteamUserStats.StoreStats())
                            {
                                if (SteamUserStats.GetAchievement(achievement, out bool achieved) && achieved)
                                {
                                    Debug.WriteDebugLineOnScreen($"Steam achievement '{achievement}' appears to still be unlocked!");
                                }
                            }
                        }
                    }
                    catch (System.Exception e)
                    {
                        Debug.WriteDebugLineOnScreen($"Steam direct reset for '{achievement}' failed! \r\n{e.ToString()}");
                    }


                    if (!success)
                    {
                        return $"Encountered failures when attempting to reset achievement: {achievement}";
                    }

                    return $"Reset achievement: {achievement}";
                }

                if (!success)
                {

                    if (!achievements.TryGetValue(achievement, out var statRequirements))
                    {
                        return $"Achievement '{achievement}' not found!";
                    }
                    foreach (var item in statRequirements)
                    {
                        success = AchievementManager.SetStat(item.Key, item.Value);
                    }
                }

                if (!success)
                {
                    try
                    {
                        if (AchievementManager.AchievementService is SteamAchievementService steamAchievementService)
                        {
                            Debug.WriteDebugLineOnScreen($"Failed to unlock '{achievement}' using stats, attempting to directly unlock on Steam");
                            success = SteamUserStats.SetAchievement(achievement);
                            if (success && SteamUserStats.StoreStats())
                            {
                                if (!SteamUserStats.GetAchievement(achievement, out bool achieved) || !achieved)
                                {
                                    Debug.WriteDebugLineOnScreen($"Steam achievement '{achievement}' appears to still be locked!");
                                }
                            }
                        }
                    }
                    catch (System.Exception e)
                    {
                        Debug.WriteDebugLineOnScreen($"Steam direct unlock for '{achievement}' failed! \r\n{e.ToString()}");
                    }
                }


                if (!success)
                {
                    return $"Encountered failures when attempting to unlock achievement: {achievement}";
                }

                return $"Unlocked achievement: {achievement}";
            }
            catch (System.Exception e)
            {
                return e.ToString();
            }
        }

        private static string ArgsToString(List<string> args)
        {
            return string.Join(" ", args).Trim();
        }
    }
}
