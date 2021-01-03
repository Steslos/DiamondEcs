using System.Collections.Generic;

namespace Steslos.DiamondEcs
{
    /// <summary>
    /// ECS system that uses holds a signature; entities with a matching signature will automatically be added
    /// to the system for easy querying of relevent entities via the ECS Agent.
    /// </summary>
    public abstract class EcsSystem
    {
        internal EcsAgent EcsAgent { get; set; } = null;
        internal ICollection<EcsEntity> Entities { get; } = new HashSet<EcsEntity>();
        internal EcsSignature Signature { get; } = new EcsSignature();

        /// <summary>
        /// The ECS Agent that registered this system; use this agent to manage entities or components.
        /// </summary>
        protected EcsAgent _ecsAgent => EcsAgent;

        /// <summary>
        /// The entities that (at a minimum) have all the components in this system's signature.
        /// </summary>
        protected ICollection<EcsEntity> _entities => Entities;
    }
}
