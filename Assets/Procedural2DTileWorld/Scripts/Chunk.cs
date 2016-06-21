using UnityEngine;
using System.Collections;

namespace Procedural2DTileWorld
{
    public class Chunk : MonoBehaviour
    {
        //[Header("Health Settings")]
        [Tooltip("Wich tiled world this chunk belongs")] [SerializeField] private World world;
        [Tooltip("(X,Y) Position in tiled world.")] [SerializeField] private Vector2 position;
        [Tooltip("Size of the chunk.")] [SerializeField] private uint size;

        private GameObject[,] tiles;

        public World World
        {
            get { return world; }
            set { world = value; }
        }

        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        public uint Size
        {
            get { return size; }
            set
            {
                size = value;
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
        /// Generate the tiles in this chunk.
        /// </summary>
        public void Generate()
        {
            tiles = new GameObject[size, size];
            for (int i = 0; i < tiles.GetLength(0); i++)
            {
                for (int j = 0; j < tiles.GetLength(1); j++)
                {
                    //TODO Use object pool
                    tiles[i, j] = Instantiate(world.Terrain.Cobblestone);
                    tiles[i, j].transform.parent = transform;
                    tiles[i, j].transform.localPosition = Vector3.right*i + Vector3.down*j;
                }
            }
        }
    }
}