using Igumania.Objects;

namespace Igumania
{
    public interface IBudgetWatcherController : IBehaviour
    {
        UpgradeObjectScript Upgrade { get; set; }

        event BudgetReachedDelegate OnBudgetReached;
    }
}
