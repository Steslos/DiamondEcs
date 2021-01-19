# DiamondEcs
DiamondECS is a entity-component-system (ECS) library in C#, supporting applications that utiltize an ECS architecture.

# What is ECS?
Briefly, ECS is a architectural-pattern involving three main parts:
- Entity: An empty class representing a "thing" in the application (a scene, an animation, a boat, etc)
- Component: A class that only contains state/data (numbers, strings, references, etc)
- System: A class that only contains logic (ie, methods)

These parts are managed together so that components are attached to entities, and entities are populated into systems that care about those components. A system that works with `BoatComponent` will be populated only with entities that have a `BoatComponent` attached.

# Setting up DiamondECS
(coming soon)

# Developing with DiamondECS
- Instanciate an instance of `EcsAgent`.
- Register all components your app will use with `EcsAgent.RegisterComponent<T>()`.
- Register all systems your app will use with `EcsAgent.RegisterSystem<T>()`. `RegisterSystem<T>()` returns an instance of your system. Your systems must inherit from `EcsSystem`.
- Set the signatures of your systems by creating an instance of `EcsSignature`, adding the components to the signature with `EcsSignature.AddSignature(EcsAgent.GetComponentSignature<T>())`, then setting the system signature with `EcsAgent.SetSystemSignature<T>(EcsSignature)`.
- Run your app, calling methods in your systems. `EcsSystem`s automatically have access to the `EcsAgent` via the protected field `_ecsAgent`. Entities that contain the components that were set for a system with `EcsAgent.SetSystemSignature<T>(EcsSignature)` are automatically populated into that system's protected field `_entities`.
- Create entities with `EcsAgent.CreateEntity()`. Attach components to entities with `EcsAgent.AddComponent<T>(EcsEntity)`, and add singleton components with `EcsAgent.AddComponent<T>()`.
- Work with the entities for that system by iterating over the protected `_entities` collection from your system, and retrieve the components via the protected `_ecsAgent` field with `EcsAgent.GetComponent<T>(EcsEntity)`.
