using Igumania.Objects;
using UnityEngine;
using UnityEngine.Events;

namespace Igumania.Controllers
{
    public class UpgradeInstallationWatcherControllerScript : MonoBehaviour, IUpgradeInstallationWatcherController
    {
        [SerializeField]
        private UpgradeObjectScript upgrade = default;

        [SerializeField]
        private UnityEvent onUpgradeInstalled = default;

        public UpgradeObjectScript Upgrade
        {
            get => upgrade;
            set => upgrade = value;
        }

        public IProfile Profile { get; private set; }

        public event UpgradeInstalledDelegate OnUpgradeInstalled;

        private void UpgradeInstalledEvent(UpgradeObjectScript upgrade)
        {
            if (this.upgrade && (upgrade == this.upgrade))
            {
                if (onUpgradeInstalled != null)
                {
                    onUpgradeInstalled.Invoke();
                }
                OnUpgradeInstalled?.Invoke(upgrade);
            }
        }

        private void OnEnable()
        {
            Profile = GameManager.SelectedProfile;
            if (Profile != null)
            {
                Profile.OnUpgradeInstalled += UpgradeInstalledEvent;
            }
        }

        private void OnDisable()
        {
            if (Profile != null)
            {
                Profile.OnUpgradeInstalled -= UpgradeInstalledEvent;
                Profile = null;
            }
        }
    }
}
