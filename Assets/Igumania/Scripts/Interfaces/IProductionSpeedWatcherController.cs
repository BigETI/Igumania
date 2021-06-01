namespace Igumania
{
    public interface IProductionSpeedWatcherController : IBehaviour
    {
        IProfile Profile { get; }

        float ProductionSpeed { get; }

        event ProductionSpeedUpdatedDelegate OnProductionSpeedUpdated;
    }
}
