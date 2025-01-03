﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region 싱글톤
    static GameManager instance;
    public static GameManager GM { get { return instance; } set { instance = value; } }
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(instance);

        DontDestroyOnLoad(gameObject);
    }
    #endregion

    public bool GamePause {  get; private set; }

    public bool resetData;

    private void Start()
    {
        if (resetData)
        {
            PlayerPrefs.DeleteAll();
        }
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    #region 씬 관련
    string sceneName;
    public string SceneName
    {
        get { return sceneName; }
        set { sceneName = value; LoadScene(sceneName); }
    }

    public void LoadScene(string sceneName)
    {
        ResumeGame();
        SceneManager.LoadScene(sceneName);
    }

    public void StageMove(int stageNum)
    {
        ResumeGame();
        SceneManager.LoadScene("Stage_" + stageNum);
    }

    public void RestartScene()
    {
        ResumeGame();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    #endregion

    #region 시간 관련
    public void PauseGame()
    {
        GamePause = true;
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        GamePause = false;
        Time.timeScale = 1;
    }
    #endregion
}
