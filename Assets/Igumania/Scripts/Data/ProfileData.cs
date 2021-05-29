using Igumania.Objects;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Igumania.Data
{
    [Serializable]
    public class ProfileData : IProfileData, ICloneable<ProfileData>
    {
        [SerializeField]
        private string name;

        [SerializeField]
        private byte productionLevel;

        [SerializeField]
        private long money;

        [SerializeField]
        private RobotData[] robots;

        [SerializeField]
        private List<string> upgrades;

        public string Name
        {
            get => name ?? string.Empty;
            set => name = value ?? throw new ArgumentNullException(nameof(value));
        }

        public byte ProductionLevel
        {
            get => productionLevel;
            set => productionLevel = value;
        }

        public long Money
        {
            get => money;
            set => money = value;
        }

        public IReadOnlyList<RobotData> Robots => robots ?? Array.Empty<RobotData>();

        public IReadOnlyList<string> Upgrades => upgrades ??= new List<string>();

        public ProfileData()
        {
            // ...
        }

        public ProfileData(string name, byte productionLevel, long money, IReadOnlyList<RobotData> robots, IReadOnlyList<string> upgrades)
        {
            if (robots == null)
            {
                throw new ArgumentNullException(nameof(robots));
            }
            if (upgrades == null)
            {
                throw new ArgumentNullException(nameof(upgrades));
            }
            this.name = name ?? throw new ArgumentNullException(nameof(name));
            this.productionLevel = productionLevel;
            this.money = money;
            this.robots = new RobotData[robots.Count];
            for (int robot_index = 0; robot_index < robots.Count; robot_index++)
            {
                RobotData robot = robots[robot_index];
                this.robots[robot_index] = (robot == null) ? null : new RobotData(robot.ElapsedTimeSinceLastLubrication, robot.ElapsedTimeSinceLastRepair, robot.RobotParts);
            }
            this.upgrades = new List<string>(upgrades);
        }

        public void SetUpgrades(IReadOnlyList<UpgradeObjectScript> upgrades)
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
                    string key = upgrade.name;
                    if (this.upgrades.Contains(key))
                    {
                        Debug.LogWarning($"Found duplicate upgrade entry \"{ key }\".");
                    }
                    else
                    {
                        this.upgrades.Add(key);
                    }
                }
                else
                {
                    Debug.LogError("Upgrades contains null.");
                }
            }
        }

        public void SetRobots(IReadOnlyList<IRobot> robots)
        {
            if (robots == null)
            {
                throw new ArgumentNullException(nameof(robots));
            }
            if ((this.robots == null) || (this.robots.Length != robots.Count))
            {
                this.robots = new RobotData[robots.Count];
            }
            for (int robot_index = 0; robot_index < robots.Count; robot_index++)
            {
                IRobot robot = robots[robot_index];
                if (robot == null)
                {
                    this.robots[robot_index] = null;
                }
                else
                {
                    List<string> robot_parts = new List<string>();
                    foreach (RobotPartObjectScript robot_part in robot.RobotParts)
                    {
                        string robot_part_name = robot_part.name;
                        if (robot_parts.Contains(robot_part_name))
                        {
                            Debug.LogWarning($"Found duplicate robot part entry \"{ robot_part_name }\".");
                        }
                        else
                        {
                            robot_parts.Add(robot_part.name);
                        }
                    }
                    this.robots[robot_index] = new RobotData(robot.ElapsedTimeSinceLastLubrication, robot.ElapsedTimeSinceLastRepair, robot_parts);
                    robot_parts.Clear();
                }
            }
        }

        public ProfileData Clone()
        {
            RobotData[] robots = Array.Empty<RobotData>();
            if (this.robots != null)
            {
                for (int robot_index = 0; robot_index < robots.Length; robot_index++)
                {
                    RobotData robot = robots[robot_index];
                    this.robots[robot_index] = robot?.Clone();
                }
            }
            return new ProfileData(Name, productionLevel, money, robots, upgrades);
        }
    }
}
