using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region �̱���
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

    #region �� ����
    string sceneName;
    public string SceneName
    {
        get { return sceneName; }
        set { sceneName = value; LoadScene(sceneName); }
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    #endregion

    #region �ð� ����
    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
    }
    #endregion

    #region �Ҹ� ����
    public float volumeScale;

    /// <summary>
    /// ������ ����
    /// </summary>
    public void SetVolumeScale(float volumeScale)
    {
        PlayerPrefs.SetFloat("volumeScale", volumeScale);
        this.volumeScale = volumeScale;
    }

    /// <summary>
    /// ������ �ҷ�����
    /// </summary>
    public void LoadVolumeScale()
    {
        volumeScale = PlayerPrefs.GetFloat("volumeScale", 1);
    }
    #endregion
}
