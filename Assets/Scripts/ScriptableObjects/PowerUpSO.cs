using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PowerUpSO", menuName = "ScriptableObjects/PowerUpSO")]
public class PowerUpSO : ScriptableObject
{
    public Transform powerUpPrefab;
    public string powerUpName;
    public Sprite powerUpIcon;
}
