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
        void Awake()
        {
            _chunks = new Chunk[_settings.Size, _settings.Size];
            GenerateWorld(Vector2.zero);
        }

        // Use this for initialization
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        }

        /// <summary>
        /// Populate the world generating the chunks around a central position.
        /// </summary>
        /// <param name="position">The position of the central chunk.</param>
        private void GenerateWorld(Vector2 position)
        {
            position += (Vector2.left + Vector2.up)*_settings.StreamRadius;
            for (int i = 0; i < _settings.Size; i++)
            {
                for (int j = 0; j < _settings.Size; j++)
                {
                    //TODO Use object pool
                    if (_chunks[i, j])
                        Destroy(_chunks[i, j]);
                    _chunks[i, j] = GenerateChunk(position);
                    position += Vector2.down;
                }
                position += Vector2.right + Vector2.up*_settings.Size;
            }
        }

        /// <summary>
        /// Generate an individual chunk.
        /// </summary>
        /// <param name="position">The position of the chunk</param>
        private Chunk GenerateChunk(Vector2 position)
        {
            //TODO Use object pool
            GameObject chunkGameObject = Instantiate(_settings.Chunk);
            var chunk = chunkGameObject.GetComponent<Chunk>();
            chunk.transform.parent = transform;
            chunk.World = this;
            chunk.Size = _settings.ChunkSize;
            chunk.Position = position;
            chunk.Generate();
            return chunk;
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
        [Tooltip("Size of the chunks.")] [SerializeField] public uint ChunkSize;
        [Tooltip("Prefab of chunk.")] [SerializeField] public GameObject Chunk;
        [SerializeField] public NoiseSettings HeightMap;
        [SerializeField] public NoiseSettings SpawnProbability;
        [SerializeField] public uint StreamRadius;

        public uint Size
        {
            get { return 2*StreamRadius + 1; }
        }
    }
}