using Igumania.Objects;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Igumania.Controllers
{
    [RequireComponent(typeof(RectTransform))]
    public class UpgradeToolTipPanelUIControllerScript : AControllersControllerScript<UpgradeToolTipPanelUIControllerScript>, IUpgradeToolTipPanelUIController
    {
        [SerializeField]
        private Image iconImage = default;

        [SerializeField]
        private TextMeshProUGUI nameText = default;

        [SerializeField]
        private TextMeshProUGUI descriptionText = default;

        [SerializeField]
        private UnityEvent onUpgradeToolTipLoaded = default;

        [SerializeField]
        private UnityEvent onUpgradeToolTipUnloaded = default;

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

        public UpgradeObjectScript SelectedUpgrade
        {
            get => selectedUpgrade;
            set
            {
                selectedUpgrade = value;
                ProcessSelectedUpgradeState();
            }
        }

        public RectTransform RectangleTransform { get; private set; }

        public Canvas ParentCanvas { get; private set; }

        public CanvasScaler ParentCanvasScaler { get; private set; }

        public event UpgradeToolTipLoadedDelegate OnUpgradeToolTipLoaded;

        public event UpgradeToolTipUnloadedDelegate OnUpgradeToolTipUnloaded;

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
            }
        }

        private void ProcessSelectedUpgradeState()
        {
            if (lastSelectedUpgrade != selectedUpgrade)
            {
                UpgradeObjectScript old_selected_upgrade = selectedUpgrade;
                lastSelectedUpgrade = selectedUpgrade;
                UpdateVisuals();
                if (onUpgradeToolTipUnloaded != null)
                {
                    onUpgradeToolTipUnloaded.Invoke();
                }
                OnUpgradeToolTipUnloaded?.Invoke(old_selected_upgrade);
                if (selectedUpgrade)
                {
                    if (onUpgradeToolTipLoaded != null)
                    {
                        onUpgradeToolTipLoaded.Invoke();
                    }
                    OnUpgradeToolTipLoaded?.Invoke(selectedUpgrade);
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
            if (TryGetComponent(out RectTransform rectangle_transform))
            {
                RectangleTransform = rectangle_transform;
            }
            else
            {
                Debug.LogError($"Please attach a \"{ nameof(RectTransform) }\" component to this game object.", this);
            }
            ParentCanvas = GetComponentInParent<Canvas>();
            if (!ParentCanvas)
            {
                Debug.LogError("Please add this game object as a child of a canvas.", this);
            }
            ParentCanvasScaler = GetComponentInParent<CanvasScaler>();
            if (!ParentCanvasScaler)
            {
                Debug.LogError("Please add this game object as a child of a canvas scaler.", this);
            }
            UpdateVisuals();
        }

        private void Update()
        {
            ProcessSelectedUpgradeState();
            if (selectedUpgrade && RectangleTransform && ParentCanvas && ParentCanvasScaler)
            {
                Mouse mouse = Mouse.current;
                if (mouse != null)
                {
                    RectangleTransform.anchoredPosition = mouse.position.ReadValue() * ParentCanvasScaler.referenceResolution.x / ParentCanvas.renderingDisplaySize.x;
                }
            }
        }
    }
}
