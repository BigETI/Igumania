using Igumania.Objects;
using UnityEngine;

namespace Igumania.Controllers
{
    public class CharacterDialogTriggerControllerScript : MonoBehaviour, ICharacterDialogTriggerController
    {
        [SerializeField]
        private ECharacterDialogTriggerMethod characterDialogTriggerMethod = ECharacterDialogTriggerMethod.None;

        [SerializeField]
        private DialogEventObjectScript dialogEvent;

        public ECharacterDialogTriggerMethod CharacterDialogTriggerMethod
        {
            get => characterDialogTriggerMethod;
            set => characterDialogTriggerMethod = value;
        }

        public DialogEventObjectScript DialogEvent
        {
            get => dialogEvent;
            set => dialogEvent = value;
        }

        public void TriggerCharacterDialog()
        {
            if (dialogEvent)
            {
                CharacterDialogs.ShowDialog(dialogEvent);
            }
        }

        private void Awake()
        {
            if (characterDialogTriggerMethod == ECharacterDialogTriggerMethod.Awake)
            {
                TriggerCharacterDialog();
            }
        }

        private void OnEnable()
        {
            if (characterDialogTriggerMethod == ECharacterDialogTriggerMethod.Enable)
            {
                TriggerCharacterDialog();
            }
        }

        private void OnDisable()
        {
            if (characterDialogTriggerMethod == ECharacterDialogTriggerMethod.Disable)
            {
                TriggerCharacterDialog();
            }
        }

        private void Start()
        {
            if (characterDialogTriggerMethod == ECharacterDialogTriggerMethod.Start)
            {
                TriggerCharacterDialog();
            }
        }

        private void OnDestroy()
        {
            if (characterDialogTriggerMethod == ECharacterDialogTriggerMethod.Destroy)
            {
                TriggerCharacterDialog();
            }
        }
    }
}
