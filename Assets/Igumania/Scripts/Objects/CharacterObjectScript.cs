using UnityEngine;
using UnityTranslator.Objects;

namespace Igumania.Objects
{
    [CreateAssetMenu(fileName = "Character", menuName = "Igumania/Character")]
    public class CharacterObjectScript : ScriptableObject, ICharacterObject
    {
        [SerializeField]
        private StringTranslationObjectScript characterNameStringTranslation = default;

        [SerializeField]
        private SpriteTranslationObjectScript avatarSpriteTranslation = default;

        public string CharacterName => characterNameStringTranslation ? characterNameStringTranslation.ToString() : string.Empty;

        public Sprite AvatarSprite => avatarSpriteTranslation ? avatarSpriteTranslation.Sprite : null;
    }
}
