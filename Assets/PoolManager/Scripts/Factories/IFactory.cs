using System;
using UnityEngine;
using System.Collections;

namespace PoolingSystem.Factories
{
    public interface IFactory: ITaskRunner<FactoryParameters>
    {
        new void Setup(FactoryParameters factoryParameters);
        new void Run();
    }

    [Serializable]
    public struct FactoryParameters
    {
        [SerializeField] [Tooltip("How many instances to create per round.\nOnly for FixedFactory.")] public uint
            FixedAmmount;

        [SerializeField] [Tooltip("Proportion of instances to create per round.\nOnly for ProportionFactory.")] public
            float Proportion;
    }

    public enum FactoryProviders
    {
        FixedFactory,
        ProportionFactory
    }

    public static class Providers
    {
        private static readonly FixedFactory FixedFactory = new FixedFactory();
        private static readonly ProportionFactory ProportionFactory = new ProportionFactory();

        public static IFactory GetInstance(this FactoryProviders factoryProvider)
        {
            switch (factoryProvider)
            {
                case FactoryProviders.FixedFactory:
                    return FixedFactory;
                case FactoryProviders.ProportionFactory:
                    return ProportionFactory;
                default:
                    return null;
            }
        }
    }
}