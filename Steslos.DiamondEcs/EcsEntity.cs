namespace Steslos.DiamondEcs
{
    /// <summary>
    /// Entity that can have multiple components attached to it.
    /// </summary>
    public sealed class EcsEntity
    {
        /// <summary>
        /// The maximum amount of entities that can exist at once.
        /// One entity is reserved for attaching singleton components to; users should subtract one
        /// from this value for the usable amount of entities available.
        /// </summary>
        public const int MaximumEntities = 5000;

        internal EcsSignature Signature { get; } = new EcsSignature();

        internal EcsEntity()
        {
        }
    }
}
