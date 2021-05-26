using UnityEngine;

namespace Igumania
{
    public interface IItemObject : IScriptableObject
    {
        string ItemName { get; }

        string ItemDescription { get; }

        Sprite IconSprite { get; }

        ulong Cost { get; }

        long ProfitAddition { get; }

        float ProfitMultiplierAddition { get; }
    }
}
