using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;

    [SerializeField] private float fadeDuration = 1.0f;

    [SerializeField] private bool fadeIn = false;

    private void Start()
    {
        if (fadeIn)
            FadeIn();
        else
            FadeOut();
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float start, float end, float durattion)
    {
        float elapsedTime = 0.0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(start, end, elapsedTime / durattion);
            yield return null;
        }
        canvasGroup.alpha = end;
    }

    public void FadeIn()
    {
        StartCoroutine(FadeCanvasGroup(canvasGroup, canvasGroup.alpha, 0, fadeDuration));
    }

    public void FadeOut()
    {
        StartCoroutine(FadeCanvasGroup(canvasGroup, canvasGroup.alpha, 1, fadeDuration));
    }

    public void LoadMainMenuScene()
    {
        FadeOut();
        SceneManager.LoadScene(0);
    }

    public void LoadGameScene()
    {
        FadeOut();
        SceneManager.LoadScene(1);
    }

    public void ExitApplication()
    {
        FadeOut();
        Application.Quit();
    }
}
