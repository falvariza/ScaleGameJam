using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerProgress
{
    public static int CurrentLevel { get; private set; } = 0;

    public static void SetCurrentLevel(int level)
    {
        CurrentLevel = level;
    }

    public static void IncreaseLevel()
    {
        CurrentLevel++;
    }

    public static void ResetLevel()
    {
        CurrentLevel = 0;
    }
}
