using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestUIMAnager : MonoBehaviour
{

    public GameObject questPanel;
    public Text questTitle;
    public Text questDsc;
    public GameObject questMissionHandler;
    public GameObject missionPrefabUI;
    public GameObject questCurrentHandler;
    public GameObject questFinishHandler;
    public GameObject prefabButton;

    // Start is called before the first frame update
    void Start()
    {
        Quit();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            questPanel.SetActive(!questPanel.activeSelf);
            SetUpCurrentQuest();
            SetUpFinishQuest();
        }
    }

    public void Quit()
    {
        questPanel.SetActive(false);
    }

    public void SetUpQuestDsc(Quest q)
    {
        questTitle.text = q.questName;
        questDsc.text = q.questDescription;

        for (int i = questMissionHandler.transform.childCount-1; i >=0; i--)
        {
            Destroy(questMissionHandler.transform.GetChild(i).gameObject);
        }

        foreach (Mission m in q.missions)
        {
            if (m.order <= q.levelOder)
            {
                GameObject tmp = Instantiate(missionPrefabUI);
                tmp.GetComponent<Text>().text = "Objectif : " + m.missionName +
                    "\n Description : " + m.missionDescription + " ( " + m.current + " / " + m.max + " " + ObjToStr(m.objectif) + ")";
                tmp.transform.SetParent(questMissionHandler.transform);
            }
        }

    }

    public void SetUpCurrentQuest()
    {
        for (int i = questCurrentHandler.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(questCurrentHandler.transform.GetChild(i).gameObject);
        }
        
        List<Quest> lq = GameObject.Find("QuestManager").GetComponent<QuestManager>().questsActive;
        foreach (Quest q in lq)
        {
            GameObject tmp = Instantiate(prefabButton);
            tmp.GetComponentInChildren<Text>().text = q.questName;
            tmp.GetComponent<Button>().onClick.AddListener(() => SetUpQuestDsc(q));
            tmp.transform.SetParent(questCurrentHandler.transform);
        }
    }


    public void SetUpFinishQuest()
    {
        for (int i = questFinishHandler.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(questFinishHandler.transform.GetChild(i).gameObject);
        }

        List<Quest> lq = GameObject.Find("QuestManager").GetComponent<QuestManager>().questsFinish;
        foreach (Quest q in lq)
        {
            GameObject tmp = Instantiate(prefabButton);
            tmp.GetComponentInChildren<Text>().text = q.questName;
            tmp.GetComponent<Button>().onClick.AddListener(() => SetUpQuestDsc(q));
            tmp.transform.SetParent(questFinishHandler.transform);
        }
    }

    private string ObjToStr(Objectif obj)
    {
        switch (obj)
        {
            case Objectif.DESTROY:
                return "Destroy";
            case Objectif.INTERACT:
                return "Interact";
            case Objectif.KILL:
                return "Kill";
            case Objectif.POSITION:
                return "Position";
            case Objectif.SAVE:
                return "Save";
            case Objectif.TIME:
                return "Time";
            default:
                return "";
        }
    }

}
