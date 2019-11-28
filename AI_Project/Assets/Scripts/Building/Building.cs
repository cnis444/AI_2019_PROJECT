using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public int seed;
    public bool useRandomSeed = true;

    public int width, length, height;
    [Range(0,1)]
    public float windowProbability;

    List<Wall>[] stages;

    // Start is called before the first frame update
    void Start()
    {
        if (useRandomSeed) {
            seed = (int)System.DateTime.Now.Ticks;
        }
        Random.InitState(seed);

        // Init walls
        stages = new List<Wall>[height];
        for (int k = 0; k < height; k++) {
            List<Vector3> corners = new List<Vector3>();
            corners.Add(new Vector3(0, k, 0));
            corners.Add(new Vector3(width, k, 0));
            corners.Add(new Vector3(width, k, length));
            corners.Add(new Vector3(0, k, length));

            stages[k] = new List<Wall>();
            for (int i = 0; i < corners.Count - 1; i++) {
                stages[k].Add(new Wall(corners[i], corners[i + 1]));
            }
            stages[k].Add(new Wall(corners[corners.Count - 1], corners[0]));
        }

        // Set a door
        stages[0][Random.Range(0, stages[0].Count)].AddDoor();

        // Add windows
        foreach (List<Wall> walls in stages) {
            foreach (Wall w in walls) {
                w.AddWindows(windowProbability);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        DebugSquare();
        foreach (List<Wall> walls in stages) {
            foreach (Wall w in walls) {
                DebugWall(w);
            }
        }
    }

    void DebugWall(Wall wall) {
        Debug.DrawLine(wall.start, wall.end, Color.blue);
        for (int i = 0; i < wall.types.Length; i++) {
            if (wall.types[i] == WallType.Door) {
                Vector3 dir = Vector3.Normalize(wall.end - wall.start);
                Debug.DrawLine(wall.start + i * dir, wall.start + i * dir + Vector3.up, Color.green);
                Debug.DrawLine(wall.start + (i+1) * dir, wall.start + (i+1) * dir + Vector3.up, Color.green);
            }
            if (wall.types[i] == WallType.Window) {
                Vector3 dir = Vector3.Normalize(wall.end - wall.start);
                Vector3 v1 = wall.start + i * dir + 0.25f * (dir + Vector3.up);
                Vector3 v2 = wall.start + (i + 1) * dir + 0.25f * (-dir + Vector3.up);
                Vector3 v3 = v1 + Vector3.up * 0.5f;
                Vector3 v4 = v2 + Vector3.up * 0.5f;
                Debug.DrawLine(v1, v2, Color.green);
                Debug.DrawLine(v2, v4, Color.green);
                Debug.DrawLine(v4, v3, Color.green);
                Debug.DrawLine(v3, v1, Color.green);
            }
        }
    }

    void DebugSquare() {
        // Bottom rectangle
        Debug.DrawLine(new Vector3(0, 0, 0), new Vector3(width, 0, 0), Color.red);
        Debug.DrawLine(new Vector3(width, 0, 0), new Vector3(width, 0, length), Color.red);
        Debug.DrawLine(new Vector3(width, 0, length), new Vector3(0, 0, length), Color.red);
        Debug.DrawLine(new Vector3(0, 0, length), new Vector3(0, 0, 0), Color.red);

        // Top rectangle
        Debug.DrawLine(new Vector3(0, height, 0), new Vector3(width, height, 0), Color.red);
        Debug.DrawLine(new Vector3(width, height, 0), new Vector3(width, height, length), Color.red);
        Debug.DrawLine(new Vector3(width, height, length), new Vector3(0, height, length), Color.red);
        Debug.DrawLine(new Vector3(0, height, length), new Vector3(0, height, 0), Color.red);

        // Vertical lines
        Debug.DrawLine(new Vector3(0, 0, 0), new Vector3(0, height, 0), Color.red);
        Debug.DrawLine(new Vector3(width, 0, 0), new Vector3(width, height, 0), Color.red);
        Debug.DrawLine(new Vector3(width, 0, length), new Vector3(width, height, length), Color.red);
        Debug.DrawLine(new Vector3(0, 0, length), new Vector3(0, height, length), Color.red);
    }
}
