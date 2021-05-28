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
        private readonly Dictionary<string, UpgradeObjectScript> upgrades = new Dictionary<string, UpgradeObjectScript>();

        private IRobot[] robots = Array.Empty<IRobot>();

        public byte ProfileIndex { get; }

#if UNITY_EDITOR
        public bool IsFake { get; private set; }
#endif

        public string Name { get; }

        public byte ProductionLevel { get; set; }

        public long Money { get; set; }

        public IReadOnlyList<IRobot> Robots => robots;

        public IEnumerable<UpgradeObjectScript> Upgrades => upgrades.Values;

        public Profile(byte profileIndex, string name, byte productionLevel, long money, IReadOnlyList<IRobot> robots, IEnumerable<UpgradeObjectScript> upgrades)
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
                    this.upgrades.Add(upgrade.name, upgrade);
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

        public void SetRobot(byte robotIndex, float elapsedTimeSinceLastLubrication, float elapsedTimeSinceLastRepair, IEnumerable<RobotPartObjectScript> robotParts)
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

        public bool IsUpgradeInstalled(UpgradeObjectScript upgrade)
        {
            if (!upgrade)
            {
                throw new ArgumentNullException(nameof(upgrade));
            }
            return upgrades.ContainsKey(upgrade.name);
        }

        public bool InstallUpgrade(UpgradeObjectScript upgrade)
        {
            if (!upgrade)
            {
                throw new ArgumentNullException(nameof(upgrade));
            }
            string key = upgrade.name;
            bool ret = !upgrades.ContainsKey(key);
            if (ret)
            {
                upgrades.Add(key, upgrade);
            }
            return ret;
        }

        public void SetUpgrades(IEnumerable<UpgradeObjectScript> upgrades)
        {
            if (upgrades == null)
            {
                throw new ArgumentNullException(nameof(upgrades));
            }
            this.upgrades.Clear();
            foreach (UpgradeObjectScript upgrade in upgrades)
            {
                if (upgrade)
                {
                    this.upgrades.Add(upgrade.name, upgrade);
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
            return upgrades.Remove(upgrade.name);
        }

        public void UninstallAllUpgrades() => upgrades.Clear();

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
                    save_game.Data.WriteProfile(ProfileIndex, Name, ProductionLevel, Money, robots, upgrades.Keys);
                    ret = save_game.Save();
                }
#if UNITY_EDITOR
            }
#endif
            return ret;
        }
    }
}
