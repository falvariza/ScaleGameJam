using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public enum Scene
    {
        MainMenuScene,
        LoadingScene,
        SceneLevel1,
        SceneLevel2,
    }

    private static Scene targetScene;

    public static void Load(Scene targetScene)
    {
        // SceneLoader.targetScene = targetScene;

        SceneManager.LoadScene(targetScene.ToString());
    }

    public static void LoaderCallback()
    {
        SceneManager.LoadScene(targetScene.ToString());
    }
}
