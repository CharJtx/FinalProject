using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class MapGenerator: MonoBehaviour
{
    public int levels = 5;
    public int nowLevel = 0;
    public int MinNodesPerLevel = 2;
    public int MaxNodesPerLevel = 4;

    public Canvas canvas;
    public GameObject mapPanel;
    public Button battleButton;
    public int seed = 0;
    public GameObject arrow;

    
    
    private List<MapNodeList> allNodes = new List<MapNodeList>();
    private List<string> nodeTypes = new List<string> { "Battle", "SpaceStation","Treasure"};
    private MapNode startNode;
    private RectTransform contentRectTransform;
    private RectTransform scrollRectTransform;

    private float scrollWidth = 0;


    // Start is called before the first frame update
    void Start()
    {
        if (seed == 0)
        {
            seed = Environment.TickCount;
        }

        UnityEngine.Random.InitState(seed);
        //Transform contentTransform = MyTools.FindChildByName(mapPanel.transform, "MapContent");

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
        for (int i = 1; i < levels+1; i++)
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

        startNode = new MapNode(0,"Start");

        // generate the path

        for (int i = 0; i < levels; i++)
        {
            int nodesNumInThisLevel = allNodes[i].nodes.Count;
            for (int j = 0; j < nodesNumInThisLevel; j++)
            {
                if (i < levels-1)
                {
                    GenerateMapTree(allNodes[i].nodes[j], allNodes[i + 1], 0);
                }
                // calculate posX
                allNodes[i].nodes[j].posX = (j + 1) * spaces[nodesNumInThisLevel];
                allNodes[i].nodes[j].posY = -(ySpace * (i + 1) + 100 + UnityEngine.Random.Range(-50,50));

            }
        }

        
        Debug.Log(startNode);

    }

    void GenerateMapTree(MapNode currentNode, MapNodeList nextLevelNodes,int currentLevel)
    {
        if (currentLevel >= allNodes.Count -1 || currentNode.beGenerated)
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

        for (int i = 0; i < allNodes.Count; i++)
        {
            for (int j = 0; j < allNodes[i].nodes.Count; j++)
            {
                float posX = allNodes[i].nodes[j].posX;
                float posY = allNodes[i].nodes[j].posY;

                Button newButton = Instantiate(battleButton, contentRectTransform);

                newButton.name = "ButtonX" + j + "Y" + i;
                // get the bntton recttransform
                RectTransform buttonRectTransform = newButton.GetComponent<RectTransform>();

                // set button anchor, pivot and position
                buttonRectTransform.anchorMin = new Vector2(0, 1);
                buttonRectTransform.anchorMax = new Vector2(0, 1);
                buttonRectTransform.pivot = new Vector2(0.5f, 0.5f);

                buttonRectTransform.anchoredPosition = new Vector2(posX,posY);

                // calculate the arrow direction and width
                for (int k = 0; k < allNodes[i].nodes[j].Children.Count; k++)
                {
                    float endPosX = allNodes[i + 1].nodes[allNodes[i].nodes[j].Children[k].order].posX;
                    float endPosY = allNodes[i + 1].nodes[allNodes[i].nodes[j].Children[k].order].posY;

                    GameObject newArrow = Instantiate(arrow, contentRectTransform);

                    Vector2 startPosition = new Vector2(posX, posY);
                    Vector2 endPosition = new Vector2(endPosX, endPosY);

                    Vector2 direction = endPosition - startPosition;
                    float distance = direction.magnitude;

                    RectTransform arrowRectTransform = newArrow.GetComponent<RectTransform>();
                    arrowRectTransform.anchorMin = new Vector2 (0, 1);
                    arrowRectTransform.anchorMax = new Vector2 (0, 1);
                    arrowRectTransform.pivot = new Vector2(0.5f, 0.5f);
                    Vector2 arrowposition = 0.5f * startPosition + 0.5f * endPosition;

                    arrowRectTransform.anchoredPosition = arrowposition;

                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    arrowRectTransform.rotation = Quaternion.Euler(0, 0, angle);
                }

            }


        }
        

    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
