﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButton : MonoBehaviour
{
    public GameObject[] pannels;
    public void ExitGame()
    {
        GameManager.GM.ExitGame();
    }

    #region 오브젝트 활성화 / 비활성화 관련
    public void EnableObject(int idx)
    {
        pannels[idx].SetActive(true);
    }

    public void DisAbleObject(int idx)
    {
        pannels[idx].SetActive(false);
    }
    #endregion

    #region 씬 관련
    /// <summary>
    /// 씬 재시작 버튼
    /// </summary>
    public void RestartScene()
    {
        GameManager.GM.RestartScene();
    }

    /// <summary>
    /// 씬 불러오는 버튼
    /// </summary>
    /// <param name="sceneName"></param>
    public void LoadScene(string sceneName)
    {
        GameManager.GM.LoadScene(sceneName);
    }

    public void GoToTitle()
    {
        SoundManager.SM.SetBGM(SoundManager.SM.titleBGM);
        GameManager.GM.LoadScene("Title");
    }

    public void StageMove(int sceneIdx)
    {
        GameManager.GM.StageMove(sceneIdx);
    }
    #endregion

    #region 시간 과련
    /// <summary>
    /// 일시정지 버튼
    /// </summary>
    public void PauseGame()
    {
        GameManager.GM.PauseGame();
    }

    /// <summary>
    /// 게임재개 버튼
    /// </summary>
    public void ResumeGame()
    {
        GameManager.GM.ResumeGame();
    }
    #endregion

    #region 소리 관련
    /// <summary>
    /// 볼륨 설정
    /// </summary>
    /// <param name="volumeScale"></param>
    public void SetVolume(float volumeScale)
    {
        SoundManager.SM.SetVolume(volumeScale);
    }
    #endregion
}