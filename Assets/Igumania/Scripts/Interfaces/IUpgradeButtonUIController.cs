using Igumania.Objects;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Igumania
{
    public interface IUpgradeButtonUIController : IBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        UpgradeObjectScript Upgrade { get; set; }

        Image IconImage { get; set; }

        Button UpgradeButton { get; }
    }
}
