using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class ButtonMenu : MonoBehaviour
{
    //pass data to generate map
    private GameObject setUp;
    private DatasFiles dataFiles;

    [Header("Special Panel")]
    public GameObject toolTipPanel;
    public GameObject alertPanel;
    public GameObject pipelinePanel;

    [Header("dropdown")]
    public Dropdown mapChoice;
    public Dropdown buldingChoice;
    public Dropdown itemChoice;
    public Dropdown NPCChoice;
    public Dropdown NPCAIChoice;
    public Dropdown questChoice;
   
    [Header("Selected data")]
    public MapParam selectedMap;//the map param that will be pass
    public SetOfBulding selectedBulding;
    public SetOfItem selectedItem;
    public SetOfNPC selectedNPC;
    public SetOfQuest selectedQuest;

    [Header("Prefab")]
    public GameObject terrainTypeUIPrefab;
    public GameObject NPCBtnPrefab;

    private GameObject customColorHolder;
    private GameObject colorPreview;
    public TerrainType nexTerrainType;//the next region
    public NPCParam nextNPC;

    [Header("default list data")]
    public List<MapParam> defaultMap;
    public List<SetOfBulding> defaultSetBulding = new List<SetOfBulding>();
    public List<SetOfItem> defaultSetItem = new List<SetOfItem>();
    public List<SetOfNPC> defaultSetNPC = new List<SetOfNPC>();
    public List<SetOfQuest> defaultSetQuest = new List<SetOfQuest>();

    [Header("list panels")]
    public List<RuntimeAnimatorController> defaultNPCAI;
    public List<GameObject> panels = new List<GameObject>();
    private List<MapParam> mapParams = new List<MapParam>();
    private List<SetOfBulding> buldingParams = new List<SetOfBulding>();
    private List<SetOfItem> itemsParams = new List<SetOfItem>();
    private List<SetOfNPC> NPCParams = new List<SetOfNPC>();
    private List<SetOfQuest> questParams = new List<SetOfQuest>();    

    public void Awake()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "datasFile.json");
        if (!File.Exists(filePath))
        {
            Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, "Maps"));
            Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, "Buldings"));
            Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, "Items"));
            Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, "NPCs"));
            Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, "Quests"));
            dataFiles.mapDatas = new List<string>();
            dataFiles.buldingDatas = new List<string>();
            dataFiles.itemDatas = new List<string>();
            dataFiles.NPCDatas = new List<string>();
            dataFiles.scenarioDatas = new List<string>();

            foreach (MapParam item in defaultMap)
            {
                string mapFile = Path.Combine(Application.persistentDataPath, "Maps", item.mapName + ".json");
                string mapJSON = JsonUtility.ToJson(item);
                File.WriteAllText(mapFile, mapJSON);
                dataFiles.mapDatas.Add(item.mapName);
            }
            foreach (SetOfBulding item in defaultSetBulding)
            {
                string file = Path.Combine(Application.persistentDataPath, "Buldings", item.setName + ".json");
                string json = JsonUtility.ToJson(item);
                File.WriteAllText(file, json);
                dataFiles.buldingDatas.Add(item.setName);
            }
            foreach (SetOfItem item in defaultSetItem)
            {
                string file = Path.Combine(Application.persistentDataPath, "Items", item.setName + ".json");
                string json = JsonUtility.ToJson(item);
                File.WriteAllText(file, json);
                dataFiles.itemDatas.Add(item.setName);
            }
            foreach (SetOfNPC item in defaultSetNPC)
            {
                
                string file = Path.Combine(Application.persistentDataPath, "NPCs", item.setName + ".json");
                string json = JsonUtility.ToJson(item);
                File.WriteAllText(file, json);
                dataFiles.NPCDatas.Add(item.setName);
            }
            foreach (SetOfQuest item in defaultSetQuest)
            {
                string file = Path.Combine(Application.persistentDataPath, "Quests", item.setName + ".json");
                string json = JsonUtility.ToJson(item);
                File.WriteAllText(file, json);
                dataFiles.scenarioDatas.Add(item.setName);
            }

            string saveDataFiles = JsonUtility.ToJson(dataFiles);
            File.WriteAllText(filePath, saveDataFiles);
            #if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
            #endif

        }
        else
        {
            string dataAsJson = File.ReadAllText(filePath);
            dataFiles = JsonUtility.FromJson<DatasFiles>(dataAsJson);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        setUp = GameObject.Find("SetUp");
        ActiveOnePanel(panels[0]);
        ActivePipelinePanel(false);
    }
    public void ActiveOnePanel(GameObject panel)
    {
        foreach (GameObject p in panels)
        {
            p.SetActive(p == panel);
        }
    }

    #region DropDownBtn

    public void SetMapChoice()
    {
        mapChoice.ClearOptions();
        mapParams.Clear();
        foreach (string map in dataFiles.mapDatas)
        {
            string filePath = Path.Combine(Application.persistentDataPath, "Maps", map);
            if (!File.Exists(filePath))
            {
                string dataAsJson = File.ReadAllText(filePath+".json");
                MapParam tmp = new MapParam();
                JsonUtility.FromJsonOverwrite(dataAsJson, tmp);
                mapParams.Add(tmp);
            }
        }
        mapChoice.AddOptions(dataFiles.mapDatas);
    }

    public void SetDropDownMap(int n)
    {
        selectedMap = mapParams[n];
    }

    public void SetBuldingChoice()
    {
        buldingChoice.ClearOptions();
        buldingParams.Clear();
        foreach (string bul in dataFiles.buldingDatas)
        {
            string filePath = Path.Combine(Application.persistentDataPath, "Buldings", bul);
            if (!File.Exists(filePath))
            {
                string dataAsJson = File.ReadAllText(filePath + ".json");
                SetOfBulding tmp = new SetOfBulding();
                JsonUtility.FromJsonOverwrite(dataAsJson, tmp);
                buldingParams.Add(tmp);
            }
        }
        buldingChoice.AddOptions(dataFiles.buldingDatas);
    }

    public void SetDropDownBulding(int n)
    {
        Debug.Log("ici");
        selectedBulding = buldingParams[n];
    }

    public void SetItemChoice()
    {
        itemChoice.ClearOptions();
        itemsParams.Clear();
        foreach (string bul in dataFiles.itemDatas)
        {
            string filePath = Path.Combine(Application.persistentDataPath, "Items", bul);
            if (!File.Exists(filePath))
            {
                string dataAsJson = File.ReadAllText(filePath + ".json");
                SetOfItem tmp = new SetOfItem();
                JsonUtility.FromJsonOverwrite(dataAsJson, tmp);
                itemsParams.Add(tmp);
            }
        }
        itemChoice.AddOptions(dataFiles.itemDatas);
    }

    public void SetDropDownItem(int n)
    {
        selectedItem = itemsParams[n];
    }

    public void SetNPCChoice()
    {
        NPCChoice.ClearOptions();
        NPCParams.Clear();
        Debug.Log(dataFiles.NPCDatas);
        foreach (string bul in dataFiles.NPCDatas)
        {
            string filePath = Path.Combine(Application.persistentDataPath, "NPCs", bul);
            if (!File.Exists(filePath))
            {
                string dataAsJson = File.ReadAllText(filePath + ".json");
                SetOfNPC tmp = new SetOfNPC();
                JsonUtility.FromJsonOverwrite(dataAsJson, tmp);
                NPCParams.Add(tmp);
            }
        }
        NPCChoice.AddOptions(dataFiles.NPCDatas);
    }

    public void SetDropDownNPC(int n)
    {
        selectedNPC = NPCParams[n];
    }

    public void SetQuestsChoice()
    {
        questChoice.ClearOptions();
        questParams.Clear();
        foreach (string q in dataFiles.scenarioDatas)
        {
            string filePath = Path.Combine(Application.persistentDataPath, "Quests", q);
            if (!File.Exists(filePath))
            {
                string dataAsJson = File.ReadAllText(filePath + ".json");
                SetOfQuest tmp = new SetOfQuest();
                JsonUtility.FromJsonOverwrite(dataAsJson, tmp);
                questParams.Add(tmp);
            }
        }
        questChoice.AddOptions(dataFiles.scenarioDatas);
    }

    public void SetDropDownQuest(int n)
    {
        selectedQuest = questParams[n];
    }

    #endregion

    public void ActivePipelinePanel(bool b)
    {
        pipelinePanel.SetActive(b);
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
        setUp.GetComponent<SetUp>().mapParam = selectedMap;

        SceneManager.LoadScene("MapGeneration");
    }

    private void HideAll()
    {
        foreach (GameObject item in panels)
        {
            item.SetActive(false);
        }
    }

    #endregion

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
        selectedMap.mapName = s;
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
            b.onClick.AddListener(() => ActiveOnePanel(panels[2]));
        }
        else
        {
            ActiveOnePanel(panels[7]);
            string save = JsonUtility.ToJson(selectedMap);
            string path = Path.Combine(Application.persistentDataPath, "Maps",selectedMap.name + ".json");
            File.WriteAllText(path, save);

            dataFiles.mapDatas.Add(selectedMap.mapName);
            File.WriteAllText(Path.Combine(Application.persistentDataPath, "datasFile.json"), JsonUtility.ToJson(dataFiles));
            #if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
            #endif
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
        Color32 tmp = colorPreview.GetComponent<Image>().color;
        colorPreview.GetComponent<Image>().color = new Color32(tmp.r, (byte)v, tmp.b, tmp.a);
    }

    public void SetBColor(string s)
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
        Color32 tmp = colorPreview.GetComponent<Image>().color;
        colorPreview.GetComponent<Image>().color = new Color32(tmp.r, tmp.g, (byte)v, tmp.a);
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

        if (nexTerrainType.colour == null || colorPreview == null)
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
            b.onClick.AddListener(() => ActiveOnePanel(panels[3]));
        }
        else
        {
            Color32 tmpColor = colorPreview.GetComponent<Image>().color;
            GameObject tmp = Instantiate(terrainTypeUIPrefab) as GameObject;
            SetColorRegionUI tmpUI = tmp.GetComponent<SetColorRegionUI>();
            tmpUI.setName(nexTerrainType.name);
            tmpUI.setValue(nexTerrainType.height);
            tmpUI.setColor(tmpColor);
            tmp.transform.SetParent(customColorHolder.transform);
            nexTerrainType.colour = new Color32(tmpColor.r, tmpColor.g, tmpColor.b, (byte)255);
            selectedMap.regions.Add(nexTerrainType);
        }

        Debug.Log(
            "Name : " + nexTerrainType.name +
            "\nColor : " + nexTerrainType.colour.r + " " + nexTerrainType.colour.g + " " + nexTerrainType.colour.b +
            "\nValue : " + nexTerrainType.height
        );
    }

    #endregion

    #region CUSTOM NPC

    public void CustomNPCButton()
    {
        selectedNPC = new SetOfNPC();
        selectedNPC.setName = "";
        selectedNPC.NPCs = new List<NPCParam>();
        nextNPC = new NPCParam();
        NPCAIChoice.ClearOptions();
        List<string> tmp = new List<string>();
        foreach (RuntimeAnimatorController item in defaultNPCAI)
        {
            tmp.Add(item.name);
        }
        NPCAIChoice.AddOptions(tmp);
        GameObject tmpGO = GameObject.Find("AllNPCAIHolder");
        for (int a = tmpGO.transform.childCount-1; a>=0; a--)
        {
            Destroy(tmpGO.transform.GetChild(a));
        }
    }

    public void SetSetNameNPC(string s)
    {
        selectedNPC.setName = s;
    }

    public void NameNPC(string s)
    {
        if (nextNPC == null)
            nextNPC = new NPCParam();
        nextNPC.NPCName = s;
    }

    public void NumberNPC(string s)
    {
        if (nextNPC == null)
            nextNPC = new NPCParam();

        int n = -1;
        try
        {
            n = int.Parse(s, System.Globalization.CultureInfo.InvariantCulture);
        }
        catch (System.Exception)
        {
            n = -1;
        }

        nextNPC.numberOf = n;
    }

    public void DifficultyNPC(float n)
    {
        if (nextNPC == null)
            nextNPC = new NPCParam();
        nextNPC.difficulty = Mathf.RoundToInt(n);
        GameObject.Find("TextDifficultyNPCSlider").GetComponent<Text>().text = n.ToString();
    }

    public void SetDropDownNPCAI(int n)
    {
        nextNPC.behaviorGraph = defaultNPCAI[n];
    }

    public void AddNPC(GameObject content)
    {
        bool error = false;
        string alertMsg = "Incomplete selection : ";

        if (nextNPC.NPCName.Length == 0)
        {
            error = true;
            alertMsg += "\n\t name invalid";
        }

        if (nextNPC.numberOf <= 1)
        {
            error = true;
            alertMsg += "\n\t NPC number";
        }

        if (nextNPC.difficulty <= 0 || nextNPC.difficulty > 6)
        {
            error = true;
            alertMsg += "\n\t NPC difficulty";
        }

        if (nextNPC.behaviorGraph == null)
        {
            error = true;
            alertMsg += "\n\t NPC AI";
        }

        if (error)
        {
            alertPanel.SetActive(true);
            alertPanel.GetComponentInChildren<Text>().text = alertMsg;
            Button b = alertPanel.GetComponentInChildren<Button>();
            b.onClick.RemoveAllListeners();
            b.onClick.AddListener(() => ActiveOnePanel(panels[6]));
        }
        else
        {
            selectedNPC.NPCs.Add(nextNPC);
            GameObject tmpPrefab = Instantiate(NPCBtnPrefab);
            tmpPrefab.transform.SetParent(content.transform);
            tmpPrefab.GetComponent<Button>().onClick.RemoveAllListeners();
            tmpPrefab.GetComponent<Button>().onClick.AddListener(() => showNPCDesc(nextNPC));
            tmpPrefab.GetComponentInChildren<Text>().text = nextNPC.NPCName;
        }
    }

    public void showNPCDesc(NPCParam nPC)
    {
        GameObject.Find("NameNPCDesc").GetComponent<Text>().text = nPC.NPCName;
        GameObject.Find("numNPCDesc").GetComponent<Text>().text = nPC.numberOf.ToString();
        GameObject.Find("diffNPCDesc").GetComponent<Text>().text = nPC.difficulty.ToString();
        GameObject.Find("AiNPCDesc").GetComponent<Text>().text = nPC.behaviorGraph.name;
    }

    public void AcceptCustomNPC()
    {
        bool error = false;
        string alertMsg = "Incomplete selection : ";

        if (selectedNPC.setName.Length == 0)
        {
            error = true;
            alertMsg += "\n\t name invalid";
        }

        if (selectedNPC.NPCs.Count <= 0)
        {
            error = true;
            alertMsg += "\n\t List NPC empty";
        }

        if (error)
        {
            alertPanel.SetActive(true);
            alertPanel.GetComponentInChildren<Text>().text = alertMsg;
            Button b = alertPanel.GetComponentInChildren<Button>();
            b.onClick.RemoveAllListeners();
            b.onClick.AddListener(() => ActiveOnePanel(panels[6]));
        }
        else
        {
            ActiveOnePanel(panels[10]);
            string save = JsonUtility.ToJson(selectedNPC);
            string path = Path.Combine(Application.persistentDataPath, "NPCs", selectedNPC.setName + ".json");
            File.WriteAllText(path, save);

            dataFiles.NPCDatas.Add(selectedNPC.setName);
            File.WriteAllText(Path.Combine(Application.persistentDataPath, "datasFile.json"), JsonUtility.ToJson(dataFiles));
            #if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
            #endif
        }

        Debug.Log(
            "Name : " + nextNPC.NPCName +
            "\nsize : " + nextNPC.numberOf
        );
    }

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

[SerializeField]
struct DatasFiles
{
    public List<string> mapDatas;
    public List<string> buldingDatas;
    public List<string> itemDatas;
    public List<string> NPCDatas;
    public List<string> scenarioDatas;
}
