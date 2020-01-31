using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum WallType { None, Corner, Door, Window, Wall}


public class Wall
{
    public Vector3 start, end;
    public WallType[] types;

    public Wall(Vector3 start, Vector3 end) {
        this.start = start;
        this.end = end;
        types = new WallType[(int)Mathf.Abs(Vector3.Magnitude(end - start))];
        //types[0] = WallType.Corner;
        //types[types.Length - 1] = WallType.Corner;
        for (int i = 0; i < types.Length; i++) {
            types[i] = WallType.Wall;
        }
    }

    public void AddDoor() {
        if (types.Length > 0) {
            int idx = Random.Range(0, types.Length);
            types[idx] = WallType.Door;
        }
        else {
            Debug.LogWarning("Trying to add a door on a wall of size 0");
        }
    }

    public void AddWindows(float probability) {
        for (int i = 0; i < types.Length; i++) {
            if (types[i] == WallType.Wall) {
                if (Random.value < probability) {
                    types[i] = WallType.Window;
                }
            }
        }
    }

    public override string ToString() {
        return string.Format("[{0}, {1} ({2})]", start, end, types.Length);
    }
}
