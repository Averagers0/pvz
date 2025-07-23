using System.Collections;
using UnityEngine;

public class Sun : MonoBehaviour
{
    public int sunValue = 25;          // 阳光数值
    public float dropDuration = 2f;    // 掉落动画时长
    public float lifeTime = 7f;        // 存活时间
    public float flyDuration = 0.4f;   // 飞向UI的动画时长（更快）

    private bool isCollected = false;
    private Vector3 uiTargetPos;       // UI位置
    private Coroutine lifeCoroutine;
    private Coroutine dropCoroutine;   // 掉落动画协程

    private void Awake()
    {
        // 自动增加点击区域
        var collider = GetComponent<CircleCollider2D>();
        if (collider != null)
        {
            collider.radius *= 1.8f; // 增加80% 点击范围
        }
    }

    public void Init(Vector3 targetPos)
    {
        dropCoroutine = StartCoroutine(DropAnimation(targetPos));
        lifeCoroutine = StartCoroutine(LifeTimer());
    }

    private IEnumerator DropAnimation(Vector3 targetPos)
    {
        Vector3 startPos = transform.position;
        float elapsed = 0f;

        while (elapsed < dropDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / dropDuration;
            transform.position = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }

        transform.position = targetPos;
    }

    private IEnumerator LifeTimer()
    {
        yield return new WaitForSeconds(lifeTime);
        if (!isCollected)
        {
            Destroy(gameObject);
        }
    }

    public void Collect()
    {
        if (isCollected) return;
        isCollected = true;

        if (dropCoroutine != null) StopCoroutine(dropCoroutine);
        if (lifeCoroutine != null) StopCoroutine(lifeCoroutine);

        uiTargetPos = SunManager.Instance.GetSunUIPosition();
        StartCoroutine(FlyToUI());
    }


    private IEnumerator FlyToUI()
    {
        Vector3 startPos = transform.position;
        float elapsed = 0f;

        while (elapsed < flyDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / flyDuration;
            t = t * t; // 缓动加速 (EaseIn)

            transform.position = Vector3.Lerp(startPos, uiTargetPos, t);
            yield return null;
        }

        transform.position = uiTargetPos;

        SunManager.Instance.setSunPoint(SunManager.Instance.getSunPoint() + sunValue);
        Destroy(gameObject);
    }
}
