using Igumania.Objects;

namespace Igumania
{
    public interface IVisualUpgradeData
    {
        UpgradeObjectScript Upgrade { get; }

        event UpgradeInstalledDelegate OnUpgradeInstalled;

        event UpgradeUninstalledDelegate OnUpgradeUninstalled;

        void InvokeUpgradeInstalled();

        void InvokeUpgradeUninstalled();
    }
}
