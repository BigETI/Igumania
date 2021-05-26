using Igumania.Objects;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Igumania
{
    internal class Robot : IRobot
    {
        private readonly Dictionary<string, RobotPartObjectScript> robotParts = new Dictionary<string, RobotPartObjectScript>();

        private float elapsedTimeSinceLastLubrication;

        private float elapsedTimeSinceLastRepair;

        public IEnumerable<RobotPartObjectScript> RobotParts => robotParts.Values;

        public float ElapsedTimeSinceLastLubrication
        {
            get => elapsedTimeSinceLastLubrication;
            set
            {
                if (value < 0.0f)
                {
                    throw new ArgumentException("Elapsed time since last lubrication can't be negative.", nameof(value));
                }
                elapsedTimeSinceLastLubrication = value;
            }
        }

        public float ElapsedTimeSinceLastRepair
        {
            get => elapsedTimeSinceLastRepair;
            set
            {
                if (value < 0.0f)
                {
                    throw new ArgumentException("Elapsed time since last repair can't be negative.", nameof(value));
                }
                elapsedTimeSinceLastRepair = value;
            }
        }

        public Robot(float elapsedTimeSinceLastLubrication, float elapsedTimeSinceLastRepair, IEnumerable<RobotPartObjectScript> robotParts)
        {
            if (elapsedTimeSinceLastLubrication < 0.0f)
            {
                throw new ArgumentException("Elapsed time since last lubrication can't be negative.", nameof(elapsedTimeSinceLastLubrication));
            }
            if (elapsedTimeSinceLastRepair < 0.0f)
            {
                throw new ArgumentException("Elapsed time since last lubrication can't be negative.", nameof(elapsedTimeSinceLastRepair));
            }
            ElapsedTimeSinceLastLubrication = elapsedTimeSinceLastLubrication;
            ElapsedTimeSinceLastRepair = elapsedTimeSinceLastRepair;
            foreach (RobotPartObjectScript robot_part in robotParts)
            {
                if (robot_part)
                {
                    string key = robot_part.name;
                    if (this.robotParts.ContainsKey(key))
                    {
                        Debug.LogWarning($"Found duplicate robot part entry \"{ key }\".");
                    }
                    else
                    {
                        this.robotParts.Add(robot_part.name, robot_part);
                    }
                }
            }
        }

        public bool IsRobotPartInstalled(RobotPartObjectScript robotPart)
        {
            if (!robotPart)
            {
                throw new ArgumentNullException(nameof(robotPart));
            }
            return robotParts.ContainsKey(robotPart.name);
        }

        public bool InstallRobotPart(RobotPartObjectScript robotPart)
        {
            if (!robotPart)
            {
                throw new ArgumentNullException(nameof(robotPart));
            }
            string key = robotPart.name;
            bool ret = !robotParts.ContainsKey(key);
            if (ret)
            {
                robotParts.Add(key, robotPart);
            }
            return ret;
        }

        public void SetRobotParts(IEnumerable<RobotPartObjectScript> robotParts)
        {
            if (robotParts == null)
            {
                throw new ArgumentNullException(nameof(robotParts));
            }
            this.robotParts.Clear();
            foreach (RobotPartObjectScript robot_part in robotParts)
            {
                if (robot_part)
                {
                    this.robotParts.Add(robot_part.name, robot_part);
                }
                else
                {
                    Debug.LogError("Robot parts contain null.");
                }
            }
        }

        public bool UninstallRobotPart(RobotPartObjectScript robotPart)
        {
            if (!robotPart)
            {
                throw new ArgumentNullException(nameof(robotPart));
            }
            return robotParts.Remove(robotPart.name);
        }

        public void UninstallAllRobotParts() => robotParts.Clear();
    }
}
