using Igumania.Objects;

namespace Igumania
{
    public interface IShopSelectionController : IControllersController
    {
        IProfile Profile { get; }

        long Money { get; }

        IRobot SelectedRobot { get; set; }

        RobotPartObjectScript SelectedRobotPart { get; set; }

        event RobotPartPurchasedDelegate OnRobotPartPurchased;

        void Deselect();

        void Purchase();
    }
}
