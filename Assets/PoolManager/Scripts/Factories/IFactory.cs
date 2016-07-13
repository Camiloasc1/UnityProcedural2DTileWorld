using UnityEngine;
using System.Collections;

namespace PoolingSystem.Factories
{
    public interface IFactory
    {
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
                    //TODO
                    return null;
                default:
                    return null;
            }
        }
    }
}