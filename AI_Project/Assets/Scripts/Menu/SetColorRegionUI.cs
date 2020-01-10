using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetColorRegionUI : MonoBehaviour
{
    public Text regionName;
    public Image color;
    public Text value;
    public Button up;
    public Button down;
    public Button del;

    public void setName(string n)
    {
        regionName.text = n;
    }

    public void setColor(Color n)
    {
        color.color = new Color32((byte)n.r, (byte)n.g, (byte)n.b, (byte)255);
    }

    public void setValue(float v)
    {
        value.text = "Value : " + v;
    }

}
