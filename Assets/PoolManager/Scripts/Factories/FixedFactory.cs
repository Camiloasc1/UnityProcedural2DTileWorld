using System;
using UnityEngine;
using System.Collections;

namespace PoolingSystem.Factories
{
    [Serializable]
    public class FixedFactory : IFactory
    {
        private FactoryParameters _factoryParameters;

        public void Setup(FactoryParameters factoryParameters)
        {
            _factoryParameters = factoryParameters;
        }

        public void Run()
        {
            var count = 0u;
            foreach (var objectPool in PoolManager.Instance)
            {
                while (NextRatio(objectPool) > objectPool.Ratio)
                {
                    if (objectPool.Instantiate())
                    {
                        if (++count == _factoryParameters.FixedAmmount)
                            return;
                    }
                    else
                        break;
                }
            }
        }

        private static float NextRatio(IObjectPool objectPool)
        {
            return (float) objectPool.ActiveCount/(objectPool.Count + 1);
        }
    }
}