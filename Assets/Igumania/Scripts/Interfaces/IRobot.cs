using Igumania.Objects;
using System.Collections.Generic;

namespace Igumania
{
    public interface IRobot
    {
        float ElapsedTimeSinceLastLubrication { get; set; }

        float ElapsedTimeSinceLastRepair { get; set; }

        IReadOnlyList<RobotPartObjectScript> RobotParts { get; }

        event RobotPartInstalledDelegate OnRobotPartInstalled;

        event RobotPartUninstalledDelegate OnRobotPartUninstalled;

        bool IsInstallingRobotPartAllowed(RobotPartObjectScript robotPart);

        bool IsRobotPartInstalled(RobotPartObjectScript robotPart);

        bool InstallRobotPart(RobotPartObjectScript robotPart);

        void SetRobotParts(IReadOnlyList<RobotPartObjectScript> robotParts);

        bool UninstallRobotPart(RobotPartObjectScript robotPart);

        void UninstallAllRobotParts();
    }
}
