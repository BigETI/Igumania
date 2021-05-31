using Igumania.Objects;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Igumania
{
    internal class Robot : IRobot
    {
        private readonly List<RobotPartObjectScript> robotParts = new List<RobotPartObjectScript>();

        private float elapsedTimeSinceLastLubrication;

        private float elapsedTimeSinceLastRepair;

        public IReadOnlyList<RobotPartObjectScript> RobotParts => robotParts;

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

        public event RobotPartInstalledDelegate OnRobotPartInstalled;

        public event RobotPartUninstalledDelegate OnRobotPartUninstalled;

        public Robot(float elapsedTimeSinceLastLubrication, float elapsedTimeSinceLastRepair, IReadOnlyList<RobotPartObjectScript> robotParts)
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
                    if (this.robotParts.Contains(robot_part))
                    {
                        Debug.LogWarning($"Found duplicate robot part entry \"{ robot_part.name }\".");
                    }
                    else
                    {
                        this.robotParts.Add(robot_part);
                    }
                }
            }
        }

        public bool IsInstallingRobotPartAllowed(RobotPartObjectScript robotPart)
        {
            if (!robotPart)
            {
                throw new ArgumentNullException(nameof(robotPart));
            }
            bool ret = false;
            if (!robotParts.Contains(robotPart))
            {
                ret = true;
                foreach (RobotPartObjectScript required_robot_part in robotPart.RequiredUpgrades)
                {
                    if (!robotParts.Contains(required_robot_part))
                    {
                        ret = false;
                        break;
                    }
                }
            }
            return ret;
        }

        public bool IsRobotPartInstalled(RobotPartObjectScript robotPart)
        {
            if (!robotPart)
            {
                throw new ArgumentNullException(nameof(robotPart));
            }
            return robotParts.Contains(robotPart);
        }

        public bool InstallRobotPart(RobotPartObjectScript robotPart)
        {
            if (!robotPart)
            {
                throw new ArgumentNullException(nameof(robotPart));
            }
            bool ret = IsInstallingRobotPartAllowed(robotPart);
            if (ret)
            {
                robotParts.Add(robotPart);
                OnRobotPartInstalled?.Invoke(robotPart);
            }
            return ret;
        }

        public void SetRobotParts(IReadOnlyList<RobotPartObjectScript> robotParts)
        {
            if (robotParts == null)
            {
                throw new ArgumentNullException(nameof(robotParts));
            }
            UninstallAllRobotParts();
            foreach (RobotPartObjectScript robot_part in robotParts)
            {
                if (robot_part)
                {
                    if (this.robotParts.Contains(robot_part))
                    {
                        Debug.LogWarning($"Found duplicate robot part entry \"{ robot_part.name }\".");
                    }
                    else
                    {
                        this.robotParts.Add(robot_part);
                        OnRobotPartInstalled?.Invoke(robot_part);
                    }
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
            bool ret = robotParts.Remove(robotPart);
            if (ret && (OnRobotPartUninstalled != null))
            {
                OnRobotPartUninstalled.Invoke(robotPart);
            }
            return ret;
        }

        public void UninstallAllRobotParts()
        {
            if (OnRobotPartUninstalled != null)
            {
                foreach (RobotPartObjectScript robot_part in robotParts)
                {
                    OnRobotPartUninstalled.Invoke(robot_part);
                }
            }
            robotParts.Clear();
        }
    }
}
