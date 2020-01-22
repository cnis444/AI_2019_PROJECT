using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AIParam {

    static int id = 0;
    static string[] names = {"a", "b" };
    static int[] radiusCom = new int[2] { 5, 10 };
    static int levelMax = 5;

    public static int NextId(){ return id++;}

    public static string RandomName() { return names[Random.Range(0, names.Length)]; }

    public static int RadiusCommunication() { return Random.Range(radiusCom[0], radiusCom[1]); }

    public static int StartLevel() { return Random.Range(1, levelMax); }


}

