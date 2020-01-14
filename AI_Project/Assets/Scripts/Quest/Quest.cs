using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest 
{

    public string questName;
    public int id;
    public string questDescription;
    public int levelOder;
    public int maxOrder;
    public List<Mission> missions;
    public List<int> next;
    public bool finish;

    public bool isKeyActive(string key)
    {
        for (int i = 0; i < missions.Count; i++)
        {
            if (string.Equals(missions[i].missionKey, key) && missions[i].order <= levelOder)
                return true;
        }
        return false;
    }

    public void FinishMission(string key, int n)
    {
        for (int i = 0; i < missions.Count; i++)
        {
            if (!missions[i].finish && missions[i].order <= levelOder && string.Equals(missions[i].missionKey, key)) 
            {
                missions[i].AddKey(n);
                if (missions[i].finish)
                {
                    NextLevelOrder();
                }
            }
        }
    }


    public void NextLevelOrder()
    {
        for (int i = 0; i < missions.Count; i++)
        {
            if(missions[i].order <= levelOder && !missions[i].finish)
            {
                return;
            }
        }
        levelOder++;
        if(levelOder > maxOrder)
        {
            finish = true;
            GameObject.Find("Canvas").GetComponent<ShowEvent>().ShowInfo("Quest : " + questName + " END", 5);
            if(next.Count > 0)
                GameObject.Find("Canvas").GetComponent<ShowEvent>().ShowInfo("New Quest are avaible !!\n Press J to consult them.", 5);
        }
    }

    public void Add()
    {
        missions[0].AddKey(1);
    }

}

public enum Objectif
{
    KILL, SAVE, DESTROY, INTERACT, POSITION, TIME
}

[System.Serializable]
public class Mission
{
    public string missionName;
    public string missionDescription;
    public string missionKey;
    public Objectif objectif;
    public int max;
    public int current;
    public int order;
    public bool finish;

    public void AddKey(int n)
    {
        current += n;
        if (current >= max)
        {
            finish = true;
            GameObject.Find("Canvas").GetComponent<ShowEvent>().ShowInfo("Objectif : " + missionName + " END", 5);
            return;
        }
        GameObject.Find("Canvas").GetComponent<ShowEvent>().ShowInfo("Objectif : " + missionName + " UPDATE : " + current +"/"+max, 5);
    }
}
