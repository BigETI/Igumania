using UnityEngine;

namespace Igumania
{
    public interface ICharacterObject : IScriptableObject
    {
        string CharacterName { get; }

        Sprite AvatarSprite { get; }
    }
}
