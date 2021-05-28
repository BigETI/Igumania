using UnityEngine;
using UnityEngine.InputSystem;

namespace Igumania.Controllers
{
    public class RobotsInteractionsControllerScript : MonoBehaviour, IRobotsInteractionsController
    {
        private bool isLeftMouseButtonDown;

        private bool isRightMouseButtonDown;

        public Camera GameCamera { get; private set; }

        public RobotContextMenuUIControllerScript RobotContextMenuUIController { get; private set; }

        private void Start()
        {
            GameCamera = FindObjectOfType<Camera>();
            if (!GameCamera)
            {
                Debug.LogError("Please add a camera to the scene before loading this game object.", this);
            }
            RobotContextMenuUIController = FindObjectOfType<RobotContextMenuUIControllerScript>();
            if (!RobotContextMenuUIController)
            {
                Debug.LogError("Please add a robot context menu UI controller to the scene before loading this game object.", this);
            }
        }

        private void FixedUpdate()
        {
            if (GameCamera)
            {
                Mouse mouse = Mouse.current;
                bool is_left_mouse_button_down = (mouse != null) && mouse.leftButton.isPressed;
                bool is_right_mouse_button_down = (mouse != null) && mouse.rightButton.isPressed;
                bool is_left_mouse_button_clicked = isLeftMouseButtonDown && !is_left_mouse_button_down;
                bool is_right_mouse_button_clicked = isRightMouseButtonDown && !is_right_mouse_button_down;
                if (is_left_mouse_button_clicked || is_right_mouse_button_clicked)
                {
                    Vector2 mouse_posiiton = mouse.position.ReadValue();
                    RaycastHit[] raycast_hits = Physics.RaycastAll(GameCamera.ScreenPointToRay(new Vector3(mouse_posiiton.x, mouse_posiiton.y, 0.0f)));
                    if (raycast_hits != null)
                    {
                        foreach (RaycastHit raycast_hit in raycast_hits)
                        {
                            RobotControllerScript robot_controller = raycast_hit.collider.GetComponentInParent<RobotControllerScript>();
                            if (robot_controller)
                            {
                                if (is_left_mouse_button_clicked)
                                {
                                    robot_controller.Interact();
                                }
                                if (is_right_mouse_button_clicked)
                                {
                                    robot_controller.OpenContextMenu();
                                }
                            }
                        }
                    }
                    if (!is_right_mouse_button_clicked && RobotContextMenuUIController)
                    {
                        RobotContextMenuUIController.SelectedRobotController = null;
                    }
                }
                isLeftMouseButtonDown = is_left_mouse_button_down;
                isRightMouseButtonDown = is_right_mouse_button_down;
            }
        }
    }
}
