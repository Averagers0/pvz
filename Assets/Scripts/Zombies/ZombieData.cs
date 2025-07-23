using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ZombieData", menuName = "Zombies/New Zombie")]
public class ZombieData : ScriptableObject
{
    public string Name;
    public int health;
    public Sprite icon;
    public GameObject prefab; // 预制体

    public float originalSpeed = 0.1f;

    public int originalDamage = 10;
    public float originalInterval = 1.0f;
}