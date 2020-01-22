using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NPCParam
{
    public string NPCName;
    public int numberOf;
    public int difficulty;
    public RuntimeAnimatorController behaviorGraph;


}

[System.Serializable]
public class SetOfNPC
{
    public string setName;
    public List<NPCParam> NPCs;
}
