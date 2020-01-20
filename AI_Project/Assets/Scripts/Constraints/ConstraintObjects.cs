using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class ConstraintObject : MonoBehaviour
{
    public List<Constraint> Constraints { get; }
    public int maxEntity { get; }

}