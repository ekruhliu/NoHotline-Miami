using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoading : MonoBehaviour
{
    public string SceneName;
    public bool Loading;
    public bool zamytit = true;

    void Update()
    {
        if (zamytit)
        {
            Invoke("trueLoad", 5);
            zamytit = false;
        }
        if(Loading)
            StartCoroutine(AsyncLoad());
    }

    IEnumerator AsyncLoad()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneName);
        while (!operation.isDone)
        {
            float progress = operation.progress / 0.9f;
            yield return null;
        }
    }
    void trueLoad()
    {
        Loading = true;
    }
}
