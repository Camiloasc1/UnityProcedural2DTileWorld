using UnityEngine;
using System.Collections;

namespace PoolingSystem.GarbageCollectors
{
    public interface IGarbageCollector
    {
    }

    public enum GarbageCollectorProviders
    {
        FixedGarbageCollector,
        ProportionGarbageCollector
    }
    public static class Providers
    {
        public static IGarbageCollector GetInstance(this GarbageCollectorProviders factoryProvider)
        {
            switch (factoryProvider)
            {
                case GarbageCollectorProviders.FixedGarbageCollector:
                    return new FixedGarbageCollector();
                case GarbageCollectorProviders.ProportionGarbageCollector:
                    //TODO
                    return null;
                default:
                    return null;
            }
        }
    }
}