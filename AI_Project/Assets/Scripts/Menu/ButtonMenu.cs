using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonMenu : MonoBehaviour
{

    public List<GameObject> panels = new List<GameObject>();
    public GameObject toolTipPanel;
    public Dropdown mapChoice;

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

    public void SetMapChoice()
    {
        mapChoice.ClearOptions();
        Object[] allMapParams = Resources.FindObjectsOfTypeAll(typeof(MapParam));
        List<string> optionNames = new List<string>();
        foreach (Object item in allMapParams)
        {
            optionNames.Add(((MapParam)item).mapName);
        }
        mapChoice.AddOptions(optionNames);
    }

    public void ActiveToolTip(bool b)
    {
        toolTipPanel.SetActive(b);
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
