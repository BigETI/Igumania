using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnitySceneLoaderManager;

namespace Igumania.Controllers
{
    public class ProfileNameInputFieldUIControllerScript : MonoBehaviour, IProfileNameInputFieldUIController
    {
        [SerializeField]
        private TMP_InputField profileNameInputField = default;

        [SerializeField]
        private UnityEvent onNewProfileCreated = default;

        [SerializeField]
        private UnityEvent onNewProfileCreationFailed = default;

        [SerializeField]
        private UnityEvent onNewProfileCreationCanceled = default;

        private bool isKeyboardEscapeKeyDown;

        public TMP_InputField ProfileNameInputField
        {
            get => profileNameInputField;
            set => profileNameInputField = value;
        }

        public event NewProfileCreatedDelegate OnNewProfileCreated;

        public event NewProfileCreationFailedDelegate OnNewProfileCreationFailed;

        public event NewProfileCreationCanceledDelegate OnNewProfileCreationCanceled;

        public void CreateNewProfile()
        {
            if (profileNameInputField)
            {
                string profile_name = profileNameInputField.text;
                if (string.IsNullOrWhiteSpace(profile_name))
                {
                    if (onNewProfileCreationFailed != null)
                    {
                        onNewProfileCreationFailed.Invoke();
                    }
                    OnNewProfileCreationFailed?.Invoke(GameManager.SelectedProfileIndex);
                }
                else
                {
                    Profiles.CreateNewProfile(GameManager.SelectedProfileIndex, profile_name);
                    GameManager.ReloadSelectedProfile();
                    if (onNewProfileCreated != null)
                    {
                        onNewProfileCreated.Invoke();
                    }
                    OnNewProfileCreated?.Invoke(GameManager.SelectedProfile);
                    SceneLoaderManager.LoadScenes("GameScene");
                }
            }
        }

        public void Cancel()
        {
            if (onNewProfileCreationCanceled != null)
            {
                onNewProfileCreationCanceled.Invoke();
            }
            OnNewProfileCreationCanceled?.Invoke(GameManager.SelectedProfileIndex);
        }

        private void Update()
        {
            Keyboard keyboard = Keyboard.current;
            bool is_keyboard_escape_key_down = (keyboard != null) && keyboard.escapeKey.isPressed;
            if (isKeyboardEscapeKeyDown && !is_keyboard_escape_key_down)
            {
                Cancel();
            }
            isKeyboardEscapeKeyDown = is_keyboard_escape_key_down;
        }
    }
}
