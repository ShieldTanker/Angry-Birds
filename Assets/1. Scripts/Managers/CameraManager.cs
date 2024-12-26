using System.Collections;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    #region 싱글톤
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

    private Vector3 originPos;
    public Vector3 offset = Vector3.zero;

    private Transform target;
    public int targetLayer;
    public float maxDistance;
    public float camSpeed;

    private Coroutine cameraCoroutine;

    private void Start()
    {
        targetLayer = LayerMask.NameToLayer("Bird");
        StartFollowing(); // 카메라 코루틴 시작
    }

    /// <summary>
    /// 카메라 타겟을 설정
    /// </summary>
    /// <param name="newTarget"></param>
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;

        // 실행중인 코루틴이 있을경우 정지
        if (cameraCoroutine != null)
            StopCoroutine(cameraCoroutine);
        

        StartFollowing(); // 새 타겟에 대한 추적을 시작
    }

    public void StartFollowing()
    {
        // 이미 실행중인 코루틴이 있을경우 정지
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
                Vector3 toTarget = targetPoWithOffset - transform.position;

                transform.position += toTarget.magnitude > maxDistance 
                    ? toTarget.normalized * (toTarget.magnitude - maxDistance) : toTarget * (camSpeed * Time.deltaTime);
            }
            else
            {
                float distance = Vector3.Distance(transform.position, originPos);

                transform.position = distance > 0.2f 
                    ? Vector3.Lerp(transform.position, originPos, camSpeed * Time.deltaTime) : originPos;
            }

            // 프레임마다 대기
            yield return null;
        }
    }
}
