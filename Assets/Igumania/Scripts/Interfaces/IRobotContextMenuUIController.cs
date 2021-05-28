using Igumania.Controllers;
using UnityEngine;
using UnityEngine.UI;

namespace Igumania
{
    public interface IRobotContextMenuUIController : IBehaviour
    {
        Canvas ParentCanvas { get; }

        CanvasScaler ParentCanvasScaler { get; }

        Camera GameCamera { get; }

        RobotControllerScript SelectedRobotController { get; set; }

        event RobotControllerSelectedDelegate OnRobotControllerSelected;

        event RobotControllerDeselectedDelegate OnRobotControllerDeselected;

        void SelectRobotForShopMenu();
    }
}
