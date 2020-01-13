using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class CreateTerrain : MonoBehaviour
{
    const float scale = 1f;
    public int numberOfChunk;
    public Transform viewer;
    public Material mapMaterial;

    static MapGenerator mapGenerator;
    int chunkSize;

    public GameObject prefabWaterPlane;
    public TextAsset text;

    public string str;

    private void Start()
    {
        mapGenerator = FindObjectOfType<MapGenerator>();
        chunkSize = MapGenerator.mapChunkSize - 1;

        SetUp set = GameObject.Find("SetUp").GetComponent<SetUp>();

        mapGenerator.name = set.mapParam.mapName;
        mapGenerator.octave = set.mapParam.octave;
        mapGenerator.persistance = set.mapParam.persistance;
        mapGenerator.lacunarity = set.mapParam.lacunarity;
        mapGenerator.seed = set.mapParam.seed;
        numberOfChunk = set.mapParam.chunks;
        mapGenerator.meshHeightMultiplier = set.mapParam.highCoeff;
        mapGenerator.regions = new TerrainType[set.mapParam.regions.Count];
        for (int i = 0; i < set.mapParam.regions.Count; i++)
        {
            mapGenerator.regions[i] = set.mapParam.regions[i];
        }

        CreateWorld();
        HandleText.RunProcess("test.txt");

    }

    public void Update()
    {
       
    }

    void CreateWorld()
    {
        int mid = Mathf.RoundToInt(numberOfChunk / 2);
        for (int i = 0; i < numberOfChunk; i++)
        {
            for (int j = 0; j < numberOfChunk; j++)
            {
                TerrainChunk tmp = new TerrainChunk(new Vector2(i,j), chunkSize, transform, mapMaterial, prefabWaterPlane);
                //HandleText.WriteData("Assets/Ressources/test.txt", tmp.mapData);
            }
            
        }
        

    }

    public class TerrainChunk
    {
        public GameObject meshObject;
        Vector2 position;

        MeshRenderer meshRenderer;
        MeshFilter meshFilter;
        public MeshCollider meshCollider;

        public MapData mapData;
        GameObject prefabWaterPlane;


        bool alreadyMaze = false;

        public TerrainChunk(Vector2 coord, int size, Transform parent, Material material,GameObject prefabWaterPlane)
        {
            this.prefabWaterPlane = prefabWaterPlane;

            position = coord * size;
            Vector3 positionV3 = new Vector3(position.x, 0, position.y);

            meshObject = new GameObject("Terrain Chunk");
            meshRenderer = meshObject.AddComponent<MeshRenderer>();
            meshFilter = meshObject.AddComponent<MeshFilter>();
            meshRenderer.material = material;


            meshObject.transform.position = positionV3 * scale;
            meshObject.transform.parent = parent;
            meshObject.transform.localScale = Vector3.one * scale;

            GameObject water = Instantiate(prefabWaterPlane, meshObject.transform);
            water.transform.localScale = Vector3.one * MapGenerator.mapChunkSize / 10;

            CreateMapChunk(mapGenerator.GenerateMapData(position), position);
        }

        void CreateMapChunk(MapData mapData, Vector2 position)
        {
            this.mapData = mapData;
            System.Random prng = new System.Random(Mathf.RoundToInt(1000 * position.x + position.y));
            float r = prng.Next(0, 100) / 100f;

            if (r >= 0.6f && !alreadyMaze)
            {
                alreadyMaze = true;
                GameObject lab = new GameObject("lab");
                lab.transform.SetParent(meshObject.transform);
                lab.transform.localPosition = new Vector3(0, 0, 0);
            }

            Texture2D texture = TextureGenerator.TextureFromColourMap(mapData.colourMap, MapGenerator.mapChunkSize, MapGenerator.mapChunkSize);
            meshRenderer.material.mainTexture = texture;

            MeshData meshData = MeshGenerator.GenerateTerrainMesh(this.mapData.heighMap, mapGenerator.meshHeightMultiplier, mapGenerator.meshHeightCurve);
            meshFilter.mesh = meshData.CreateMesh();
            meshCollider = meshObject.AddComponent<MeshCollider>();
            meshCollider.sharedMesh = meshFilter.mesh;
        }
    }
}
