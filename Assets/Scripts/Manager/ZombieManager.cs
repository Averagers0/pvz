using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieManager : MonoBehaviour
{
    public static ZombieManager Instance;  // 单例

    [Header("Zombie Settings")]
    public ZombieData[] zombieTypes;       // 僵尸数据列表
    public Transform[] spawnPoints;        // 僵尸出生点（不同行）

    public float spawnInterval = 30f;       // 僵尸生成间隔

    private List<Zombie> zombies = new List<Zombie>(); // 场景中的僵尸

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        // 自动生成僵尸
        StartCoroutine(SpawnZombies());
    }

    /// <summary>
    /// 自动生成僵尸
    /// </summary>
    private IEnumerator SpawnZombies()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            SpawnRandomZombie();
        }
    }

    /// <summary>
    /// 生成一个随机僵尸
    /// </summary>
    public void SpawnRandomZombie()
    {
        if (zombieTypes.Length == 0 || spawnPoints.Length == 0)
        {
            Debug.LogWarning("ZombieManager: 没有配置 zombieTypes 或 spawnPoints！");
            return;
        }

        // 随机选一个僵尸和出生点
        ZombieData randomZombie = zombieTypes[Random.Range(0, zombieTypes.Length)];
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        GameObject zombieObj = Instantiate(randomZombie.prefab, spawnPoint.position, Quaternion.identity);
        Zombie zombie = zombieObj.GetComponent<Zombie>();
        if (zombie != null)
        {
            zombies.Add(zombie);
        }
        else
        {
            Debug.LogWarning("ZombieManager: 生成的僵尸缺少 Zombie 脚本！");
        }
    }

    /// <summary>
    /// 移除一个僵尸（死亡时调用）
    /// </summary>
    public void RemoveZombie(Zombie zombie)
    {
        if (zombies.Contains(zombie))
        {
            zombies.Remove(zombie);
        }
    }

    /// <summary>
    /// 清空所有僵尸
    /// </summary>
    public void ClearZombies()
    {
        foreach (Zombie z in zombies)
        {
            if (z != null) Destroy(z.gameObject);
        }
        zombies.Clear();
    }

    /// <summary>
    /// 检查某一行是否有僵尸
    /// </summary>
    public bool HasZombieInRow(int rowIndex)
    {
        if (rowIndex < 0 || rowIndex >= spawnPoints.Length)
            return false;

        float rowY = spawnPoints[rowIndex].position.y;

        foreach (Zombie z in zombies)
        {
            if (z != null && Mathf.Abs(z.transform.position.y - rowY) < 0.1f)
            {
                return true;
            }
        }
        return false;
    }
}
