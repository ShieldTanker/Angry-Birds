using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    #region �̱���
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

    #region ���� ����
    public int highScore;
    public int currentScore;

    [Tooltip("���� ����")]
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

    #region ī�޶� ������Ʈ
    [Space]
    public Transform[] viewPoint;
    public float[] viewTimer;
    public float viewLength;
    #endregion

    [Space]
    AudioSource audioSource;
    public AudioClip[] audioClips;


    #region �⺻ �Լ�
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        // �� ����Ʈ ��ȸ �ϱ�
        StartCoroutine(circuitViewPoint());
    }
    #endregion


    #region ���� �Լ�

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


    #region �ڷ�ƾ
    public void CheckCount()
    {
        // Ÿ�̸Ӱ� ���������� �����ϰ� �ٽý���
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

        // �������� ī��Ʈ ��ٸ���
        yield return new WaitForSeconds(gameToEndTime);
        Debug.Log("���� ��");


        // ���� ���������� ���
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
        else // ���� ����
        {
            UIManager.UI.ResultPanel.SetText(1, "Game Over!");
        }

        // �����ۿ� ���ֱ� ���� �ð�����
        Time.timeScale = 0.0f;

        // ����� ������� �������� ǥ��
        UIManager.UI.ResultPanel.SetScoreTxt(1, currentScore);
        UIManager.UI.ResultPanel.gameObject.SetActive(true);

        onTimer = false;
    }

    /// <summary>
    /// ���� ����
    /// </summary>
    IEnumerator AdjustmentScore()
    {
        Debug.Log("���� ����");

        for (int i = 0; i < birdIdxLength; i++)
        {
            // Debug.Log($"{i}��° ����, �迭 ũ�� : {BirdCount}, {ignoreIdx}��° ���� ����");
            if (i <= ignoreIdx)
            {
                // Debug.Log($"{i}��° �� ����");
                continue;
            }

            yield return StartCoroutine(SlingShot.SS.birds[i].ShowPointTxt());
        }

        // Debug.Log("���곡");
    }

    /// <summary>
    /// ������Ʈ ��ȸ
    /// </summary>
    /// <returns></returns>
    IEnumerator circuitViewPoint()
    {
        int idx = 0;
        float maxDis = CameraManager.CM.maxDistance;

        while (idx < viewPoint.Count())
        {
            Debug.Log("������Ʈ ����");

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