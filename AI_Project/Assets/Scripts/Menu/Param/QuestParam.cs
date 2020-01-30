using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class QuestParam 
{
    public string questName;
    public int id;
    public string description;
    public List<Mission> missions;
    public List<int> next;

}

[System.Serializable]
public class SetOfQuest
{
    public string setName;
    public List<QuestParam> quests;
}