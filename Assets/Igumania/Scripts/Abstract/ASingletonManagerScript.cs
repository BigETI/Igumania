using UnityEngine;

namespace Igumania.Managers
{
    /// <summary>
    /// An abstract class that describes a singleton manager script
    /// </summary>
    /// <typeparam name="T">Manager script type</typeparam>
    public abstract class ASingletonManagerScript<T> : AManagerScript<T>, ISingletonManager where T : AManagerScript<T>
    {
        [SerializeField]
        private bool isNotBeingDestroyedOnLoad;

        public bool IsNotBeingDestroyedOnLoad
        {
            get => isNotBeingDestroyedOnLoad;
            set => isNotBeingDestroyedOnLoad = value;
        }

        /// <summary>
        /// Gets invoked when script gets initialized
        /// </summary>
        protected virtual void Awake()
        {
            if ((Instance != null) && (Instance != this))
            {
                Destroy(gameObject);
            }
            else if (isNotBeingDestroyedOnLoad)
            {
                DontDestroyOnLoad(gameObject);
            }
        }
    }
}
