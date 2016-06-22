﻿using UnityEngine;
using System.Collections;

namespace Procedural2DTileWorld
{
    public class Chunk : MonoBehaviour
    {
        //[Header("Health TerrainSettings")]
        [Tooltip("Wich tiled world this chunk belongs")] [SerializeField] private World _world;
        [Tooltip("(X,Y) Position in tiled world.")] [SerializeField] private Vector2 _position;
        [Tooltip("Size of the chunk.")] [SerializeField] private uint _size;

        private GameObject[,] terrain;
        private GameObject[,] enviroment;

        public World World
        {
            get { return _world; }
            set { _world = value; }
        }

        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public uint Size
        {
            get { return _size; }
            set
            {
                _size = value;
                Generate();
            }
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
            terrain = new GameObject[_size, _size];
            enviroment = new GameObject[_size, _size];
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
            float value = Mathf.PerlinNoise(
                (_position.x*_size + x)*_world.Settings.Scale.x + _world.Settings.Offset.x,
                (_position.y*_size + y)*_world.Settings.Scale.y + _world.Settings.Offset.y
                );
            //value = Mathf.Clamp01(value);
            value *= _world.Settings.Multiplier;
            if (value < _world.Terrain.TerrainSettings.Water)
            {
                SetTerrain(x, y, _world.Terrain.Water);
            }
            else if (value < _world.Terrain.TerrainSettings.Sand)
            {
                SetTerrain(x, y, _world.Terrain.Sand);
            }
            else if (value < _world.Terrain.TerrainSettings.Grass)
            {
                SetTerrain(x, y, _world.Terrain.Grass);
            }
            //else if (value < world.Terrain.TerrainSettings.Tree)
            else
            {
                SetTerrain(x, y, _world.Terrain.Grass);
                SetEnviroment(x, y, _world.Enviroment.Tree);
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
            terrain[x, y] = Instantiate(tile);
            terrain[x, y].transform.parent = transform;
            terrain[x, y].transform.localPosition = Vector3.right*x + Vector3.down*y;
        }

        /// <summary>
        /// Set the enviroment object at (X,Y)
        /// </summary>
        /// <param name="x">X position.</param>
        /// <param name="y">Y position.</param>
        /// <param name="tile">The tile to set.</param>
        private void SetEnviroment(int x, int y, GameObject tile)
        {
            //TODO Use object pool
            enviroment[x, y] = Instantiate(tile);
            enviroment[x, y].transform.parent = transform;
            enviroment[x, y].transform.localPosition = Vector3.right*x + Vector3.down*y;
        }
    }
}