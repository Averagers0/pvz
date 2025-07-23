using System.Collections;
using UnityEngine;

public class SunFlower : Plant
{
    public override void SpecialAbility()
    {
        Debug.Log("向日葵开始生产阳光");
        // 这里可以写生产阳光逻辑
    }

    [Header("向日葵属性")]
    public GameObject sunPrefab;            // 阳光预制体
    public Vector3 sunSpawnOffset = new Vector3(0, 1.5f, 0); // 阳光生成起点偏移
    public float firstDelayMin = 5f;        // 首次延迟最小值
    public float firstDelayMax = 7f;        // 首次延迟最大值
    public float produceInterval = 24f;     // 生产间隔（原版24秒）

    private Coroutine produceRoutine;

    public override void Start()
    {
        base.Start();
        produceRoutine = StartCoroutine(ProduceSunLoop());
    }

    private IEnumerator ProduceSunLoop()
    {
        // 初次延迟
        float firstDelay = Random.Range(firstDelayMin, firstDelayMax);
        yield return new WaitForSeconds(firstDelay);

        while (true)
        {
            SpawnSun();
            yield return new WaitForSeconds(produceInterval);
        }
    }

    private SpriteRenderer spriteRenderer;
    [SerializeField] private float glowDuration = 9.0f;

    private void SpawnSun()
    {
        // 发光动画
        StartCoroutine(GlowEffect());

        // 阳光生成位置
        Vector3 spawnPos = transform.position + sunSpawnOffset;
        GameObject sun = Instantiate(sunPrefab, spawnPos, Quaternion.identity);

        Sun sunScript = sun.GetComponent<Sun>();
        if (sunScript != null)
        {
            Vector3 targetPos = transform.position + new Vector3(Random.Range(-0.5f, 0.5f), 0f, 0f);
            sunScript.Init(targetPos);
        }
    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private IEnumerator GlowEffect()
    {
        Color originalColor = spriteRenderer.color;
        spriteRenderer.color = Color.yellow;  // 发光颜色
        yield return new WaitForSeconds(glowDuration);
        spriteRenderer.color = originalColor; // 恢复原色
    }

    private void OnDestroy()
    {
        if (produceRoutine != null)
        {
            StopCoroutine(produceRoutine);
        }
    }


}
