namespace Igumania
{
    public interface IIntroMenuController : IBehaviour
    {
        bool IsNotShowingMainMenu { get; }

        void ShowMainMenu();
    }
}
