using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestDestroyer : MonoBehaviour
{

    public string key;
    public int n;
    QuestManager tmp;

    public void OnDestroy()
    {
        GameObject qm = GameObject.Find("QuestManager");
        if (qm != null)
        {
            tmp = qm.GetComponent<QuestManager>();
        }
        if(tmp != null)
            tmp.FinishQuest(key, n);
    }
}
