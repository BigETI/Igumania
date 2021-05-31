using Igumania.Objects;

namespace Igumania
{
    public interface ICharacterDialogTriggerController : IBehaviour
    {
        ECharacterDialogTriggerMethod CharacterDialogTriggerMethod { get; set; }

        DialogEventObjectScript DialogEvent { get; set; }

        void TriggerCharacterDialog();
    }
}
