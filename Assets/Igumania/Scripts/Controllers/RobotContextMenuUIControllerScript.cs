using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Igumania.Controllers
{
    [RequireComponent(typeof(RectTransform))]
    public class RobotContextMenuUIControllerScript : MonoBehaviour, IRobotContextMenuUIController
    {
        [SerializeField]
        private UnityEvent onRobotControllerSelected = default;

        [SerializeField]
        private UnityEvent onRobotControllerDeselected = default;

        private RobotControllerScript selectedRobotController;

        public RectTransform RectangleTransform { get; private set; }

        public Canvas ParentCanvas { get; private set; }

        public CanvasScaler ParentCanvasScaler { get; private set; }

        public Camera GameCamera { get; private set; }

        public RobotControllerScript SelectedRobotController
        {
            get => selectedRobotController;
            set
            {
                if (selectedRobotController != value)
                {
                    RobotControllerScript old_selected_robot_controller = selectedRobotController;
                    selectedRobotController = value;
                    if (selectedRobotController)
                    {
                        if (onRobotControllerSelected != null)
                        {
                            onRobotControllerSelected.Invoke();
                        }
                        OnRobotControllerSelected?.Invoke(selectedRobotController);
                    }
                    else
                    {
                        if (onRobotControllerDeselected != null)
                        {
                            onRobotControllerDeselected.Invoke();
                        }
                        OnRobotControllerDeselected?.Invoke(old_selected_robot_controller);
                    }
                }
            }
        }

        public event RobotControllerSelectedDelegate OnRobotControllerSelected;

        public event RobotControllerDeselectedDelegate OnRobotControllerDeselected;

        public void SelectRobotForShopMenu()
        {
            if (selectedRobotController)
            {
                IRobot robot = selectedRobotController.Robot;
                foreach (ShopSelectionControllerScript shop_selection_controller in ShopSelectionControllerScript.EnabledControllers)
                {
                    shop_selection_controller.SelectedRobot = robot;
                }
            }
        }

        private void Start()
        {
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
            GameCamera = FindObjectOfType<Camera>();
            if (!GameCamera)
            {
                Debug.LogError("Please add a camera to the scene before loading this game object.", this);
            }
        }

        private void Update()
        {
            if (RectangleTransform && ParentCanvasScaler && GameCamera && selectedRobotController)
            {
                Vector3 screen_point = GameCamera.WorldToScreenPoint(selectedRobotController.transform.position);
                Vector2 local_position = new Vector2(screen_point.x, screen_point.y) * ParentCanvasScaler.referenceResolution.x / ParentCanvas.renderingDisplaySize.x;
                RectangleTransform.anchoredPosition = local_position;
            }
        }
    }
}
