using Steslos.DiamondEcs.Gateways;

namespace Steslos.DiamondEcs
{
    /// <summary>
    /// The agent class that represents a "world" that contains entities, components, and systems.
    /// Management of these are done through the agent, which is populated into systems that are registered.
    /// </summary>
    public sealed class EcsAgent
    {
        private readonly ComponentGateway _componentGateway = new ComponentGateway();
        private readonly EntityGateway _entityGateway = new EntityGateway();
        private readonly EcsEntity _singletonEntity = null;
        private readonly SystemGateway _systemGateway = new SystemGateway();

        /// <summary>
        /// Create a new ECS agent class.
        /// </summary>
        public EcsAgent()
        {
            _singletonEntity = _entityGateway.CreateEntity();
        }

        /// <summary>
        /// Adds a singleton component to the ECS agent.
        /// </summary>
        /// <typeparam name="T">The type of the component.</typeparam>
        /// <param name="singletonComponent">The instance of the component.</param>
        public void AddComponent<T>(T singletonComponent)
        {
            AddComponent<T>(_singletonEntity, singletonComponent);
        }

        /// <summary>
        /// Adds a component to a given entity.
        /// </summary>
        /// <typeparam name="T">The type of the component.</typeparam>
        /// <param name="entity">The entity to attach the component to.</param>
        /// <param name="component">The instance of the component.</param>
        public void AddComponent<T>(EcsEntity entity, T component)
        {
            _componentGateway.AddComponent(entity, component);
            var componentSignature = _componentGateway.GetComponentSignature<T>();
            entity.Signature.AddSignature(componentSignature);
            _systemGateway.EntitySignatureChanged(entity);
        }

        /// <summary>
        /// Gets an entity, up to the maximum amount of active entities.
        /// When done with an entity, pass it to DestroyEntity().
        /// </summary>
        /// <returns>The empty (no components) entity.</returns>
        public EcsEntity CreateEntity()
        {
            return _entityGateway.CreateEntity();
        }

        /// <summary>
        /// Frees an entity back to the agent's pool of available entities.
        /// Destroying an entity will also destroy any components attached to it.
        /// </summary>
        /// <param name="entity">The entity to destroy/free.</param>
        public void DestroyEntity(EcsEntity entity)
        {
            _componentGateway.EntityDestroyed(entity);
            _systemGateway.EntityDestroyed(entity);
            _entityGateway.DestroyEntity(entity);
        }

        /// <summary>
        /// Gets a singleton component.
        /// </summary>
        /// <typeparam name="T">The type of the singleton component.</typeparam>
        /// <returns>The instance of the singleton component.</returns>
        public T GetComponent<T>()
        {
            return GetComponent<T>(_singletonEntity);
        }

        /// <summary>
        /// Gets a component attached to the given entity.
        /// </summary>
        /// <typeparam name="T">The type of the component.</typeparam>
        /// <param name="entity">The entity that has the component added to it.</param>
        /// <returns>The instance of the component attached to the given entity.</returns>
        public T GetComponent<T>(EcsEntity entity)
        {
            return _componentGateway.GetComponent<T>(entity);
        }

        /// <summary>
        /// Gets the signature of a given component type.
        /// This is useful when creating a EcsSignature to set a system's signature with SetSystemSignature().
        /// </summary>
        /// <typeparam name="T">The type of the component.</typeparam>
        /// <returns>The signature of the given component.</returns>
        public EcsSignature GetComponentSignature<T>()
        {
            return _componentGateway.GetComponentSignature<T>();
        }

        /// <summary>
        /// Registers a given component type, making the agent aware of that component and allowing future calls to
        /// AddComponent(), GetComponent(), and GetComponentSignature().
        /// </summary>
        /// <typeparam name="T">The type of the component to have the agent aware of.</typeparam>
        public void RegisterComponent<T>()
        {
            _componentGateway.RegisterComponent<T>();
        }

        /// <summary>
        /// Registers a given system into the agent, then returns the system instance populated with this agent as a
        /// protected property.
        /// Systems registered have a blank signature, and must be set with SetSystemSignature() for the system's
        /// protected entities property to populate.
        /// </summary>
        /// <typeparam name="T">The type of the system.</typeparam>
        /// <returns>The system instance, populated with this agent as a protected property.</returns>
        public T RegisterSystem<T>()
            where T : EcsSystem, new()
        {
            return _systemGateway.RegisterSystem<T>(this);
        }

        /// <summary>
        /// Removes a singleton component.
        /// </summary>
        /// <typeparam name="T">The type of the singleton component to remove.</typeparam>
        public void RemoveComponent<T>()
        {
            RemoveComponent<T>(_singletonEntity);
        }

        /// <summary>
        /// Removes a component attached to the given entity.
        /// </summary>
        /// <typeparam name="T">The type of component to remove.</typeparam>
        /// <param name="entity">The entity with the given component attached.</param>
        public void RemoveComponent<T>(EcsEntity entity)
        {
            _componentGateway.RemoveComponent<T>(entity);
            var componentSignature = _componentGateway.GetComponentSignature<T>();
            entity.Signature.RemoveSignature(componentSignature);
            _systemGateway.EntitySignatureChanged(entity);
        }

        /// <summary>
        /// Sets a previously registered EcsSystem signature.
        /// Once set, when components are added to entities and they (at a minimum) match the system's signature,
        /// they will automatically be populated into the system's entities property.
        /// </summary>
        /// <typeparam name="T">The type of the system.</typeparam>
        /// <param name="signature">
        ///     The signature to set the system to; this does not add to, but instead overwrites, the signature.
        /// </param>
        public void SetSystemSignature<T>(EcsSignature signature)
            where T : EcsSystem
        {
            _systemGateway.SetSystemSignature<T>(signature);
        }
    }
}
