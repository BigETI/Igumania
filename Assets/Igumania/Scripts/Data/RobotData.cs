using Igumania.Objects;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Igumania.Data
{
    [Serializable]
    public class RobotData : IRobotData, ICloneable<RobotData>
    {
        [SerializeField]
        private float elapsedTimeSinceLastLubrication;

        [SerializeField]
        private float elapsedTimeSinceLastRepair;

        [SerializeField]
        private List<string> robotParts;

        public float ElapsedTimeSinceLastLubrication
        {
            get => elapsedTimeSinceLastLubrication;
            set => elapsedTimeSinceLastLubrication = Mathf.Max(value, 0.0f);
        }

        public float ElapsedTimeSinceLastRepair
        {
            get => elapsedTimeSinceLastRepair;
            set => elapsedTimeSinceLastRepair = Mathf.Max(value, 0.0f);
        }

        public IEnumerable<string> Parts => robotParts ??= new List<string>();

        public RobotData(float elapsedTimeSinceLastLubrication, float elapsedTimeSinceLastRepair, IEnumerable<string> robotParts)
        {
            if (robotParts == null)
            {
                throw new ArgumentNullException(nameof(robotParts));
            }
            if (elapsedTimeSinceLastLubrication < 0.0f)
            {
                Debug.LogError("Elapsed time since last lubrication can't be negative.");
                throw new ArgumentException("", nameof(elapsedTimeSinceLastLubrication));
            }
            if (elapsedTimeSinceLastRepair < 0.0f)
            {
                Debug.LogError("Elapsed time since last repair can't be negative.");
                throw new ArgumentException("", nameof(elapsedTimeSinceLastRepair));
            }
            this.robotParts = new List<string>(robotParts ?? throw new ArgumentNullException(nameof(robotParts)));
            this.elapsedTimeSinceLastLubrication = elapsedTimeSinceLastLubrication;
            this.elapsedTimeSinceLastRepair = elapsedTimeSinceLastRepair;
        }

        public void SetParts(IEnumerable<RobotPartObjectScript> robotParts)
        {
            if (this.robotParts == null)
            {
                throw new ArgumentNullException(nameof(RobotData.robotParts));
            }
            if (this.robotParts == null)
            {
                this.robotParts = new List<string>();
            }
            else
            {
                this.robotParts.Clear();
            }
            foreach (RobotPartObjectScript robot_part in robotParts)
            {
                if (robot_part)
                {
                    string key = robot_part.name;
                    if (this.robotParts.Contains(key))
                    {
                        Debug.LogWarning($"Found duplicate robot part \"{ key }\".");
                    }
                    else
                    {
                        this.robotParts.Add(key);
                    }
                }
                else
                {
                    Debug.LogError("Robot parts contains null.");
                }
            }
        }

        public RobotData Clone() => new RobotData(elapsedTimeSinceLastLubrication, elapsedTimeSinceLastRepair, (IEnumerable<string>)robotParts ?? Array.Empty<string>());
    }
}
