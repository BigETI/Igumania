namespace Igumania
{
    /// <summary>
    /// An interface that represents a singleton manager
    /// </summary>
    public interface ISingletonManager : IManager
    {
        bool IsNotBeingDestroyedOnLoad { get; set; }
    }
}
