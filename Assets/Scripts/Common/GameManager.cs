using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager Instance;

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
    }

    public void RegisterPersistentObject (GameObject obj)
    {
        DontDestroyOnLoad (obj);
    }

    public void SavePlayerData()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
        }
    }

    public void LoadPlayerData()
    {

    }

    public void ChangeScene(string sceneName)
    {
        SavePlayerData();
        SceneManager.LoadScene(sceneName);
    }

    void OnSceneLoaded (Scene scene)
    {
        LoadPlayerData();
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
