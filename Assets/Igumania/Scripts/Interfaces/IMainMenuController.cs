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

        event SettingsMenuShownDelegate OnSettingsMenuShown;

        event SettingsMenuHiddenDelegate OnSettingsMenuHidden;

        event ProfilesMenuShownDelegate OnProfilesMenuShown;

        event ProfilesMenuHiddenDelegate OnProfilesMenuHidden;

        void ShowMainMenu();

        void ShowSettingsMenu();

        void ShowProfilesMenu();

        void RequestExitingGame();

        void ExitGame();
    }
}
