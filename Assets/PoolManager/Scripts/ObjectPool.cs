using System;
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
        [SerializeField] [Range(0.0f, 1.0f)] [Tooltip("The target usage ratio")] private float _usageRatio;
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

        public bool IsPredefined { get; private set; }

        public void FromPredefined(PredefinedObjectPool value)
        {
            IsPredefined = true;
            _prefab = value.Prefab;
            _min = value.Min;
            _max = value.Max;
            _usageRatio = value.UsageRatio;
        }

        public GameObject Spawn()
        {
            GameObject element;
            if (_inactive.Count > 0)
                element = _inactive.Pop();
            else if (_max == 0 || Count < _max)
                element = Instantiate(_prefab);
            else
                return null;
            _active.Add(element);
            element.transform.parent = null;
            element.SendMessage("OnSpawn", null, SendMessageOptions.DontRequireReceiver);
            element.SetActive(true);
            return element;
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
            var element = Spawn();
            if (!element)
                return element;
            element.transform.position = position;
            element.transform.rotation = rotation;
            element.transform.parent = parent;
            return element;
        }

        public void Despawn()
        {
            foreach (var instance in _active)
            {
                Despawn(instance);
                break;
            }
        }

        public void Despawn(GameObject element)
        {
            if (!_active.Contains(element))
                return;

            _active.Remove(element);
            _inactive.Push(element);
            element.SetActive(false);
            element.SendMessage("OnDespawn", null, SendMessageOptions.DontRequireReceiver);
            element.transform.parent = transform;
        }

        // Awake is called when the script element is being loaded
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
        void OnSpawn();
        void OnDespawn();
    }
}