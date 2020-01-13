using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{

    public List<Quest> questsActive;
    public List<Quest> questsInactive;
    public List<Quest> questsFinish;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FinishQuest(string key, int n)
    {
        for (int i = questsActive.Count - 1; i >= 0; i--) 
        {
            Quest tmpQuest = questsActive[i];
            tmpQuest.FinishMission(key, n);
            if (tmpQuest.finish)
            {
                questsActive.RemoveAt(i);
                questsFinish.Add(tmpQuest);
                questsActive.AddRange(GetNextQuest(tmpQuest.next));
            }
        }

        if(questsActive.Count == 0)
        {
            //TODO VICTORY
        }
    }

    public List<Quest> GetNextQuest(List<int> ids)
    {
        List<Quest> toret = new List<Quest>();
        for (int i = questsInactive.Count -1; i >=0; i--)
        {
            Quest tmpQuest = questsInactive[i];
            if (ids.Contains(tmpQuest.id))
            {
                toret.Add(tmpQuest);
                questsInactive.RemoveAt(i);
            }
        }
        return toret;
    }
     

}
