using System;
using UnityEngine;
using System.Collections;

namespace PoolingSystem.GarbageCollectors
{
    [Serializable]
    public class FixedGarbageCollector : IGarbageCollector
    {
        private GarbageCollectorParameters _garbageCollectorParameters;

        public void Setup(GarbageCollectorParameters garbageCollectorParameters)
        {
            _garbageCollectorParameters = garbageCollectorParameters;
        }

        public void Run()
        {
            var count = 0u;
            foreach (var objectPool in PoolManager.Instance)
            {
                while (PreviousRatio(objectPool) < objectPool.Ratio)
                {
                    if (objectPool.Destroy())
                    {
                        if (++count == _garbageCollectorParameters.FixedAmmount)
                            return;
                    }
                    else
                        break;
                }
            }
        }

        private static float PreviousRatio(IObjectPool objectPool)
        {
            return (float) objectPool.ActiveCount/ (objectPool.Count - 1);
        }
    }
}