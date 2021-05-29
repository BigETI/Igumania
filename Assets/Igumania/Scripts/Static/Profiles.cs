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
                        foreach (string robot_part_name in robot.RobotParts)
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
                    List<IRobot> robots = new List<IRobot>();
                    List<UpgradeObjectScript> upgrades = new List<UpgradeObjectScript>();
                    foreach (RobotData robot in profile_data.Robots)
                    {
                        if (robot != null)
                        {
                            List<RobotPartObjectScript> robot_parts = new List<RobotPartObjectScript>();
                            foreach (string robot_part_name in robot.RobotParts)
                            {
                                if (string.IsNullOrWhiteSpace(robot_part_name))
                                {
                                    Debug.LogError("Robot contains invalid robot parts.");
                                }
                                else
                                {
                                    RobotPartObjectScript robot_part = Resources.Load<RobotPartObjectScript>($"RobotParts/{ robot_part_name }");
                                    if (robot_part)
                                    {
                                        robot_parts.Add(robot_part);
                                    }
                                    else
                                    {
                                        Debug.LogError($"Robot contains unknown robot part \"{ robot_part_name }\".");
                                    }
                                }

                            }
                            robots.Add(new Robot(robot.ElapsedTimeSinceLastLubrication, robot.ElapsedTimeSinceLastRepair, robot_parts));
                        }
                    }
                    foreach (string upgrade_name in profile_data.Upgrades)
                    {
                        if (string.IsNullOrWhiteSpace(upgrade_name))
                        {
                            Debug.LogError("Profile contains invalid upgrade.");
                        }
                        else
                        {
                            UpgradeObjectScript upgrade = Resources.Load<UpgradeObjectScript>($"Upgrades/{ upgrade_name }");
                            if (upgrade)
                            {
                                upgrades.Add(upgrade);
                            }
                            else
                            {
                                Debug.LogError($"Profile contains invalid upgrade \"{ upgrade_name }\".");
                            }
                        }
                    }
                    ret = new Profile(profileIndex, profile_data.Name, profile_data.ProductionLevel, profile_data.Money, robots, upgrades);
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
                if (ret)
                {
                    save_game.Save();
                }
            }
            return ret;
        }
    }
}
