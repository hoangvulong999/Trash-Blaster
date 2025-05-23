using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    [SerializeField] private Slider loadingSlider;
    [SerializeField] private float loadingTime = 2f;
    // private bool isLoadingComplete = false;

    void Start()
    {
        if (loadingSlider != null)
        {
            loadingSlider.value = 0f;
            StartCoroutine(FillLoadingBar());
        }
    }

    private IEnumerator FillLoadingBar()
    {
        float elapsedTime = 0f;
        
        while (elapsedTime < loadingTime)
        {
            elapsedTime += Time.deltaTime;
            if (loadingSlider != null)
            {
                loadingSlider.value = elapsedTime / loadingTime;
            }
            yield return null;
        }

        // isLoadingComplete = true;
        StartCoroutine(WaitAfterLoading());
    }

    private IEnumerator WaitAfterLoading()
    {
        yield return new WaitForSeconds(1f); // Wait for 2 seconds after loading is complete
        LoadNextScene();
    }

    private void LoadNextScene()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
    }

    void Update()
    {
        // You can add additional loading logic here if needed
    }
}
