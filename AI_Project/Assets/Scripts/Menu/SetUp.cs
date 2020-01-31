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

        BaseConstraint baseConstraint = new BaseConstraint();
        List<Constraint> baseC = new List<Constraint>();
        baseC.Add(new NumberEntityGreaterStrictConstraint(typeof(BaseConstraint), 2));
        baseC.Add(new NumberEntityLessStrictConstraint(typeof(BaseConstraint), 8));
        baseConstraint.Constraints = baseC;

        List<Constraint> chiefC = new List<Constraint>();
        chiefC.Add(new NumberEntityPerEntityGreaterStrictConstraint(typeof(ChiefConstraint),typeof( GuardConstraint), 2, "Joffrey t nul"));
        chiefC.Add(new NumberEntityPerEntityLessStrictConstraint(typeof(ChiefConstraint), typeof(GuardConstraint), 8, "Joffrey t nul"));

        ChiefConstraint chiefConstraint = new ChiefConstraint();
        chiefConstraint.Constraints = chiefC;
        GuardConstraint guardConstraint = new GuardConstraint();
        guardConstraint.maxEntity = 12;

        sol.objects.Add(baseConstraint);
        sol.objects.Add(chiefConstraint);
        sol.objects.Add(guardConstraint);

        List<Constraint> guide = new List<Constraint>();
        guide.Add(new NumberEntityPerEntityGreaterConstraint(typeof(GuardConstraint), typeof(BaseConstraint), 1, "copy pasta"));
        guide.Add(new EntityRelativeDistanceGreaterStrictConstraint(typeof(BaseConstraint), typeof(BaseConstraint), 50));

        //sol.Solve(guide);



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
