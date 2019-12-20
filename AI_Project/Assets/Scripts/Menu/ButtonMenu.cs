using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonMenu : MonoBehaviour
{

    public List<GameObject> panels = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        ActiveOnePanel(panels[0]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActiveOnePanel(GameObject panel)
    {
        foreach (GameObject p in panels)
        {
            p.SetActive(p == panel);
        }
    }

    #region START BUTTON

    public void StartB()
    {
        Debug.Log("start WIP");
    }

    #endregion

    #region EDIT BUTTON


    #endregion

    #region OPTION BUTTON


    #endregion 

    #region QUIT BUTTON

    public void Quit()
    {
        Application.Quit();
    }

    #endregion



}
