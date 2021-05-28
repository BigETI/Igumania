using Igumania.Controllers;

namespace Igumania
{
    public interface IRobotController : IControllersController
    {
        byte RobotIndex { get; set; }

        float MaximalInteractionPauseTime { get; set; }

        long InteractionMoney { get; set; }

        float RemainingInteractionPauseTime { get; }

        RobotsVisualsControllerScript RobotsVisualsController { get; }

        RobotContextMenuUIControllerScript RobotContextMenuUIController { get; }

        event InteractedDelegate OnInteracted;

        IProfile Profile { get; }

        IRobot Robot { get; }

        GameMenuControllerScript GameMenuController { get; }

        void Interact();

        void OpenContextMenu();
    }
}
