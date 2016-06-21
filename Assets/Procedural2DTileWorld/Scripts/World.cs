using System;
using UnityEngine;
using System.Collections;

namespace Procedural2DTileWorld
{
    public class World : MonoBehaviour
    {
        [Tooltip("Size of the chunks.")] [SerializeField] private uint chunkSize = 16;
        [Tooltip("Prefab of chunk.")] [SerializeField] private GameObject chunk;
        [Tooltip("Enviroment tiles prefabs.")] [SerializeField] private Enviroment enviroment;
        [Tooltip("Terrain tiles prefabs.")] [SerializeField] private Terrain terrain;
        [Tooltip("Settings for procedural generation.")] [SerializeField] private Settings settings;

        private Chunk[,] chunks;

        public Enviroment Enviroment
        {
            get { return enviroment; }
        }

        public Terrain Terrain
        {
            get { return terrain; }
        }

        public Settings Settings
        {
            get { return settings; }
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
                    GameObject chunkGameObject = Instantiate(chunk);
                    chunks[i, j] = chunkGameObject.GetComponent<Chunk>();
                    chunks[i, j].transform.parent = transform;
                    chunks[i, j].transform.localPosition = Vector3.right*i + Vector3.down*j;
                    chunks[i, j].World = this;
                    chunks[i, j].Size = chunkSize;
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
    public struct Enviroment
    {
        [SerializeField] public GameObject DeadBush;
        [SerializeField] public GameObject Flower;
        [SerializeField] public GameObject Mushroom;
        [SerializeField] public GameObject TallGrass;
        [SerializeField] public GameObject Tree;
        [SerializeField] public GameObject WaterLily;
    }

    [Serializable]
    public struct Terrain
    {
        [SerializeField] public GameObject Cobblestone;
        [SerializeField] public GameObject Dirt;
        [SerializeField] public GameObject Grass;
        [SerializeField] public GameObject Gravel;
        [SerializeField] public GameObject Podzol;
        [SerializeField] public GameObject Sand;
        [SerializeField] public GameObject Sandstone;
        [SerializeField] public GameObject Water;
    }

    [Serializable]
    public struct Settings
    {
        [SerializeField] public float Water;
        [SerializeField] public float Sand;
        [SerializeField] public float Grass;
        [SerializeField] public float Tree;
    }
}