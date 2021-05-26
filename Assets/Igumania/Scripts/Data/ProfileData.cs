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

        public IEnumerable<string> Upgrades => upgrades ??= new List<string>();

        public ProfileData()
        {
            // ...
        }

        public ProfileData(string name, byte productionLevel, long money, IReadOnlyList<RobotData> robots, IEnumerable<string> upgrades)
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
                this.robots[robot_index] = (robot == null) ? null : new RobotData(robot.ElapsedTimeSinceLastLubrication, robot.ElapsedTimeSinceLastRepair, robot.Parts);
            }
            this.upgrades.AddRange(upgrades);
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
                    HashSet<string> robot_parts = new HashSet<string>();
                    foreach (RobotPartObjectScript robot_part in robot.RobotParts)
                    {
                        robot_parts.Add(robot_part.name);
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
