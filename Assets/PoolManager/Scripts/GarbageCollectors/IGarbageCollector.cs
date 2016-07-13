using UnityEngine;
using System.Collections;

namespace PoolingSystem.GarbageCollectors
{
    public interface IGarbageCollector
    {
        void Setup(GarbageCollectorParameters garbageCollectorParameters);
        void Run();
    }

    public struct GarbageCollectorParameters
    {
        [SerializeField] [Tooltip("How many instances to destroy per round.\nOnly for FixedFactory.")] public uint
            FixedAmmount;

        [SerializeField] [Tooltip("Proportion of instances to destroy per round.\nOnly for ProportionFactory.")] public
            float Proportion;
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
                    return new ProportionGarbageCollector();
                default:
                    return null;
            }
        }
    }
}