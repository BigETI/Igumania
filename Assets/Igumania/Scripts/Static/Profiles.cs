using Igumania.Data;
using Igumania.Objects;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnitySaveGame;

namespace Igumania
{
    public static class Profiles
    {
        public static bool IsProfileAvailable(byte profileIndex)
        {
            bool ret = false;
            SaveGame<SaveGameData> save_game = SaveGames.Get<SaveGameData>();
            if (save_game.Data != null)
            {
                ret = save_game.Data.IsProfileAvailable(profileIndex);
            }
            return ret;
        }

        public static IProfile CreateNewProfile(byte profileIndex, string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            IProfile ret = null;
            SaveGame<SaveGameData> save_game = SaveGames.Get<SaveGameData>();
            if (save_game.Data != null)
            {
                IProfileData profile_data = save_game.Data.CreateNewProfile(profileIndex, name);
                if (profile_data != null)
                {
                    IRobot[] robots = new IRobot[profile_data.Robots.Count];
                    List<UpgradeObjectScript> upgrades = new List<UpgradeObjectScript>();
                    for (int robot_index = 0; robot_index < robots.Length; robot_index++)
                    {
                        RobotData robot = profile_data.Robots[robot_index];
                        List<RobotPartObjectScript> robot_parts = new List<RobotPartObjectScript>();
                        foreach (string robot_part_name in robot.Parts)
                        {
                            string robot_part_resource_path = $"RobotParts/{ robot_part_name }";
                            RobotPartObjectScript robot_part = Resources.Load<RobotPartObjectScript>(robot_part_resource_path);
                            if (robot_part)
                            {
                                robot_parts.Add(robot_part);
                            }
                            else
                            {
                                Debug.LogError($"Failed to load resource \"{ robot_part_resource_path }\".");
                            }
                        }
                        robots[robot_index] = (robot == null) ? null : new Robot(robot.ElapsedTimeSinceLastLubrication, robot.ElapsedTimeSinceLastRepair, robot_parts);
                        robot_parts.Clear();
                    }
                    foreach (string upgrade_name in profile_data.Upgrades)
                    {
                        string upgrade_resource_path = $"Upgrades/{ upgrade_name }";
                        RobotPartObjectScript upgrade = Resources.Load<RobotPartObjectScript>(upgrade_resource_path);
                        if (upgrade)
                        {
                            upgrades.Add(upgrade);
                        }
                        else
                        {
                            Debug.LogError($"Failed to load resource \"{ upgrade_resource_path }\".");
                        }
                    }
                    ret = new Profile(profileIndex, profile_data.Name, profile_data.ProductionLevel, profile_data.Money, robots, upgrades);
                    upgrades.Clear();
                    ret.Save();
                }
            }
            return ret;
        }

        public static IProfile LoadProfile(byte profileIndex)
        {
            IProfile ret = null;
            SaveGame<SaveGameData> save_game = SaveGames.Get<SaveGameData>();
            if (save_game.Data != null)
            {
                IProfileData profile_data = save_game.Data.LoadProfile(profileIndex);
                if (profile_data != null)
                {
                    ret = new Profile(profileIndex, profile_data.Name, profile_data.ProductionLevel, profile_data.Money, Array.Empty<IRobot>(), Array.Empty<UpgradeObjectScript>());
                }
            }
            return ret;
        }

        public static bool RemoveProfile(byte profileIndex)
        {
            bool ret = false;
            SaveGame<SaveGameData> save_game = SaveGames.Get<SaveGameData>();
            if (save_game.Data != null)
            {
                ret = save_game.Data.RemoveProfile(profileIndex);
            }
            return ret;
        }
    }
}
