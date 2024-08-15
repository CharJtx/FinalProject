using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResumePanel : MonoBehaviour
{
    public Button resumeButton;
    public Button pauseButton;
    public GameObject pausePanel;

    // Start is called before the first frame update
    void Start()
    {
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }
        else
        {
            Debug.Log("Can not find Pause Panel");
        }

        pauseButton.onClick.AddListener(showPausePanel);
        resumeButton.onClick.AddListener(hidePausePanel);

    }

    void showPausePanel ()
    {
        Time.timeScale = 0f;

        if (pausePanel != null)
        {
            pausePanel.SetActive(true);
        }
    }

    void hidePausePanel()
    {
        Time.timeScale = 1f;
        if (pausePanel != null)
            pausePanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
