using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SunManager : MonoBehaviour
{
    //单例模式
    public static SunManager Instance
    {
        get;
        private set;
    }

    //单例构造
    private void Awake()
    {
        Instance = this;
    }


    void Start()
    {
        updateSunPointText();
    }

    public void Update()
    {
        HandleSunClick();
    }

    //sun的get和set
    [SerializeField]
    private int sunPoint = 100;
    public int getSunPoint()
    {
        return sunPoint;
    }
    public void setSunPoint(int sun)
    {
        this.sunPoint = sun;
        updateSunPointText();
    }

    //绑定gui
    public TextMeshProUGUI sunPointGui;
    public void updateSunPointText()
    {
        sunPointGui.text = sunPoint.ToString();
    }

    public Vector3 GetSunUIPosition()
    {
        // 把UI的屏幕坐标转成世界坐标
        return Camera.main.ScreenToWorldPoint(sunPointGui.transform.position);
    }


    /// <summary>
    /// 检测鼠标点击是否点到阳光，触发阳光收集
    /// </summary>
    private void HandleSunClick()
    {
        if (!Input.GetMouseButtonDown(0)) return;

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos, Vector2.zero);

        if (hits.Length == 0) return;

        Sun topSun = GetTopSortingOrderSun(hits);
        if (topSun != null)
        {
            topSun.Collect(); // 触发飞向 UI 动画
        }
    }

    /// <summary>
    /// 获取点击位置中 SortingOrder 最高的阳光
    /// </summary>
    private Sun GetTopSortingOrderSun(RaycastHit2D[] hits)
    {
        Sun topSun = null;
        int topOrder = int.MinValue;

        foreach (var hit in hits)
        {
            Sun sun = hit.collider.GetComponent<Sun>();
            if (sun != null)
            {
                int order = sun.GetComponent<SpriteRenderer>().sortingOrder;
                if (order > topOrder)
                {
                    topOrder = order;
                    topSun = sun;
                }
            }
        }
        return topSun;
    }
}
