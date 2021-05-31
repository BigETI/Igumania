using UnityEngine;
using UnityTranslator.Objects;

namespace Igumania.Objects
{
    [CreateAssetMenu(fileName = "DialogEvent", menuName = "Igumania/Dialog event")]
    public class DialogEventObjectScript : ScriptableObject, IDialogEventObject
    {
        [SerializeField]
        private bool isAnOneTimeEvent = true;

        [SerializeField]
        private CharacterObjectScript character = default;

        [SerializeField]
        private StringTranslationObjectScript messageStringTranslation = default;

        [SerializeField]
        private DialogEventObjectScript nextDialogEvent = default;

        public bool IsAnOneTimeEvent => isAnOneTimeEvent;

        public CharacterObjectScript Character => character;

        public string Message => messageStringTranslation ? messageStringTranslation.ToString() : string.Empty;

        public DialogEventObjectScript NextDialogEvent => nextDialogEvent;
    }
}
