using Igumania.Objects;
using UnityEngine;

namespace Igumania
{
    public interface ICharacterDialogData
    {
        CharacterObjectScript Character { get; }

        event DialogShownDelegate OnDialogShown;

        event CharacterNameUpdatedDelegate OnCharacterNameUpdated;

        event CharacterAvatarUpdatedDelegate OnCharacterAvatarUpdated;

        event DialogStartedDelegate OnDialogStarted;

        event DialogCharacterAppendedDelegate OnDialogCharacterAppended;

        event DialogWaitingStartedDelegate OnDialogWaitingStarted;

        event DialogHiddenDelegate OnDialogHidden;

        void InvokeDialogShownEvent();

        void InvokeCharacterNameUpdatedEvent(string characterName);

        void InvokeCharacterAvatarUpdatedEvent(Sprite avatarSprite);

        void InvokeDialogStartedEvent(string message);

        void InvokeDialogCharactersAppendedEvent(string message);

        void InvokeDialogWaitingStartedDelegate(string message);

        void InvokeDialogHiddenDelegate();
    }
}
