using Igumania.Objects;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Igumania.Controllers
{
    public class AllRobotPartsInstallationWatcherControllerScript : MonoBehaviour, IAllRobotPartsInstallationWatcherController
    {
        [SerializeField]
        private UnityEvent onAllRobotPartsInstalled = default;

        private RobotPartObjectScript[] allRobotParts = Array.Empty<RobotPartObjectScript>();

        private bool isNotNotified = true;

        public IProfile Profile { get; private set; }

        public event AllRobotPartsInstalledDelegate OnAllRobotPartsInstalled;

        private void UpgradeInstalledEvent(UpgradeObjectScript upgrade)
        {
            if (isNotNotified)
            {
                bool are_all_robot_parts_installed = true;
                foreach (RobotControllerScript robot_controller in RobotControllerScript.Controllers)
                {
                    if (Profile.IsRobotAvailable(robot_controller.RobotIndex))
                    {
                        IRobot robot = Profile.GetRobot(robot_controller.RobotIndex);
                        foreach (RobotPartObjectScript robot_part in allRobotParts)
                        {
                            if (!robot.IsRobotPartInstalled(robot_part))
                            {
                                are_all_robot_parts_installed = false;
                                break;
                            }
                        }
                        if (!are_all_robot_parts_installed)
                        {
                            break;
                        }
                    }
                    else
                    {
                        are_all_robot_parts_installed = false;
                        break;
                    }
                }
                if (are_all_robot_parts_installed)
                {
                    isNotNotified = false;
                    if (onAllRobotPartsInstalled != null)
                    {
                        onAllRobotPartsInstalled.Invoke();
                    }
                    OnAllRobotPartsInstalled?.Invoke();
                }
            }
        }

        private void OnEnable()
        {
            allRobotParts = Resources.LoadAll<RobotPartObjectScript>("RobotParts") ?? Array.Empty<RobotPartObjectScript>();
            Profile = GameManager.SelectedProfile;
            if (Profile != null)
            {
                Profile.OnUpgradeInstalled += UpgradeInstalledEvent;
            }
        }

        private void OnDisable()
        {
            allRobotParts = Array.Empty<RobotPartObjectScript>();
            if (Profile != null)
            {
                Profile.OnUpgradeInstalled -= UpgradeInstalledEvent;
            }
        }
    }
}
