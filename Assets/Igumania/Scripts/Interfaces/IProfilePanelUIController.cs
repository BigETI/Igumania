using TMPro;
using UnityTranslator.Objects;

namespace Igumania
{
    public interface IProfilePanelUIController : IBehaviour
    {
        string ProductionLevelStringFormat { get; set; }

        string MoneyStringFormat { get; set; }

        StringTranslationObjectScript NoProfileStringTranslation { get; set; }

        StringTranslationObjectScript ProductionLevelStringFormatStringTranslation { get; set; }

        StringTranslationObjectScript MoneyStringFormatStringTranslation { get; set; }

        StringTranslationObjectScript DeleteProfileTitleStringTranslation { get; set; }

        StringTranslationObjectScript DeleteProfileMessageStringTranslation { get; set; }

        TextMeshProUGUI ProfileNameText { get; set; }

        TextMeshProUGUI ProductionLevelText { get; set; }

        TextMeshProUGUI MoneyText { get; set; }

        IProfile Profile { get; }

        event ProfileLoadedDelegate OnProfileLoaded;

        event ProfileNameRequestedDelegate OnProfileNameRequested;

        void SelectProfile();

        void RequestDeletingProfile();
    }
}
