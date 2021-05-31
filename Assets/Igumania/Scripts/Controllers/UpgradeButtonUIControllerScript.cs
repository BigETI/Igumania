using Igumania.Objects;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Igumania.Controllers
{
    [RequireComponent(typeof(Button))]
    public class UpgradeButtonUIControllerScript : MonoBehaviour, IUpgradeButtonUIController
    {
        [SerializeField]
        private UpgradeObjectScript upgrade = default;

        [SerializeField]
        private Image iconImage = default;

        public UpgradeObjectScript Upgrade
        {
            get => upgrade;
            set => upgrade = value;
        }

        public Image IconImage
        {
            get => iconImage;
            set => iconImage = value;
        }

        public Button UpgradeButton { get; private set; }

        private void HandleButtonClickEvent()
        {
            foreach (UpgradeDetailsPanelUIControllerScript upgrade_details_controller in UpgradeDetailsPanelUIControllerScript.EnabledControllers)
            {
                upgrade_details_controller.SelectedUpgrade = upgrade;
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            foreach (UpgradeToolTipPanelUIControllerScript upgrade_tool_tip_panel_controller in UpgradeToolTipPanelUIControllerScript.EnabledControllers)
            {
                upgrade_tool_tip_panel_controller.SelectedUpgrade = upgrade;
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            foreach (UpgradeToolTipPanelUIControllerScript upgrade_tool_tip_panel_controller in UpgradeToolTipPanelUIControllerScript.EnabledControllers)
            {
                upgrade_tool_tip_panel_controller.SelectedUpgrade = null;
            }
        }

        private void OnEnable()
        {
            if (TryGetComponent(out Button upgrade_button))
            {
                IProfile profile = GameManager.SelectedProfile;
                UpgradeButton = upgrade_button;
                if (upgrade is RobotPartObjectScript robot_part)
                {
                    IRobot selected_robot = null;
                    foreach (ShopSelectionControllerScript shop_selection_controller in ShopSelectionControllerScript.EnabledControllers)
                    {
                        selected_robot = shop_selection_controller.SelectedRobot;
                        if (selected_robot != null)
                        {
                            break;
                        }
                    }
                    UpgradeButton.interactable = (selected_robot == null) ? profile.IsInstallingUpgradeAllowed(robot_part) : selected_robot.IsInstallingRobotPartAllowed(robot_part);
                }
                else
                {
                    UpgradeButton.interactable = (profile != null) && profile.IsInstallingUpgradeAllowed(upgrade);
                }
                UpgradeButton.onClick.AddListener(HandleButtonClickEvent);
            }
        }

        private void OnDisable()
        {
            if (UpgradeButton)
            {
                UpgradeButton.onClick.RemoveListener(HandleButtonClickEvent);
                UpgradeButton = null;
            }
        }

        private void Start()
        {
            if (!iconImage)
            {
                Debug.LogError("Please assign an icon image to this component.", this);
            }
            if (!UpgradeButton)
            {
                Debug.LogError($"Please attach a \"{ nameof(Button) }\" component to this game object.", this);
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (iconImage)
            {
                iconImage.sprite = upgrade ? upgrade.IconSprite : null;
            }
        }
#endif
    }
}
