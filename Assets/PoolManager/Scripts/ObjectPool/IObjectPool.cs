using UnityEngine;
using System.Collections;

namespace PoolingSystem
{
    public interface IObjectPool
    {
        /// <summary>
        /// The prefab that defines the pool.
        /// </summary>
        GameObject Prefab { get; }

        /// <summary>
        /// The minimum number of instances to keep.
        /// </summary>
        uint Min { get; set; }

        /// <summary>
        /// The maximum number of instances to keep.
        /// </summary>
        uint Max { get; set; }

        /// <summary>
        /// The target usage ratio.
        /// </summary>
        float Ratio { get; set; }

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
        /// The current usage ratio.
        /// </summary>
        float CurrentRatio { get; }

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
        bool Instantiate();

        /// <summary>
        /// Forcedly destroy an inactive instance.
        /// </summary>
        bool Destroy();
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