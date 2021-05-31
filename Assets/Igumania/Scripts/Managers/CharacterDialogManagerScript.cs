using Igumania.Data;
using Igumania.Objects;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityTiming.Data;

namespace Igumania.Managers
{
    public class CharacterDialogManagerScript : ASingletonManagerScript<CharacterDialogManagerScript>, ICharacterDialogManager
    {
        private readonly Dictionary<string, CharacterDialogData> characterDialogLookup = new Dictionary<string, CharacterDialogData>();

        private readonly Queue<(CharacterObjectScript, string)> characterDialogQueue = new Queue<(CharacterObjectScript, string)>();

        private readonly StringBuilder currentShownMessageStringBuilder = new StringBuilder();

        [SerializeField]
        private TimingData messageCharacterWriteTiming = new TimingData(0.015625f);

        [SerializeField]
        private CharacterDialogData[] characterDialogs;

        private bool isAnyKeyboardKeyDown;

        private bool isLeftMouseButtonDown;

        private bool isRightMouseButtonDown;

        private bool isMiddleMouseButtonDown;

        private bool isForwardMouseButtonDown;

        private bool isBackMouseButtonDown;

        private bool isTouchscreenPressDown;

        public IEnumerable<CharacterDialogData> CharacterDialogs => characterDialogLookup.Values;

        public IReadOnlyCollection<(CharacterObjectScript, string)> CharacterDialogQueue => characterDialogQueue;

        public string CurrentlyShownMessage => currentShownMessageStringBuilder.ToString();

        public TimingData MessageCharacterWriteTiming
        {
            get => messageCharacterWriteTiming;
            set => messageCharacterWriteTiming = value;
        }

        public CharacterObjectScript CurrentCharacter { get; private set; }

        public string CurrentMessage { get; private set; } = string.Empty;

        public EDialogState DialogState { get; private set; } = EDialogState.Hidden;

        private void SetWaitingDialogState()
        {
            string key = CurrentCharacter.name;
            currentShownMessageStringBuilder.Clear();
            currentShownMessageStringBuilder.Append(CurrentMessage);
            DialogState = EDialogState.Waiting;
            if (characterDialogLookup.ContainsKey(key))
            {
                characterDialogLookup[key].InvokeDialogWaitingStartedDelegate(CurrentMessage);
            }
            messageCharacterWriteTiming.Reset();
        }

        private void ShowDialogInternally(CharacterObjectScript character, string message)
        {
            string key = character.name;
            HideDialog();
            CurrentCharacter = character;
            currentShownMessageStringBuilder.Clear();
            CurrentMessage = message ?? throw new ArgumentNullException(nameof(message));
            DialogState = EDialogState.Started;
            if (characterDialogLookup.ContainsKey(key))
            {
                CharacterDialogData character_dialog = characterDialogLookup[key];
                character_dialog.InvokeDialogShownEvent();
                character_dialog.InvokeCharacterNameUpdatedEvent(character.CharacterName);
                character_dialog.InvokeCharacterAvatarUpdatedEvent(character.AvatarSprite);
                foreach (char message_character in message)
                {
                    if (char.IsWhiteSpace(message_character))
                    {
                        currentShownMessageStringBuilder.Append(message_character);
                    }
                    else
                    {
                        break;
                    }
                }
                if (currentShownMessageStringBuilder.Length > 0)
                {
                    DialogState = EDialogState.Writing;
                    character_dialog.InvokeDialogStartedEvent(CurrentlyShownMessage);
                }
            }
        }

        private void SetNextDialog()
        {
            if (characterDialogQueue.Count > 0)
            {
                (CharacterObjectScript, string) character_dialog = characterDialogQueue.Dequeue();
                ShowDialogInternally(character_dialog.Item1, character_dialog.Item2);
            }
            else
            {
                HideDialog();
            }
        }

        public void EnqueueShowingDialog(CharacterObjectScript character, string message)
        {
            if (!character)
            {
                throw new ArgumentNullException(nameof(character));
            }
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }
            if (DialogState == EDialogState.Hidden)
            {
                ShowDialogInternally(character, message);
            }
            else
            {
                characterDialogQueue.Enqueue((character, message));
            }
        }

        public void ShowDialog(CharacterObjectScript character, string message)
        {
            if (!character)
            {
                throw new ArgumentNullException(nameof(character));
            }
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }
            characterDialogQueue.Clear();
            ShowDialogInternally(character, message);
        }

        public void SkipDialog()
        {
            switch (DialogState)
            {
                case EDialogState.Started:
                case EDialogState.Writing:
                    SetWaitingDialogState();
                    break;
                case EDialogState.Waiting:
                    SetNextDialog();
                    break;
            }
        }

        public void HideDialog()
        {
            if (DialogState != EDialogState.Hidden)
            {
                string key = CurrentCharacter.name;
                currentShownMessageStringBuilder.Clear();
                CurrentCharacter = null;
                CurrentMessage = string.Empty;
                DialogState = EDialogState.Hidden;
                if (characterDialogLookup.ContainsKey(key))
                {
                    characterDialogLookup[key].InvokeDialogHiddenDelegate();
                }
            }
        }

        private void Start()
        {
            characterDialogLookup.Clear();
            if (characterDialogs != null)
            {
                foreach (CharacterDialogData character_dialog in characterDialogs)
                {
                    if (character_dialog.Character)
                    {
                        string key = character_dialog.Character.name;
                        if (characterDialogLookup.ContainsKey(key))
                        {
                            Debug.LogError($"Found duplicated character dialog entry \"{ key }\".", this);
                        }
                        else
                        {
                            characterDialogLookup.Add(key, character_dialog);
                        }
                    }
                    else
                    {
                        Debug.LogError("Please add a character to a character dialog or remove it if not needed at all.", this);
                    }
                }
            }
        }

        private void OnDestroy()
        {
            HideDialog();
            characterDialogLookup.Clear();
        }

        private void Update()
        {
            switch (DialogState)
            {
                case EDialogState.Started:
                case EDialogState.Writing:
                    for (int tick_index = 0, tick_count = messageCharacterWriteTiming.ProceedUpdate(false, false); tick_index < tick_count; tick_index++)
                    {
                        if (currentShownMessageStringBuilder.Length < CurrentMessage.Length)
                        {
                            string key = CurrentCharacter.name;
                            for (int current_message_character_index = currentShownMessageStringBuilder.Length; current_message_character_index < CurrentMessage.Length; current_message_character_index++)
                            {
                                char current_message_character = CurrentMessage[current_message_character_index];
                                currentShownMessageStringBuilder.Append(current_message_character);
                                if (!char.IsWhiteSpace(current_message_character))
                                {
                                    break;
                                }
                            }
                            if (DialogState == EDialogState.Writing)
                            {
                                if (characterDialogLookup.ContainsKey(key))
                                {
                                    characterDialogLookup[key].InvokeDialogCharactersAppendedEvent(CurrentlyShownMessage);
                                }
                            }
                            else
                            {
                                DialogState = EDialogState.Writing;
                                if (characterDialogLookup.ContainsKey(key))
                                {
                                    characterDialogLookup[key].InvokeDialogStartedEvent(CurrentlyShownMessage);
                                }
                            }
                            if (currentShownMessageStringBuilder.Length >= CurrentMessage.Length)
                            {
                                SetWaitingDialogState();
                                break;
                            }
                        }
                        else
                        {
                            if (DialogState != EDialogState.Writing)
                            {
                                DialogState = EDialogState.Writing;
                                string key = CurrentCharacter.name;
                                if (characterDialogLookup.ContainsKey(key))
                                {
                                    characterDialogLookup[key].InvokeDialogStartedEvent(CurrentlyShownMessage);
                                }
                            }
                            SetWaitingDialogState();
                            break;
                        }
                    }
                    break;
                case EDialogState.Waiting:
                    Keyboard keyboard = Keyboard.current;
                    Mouse mouse = Mouse.current;
                    Touchscreen touchscreen = Touchscreen.current;
                    bool is_keyboard_key_down = (keyboard != null) && keyboard.anyKey.isPressed;
                    bool is_left_mouse_button_down = (mouse != null) && mouse.leftButton.isPressed;
                    bool is_right_mouse_button_down = (mouse != null) && mouse.rightButton.isPressed;
                    bool is_middle_mouse_button_down = (mouse != null) && mouse.middleButton.isPressed;
                    bool is_forward_mouse_button_down = (mouse != null) && mouse.forwardButton.isPressed;
                    bool is_back_mouse_button_down = (mouse != null) && mouse.backButton.isPressed;
                    bool is_touchscreen_press_down = (touchscreen != null) && touchscreen.press.isPressed;
                    if
                    (
                        (isAnyKeyboardKeyDown && !is_keyboard_key_down) ||
                        (isLeftMouseButtonDown && !is_left_mouse_button_down) ||
                        (isRightMouseButtonDown && !is_right_mouse_button_down) ||
                        (isMiddleMouseButtonDown && !is_middle_mouse_button_down) ||
                        (isForwardMouseButtonDown && !is_forward_mouse_button_down) ||
                        (isBackMouseButtonDown && !is_back_mouse_button_down) ||
                        (isTouchscreenPressDown && !is_touchscreen_press_down)
                    )
                    {
                        SkipDialog();
                    }
                    isAnyKeyboardKeyDown = is_keyboard_key_down;
                    isLeftMouseButtonDown = is_left_mouse_button_down;
                    isRightMouseButtonDown = is_right_mouse_button_down;
                    isMiddleMouseButtonDown = is_middle_mouse_button_down;
                    isForwardMouseButtonDown = is_forward_mouse_button_down;
                    isBackMouseButtonDown = is_back_mouse_button_down;
                    isTouchscreenPressDown = is_touchscreen_press_down;
                    break;
            }
            if (DialogState != EDialogState.Waiting)
            {
                isAnyKeyboardKeyDown = false;
                isLeftMouseButtonDown = false;
                isRightMouseButtonDown = false;
                isMiddleMouseButtonDown = false;
                isForwardMouseButtonDown = false;
                isBackMouseButtonDown = false;
                isTouchscreenPressDown = false;
            }
        }

#if UNITY_EDITOR
        private void OnValidate() => messageCharacterWriteTiming.TickTime = Mathf.Max(messageCharacterWriteTiming.TickTime, float.Epsilon + float.Epsilon);
#endif
    }
}
