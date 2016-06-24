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

        private Chunk[,] chunks;

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
            chunks = new Chunk[_settings.Size, _settings.Size];
            UpdateWorldChunks(Vector2.zero);
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
        /// Update the world chunks.
        /// </summary>
        /// <param name="position">The position of the central chunk.</param>
        private void UpdateWorldChunks(Vector2 position)
        {
            position += (Vector2.left + Vector2.up)*_settings.StreamRadius;
            for (int i = 0; i < _settings.Size; i++)
            {
                for (int j = 0; j < _settings.Size; j++)
                {
                    //TODO Use object pool
                    GameObject chunkGameObject = Instantiate(_settings.Chunk);
                    chunks[i, j] = chunkGameObject.GetComponent<Chunk>();
                    chunks[i, j].transform.parent = transform;
                    chunks[i, j].World = this;
                    chunks[i, j].Size = _settings.ChunkSize;
                    chunks[i, j].Position = position;
                    chunks[i, j].Generate();

                    position += Vector2.down;
                }
                position += Vector2.right + Vector2.up*_settings.Size;
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

        public bool IsInRange(float value)
        {
            return MinHeight <= value && value <= MaxHeight;
        }
    }

    [Serializable]
    public struct Settings
    {
        [Tooltip("Size of the chunks.")] [SerializeField] public uint ChunkSize;
        [Tooltip("Prefab of chunk.")] [SerializeField] public GameObject Chunk;
        [SerializeField] public Vector2 Scale;
        [SerializeField] public Vector2 Offset;
        [SerializeField] public uint StreamRadius;

        public uint Size
        {
            get { return 2*StreamRadius + 1; }
        }
    }
}