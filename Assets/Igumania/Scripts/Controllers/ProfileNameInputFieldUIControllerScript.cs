using TMPro;
using UnityEngine;
using UnityEngine.Events;

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

        public TMP_InputField ProfileNameInputField
        {
            get => profileNameInputField;
            set => profileNameInputField = value;
        }

        public event NewProfileCreatedDelegate OnNewProfileCreated;

        public event NewProfileCreationFailedDelegate OnNewProfileCreationFailed;

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
                }
            }
        }
    }
}
