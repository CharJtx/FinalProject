using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManger : MonoBehaviour
{
    public Button startButton;
    public Button exitButton;
    // Start is called before the first frame update
    void Start()
    {
        startButton.onClick.AddListener(StartGame);
        exitButton.onClick.AddListener(ExitGame);
    }

    void StartGame()
    {
        SceneManager.LoadScene("Start");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
