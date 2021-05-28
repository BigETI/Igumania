using Igumania.Controllers;
using UnityEngine;

namespace Igumania
{
    public interface IRobotsInteractionsController : IBehaviour
    {
        Camera GameCamera { get; }

        RobotContextMenuUIControllerScript RobotContextMenuUIController { get; }
    }
}
