using UnityEngine;
using System.Collections;


namespace PoolingSystem
{
    public interface IPoolElement
    {
        /// <summary>
        /// OnSpawn is called when the PoolElement is being spawned.
        /// </summary>
        void OnSpawn();

        /// <summary>
        /// OnSpawn is called when the PoolElement is being despawned.
        /// </summary>
        void OnDespawn();
    }
}