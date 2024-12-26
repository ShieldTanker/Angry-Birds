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
    public int PigCount { get; set; }
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
    AudioSource audioSource;
    public AudioClip[] audioClips;


    #region 기본 함수
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        // 뷰 포인트 순회 하기
        StartCoroutine(circuitViewPoint());
    }
    #endregion


    #region 만든 함수

    public void PlayRandomAudio(int minIdx, int maxIdx)
    {
        int idx = UnityEngine.Random.Range(minIdx, maxIdx);
        audioSource.PlayOneShot(audioClips[idx]);
    }

    public void PlayAudio(int idx)
    {
        audioSource.PlayOneShot(audioClips[idx]);
    }
    #endregion


    #region 코루틴
    public void CheckCount()
    {
        // 타이머가 돌고있으면 리셋하고 다시실행
        if (onTimer)
        {
            // Debug.Log("StopCoroutine(timer)");
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
        }
        else // 게임 오버
        {
            UIManager.UI.ResultPanel.SetText(1, "Game Over!");
        }

        // 물리작용 없애기 위해 시간정지
        Time.timeScale = 0.0f;

        // 결과와 상관없이 현재점수 표시
        UIManager.UI.ResultPanel.SetScoreTxt(1, currentScore);
        UIManager.UI.ResultPanel.gameObject.SetActive(true);

        onTimer = false;
    }

    /// <summary>
    /// 점수 정산
    /// </summary>
    IEnumerator AdjustmentScore()
    {
        Debug.Log("정산 시작");

        for (int i = 0; i < birdIdxLength; i++)
        {
            // Debug.Log($"{i}번째 시작, 배열 크기 : {BirdCount}, {ignoreIdx}번째 까지 무시");
            if (i <= ignoreIdx)
            {
                // Debug.Log($"{i}번째 는 무시");
                continue;
            }

            yield return StartCoroutine(SlingShot.SS.birds[i].ShowPointTxt());
        }

        // Debug.Log("정산끝");
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
            Debug.Log("뷰포인트 세팅");

            CameraManager.CM.maxDistance = viewLength;
            CameraManager.CM.SetTarget(viewPoint[idx]);

            yield return new WaitForSeconds(viewTimer[idx]);
            idx++;
        }

        CameraManager.CM.SetTarget(null);
        CameraManager.CM.maxDistance = maxDis;

        SlingShot.SS.SetBird();
        CanShot = true;
    }
    #endregion
}