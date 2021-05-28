using Igumania.Objects;
using System.Collections.Generic;

namespace Igumania
{
    public interface IUpgradeObject : IItemObject
    {
        IReadOnlyList<UpgradeObjectScript> RequiredUpgrades { get; }
    }
}
