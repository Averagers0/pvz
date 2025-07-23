using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlantManager : MonoBehaviour
{
    public static PlantManager Instance;

    [Header("Shovel UI")]
    public GameObject shovelCursor; // 铲子图片 UI
    private bool isRemoving = false;

    public Texture2D shovelTexture;

    private void Awake()
    {
        Instance = this;
    }


    private void Update()
    {
        if (isRemoving)
        {
            // 让铲子 UI 跟随鼠标（如有UI跟随逻辑可写在这里）
            Vector3 mousePos = Input.mousePosition;

            // 点击左键尝试铲除植物
            if (Input.GetMouseButtonDown(0))
            {
                if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) return;

                // 将鼠标位置从屏幕坐标转为世界坐标
                Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
                Vector2 worldPos2D = new Vector2(worldPos.x, worldPos.y);

                // 2D 射线检测（点射线）
                RaycastHit2D hit = Physics2D.Raycast(worldPos2D, Vector2.zero);

                if (hit.collider != null)
                {
                    Debug.Log($"射线击中物体：{hit.collider.name}");
                    Plant plant = hit.collider.GetComponent<Plant>();
                    Debug.Log("GetComponent Plant: " + plant);

                    if (plant != null)
                    {
                        Debug.Log($"铲除植物：{plant.name}");
                        Destroy(plant.gameObject);
                        DeactivateShovelMode();
                    }
                    else
                    {
                        Debug.Log("未找到Plant组件");
                    }
                }
            }

            // 右键退出模式
            if (Input.GetMouseButtonDown(1))
            {
                DeactivateShovelMode();
            }
        }
    }


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
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    Vector3 position = hit.collider.bounds.center;
                    Debug.Log($"格子中心位置：{position}");
                    PlacePlant(plantData, position);
                    placed = true;
                }
            }
            yield return null;
        }
    }

    private void PlacePlant(PlantData plantData, Vector3 position)
    {
        GameObject newPlant = Instantiate(plantData.prefab, position, Quaternion.identity);
        Plant plantComponent = newPlant.GetComponent<Plant>();
        plantComponent.plantData = plantData;
        plantComponent.OnPlant();
        plantComponent.SpecialAbility();
    }

    public void ActivateShovelMode()
    {
        isRemoving = true;
        Cursor.SetCursor(shovelTexture, Vector2.zero, CursorMode.Auto);
        Debug.Log("进入铲除模式");
    }

    public void DeactivateShovelMode()
    {
        isRemoving = false;
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        Debug.Log("退出铲除模式");
    }
}