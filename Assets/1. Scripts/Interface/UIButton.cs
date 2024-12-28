using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIButton : MonoBehaviour
{
    public void ExitGame()
    {
        GameManager.GM.ExitGame();
    }

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
        UIManager.UI.OptionPanel.panel.gameObject.SetActive(true);
    }

    /// <summary>
    /// 게임재개 버튼
    /// </summary>
    public void ResumeGame()
    {
        GameManager.GM.ResumeGame();
        UIManager.UI.OptionPanel.panel.gameObject.SetActive(false);
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