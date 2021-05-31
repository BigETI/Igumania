using Igumania.Data;
using Igumania.Objects;
using System.Collections.Generic;
using UnityTiming.Data;

namespace Igumania
{
    public interface ICharacterDialogManager : ISingletonManager
    {
        IEnumerable<CharacterDialogData> CharacterDialogs { get; }

        IReadOnlyCollection<(CharacterObjectScript, string)> CharacterDialogQueue { get; }

        string CurrentlyShownMessage { get; }

        TimingData MessageCharacterWriteTiming { get; set; }

        CharacterObjectScript CurrentCharacter { get; }

        string CurrentMessage { get; }

        EDialogState DialogState { get; }

        void EnqueueShowingDialog(CharacterObjectScript character, string message);

        void ShowDialog(CharacterObjectScript character, string message);

        void SkipDialog();

        void HideDialog();
    }
}
