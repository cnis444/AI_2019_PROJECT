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
    private SetUp setUp;

    public GameObject prefabBulding;

    private void Start()
    {
        mapGenerator = FindObjectOfType<MapGenerator>();
        chunkSize = MapGenerator.mapChunkSize - 1;

        GameObject tmpGO = GameObject.Find("SetUp");
        if (tmpGO != null)
        {
            setUp = tmpGO.GetComponent<SetUp>();
            mapGenerator.name = setUp.mapParam.mapName;
            mapGenerator.octave = setUp.mapParam.octave;
            mapGenerator.persistance = setUp.mapParam.persistance;
            mapGenerator.lacunarity = setUp.mapParam.lacunarity;
            mapGenerator.seed = setUp.mapParam.seed;
            numberOfChunk = setUp.mapParam.chunks;
            mapGenerator.meshHeightMultiplier = setUp.mapParam.highCoeff;
            mapGenerator.regions = new TerrainType[setUp.mapParam.regions.Count];
            for (int i = 0; i < setUp.mapParam.regions.Count; i++)
            {
                mapGenerator.regions[i] = setUp.mapParam.regions[i];
            }
        }
        CreateWorld();

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
                BuldingParam tmpBld = null;
                if (setUp.setOfBulding.buldings.Count > 0)
                {
                    tmpBld = setUp.setOfBulding.buldings[setUp.setOfBulding.buldings.Count - 1];
                    setUp.setOfBulding.buldings.RemoveAt(setUp.setOfBulding.buldings.Count - 1);
                }
                TerrainChunk tmp = new TerrainChunk(new Vector2(i,j), chunkSize, transform, mapMaterial, prefabWaterPlane, tmpBld, prefabBulding);
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
        GameObject prefabBulding;


        bool alreadyMaze = false;

        public TerrainChunk(Vector2 coord, int size, Transform parent, Material material,
            GameObject prefabWaterPlane, BuldingParam bldParam, GameObject prefabBulding)
        {
            this.prefabWaterPlane = prefabWaterPlane;
            this.prefabBulding = prefabBulding;

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

            CreateMapChunk(mapGenerator.GenerateMapData(position), position, bldParam);
        }

        void CreateMapChunk(MapData mapData, Vector2 position, BuldingParam bldParam)
        {
            this.mapData = mapData;

            if (bldParam != null && !alreadyMaze)
            {
                alreadyMaze = true;
                GameObject bldGO = Instantiate(prefabBulding);
                SetBuildingParam(bldGO.GetComponent<Building>(), bldParam);
                bldGO.transform.SetParent(meshObject.transform);
                bldGO.transform.localPosition = new Vector3(-bldParam.width, 1.8f, -bldParam.length);

                int half = MapGenerator.mapChunkSize / 2;
                for (int i = half - bldParam.width-2; i < half + bldParam.width+2; i++)
                {
                    for (int j = half - bldParam.length-2; j < half + bldParam.length+2 ; j++)
                    {
                        mapData.heighMap[i, j] = 0.5f;
                        mapData.colourMap[j * MapGenerator.mapChunkSize + i] = Color.grey;

                    }
                }

            }

            Texture2D texture = TextureGenerator.TextureFromColourMap(mapData.colourMap, MapGenerator.mapChunkSize, MapGenerator.mapChunkSize);
            meshRenderer.material.mainTexture = texture;

            MeshData meshData = MeshGenerator.GenerateTerrainMesh(this.mapData.heighMap, mapGenerator.meshHeightMultiplier, mapGenerator.meshHeightCurve);
            meshFilter.mesh = meshData.CreateMesh();
            meshCollider = meshObject.AddComponent<MeshCollider>();
            meshCollider.sharedMesh = meshFilter.mesh;
        }

        public void SetBuildingParam(Building b, BuldingParam p)
        {
            print(p.buldingName);
            b.width = p.width;
            b.height = p.height;
            b.length = p.length;
            b.windowProbability = p.windowProbability;
            b.minPartitionSize = p.minPartitionSize;
            b.maxPartitionSize = p.maxPartitionSize;
            b.volumeReduction = p.volumeReduction;
        }
    }
}
