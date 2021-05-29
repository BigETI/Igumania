using Igumania.Data;
using Igumania.Objects;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Igumania.Controllers
{
    public class EnvironmentVisualUpgradesControllerScript : MonoBehaviour
    {
        [SerializeField]
        private VisualUpgradeData[] visualUpgrades = Array.Empty<VisualUpgradeData>();

        private Dictionary<string, IVisualUpgradeData> visualUpgradeLookup = new Dictionary<string, IVisualUpgradeData>();

        private bool isNotInitialized = true;
        
        public IEnumerable<IVisualUpgradeData> VisualUpgrades => visualUpgradeLookup.Values;

        public IProfile Profile { get; private set; }

        private void UpgradeInstalledEvent(UpgradeObjectScript upgrade)
        {
            string key = upgrade.name;
            if (visualUpgradeLookup.ContainsKey(key))
            {
                visualUpgradeLookup[key].InvokeUpgradeInstalled();
            }
        }

        private void UpgradeUninstalledEvent(UpgradeObjectScript upgrade)
        {
            string key = upgrade.name;
            if (visualUpgradeLookup.ContainsKey(key))
            {
                visualUpgradeLookup[key].InvokeUpgradeUninstalled();
            }
        }

        private void Start()
        {
            Profile = GameManager.SelectedProfile;
            visualUpgradeLookup.Clear();
            if (visualUpgrades != null)
            {
                foreach (VisualUpgradeData visual_upgrade in visualUpgrades)
                {
                    if (visual_upgrade.Upgrade)
                    {
                        string key = visual_upgrade.Upgrade.name;
                        if (visualUpgradeLookup.ContainsKey(key))
                        {
                            Debug.LogError($"Found duplicate visual upgrade entry \"{ key }\" in visual upgrades.", this);
                        }
                        else
                        {
                            visualUpgradeLookup.Add(key, visual_upgrade);
                        }
                    }
                    else
                    {
                        Debug.LogError("Please define a visual upgrade or remove it from the array, if not needed at all.", this);
                    }
                }
            }
        }

        private void OnDestroy()
        {
            if (!isNotInitialized)
            {
                if (Profile != null)
                {
                    Profile.OnUpgradeInstalled -= UpgradeInstalledEvent;
                    Profile.OnUpgradeUninstalled -= UpgradeUninstalledEvent;
                    foreach (UpgradeObjectScript upgrade in Profile.Upgrades)
                    {
                        string key = upgrade.name;
                        if (visualUpgradeLookup.ContainsKey(key))
                        {
                            visualUpgradeLookup[key].InvokeUpgradeUninstalled();
                        }
                    }
                }
                isNotInitialized = true;
            }
            visualUpgradeLookup.Clear();
        }

        private void Update()
        {
            if (isNotInitialized)
            {
                if (Profile != null)
                {
                    foreach (UpgradeObjectScript upgrade in Profile.Upgrades)
                    {
                        string key = upgrade.name;
                        if (visualUpgradeLookup.ContainsKey(key))
                        {
                            visualUpgradeLookup[key].InvokeUpgradeInstalled();
                        }
                    }
                    Profile.OnUpgradeInstalled += UpgradeInstalledEvent;
                    Profile.OnUpgradeUninstalled += UpgradeUninstalledEvent;
                }
                isNotInitialized = false;
            }
        }
    }
}
