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
        private static IProfile CreateProfileFromProfileData(byte profileIndex, IProfileData profileData)
        {
            IRobot[] robots = new IRobot[profileData.Robots.Count];
            List<UpgradeObjectScript> upgrades = new List<UpgradeObjectScript>();
            Dictionary<string, DialogEventObjectScript> passed_dialog_events = new Dictionary<string, DialogEventObjectScript>();
            for (int robot_index = 0; robot_index < robots.Length; robot_index++)
            {
                RobotData robot = profileData.Robots[robot_index];
                if (robot != null)
                {
                    List<RobotPartObjectScript> robot_parts = new List<RobotPartObjectScript>();
                    foreach (string robot_part_name in robot.RobotParts)
                    {
                        string robot_part_resource_path = $"RobotParts/{ robot_part_name }";
                        RobotPartObjectScript robot_part = Resources.Load<RobotPartObjectScript>(robot_part_resource_path);
                        if (robot_part)
                        {
                            if (robot_parts.Contains(robot_part))
                            {
                                Debug.LogWarning($"Found duplicate robot part entry \"{ robot_part.name }\".");
                            }
                            else
                            {
                                robot_parts.Add(robot_part);
                            }
                        }
                        else
                        {
                            Debug.LogError($"Failed to load resource \"{ robot_part_resource_path }\".");
                        }
                    }
                    robots[robot_index] = new Robot(robot.ElapsedTimeSinceLastLubrication, robot.ElapsedTimeSinceLastRepair, robot_parts);
                    robot_parts.Clear();
                }
            }
            foreach (string upgrade_name in profileData.Upgrades)
            {
                string upgrade_resource_path = $"Upgrades/{ upgrade_name }";
                UpgradeObjectScript upgrade = Resources.Load<UpgradeObjectScript>(upgrade_resource_path);
                if (upgrade)
                {
                    if (upgrades.Contains(upgrade))
                    {
                        Debug.LogWarning($"Found duplicate upgrade entry \"{ upgrade.name }\".");
                    }
                    else
                    {
                        upgrades.Add(upgrade);
                    }
                }
                else
                {
                    Debug.LogError($"Failed to load resource \"{ upgrade_resource_path }\".");
                }
            }
            foreach (string passed_dialog_event_name in profileData.PassedDialogEvents)
            {
                string passed_dialog_event_resource_path = $"DialogEvents/{ passed_dialog_event_name }";
                DialogEventObjectScript dialog_event = Resources.Load<DialogEventObjectScript>(passed_dialog_event_resource_path);
                if (dialog_event)
                {
                    string key = dialog_event.name;
                    if (passed_dialog_events.ContainsKey(key))
                    {
                        Debug.LogWarning($"Found duplicate dialog event entry \"{ key }\".");
                    }
                    else
                    {
                        passed_dialog_events.Add(key, dialog_event);
                    }
                }
                else
                {
                    Debug.LogError($"Failed to load resource \"{ passed_dialog_event_resource_path }\".");
                }
            }
            IProfile ret = new Profile(profileIndex, profileData.Name, profileData.ProductionLevel, profileData.Money, robots, upgrades, passed_dialog_events.Values);
            upgrades.Clear();
            passed_dialog_events.Clear();
            return ret;
        }

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
                    ret = CreateProfileFromProfileData(profileIndex, profile_data);
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
                    ret = CreateProfileFromProfileData(profileIndex, profile_data);
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
