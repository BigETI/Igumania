using UnityEngine;
using UnityTranslator.Objects;

namespace Igumania.Objects
{
    public abstract class AItemObjectScript : ScriptableObject, IItemObject
    {
        [SerializeField]
        private StringTranslationObjectScript itemNameStringTranslation = default;

        [SerializeField]
        private StringTranslationObjectScript itemDescriptionStringTranslation = default;

        [SerializeField]
        private SpriteTranslationObjectScript iconSpriteTranslation = default;

        [SerializeField]
        private string url = string.Empty;

        [SerializeField]
        private ulong cost;

        [SerializeField]
        private long profitAddition;

        [SerializeField]
        private float profitMultiplierAddition;

        public string ItemName => itemNameStringTranslation ? itemNameStringTranslation.ToString() : string.Empty;

        public string ItemDescription => itemDescriptionStringTranslation ? itemDescriptionStringTranslation.ToString() : string.Empty;

        public Sprite IconSprite => iconSpriteTranslation ? iconSpriteTranslation.Sprite : null;

        public string URL => url ?? string.Empty;

        public ulong Cost => cost;

        public long ProfitAddition => profitAddition;

        public float ProfitMultiplierAddition => profitMultiplierAddition;
    }
}
