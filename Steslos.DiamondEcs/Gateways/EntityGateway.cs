using System.Collections.Generic;
using System.Diagnostics;

namespace Steslos.DiamondEcs.Gateways
{
    internal sealed class EntityGateway
    {
        private readonly Queue<EcsEntity> _availableEntities = new Queue<EcsEntity>();

        public EntityGateway()
        {
            for (int i = 0; i < EcsEntity.MaximumEntities; ++i)
            {
                _availableEntities.Enqueue(new EcsEntity());
            }
        }

        public EcsEntity CreateEntity()
        {
            Debug.Assert(_availableEntities.Count > 0, "Exhausted available entities.");
            return _availableEntities.Dequeue();
        }

        public void DestroyEntity(EcsEntity entity)
        {
            Debug.Assert(_availableEntities.Count <= EcsEntity.MaximumEntities, "Too many entities in queue, at least one entity destroyed more than once.");
            entity.Signature.ResetSignature();
            _availableEntities.Enqueue(entity);
        }
    }
}
