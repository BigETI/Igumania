using Igumania.Data;
using Igumania.Objects;
using System.Collections.Generic;

namespace Igumania
{
    public interface IProfileData
    {
        string Name { get; set; }

        byte ProductionLevel { get; set; }

        long Money { get; set; }

        IReadOnlyList<string> Upgrades { get; }

        IReadOnlyList<RobotData> Robots { get; }

        IEnumerable<string> PassedDialogEvents { get; }

        void SetUpgrades(IReadOnlyList<UpgradeObjectScript> upgrades);

        void SetRobots(IReadOnlyList<IRobot> robots);
    }
}
