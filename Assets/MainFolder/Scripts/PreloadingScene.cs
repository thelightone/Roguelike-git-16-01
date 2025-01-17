using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PreloadingScene : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(LoadScene());
    }

    private IEnumerator LoadScene()
    {
        var op = SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);

        while (!op.isDone)
        {
            yield return null;
        }
    }
}
