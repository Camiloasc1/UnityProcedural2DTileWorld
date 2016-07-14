using System;
using UnityEngine;
using System.Collections;

namespace PoolingSystem.Factories
{
    [Serializable]
    public class ProportionFactory : IFactory
    {
        private FactoryParameters _factoryParameters;

        public void Setup(FactoryParameters factoryParameters)
        {
            _factoryParameters = factoryParameters;
        }

        public void Run()
        {
            throw new NotImplementedException();
        }
    }
}