using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using PoolingSystem.Factories;
using PoolingSystem.GarbageCollectors;

namespace PoolingSystem
{
    public class PoolManager : MonoBehaviour, IEnumerable<IObjectPool>
    {
        private static PoolManager _instance;

        /// <summary>
        /// The singleton instance of pool manager in this scene.
        /// </summary>
        public static PoolManager Instance
        {
            get
            {
                if (!_instance)
                {
                    _instance = FindObjectOfType<PoolManager>();
                    _instance.Setup();
                }
                if (!_instance)
                {
                    _instance = new GameObject().AddComponent<PoolManager>();
                    _instance.Setup();
                }
                return _instance;
            }
        }

        /// <summary>
        /// The singleton instance of pool manager in this scene.
        /// </summary>
        public static PoolManager Pools
        {
            get { return Instance; }
        }

        [SerializeField] [Tooltip("Predefined pools.")] private PredefinedObjectPool[] _predefinedPools;
        private bool _initialized;
        private readonly Dictionary<GameObject, IObjectPool> _pools = new Dictionary<GameObject, IObjectPool>();
        [SerializeField] private PoolingSetting poolingSetting;
        private IEnumerator _worker;

        /// <summary>
        /// Access to an specified pool.
        /// </summary>
        /// <param name="prefab">The prefab that defines the pool.</param>
        /// <returns>The pool asociated with the prefab.</returns>
        public IObjectPool this[GameObject prefab]
        {
            get { return _pools[prefab]; }
            set { _pools[prefab] = value; }
        }

        /// <summary>
        /// Load all the pools defined in the inspector.
        /// </summary>
        private void LoadPredefined()
        {
            foreach (var poolManager in FindObjectsOfType<PoolManager>())
            {
                if (poolManager._predefinedPools == null)
                    continue;
                foreach (var predefinedPool in poolManager._predefinedPools)
                {
                    var pool = ObjectPool.FromPredefined(predefinedPool);
                    pool.transform.parent = transform;
                    this[pool.Prefab] = pool;
                }
                poolManager._predefinedPools = null;
            }
        }

        /// <summary>
        /// Initialize, set name and tag.
        /// </summary>
        private void Setup()
        {
            if (_initialized)
                return;
            name = "Pool Manager";
            tag = "Pool Manager";
            LoadPredefined();
            _initialized = true;
        }

        // Awake is called when the script instance is being loaded
        public void Awake()
        {
            if (!_instance)
            {
                _instance = this;
                Setup();
            }
        }

        // Start is called just before any of the Update methods is called the first time
        public void Start()
        {
            if (this != _instance)
                Destroy(this);

            _worker = Worker();
            StartCoroutine(_worker);
        }

        // This function is called when the MonoBehaviour will be destroyed
        public void OnDestroy()
        {
            if (_instance != this)
                return;

            _instance = null;
            StopCoroutine(_worker);
        }

        private IEnumerator Worker()
        {
            poolingSetting.Factory.GetInstance().Setup(poolingSetting.FactoryParameters);
            poolingSetting.GarbageCollector.GetInstance().Setup(poolingSetting.GarbageCollectorParameters);
            while (true)
            {
                poolingSetting.Factory.GetInstance().Run();
                poolingSetting.GarbageCollector.GetInstance().Run();
                yield return null;
            }
        }

        public IEnumerator<IObjectPool> GetEnumerator()
        {
            return  _pools.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    [Serializable]
    public struct PoolingSetting
    {
        [SerializeField] [Tooltip("Wich Factory to use.")] public FactoryProviders Factory;
        [SerializeField] [Tooltip("Wich Garbage Collector to use.")] public GarbageCollectorProviders GarbageCollector;
        [SerializeField] [Tooltip("Parameters for the Factory.")] public FactoryParameters FactoryParameters;

        [SerializeField] [Tooltip("Parameters for the Garbage Collector.")] public GarbageCollectorParameters
            GarbageCollectorParameters;
    }

    public interface ITaskRunner<T>
    {
        void Setup(T parameters);
        void Run();
    }

    [Serializable]
    public struct PredefinedObjectPool
    {
        [SerializeField] [Tooltip("The base prefab of this pool.")] public GameObject Prefab;
        [SerializeField] [Tooltip("The minimum number of objects to keep.")] public uint Min;
        [SerializeField] [Tooltip("The maximum number of objects to keep.\n(0 = No limit.).")] public uint Max;
        [SerializeField] [Range(0.1f, 1.0f)] [Tooltip("The target usage ratio.")] public float UsageRatio;
    }

    public interface IObjectPool
    {
        /// <summary>
        /// The prefab that defines the pool.
        /// </summary>
        GameObject Prefab { get; }

        /// <summary>
        /// The ammount of elements currently allocated.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// The ammount of elements currently active.
        /// </summary>
        int ActiveCount { get; }

        /// <summary>
        /// The ammount of elements currently inactive.
        /// </summary>
        int InactiveCount { get; }

        /// <summary>
        /// Retrieves or instantiates a new one if the pool is empty and the limit is not exceeded.
        /// Internally called by the other Spawn() methods.
        /// </summary>
        /// <returns>A prefab instance ready to use.</returns>
        GameObject Spawn();

        /// <summary>
        /// Retrieves or instantiates a new one if the pool is empty and the limit is not exceeded.
        /// </summary>
        /// <param name="transform">The transform with position and rotation to set.</param>
        /// <returns>A prefab instance ready to use</returns>
        GameObject Spawn(Transform transform);

        /// <summary>
        /// Retrieves or instantiates a new one if the pool is empty and the limit is not exceeded.
        /// </summary>
        /// <param name="transform">The transform with position and rotation to set.</param>
        /// <param name="parent">The parent transform in the scene to set.</param>
        /// <returns>A prefab instance ready to use</returns>
        GameObject Spawn(Transform transform, Transform parent);

        /// <summary>
        /// Retrieves or instantiates a new one if the pool is empty and the limit is not exceeded.
        /// </summary>
        /// <param name="position">The position to set.</param>
        /// <param name="rotation">The rotation to set.</param>
        /// <returns>A prefab instance ready to use</returns>
        GameObject Spawn(Vector3 position, Quaternion rotation);

        /// <summary>
        /// Retrieves or instantiates a new one if the pool is empty and the limit is not exceeded.
        /// </summary>
        /// <param name="position">The position to set.</param>
        /// <param name="rotation">The rotation to set.</param>
        /// <param name="parent">The parent transform in the scene to set.</param>
        /// <returns>A prefab instance ready to use</returns>
        GameObject Spawn(Vector3 position, Quaternion rotation, Transform parent);

        /// <summary>
        /// Forcedly despawn an active instance.
        /// </summary>
        void Despawn();

        /// <summary>
        /// Despawn the selected instance.
        /// </summary>
        /// <param name="instance">The instance to despawn</param>
        void Despawn(GameObject instance);

        /// <summary>
        /// Forcedly create a new instance.
        /// </summary>
        void Instantiate();

        /// <summary>
        /// Forcedly destroy an inactive instance.
        /// </summary>
        void Destroy();
    }
}