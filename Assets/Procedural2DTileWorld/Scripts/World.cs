using System;
using UnityEngine;
using System.Collections;

namespace Procedural2DTileWorld
{
    public class World : MonoBehaviour
    {
        [Tooltip("Environment tiles settings.")] [SerializeField] private TileSettings[] _environment;
        [Tooltip("Terrain tiles settings.")] [SerializeField] private TileSettings[] _terrain;
        [Tooltip("Settings for procedural generation.")] [SerializeField] private Settings _settings;

        private Chunk[,] _chunks;
        private Vector2 _currentPosition;
        private Transform _camera;

        public TileSettings[] Environment
        {
            get { return _environment; }
        }

        public TileSettings[] Terrain
        {
            get { return _terrain; }
        }

        public Settings Settings
        {
            get { return _settings; }
        }

        // Awake is called when the script instance is being loaded
        public void Awake()
        {
            _chunks = new Chunk[_settings.WorldSize, _settings.WorldSize];
            _camera = Camera.main.transform;
            CreateWorld();
            GenerateWorld(Vector2.zero);
        }

        // Use this for initialization
        public void Start()
        {
        }

        // Update is called once per frame
        public void Update()
        {
            var cameraPosition = new Vector2(Mathf.Floor(_camera.position.x/_settings.ChunkSize),
                Mathf.Floor(_camera.position.y/_settings.ChunkSize));
            if (!Mathf.Approximately(_currentPosition.x, cameraPosition.x) ||
                !Mathf.Approximately(_currentPosition.y, cameraPosition.y))
                GenerateWorld(cameraPosition);
        }

        /// <summary>
        /// Create all the chunks in the world.
        /// </summary>
        private void CreateWorld()
        {
            for (var x = 0; x < _settings.WorldSize; x++)
            {
                for (var y = 0; y < _settings.WorldSize; y++)
                {
                    _chunks[x, y] = CreateChunk();
                }
            }
        }

        /// <summary>
        /// Create a new chunk.
        /// </summary>
        /// <returns>A new chunk.</returns>
        private Chunk CreateChunk()
        {
            var chunkGameObject = Instantiate(_settings.Chunk);
            var chunk = chunkGameObject.GetComponent<Chunk>();
            chunk.transform.parent = transform;
            chunk.World = this;
            chunk.Size = _settings.ChunkSize;
            return chunk;
        }

        /// <summary>
        /// Generate the world based on a central position.
        /// </summary>
        /// <param name="position">The position of the central chunk.</param>
        private void GenerateWorld(Vector2 position)
        {
            _currentPosition = position;
            position += (Vector2.left + Vector2.up)*_settings.StreamRadius;
            for (var x = 0; x < _settings.WorldSize; x++)
            {
                for (var y = 0; y < _settings.WorldSize; y++)
                {
                    _chunks[x, y].Position = position;
                    _chunks[x, y].Generate();
                    position += Vector2.down;
                }
                position += Vector2.right + Vector2.up*_settings.WorldSize;
            }
        }
    }

    [Serializable]
    public struct TileSettings
    {
        [Tooltip("The name of this tile.")] [SerializeField] public string Name;
        [Tooltip("The prefab of this tile.")] [SerializeField] public GameObject Prefab;
        [Tooltip("The minimun height to spawn.")] [Range(0.0f, 1.0f)] [SerializeField] public float MinHeight;
        [Tooltip("The maximum height to spawn.")] [Range(0.0f, 1.0f)] [SerializeField] public float MaxHeight;
        [Tooltip("Use conditional spawn.")] [SerializeField] public bool UseProbability;
        [Tooltip("The probability to spawn.")] [Range(0.0f, 1.0f)] [SerializeField] public float Probability;

        public bool IsInHeightRange(float height)
        {
            return MinHeight <= height && height <= MaxHeight;
        }

        public bool IsInProbabilityRange(float probability)
        {
            if (UseProbability)
                return probability < Probability;
            return true;
        }

        public bool CanSpawn(float height, float probability)
        {
            return IsInHeightRange(height) && IsInProbabilityRange(probability);
        }
    }

    [Serializable]
    public struct NoiseSettings
    {
        [Tooltip("Scale multiplier.")] [SerializeField] public Vector2 Scale;
        [Tooltip("Offset.")] [SerializeField] public Vector2 Offset;
    }

    [Serializable]
    public struct Settings
    {
        [Header("Chunk Settings")] [Tooltip("Size of the chunks.")] [SerializeField] public uint ChunkSize;
        [Tooltip("Prefab of chunk.")] [SerializeField] public GameObject Chunk;
        [Header("World Settings")] [SerializeField] public uint StreamRadius;
        [Header("Noise Settings")] [SerializeField] public NoiseSettings HeightMap;
        [SerializeField] public NoiseSettings SpawnProbability;

        public uint WorldSize
        {
            get { return 2*StreamRadius + 1; }
        }
    }
}