using System;

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapGenerator: MonoBehaviour
{
    public int levels = 5;
    public int nowLevel = 0;
    public int MinNodesPerLevel = 2;
    public int MaxNodesPerLevel = 4;
    public static MapGenerator Instance = null;

    public Canvas canvas;
    public GameObject mapPanel;
    public Button battleButton;
    public Button spaceStationButton;
    public Button treasureButton;
    public Button startButton;
    public Button endButton;
    public int seed = 0;
    public GameObject arrow;

    
    
    private List<MapNodeList> allNodes = new List<MapNodeList>();
    private List<string> nodeTypes = new List<string> { "Battle", "SpaceStation","Treasure"};
    private MapNode startNode;
    private MapNode endNode;
    private RectTransform contentRectTransform;
    private RectTransform scrollRectTransform;

    private float scrollWidth = 0;


    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        if (seed == 0)
        {
            seed = Environment.TickCount;
        }

        UnityEngine.Random.InitState(seed);
        //Transform contentTransform = MyTools.FindChildByName(mapPanel.transform, "MapContent");

        if (mapPanel != null)
        {
            mapPanel.SetActive(false);
        }

        // find the Map Scroll
        scrollRectTransform = mapPanel.transform.GetChild(0).GetComponent<RectTransform>();

        if (scrollRectTransform != null)
        {
            float width = Screen.width;
            float height = Screen.height;

            scrollRectTransform.sizeDelta = new Vector2(0.8f * width, 0.8f * height);
        }
        else
        {
            Debug.LogError("scroll not found!");
        }
        Transform contentTransform = scrollRectTransform.transform.GetChild(0).transform.GetChild(0).transform;

        if (contentTransform != null)
        {
            contentRectTransform = contentTransform.GetComponent<RectTransform>();
            Debug.Log("Content found: " + contentRectTransform.name);
        }
        else
        {
            Debug.LogError("Content not found!");
        }
        

        GenerateMap();

        contentRectTransform.sizeDelta = new Vector2(scrollRectTransform.sizeDelta.x, (allNodes.Count + 2) * 200);

        VisualizeMap();
        
    }

    void GenerateMap()
    {
       
        scrollWidth = scrollRectTransform.sizeDelta.x;
        
        float[] spaces = new float[MaxNodesPerLevel];
        float ySpace = 200;

        

        // calculate the space between each button 
        for (int i = 0; i < spaces.Length; i++)
        {
            spaces[i] = scrollWidth / (i + 1);
        }

        // which type of node will be generated into the map
        for (int i = 0; i < levels; i++)
        {
            MapNodeList currentLevelNodes = new MapNodeList();
            int nodesNumInLevel = UnityEngine.Random.Range(MinNodesPerLevel, MaxNodesPerLevel);
            for (int j = 0; j < nodesNumInLevel; j++)
            {
                string typeOfNode = nodeTypes[UnityEngine.Random.Range(0, nodeTypes.Count)];
                MapNode currentNode = new MapNode(i, typeOfNode);
                currentNode.order = j;
                currentLevelNodes.nodes.Add(currentNode);
                
            }

            allNodes.Add(currentLevelNodes);
        }

        startNode = new MapNode(-1, "Start");
        startNode.posX = spaces[1];
        startNode.posY = -100;

        for (int i = 0; i < allNodes[0].nodes.Count; i++)
        {
            startNode.Children.Add(allNodes[0].nodes[i]);
        }

        endNode = new MapNode(levels, "End");
        endNode.order = 0;
        endNode.posX = spaces[1];
        endNode.posY = -(ySpace * (levels + 2) + 100);

        MapNodeList endNodeList = new MapNodeList();
        endNodeList.nodes.Add(endNode);
        allNodes.Add(endNodeList);

        // generate the path

        for (int i = 0; i < levels; i++)
        {
            int nodesNumInThisLevel = allNodes[i].nodes.Count;
            for (int j = 0; j < nodesNumInThisLevel; j++)
            {
                if (i < levels)
                {
                    GenerateMapTree(allNodes[i].nodes[j], allNodes[i + 1], 0);
                }
                // calculate posX
                allNodes[i].nodes[j].posX = (j + 1) * spaces[nodesNumInThisLevel];
                allNodes[i].nodes[j].posY = -(ySpace * (i + 1) + 100 + UnityEngine.Random.Range(-50,50));

            }
        }

        
        //Debug.Log(startNode);

    }

    void GenerateMapTree(MapNode currentNode, MapNodeList nextLevelNodes,int currentLevel)
    {
        if (currentLevel >= allNodes.Count - 1 || currentNode.beGenerated)
        {
                return;
        }
        else
        {
            int[] randomPath = MyTools.RandomlyGenerateArray(1, nextLevelNodes.nodes.Count);

            for (int i = 0; i < randomPath.Length; i++)
            {
                MapNode nextNode = nextLevelNodes.nodes[randomPath[i]];
                currentNode.Children.Add(nextNode);
            }

            currentNode.beGenerated = true;
            return;
        }
    }

    void VisualizeMap()
    {

        CreateButton(startNode);

        for (int i = 0; i < allNodes.Count; i++)
        {
            for (int j = 0; j < allNodes[i].nodes.Count; j++)
            {

                CreateButton(allNodes[i].nodes[j]);

            }


        }
        

    }

    void CreateButton(MapNode mapNode)
    {
        float posX = mapNode.posX;
        float posY = mapNode.posY;

        Button newButtonType = null;
        switch (mapNode.Type)
        {
            case "Battle":
                newButtonType = battleButton;
                break;
            case "SpaceStation":
                newButtonType = spaceStationButton;
                break;
            case "Treasure":
                newButtonType = treasureButton;
                break;
            case "Start":
                newButtonType = startButton;
                break;
            case "End":
                newButtonType = endButton;
                break;
            default:
                Debug.Log("Unknown node type: " + mapNode.Type);
                break;
        }

        if (newButtonType != null)
        {
            Button newButton = Instantiate(newButtonType, contentRectTransform);
            if (mapNode.level != nowLevel)
            {
                newButton.interactable = false;
            }


            newButton.name = "ButtonX" + mapNode.order + "Y" + mapNode.level;
            // get the bntton recttransform
            RectTransform buttonRectTransform = newButton.GetComponent<RectTransform>();

            // set button anchor, pivot and position
            buttonRectTransform.anchorMin = new Vector2(0, 1);
            buttonRectTransform.anchorMax = new Vector2(0, 1);
            buttonRectTransform.pivot = new Vector2(0.5f, 0.5f);

            buttonRectTransform.anchoredPosition = new Vector2(posX, posY);

            // calculate the arrow direction and width
            for (int k = 0; k < mapNode.Children.Count; k++)
            {
                int nextLevel = mapNode.level + 1;

                float endPosX = allNodes[nextLevel].nodes[mapNode.Children[k].order].posX;
                float endPosY = allNodes[nextLevel].nodes[mapNode.Children[k].order].posY;

                GameObject newArrow = Instantiate(arrow, contentRectTransform);

                Vector2 startPosition = new Vector2(posX, posY);
                Vector2 endPosition = new Vector2(endPosX, endPosY);

                Vector2 direction = endPosition - startPosition;
                float distance = direction.magnitude;

                RectTransform arrowRectTransform = newArrow.GetComponent<RectTransform>();
                arrowRectTransform.anchorMin = new Vector2(0, 1);
                arrowRectTransform.anchorMax = new Vector2(0, 1);
                arrowRectTransform.pivot = new Vector2(0.5f, 0.5f);
                Vector2 arrowposition = 0.5f * startPosition + 0.5f * endPosition;

                arrowRectTransform.anchoredPosition = arrowposition;

                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                arrowRectTransform.rotation = Quaternion.Euler(0, 0, angle);

                arrowRectTransform.sizeDelta = new Vector2(distance - 50, 20);
            }

            GameManager gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
            string scenceType = mapNode.Type;
            int nodeLevel = mapNode.level;
            int nodeOrder = mapNode.order;

            newButton.onClick.AddListener(() => ScenceButtonClick(scenceType, nodeLevel, nodeOrder));
        }

    }


    void ScenceButtonClick(string scenceType,int level, int nodeOrder)
    {
        GameManager.Instance.ChangeScene(scenceType);

        if (level >= 0)
        {
            for (int i = 0; i < allNodes[level].nodes.Count; i++)
            {
                Transform levelButton = contentRectTransform.gameObject.transform.Find("ButtonX" + i + "Y" + level);
                if (levelButton != null)
                {
                    levelButton.gameObject.GetComponent<Button>().interactable = false;
                }
            }

            for (int i = 0; i < allNodes[level].nodes[nodeOrder].Children.Count; i++)
            {
                int nextLevel = allNodes[level].nodes[nodeOrder].Children[i].level;
                int nextOrder = allNodes[level].nodes[nodeOrder].Children[i].order;
                Transform nextButton = contentRectTransform.gameObject.transform.Find("ButtonX" + nextOrder + "Y" + nextLevel);
                if (nextButton != null)
                {
                    nextButton.gameObject.GetComponent<Button>().interactable = true;
                }
            }
            nowLevel += 1;
            Debug.Log(nowLevel);
            mapPanel.SetActive(false);
            Time.timeScale = 1f;
        }

        
    }

    public void ShowPanel()
    {

        mapPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.M))
        {
            mapPanel.SetActive(true);
            Time.timeScale = 0f;
        }
        else if (Input.GetKey(KeyCode.N))
        {
            mapPanel.SetActive(false);
            Time.timeScale = 1f;
        }
    }
}
