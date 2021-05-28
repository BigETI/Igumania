using TMPro;
using UnityEngine;

namespace Igumania
{
    public interface IMoneyPanelUIController : IBehaviour
    {
        float WidthPerCharacter { get; set; }

        TextMeshProUGUI MoneyText { get; set; }

        float BasePanelWidth { get; }

        IProfile Profile { get; }

        long Money { get; }

        RectTransform RectangleTransform { get; }

        event MoneyChangedDelegate OnMoneyChanged;
    }
}
