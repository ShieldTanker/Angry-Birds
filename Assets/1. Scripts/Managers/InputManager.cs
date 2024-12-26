using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    #region 싱글톤
    static InputManager instance;
    public static InputManager IM { get { return instance; } set { instance = value; } }
    private void Awake()
    {
        if (InputManager.instance == null)
        {
            instance = this;
        }
        else 
        {
            Destroy(this.gameObject);
        }
    }
    #endregion

    [SerializeField] Vector3 dragStart;
    [SerializeField] Vector3 dragEnd;

    public GameObject testOBJ1;
    public GameObject testOBJ2;
    GameObject go1;
    GameObject go2;

    private void Start()
    {
        go1 = Instantiate(testOBJ1);
        go2 = Instantiate(testOBJ2);
    }
   
    public void OnPointerDown(PointerEventData eventData)
    {
        // 발사한 새가 null 이 아니고, hit 되지 않았으며, 능력 사용 안했을때
        if (SlingShot.SS.ShottedBird != null && !SlingShot.SS.ShottedBird.Hit && !SlingShot.SS.ShottedBird.usedAbility)
        {
            SlingShot.SS.ShottedBird.BirldAbility(0f);
        }

        if (!SlingShot.SS.isShoted)
        {
            SoundManager.SM.PlayAudio(SlingShot.SS.audioSource, SlingShot.SS.audioClip);
        }

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0; // 카메라와의 거리 설정

        dragStart = mousePos;

        Vector3 dir = mousePos - dragStart;
        dragEnd = SlingShot.SS.middlePos.position + dir;

        // 테스트용 오브젝트 배치
        go1.transform.position = dragStart;
        go2.transform.position = dragEnd;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (SlingShot.SS.birdTarget == null || !StageManager.SM.CanShot)
            return;

        // 마우스 찍은 위치(끝점)
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        // 마우스 시작점에서 마우스 찍은 위치로 방향 구하기
        Vector3 dir = mousePos - dragStart;

        // 마우스 시작위치에 오브젝트 생성(시작점 알기위함)
        go2.transform.position = mousePos;

        float dist = Mathf.Clamp(dir.magnitude, -SlingShot.SS.maxLine, SlingShot.SS.maxLine);
        dragEnd = SlingShot.SS.middlePos.position + dir.normalized * dist;

        SlingShot.SS.lineRenderer.SetPosition(1, dragEnd);
        SlingShot.SS.birdTarget.transform.position = dragEnd;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (SlingShot.SS.birdTarget == null || SlingShot.SS.isShoted || !StageManager.SM.CanShot)
            return;

        // 방향 설정
        Vector3 dir = SlingShot.SS.middlePos.position - dragEnd;

        // 날리는 힘 설정
        float power = dir.magnitude * SlingShot.SS.birdTarget.power * 2;

        SlingShot.SS.birdTarget.Hit = false;

        SlingShot.SS.Shot(dir, power);

        SettingBird();
    }

    void SettingBird()
    {
        CameraManager.CM.SetTarget(SlingShot.SS.birdTarget.transform);
        SlingShot.SS.isShoted = true;

        SlingShot.SS.ShottedBird = SlingShot.SS.birdTarget;
        SlingShot.SS.lineTarget = SlingShot.SS.birdTarget.transform;

        SlingShot.SS.birdTarget = null;
    }
}