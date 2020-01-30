using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class BuldingParam 
{
    public string buldingName;
    public int width;
    public int length;
    public int height;
    public float windowProbability;
    public int minPartitionSize;
    public int maxPartitionSize;
    public float volumeReduction;

}

[System.Serializable]
public class SetOfBulding
{
    public string setName;
    public List<BuldingParam> buldings;
}