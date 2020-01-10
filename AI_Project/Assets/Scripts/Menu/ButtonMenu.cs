using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonMenu : MonoBehaviour
{
    //pass data to generate map
    private GameObject setUp;

    //all the panels
    public List<GameObject> panels = new List<GameObject>();
    public GameObject toolTipPanel;
    public GameObject alertPanel;

    //to select the map
    public Dropdown mapChoice;
    private List<MapParam> mapParams = new List<MapParam>();
    public MapParam selectedMap;//the map param that will be pass
    private bool customMap = false;//use the custom param

    private GameObject customColorHolder;
    private GameObject colorPreview;
    public TerrainType nexTerrainType;//the next region

    //Prefab
    public GameObject terrainTypeUIPrefab;


    // Start is called before the first frame update
    void Start()
    {
        setUp = GameObject.Find("SetUp");
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
        mapParams.Clear();
        MapParam[] allMapParams = Resources.LoadAll<MapParam>("");
        List<string> optionNames = new List<string>();
        foreach (MapParam item in allMapParams)
        {
            optionNames.Add(item.mapName);
            mapParams.Add(item);
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
        if(mapParams.Count == 0)
        {
            SetMapChoice();
        }
        setUp.GetComponent<SetUp>().mapParam = customMap ? selectedMap : mapParams[0];

        SceneManager.LoadScene("Ced");
    }

    private void HideAll()
    {
        foreach (GameObject item in panels)
        {
            item.SetActive(false);
        }
    }

    #endregion

    #region EDIT BUTTON

    public void SetDropDownMap(int n)
    {
        selectedMap = mapParams[n];
    }

    #region CUSTOM MAP

    public void CustomButton()
    {
        selectedMap = ScriptableObject.CreateInstance(typeof(MapParam)) as MapParam;
        selectedMap.name = "";
        selectedMap.octave = -1;
        selectedMap.persistance = -1f;
        selectedMap.lacunarity = -1f;
        selectedMap.seed = -1;
        selectedMap.chunks = -1;
        selectedMap.highCoeff = -1;
        selectedMap.regions = new List<TerrainType>();
    }

    public void NameMap(string s)
    {
        selectedMap.name = s;
    }

    public void OctaveMap(float n)
    {
        selectedMap.octave = Mathf.RoundToInt(n);
        GameObject.Find("TextOctaveSlider").GetComponent<Text>().text = n.ToString();
    }

    public void PersistanceMap(float n)
    {
        selectedMap.persistance = Mathf.Round(n * 100)/100;
        GameObject.Find("TextPersistanceSlider").GetComponent<Text>().text = (Mathf.Round(n * 100) / 100).ToString();
    }

    public void LacunarityMap(float n)
    {
        selectedMap.lacunarity = Mathf.Round(n * 100) / 100;
        GameObject.Find("TextLacunaritySlider").GetComponent<Text>().text = (Mathf.Round(n * 100) / 100).ToString();
    }

    public void SeedMap(string s)
    {
        int n = -1;
        try
        {
            n = int.Parse(s, System.Globalization.CultureInfo.InvariantCulture);
        }
        catch (System.Exception)
        {
            n = -1;
        }

        selectedMap.seed = n;
    }

    public void ChunksMap(string s)
    {
        int n = -1;
        try
        {
            n = int.Parse(s, System.Globalization.CultureInfo.InvariantCulture);
        }
        catch (System.Exception)
        {
            n = -1;
        }

        selectedMap.chunks = n;
    }

    public void HeighCoeffMap(string s)
    {
        int n = -1;
        try
        {
            n = int.Parse(s, System.Globalization.CultureInfo.InvariantCulture);
        }
        catch (System.Exception)
        {
            n = -1;
        }

        selectedMap.highCoeff = n;
    }

    public void GoToCustomColor(GameObject customColorH)
    {
        customColorHolder = customColorH;
        for (int i = customColorH.transform.childCount-1 ; i >=0 ; i--)
        {
            Destroy(customColorH.transform.GetChild(i).gameObject);
        }

        foreach (TerrainType item in selectedMap.regions)
        {
            GameObject tmp = Instantiate(terrainTypeUIPrefab) as GameObject;
            SetColorRegionUI tmpUI = tmp.GetComponent<SetColorRegionUI>();
            tmpUI.setName(item.name);
            tmpUI.setValue(item.height);
            tmpUI.setColor(item.colour);
            tmp.transform.SetParent(customColorH.transform);
        }

        nexTerrainType = new TerrainType();
        nexTerrainType.name = "";
        nexTerrainType.height = -1;
    }

    public void AcceptCustomMap()
    {
        bool error = false;
        string alertMsg = "Incomplete selection : ";

        if(selectedMap.name.Length == 0)
        {
            error = true;
            alertMsg += "\n\t name invalid";
        }

        if (selectedMap.octave <= 0 || selectedMap.octave >  8)
        {
            error = true;
            alertMsg += "\n\t octave invalid";
        }

        if (selectedMap.persistance <= 0 || selectedMap.persistance >= 1)
        {
            error = true;
            alertMsg += "\n\t persistance invalid";
        }

        if (selectedMap.lacunarity <= 1 || selectedMap.lacunarity > 4)
        {
            error = true;
            alertMsg += "\n\t lacunarity invalid";
        }

        if (selectedMap.seed <= 0 || selectedMap.seed > 100000)
        {
            error = true;
            alertMsg += "\n\t seed invalid";
        }

        if (selectedMap.chunks <= 0 || selectedMap.chunks > 20)
        {
            error = true;
            alertMsg += "\n\t chunk invalid";
        }

        if (selectedMap.highCoeff <= 1 || selectedMap.highCoeff > 300)
        {
            error = true;
            alertMsg += "\n\t Mesh heigh invalid";
        }

        if (selectedMap.regions.Count <= 0)
        {
            error = true;
            alertMsg += "\n\t color invalid";
        }

        if (error)
        {
            alertPanel.SetActive(true);
            alertPanel.GetComponentInChildren<Text>().text = alertMsg;
            Button b = alertPanel.GetComponentInChildren<Button>();
            b.onClick.RemoveAllListeners();
            b.onClick.AddListener(() => ActiveOnePanel(panels[3]));
        }
        else
        {
            customMap = true;
            ActiveOnePanel(panels[2]);
        }

        Debug.Log(
            "Name : " + selectedMap.name +
            "\nOctave : " + selectedMap.octave +
            "\nPersistance : " + selectedMap.persistance +
            "\nLacunarity : " + selectedMap.lacunarity +
            "\nSeed : " + selectedMap.seed +
            "\nChunks : " + selectedMap.chunks +
            "\nHigh Coeff : " + selectedMap.highCoeff +
            "\nRegions : " + selectedMap.regions.Count
        );
    }

    #endregion

    #region CUSTOM COLOR

    public void SetNameColor(string s)
    {
        nexTerrainType.name = s;
    }

    public void SetValueColor(string s)
    {
        float v = -1;
        try
        {
            v = float.Parse(s, System.Globalization.CultureInfo.InvariantCulture);
        }
        catch (System.Exception)
        {
            v = -1;
        }
        nexTerrainType.height = v;
    }

    public void GiveColorImagePreview(GameObject g)
    {
        colorPreview = g;
    }

    public void SetRColor(string s)
    {
        int v = 0;
        try
        {
            v = int.Parse(s, System.Globalization.CultureInfo.InvariantCulture);
        }
        catch (System.Exception)
        {
            v = 0;
        }
        nexTerrainType.colour.r = (byte)v;
        Color32 tmp = colorPreview.GetComponent<Image>().color;
        colorPreview.GetComponent<Image>().color = new Color32((byte)v, tmp.g, tmp.b, tmp.a);
    }

    public void SetGColor(string s)
    {
        int v = 0;
        try
        {
            v = int.Parse(s, System.Globalization.CultureInfo.InvariantCulture);
        }
        catch (System.Exception)
        {
            v = 0;
        }
        nexTerrainType.colour.g = (byte)v;
        Color32 tmp = colorPreview.GetComponent<Image>().color;
        colorPreview.GetComponent<Image>().color = new Color32(tmp.r, (byte)v, tmp.b, tmp.a);
    }

    public void AddNewColorREgion()
    {
        bool error = false;
        string alertMsg = "Incomplete selection : ";
        if (nexTerrainType.name.Length == 0)
        {
            error = true;
            alertMsg += "\n\t name invalid";
        }

        if (nexTerrainType.colour == null)
        {
            error = true;
            alertMsg += "\n\t color invalid";
        }

        if (nexTerrainType.height < 0 || nexTerrainType.height > 1)
        {
            error = true;
            alertMsg += "\n\t value invalid";
        }

        if (error)
        {
            alertPanel.SetActive(true);
            alertPanel.GetComponentInChildren<Text>().text = alertMsg;
            Button b = alertPanel.GetComponentInChildren<Button>();
            b.onClick.RemoveAllListeners();
            b.onClick.AddListener(() => ActiveOnePanel(panels[4]));
        }
        else
        {
            selectedMap.regions.Add(nexTerrainType);
            GameObject tmp = Instantiate(terrainTypeUIPrefab) as GameObject;
            SetColorRegionUI tmpUI = tmp.GetComponent<SetColorRegionUI>();
            tmpUI.setName(nexTerrainType.name);
            tmpUI.setValue(nexTerrainType.height);
            tmpUI.setColor(nexTerrainType.colour);
            tmp.transform.SetParent(customColorHolder.transform);
        }

        Debug.Log(
            "Name : " + nexTerrainType.name +
            "\nColor : " + nexTerrainType.colour.r + " " + nexTerrainType.colour.g + " " + nexTerrainType.colour.b +
            "\nValue : " + nexTerrainType.height
        );
    }

    #endregion



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
