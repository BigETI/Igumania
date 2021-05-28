using UnityTranslator.Objects;

namespace Igumania
{
    public interface IGameMenuController : IBehaviour
    {
        StringTranslationObjectScript QuitGameTitleStringTranslation { get; set; }

        StringTranslationObjectScript QuitGameMessageStringTranslation { get; set; }

        EGameMenuState GameMenuState { get; set; }

        bool IsNotShowingDialog { get; }

        event PlayStartedDelegate OnPlayStarted;

        event ShopMenuShownDelegate OnShopMenuShown;

        event SelectRobotMenuShownDelegate OnSelectRobotMenuShown;

        event GameMenuShownDelegate OnGameMenuShown;

        event PlayStoppedDelegate OnPlayStopped;

        event ShopMenuHiddenDelegate OnShopMenuHidden;

        event SelectRobotMenuHiddenDelegate OnSelectRobotMenuHidden;

        event GameMenuHiddenDelegate OnGameMenuHidden;

        void ResumeGame();

        void ShowShopMenu();

        void ShowSelectRobotMenu();

        void ShowGameMenu();

        void ToggleGameMenu();

        void RequestShowingMainMenu();

        void ShowMainMenu();

        void SaveGame();
    }
}
