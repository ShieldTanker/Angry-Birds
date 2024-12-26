using System.Collections;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    #region �̱���
    static CameraManager cm;
    public static CameraManager CM { get { return cm; } set { cm = value; } }
    private void Awake()
    {
        if (cm == null)
        { 
            cm = this;
            originPos = transform.position;
        }
        else
            Destroy(gameObject);
    }
    #endregion

    private Vector3 originPos; // ���� ��ġ�� ������ ����
    private Transform target; // ������ Ÿ��
    public Vector3 offset = Vector3.zero; // Ÿ�ٰ��� ������
    public float maxDistance; // �ִ� �Ÿ�
    public float camSpeed; // ī�޶� �̵� �ӵ�

    private Coroutine cameraCoroutine;

    private void Start()
    {
        StartFollowing(); // ī�޶� �ڷ�ƾ ����
    }

    /// <summary>
    /// ī�޶� Ÿ���� ����
    /// </summary>
    /// <param name="newTarget"></param>
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;

        // �������� �ڷ�ƾ�� ������� ����
        if (cameraCoroutine != null)
            StopCoroutine(cameraCoroutine);
        

        StartFollowing(); // �� Ÿ�ٿ� ���� ������ ����
    }

    public void StartFollowing()
    {
        // �̹� �������� �ڷ�ƾ�� ������� ����
        if (cameraCoroutine != null)
            StopCoroutine(cameraCoroutine);

        cameraCoroutine = StartCoroutine(CameraFollowRoutine());
    }

    private IEnumerator CameraFollowRoutine()
    {
        while (true)
        {
            if (target != null)
            {
                Vector3 targetPoWithOffset = target.position + offset;

                float distance = Vector3.Distance(transform.position, targetPoWithOffset);
                
                // �ִ� �Ÿ� �ʰ� ��, Ÿ���� maxDistance��ŭ ���󰡵��� ����
                if (distance > maxDistance)
                {
                    Vector3 dir = (targetPoWithOffset - transform.position).normalized;
                    transform.position = transform.position + dir * (distance - maxDistance);
                }
                else
                    transform.position = Vector3.Lerp(transform.position, targetPoWithOffset, camSpeed * Time.deltaTime);

            }
            else
            {
                // Ÿ���� ���� ��� ���� ��ġ�� ����
                float distance = Vector3.Distance(transform.position, originPos);

                if (distance > 0.2f)
                    transform.position = Vector3.Lerp(transform.position, originPos, camSpeed * Time.deltaTime);
                else
                    transform.position = originPos;
            }

            // �����Ӹ��� ���
            yield return null;
        }
    }
}
