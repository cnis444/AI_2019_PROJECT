using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class DragPanel : EventTrigger
{
    private bool dragging;
    private float width;
    private float height;

    private void Start()
    {
        width = transform.parent.GetComponent<RectTransform>().sizeDelta.x;
        height = transform.parent.GetComponent<RectTransform>().sizeDelta.y;
    }


    // Update is called once per frame
    void Update()
    {
        if (dragging)
        {
            transform.parent.transform.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y-height/2+10);
        }
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        dragging = true;
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        dragging = false;
    }
}
