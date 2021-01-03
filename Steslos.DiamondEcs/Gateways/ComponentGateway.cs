using System.Collections.Generic;
using System.Diagnostics;

namespace Steslos.DiamondEcs.Gateways
{
    internal sealed class ComponentGateway
    {
        private readonly IDictionary<string, IComponentCache> _componentCaches = new Dictionary<string, IComponentCache>();
        private int _nextComponentTypeBit = 0;

        public void AddComponent<T>(EcsEntity entity, T component)
        {
            GetComponentCache<T>().InsertComponent(entity, component);
        }

        public void EntityDestroyed(EcsEntity entity)
        {
            foreach (var componentCachePair in _componentCaches)
            {
                componentCachePair.Value.EntityDestroyed(entity);
            }
        }

        public T GetComponent<T>(EcsEntity entity)
        {
            return GetComponentCache<T>().GetComponent(entity);
        }

        public EcsSignature GetComponentSignature<T>()
        {
            return GetComponentCache<T>().Signature;
        }

        public void RegisterComponent<T>()
        {
            Debug.Assert(_nextComponentTypeBit < 64, "Too many components registered.");
            var componentName = typeof(T).Name;
            Debug.Assert(!_componentCaches.ContainsKey(componentName), "Registering component more than once.");
            var newComponentCache = new ComponentCache<T>();
            newComponentCache.Signature.EnableBit(_nextComponentTypeBit);
            _componentCaches.Add(componentName, newComponentCache);
            _nextComponentTypeBit++;
        }

        public void RemoveComponent<T>(EcsEntity entity)
        {
            GetComponentCache<T>().RemoveComponent(entity);
        }

        private ComponentCache<T> GetComponentCache<T>()
        {
            var componentName = typeof(T).Name;
            Debug.Assert(_componentCaches.ContainsKey(componentName), "Component not registered before use.");
            return (ComponentCache<T>)_componentCaches[componentName];
        }
    }
}
