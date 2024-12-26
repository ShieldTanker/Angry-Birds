using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;

public class ResultPanel : MonoBehaviour
{
    public TMP_Text stageNameTxt;
    public TMP_Text clearedResultTxt;

    public TMP_Text highScoreTxt;
    public TMP_Text currentScoreTxt;

    public int highScore;
    public int currentScore;

    void Start()
    {
        Debug.Log("Pannel Setted");
        UIManager.UI.ResultPanel = this;

        highScore = StageManager.SM.highScore;
        highScoreTxt.text = "" + highScore;
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
                highScoreTxt.text = "" + score;
                break;
            case 1:
                currentScoreTxt.text = "" + score;
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

            default: Debug.Log("인덱스 범위 초과");
                break;
        }
    }
}
