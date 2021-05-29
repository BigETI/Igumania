using UnityDialog;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnitySceneLoaderManager;
using UnityTranslator.Objects;

namespace Igumania.Controllers
{
    public class GameMenuControllerScript : MonoBehaviour, IGameMenuController
    {
        [SerializeField]
        private StringTranslationObjectScript quitGameTitleStringTranslation = default;

        [SerializeField]
        private StringTranslationObjectScript quitGameMessageStringTranslation = default;

        [SerializeField]
        private UnityEvent onPlayStarted = default;

        [SerializeField]
        private UnityEvent onShopMenuShown = default;

        [SerializeField]
        private UnityEvent onSelectRobotMenuShown = default;

        [SerializeField]
        private UnityEvent onGameMenuShown = default;

        [SerializeField]
        private UnityEvent onPlayStopped = default;

        [SerializeField]
        private UnityEvent onShopMenuHidden = default;

        [SerializeField]
        private UnityEvent onSelectRobotMenuHidden = default;

        [SerializeField]
        private UnityEvent onGameMenuHidden = default;

        private EGameMenuState gameMenuState = EGameMenuState.Nothing;

        private bool isKeyboardEscapeKeyDown;

        public StringTranslationObjectScript QuitGameTitleStringTranslation
        {
            get => quitGameTitleStringTranslation;
            set => quitGameTitleStringTranslation = value;
        }

        public StringTranslationObjectScript QuitGameMessageStringTranslation
        {
            get => quitGameMessageStringTranslation;
            set => quitGameMessageStringTranslation = value;
        }

        public EGameMenuState GameMenuState
        {
            get => gameMenuState;
            set
            {
                if ((gameMenuState != value) && (value != EGameMenuState.Nothing))
                {
                    EGameMenuState old_game_menu_state = gameMenuState;
                    gameMenuState = value;
                    switch (old_game_menu_state)
                    {
                        case EGameMenuState.Playing:
                            if (onPlayStopped != null)
                            {
                                onPlayStopped.Invoke();
                            }
                            OnPlayStopped?.Invoke();
                            break;
                        case EGameMenuState.ShopMenu:
                            if (onShopMenuHidden != null)
                            {
                                onShopMenuHidden.Invoke();
                            }
                            OnShopMenuHidden?.Invoke();
                            break;
                        case EGameMenuState.SelectRobotMenu:
                            if (onSelectRobotMenuHidden != null)
                            {
                                onSelectRobotMenuHidden.Invoke();
                            }
                            OnSelectRobotMenuHidden?.Invoke();
                            break;
                        case EGameMenuState.GameMenu:
                            if (onGameMenuHidden != null)
                            {
                                onGameMenuHidden.Invoke();
                            }
                            OnGameMenuHidden?.Invoke();
                            break;
                    }
                    switch (gameMenuState)
                    {
                        case EGameMenuState.Playing:
                            if (onPlayStarted != null)
                            {
                                onPlayStarted.Invoke();
                            }
                            OnPlayStarted?.Invoke();
                            break;
                        case EGameMenuState.ShopMenu:
                            if (onShopMenuShown != null)
                            {
                                onShopMenuShown.Invoke();
                            }
                            OnShopMenuShown?.Invoke();
                            break;
                        case EGameMenuState.SelectRobotMenu:
                            if (onSelectRobotMenuShown != null)
                            {
                                onSelectRobotMenuShown.Invoke();
                            }
                            OnSelectRobotMenuShown?.Invoke();
                            break;
                        case EGameMenuState.GameMenu:
                            if (onGameMenuShown != null)
                            {
                                onGameMenuShown.Invoke();
                            }
                            OnGameMenuShown?.Invoke();
                            break;
                    }
                }
            }
        }

        public bool IsNotShowingDialog { get; private set; } = true;

        public event PlayStartedDelegate OnPlayStarted;

        public event ShopMenuShownDelegate OnShopMenuShown;

        public event SelectRobotMenuShownDelegate OnSelectRobotMenuShown;

        public event GameMenuShownDelegate OnGameMenuShown;

        public event PlayStoppedDelegate OnPlayStopped;

        public event ShopMenuHiddenDelegate OnShopMenuHidden;

        public event SelectRobotMenuHiddenDelegate OnSelectRobotMenuHidden;

        public event GameMenuHiddenDelegate OnGameMenuHidden;

        public void ResumeGame() => GameMenuState = EGameMenuState.Playing;

        public void ShowShopMenu() => GameMenuState = EGameMenuState.ShopMenu;

        public void ShowSelectRobotMenu() => GameMenuState = EGameMenuState.SelectRobotMenu;

        public void ShowGameMenu() => GameMenuState = EGameMenuState.GameMenu;

        public void ToggleGameMenu() => GameMenuState = (gameMenuState == EGameMenuState.GameMenu) ? EGameMenuState.Playing : EGameMenuState.GameMenu;

        public void RequestShowingMainMenu()
        {
            IsNotShowingDialog = false;
            Dialogs.Show
            (
                quitGameTitleStringTranslation ? quitGameTitleStringTranslation.ToString() : string.Empty,
                quitGameMessageStringTranslation ? quitGameMessageStringTranslation.ToString() : string.Empty,
                EDialogType.Warning,
                EDialogButtons.YesNo,
                (response, _) =>
                {
                    if (response == EDialogResponse.Yes)
                    {
                        ShowMainMenu();
                    }
                    IsNotShowingDialog = true;
                }
            );
        }

        public void ShowMainMenu()
        {
            GameManager.UnloadSelectedProfile();
            SceneLoaderManager.LoadScenes("MainMenuScene");
        }

        public void SaveGame() => GameManager.SelectedProfile?.Save();

        private void Start() => GameMenuState = EGameMenuState.Playing;

        private void Update()
        {
            Keyboard keyboard = Keyboard.current;
            bool is_keyboard_escape_key_pressed = IsNotShowingDialog && (keyboard != null) && keyboard.escapeKey.isPressed;
            if (IsNotShowingDialog && isKeyboardEscapeKeyDown && !is_keyboard_escape_key_pressed)
            {
                switch (gameMenuState)
                {
                    case EGameMenuState.Playing:
                        GameMenuState = EGameMenuState.GameMenu;
                        break;
                    case EGameMenuState.ShopMenu:
                    case EGameMenuState.GameMenu:
                        GameMenuState = EGameMenuState.Playing;
                        break;
                }
            }
            isKeyboardEscapeKeyDown = is_keyboard_escape_key_pressed;
        }
    }
}
