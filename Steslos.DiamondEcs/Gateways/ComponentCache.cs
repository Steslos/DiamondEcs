using System.Collections.Generic;
using System.Diagnostics;

namespace Steslos.DiamondEcs.Gateways
{
    internal sealed class ComponentCache<T> : IComponentCache
    {
        public EcsSignature Signature { get; } = new EcsSignature();

        private readonly IDictionary<EcsEntity, T> _entityComponents = new Dictionary<EcsEntity, T>();

        public void EntityDestroyed(EcsEntity entity)
        {
            if (_entityComponents.ContainsKey(entity))
            {
                RemoveComponent(entity);
            }
        }

        public T GetComponent(EcsEntity entity)
        {
            Debug.Assert(_entityComponents.ContainsKey(entity), "Entity does not have the component requested.");
            return _entityComponents[entity];
        }

        public void InsertComponent(EcsEntity entity, T component)
        {
            Debug.Assert(!_entityComponents.ContainsKey(entity), "Entity already contains the given component.");
            _entityComponents.Add(entity, component);
        }

        public void RemoveComponent(EcsEntity entity)
        {
            Debug.Assert(_entityComponents.ContainsKey(entity), "Entity does not contain the component requested to remove.");
            _entityComponents.Remove(entity);
        }
    }
}
