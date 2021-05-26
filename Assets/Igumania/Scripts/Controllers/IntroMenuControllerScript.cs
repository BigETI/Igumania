using UnityEngine;
using UnityEngine.InputSystem;
using UnitySceneLoaderManager;

namespace Igumania.Controllers
{
    public class IntroMenuControllerScript : MonoBehaviour, IIntroMenuController
    {
        public bool IsNotShowingMainMenu { get; private set; } = true;

        public void ShowMainMenu()
        {
            IsNotShowingMainMenu = false;
            SceneLoaderManager.LoadScenes("MainMenuScene");
        }

        private void Update()
        {
            if (IsNotShowingMainMenu)
            {
                Keyboard keyboard = Keyboard.current;
                if ((keyboard != null) && keyboard.escapeKey.isPressed)
                {
                    ShowMainMenu();
                }
            }
        }
    }
}
