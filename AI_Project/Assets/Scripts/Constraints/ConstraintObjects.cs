using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ConstraintObject : MonoBehaviour
{
    public List<Constraint> Constraints { get; set; }
    public int maxEntity { get; set; }

}