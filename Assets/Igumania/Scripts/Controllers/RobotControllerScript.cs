using Igumania.Data;
using Igumania.Objects;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Igumania.Controllers
{
    public class RobotControllerScript : AControllersControllerScript<RobotControllerScript>, IRobotController
    {
        private readonly Dictionary<string, IVisualUpgradeData> visualUpgradeLookup = new Dictionary<string, IVisualUpgradeData>();

        [SerializeField]
        private byte robotIndex;

        [SerializeField]
        private float maximalInteractionPauseTime = 0.25f;

        [SerializeField]
        private long interactionMoney = 1L;

        [SerializeField]
        private VisualUpgradeData[] visualUpgrades = Array.Empty<VisualUpgradeData>();

        [SerializeField]
        private UnityEvent onInteracted = default;

        private bool isNotInitialized = true;

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

        public IEnumerable<IVisualUpgradeData> VisualUpgrades => visualUpgradeLookup.Values;

        public float RemainingInteractionPauseTime { get; private set; }

        public RobotContextMenuUIControllerScript RobotContextMenuUIController { get; private set; }

        public event InteractedDelegate OnInteracted;

        public IProfile Profile { get; private set; }

        public IRobot Robot { get; private set; }

        public GameMenuControllerScript GameMenuController { get; private set; }

        private void UpgradeInstalledEvent(UpgradeObjectScript upgrade)
        {
            string key = upgrade.name;
            if (visualUpgradeLookup.ContainsKey(key))
            {
                visualUpgradeLookup[key].InvokeUpgradeInstalled();
            }
        }

        private void UpgradeUninstalledEvent(UpgradeObjectScript upgrade)
        {
            string key = upgrade.name;
            if (visualUpgradeLookup.ContainsKey(key))
            {
                visualUpgradeLookup[key].InvokeUpgradeUninstalled();
            }
        }

        private void RobotPartInstalledEvent(RobotPartObjectScript robotPart)
        {
            string key = robotPart.name;
            if (visualUpgradeLookup.ContainsKey(key))
            {
                visualUpgradeLookup[key].InvokeUpgradeInstalled();
            }
        }

        private void RobotPartUninstalledEvent(RobotPartObjectScript robotPart)
        {
            string key = robotPart.name;
            if (visualUpgradeLookup.ContainsKey(key))
            {
                visualUpgradeLookup[key].InvokeUpgradeUninstalled();
            }
        }

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
            visualUpgradeLookup.Clear();
            if (visualUpgrades != null)
            {
                foreach (VisualUpgradeData visual_upgrade in visualUpgrades)
                {
                    if (visual_upgrade.Upgrade)
                    {
                        string key = visual_upgrade.Upgrade.name;
                        if (visualUpgradeLookup.ContainsKey(key))
                        {
                            Debug.LogError($"Found duplicate visual upgrade entry \"{ key }\" in visual upgrades.", this);
                        }
                        else
                        {
                            visualUpgradeLookup.Add(key, visual_upgrade);
                        }
                    }
                    else
                    {
                        Debug.LogError("Please define a visual upgrade or remove it from the array, if not needed at all.", this);
                    }
                }
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            if (!isNotInitialized)
            {
                if (Profile != null)
                {
                    Profile.OnUpgradeInstalled -= UpgradeInstalledEvent;
                    Profile.OnUpgradeUninstalled -= UpgradeUninstalledEvent;
                    foreach (UpgradeObjectScript upgrade in Profile.Upgrades)
                    {
                        string key = upgrade.name;
                        if (visualUpgradeLookup.ContainsKey(key))
                        {
                            visualUpgradeLookup[key].InvokeUpgradeUninstalled();
                        }
                    }
                }
                if (Robot != null)
                {
                    Robot.OnRobotPartInstalled -= RobotPartInstalledEvent;
                    Robot.OnRobotPartUninstalled -= RobotPartUninstalledEvent;
                    foreach (RobotPartObjectScript robot_part in Robot.RobotParts)
                    {
                        string key = robot_part.name;
                        if (visualUpgradeLookup.ContainsKey(key))
                        {
                            visualUpgradeLookup[key].InvokeUpgradeUninstalled();
                        }
                    }
                }
                isNotInitialized = true;
            }
            visualUpgradeLookup.Clear();
        }

        private void Update()
        {
            if (isNotInitialized)
            {
                if (Profile != null)
                {
                    foreach (UpgradeObjectScript upgrade in Profile.Upgrades)
                    {
                        string key = upgrade.name;
                        if (visualUpgradeLookup.ContainsKey(key))
                        {
                            visualUpgradeLookup[key].InvokeUpgradeInstalled();
                        }
                    }
                    Profile.OnUpgradeInstalled += UpgradeInstalledEvent;
                    Profile.OnUpgradeUninstalled += UpgradeUninstalledEvent;
                }
                if (Robot != null)
                {
                    foreach (RobotPartObjectScript robot_part in Robot.RobotParts)
                    {
                        string key = robot_part.name;
                        if (visualUpgradeLookup.ContainsKey(key))
                        {
                            visualUpgradeLookup[key].InvokeUpgradeInstalled();
                        }
                    }
                    Robot.OnRobotPartInstalled += RobotPartInstalledEvent;
                    Robot.OnRobotPartUninstalled += RobotPartUninstalledEvent;
                }
                isNotInitialized = false;
            }
            RemainingInteractionPauseTime = Mathf.Max(RemainingInteractionPauseTime - Time.deltaTime, 0.0f);
        }

#if UNITY_EDITOR
        private void OnValidate() => maximalInteractionPauseTime = Mathf.Max(maximalInteractionPauseTime, 0.0f);
#endif
    }
}
