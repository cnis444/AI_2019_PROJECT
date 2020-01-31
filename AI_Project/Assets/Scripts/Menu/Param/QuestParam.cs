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
    public bool onStart;

    public int MaxOrder()
    {
        int toret = 0;
        foreach (Mission item in missions)
        {
            if (item.order > toret)
                toret = item.order;
        }

        return toret;
    }


}

[System.Serializable]
public class SetOfQuest
{
    public string setName;
    public List<QuestParam> quests;

    public bool OnStart(int id)
    {
        foreach (QuestParam item in quests)
        {
            if (item.next.Contains(id))
                return false;
        }
        return true;
    }

}