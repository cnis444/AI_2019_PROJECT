using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetUp : MonoBehaviour
{

    public MapParam mapParam;
    public SetOfBulding setOfBulding;
    public SetOfItem setOfItem;
    public SetOfNPC setOfNPC;
    public SetOfQuest setOfQuest;
    Solver sol; 
    

    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
        sol = new Solver(new List<ConstraintObject>());
    }

    public void CreateAllConstraint()
    {
        sol.objects.Clear();


        //foreach (NPCParam item in setOfNPC.NPCs)
        //{
        //    ConstraintObject tmpC = new ConstraintObject();
        //    tmpC.maxEntity = item.numberOf;
        //    tmpC.Constraints = new List<Constraint>();
        //}

        
        GameObject tmpPlay = GameObject.Find("PlayButton");
        if (tmpPlay != null)
        {
            tmpPlay.transform.parent.GetComponentInChildren<LoadBar>().enabled = false;
            GameObject tmpSlider = tmpPlay.transform.parent.GetComponentInChildren<LoadBar>().gameObject;
            tmpSlider.GetComponent<Slider>().value = tmpSlider.GetComponent<Slider>().maxValue;
            tmpPlay.GetComponent<Button>().interactable = true;
        }

        QuestManager tmpQuestManager = GameObject.Find("QuestManager").GetComponent<QuestManager>();
        tmpQuestManager.questsActive = new List<Quest>();
        tmpQuestManager.questsFinish = new List<Quest>();
        tmpQuestManager.questsInactive = new List<Quest>();
        foreach (QuestParam item in setOfQuest.quests)
        {
            Quest tmpQuest = new Quest();
            tmpQuest.questName = item.questName;
            tmpQuest.id = item.id;
            tmpQuest.questDescription = item.description;
            tmpQuest.levelOder = 0;
            tmpQuest.maxOrder = item.MaxOrder();
            tmpQuest.missions = item.missions;
            tmpQuest.next = item.next;
            tmpQuest.finish = false;
            if (setOfQuest.OnStart(item.id))
                tmpQuestManager.questsActive.Add(tmpQuest);
            else
                tmpQuestManager.questsInactive.Add(tmpQuest);
        }

    }




}
