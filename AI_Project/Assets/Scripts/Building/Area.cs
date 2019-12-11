using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Area
{
    List<List<Vector2>> vertices;
    int step;

    public List<Vector2> Main {
        get {
            return vertices.Count > 0 ? vertices[0] : new List<Vector2>();
        }
    }

    public List<List<Vector2>> Surfaces {
        get {
            return vertices;
        }
    }

    public Area() {
        vertices = new List<List<Vector2>>();
        step = -1;
    }

    private static List<Vector2> RectToList(RectInt rect) {
        return new List<Vector2> {
            new Vector2(rect.xMin, rect.yMin),
            new Vector2(rect.xMax, rect.yMin),
            new Vector2(rect.xMax, rect.yMax),
            new Vector2(rect.xMin, rect.yMax)
        };
    }

    public bool Contain(Vector2 point, bool mainAreaOnly = false) {
        float angle = Mathf.PI / 41;
        Vector2 u = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        //Debug.DrawLine(new Vector3(point.x, 0, point.y), new Vector3(point.x + 20 * u.x, 0, point.y + 20 * u.y), Color.magenta, 300);
        Vector2 c = point, d = u;
        int stop = mainAreaOnly ? 1 : vertices.Count;
        for (int k = 0; k < stop; k++) {
            int cpt = 0;
            bool badSituation = false;
            do {
                for (int i = 0; i < vertices[k].Count - 1; i++) {
                    if (PointsAreAligned(point, point + u, vertices[k][i], vertices[k][i + 1])) {
                        // Change u and restart
                        badSituation = true;
                        u = new Vector2(u.x * Mathf.Cos(angle) - u.y * Mathf.Sin(angle), u.x * Mathf.Sin(angle) + u.y * Mathf.Cos(angle));
                        break;
                    }
                    if (Intersect(point, u, vertices[k][i], vertices[k][i + 1])) {
                        cpt++;
                    }
                }
                // Last edge
                if (!badSituation) {
                    Vector2 last = vertices[k][vertices[k].Count - 1];
                    if (PointsAreAligned(point, point + u, last, vertices[k][0])) {
                        // Change u and restart
                        badSituation = true;
                        u = new Vector2(u.x * Mathf.Cos(angle) - u.y * Mathf.Sin(angle), u.x * Mathf.Sin(angle) + u.y * Mathf.Cos(angle));
                        break;
                    }
                    if (Intersect(point, u, last, vertices[k][0])) {
                        cpt++;
                    }
                }
            } while (badSituation);

            if ((cpt & 1) == 1) { // true if cpt is odd
                //Debug.Log(string.Format("Area contains {0} ({1} intersections)", point, cpt));
                return true;
            }
            else {
                //Debug.Log(string.Format("Area doesn't contain {0} ({1} intersections)", point, cpt));
            }
        }
        //Debug.Log(string.Format("Area doesn't contain {0}", point));
        return false;
    }

    private bool Intersect(Vector2 point, Vector2 u, Vector2 v1, Vector2 v2) {
        Vector2 v = (v2 - v1).normalized;
        //Debug.Log(string.Format("point {0} with {1},{2}: u = {3}, v = {4}", point, v1, v2, u, v));
        if (u != Vector2.zero && v != Vector2.zero && Vector2.Angle(u,v) % 180 == 0) { return false; } // Parallel
        float t;
        if (v.x == 0) {
            t = (v1.x - point.x) / u.x;
        }
        else {
            float D = u.y - v.y * u.x / v.x;
            float B = v1.y - point.y + (v1.x - point.x) * v.y / v.x;
            t = B / D;
        }
        //Debug.Log(string.Format("point {0} with {1},{2}: t = {3}", point, v1, v2, t));
        if (t > 0) {
            Vector2 intersect = point + u * t;


            if (OrderIs(v1, intersect, v2) && intersect != v2) {
                return true;
            }
        }
        return false;

    }

    #region Touching Edges

    public static List<Vector2[]> TouchingEdges(RectInt rect1, RectInt rect2) {
        return TouchingEdges(RectToList(rect1), RectToList(rect2));
    }

    private static List<Vector2[]> TouchingEdges(List<Vector2> area1, List<Vector2> area2) {
        List<Vector2[]> list = new List<Vector2[]>();
        int cpt = 0;
        Vector2 a, b, c, d;
        for (int i = 0; i < area1.Count - 1; i++) {
            a = area1[i];
            b = area1[i + 1];
            for (int j = 0; j < area2.Count - 1; j++) {
                c = area2[j];
                d = area2[j + 1];
                if (PointsAreAligned(a, b, c, d) && b != d && a != c && (OrderIs(a, d, b) || OrderIs(d, a, c))) {
                    list.Add(new Vector2[] { a, b, c, d });
                    cpt++;
                }
            }
            // last edge of area2
            c = area2[area2.Count - 1];
            d = area2[0];
            if (PointsAreAligned(a, b, c, d) && b != d && a != c && (OrderIs(a, d, b) || OrderIs(d, a, c))) {
                list.Add(new Vector2[] { a, b, c, d });
                cpt++;
            }
        }
        // last edge of area1
        a = area1[area1.Count - 1];
        b = area1[0];
        for (int j = 0; j < area2.Count - 1; j++) {
            c = area2[j];
            d = area2[j + 1];
            if (PointsAreAligned(a, b, c, d) && b != d && a != c && (OrderIs(a, d, b) || OrderIs(d, a, c))) {
                list.Add(new Vector2[] { a, b, c, d });
                cpt++;
            }
        }
        // last edge of area2
        c = area2[area2.Count - 1];
        d = area2[0];
        if (PointsAreAligned(a, b, c, d) && b != d && a != c && (OrderIs(a, d, b) || OrderIs(d, a, c))) {
            list.Add(new Vector2[] { a, b, c, d });
            cpt++;
        }

        return list;
    }

    private static List<int[]> TouchingEdgesIndex(List<Vector2> area1, List<Vector2> area2) {
        List<int[]> list = new List<int[]>();
        int cpt = 0;
        Vector2 a, b, c, d;
        for (int i = 0; i < area1.Count - 1; i++) {
            a = area1[i];
            b = area1[i + 1];
            for (int j = 0; j < area2.Count - 1; j++) {
                c = area2[j];
                d = area2[j + 1];
                if (PointsAreAligned(a, b, c, d) && b != d && a != c && (OrderIs(a, d, b) || OrderIs(d, a, c))) {
                    list.Add(new int[] { i, i+1, j, j+1 });
                    cpt++;
                }
            }
            // last edge of area2
            c = area2[area2.Count - 1];
            d = area2[0];
            if (PointsAreAligned(a, b, c, d) && b != d && a != c && (OrderIs(a, d, b) || OrderIs(d, a, c))) {
                list.Add(new int[] { i, i+1, area2.Count-1, 0 });
                cpt++;
            }
        }
        // last edge of area1
        a = area1[area1.Count - 1];
        b = area1[0];
        for (int j = 0; j < area2.Count - 1; j++) {
            c = area2[j];
            d = area2[j + 1];
            if (PointsAreAligned(a, b, c, d) && b != d && a != c && (OrderIs(a, d, b) || OrderIs(d, a, c))) {
                list.Add(new int[] { area1.Count-1, 0, j, j+1 });
                cpt++;
            }
        }
        // last edge of area2
        c = area2[area2.Count - 1];
        d = area2[0];
        if (PointsAreAligned(a, b, c, d) && b != d && a != c && (OrderIs(a, d, b) || OrderIs(d, a, c))) {
            list.Add(new int[] { area1.Count - 1, 0, area2.Count-1, 0 });
            cpt++;
        }

        bool continius = true;
        for (int i = 0; i < list.Count - 1; i++) {
            if (list[i][1] != list[i + 1][0]) {
                continius = false;
                break;
            }
        }

        return continius ? list : new List<int[]>();
    }

    #endregion

    #region Usefull Geometry

    private static bool PointsAreAligned(Vector2 a, Vector2 b, Vector2 c, Vector2 d) {
        Vector2 ab = (b - a).normalized;
        Vector2 ac = (c - a).normalized;
        Vector2 ad = (d - a).normalized;

        //bool ret = (ab == ac || ab == -ac) && (ab == ad || ab == -ad);
        bool ret = Vector2.Angle(ab, ac) == 0 && Vector2.Angle(ab, ad) == 0;
        //Debug.Log(string.Format("{0}, {1}, {2}, {3} {4} aligned ({5}  {6})", a, b, c, d, ret ? "are" : "are not", Vector2.Angle(ab, ac), Vector2.Angle(ab, ad)));
        return ret;
    }

    private static bool PointsAreAligned(Vector2 a, Vector2 b, Vector2 c) {
        Vector2 ab = (b - a).normalized;
        Vector2 ac = (c - a).normalized;

        //Debug.Log(string.Format("points {0},{1},{2} have angle {3}", a, b, c, Vector2.Angle(ab, ac)));
        return Vector2.Angle(ab, ac) % 180 == 0;
    }

    private static bool OrderIs(Vector2 a, Vector2 b, Vector2 c) {
        bool ret = ((a.x <= b.x && b.x <= c.x) || (a.x >= b.x && b.x >= c.x)) && 
            ((a.y <= b.y && b.y <= c.y) || (a.y >= b.y && b.y >= c.y));
        //Debug.Log(string.Format("Order {0}, {1}, {2} is {3}", a, b, c, ret));
        return ret;
    }

    #endregion

    #region Add

    public void Add(RectInt rect) {
        Add(RectToList(rect));
    }

    public void Add(List<Vector2> area) {
        step++;
        if (vertices.Count == 0) {
            //Debug.Log(string.Format("Step {0}: first area", step));
            vertices.Add(area);
        }
        else {
            // How many edges are touching?
            List<int[]> touchingEdges;
            int idx = -1;
            do {
                idx++;
                touchingEdges = TouchingEdgesIndex(vertices[idx], area);
            } while (touchingEdges.Count == 0 && idx < vertices.Count - 1);
            //Debug.Log(string.Format("Step {0}: {1} touching edges with surface {2}/{3}", step, touchingEdges.Count, idx + 1, vertices.Count));

            switch (touchingEdges.Count) {
                case 0:
                    vertices.Add(area);
                    break;
                default:
                    Add(idx, area, touchingEdges);
                    break;
            }
        }
        //Debug.Log("Step " + step + ": " + (vertices.Count - 1) + " secondary area(s)");
    }

    private void Add(int addTo, List<Vector2> area, List<Vector2[]> touchingEdges) {
        int[] mainIndices = new int[1 + touchingEdges.Count];
        int[] areaIndices = new int[1 + touchingEdges.Count];
        mainIndices[0] = vertices[0].IndexOf(touchingEdges[0][0]);
        areaIndices[0] = area.IndexOf(touchingEdges[0][3]);
        for (int i = 0; i < touchingEdges.Count; i++) {
            mainIndices[i + 1] = vertices[0].IndexOf(touchingEdges[i][1]);
            areaIndices[i + 1] = area.IndexOf(touchingEdges[i][2]);
        }

        Add(addTo, area, mainIndices, areaIndices);
    }

    private void Add(int addTo, List<Vector2> area, List<int[]> touchingEdges) {
        int[] mainIndices = new int[1 + touchingEdges.Count];
        int[] areaIndices = new int[1 + touchingEdges.Count];
        mainIndices[0] = touchingEdges[0][0];
        areaIndices[0] = touchingEdges[0][3];
        for (int i = 0; i < touchingEdges.Count; i++) {
            mainIndices[i + 1] = touchingEdges[i][1];
            areaIndices[i + 1] = touchingEdges[i][2];
        }

        Add(addTo, area, mainIndices, areaIndices);
    }

    private void Add(int addTo, List<Vector2> area, int[] mainIndices, int[] areaIndices) {

        // Debug mainIndices
        string s = mainIndices[0].ToString();
        for (int i = 1; i < mainIndices.Length; i++) {
            s += " -> " + mainIndices[i];
        }
        //Debug.Log(string.Format("Step {0}: mainIndices: {1} ({2} points)", step, s, vertices[addTo].Count));

        // Add points to the main area

        List<Vector2> list = new List<Vector2>();
        // First point
        if (area[areaIndices[0]] == vertices[addTo][mainIndices[0]]) {
            vertices[addTo].RemoveAt(mainIndices[0]);
            for (int i = 0; i < mainIndices.Length; i++) {
                if (mainIndices[i] >= mainIndices[0]) {
                    mainIndices[i]--;
                }
            }
            //Debug.Log(string.Format("Step {0}: Delete first (idx: {1}, pos: {2})", step, areaIndices[0], area[areaIndices[0]]));
        }
        else {
            list.Add(area[areaIndices[0]]);
            //Debug.Log(string.Format("Step {0}: Add first (idx: {1}, pos: {2})", step, areaIndices[0], area[areaIndices[0]]));
        }
        // Intermediate points
        for (int i = areaIndices[0] + 1; i < area.Count + areaIndices[0] - areaIndices.Length + 1; i++) {
            list.Add(area[i % area.Count]);
            //Debug.Log(string.Format("Step {0}: Add intermediate (idx: {1}, pos: {2})", step, i % area.Count, area[i % area.Count]));
        }
        // Last point (if different from the first one)
        if (areaIndices[areaIndices.Length - 1] != areaIndices[0]) {
            if (area[areaIndices[areaIndices.Length - 1]] == vertices[addTo][mainIndices[mainIndices.Length - 1]]) {
                vertices[addTo].RemoveAt(mainIndices[mainIndices.Length - 1]);
                for (int i = 0; i < mainIndices.Length; i++) {
                    if (mainIndices[i] >= mainIndices[mainIndices.Length - 1]) {
                        mainIndices[i]--;
                    }
                }
                //Debug.Log(string.Format("Step {0}: Delete last (idx: {1}, pos: {2})", step, areaIndices[areaIndices.Length - 1], area[areaIndices[areaIndices.Length - 1]]));
            }
            else {
                list.Add(area[areaIndices[areaIndices.Length - 1]]);
                //Debug.Log(string.Format("Step {0}: Add last (idx: {1}, pos: {2})", step, areaIndices[areaIndices.Length - 1], area[areaIndices[areaIndices.Length - 1]]));
            }
        }
        // Points between last and first
        for (int i = 1; i < areaIndices.Length - 1; i++) {
            if (area[areaIndices[i]] == vertices[addTo][mainIndices[i]]) {
                vertices[addTo].RemoveAt(mainIndices[i]);
                for (int j = 0; j < mainIndices.Length; j++) {
                    if (mainIndices[j] >= mainIndices[i]) {
                        mainIndices[j]--;
                    }
                }
                //Debug.Log(string.Format("Step {0}: Delete other (idx: {1}, pos: {2})", step, areaIndices[i], area[areaIndices[i]]));
            }
            else {
                Debug.LogWarning("encountered a point between last and first that do not match it's corresponding point in the main area");
            }
        }
        //Debug.Log(string.Format("Step {0}: inserting {1} points at index {2}/{3}", step, list.Count, mainIndices[0] + 1 /*% vertices[addTo].Count*/, vertices[addTo].Count));
        vertices[addTo].InsertRange((mainIndices[0] + 1)/* % vertices[addTo].Count*/, list);

        // Try to add secondary areas to the main area;
        if (vertices.Count > 1) {
            List<List<Vector2>> tmp = vertices.GetRange(1, vertices.Count - 1);
            vertices.RemoveRange(1, vertices.Count - 1);
            for (int i = 0; i < tmp.Count; i++) {
                Add(tmp[i]);
            }
        }
    }
    
    #endregion
}
