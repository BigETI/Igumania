using UnityEngine;

namespace Igumania
{
    public interface IPlatformSpecificTrigger : IBehaviour
    {
        bool IsUsingPlatformExlusions { get; set; }

        RuntimePlatform[] SupportedPlatforms { get; set; }

        RuntimePlatform[] ExludedPlatforms { get; set; }
    }
}
