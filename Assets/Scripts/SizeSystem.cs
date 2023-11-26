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
    public PlayerSize CurrentSize => playerSizes[currentSizeIndex];

    [SerializeField] private PlayerSize[] playerSizes;
    private int currentSizeIndex = 0;


    private List<float> speedBoosts = new List<float>();

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
        speedBoosts.Clear();
    }

    public void AddSpeedBoost(float boost)
    {
        speedBoosts.Add(boost);
    }

    public void RemoveSpeedBoost()
    {
        if (speedBoosts.Count > 0)
        {
            speedBoosts.RemoveAt(speedBoosts.Count - 1);
        }
    }

    public float GetSpeed()
    {
        float boost = 1;

        foreach (float speedBoost in speedBoosts)
        {
            boost += speedBoost;
        }

        return CurrentSize.speed * boost;
    }
}
