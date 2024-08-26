using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Newtonsoft.Json;
using TMPro;


public class UpgradePanelManager : MonoBehaviour
{
    //private List<Button> upgradeButtons = new List<Button>();
    private int upgradeButtonCount = 3;
    private GameObject player;
    private List<UpgradeOption> upgradeOptions = new List<UpgradeOption>();
    private List<UpgradeOption> selectedUpgradOptions = new List<UpgradeOption>();
    private List<int> valueList = new List<int>();

    private Rect windowRect;
    public GUISkin guiskin;
    private bool showWindow = false;
    private int screenHight;
    private int screenWidth;
    private int panelSpacing = 20;
    private float panelWidth;
    private float panelHeight;
    private GUIStyle textBoxStyle;
    public static UpgradePanelManager instance;


    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        //upgradePanel.SetActive(false);
        screenHight = Screen.height;
        screenWidth = Screen.width;
        Debug.Log(screenHight);
        Debug.Log(screenWidth);
        screenHight = 500;
        screenWidth = 800;
        panelWidth = screenWidth - 2 * panelSpacing;
        panelHeight = screenHight - 2 * panelSpacing;

        windowRect = new Rect(panelSpacing, panelSpacing, panelWidth, panelHeight);
        LoadUpgradeOptions();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnGUI()
    {
        textBoxStyle = new GUIStyle(GUI.skin.box);
        textBoxStyle.alignment = TextAnchor.MiddleCenter;
        textBoxStyle.wordWrap = true;
        GUI.skin = guiskin;
        if (showWindow)
        {
            windowRect = GUI.Window(0, windowRect, DoMyWindow, "UPDATE OPTIONS");
        }
    }

    void DoMyWindow(int windowID)
    {
        for (int i = 0; i < upgradeButtonCount; i++)
        {
            UpgradeOption option = selectedUpgradOptions[i];
            int randomValue = valueList[i];
            string buttonDescription = option.description.Replace("{Num}", randomValue.ToString());
            string buttonText = option.name;
            float spacing = 20;

            float Rank = panelWidth / upgradeButtonCount;
            float buttonWidth = Rank - 3*spacing;
            float buttonHeight = 30;

            float buttonY = screenHight - buttonHeight - 2* spacing -panelSpacing - 10;
            float buttonX = panelSpacing + spacing + Rank * i;

            float textBoxHeight = screenHight - 3*spacing - 2 * panelSpacing - buttonHeight - 20;
            float textBoxWidth = buttonWidth;

            float textY = panelSpacing + spacing;
            float textX = buttonX;

            
            Rect textBoxRect = new Rect(textX, textY, textBoxWidth, textBoxHeight);
            GUI.TextArea(textBoxRect, buttonDescription, textBoxStyle);

            if (GUI.Button(new Rect(buttonX, buttonY, buttonWidth, buttonHeight), buttonText))
            {
                OnUpgradeButtonClicked(option, randomValue);
            }

            
            
        }
    }
    void LoadUpgradeOptions()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "upgradeOptions").Replace("\\", "/");
        Debug.Log(filePath);
        if (!File.Exists(filePath))
        {
            Debug.Log("Can not find upgrade path");
            return;
        }
        else
        {
            string upgradeData = File.ReadAllText(filePath);
            UpgradeOptionsList loadedData = JsonConvert.DeserializeObject<UpgradeOptionsList>(upgradeData);
            upgradeOptions = loadedData.upgrades;
        }
    }

    public bool ShowUpgradeOptions()
    {
        selectedUpgradOptions.Clear();
        valueList.Clear();

        System.Random random = new System.Random();

        for (int i = 0; i < upgradeButtonCount; i++)
        {
            UpgradeOption option = upgradeOptions[Random.Range(0, upgradeOptions.Count)];
            selectedUpgradOptions.Add(option);

            int randomValue = 0;
            if (option.type == "value")
            {
                randomValue = Random.Range(option.min, option.max);
            }
            else if (option.type == "percentage")
            {
                randomValue = Random.Range(option.min, option.max);
            }

            valueList.Add(randomValue);
        }

        Time.timeScale = 0;
        showWindow = true;

        return true;
    }

    void OnUpgradeButtonClicked(UpgradeOption option, int value)
    {
        performUpgradeMethod(option, value);
        showWindow = false;
        //upgradePanel.SetActive(false);

    }

    void performUpgradeMethod(UpgradeOption option, int value)
    {
        string componentName = option.branch;
        string methodName = option.method;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Component[] components = MyTools.GetComponentByName(player, componentName);
        MyTools.CallMethod(components, methodName, value);
        Time.timeScale = 1;
    }



}