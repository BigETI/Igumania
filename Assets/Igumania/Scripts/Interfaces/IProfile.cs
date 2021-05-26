using Igumania.Objects;
using System.Collections.Generic;

namespace Igumania
{
    public interface IProfile
    {
        byte ProfileIndex { get; }

#if UNITY_EDITOR
        public bool IsFake { get; }
#endif

        string Name { get; }

        byte ProductionLevel { get; set; }

        long Money { get; set; }

        IReadOnlyList<IRobot> Robots { get; }

        IEnumerable<UpgradeObjectScript> Upgrades { get; }

        bool IsRobotAvailable(byte robotIndex);

        IRobot CreateNewRobot(byte robotIndex);

        IRobot GetRobot(byte robotIndex);

        void SetRobot(byte robotIndex, float elapsedTimeSinceLastLubrication, float elapsedTimeSinceLastRepair, IEnumerable<RobotPartObjectScript> robotParts);

        bool Save();
    }
}
