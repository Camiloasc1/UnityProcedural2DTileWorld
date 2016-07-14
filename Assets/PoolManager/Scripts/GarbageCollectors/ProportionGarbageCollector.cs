using System;
using UnityEngine;
using System.Collections;

namespace PoolingSystem.GarbageCollectors
{
    [Serializable]
    public class ProportionGarbageCollector : IGarbageCollector
    {
        private GarbageCollectorParameters _garbageCollectorParameters;

        public void Setup(GarbageCollectorParameters garbageCollectorParameters)
        {
            _garbageCollectorParameters = garbageCollectorParameters;
        }

        public void Run()
        {
            throw new NotImplementedException();
        }
    }
}