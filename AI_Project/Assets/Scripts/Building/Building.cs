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
    public int minPartitionSize, maxPartitionSize;
    public int RectangleToRemove;
    public int m;

    List<Wall>[] stages;
    SpacePartition partition;
    List<RectInt> rects;
    Area area;

    // Start is called before the first frame update
    void Start()
    {
        if (useRandomSeed) {
            seed = (int)System.DateTime.Now.Ticks;
        }
        Random.InitState(seed);

        partition = new SpacePartition(new RectInt(0, 0, width, length), minPartitionSize, maxPartitionSize, Random.Range(-1000000, 1000000));
        rects = partition.GetSpaces();

        //InitSpace();
        //InitWalls();
        stages = new List<Wall>[height];
        for (int k = 0; k < height; k++) {
            RemoveSpace();
            BuildArea();
            BuildWall(k);
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
        DebugPartition();
        foreach (List<Wall> walls in stages) {
            foreach (Wall w in walls) {
                DebugWall(w);
            }
        }
        //DebugArea();
    }

    void InitSpace() {
        RemoveSpace();

        BuildArea();
    }

    void InitWalls() {
        stages = new List<Wall>[height];
        for (int k = 0; k < height; k++) {
            BuildWall(k);
        }
    }

    void RemoveSpace() {
        List<RectInt> border = new List<RectInt>();
        List<RectInt> inside = new List<RectInt>();
        foreach (RectInt r in rects) {
            if (r.xMin == 0 || r.yMin == 0 || r.xMax == width || r.yMax == length) {
                border.Add(r);
            }
            else {
                inside.Add(r);
            }
        }

        // Delete some rectangles on the border
        for (int k = 0; k < RectangleToRemove; k++) {
            int idx = Random.Range(0, border.Count);
            RectInt rect = border[idx];
            Debug.Log(string.Format("Deleting rectangle {0}", rect));
            border.RemoveAt(idx);

            // Some rectangles from the inside are now on the border
            for (int i = inside.Count - 1; i >= 0; i--) {
                if (Area.TouchingEdges(rect, inside[i]).Count > 0) {
                    border.Add(inside[i]);
                    inside.RemoveAt(i);
                }
            }
        }
        rects = border;
        rects.AddRange(inside);
    }

    void BuildWall(int h) {
        List<Vector3> corners = new List<Vector3>();
        //corners.Add(new Vector3(0, k, 0));
        //corners.Add(new Vector3(width, k, 0));
        //corners.Add(new Vector3(width, k, length));
        //corners.Add(new Vector3(0, k, length));
        for (int i = 0; i < area.Main.Count; i++) {
            corners.Add(new Vector3(area.Main[i].x, h, area.Main[i].y));
        }

        stages[h] = new List<Wall>();
        for (int i = 0; i < corners.Count - 1; i++) {
            stages[h].Add(new Wall(corners[i], corners[i + 1]));
        }
        stages[h].Add(new Wall(corners[corners.Count - 1], corners[0]));
    }

    void BuildArea() {
        // Build the area
        area = new Area();
        Debug.Log(string.Format("{0} rectangles remaining", rects.Count));
        for (int i = 0; i < rects.Count; i++) {
            area.Add(rects[i]);
        }
        Debug.Log(string.Format("area has {0} points", area.Main.Count));
    }

    #region Debug

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

    void DebugPartition() {
        //Debug.Log(rects);
        foreach (RectInt r in rects) {
            Vector3 v1 = new Vector3(r.xMin, 0, r.yMin);
            Vector3 v2 = new Vector3(r.xMin, 0, r.yMax);
            Vector3 v3 = new Vector3(r.xMax, 0, r.yMax);
            Vector3 v4 = new Vector3(r.xMax, 0, r.yMin);
            Debug.DrawLine(v1, v2, Color.yellow);
            Debug.DrawLine(v2, v3, Color.yellow);
            Debug.DrawLine(v3, v4, Color.yellow);
            Debug.DrawLine(v4, v1, Color.yellow);
        }
    }

    void DebugArea() {

        foreach (List<Vector2> points in area.Surfaces) {
            //List<Vector2> points = area.Main;
            for (int i = 0; i < points.Count - 1; i++) {
                Vector3 p1 = new Vector3(points[i].x, 0, points[i].y);
                Vector3 p2 = new Vector3(points[i + 1].x, 0, points[i + 1].y);
                Debug.DrawLine(p1, p2, Color.magenta);
                Debug.DrawLine(p1, p1 + Vector3.up * 0.5f, Color.magenta);
            }
            Debug.DrawLine(new Vector3(points[points.Count - 1].x, 0, points[points.Count - 1].y), new Vector3(points[0].x, 0, points[0].y), Color.magenta);
            Debug.DrawLine(new Vector3(points[points.Count - 1].x, 0, points[points.Count - 1].y), new Vector3(points[points.Count - 1].x, 0.5f, points[points.Count - 1].y), Color.magenta);
        }

        if (m < rects.Count) {
            RectInt r = rects[m];
            Vector3 v1 = new Vector3(r.xMin, 0, r.yMin);
            Vector3 v2 = new Vector3(r.xMin, 0, r.yMax);
            Vector3 v3 = new Vector3(r.xMax, 0, r.yMax);
            Vector3 v4 = new Vector3(r.xMax, 0, r.yMin);
            Debug.DrawLine(v1, v2, Color.yellow);
            Debug.DrawLine(v2, v3, Color.yellow);
            Debug.DrawLine(v3, v4, Color.yellow);
            Debug.DrawLine(v4, v1, Color.yellow);
        }
    }

    #endregion
}
