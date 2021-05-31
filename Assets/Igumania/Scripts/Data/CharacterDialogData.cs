using Igumania.Objects;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Igumania.Data
{
    [Serializable]
    public struct CharacterDialogData : ICharacterDialogData
    {
        [SerializeField]
        private CharacterObjectScript character;

        [SerializeField]
        private UnityEvent onDialogShown;

        [SerializeField]
        private UnityEvent<string> onCharacterNameUpdated;

        [SerializeField]
        private UnityEvent<Sprite> onCharacterAvatarUpdated;

        [SerializeField]
        private UnityEvent<string> onDialogStarted;

        [SerializeField]
        private UnityEvent<string> onDialogCharacterAppended;

        [SerializeField]
        private UnityEvent<string> onDialogWaitingStarted;

        [SerializeField]
        private UnityEvent onDialogHidden;

        public CharacterObjectScript Character => character;

        public event DialogShownDelegate OnDialogShown;

        public event CharacterNameUpdatedDelegate OnCharacterNameUpdated;

        public event CharacterAvatarUpdatedDelegate OnCharacterAvatarUpdated;

        public event DialogStartedDelegate OnDialogStarted;

        public event DialogCharacterAppendedDelegate OnDialogCharacterAppended;

        public event DialogWaitingStartedDelegate OnDialogWaitingStarted;

        public event DialogHiddenDelegate OnDialogHidden;

        public void InvokeDialogShownEvent()
        {
            if (onDialogShown != null)
            {
                onDialogShown.Invoke();
            }
            OnDialogShown?.Invoke();
        }

        public void InvokeCharacterNameUpdatedEvent(string characterName)
        {
            if (characterName == null)
            {
                throw new ArgumentNullException(nameof(characterName));
            }
            if (onCharacterNameUpdated != null)
            {
                onCharacterNameUpdated.Invoke(characterName);
            }
            OnCharacterNameUpdated?.Invoke(characterName);
        }

        public void InvokeCharacterAvatarUpdatedEvent(Sprite avatarSprite)
        {
            if (onCharacterAvatarUpdated != null)
            {
                onCharacterAvatarUpdated.Invoke(avatarSprite);
            }
            OnCharacterAvatarUpdated?.Invoke(avatarSprite);
        }

        public void InvokeDialogStartedEvent(string message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }
            if (onDialogStarted != null)
            {
                onDialogStarted.Invoke(message);
            }
            OnDialogStarted?.Invoke(message);
        }

        public void InvokeDialogCharactersAppendedEvent(string message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }
            if (onDialogCharacterAppended != null)
            {
                onDialogCharacterAppended.Invoke(message);
            }
            OnDialogCharacterAppended?.Invoke(message);
        }

        public void InvokeDialogWaitingStartedDelegate(string message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }
            if (onDialogWaitingStarted != null)
            {
                onDialogWaitingStarted.Invoke(message);
            }
            OnDialogWaitingStarted?.Invoke(message);
        }

        public void InvokeDialogHiddenDelegate()
        {
            if (onDialogHidden != null)
            {
                onDialogHidden.Invoke();
            }
            OnDialogHidden?.Invoke();
        }
    }
}
