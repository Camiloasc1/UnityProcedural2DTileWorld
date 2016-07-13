using System;
using UnityEngine;
using System.Collections;

namespace PoolingSystem.Factories
{
    public interface IFactory
    {
        void Setup(FactoryParameters factoryParameters);
        void Run();
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
        public static IFactory GetInstance(this FactoryProviders factoryProvider)
        {
            switch (factoryProvider)
            {
                case FactoryProviders.FixedFactory:
                    return new FixedFactory();
                case FactoryProviders.ProportionFactory:
                    return new ProportionFactory();
                default:
                    return null;
            }
        }
    }
}