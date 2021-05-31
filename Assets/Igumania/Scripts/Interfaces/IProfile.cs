using Igumania.Objects;
using System.Collections.Generic;

namespace Igumania
{
    public interface IProfile
    {
        byte ProfileIndex { get; }

#if UNITY_EDITOR
        bool IsFake { get; }
#endif

        string Name { get; }

        byte ProductionLevel { get; set; }

        long Money { get; set; }

        IReadOnlyList<IRobot> Robots { get; }

        IReadOnlyList<UpgradeObjectScript> Upgrades { get; }

        event UpgradeInstalledDelegate OnUpgradeInstalled;

        event UpgradeUninstalledDelegate OnUpgradeUninstalled;

        event RobotEnabledDelegate OnRobotEnabled;

        event RobotDisabledDelegate OnRobotDisabled;

        bool IsInstallingUpgradeAllowed(UpgradeObjectScript upgrade);

        bool IsUpgradeInstalled(UpgradeObjectScript upgrade);

        bool InstallUpgrade(UpgradeObjectScript upgrade);

        void SetUpgrades(IReadOnlyList<UpgradeObjectScript> upgrades);

        bool UninstallUpgrade(UpgradeObjectScript upgrade);

        void UninstallAllUpgrades();

        bool IsRobotAvailable(byte robotIndex);

        IRobot CreateNewRobot(byte robotIndex);

        IRobot GetRobot(byte robotIndex);

        void SetRobot(byte robotIndex, float elapsedTimeSinceLastLubrication, float elapsedTimeSinceLastRepair, IReadOnlyList<RobotPartObjectScript> robotParts);

        bool IsDialogEventPassed(DialogEventObjectScript passedDialogEvent);

        bool AddPassedDialogEvent(DialogEventObjectScript passedDialogEvent);

        void SetPassedDialogEvents(IEnumerable<DialogEventObjectScript> passedDialogEvents);

        bool RemovePassedDialogEvent(DialogEventObjectScript passedDialogEvent);

        void ClearPassedDialogEvents();

        bool Save();
    }
}
