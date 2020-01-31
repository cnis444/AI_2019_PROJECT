using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCUI : MonoBehaviour
{

    public Text InfoText;
    public GameObject starSprite;
    public GameObject starHolder;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetInfo(string name, string role)
    {
        InfoText.text = name + "\n" + role;
    }

    public void SetStarLevel(int n)
    {
        foreach (Transform child in starHolder.GetComponentsInChildren<Transform>())
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < n; i++)
        {
            GameObject tmp = Instantiate(starSprite);
            tmp.transform.SetParent(starHolder.transform);
        }
    }
}
