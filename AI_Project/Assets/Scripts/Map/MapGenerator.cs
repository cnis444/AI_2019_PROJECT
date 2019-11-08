using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{

    public enum DrawMode { NoiseMap, ColourMap, Mesh };
    public DrawMode drawMode;

    public const int mapChunkSize = 241;
    [Range(0, 6)]
    public int levelOfDetail;
    public float noiseScale;

    public int octave;
    [Range(0, 1)]
    public float persistance;
    public float lacunarity;

    public int seed;
    public Vector2 offset;

    public float meshHeightMultiplier;
    public AnimationCurve meshHeightCurve;

    public bool autoUpdate;

    public TerrainType[] regions;

    public void GenerateMap()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapChunkSize, mapChunkSize, seed, noiseScale,
            octave, persistance, lacunarity, offset);


        Color[] colorMap = new Color[mapChunkSize * mapChunkSize];
        for (int y = 0; y < mapChunkSize; y++)
        {
            for (int x = 0; x < mapChunkSize; x++)
            {
                float currentHeight = noiseMap[x, y];
                for (int i = 0; i < regions.Length; i++)
                {
                    if (currentHeight <= regions[i].height)
                    {
                        colorMap[y * mapChunkSize + x] = regions[i].colour;
                        break;
                    }

                }
            }
        }

        MapDisplay mapDisplay = FindObjectOfType<MapDisplay>();
        if (drawMode == DrawMode.NoiseMap)
            mapDisplay.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap));
        else if (drawMode == DrawMode.ColourMap)
            mapDisplay.DrawTexture(TextureGenerator.TextureFromColourMap(colorMap, mapChunkSize, mapChunkSize));
        else if (drawMode == DrawMode.Mesh)
            mapDisplay.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap, meshHeightMultiplier, meshHeightCurve, levelOfDetail), TextureGenerator.TextureFromColourMap(colorMap, mapChunkSize, mapChunkSize));


    }

    private void OnValidate()
    {
        if (lacunarity < 1)
            lacunarity = 1;

        if (octave < 0)
            octave = 0;


    }
}


[System.Serializable]
public struct TerrainType
{
    public string name;
    public float height;
    public Color colour;
}
