using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace PoolingSystem
{
    public class PoolManager : MonoBehaviour
    {
        private static PoolManager _instance;

        public static PoolManager Instance
        {
            get
            {
                if (!_instance)
                    _instance = new GameObject("PoolManager").AddComponent<PoolManager>();
                return _instance;
            }
        }

        [SerializeField] [Tooltip("Predefined pools")] private PredefinedObjectPool[] _predefinedPools;
        private readonly Dictionary<GameObject, ObjectPool> _pools = new Dictionary<GameObject, ObjectPool>();

        public ObjectPool this[GameObject key]
        {
            get { return _pools[key]; }
            protected set { _pools[key] = value; }
        }

        // Awake is called when the script instance is being loaded
        public void Awake()
        {
            if (!_instance)
                _instance = this;
            else if (this != _instance)
            {
                Destroy(this);
                return;
            }

            //Load all the pools defined in the inspector.
            if (_predefinedPools != null)
                foreach (var predefinedPool in _predefinedPools)
                {
                    var pool = new GameObject(predefinedPool.Prefab.name + "Pool").AddComponent<ObjectPool>();
                    pool.transform.parent = transform;
                    pool.FromPredefined(predefinedPool);
                    this[pool.Prefab] = pool;
                }
        }

        // This function is called when the MonoBehaviour will be destroyed
        public void OnDestroy()
        {
            if (_instance == this)
                _instance = null;
        }
    }

    class InstanceIDComparer : IEqualityComparer<Object>
    {
        public bool Equals(Object x, Object y)
        {
            return x.GetInstanceID() == y.GetInstanceID();
        }

        public int GetHashCode(Object obj)
        {
            return obj.GetInstanceID();
        }
    }

    public interface IObjectPool
    {
        GameObject Spawn();
        GameObject Spawn(Transform transform);
        GameObject Spawn(Transform transform, Transform parent);
        GameObject Spawn(Vector3 position, Quaternion rotation);
        GameObject Spawn(Vector3 position, Quaternion rotation, Transform parent);
        void Despawn();
        void Despawn(GameObject element);
    }
}