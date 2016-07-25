using UnityEngine;
using System.Collections;

namespace PoolingSystem
{
    public class PoolElement : MonoBehaviour
    {
        /// <summary>
        /// The pool which this PoolElement belongs.
        /// </summary>
        public ObjectPool Pool;

        /// <summary>
        /// The base prefab of this PoolElement.
        /// </summary>
        public GameObject Prefab
        {
            get { return Pool.Prefab; }
        }

        /// <summary>
        /// Despawn this PoolElement.
        /// </summary>
        public void Despawn()
        {
            Pool.Despawn(gameObject);
        }
    }
}