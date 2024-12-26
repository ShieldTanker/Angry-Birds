using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButton : MonoBehaviour
{
    #region �� ����
    /// <summary>
    /// �� ����� ��ư
    /// </summary>
    public void RestartScene()
    {
        GameManager.GM.RestartScene();
    }

    /// <summary>
    /// �� �ҷ����� ��ư
    /// </summary>
    /// <param name="sceneName"></param>
    public void LoadScene(string sceneName)
    {
        GameManager.GM.LoadScene(sceneName);
    }
    #endregion

    #region �ð� ����
    /// <summary>
    /// �Ͻ����� ��ư
    /// </summary>
    public void PauseGame()
    {
        GameManager.GM.PauseGame();
        UIManager.UI.OptionPanel.gameObject.SetActive(true);
    }

    /// <summary>
    /// �����簳 ��ư
    /// </summary>
    public void ResumeGame()
    {
        GameManager.GM.ResumeGame();
        UIManager.UI.OptionPanel.gameObject.SetActive(false);
    }
    #endregion

    #region �Ҹ� ����
    /// <summary>
    /// ���� ����
    /// </summary>
    /// <param name="volumeScale"></param>
    public void SetVolume(float volumeScale)
    {
        SoundManager.SM.SetVolume(volumeScale);
    }
    #endregion
}