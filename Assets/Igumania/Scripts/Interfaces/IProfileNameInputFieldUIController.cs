using TMPro;

namespace Igumania
{
    public interface IProfileNameInputFieldUIController : IBehaviour
    {
        TMP_InputField ProfileNameInputField { get; set; }

        event NewProfileCreatedDelegate OnNewProfileCreated;

        event NewProfileCreationFailedDelegate OnNewProfileCreationFailed;

        void CreateNewProfile();
    }
}
