using Igumania.Objects;
using System.Collections.Generic;

namespace Igumania
{
    public interface IRobot
    {
        float ElapsedTimeSinceLastLubrication { get; set; }

        float ElapsedTimeSinceLastRepair { get; set; }

        IEnumerable<RobotPartObjectScript> RobotParts { get; }

        bool IsRobotPartInstalled(RobotPartObjectScript robotPart);

        bool InstallRobotPart(RobotPartObjectScript robotPart);

        void SetRobotParts(IEnumerable<RobotPartObjectScript> robotParts);

        bool UninstallRobotPart(RobotPartObjectScript robotPart);

        void UninstallAllRobotParts();
    }
}
