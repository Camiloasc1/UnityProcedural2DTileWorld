using UnityEngine;
using System.Collections;

namespace Procedural2DTileWorld
{
    public class Chunk : MonoBehaviour
    {
        //[Header("Health TerrainSettings")]
        [Tooltip("Wich tiled world this chunk belongs")] [SerializeField] private World _world;
        [Tooltip("(X,Y) Position in tiled world.")] [SerializeField] private Vector2 _position;
        [Tooltip("Size of the chunk.")] [SerializeField] private uint _size;

        private GameObject[,] _terrain;
        private GameObject[,] _environment;

        public World World
        {
            get { return _world; }
            set { _world = value; }
        }

        public Vector2 Position
        {
            get { return _position; }
            set
            {
                _position = value;
                transform.position = _position*_size;
            }
        }

        public uint Size
        {
            get { return _size; }
            set { _size = value; }
        }

        // Awake is called when the script instance is being loaded
        void Awake()
        {
            Generate();
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
        /// Generate all the tiles in this chunk.
        /// </summary>
        public void Generate()
        {
            _terrain = new GameObject[_size, _size];
            _environment = new GameObject[_size, _size];
            for (int x = 0; x < _size; x++)
            {
                for (int y = 0; y < _size; y++)
                {
                    Generate(x, y);
                }
            }
        }

        /// <summary>
        /// Generate the tile in (X,Y)
        /// </summary>
        /// <param name="x">X position.</param>
        /// <param name="y">Y position.</param>
        private void Generate(int x, int y)
        {
            float height = Mathf.PerlinNoise(
                (_position.x*_size + x)*_world.Settings.HeightMap.Scale.x + _world.Settings.HeightMap.Offset.x,
                (_position.y*_size + y)*_world.Settings.HeightMap.Scale.y + _world.Settings.HeightMap.Offset.y
                );
            height = Mathf.Clamp01(height);
            float probability = Mathf.PerlinNoise(
                (_position.x*_size + x)*_world.Settings.SpawnProbability.Scale.x +
                _world.Settings.SpawnProbability.Offset.x,
                (_position.y*_size + y)*_world.Settings.SpawnProbability.Scale.y +
                _world.Settings.SpawnProbability.Offset.y
                );
            probability = Mathf.Clamp01(probability);
            foreach (var tile in _world.Terrain)
            {
                if (tile.CanSpawn(height, probability))
                {
                    SetTerrain(x, y, tile.Prefab);
                    break;
                }
            }
            foreach (var tile in _world.Environment)
            {
                if (tile.CanSpawn(height, probability))
                {
                    SetEnviroment(x, y, tile.Prefab);
                    break;
                }
            }
        }

        /// <summary>
        /// Set the terrain at (X,Y)
        /// </summary>
        /// <param name="x">X position.</param>
        /// <param name="y">Y position.</param>
        /// <param name="tile">The tile to set.</param>
        private void SetTerrain(int x, int y, GameObject tile)
        {
            //TODO Use object pool
            if (_terrain[x, y])
                Destroy(_terrain[x, y]);
            _terrain[x, y] = Instantiate(tile);
            _terrain[x, y].transform.parent = transform;
            _terrain[x, y].transform.localPosition = Vector3.right*x + Vector3.up*y;
        }

        /// <summary>
        /// Set the environment object at (X,Y)
        /// </summary>
        /// <param name="x">X position.</param>
        /// <param name="y">Y position.</param>
        /// <param name="tile">The tile to set.</param>
        private void SetEnviroment(int x, int y, GameObject tile)
        {
            //TODO Use object pool
            if (_environment[x, y])
                Destroy(_environment[x, y]);
            _environment[x, y] = Instantiate(tile);
            _environment[x, y].transform.parent = transform;
            _environment[x, y].transform.localPosition = Vector3.right*x + Vector3.up*y;
        }
    }
}