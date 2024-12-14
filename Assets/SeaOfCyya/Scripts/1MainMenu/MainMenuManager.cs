using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    private void Start()
    {
#if UNITY_STANDALONE_LINUX || PLATFORM_STANDALONE_LINUX || UNITY_STANDALONE_LINUX_API
        SetChangeScene(1);

        return;
#endif

    }
    public void SetChangeScene(int sceneId)
    {
        SceneManager.LoadScene(sceneId);
    }
}
