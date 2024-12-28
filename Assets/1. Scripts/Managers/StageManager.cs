using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    #region 싱글톤
    static StageManager instance;
    public static StageManager SM { get { return instance; } set { instance = value; } }
    private void Awake()
    {
        if (instance == null)
            instance = this;

        else
            Destroy(this.gameObject);

        highScore = PlayerPrefs.GetInt(stageName + "highScore", 0);
    }
    #endregion

    #region 점수 관련
    public int highScore;
    public int currentScore;

    [Tooltip("정산 여부")]
    public bool adjustmented;
    public int ignoreIdx = -1;
    #endregion

    [Space]
    public int birdIdxLength;
    public int BirdCount { get; set; }
    public int PigCount;
    public bool CanShot { get; set; }

    [Space]
    public string stageName;

    [Space]
    public bool onTimer;
    public float gameToEndTime;
    private IEnumerator timer;

    #region 카메라 뷰포인트
    [Space]
    public Transform[] viewPoint;
    public float[] viewTimer;
    public float viewLength;
    #endregion

    [Space]
    public AudioSource audioSource;
    public AudioClip stageAudioClip;
    public AudioClip[] startAudios;
    public AudioClip[] adjustAudios;


    #region 기본 함수
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        // 뷰 포인트 순회 하기
        StartCoroutine(circuitViewPoint());

        SoundManager.SM.SetBGM(stageAudioClip);
        SoundManager.SM.PlayRandomAudio(startAudios);
    }
    #endregion


    #region 만든 함수


    #endregion


    #region 코루틴
    public void CheckCount()
    {
        // 타이머가 돌고있으면 리셋하고 다시실행
        if (onTimer)
        {
            StopCoroutine(timer);
            onTimer = false;
        }

        if (BirdCount <= 0 || PigCount <= 0)
        {
            CanShot = false;
            if (!onTimer)
            {
               timer = GameEndTimer();
               StartCoroutine(timer);
            }
        }
    }

    IEnumerator GameEndTimer()
    {
        onTimer = true;
        Debug.Log("GameEndTimer()");

        // 게임종료 카운트 기다리기
        yield return new WaitForSeconds(gameToEndTime);
        Debug.Log("게임 끝");

        CanShot = false ;
        CameraManager.CM.SetTarget(null);

        // 정산 끝날때까지 대기
        yield return StartCoroutine(AdjustmentScore());

        if (PigCount <= 0)
        {
            if (currentScore >= highScore)
            {
                highScore = currentScore;

                PlayerPrefs.SetInt(stageName + "highScore", highScore);
                UIManager.UI.ResultPanel.SetScoreTxt(0, highScore);
            }
            UIManager.UI.ResultPanel.SetText(1, "Level Cleared!");
            UIManager.UI.ResultPanel.isCleared = true;
        }
        else // 게임 오버
        {
            UIManager.UI.ResultPanel.SetText(1, "Game Over!");
            UIManager.UI.ResultPanel.isCleared = false;
        }

        // 물리작용 없애기 위해 시간정지
        Time.timeScale = 0.0f;

        // 결과와 상관없이 현재점수 표시
        UIManager.UI.ResultPanel.SetScoreTxt(1, currentScore);
        UIManager.UI.ResultPanel.EnablePanel();

        onTimer = false;
    }

    /// <summary>
    /// 점수 정산
    /// </summary>
    IEnumerator AdjustmentScore()
    {
        SoundManager.SM.PlayRandomAudio(adjustAudios);
        Debug.Log("정산 시작");

        foreach (BirdBase bird in SlingShot.SS.birds)
        {
            if (bird.idx <= ignoreIdx)
                continue;

            yield return StartCoroutine(bird.ShowPointTxt());
        }
    }

    /// <summary>
    /// 뷰포인트 순회
    /// </summary>
    /// <returns></returns>
    IEnumerator circuitViewPoint()
    {
        int idx = 0;
        float maxDis = CameraManager.CM.maxDistance;

        while (idx < viewPoint.Count())
        {
            CameraManager.CM.maxDistance = viewLength;
            CameraManager.CM.SetTarget(viewPoint[idx]);

            yield return new WaitForSeconds(viewTimer[idx]);
            idx++;
        }

        CameraManager.CM.SetTarget(null);
        CameraManager.CM.maxDistance = maxDis;

        CanShot = true;
        SlingShot.SS.SetBird();
    }
    #endregion
}