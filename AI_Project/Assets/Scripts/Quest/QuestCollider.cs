using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class QuestCollider : MonoBehaviour
{
    public string key;
    public int n;
    public int nUse = 1;
    public bool destroy;

    public void OnTriggerEnter(Collider other)
    {
        if(string.Equals(other.transform.tag, "Player"))
        {
            if (GameObject.Find("QuestManager").GetComponent<QuestManager>().isKeyActive(key))
            {
                GameObject.Find("QuestManager").GetComponent<QuestManager>().FinishQuest(key, n);
                nUse--;
                if (nUse <= 0 && destroy)
                {
                    Destroy(gameObject);
                }
                else if(nUse <= 0)
                {
                    Destroy(this);
                }
            }

        }
    }
}
