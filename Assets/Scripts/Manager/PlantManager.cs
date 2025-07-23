using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlantManager : MonoBehaviour
{
    public static PlantManager Instance;

    [Header("Grid Settings")]
    public int rows = 5;
    public int cols = 9;

    // 每个格子的植物对象引用
    private Plant[,] plantsGrid;

    // 每个格子的中心世界坐标
    private Vector3[,] gridPositions;

    // 行父物体（行下有9个格子）
    public GameObject[] rowObjects; // Inspector中赋值，长度为5

    [Header("Shovel UI")]
    public GameObject shovelCursor; // 铲子图片UI
    public Texture2D shovelTexture;

    private bool isRemoving = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        plantsGrid = new Plant[rows, cols];
        gridPositions = new Vector3[rows, cols];

        InitGridPositions();
    }

    // 初始化格子中心点坐标
    private void InitGridPositions()
    {
        for (int r = 0; r < rows; r++)
        {
            Transform rowTransform = rowObjects[r].transform;
            for (int c = 0; c < cols; c++)
            {
                Transform cell = rowTransform.GetChild(c);
                gridPositions[r, c] = cell.position;
            }
        }
    }

    private void Update()
    {
        if (isRemoving)
        {

            // 左键点击铲除植物
            if (Input.GetMouseButtonDown(0))
            {
                if (EventSystem.current.IsPointerOverGameObject()) return;

                Plant plant = GetPlantUnderMouse();
                if (plant != null)
                {
                    RemovePlant(plant);
                    DeactivateShovelMode();
                }
            }

            // 右键退出铲除模式
            if (Input.GetMouseButtonDown(1))
            {
                DeactivateShovelMode();
            }
        }
    }

    // 检测鼠标下的植物
    private Plant GetPlantUnderMouse()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 worldPos2D = new Vector2(mouseWorldPos.x, mouseWorldPos.y);

        RaycastHit2D hit = Physics2D.Raycast(worldPos2D, Vector2.zero);
        if (hit.collider != null)
        {
            Plant plant = hit.collider.GetComponent<Plant>();
            return plant;
        }
        return null;
    }

    // 选择植物后调用，等待玩家点击格子种植
    public void SelectPlant(PlantData plantData)
    {
        StartCoroutine(WaitForPlacement(plantData));
    }

    private IEnumerator WaitForPlacement(PlantData plantData)
    {
        Debug.Log($"选择植物：{plantData.plantName}，请点击种植位置...");

        bool placed = false;
        while (!placed)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (EventSystem.current.IsPointerOverGameObject()) yield return null;

                if (GetGridIndexFromMouse(out int row, out int col))
                {
                    bool success = PlantAt(plantData, row, col);
                    if (success)
                    {
                        Debug.Log($"成功种植植物在位置：{row},{col}");
                        placed = true;
                    }
                    else
                    {
                        Debug.Log("该格子已被占用，无法种植");
                    }
                }
            }
            yield return null;
        }
    }

    // 在指定格子种植植物
    public bool PlantAt(PlantData plantData, int row, int col)
    {
        if (!IsValidGrid(row, col)) return false;
        if (plantsGrid[row, col] != null) return false;

        Vector3 pos = gridPositions[row, col];
        GameObject newPlantObj = Instantiate(plantData.prefab, pos, Quaternion.identity);
        Plant newPlant = newPlantObj.GetComponent<Plant>();

        if (newPlant == null)
        {
            Debug.LogError("植物预制体没有 Plant 脚本");
            Destroy(newPlantObj);
            return false;
        }

        SetPlantSortingOrder(newPlant, row);
        newPlant.plantData = plantData;
        newPlant.OnPlant();
        newPlant.SpecialAbility();

        plantsGrid[row, col] = newPlant;
        return true;
    }

    private void SetPlantSortingOrder(Plant plant, int row)
    {
        SpriteRenderer sr = plant.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            sr.sortingOrder = row;
        }
    }


    // 铲除指定植物
    private void RemovePlant(Plant plant)
    {
        for (int r = 0; r < rows; r++)
        {
            for (int c = 0; c < cols; c++)
            {
                if (plantsGrid[r, c] == plant)
                {
                    Destroy(plantsGrid[r, c].gameObject);
                    plantsGrid[r, c] = null;
                    Debug.Log($"植物已从 {r},{c} 铲除");
                    return;
                }
            }
        }
        Debug.LogWarning("未在网格中找到植物对象");
    }

    // 判断行列是否有效
    private bool IsValidGrid(int row, int col)
    {
        return row >= 0 && row < rows && col >= 0 && col < cols;
    }

    // 通过鼠标坐标射线检测获得格子索引
    public bool GetGridIndexFromMouse(out int row, out int col)
    {
        row = -1;
        col = -1;

        Vector3 mousePos = Input.mousePosition;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);

        RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);

        if (hit.collider != null)
        {
            GameObject hitObj = hit.collider.gameObject;

            // 遍历行列，找到匹配的格子物体
            for (int r = 0; r < rows; r++)
            {
                Transform rowTransform = rowObjects[r].transform;
                for (int c = 0; c < cols; c++)
                {
                    if (rowTransform.GetChild(c).gameObject == hitObj)
                    {
                        row = r;
                        col = c;
                        return true;
                    }
                }
            }
        }

        return false;
    }

    public void ActivateShovelMode()
    {
        isRemoving = true;
        Cursor.SetCursor(shovelTexture, Vector2.zero, CursorMode.Auto);
        shovelCursor.SetActive(false);
        Debug.Log("进入铲除模式");
    }

    public void DeactivateShovelMode()
    {
        isRemoving = false;
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        shovelCursor.SetActive(true);
        Debug.Log("退出铲除模式");
    }
}



