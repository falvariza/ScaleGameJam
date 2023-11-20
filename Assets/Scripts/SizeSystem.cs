using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeSystem : MonoBehaviour
{
    public EventHandler<OnSizeIncreasedArgs> OnSizeIncreased;
    public EventHandler OnSizeDecreased;

    public class OnSizeIncreasedArgs : EventArgs
    {
        public PlayerSize playerSize;
    }

    [SerializeField] private PlayerSize[] playerSizes;
    private int currentSizeIndex = 0;

    public PlayerSize CurrentSize => playerSizes[currentSizeIndex];

    public void IncreaseSize()
    {
        if (IsExploded()) return;
        currentSizeIndex++;

        if (IsExploded()) return;

        OnSizeIncreased?.Invoke(this, new OnSizeIncreasedArgs {
            playerSize = playerSizes[currentSizeIndex]
        });
    }

    public void DecreaseSize()
    {
        if (currentSizeIndex > 0)
        {
            currentSizeIndex--;
            OnSizeDecreased?.Invoke(this, EventArgs.Empty);
        }
    }

    public Boolean IsExploded()
    {
        return currentSizeIndex >= playerSizes.Length;
    }

    public void ResetSize()
    {
        currentSizeIndex = 0;
    }
}
