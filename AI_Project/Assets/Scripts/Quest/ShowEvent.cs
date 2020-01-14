using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowEvent : MonoBehaviour
{

    public GameObject eventPanel;
    public Text text;
    private float delay;

    public void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void Start()
    {
        ShowInfo("GLHF", 2);
    }

    public void Update()
    {
        if(delay > 0)
        {
            delay -= Time.deltaTime;
            if(delay <= 0)
            {
                eventPanel.SetActive(false);
            }
        }
    }

    public void ShowInfo(string txt, float s)
    {
        if (delay >= 0)
            text.text += "\n" + txt;
        else
            text.text = txt;
        delay = s;
        eventPanel.SetActive(true);
    }


}
