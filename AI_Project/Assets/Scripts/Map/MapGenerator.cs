using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;

public class MapGenerator : MonoBehaviour
{

    public const int mapChunkSize = 100;
    public float noiseScale;

    public int octave;
    [Range(0, 1)]
    public float persistance;
    public float lacunarity;

    public int seed;
    public Vector2 offset;

    public bool useFaloff;

    public float meshHeightMultiplier;
    public AnimationCurve meshHeightCurve;

    public TerrainType[] regions;

    float[,] fallofMap;


    public void Awake()
    {
        fallofMap = FallOfGenerator.GenerateFallofMap(mapChunkSize);
    }

    private void Start()
    {
        
    }

    public MapData GenerateMapData(Vector2 center)
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapChunkSize, mapChunkSize, seed, noiseScale,
            octave, persistance, lacunarity, center + offset); 

         Color[] colorMap = new Color[mapChunkSize * mapChunkSize];
        for (int y = 0; y < mapChunkSize; y++)
        {
            for (int x = 0; x < mapChunkSize; x++)
            {
                float currentHeight = noiseMap[x, y];
                for (int i = 0; i < regions.Length; i++)
                {
                    if (currentHeight >= regions[i].height)
                    {
                        colorMap[y * mapChunkSize + x] = regions[i].colour;
                    }
                    else break;
                }
            }
        }

        return new MapData(noiseMap, colorMap);

    }


    private void Update()
    {
        
    }

    private void OnValidate()
    {
        if (lacunarity < 1)
            lacunarity = 1;

        if (octave < 0)
            octave = 0;

        if (octave > 10)
            octave = 9;

        fallofMap = FallOfGenerator.GenerateFallofMap(mapChunkSize);
    }

}


[System.Serializable]
public struct TerrainType
{
    public string name;
    public float height;
    public Color colour;
}

public struct MapData
{
    public readonly float[,] heighMap;
    public readonly Color[] colourMap;

    public MapData(float[,] heighMap, Color[] colourMap)
    {
        this.heighMap = heighMap;
        this.colourMap = colourMap;
    }
}
