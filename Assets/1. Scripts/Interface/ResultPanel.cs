using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;

public class ResultPanel : MonoBehaviour
{
    public GameObject panel;

    public TMP_Text stageNameTxt;
    public TMP_Text clearedResultTxt;

    public TMP_Text highScorePointTxt;
    public TMP_Text currentScorePointTxt;

    public AudioSource audioSource;
    public AudioClip[] clearClips;
    public AudioClip[] failedClips;
    public bool isCleared;

    public int highScore;
    public int currentScore;

    private void Start()
    {
        UIManager.UI.ResultPanel = this;

        highScore = StageManager.SM.highScore;
        highScorePointTxt.text = "" + highScore;
        panel.SetActive(false);
    }

    public void EnablePanel()
    {
        panel.SetActive(true);

        if (isCleared)
            // SoundManager.SM.PlayRandomAudio(audioSource, clearClips);
            SoundManager.SM.PlayRandomAudio(clearClips);
        else
            // SoundManager.SM.PlayRandomAudio(audioSource, failedClips);
            SoundManager.SM.PlayRandomAudio(failedClips);

        GameManager.GM.PauseGame();
    }

    public void DisAblePanel()
    {
        panel.SetActive(false);
        GameManager.GM.ResumeGame();
    }

    /// <summary>
    /// 0 : 최고 점수, 1 : 현재 점수
    /// </summary>
    /// <param name="idx"></param>
    /// <param name="score"></param>
    public void SetScoreTxt(int idx, int score)
    {
        switch (idx)
        {
            case 0:
                highScorePointTxt.text = "" + score;
                break;
            case 1:
                currentScorePointTxt.text = "" + score;
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 0 : 스테이지 이름, 1 : 레벨 클리어 텍스트
    /// </summary>
    /// <param name="idx"></param>
    /// <param name="text"></param>
    public void SetText(int idx, string text)
    {
        switch (idx)
        {
            case 0:
                stageNameTxt.text = text;
                break;
            case 1:
                clearedResultTxt.text = text;
                break;

            default:
                Debug.Log("인덱스 범위 초과");
                break;
        }
    }
}
