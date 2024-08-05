using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using Newtonsoft.Json;


public class UpgradePanelManager : MonoBehaviour
{
    public GameObject upgradePanel;
    public Button buttonPrefab;
    private List<Button> upgradeButtons = new List<Button>();
    private int upgradeButtonCount = 3;
    private GameObject player;
    private List<UpgradeOption> upgradeOptions;


    // Start is called before the first frame update
    void Start()
    {
        upgradePanel.SetActive(false);
        LoadUpgradeOptions();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void LoadUpgradeOptions()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, "upgradeOptions").Replace("\\","/");
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

    public void ShowUpgradeOptions()
    {
        foreach (Button button in upgradeButtons)
        {
            Destroy(button.gameObject);
        }
        upgradeButtons.Clear();

        Time.timeScale = 0;

        string[] upgradeDescriptionArray = new string[upgradeButtonCount];
        string[] upgradeNameArray = new string[upgradeButtonCount];

        for (int i = 0; i < upgradeButtonCount; i++)
        {
            Button newButton = Instantiate(buttonPrefab, upgradePanel.transform);
            newButton.name = "Button" + i;

            UpgradeOption option = upgradeOptions[Random.Range(0, upgradeOptions.Count)];
            string buttonText = option.name;
            int randomValue = 0;
            if (option.type == "value")
            {
                randomValue = Random.Range(10, 100);
            }
            else if (option.type == "health")
            {
                randomValue = Random.Range(1, 50);
            }
            string buttonDescription = option.description.Replace("{percentage}", randomValue.ToString());

            //newButton.GetComponentInChildren<Text>().text = buttonDescription;
            newButton.onClick.AddListener(() => OnUpgradeButtonClicked(option, randomValue));
            upgradeButtons.Add(newButton);

        }

        upgradePanel.SetActive(true);
    }

    void OnUpgradeButtonClicked(UpgradeOption option, int value)
    {
        performUpgradeMethod(option, value);
        upgradePanel.SetActive(false);
        Time.timeScale = 1;
    }

    void performUpgradeMethod(UpgradeOption option, int value)
    {
        string componentName = option.branch;
        string methodName = option.method;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Component[] components = MyTools.GetComponentByName(player, componentName);
        MyTools.CallMethod(components, methodName, value);

    }

}
