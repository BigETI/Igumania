using UnityEngine;
using UnityEngine.Events;

namespace Igumania.Controllers
{
    [RequireComponent(typeof(RobotsVisualsControllerScript))]
    public class RobotControllerScript : AControllersControllerScript<RobotControllerScript>, IRobotController
    {
        [SerializeField]
        private byte robotIndex;

        [SerializeField]
        private float maximalInteractionPauseTime = 0.25f;

        [SerializeField]
        private long interactionMoney = 1L;

        [SerializeField]
        private UnityEvent onInteracted = default;

        public byte RobotIndex
        {
            get => robotIndex;
            set => robotIndex = value;
        }

        public float MaximalInteractionPauseTime
        {
            get => maximalInteractionPauseTime;
            set => maximalInteractionPauseTime = Mathf.Max(value, 0.0f);
        }

        public long InteractionMoney
        {
            get => interactionMoney;
            set => interactionMoney = value;
        }

        public float RemainingInteractionPauseTime { get; private set; }

        public RobotsVisualsControllerScript RobotsVisualsController { get; private set; }

        public RobotContextMenuUIControllerScript RobotContextMenuUIController { get; private set; }

        public event InteractedDelegate OnInteracted;

        public IProfile Profile { get; private set; }

        public IRobot Robot { get; private set; }

        public GameMenuControllerScript GameMenuController { get; private set; }

        public void Interact()
        {
            if (RemainingInteractionPauseTime <= 0.0f)
            {
                RemainingInteractionPauseTime = maximalInteractionPauseTime;
                if (GameMenuController)
                {
                    switch (GameMenuController.GameMenuState)
                    {
                        case EGameMenuState.Playing:
                            if (Profile != null)
                            {
                                Profile.Money += interactionMoney;
                            }
                            break;
                        case EGameMenuState.SelectRobotMenu:
                            foreach (ShopSelectionControllerScript shop_selection_controller in ShopSelectionControllerScript.EnabledControllers)
                            {
                                shop_selection_controller.SelectedRobot = Robot;
                                shop_selection_controller.Purchase();
                            }
                            GameMenuController.GameMenuState = EGameMenuState.Playing;
                            break;
                    }
                }
                if (onInteracted != null)
                {
                    onInteracted.Invoke();
                }
                OnInteracted?.Invoke();
            }
        }

        public void OpenContextMenu()
        {
            if (RobotContextMenuUIController)
            {
                RobotContextMenuUIController.SelectedRobotController = this;
            }
        }

        private void Start()
        {
            if (TryGetComponent(out RobotsVisualsControllerScript robots_visuals_controller))
            {
                RobotsVisualsController = robots_visuals_controller;
            }
            else
            {
                Debug.LogError($"Please attach a \"{ nameof(RobotsVisualsControllerScript) }\" component to this game object.", this);
            }
            RobotContextMenuUIController = FindObjectOfType<RobotContextMenuUIControllerScript>();
            if (!RobotContextMenuUIController)
            {
                Debug.LogError("Please add a robot context menu UI controller to the scene before loading this game object.", this);
            }
            GameMenuController = FindObjectOfType<GameMenuControllerScript>();
            if (!GameMenuController)
            {
                Debug.LogError("Please add a game menu controller to the scene before loading this game object.", this);
            }
            Profile = GameManager.SelectedProfile;
            if (Profile != null)
            {
                Robot = Profile.IsRobotAvailable(robotIndex) ? Profile.GetRobot(robotIndex) : Profile.CreateNewRobot(robotIndex);
            }
        }

        private void Update()
        {
            RemainingInteractionPauseTime = Mathf.Max(RemainingInteractionPauseTime - Time.deltaTime, 0.0f);
            if (Robot != null)
            {
                // TODO: Update visuals
            }
        }

#if UNITY_EDITOR
        private void OnValidate() => maximalInteractionPauseTime = Mathf.Max(maximalInteractionPauseTime, 0.0f);
#endif
    }
}
