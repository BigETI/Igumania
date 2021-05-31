using Igumania.Managers;
using Igumania.Objects;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Igumania
{
    public static class CharacterDialogs
    {
        public static EDialogState DialogState => CharacterDialogManagerScript.Instance ? CharacterDialogManagerScript.Instance.DialogState : EDialogState.Hidden;

        public static void EnqueueShowingDialog(CharacterObjectScript character, string message)
        {
            if (!character)
            {
                throw new ArgumentNullException(nameof(character));
            }
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }
            if (CharacterDialogManagerScript.Instance)
            {
                CharacterDialogManagerScript.Instance.EnqueueShowingDialog(character, message);
            }
        }

        public static void ShowDialog(DialogEventObjectScript dialogEvent)
        {
            if (!dialogEvent)
            {
                throw new ArgumentNullException(nameof(dialogEvent));
            }
            IProfile profile = GameManager.SelectedProfile;
            bool is_safe = true;
            HashSet<string> watch_dog = new HashSet<string>();
            for (DialogEventObjectScript current_dialog_event = dialogEvent; current_dialog_event; current_dialog_event = current_dialog_event.NextDialogEvent)
            {
                if (!watch_dog.Add(current_dialog_event.name))
                {
                    is_safe = false;
                    Debug.LogError($"Dialog event \"{ dialogEvent.name }\" at \"{ current_dialog_event.name }\" starts to loop into itself.");
                    break;
                }
            }
            watch_dog.Clear();
            if (is_safe)
            {
                for (DialogEventObjectScript current_dialog_event = dialogEvent; current_dialog_event; current_dialog_event = current_dialog_event.NextDialogEvent)
                {
                    if (!current_dialog_event.IsAnOneTimeEvent || ((profile != null) && profile.AddPassedDialogEvent(current_dialog_event)))
                    {
                        EnqueueShowingDialog(current_dialog_event.Character, current_dialog_event.Message);
                    }
                }
            }
        }

        public static void ShowDialog(CharacterObjectScript character, string message)
        {
            if (!character)
            {
                throw new ArgumentNullException(nameof(character));
            }
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }
            if (CharacterDialogManagerScript.Instance)
            {
                CharacterDialogManagerScript.Instance.ShowDialog(character, message);
            }
        }
    }
}
