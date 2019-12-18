using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonMenu : MonoBehaviour
{
    public GameObject menuPanel;
    public GameObject optionPanel;
    public GameObject editPanel;

    // Start is called before the first frame update
    void Start()
    {
        menuPanel.SetActive(true);
        optionPanel.SetActive(false);
        editPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region START BUTTON

    public void StartB()
    {
        Debug.Log("start WIP");
    }

    #endregion

    #region EDIT BUTTON

    public void Edit()
    {
        Debug.Log("edit WIP");
        editPanel.SetActive(true);
        menuPanel.SetActive(false);
        optionPanel.SetActive(false);
    }

    #endregion

    #region OPTION BUTTON

    public void Option()
    {
        Debug.Log("option WIP");
        optionPanel.SetActive(true);
        menuPanel.SetActive(false);
        editPanel.SetActive(false);
       
    }

    #endregion 

    #region QUIT BUTTON

    public void Quit()
    {
        Application.Quit();
    }

    public void Back()
    {
        menuPanel.SetActive(true);
        editPanel.SetActive(false);
        optionPanel.SetActive(false);
    }

    #endregion



}
