namespace Steslos.DiamondEcs
{
    /// <summary>
    /// Entity that can have multiple components attached to it.
    /// </summary>
    public sealed class EcsEntity
    {
        public const int MaximumEntities = 5000;

        internal EcsSignature Signature { get; } = new EcsSignature();
    }
}
