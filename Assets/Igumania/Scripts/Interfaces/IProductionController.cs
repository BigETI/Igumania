using UnityTiming.Data;

namespace Igumania
{
    public interface IProductionController : IBehaviour
    {
        TimingData DayTiming { get; set; }

        long MinimalProfitPerDay { get; set; }

        long AdditionalProfitPerRobot { get; set; }

        IProfile Profile { get; }

        float ElapsedProductionHaltingTime { get; }

        event GameDayHasPassedDelegate OnGameDayHasPassed;
    }
}
