using System;
using System.Collections.Generic;
using UnityEngine;

public class Solutions
{
    public ConstraintObject type { get; }
    public Vector3 pos { get; }

    public Solutions(ConstraintObject type, float x, float y, float z)
    {
        this.type = type;
        pos = new Vector3(x,y,z);
    }
}
