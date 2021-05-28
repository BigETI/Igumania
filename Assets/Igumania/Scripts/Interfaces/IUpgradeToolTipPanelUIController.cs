using Igumania.Objects;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Igumania
{
    public interface IUpgradeToolTipPanelUIController : IControllersController
    {
        Image IconImage { get; set; }

        TextMeshProUGUI NameText { get; set; }

        TextMeshProUGUI DescriptionText { get; set; }

        UpgradeObjectScript SelectedUpgrade { get; set; }

        RectTransform RectangleTransform { get; }

        Canvas ParentCanvas { get; }

        CanvasScaler ParentCanvasScaler { get; }

        event UpgradeToolTipLoadedDelegate OnUpgradeToolTipLoaded;

        event UpgradeToolTipUnloadedDelegate OnUpgradeToolTipUnloaded;

        void DeselectUpgrade();
    }
}
