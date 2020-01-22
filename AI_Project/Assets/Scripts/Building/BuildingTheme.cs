using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Theme", menuName = "Building/Theme", order = 1)]
public class BuildingTheme : ScriptableObject
{
    public Transform wallPrefab;
    public Transform doorPrefab;
    public Transform windowPrefab;
    public Transform cornerPrefab;
    public Transform roofPrefab;
    public Transform elevatorPrefab;

    public Vector3 wallDim;
    public Vector3 cornerDim;
    public Vector3 roofDim;
    public Vector3 elevatorDim;
}
