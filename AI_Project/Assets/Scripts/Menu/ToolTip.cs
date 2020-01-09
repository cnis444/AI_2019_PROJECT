using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ToolTip : MonoBehaviour, IPointerClickHandler
{
    public GameObject tooltipPanel;
    public Text tooltipText;
    public string txt;


    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            tooltipPanel.SetActive(true);
            tooltipText.text = txt;
        }

    }


}
