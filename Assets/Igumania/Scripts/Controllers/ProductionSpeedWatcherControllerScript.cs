using Igumania.Objects;
using UnityEngine;
using UnityEngine.Events;

namespace Igumania.Controllers
{
    public class ProductionSpeedWatcherControllerScript : MonoBehaviour, IProductionSpeedWatcherController
    {
        [SerializeField]
        private UnityEvent<float> onProductionSpeedUpdated = default;

        public IProfile Profile { get; private set; }

        public float ProductionSpeed { get; private set; } = 1.0f;

        public event ProductionSpeedUpdatedDelegate OnProductionSpeedUpdated;

        private void UpdateProductionSpeed()
        {
            float production_speed = 1.0f;
            foreach (UpgradeObjectScript upgrade in Profile.Upgrades)
            {
                production_speed += upgrade.ProfitMultiplierAddition;
            }
            foreach (IRobot robot in Profile.Robots)
            {
                if (robot != null)
                {
                    foreach (RobotPartObjectScript robot_part in robot.RobotParts)
                    {
                        production_speed += robot_part.ProfitMultiplierAddition;
                    }
                }
            }
            if (ProductionSpeed != production_speed)
            {
                ProductionSpeed = production_speed;
                if (onProductionSpeedUpdated != null)
                {
                    onProductionSpeedUpdated.Invoke(production_speed);
                }
                OnProductionSpeedUpdated?.Invoke(production_speed);
            }
        }

        private void UpgradeInstalledUninstalledEvent(UpgradeObjectScript upgrade) => UpdateProductionSpeed();

        private void RobotEnabledDisabledEvent(IRobot robot) => UpdateProductionSpeed();

        private void OnEnable()
        {
            Profile = GameManager.SelectedProfile;
            if (Profile != null)
            {
                UpdateProductionSpeed();
                Profile.OnUpgradeInstalled += UpgradeInstalledUninstalledEvent;
                Profile.OnUpgradeUninstalled += UpgradeInstalledUninstalledEvent;
                Profile.OnRobotEnabled += RobotEnabledDisabledEvent;
                Profile.OnRobotDisabled += RobotEnabledDisabledEvent;
            }
        }

        private void OnDisable()
        {
            if (Profile != null)
            {
                Profile.OnUpgradeInstalled -= UpgradeInstalledUninstalledEvent;
                Profile.OnUpgradeUninstalled -= UpgradeInstalledUninstalledEvent;
                Profile.OnRobotEnabled -= RobotEnabledDisabledEvent;
                Profile.OnRobotDisabled -= RobotEnabledDisabledEvent;
                Profile = null;
            }
        }
    }
}
