using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeSystem : MonoBehaviour
{
    [SerializeField] private PlayerSize[] playerSizes;
    private int currentSizeIndex = 0;

    public PlayerSize CurrentSize => playerSizes[currentSizeIndex];

    public void IncreaseSize()
    {
        if (IsExploded()) return;

        currentSizeIndex++;
    }

    public void DecreaseSize()
    {
        if (currentSizeIndex > 0)
        {
            currentSizeIndex--;
        }
    }

    public Boolean IsExploded()
    {
        return currentSizeIndex == playerSizes.Length;
    }
}
