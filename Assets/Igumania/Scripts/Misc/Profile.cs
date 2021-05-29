using Igumania.Data;
using Igumania.Objects;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnitySaveGame;

namespace Igumania
{
    internal class Profile : IProfile
    {
        private readonly List<UpgradeObjectScript> upgrades = new List<UpgradeObjectScript>();

        private IRobot[] robots = Array.Empty<IRobot>();

        public byte ProfileIndex { get; }

#if UNITY_EDITOR
        public bool IsFake { get; private set; }
#endif

        public string Name { get; }

        public byte ProductionLevel { get; set; }

        public long Money { get; set; }

        public IReadOnlyList<IRobot> Robots => robots;

        public IReadOnlyList<UpgradeObjectScript> Upgrades => upgrades;

        public event UpgradeInstalledDelegate OnUpgradeInstalled;

        public event UpgradeUninstalledDelegate OnUpgradeUninstalled;

        public Profile(byte profileIndex, string name, byte productionLevel, long money, IReadOnlyList<IRobot> robots, IReadOnlyList<UpgradeObjectScript> upgrades)
        {
            if (upgrades == null)
            {
                throw new ArgumentNullException(nameof(upgrades));
            }
            ProfileIndex = profileIndex;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            ProductionLevel = productionLevel;
            Money = money;
            if (robots.Count > 0)
            {
                this.robots = new IRobot[robots.Count];
                for (int robot_index = 0; robot_index < robots.Count; robot_index++)
                {
                    IRobot robot = robots[robot_index];
                    if (robot != null)
                    {
                        this.robots[robot_index] = new Robot(robot.ElapsedTimeSinceLastLubrication, robot.ElapsedTimeSinceLastRepair, robot.RobotParts);
                    }
                }
            }
            foreach (UpgradeObjectScript upgrade in upgrades)
            {
                if (upgrade)
                {
                    if (this.upgrades.Contains(upgrade))
                    {
                        Debug.LogWarning($"Found duplicate upgrade entry \"{ upgrade.name }\".");
                    }
                    else
                    {
                        this.upgrades.Add(upgrade);
                    }
                }
                else
                {
                    Debug.LogError("Upgrades contain null.");
                }
            }
        }

#if UNITY_EDITOR
        public static IProfile CreateNewFakeProfile()
        {
            return new Profile(0, "Fake", 0, 0L, Array.Empty<IRobot>(), Array.Empty<UpgradeObjectScript>())
            {
                IsFake = true
            };
        }
#endif

        public bool IsRobotAvailable(byte robotIndex) => (robotIndex < robots.Length) && (robots[robotIndex] != null);

        public IRobot CreateNewRobot(byte robotIndex)
        {
            robots ??= Array.Empty<IRobot>();
            if (robotIndex >= robots.Length)
            {
                Array.Resize(ref robots, robotIndex + 1);
            }
            return robots[robotIndex] ??= new Robot(0.0f, 0.0f, Array.Empty<RobotPartObjectScript>());
        }

        public IRobot GetRobot(byte robotIndex) => (robotIndex < robots.Length) ? robots[robotIndex] : null;

        public void SetRobot(byte robotIndex, float elapsedTimeSinceLastLubrication, float elapsedTimeSinceLastRepair, IReadOnlyList<RobotPartObjectScript> robotParts)
        {
            if (elapsedTimeSinceLastLubrication < 0.0f)
            {
                throw new ArgumentException("Elapsed time since last lubrication can't be negative.", nameof(elapsedTimeSinceLastLubrication));
            }
            if (elapsedTimeSinceLastRepair < 0.0f)
            {
                throw new ArgumentException("Elapsed time since last repair can't be negative.", nameof(elapsedTimeSinceLastRepair));
            }
            if (robotParts == null)
            {
                throw new ArgumentNullException(nameof(robotParts));
            }
            robots ??= Array.Empty<IRobot>();
            if (robotIndex >= robots.Length)
            {
                Array.Resize(ref robots, robotIndex + 1);
            }
            ref IRobot robot = ref robots[robotIndex];
            if (robot == null)
            {
                robot = new Robot(elapsedTimeSinceLastLubrication, elapsedTimeSinceLastRepair, robotParts);
            }
            else
            {
                robot.ElapsedTimeSinceLastLubrication = elapsedTimeSinceLastLubrication;
                robot.ElapsedTimeSinceLastRepair = elapsedTimeSinceLastRepair;
                robot.SetRobotParts(robotParts);
            }
        }

        public bool IsInstallingUpgradeAllowed(UpgradeObjectScript upgrade)
        {
            if (!upgrade)
            {
                throw new ArgumentNullException(nameof(upgrade));
            }
            bool ret = false;
            if (upgrade is RobotPartObjectScript robot_part)
            {
                foreach (IRobot robot in robots)
                {
                    if (robot != null)
                    {
                        if (robot.IsInstallingRobotPartAllowed(robot_part))
                        {
                            ret = true;
                            break;
                        }
                    }
                }
            }
            else if (!upgrades.Contains(upgrade))
            {
                ret = true;
                foreach (UpgradeObjectScript required_upgrade in upgrade.RequiredUpgrades)
                {
                    if (!upgrades.Contains(required_upgrade))
                    {
                        ret = false;
                        break;
                    }
                }
            }
            return ret;
        }

        public bool IsUpgradeInstalled(UpgradeObjectScript upgrade)
        {
            if (!upgrade)
            {
                throw new ArgumentNullException(nameof(upgrade));
            }
            return upgrades.Contains(upgrade);
        }

        public bool InstallUpgrade(UpgradeObjectScript upgrade)
        {
            if (!upgrade)
            {
                throw new ArgumentNullException(nameof(upgrade));
            }
            bool ret = !(upgrade is RobotPartObjectScript) && IsInstallingUpgradeAllowed(upgrade);
            if (ret)
            {
                upgrades.Add(upgrade);
                OnUpgradeInstalled?.Invoke(upgrade);
            }
            return ret;
        }

        public void SetUpgrades(IReadOnlyList<UpgradeObjectScript> upgrades)
        {
            if (upgrades == null)
            {
                throw new ArgumentNullException(nameof(upgrades));
            }
            UninstallAllUpgrades();
            foreach (UpgradeObjectScript upgrade in upgrades)
            {
                if (upgrade)
                {
                    if (this.upgrades.Contains(upgrade))
                    {
                        Debug.LogWarning($"Found duplicate upgrade entry \"{ upgrade.name }\".");
                    }
                    else
                    {
                        this.upgrades.Add(upgrade);
                        OnUpgradeInstalled?.Invoke(upgrade);
                    }
                }
                else
                {
                    Debug.LogError("Upgrades contain null.");
                }
            }
        }

        public bool UninstallUpgrade(UpgradeObjectScript upgrade)
        {
            if (!upgrade)
            {
                throw new ArgumentNullException(nameof(upgrade));
            }
            bool ret = upgrades.Remove(upgrade);
            if (ret && (OnUpgradeUninstalled != null))
            {
                OnUpgradeUninstalled.Invoke(upgrade);
            }
            return ret;
        }

        public void UninstallAllUpgrades()
        {
            if (OnUpgradeUninstalled != null)
            {
                foreach (UpgradeObjectScript upgrade in upgrades)
                {
                    OnUpgradeUninstalled.Invoke(upgrade);
                }
            }
            upgrades.Clear();
        }

        public bool Save()
        {
            bool ret = false;
#if UNITY_EDITOR
            if (!IsFake)
            {
#endif
                SaveGame<SaveGameData> save_game = SaveGames.Get<SaveGameData>();
                if (save_game != null)
                {
                    RobotData[] robots = new RobotData[this.robots.Length];
                    string[] upgrade_names = new string[upgrades.Count];
                    for (int robot_index = 0; robot_index < robots.Length; robot_index++)
                    {
                        IRobot robot = this.robots[robot_index];
                        if (robot != null)
                        {
                            List<string> robot_parts = new List<string>();
                            foreach (RobotPartObjectScript robot_part in robot.RobotParts)
                            {
                                robot_parts.Add(robot_part ? robot_part.name : string.Empty);
                            }
                            robots[robot_index] = (robot == null) ? null : new RobotData(robot.ElapsedTimeSinceLastLubrication, robot.ElapsedTimeSinceLastRepair, robot_parts);
                            robot_parts.Clear();
                        }
                    }
                    for (int upgrade_name_index = 0; upgrade_name_index < upgrade_names.Length; upgrade_name_index++)
                    {
                        upgrade_names[upgrade_name_index] = upgrades[upgrade_name_index].name;
                    }
                    save_game.Data.WriteProfile(ProfileIndex, Name, ProductionLevel, Money, robots, upgrade_names);
                    ret = save_game.Save();
                }
#if UNITY_EDITOR
            }
#endif
            return ret;
        }
    }
}
