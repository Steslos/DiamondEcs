using System.Collections.Generic;
using System.Diagnostics;

namespace Steslos.DiamondEcs.Gateways
{
    internal sealed class SystemGateway
    {
        private readonly IDictionary<string, EcsSystem> _systems = new Dictionary<string, EcsSystem>();

        public void EntityDestroyed(EcsEntity entity)
        {
            foreach (var systemPair in _systems)
            {
                systemPair.Value.Entities.Remove(entity);
            }
        }

        public void EntitySignatureChanged(EcsEntity entity)
        {
            foreach (var systemPair in _systems)
            {
                if (entity.Signature.MatchesSignature(systemPair.Value.Signature))
                {
                    systemPair.Value.Entities.Add(entity);
                }
                else
                {
                    systemPair.Value.Entities.Remove(entity);
                }
            }
        }

        public T RegisterSystem<T>(EcsAgent agent)
            where T : EcsSystem, new()
        {
            var system = new T();
            var systemName = typeof(T).Name;
            Debug.Assert(!_systems.ContainsKey(systemName), "System already registered.");
            system.EcsAgent = agent;
            _systems.Add(systemName, system);
            return system;
        }

        public void SetSystemSignature<T>(EcsSignature signature)
            where T : EcsSystem
        {
            var systemName = typeof(T).Name;
            Debug.Assert(_systems.ContainsKey(systemName), "System not registered before use.");
            _systems[systemName].Signature.SetSignature(signature);
        }
    }
}
