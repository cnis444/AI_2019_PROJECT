using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "MapParam", menuName = "ScriptableObjects/MapParam", order = 1)]
public class MapParam : ScriptableObject
{
    public string mapName;
    public int octave;
    public float persistance;
    public float lacunarity;
    public int seed;
    public int chunks;
    public float highCoeff;
    public List<TerrainType> regions;



}
