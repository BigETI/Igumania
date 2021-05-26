namespace Igumania
{
    public interface IIntroMenuController
    {
        bool IsNotShowingMainMenu { get; }

        void ShowMainMenu();
    }
}
