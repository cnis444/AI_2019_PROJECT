using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField] private Transform rodPrefab;
    [SerializeField] private Transform floorSeparationPrefab;
    [SerializeField] private float width, length;
    [SerializeField] private float floorSepHeight;

    public float floorHeight;
    public int floorCount;

    // Start is called before the first frame update
    void Start()
    {
        InstanciateRod(0);
        for (int i = 1; i < floorCount; i++) {
            InstanciateFloorSeparation(i);
            InstanciateRod(i);
        }
    }

    private void InstanciateRod(int h) {
        Vector3 pos = new Vector3(0, (h + 0.5f) * floorHeight, 0);

        Transform r = Instantiate(rodPrefab).transform;
        r.SetParent(transform);
        r.localPosition = pos;
    }

    private void InstanciateFloorSeparation(int h) {
        Vector3 pos = new Vector3(0, h * floorHeight - floorSepHeight / 2, 0);

        Transform s = Instantiate(floorSeparationPrefab).transform;
        s.SetParent(transform);
        s.localPosition = pos;
    }
}
