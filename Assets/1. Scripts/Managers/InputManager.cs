using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    #region �̱���
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
        // �߻��� ���� null �� �ƴϰ�, hit ���� �ʾ�����, �ɷ� ��� ��������
        if (SlingShot.SS.ShottedBird != null && !SlingShot.SS.ShottedBird.Hit && !SlingShot.SS.ShottedBird.usedAbility)
        {
            SlingShot.SS.ShottedBird.BirldAbility(0f);
        }
        
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0; // ī�޶���� �Ÿ� ����

        dragStart = mousePos;

        Vector3 dir = mousePos - dragStart;
        dragEnd = SlingShot.SS.middlePos.position + dir;

        // �׽�Ʈ�� ������Ʈ ��ġ
        go1.transform.position = dragStart;
        go2.transform.position = dragEnd;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (SlingShot.SS.birdTarget == null || !StageManager.SM.CanShot)
            return;

        // ���콺 ���� ��ġ(����)
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;

        // ���콺 ���������� ���콺 ���� ��ġ�� ���� ���ϱ�
        Vector3 dir = mousePos - dragStart;

        // ���콺 ������ġ�� ������Ʈ ����(������ �˱�����)
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

        // ���� ����
        Vector3 dir = SlingShot.SS.middlePos.position - dragEnd;

        // ������ �� ����
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