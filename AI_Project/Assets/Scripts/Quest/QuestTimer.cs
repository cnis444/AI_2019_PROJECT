using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider))]
public class QuestTimer : MonoBehaviour
{
    public string key; 
    public int n;
    public float delay;
    private bool start = false;
    public GameObject timer;

    public void Start()
    {
        timer.GetComponent<Text>().text = "TIMER : " + delay + " s !!!";
        timer.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (start)
        {
            delay -= Time.deltaTime;
            if(delay <= 0)
            {
                GameObject.Find("QuestManager").GetComponent<QuestManager>().FinishQuest(key, n);
                timer.SetActive(false);
                Destroy(gameObject);
            }
            timer.GetComponent<Text>().text = "TIMER : " +delay.ToString("F2")+ " s !!!";
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if(string.Equals(other.transform.tag, "Player"))
        {
            if (GameObject.Find("QuestManager").GetComponent<QuestManager>().isKeyActive(key))
            {
                timer.SetActive(true);
                start = true;
            }
        }
    }
}
