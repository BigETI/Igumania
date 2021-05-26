using UnityTranslator.Objects;

namespace Igumania
{
    public interface IMainMenuController : IBehaviour
    {
        StringTranslationObjectScript ExitGameTitleStringTranslation { get; set; }

        StringTranslationObjectScript ExitGameMessageStringTranslation { get; set; }

        EMainMenuState MainMenuState { get; set; }

        bool IsNotShowingDialog { get; }

        event MainMenuShownDelegate OnMainMenuShown;

        event MainMenuHiddenDelegate OnMainMenuHidden;

        event ProfilesMenuShownDelegate OnProfilesMenuShown;

        event ProfilesMenuHiddenDelegate OnProfilesMenuHidden;

        event SettingsMenuShownDelegate OnSettingsMenuShown;

        event SettingsMenuHiddenDelegate OnSettingsMenuHidden;

        event CreditsMenuShownDelegate OnCreditsMenuShown;

        event CreditsMenuHiddenDelegate OnCreditsMenuHidden;

        void ShowMainMenu();

        void ShowProfilesMenu();

        void ShowSettingsMenu();

        void ShowCreditsMenu();

        void RequestExitingGame();

        void ExitGame();
    }
}
