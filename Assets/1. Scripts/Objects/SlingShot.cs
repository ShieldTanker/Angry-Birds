using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SlingShot : MonoBehaviour
{
    #region 싱글톤
    static SlingShot instance;
    public static SlingShot SS { get { return instance; } set { instance = value; } }
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);
    }
    #endregion


    public float maxLine;
    public float stringSpeed;
    public float linePower;

    #region 고무줄 위치값
    public LineRenderer lineRenderer;
    public Transform lineTarget;

    public Transform startPos;
    public Transform middlePos;
    public Transform endPos;
    #endregion

    public bool isShoted;

    public BirdBase birdTarget;

    BirdBase shottedBird;
    public BirdBase ShottedBird { get { return shottedBird; } set { shottedBird = value; } }

    public List<BirdBase> birds;
    public float settingSpeed;
    int currentIdx;

    public AudioSource audioSource;
    public AudioClip audioClip;
    public AudioClip[] audioClips;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        audioSource = GetComponent<AudioSource>();

        lineRenderer.SetPosition(0, startPos.position);
        lineRenderer.SetPosition(1, middlePos.position);
        lineRenderer.SetPosition(2, endPos.position);

        currentIdx = 0;

        // 스테이지별 새 배치
        for (int i = 0; i < birds.Count; i++)
        {
            int idx = i;
            Vector3 pos = transform.position - new Vector3(i + 1, 0, 0);
            BirdBase go = Instantiate(birds[i], pos, Quaternion.identity);

            go.idx = idx;
            birds[idx] = go;
            birds[idx].name = "Bird" + idx;
            birds[idx].DisAbleVellocity();
        }
    }

    private void Update()
    {
        // 다음 발사체가 될 새가 있을경우 고무줄 중간위치를 다음 발사체에 위치로 할당
        if (birdTarget != null)
            lineRenderer.SetPosition(1, birdTarget.transform.position);
        
        else
        {
            if (lineTarget != null)
            {
                float dis = Vector3.Distance(ShottedBird.transform.position, middlePos.position);

                if ((isShoted && dis >= linePower) || dis >= maxLine + 0.2f)
                    StartCoroutine(ReturnRubberBand());
                else
                    lineRenderer.SetPosition(1, lineTarget.position);
            }
        }
    }

    public void Shot(Vector3 dir, float power)
    {
        // 물리작용 활성
        birdTarget.EnAbleVellocity();
        birdTarget.rb.AddForce(dir * power, ForceMode2D.Impulse);
        linePower = power;

        SoundManager.SM.PlayRandomAudio(audioSource, audioClips);
        SoundManager.SM.PlayAudio(birdTarget.audioSource, birdTarget.flyAudioClip);

        StageManager.SM.BirdCount--;
        StageManager.SM.ignoreIdx++;
        StageManager.SM.CheckCount();
    }

    #region 코루틴
    public void SetBird()
    {
        // 현재 인덱스가 새들의 총량을 넘어설때 리턴
        if (currentIdx >= birds.Count || !StageManager.SM.CanShot)
            return;

        // 카메라의 타겟을 초기화
        CameraManager.CM.SetTarget(null);
        StartCoroutine(SetBirdCoroutine());
    }

    /// <summary>
    /// 다음 발사할 새를 세팅
    /// </summary>
    /// <returns></returns>
    IEnumerator SetBirdCoroutine()
    {
        isShoted = false;

        yield return new WaitForSeconds(1f);

        SoundManager.SM.PlayAudio(birds[currentIdx].audioSource, birds[currentIdx].selectedAudioClip);

        float distance = Vector3.Distance(birds[currentIdx].transform.position, middlePos.position);

        while (distance >= 0.5f)
        {
            Vector3 tmp = Vector3.Lerp(birds[currentIdx].transform.position, middlePos.position, settingSpeed * Time.deltaTime);

            distance = Vector3.Distance(birds[currentIdx].transform.position, middlePos.position);

            birds[currentIdx].transform.position = tmp;

            yield return null;
        }

        birdTarget = birds[currentIdx];

        birdTarget.transform.position = middlePos.position;

        birdTarget.rb.bodyType = RigidbodyType2D.Kinematic;
        birdTarget.rb.velocity = Vector3.zero;

        currentIdx++;
    }

    /// <summary>
    /// 고무줄 위치 초기화
    /// </summary>
    /// <returns></returns>
    IEnumerator ReturnRubberBand()
    {
        lineTarget = null;

        float distance = Vector3.Distance(middlePos.position, lineRenderer.GetPosition(1));
        while (distance >= 0.1f)
        {
            Vector3 tmp = Vector3.Lerp(lineRenderer.GetPosition(1), middlePos.position, stringSpeed * Time.deltaTime);
            distance = Vector3.Distance(middlePos.position, lineRenderer.GetPosition(1));

            lineRenderer.SetPosition(1, tmp);
            yield return null;
        }

        lineRenderer.SetPosition(1, middlePos.position);
    }
    #endregion
}