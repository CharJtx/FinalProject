using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class BattleTimer : MonoBehaviour
{
    public EnemySpawner enemySpawner;
    public GUISkin guiskin;

    public bool showWindow = false;
    //private GUIStyle textBoxStyle;
    private GUIStyle timerStyle; 


    // Start is called before the first frame update
    void Start()
    {
        enemySpawner = GetComponent<EnemySpawner>();
    }

    private void OnGUI()
    {

        


        if (enemySpawner != null)
        {
            {
                if (showWindow)
                {
                    GUI.skin = guiskin;
                    timerStyle = new GUIStyle();
                    //textBoxStyle = new GUIStyle();
                    timerStyle.alignment = TextAnchor.MiddleCenter;
                    timerStyle.wordWrap = true;
                    timerStyle.padding = new RectOffset(0, 0, 0, 0);  // È¥³ý padding
                    timerStyle.contentOffset = Vector2.zero;


                    float currentTime = enemySpawner.countdownTime;

                    string timerText = string.Format("{0:00}:{1:00}", Mathf.FloorToInt(currentTime / 60), Mathf.FloorToInt(currentTime % 60));

                    // Calculated display position
                    float screenWidth = Screen.width;
                    float screenHeight = Screen.height;
                    Rect timerRect = new Rect(screenWidth / 2 - 100, screenHeight / 5, 200, 130);
                    //Rect boxRect = new Rect(screenWidth / 2 - 100, screenHeight / 4, 200, 50);

                    //GUI.Box(new Rect(10, 50, 120, 130), "Box title");
                    GUI.Box(timerRect, timerText);
                    // Display timer
                    //GUI.Label(timerRect, timerText);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
