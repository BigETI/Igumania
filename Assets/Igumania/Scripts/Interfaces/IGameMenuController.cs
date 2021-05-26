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

        event GameMenuShownDelegate OnGameMenuShown;

        event PlayStoppedDelegate OnPlayStopped;

        event ShopMenuHiddenDelegate OnShopMenuHidden;

        event GameMenuHiddenDelegate OnGameMenuHidden;

        void ResumeGame();

        void ShowShopMenu();

        void ShowGameMenu();

        void RequestShowingMainMenu();

        void ShowMainMenu();
    }
}
