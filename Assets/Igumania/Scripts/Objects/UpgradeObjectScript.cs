using System;
using System.Collections.Generic;
using UnityEngine;

namespace Igumania.Objects
{
    [CreateAssetMenu(fileName = "Upgrade", menuName = "Igumania/Upgrade")]
    public class UpgradeObjectScript : AItemObjectScript, IUpgradeObject
    {
        [SerializeField]
        private UpgradeObjectScript[] requiredUpgrades = Array.Empty<UpgradeObjectScript>();

        public IReadOnlyList<UpgradeObjectScript> RequiredUpgrades => requiredUpgrades ?? Array.Empty<UpgradeObjectScript>();
    }
}
