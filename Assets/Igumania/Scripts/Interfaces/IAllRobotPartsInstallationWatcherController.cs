namespace Igumania
{
    public interface IAllRobotPartsInstallationWatcherController : IBehaviour
    {
        IProfile Profile { get; }

        event AllRobotPartsInstalledDelegate OnAllRobotPartsInstalled;
    }
}
