using Igumania.Objects;
using UnityEngine;
using UnityEngine.Events;

namespace Igumania.Controllers
{
    public class BudgetWatcherControllerScript : MonoBehaviour, IBudgetWatcherController
    {
        [SerializeField]
        private UpgradeObjectScript upgrade = default;

        [SerializeField]
        private UnityEvent onBudgetReached = default;

        private bool isNotNotified = true;

        public UpgradeObjectScript Upgrade
        {
            get => upgrade;
            set => upgrade = value;
        }

        public event BudgetReachedDelegate OnBudgetReached;

        private void Update()
        {
            if (isNotNotified && upgrade)
            {
                IProfile profile = GameManager.SelectedProfile;
                if ((profile != null) && ((long)upgrade.Cost <= profile.Money))
                {
                    isNotNotified = false;
                    if (onBudgetReached != null)
                    {
                        onBudgetReached.Invoke();
                    }
                    OnBudgetReached?.Invoke();
                }
            }
        }
    }
}
