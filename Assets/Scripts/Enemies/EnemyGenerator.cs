using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class EnemyGenerator
{
    private static readonly System.Random random = new System.Random();
    private static int typesOfEnemiesCount = 2;

    public static Enemy GenerateRandomEnemyAt(Vector3 position)
    {
        int enemyType = random.Next(1, typesOfEnemiesCount + 1);

        switch (enemyType)
        {
            case 1:
                return EnemyBasic.Create(position);
            case 2:
                return EnemyFollower.Create(position);
            default:
                throw new InvalidOperationException("Invalid enemy type");
        }
    }
}
