using Igumania.Objects;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Igumania.Controllers
{
    public class UpgradeDetailsPanelUIControllerScript : AControllersControllerScript<UpgradeDetailsPanelUIControllerScript>, IUpgradeDetailsPanelUIController
    {
        [SerializeField]
        private Image iconImage = default;

        [SerializeField]
        private TextMeshProUGUI nameText = default;

        [SerializeField]
        private TextMeshProUGUI descriptionText = default;

        [SerializeField]
        private TextMeshProUGUI urlText = default;

        [SerializeField]
        private TextMeshProUGUI costText = default;

        [SerializeField]
        private UnityEvent onUpgradeDetailsLoaded = default;

        [SerializeField]
        private UnityEvent onUpgradeDetailsUnloaded = default;

        [SerializeField]
        private UnityEvent onUpgradePurchased = default;

        private UpgradeObjectScript selectedUpgrade;

        private UpgradeObjectScript lastSelectedUpgrade;

        public Image IconImage
        {
            get => iconImage;
            set => iconImage = value;
        }

        public TextMeshProUGUI NameText
        {
            get => nameText;
            set => nameText = value;
        }

        public TextMeshProUGUI DescriptionText
        {
            get => descriptionText;
            set => descriptionText = value;
        }

        public TextMeshProUGUI URLText
        {
            get => urlText;
            set => urlText = value;
        }

        public TextMeshProUGUI CostText
        {
            get => costText;
            set => costText = value;
        }

        public UpgradeObjectScript SelectedUpgrade
        {
            get => selectedUpgrade;
            set
            {
                selectedUpgrade = value;
                ProcessSelectedUpgradeState();
            }
        }

        public IProfile Profile { get; private set; }

        public GameMenuControllerScript GameMenuController { get; private set; }

        public event UpgradeDetailsLoadedDelegate OnUpgradeDetailsLoaded;

        public event UpgradeDetailsUnloadedDelegate OnUpgradeDetailsUnloaded;

        public event UpgradePurchasedDelegate OnUpgradePurchased;

        private void UpdateVisuals()
        {
            if (selectedUpgrade)
            {
                if (iconImage)
                {
                    iconImage.sprite = selectedUpgrade.IconSprite;
                }
                if (nameText)
                {
                    nameText.text = selectedUpgrade.ItemName;
                }
                if (descriptionText)
                {
                    descriptionText.text = selectedUpgrade.ItemDescription;
                }
                if (urlText)
                {
                    urlText.text = selectedUpgrade.URL;
                }
                if (costText)
                {
                    costText.text = selectedUpgrade.Cost.ToString();
                }
            }
        }

        private void ProcessSelectedUpgradeState()
        {
            if (lastSelectedUpgrade != selectedUpgrade)
            {
                UpgradeObjectScript old_selected_upgrade = selectedUpgrade;
                lastSelectedUpgrade = selectedUpgrade;
                UpdateVisuals();
                if (onUpgradeDetailsUnloaded != null)
                {
                    onUpgradeDetailsUnloaded.Invoke();
                }
                OnUpgradeDetailsUnloaded?.Invoke(old_selected_upgrade);
                if (selectedUpgrade)
                {
                    if (onUpgradeDetailsLoaded != null)
                    {
                        onUpgradeDetailsLoaded.Invoke();
                    }
                    OnUpgradeDetailsLoaded?.Invoke(selectedUpgrade);
                }
            }
        }

        public void Purchase()
        {
            if (selectedUpgrade)
            {
                if (selectedUpgrade is RobotPartObjectScript selected_robot_part)
                {
                    bool is_robot_selected = false;
                    foreach (ShopSelectionControllerScript shop_selection_controller in ShopSelectionControllerScript.EnabledControllers)
                    {
                        shop_selection_controller.SelectedRobotPart = selected_robot_part;
                        if (shop_selection_controller.SelectedRobot != null)
                        {
                            is_robot_selected = true;
                            shop_selection_controller.Purchase();
                        }
                    }
                    if (GameMenuController)
                    {
                        GameMenuController.GameMenuState = is_robot_selected ? EGameMenuState.Playing : EGameMenuState.SelectRobotMenu;
                    }
                }
                else if ((Profile != null) && (Profile.Money >= (long)selectedUpgrade.Cost) && Profile.InstallUpgrade(selectedUpgrade))
                {
                    Profile.Money -= (long)selectedUpgrade.Cost;
                    foreach (ShopSelectionControllerScript shop_selection_controller in ShopSelectionControllerScript.EnabledControllers)
                    {
                        shop_selection_controller.Deselect();
                    }
                    if (GameMenuController)
                    {
                        GameMenuController.GameMenuState = EGameMenuState.Playing;
                    }
                    if (onUpgradePurchased != null)
                    {
                        onUpgradePurchased.Invoke();
                    }
                    OnUpgradePurchased?.Invoke(selectedUpgrade);
                }
            }
        }

        public void OpenURL()
        {
            if (selectedUpgrade)
            {
                string url = selectedUpgrade.URL.Trim();
                if (!string.IsNullOrWhiteSpace(url))
                {
                    Application.OpenURL(url);
                }
            }
        }

        public void DeselectUpgrade() => SelectedUpgrade = null;

        private void Start()
        {
            if (!iconImage)
            {
                Debug.LogError("Please assign an icon image to this component.", this);
            }
            if (!nameText)
            {
                Debug.LogError("Please assign a name text to this component.", this);
            }
            if (!descriptionText)
            {
                Debug.LogError("Please assign a description text to this component.", this);
            }
            if (!urlText)
            {
                Debug.LogError("Please assign an URL text to this component.", this);
            }
            if (!costText)
            {
                Debug.LogError("Please assign a cost text to this component.", this);
            }
            GameMenuController = FindObjectOfType<GameMenuControllerScript>();
            if (!GameMenuController)
            {
                Debug.LogError("Please add a game menu controller to the scene before loading this game object.", this);
            }
            Profile = GameManager.SelectedProfile;
            UpdateVisuals();
        }
    }
}
