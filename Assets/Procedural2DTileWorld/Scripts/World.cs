using System;
using UnityEngine;
using System.Collections;

namespace Procedural2DTileWorld
{
    public class World : MonoBehaviour
    {
        [Tooltip("Size of the chunks.")] [SerializeField] private uint _chunkSize = 16;
        [Tooltip("Prefab of chunk.")] [SerializeField] private GameObject _chunk;
        [Tooltip("Enviroment tiles settings.")] [SerializeField] private TileSettings[] _enviroment;
        [Tooltip("Terrain tiles settings.")] [SerializeField] private TileSettings[] _terrain;
        [Tooltip("Settings for procedural generation.")] [SerializeField] private Settings _settings;

        private Chunk[,] chunks;

        public TileSettings[] Enviroment
        {
            get { return _enviroment; }
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
            chunks = new Chunk[1, 1];
            for (int i = 0; i < chunks.GetLength(0); i++)
            {
                for (int j = 0; j < chunks.GetLength(1); j++)
                {
                    //TODO Use object pool
                    GameObject chunkGameObject = Instantiate(_chunk);
                    chunks[i, j] = chunkGameObject.GetComponent<Chunk>();
                    chunks[i, j].transform.parent = transform;
                    chunks[i, j].transform.localPosition = Vector3.right*i + Vector3.down*j;
                    chunks[i, j].World = this;
                    chunks[i, j].Size = _chunkSize;
                }
            }
        }

        // Use this for initialization
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
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
        [SerializeField] public Vector2 Scale;
        [SerializeField] public Vector2 Offset;
    }
}