using Igumania.Controllers;
using Igumania.Objects;
using TMPro;
using UnityEngine.UI;

namespace Igumania
{
    public interface IUpgradeDetailsPanelUIController : IControllersController
    {
        Image IconImage { get; set; }

        TextMeshProUGUI NameText { get; set; }

        TextMeshProUGUI DescriptionText { get; set; }

        TextMeshProUGUI URLText { get; set; }

        TextMeshProUGUI CostText { get; set; }

        UpgradeObjectScript SelectedUpgrade { get; set; }

        IProfile Profile { get; }

        GameMenuControllerScript GameMenuController { get; }

        event UpgradeDetailsLoadedDelegate OnUpgradeDetailsLoaded;

        event UpgradeDetailsUnloadedDelegate OnUpgradeDetailsUnloaded;

        event UpgradePurchasedDelegate OnUpgradePurchased;

        void Purchase();

        void OpenURL();

        void DeselectUpgrade();
    }
}
