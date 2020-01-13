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
    private int rectangleToRemove = 0;
    [Range(0,1)]
    public float volumeReduction;
    private int m = 0;

    List<Wall>[] stages;
    List<RectInt>[] stagesRect;
    Area[] stagesArea;
    SpacePartition partition;
    List<RectInt> rects;
    Area area;

    Vector2 elevatorPos;

    //public GameObject wallPrefab;
    //public Vector3 wallPrefabDim;
    //public GameObject roofPrefab;
    //public Vector3 roofPrefabDim;

    public BuildingTheme theme;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Generating building");
        if (useRandomSeed) {
            seed = (int)System.DateTime.Now.Ticks;
        }
        Random.InitState(seed);

        System.DateTime start = System.DateTime.Now;

        partition = new SpacePartition(new RectInt(0, 0, width, length), minPartitionSize, maxPartitionSize, Random.Range(-1000000, 1000000));
        rects = partition.GetSpaces();
        int count = Mathf.FloorToInt(rects.Count * volumeReduction);
        Debug.Log("floorToInt " + count);
        //rectangleToRemove = Mathf.FloorToInt(rects.Count * volumeReduction);
        //Debug.Log("floorToInt done");

        stages = new List<Wall>[height];
        stagesRect = new List<RectInt>[height];
        stagesArea = new Area[height];
        for (int k = 0; k < height; k++) {
            RemoveSpace(count);
            BuildArea(k);
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

        // Set an elevator
        if (height > 1) {
            List<Vector2> coords = new List<Vector2>();
            foreach (RectInt r in stagesRect[stages.Length - 1]) {
                for (int x = r.xMin; x < r.xMax; x++) {
                    for (int z = r.yMin; z < r.yMax; z++) {
                        Vector2 loc = new Vector2(x + 0.5f, z + 0.5f);
                        if (stagesArea[0].Contain(loc, true)) {
                            coords.Add(loc);
                        }
                    }
                }
            }
            elevatorPos = coords[Random.Range(0, coords.Count)];
        }
        else {
            elevatorPos = new Vector2(-1, -1);
        }


        BuildGround();
        for (int k = 0; k < height; k++) {
            Build(k);
        }

        Debug.Log(string.Format("Building built in {0} ms", (System.DateTime.Now - start).Milliseconds));
    }

    // Update is called once per frame
    void Update()
    {
        //DebugSquare();
        //DebugPartition();
        //foreach (List<Wall> walls in stages) {
        //    foreach (Wall w in walls) {
        //        DebugWall(w);
        //    }
        //}
        //DebugArea();
    }

    void InitWalls() {
        stages = new List<Wall>[height];
        for (int k = 0; k < height; k++) {
            BuildWall(k);
        }
    }

    void RemoveSpace() {
        RemoveSpace(rectangleToRemove);
    }

    void RemoveSpace(int count) {
        //Debug.Log("Removing space");
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
        for (int k = 0; k < Mathf.Min(count, rects.Count - 1); k++) {
            int idx = Random.Range(0, border.Count);
            RectInt rect = border[idx];
            //Debug.Log(string.Format("Deleting rectangle {0}", rect));
            border.RemoveAt(idx);
            
            // Some rectangles from the inside are now on the border
            for (int i = inside.Count - 1; i >= 0; i--) { // from last to first
                if (Area.TouchingEdges(rect, inside[i]).Count > 0) {
                    //print("switching");
                    border.Add(inside[i]); // /!\ Bug here
                    inside.RemoveAt(i);    // /!\ or here
                }
            }

            //List<RectInt> remove = new List<RectInt>();
            //foreach (RectInt r in inside) {
            //    if (Area.TouchingEdges(rect, r).Count > 0) {
            //        border.Add(r);
            //        remove.Add(r);
            //    }
            //}
            //Debug.Log("inside: " + ListToString(inside));
            //Debug.Log("remove: " + ListToString(remove));
            //if (m < 64) {
            //    Debug.Log("removing");
            //    foreach(RectInt r in remove) { inside.Remove(r); }
            //}
            //m++;
        }
        rects = border;
        rects.AddRange(inside);
        
    }

    string ListToString<T>(List<T> l) {
        if (l.Count == 0) { return "(0)<Empty>"; }
        string s = "(" + l.Count + ")<" + l[0];
        for (int i = 1; i < l.Count; i++) {
            s += ", " + l[i];
        }
        s += ">";
        return s;
    }

    void BuildWall(int h) {
        List<Vector3> corners = new List<Vector3>();
        for (int i = 0; i < area.Main.Count; i++) {
            corners.Add(new Vector3(area.Main[i].x, h, area.Main[i].y));
        }

        stages[h] = new List<Wall>();
        if (corners.Count > 0) {
            for (int i = 0; i < corners.Count - 1; i++) {
                stages[h].Add(new Wall(corners[i], corners[i + 1]));
            }
            stages[h].Add(new Wall(corners[corners.Count - 1], corners[0]));
        }
    }

    void BuildArea(int h) {
        // Build the area
        area = new Area();
        stagesRect[h] = new List<RectInt>();
        //Debug.Log(string.Format("{0} rectangles remaining", rects.Count));
        //for (int i = 0; i < m; i++) {
        for (int i = 0; i < rects.Count; i++) {
            area.Add(rects[i]);
            stagesRect[h].Add(rects[i]);
        }
        stagesArea[h] = area;
        //Debug.Log(string.Format("area has {0} points: {1}", area.Main.Count, ListToString<Vector2>(area.Main)));
    }

    void InstanciateWall(Transform prefab, Wall wall, int i, Vector3 dir) {
        Vector3 cornerPos = (new Vector3(wall.start.x, 0, wall.start.z) + (i) * dir) * theme.wallDim.x;
        float height = wall.start.y * theme.wallDim.y + theme.wallDim.y / 2;
        cornerPos.y = height;
        Vector3 pos = cornerPos + dir * 0.5f * theme.wallDim.x;
        pos.y = height;
        Vector3 rot = Vector3.up * (Vector3.SignedAngle(Vector3.right, dir, Vector3.up) + 90);
        var c = Instantiate(theme.cornerPrefab);
        c.transform.localPosition = cornerPos;
        c.transform.parent = transform;
        var w = Instantiate(prefab);
        w.transform.localPosition = pos;
        w.transform.parent = transform;
        w.transform.Rotate(rot);
    }

    void BuildGround() {
        foreach (RectInt r in stagesRect[0]) {
            for (int x = r.xMin; x < r.xMax; x++) {
                for (int z = r.yMin; z < r.yMax; z++) {
                    Vector2 loc = new Vector2(x + 0.5f, z + 0.5f);
                    Vector3 pos = new Vector3((x + 0.5f) * theme.roofDim.x, -theme.roofDim.y / 2, (z + 0.5f) * theme.roofDim.z);
                    pos.y += 0.001f; // texture clipping protection
                    if (stagesArea[0].Contain(loc, true)) {
                        var roof = Instantiate(theme.roofPrefab);
                        roof.transform.parent = transform;
                        roof.transform.localPosition = pos;
                    }
                }
            }
        }
    }

    void Build(int h) {
        // Instanciate walls
        foreach (Wall wall in stages[h]) {
            Vector3 dir = Vector3.Normalize(wall.end - wall.start);
            //Debug.Log(wall.start + " to " + wall.end);
            for (int i = 0; i < wall.types.Length; i++) {
                switch (wall.types[i]) {
                    case WallType.Wall:
                        InstanciateWall(theme.wallPrefab, wall, i, dir);
                        break;
                    case WallType.Door:
                        InstanciateWall(theme.doorPrefab, wall, i, dir);
                        break;
                    case WallType.Window:
                        InstanciateWall(theme.windowPrefab, wall, i, dir);
                        break;
                }
            }
        }

        // Instanciate roofs
        foreach (RectInt r in stagesRect[h]) {
            for (int x = r.xMin; x < r.xMax; x++) {
                for (int z = r.yMin; z < r.yMax; z++) {
                    Vector2 loc = new Vector2(x + 0.5f, z + 0.5f);
                    if (loc != elevatorPos || h == height - 1) {
                        Vector3 pos = new Vector3((x + 0.5f) * theme.roofDim.x, theme.wallDim.y * (h + 1) - theme.roofDim.y / 2, (z + 0.5f) * theme.roofDim.z);
                        pos.y += 0.001f; // texture clipping protection
                        if (stagesArea[h].Contain(loc, true)) {
                            var roof = Instantiate(theme.roofPrefab);
                            roof.transform.parent = transform;
                            roof.transform.localPosition = pos;
                        }
                    }
                }
            }
        }

        var elevator = Instantiate(theme.elevatorPrefab).transform;
        elevator.parent = transform;
        elevator.localPosition = new Vector3(elevatorPos.x * theme.elevatorDim.x, theme.wallDim.y * (h + 1) - theme.elevatorDim.y / 2, elevatorPos.y * theme.elevatorDim.z);
    }

    #region Debug

    void DebugWall(Wall wall) {
        Vector3 start = new Vector3(wall.start.x * theme.wallDim.x, wall.start.y * theme.wallDim.y, wall.start.z * theme.wallDim.x);
        Vector3 end = new Vector3(wall.end.x * theme.wallDim.x, wall.end.y * theme.wallDim.y, wall.end.z * theme.wallDim.x);
        Debug.DrawLine(start, end, Color.blue);
        for (int i = 0; i < wall.types.Length; i++) {
            if (wall.types[i] == WallType.Door) {
                Vector3 dir = Vector3.Normalize(wall.end - wall.start) * theme.wallDim.x;
                Debug.DrawLine(start + i * dir, start + i * dir + Vector3.up * theme.wallDim.y, Color.green);
                Debug.DrawLine(start + (i+1) * dir, start + (i+1) * dir + Vector3.up * theme.wallDim.y, Color.green);
            }
            if (wall.types[i] == WallType.Window) {
                Vector3 dir = Vector3.Normalize(wall.end - wall.start) * theme.wallDim.x;
                Vector3 v1 = start + i * dir/* * theme.wallDim.x*/ + 0.25f * (dir + Vector3.up);
                Vector3 v2 = start + (i + 1) * dir/* * theme.wallDim.x*/ + 0.25f * (-dir + Vector3.up);
                Vector3 v3 = v1 + Vector3.up * 0.5f * theme.wallDim.y;
                Vector3 v4 = v2 + Vector3.up * 0.5f * theme.wallDim.y;
                Debug.DrawLine(v1, v2, Color.green);
                Debug.DrawLine(v2, v4, Color.green);
                Debug.DrawLine(v4, v3, Color.green);
                Debug.DrawLine(v3, v1, Color.green);
            }
        }
    }

    void DebugSquare() {
        Vector3 v1 = new Vector3(0, 0, 0);
        Vector3 v2 = new Vector3(width * theme.wallDim.x, 0, 0);
        Vector3 v3 = new Vector3(width, 0, length) * theme.wallDim.x;
        Vector3 v4 = new Vector3(0, 0, length * theme.wallDim.x);
        Vector3 v5 = new Vector3(0, height * theme.wallDim.y, 0);
        Vector3 v6 = new Vector3(width * theme.wallDim.x, height * theme.wallDim.y, 0);
        Vector3 v7 = new Vector3(width * theme.wallDim.x, height * theme.wallDim.y, length * theme.wallDim.x);
        Vector3 v8 = new Vector3(0, height * theme.wallDim.y, length * theme.wallDim.x);

        // Bottom rectangle
        Debug.DrawLine(v1, v2, Color.red);
        Debug.DrawLine(v2, v3, Color.red);
        Debug.DrawLine(v3, v4, Color.red);
        Debug.DrawLine(v4, v1, Color.red);

        // Top rectangle
        Debug.DrawLine(v5, v6, Color.red);
        Debug.DrawLine(v6, v7, Color.red);
        Debug.DrawLine(v7, v8, Color.red);
        Debug.DrawLine(v8, v5, Color.red);

        // Vertical lines
        Debug.DrawLine(v1, v5, Color.red);
        Debug.DrawLine(v2, v6, Color.red);
        Debug.DrawLine(v3, v7, Color.red);
        Debug.DrawLine(v4, v8, Color.red);
    }

    void DebugPartition() {
        //Debug.Log(rects);
        foreach (RectInt r in rects) {
            Vector3 v1 = new Vector3(r.xMin, 0, r.yMin) * theme.wallDim.x;
            Vector3 v2 = new Vector3(r.xMin, 0, r.yMax) * theme.wallDim.x;
            Vector3 v3 = new Vector3(r.xMax, 0, r.yMax) * theme.wallDim.x;
            Vector3 v4 = new Vector3(r.xMax, 0, r.yMin) * theme.wallDim.x;
            Debug.DrawLine(v1, v2, Color.yellow);
            Debug.DrawLine(v2, v3, Color.yellow);
            Debug.DrawLine(v3, v4, Color.yellow);
            Debug.DrawLine(v4, v1, Color.yellow);
        }
    }

    void DebugArea() {

        if (m < rects.Count) {
            RectInt r = rects[m];
            Vector3 v1 = new Vector3(r.xMin, 0, r.yMin) * theme.wallDim.x;
            Vector3 v2 = new Vector3(r.xMin, 0, r.yMax) * theme.wallDim.x;
            Vector3 v3 = new Vector3(r.xMax, 0, r.yMax) * theme.wallDim.x;
            Vector3 v4 = new Vector3(r.xMax, 0, r.yMin) * theme.wallDim.x;
            Debug.DrawLine(v1, v2, Color.green);
            Debug.DrawLine(v2, v3, Color.green);
            Debug.DrawLine(v3, v4, Color.green);
            Debug.DrawLine(v4, v1, Color.green);
            Debug.DrawLine(v1, v1 + Vector3.up, Color.green);
            Debug.DrawLine(v2, v2 + Vector3.up, Color.green);
            Debug.DrawLine(v3, v3 + Vector3.up, Color.green);
            Debug.DrawLine(v4, v4 + Vector3.up, Color.green);
        }

        foreach (List<Vector2> points in area.Surfaces) {
            //List<Vector2> points = area.Main;
            for (int i = 0; i < points.Count - 1; i++) {
                Vector3 p1 = new Vector3(points[i].x, 0, points[i].y) * theme.wallDim.x;
                Vector3 p2 = new Vector3(points[i + 1].x, 0, points[i + 1].y) * theme.wallDim.x;
                Debug.DrawLine(p1, p2, Color.magenta);
                Debug.DrawLine(p1, p1 + Vector3.up * 0.5f, Color.magenta);
            }
            Debug.DrawLine(new Vector3(points[points.Count - 1].x, 0, points[points.Count - 1].y) * theme.wallDim.x, new Vector3(points[0].x, 0, points[0].y) * theme.wallDim.x, Color.magenta);
            Debug.DrawLine(new Vector3(points[points.Count - 1].x, 0, points[points.Count - 1].y) * theme.wallDim.x, new Vector3(points[points.Count - 1].x, 0.5f, points[points.Count - 1].y) * theme.wallDim.x, Color.magenta);
        }
    }

    #endregion
}
