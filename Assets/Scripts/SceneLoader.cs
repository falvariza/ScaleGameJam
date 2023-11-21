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
        Level1Scene,
        Level2Scene,
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
