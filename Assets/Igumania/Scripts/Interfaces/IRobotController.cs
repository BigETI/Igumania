using Igumania.Controllers;
using System.Collections.Generic;

namespace Igumania
{
    public interface IRobotController : IControllersController
    {
        byte RobotIndex { get; set; }

        float MaximalInteractionPauseTime { get; set; }

        long InteractionMoney { get; set; }

        IEnumerable<IVisualUpgradeData> VisualUpgrades { get; }

        float RemainingInteractionPauseTime { get; }

        RobotContextMenuUIControllerScript RobotContextMenuUIController { get; }

        event InteractedDelegate OnInteracted;

        IProfile Profile { get; }

        IRobot Robot { get; }

        GameMenuControllerScript GameMenuController { get; }

        void Interact();

        void OpenContextMenu();
    }
}
