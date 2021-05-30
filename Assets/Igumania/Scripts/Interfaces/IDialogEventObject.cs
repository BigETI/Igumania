using Igumania.Objects;

namespace Igumania
{
    public interface IDialogEventObject : IScriptableObject
    {
        bool IsAnOneTimeEvent { get; }

        CharacterObjectScript Character { get; }

        string Message { get; }
    }
}
