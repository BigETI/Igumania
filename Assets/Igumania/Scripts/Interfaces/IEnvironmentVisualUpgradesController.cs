using System.Collections.Generic;

namespace Igumania
{
    public interface IEnvironmentVisualUpgradesController : IBehaviour
    {
        IEnumerable<IVisualUpgradeData> VisualUpgrades { get; }

        IProfile Profile { get; }
    }
}
