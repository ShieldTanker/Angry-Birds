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

    private Vector3 originPos; // 원래 위치를 저장할 변수
    private Transform target; // 추적할 타겟
    public Vector3 offset = Vector3.zero; // 타겟과의 오프셋
    public float maxDistance; // 최대 거리
    public float camSpeed; // 카메라 이동 속도

    private Coroutine cameraCoroutine;

    private void Start()
    {
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

                float distance = Vector3.Distance(transform.position, targetPoWithOffset);
                
                // 최대 거리 초과 시, 타겟을 maxDistance만큼 따라가도록 조정
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
                // 타겟이 없을 경우 원래 위치로 복귀
                float distance = Vector3.Distance(transform.position, originPos);

                if (distance > 0.2f)
                    transform.position = Vector3.Lerp(transform.position, originPos, camSpeed * Time.deltaTime);
                else
                    transform.position = originPos;
            }

            // 프레임마다 대기
            yield return null;
        }
    }
}
