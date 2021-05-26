using System;
using TMPro;
using UnityDialog;
using UnityEngine;
using UnityEngine.Events;
using UnitySceneLoaderManager;
using UnityTranslator.Objects;

namespace Igumania.Controllers
{
    public class ProfilePanelUIControllerScript : MonoBehaviour, IProfilePanelUIController
    {
        private static readonly string defaultProductionLevelStringFormat = "{0}";

        private static readonly string defaultMoneyStringFormat = "{0}";

        [SerializeField]
        private byte profileIndex;

        [SerializeField]
        private string productionLevelStringFormat = defaultProductionLevelStringFormat;

        [SerializeField]
        private string moneyStringFormat = defaultMoneyStringFormat;

        [SerializeField]
        private StringTranslationObjectScript noProfileStringTranslation = default;

        [SerializeField]
        private StringTranslationObjectScript productionLevelStringFormatStringTranslation = default;

        [SerializeField]
        private StringTranslationObjectScript moneyStringFormatStringTranslation = default;

        [SerializeField]
        private StringTranslationObjectScript deleteProfileTitleStringTranslation = default;

        [SerializeField]
        private StringTranslationObjectScript deleteProfileMessageStringTranslation = default;

        [SerializeField]
        private TextMeshProUGUI profileNameText = default;

        [SerializeField]
        private TextMeshProUGUI productionLevelText = default;

        [SerializeField]
        private TextMeshProUGUI moneyText = default;

        [SerializeField]
        private UnityEvent onProfileLoaded = default;

        [SerializeField]
        private UnityEvent onProfileUnloaded = default;

        [SerializeField]
        private UnityEvent onProfileNameRequested = default;

        public string ProductionLevelStringFormat
        {
            get => productionLevelStringFormat ?? defaultProductionLevelStringFormat;
            set => productionLevelStringFormat = value ?? throw new ArgumentNullException(nameof(value));
        }

        public string MoneyStringFormat
        {
            get => moneyStringFormat ?? defaultMoneyStringFormat;
            set => moneyStringFormat = value ?? throw new ArgumentNullException(nameof(value));
        }

        public StringTranslationObjectScript NoProfileStringTranslation
        {
            get => noProfileStringTranslation;
            set => noProfileStringTranslation = value;
        }

        public StringTranslationObjectScript ProductionLevelStringFormatStringTranslation
        {
            get => productionLevelStringFormatStringTranslation;
            set => productionLevelStringFormatStringTranslation = value;
        }

        public StringTranslationObjectScript MoneyStringFormatStringTranslation
        {
            get => moneyStringFormatStringTranslation;
            set => moneyStringFormatStringTranslation = value;
        }

        public StringTranslationObjectScript DeleteProfileTitleStringTranslation
        {
            get => deleteProfileTitleStringTranslation;
            set => deleteProfileTitleStringTranslation = value;
        }

        public StringTranslationObjectScript DeleteProfileMessageStringTranslation
        {
            get => deleteProfileMessageStringTranslation;
            set => deleteProfileMessageStringTranslation = value;
        }

        public TextMeshProUGUI ProfileNameText
        {
            get => profileNameText;
            set => profileNameText = value;
        }

        public TextMeshProUGUI ProductionLevelText
        {
            get => productionLevelText;
            set => productionLevelText = value;
        }

        public TextMeshProUGUI MoneyText
        {
            get => moneyText;
            set => moneyText = value;
        }

        public IProfile Profile { get; private set; }

        public event ProfileLoadedDelegate OnProfileLoaded;

        public event ProfileUnloadedDelegate OnProfileUnloaded;

        public event ProfileNameRequestedDelegate OnProfileNameRequested;

        private void InvokeProfileUnloadedEvent()
        {
            if (onProfileUnloaded != null)
            {
                onProfileUnloaded.Invoke();
            }
            OnProfileUnloaded?.Invoke(profileIndex);
        }

        public void SelectProfile()
        {
            GameManager.SelectedProfileIndex = profileIndex;
            if (GameManager.SelectedProfile == null)
            {
                if (onProfileNameRequested != null)
                {
                    onProfileNameRequested.Invoke();
                }
                OnProfileNameRequested?.Invoke(profileIndex);
            }
            else
            {
                if (onProfileLoaded != null)
                {
                    onProfileLoaded.Invoke();
                }
                OnProfileLoaded?.Invoke(GameManager.SelectedProfile);
                SceneLoaderManager.LoadScenes("GameScene");
            }
        }

        public void RequestDeletingProfile()
        {
            if (Profiles.IsProfileAvailable(profileIndex))
            {
                Dialogs.Show
                (
                    deleteProfileTitleStringTranslation ? deleteProfileTitleStringTranslation.ToString() : string.Empty,
                    deleteProfileMessageStringTranslation ? deleteProfileMessageStringTranslation.ToString() : string.Empty,
                    EDialogType.Warning,
                    EDialogButtons.YesNo,
                    (response, _) =>
                    {
                        if (response == EDialogResponse.Yes)
                        {
                            Profiles.RemoveProfile(profileIndex);
                            InvokeProfileUnloadedEvent();
                        }
                    }
                );
            }
        }

        private void Start()
        {
            Profile = Profiles.LoadProfile(profileIndex);
            if (profileNameText)
            {
                profileNameText.text = (Profile == null) ? (noProfileStringTranslation ? noProfileStringTranslation.ToString() : string.Empty) : Profile.Name;
            }
            else
            {
                Debug.LogError("Please assign a profile name text to this component.", this);
            }
            if (productionLevelText)
            {
                productionLevelText.text = (Profile == null) ? string.Empty : string.Format(productionLevelStringFormatStringTranslation ? productionLevelStringFormatStringTranslation.ToString() : (productionLevelStringFormat ?? defaultProductionLevelStringFormat), Profile.ProductionLevel);
            }
            else
            {
                Debug.LogError("Please assign a production level text to this component.", this);
            }
            if (moneyText)
            {
                moneyText.text = (Profile == null) ? string.Empty : string.Format(moneyStringFormatStringTranslation ? moneyStringFormatStringTranslation.ToString() : (moneyStringFormat ?? defaultMoneyStringFormat), Profile.Money);
            }
            else
            {
                Debug.LogError("Please assign a money text to this component.", this);
            }
            if (Profile == null)
            {
                InvokeProfileUnloadedEvent();
            }
            else
            {
                if (onProfileLoaded != null)
                {
                    onProfileLoaded.Invoke();
                }
                OnProfileLoaded?.Invoke(Profile);
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            productionLevelStringFormat = productionLevelStringFormatStringTranslation ? productionLevelStringFormatStringTranslation.ToString() : (productionLevelStringFormat ?? defaultProductionLevelStringFormat);
            moneyStringFormat = moneyStringFormatStringTranslation ? moneyStringFormatStringTranslation.ToString() : (moneyStringFormat ?? defaultMoneyStringFormat);
        }
#endif
    }
}
