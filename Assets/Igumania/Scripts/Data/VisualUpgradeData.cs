using Igumania.Objects;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Igumania.Data
{
    [Serializable]
    public struct VisualUpgradeData : IVisualUpgradeData
    {
        [SerializeField]
        private UpgradeObjectScript upgrade;

        [SerializeField]
        private UnityEvent onUpgradeInstalled;

        [SerializeField]
        private UnityEvent onUpgradeUninstalled;

        public UpgradeObjectScript Upgrade => upgrade;

        public event UpgradeInstalledDelegate OnUpgradeInstalled;

        public event UpgradeUninstalledDelegate OnUpgradeUninstalled;

        public void InvokeUpgradeInstalled()
        {
            if (onUpgradeInstalled != null)
            {
                onUpgradeInstalled.Invoke();
            }
            OnUpgradeInstalled?.Invoke(upgrade);
        }

        public void InvokeUpgradeUninstalled()
        {
            if (onUpgradeUninstalled != null)
            {
                onUpgradeUninstalled.Invoke();
            }
            OnUpgradeUninstalled?.Invoke(upgrade);
        }
    }
}
