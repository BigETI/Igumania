using System;
using UnityEngine;

namespace Igumania.Triggers
{
    public class PlatformSpecificTriggerScript : MonoBehaviour, IPlatformSpecificTrigger
    {
        [SerializeField]
        private bool isUsingPlatformExlusions;

        [SerializeField]
        private RuntimePlatform[] supportedPlatforms = Array.Empty<RuntimePlatform>();

        [SerializeField]
        private RuntimePlatform[] exludedPlatforms = Array.Empty<RuntimePlatform>();

        public bool IsUsingPlatformExlusions
        {
            get => isUsingPlatformExlusions;
            set => isUsingPlatformExlusions = value;
        }

        public RuntimePlatform[] SupportedPlatforms
        {
            get => supportedPlatforms ?? Array.Empty<RuntimePlatform>();
            set => supportedPlatforms = value ?? throw new ArgumentNullException(nameof(value));
        }

        public RuntimePlatform[] ExludedPlatforms
        {
            get => exludedPlatforms ?? Array.Empty<RuntimePlatform>();
            set => exludedPlatforms = value ?? throw new ArgumentNullException(nameof(value));
        }

        private void Awake()
        {
            bool is_excluded = !isUsingPlatformExlusions;
            if (isUsingPlatformExlusions)
            {
                foreach (RuntimePlatform excluded_platform in ExludedPlatforms)
                {
                    if (Application.platform == excluded_platform)
                    {
                        is_excluded = true;
                        break;
                    }
                }
            }
            else
            {
                foreach (RuntimePlatform supported_platform in SupportedPlatforms)
                {
                    if (Application.platform == supported_platform)
                    {
                        is_excluded = false;
                        break;
                    }
                }
            }
            Destroy(is_excluded ? (UnityEngine.Object)gameObject : this);
        }
    }
}
