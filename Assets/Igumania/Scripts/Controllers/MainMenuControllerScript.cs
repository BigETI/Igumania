using UnityDialog;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityTranslator.Objects;

namespace Igumania.Controllers
{
    public class MainMenuControllerScript : MonoBehaviour, IMainMenuController
    {
        [SerializeField]
        private StringTranslationObjectScript exitGameTitleStringTranslation = default;

        [SerializeField]
        private StringTranslationObjectScript exitGameMessageStringTranslation = default;

        [SerializeField]
        private UnityEvent onMainMenuShown = default;

        [SerializeField]
        private UnityEvent onMainMenuHidden = default;

        [SerializeField]
        private UnityEvent onProfilesMenuShown = default;

        [SerializeField]
        private UnityEvent onProfilesMenuHidden = default;

        [SerializeField]
        private UnityEvent onSettingsMenuShown = default;

        [SerializeField]
        private UnityEvent onSettingsMenuHidden = default;

        [SerializeField]
        private UnityEvent onCreditsMenuShown = default;

        [SerializeField]
        private UnityEvent onCreditsMenuHidden = default;

        private EMainMenuState mainMenuState = EMainMenuState.Nothing;

        private bool isKeyboardEscapeKeyDown;

        private bool isTouchscreenPressDown;

        public StringTranslationObjectScript ExitGameTitleStringTranslation
        {
            get => exitGameTitleStringTranslation;
            set => exitGameTitleStringTranslation = value;
        }

        public StringTranslationObjectScript ExitGameMessageStringTranslation
        {
            get => exitGameMessageStringTranslation;
            set => exitGameMessageStringTranslation = value;
        }

        public EMainMenuState MainMenuState
        {
            get => mainMenuState;
            set
            {
                if ((mainMenuState != value) && (value != EMainMenuState.Nothing))
                {
                    EMainMenuState old_main_menu_state = mainMenuState;
                    mainMenuState = value;
                    switch (old_main_menu_state)
                    {
                        case EMainMenuState.MainMenu:
                            if (onMainMenuHidden != null)
                            {
                                onMainMenuHidden.Invoke();
                            }
                            OnMainMenuHidden?.Invoke();
                            break;
                        case EMainMenuState.ProfilesMenu:
                            if (onProfilesMenuHidden != null)
                            {
                                onProfilesMenuHidden.Invoke();
                            }
                            OnProfilesMenuHidden?.Invoke();
                            break;
                        case EMainMenuState.SettingsMenu:
                            if (onSettingsMenuHidden != null)
                            {
                                onSettingsMenuHidden.Invoke();
                            }
                            OnSettingsMenuHidden?.Invoke();
                            break;
                        case EMainMenuState.CreditsMenu:
                            if (onCreditsMenuHidden != null)
                            {
                                onCreditsMenuHidden.Invoke();
                            }
                            OnCreditsMenuHidden?.Invoke();
                            break;
                    }
                    switch (mainMenuState)
                    {
                        case EMainMenuState.MainMenu:
                            if (onMainMenuShown != null)
                            {
                                onMainMenuShown.Invoke();
                            }
                            OnMainMenuShown?.Invoke();
                            break;
                        case EMainMenuState.ProfilesMenu:
                            if (onProfilesMenuShown != null)
                            {
                                onProfilesMenuShown.Invoke();
                            }
                            OnProfilesMenuShown?.Invoke();
                            break;
                        case EMainMenuState.SettingsMenu:
                            if (onSettingsMenuShown != null)
                            {
                                onSettingsMenuShown.Invoke();
                            }
                            OnSettingsMenuShown?.Invoke();
                            break;
                        case EMainMenuState.CreditsMenu:
                            if (onCreditsMenuShown != null)
                            {
                                onCreditsMenuShown.Invoke();
                            }
                            OnCreditsMenuShown?.Invoke();
                            break;
                    }
                }
            }
        }

        public bool IsNotShowingDialog { get; private set; } = true;

        public event MainMenuShownDelegate OnMainMenuShown;

        public event MainMenuHiddenDelegate OnMainMenuHidden;

        public event ProfilesMenuShownDelegate OnProfilesMenuShown;

        public event ProfilesMenuHiddenDelegate OnProfilesMenuHidden;

        public event SettingsMenuShownDelegate OnSettingsMenuShown;

        public event SettingsMenuHiddenDelegate OnSettingsMenuHidden;

        public event CreditsMenuShownDelegate OnCreditsMenuShown;

        public event CreditsMenuHiddenDelegate OnCreditsMenuHidden;

        public void ShowMainMenu() => MainMenuState = EMainMenuState.MainMenu;

        public void ShowProfilesMenu() => MainMenuState = EMainMenuState.ProfilesMenu;

        public void ShowSettingsMenu() => MainMenuState = EMainMenuState.SettingsMenu;

        public void ShowCreditsMenu() => MainMenuState = EMainMenuState.CreditsMenu;

        public void RequestExitingGame()
        {
            IsNotShowingDialog = false;
            Dialogs.Show
            (
                exitGameTitleStringTranslation ? exitGameTitleStringTranslation.ToString() : string.Empty,
                exitGameMessageStringTranslation ? exitGameMessageStringTranslation.ToString() : string.Empty,
                EDialogType.Information,
                EDialogButtons.YesNo,
                (response, _) =>
                {
                    if (response == EDialogResponse.Yes)
                    {
                        ExitGame();
                    }
                    IsNotShowingDialog = true;
                }
            );
        }

        public void ExitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        private void Start()
        {
            GameManager.UnloadSelectedProfile();
            MainMenuState = EMainMenuState.MainMenu;
        }

        private void Update()
        {
            Keyboard keyboard = Keyboard.current;
            Touchscreen touchscreen = Touchscreen.current;
            bool is_keyboard_key_down = IsNotShowingDialog && (keyboard != null) && keyboard.escapeKey.isPressed;
            bool is_touchscreen_press_down = IsNotShowingDialog && (touchscreen != null) && touchscreen.press.isPressed;
            if (IsNotShowingDialog && ((isKeyboardEscapeKeyDown && !is_keyboard_key_down) || (isTouchscreenPressDown && !is_touchscreen_press_down)))
            {
                switch (mainMenuState)
                {
                    case EMainMenuState.MainMenu:
                        RequestExitingGame();
                        break;
                    case EMainMenuState.SettingsMenu:
                    case EMainMenuState.ProfilesMenu:
                        MainMenuState = EMainMenuState.MainMenu;
                        break;
                }
            }
            isKeyboardEscapeKeyDown = is_keyboard_key_down;
            isTouchscreenPressDown = is_touchscreen_press_down;
        }
    }
}
