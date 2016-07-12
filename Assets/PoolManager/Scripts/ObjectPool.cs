﻿using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace PoolingSystem
{
    public class ObjectPool : MonoBehaviour, IObjectPool
    {
        [SerializeField] [Tooltip("The base prefab of this pool")] private GameObject _prefab;
        [SerializeField] [Tooltip("The minimum number of objects to keep")] private uint _min;
        [SerializeField] [Tooltip("The maximum number of objects to keep")] private uint _max;
        [SerializeField] [Range(0.0f, 1.0f)] [Tooltip("The target usage ratio")] private float _usageRatio = 1.0f;
        private readonly HashSet<GameObject> _active = new HashSet<GameObject>();
        private readonly Stack<GameObject> _inactive = new Stack<GameObject>();

        public GameObject Prefab
        {
            get { return _prefab; }
        }

        public int Count
        {
            get { return _active.Count + _inactive.Count; }
        }

        public int ActiveCount
        {
            get { return _active.Count; }
        }

        public int InactiveCount
        {
            get { return _inactive.Count; }
        }

        public bool IsPredefined { get; private set; }

        public uint Min
        {
            get { return _min; }
            set { _min = value; }
        }

        public uint Max
        {
            get { return _max; }
            set { _max = value; }
        }

        public float UsageRatio
        {
            get { return _usageRatio; }
            set { _usageRatio = value; }
        }

        public static ObjectPool FromPredefined(PredefinedObjectPool predefinedPool)
        {
            var objectPool = new GameObject(predefinedPool.Prefab.name + " Pool").AddComponent<ObjectPool>();
            objectPool.IsPredefined = true;
            objectPool._prefab = predefinedPool.Prefab;
            objectPool._min = predefinedPool.Min;
            objectPool._max = predefinedPool.Max;
            objectPool._usageRatio = predefinedPool.UsageRatio;
            objectPool._active.Clear();
            objectPool._inactive.Clear();
            return objectPool;
        }

        public static ObjectPool FromPrefab(GameObject prefab)
        {
            var objectPool = new GameObject(prefab.name + " Pool").AddComponent<ObjectPool>();
            objectPool.IsPredefined = false;
            objectPool._prefab = prefab;
            objectPool._min = 0;
            objectPool._max = 0;
            objectPool._usageRatio = 1.0f;
            objectPool._active.Clear();
            objectPool._inactive.Clear();
            return objectPool;
        }

        public GameObject Spawn()
        {
            GameObject instance;
            if (_inactive.Count > 0)
                instance = _inactive.Pop();
            else if (_max == 0 || Count < _max)
                instance = Instantiate(_prefab);
            else
                return null;
            _active.Add(instance);
            instance.transform.parent = null;
            instance.SendMessage("OnSpawn", null, SendMessageOptions.DontRequireReceiver);
            instance.SetActive(true);
            return instance;
        }

        public GameObject Spawn(Transform transform)
        {
            return Spawn(transform, null);
        }

        public GameObject Spawn(Transform transform, Transform parent)
        {
            return Spawn(transform.position, transform.rotation, parent);
        }

        public GameObject Spawn(Vector3 position, Quaternion rotation)
        {
            return Spawn(position, rotation, null);
        }

        public GameObject Spawn(Vector3 position, Quaternion rotation, Transform parent)
        {
            var instance = Spawn();
            if (!instance)
                return instance;
            instance.transform.position = position;
            instance.transform.rotation = rotation;
            instance.transform.parent = parent;
            return instance;
        }

        public void Despawn()
        {
            foreach (var instance in _active)
            {
                Despawn(instance);
                break;
            }
        }

        public void Despawn(GameObject instance)
        {
            if (!_active.Contains(instance))
                return;

            _active.Remove(instance);
            _inactive.Push(instance);
            instance.SetActive(false);
            instance.SendMessage("OnDespawn", null, SendMessageOptions.DontRequireReceiver);
            instance.transform.parent = transform;
        }

        // Awake is called when the script instance is being loaded
        public void Awake()
        {
        }
    }

    [Serializable]
    public struct PredefinedObjectPool
    {
        [SerializeField] [Tooltip("The base prefab of this pool")] public GameObject Prefab;
        [SerializeField] [Tooltip("The minimum number of objects to keep")] public uint Min;
        [SerializeField] [Tooltip("The maximum number of objects to keep")] public uint Max;
        [SerializeField] [Range(0.0f, 1.0f)] [Tooltip("The target usage ratio")] public float UsageRatio;
    }

    public interface IPooleableGameObject
    {
        /// <summary>
        /// OnSpawn is called when the GameObject is being spawned.
        /// </summary>
        void OnSpawn();

        /// <summary>
        /// OnSpawn is called when the GameObject is being despawned.
        /// </summary>
        void OnDespawn();
    }
}