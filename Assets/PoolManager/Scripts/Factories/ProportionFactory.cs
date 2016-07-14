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
            foreach (var objectPool in PoolManager.Instance)
            {
                var ammount = CalcAmmount(objectPool)*_factoryParameters.Proportion;
                for (var i = 0; i < ammount; i++)
                {
                    if (!objectPool.Instantiate())
                        break;
                }
            }
        }

        private static int CalcAmmount(IObjectPool objectPool)
        {
            return Mathf.FloorToInt(objectPool.ActiveCount/objectPool.Ratio) - objectPool.Count;
        }
    }
}