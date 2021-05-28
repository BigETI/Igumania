using Igumania.Objects;
using UnityEngine;
using UnityEngine.Events;

namespace Igumania.Controllers
{
    public class ShopSelectionControllerScript : AControllersControllerScript<ShopSelectionControllerScript>, IShopSelectionController
    {
        [SerializeField]
        private UnityEvent onRobotPartPurchased = default;

        public IProfile Profile { get; private set; }

        public long Money => (Profile == null) ? 0L : Profile.Money;

        public IRobot SelectedRobot { get; set; }

        public RobotPartObjectScript SelectedRobotPart { get; set; }

        public event RobotPartPurchasedDelegate OnRobotPartPurchased;

        public void Deselect()
        {
            SelectedRobot = null;
            SelectedRobotPart = null;
        }

        public void Purchase()
        {
            if ((SelectedRobot != null) && SelectedRobotPart && (Profile != null) && (Profile.Money >= (long)SelectedRobotPart.Cost) && SelectedRobot.InstallRobotPart(SelectedRobotPart))
            {
                Profile.Money -= (long)SelectedRobotPart.Cost;
                if (onRobotPartPurchased != null)
                {
                    onRobotPartPurchased.Invoke();
                }
                OnRobotPartPurchased?.Invoke(SelectedRobot, SelectedRobotPart);
            }
            Deselect();
        }

        private void Start() => Profile = GameManager.SelectedProfile;
    }
}
