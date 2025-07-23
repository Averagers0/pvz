using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlantData", menuName = "Plants/New Plant")]
public class PlantData : ScriptableObject
{
    public string plantName;
    public int health;
    public Sprite icon;
    public GameObject prefab; // 植物预制体
    public float cdTime;
    public int sunCost;
}
