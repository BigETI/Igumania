using Igumania.Objects;

namespace Igumania
{
    public interface IUpgradeInstallationWatcherController : IBehaviour
    {
        UpgradeObjectScript Upgrade { get; set; }

        IProfile Profile { get; }

        event UpgradeInstalledDelegate OnUpgradeInstalled;
    }
}
