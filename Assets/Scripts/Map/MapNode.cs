using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class MapNode
{
    public int level;
    public int order;
    public List<MapNode> Children;
    public string Type;
    public bool beGenerated = false;
    public float posX = 0;
    public float posY = 0;

    public MapNode(int currentLevel, string type)
    {
        level = currentLevel;
        Type = type;
        Children = new List<MapNode>();
    }

}

public class MapNodeList
{
    public List<MapNode> nodes;
    public MapNodeList()
    {
        nodes = new List<MapNode>();
    }
}
