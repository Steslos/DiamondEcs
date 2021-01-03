namespace Steslos.DiamondEcs.Gateways
{
    internal interface IComponentCache
    {
        void EntityDestroyed(EcsEntity entity);
    }
}
