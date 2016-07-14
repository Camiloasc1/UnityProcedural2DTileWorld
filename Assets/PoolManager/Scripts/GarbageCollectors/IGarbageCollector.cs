using System;
using UnityEngine;
using System.Collections;

namespace PoolingSystem.GarbageCollectors
{
    public interface IGarbageCollector : ITaskRunner<GarbageCollectorParameters>
    {
        new void Setup(GarbageCollectorParameters garbageCollectorParameters);
        new void Run();
    }

    [Serializable]
    public struct GarbageCollectorParameters
    {
        [SerializeField] [Range(1, 999)] [Tooltip("How many instances to destroy per round.\nOnly for FixedFactory.")] public uint
            FixedAmmount;

        [SerializeField] [Range(0.1f, 1.0f)] [Tooltip("Proportion of instances to destroy per round.\nOnly for ProportionFactory.")] public
            float Proportion;
    }

    public enum GarbageCollectorProviders
    {
        FixedGarbageCollector,
        ProportionGarbageCollector
    }

    public static class Providers
    {
        private static readonly FixedGarbageCollector FixedGarbageCollector = new FixedGarbageCollector();
        private static readonly ProportionGarbageCollector ProportionGarbageCollector = new ProportionGarbageCollector();

        public static IGarbageCollector GetInstance(this GarbageCollectorProviders factoryProvider)
        {
            switch (factoryProvider)
            {
                case GarbageCollectorProviders.FixedGarbageCollector:
                    return FixedGarbageCollector;
                case GarbageCollectorProviders.ProportionGarbageCollector:
                    return ProportionGarbageCollector;
                default:
                    return null;
            }
        }
    }
}