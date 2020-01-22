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

    public void setColor(Color32 n)
    {
        color.color = new Color32(n.r, n.g, n.b, (byte)255);
    }

    public void setValue(float v)
    {
        value.text = "Value : " + v;
    }

    public void Del()
    {
        GameObject tmpCanvas = GameObject.Find("Canvas");
        List<TerrainType> tmpL = tmpCanvas.GetComponent<ButtonMenu>().selectedMap.regions;

        TerrainType toDel = new TerrainType();
        float.TryParse(value.text.Split(':')[1], out toDel.height);
        toDel.name = regionName.text;
        toDel.colour = color.color;

        for (int i = 0; i < tmpL.Count; i++)
        {
            if(SameTerrainType(toDel, tmpL[i]))
            {
                tmpL.RemoveAt(i);
                Destroy(gameObject);
                return;
            }
        }
    }

    public void Up()
    {
        GameObject tmpCanvas = GameObject.Find("Canvas");
        List<TerrainType> tmpL = tmpCanvas.GetComponent<ButtonMenu>().selectedMap.regions;

        TerrainType toSwap = new TerrainType();
        float.TryParse(value.text.Split(':')[1], out toSwap.height);
        toSwap.name = regionName.text;
        toSwap.colour = color.color;

        int index = 0;

        for (int i = 0; i < tmpL.Count; i++)
        {
            if (SameTerrainType(toSwap, tmpL[i]))
            {
                index = i;
            }
        }

        TerrainType tmpSwap = tmpL[index];
        tmpL[index] = tmpL[Mathf.Max(0, index - 1)];
        tmpL[Mathf.Max(0, index - 1)] = tmpSwap;
        tmpCanvas.GetComponent<ButtonMenu>().GoToCustomColor(transform.parent.gameObject);
    }


    public void Down()
    {
        GameObject tmpCanvas = GameObject.Find("Canvas");
        List<TerrainType> tmpL = tmpCanvas.GetComponent<ButtonMenu>().selectedMap.regions;

        TerrainType toSwap = new TerrainType();
        float.TryParse(value.text.Split(':')[1], out toSwap.height);
        toSwap.name = regionName.text;
        toSwap.colour = color.color;

        int index = 0;

        for (int i = 0; i < tmpL.Count; i++)
        {
            if (SameTerrainType(toSwap, tmpL[i]))
            {
                index = i;
            }
        }

        TerrainType tmpSwap = tmpL[index];
        tmpL[index] = tmpL[Mathf.Min(tmpL.Count-1, index + 1)];
        tmpL[Mathf.Min(tmpL.Count - 1, index + 1)] = tmpSwap;
        tmpCanvas.GetComponent<ButtonMenu>().GoToCustomColor(transform.parent.gameObject);
    }

    private bool SameTerrainType(TerrainType a, TerrainType b)
    {
        return string.Equals(a.name, b.name) && a.height == b.height && a.colour.r == b.colour.r && a.colour.g == b.colour.g && a.colour.b == b.colour.b;
    }

}
