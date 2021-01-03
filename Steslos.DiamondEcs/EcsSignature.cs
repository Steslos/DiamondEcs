namespace Steslos.DiamondEcs
{
    /// <summary>
    /// Signature that represents components being present or not.
    /// </summary>
    public sealed class EcsSignature
    {
        private long _bitSignature = 0;

        /// <summary>
        /// Adds the component(s) from the given signature to this one.
        /// </summary>
        /// <param name="signature">The signature to add components from.</param>
        public void AddSignature(EcsSignature signature)
            => _bitSignature |= signature._bitSignature;

        /// <summary>
        /// Removes the component(s) from the given signature from this one.
        /// </summary>
        /// <param name="signature">The signature to remove components from.</param>
        public void RemoveSignature(EcsSignature signature)
            => _bitSignature &= ~signature._bitSignature;

        internal void DisableBit(int bitPosition)
            => _bitSignature &= ~(long)1 << bitPosition;

        internal void EnableBit(int bitPosition)
            => _bitSignature |= (long)1 << bitPosition;

        internal bool MatchesSignature(EcsSignature signature)
            => (_bitSignature & signature._bitSignature) == signature._bitSignature;

        internal void ResetSignature()
            => _bitSignature = 0;

        internal void SetSignature(EcsSignature signature)
            => _bitSignature = signature._bitSignature;
    }
}
