using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    Transform playerTransform;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        //GameOver(false);
    }

    public void RegisterPersistentObject(GameObject obj)
    {
        DontDestroyOnLoad(obj);
    }

    public void SavePlayerData()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    public void LoadPlayerData()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.transform.position = new Vector3(0,1.52f,0);
        }
    }

    public void ChangeScene(string sceneName)
    {
        //SavePlayerData();
        SceneManager.LoadScene(sceneName);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (MapGenerator.Instance != null) MapGenerator.Instance.HidePanel();
        LoadPlayerData();
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void StartShieldReactivationCoroutine(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }

    public void GameOver(bool flag)
    {

        Time.timeScale = 0f;

        Transform canvasTransform = GameObject.FindGameObjectWithTag("Canvas").transform;
        canvasTransform.gameObject.SetActive(true);
        if (canvasTransform != null)
        {
            Transform endingPanel = MyTools.FindChildByName(canvasTransform.transform, "EndingPanel");
            Transform backText = MyTools.FindChildByName(canvasTransform.transform, "ReturnText");
            Transform backText2 = MyTools.FindChildByName(canvasTransform.transform, "ReturnText2");
            Transform backButton = MyTools.FindChildByName(canvasTransform.transform, "ReturnButton");

            if (endingPanel != null)
            {
                endingPanel.gameObject.SetActive(true);
            }

            if (backText != null && backText2 != null)
            {
                if (flag)
                {
                    backText.gameObject.SetActive(false);
                    backText2.gameObject.SetActive(true);
                }
                else
                {
                    backText.gameObject.SetActive(true);
                    backText2.gameObject.SetActive(false);
                }
            }

            backButton.GetComponent<Button>().onClick.AddListener(() => { SceneManager.LoadScene("MainMenu"); Time.timeScale = 1f; endingPanel.gameObject.SetActive(false); });
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
