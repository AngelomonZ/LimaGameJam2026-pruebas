using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;

    [Header("UI Referencias")]
    [SerializeField] private GameObject loadingScreenCanvas;
    [SerializeField] private Slider progressBar; 
    [SerializeField] private CanvasGroup fadeGroup; 

    [Header("Configuración")]
    [SerializeField] private float fadeDuration = 1f;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        if (loadingScreenCanvas != null) loadingScreenCanvas.SetActive(false);
    }

    // Método público que llamarás desde tus botones o triggers
    public void LoadLevel(string sceneName)
    {
        StartCoroutine(LoadLevelRoutine(sceneName));
    }

    private IEnumerator LoadLevelRoutine(string sceneName)
    {
        loadingScreenCanvas.SetActive(true);

        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.StopMusic();
        }

        progressBar.value = 0;

        yield return StartCoroutine(Fade(1f));

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.25f);

            if (progressBar != null) progressBar.value = progress;

            if (operation.progress >= 0.9f)
            {
                if (progressBar != null) progressBar.value = 1f;
                yield return new WaitForSeconds(0.5f);
                operation.allowSceneActivation = true;
            }

            yield return null;
        }

        yield return StartCoroutine(Fade(0f));

        loadingScreenCanvas.SetActive(false);
    }

    private IEnumerator Fade(float targetAlpha)
    {
        float speed = 1f / fadeDuration;
        float startAlpha = fadeGroup.alpha;
        float percent = 0f;

        while (percent < 1f)
        {
            percent += Time.deltaTime * speed;
            fadeGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, percent);
            yield return null;
        }

        fadeGroup.alpha = targetAlpha;
    }
}